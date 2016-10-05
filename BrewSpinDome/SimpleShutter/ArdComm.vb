Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Windows.Forms
Imports System.Threading

' This class contains the code necessary to communicate through TCP socket to the Arduino computer
' using the cc3000 WiFi shield
Public Class ArdComm
    Private gConnectPort As Integer = 1552
    Private gHostName As String = "192.168.1.30"

    Private myMessage As String = ""
    Private gClient As TcpClient
    'Private serverEndPoint As New IPEndPoint(IPAddress.Parse("192.168.1.30"), 1552)
    Private gEncoder As New ASCIIEncoding()

    Private gRestartConnect As Boolean = False
    Private gConnectTimeoutObject As New ManualResetEvent(False)
    Private gIsConnectionSuccessful As Boolean = False
    Private txtLog As TextBox
    Private LogStatus As Boolean = True

    Public CommStatus As Integer
    Public Const COMM_NONE As Integer = 0
    Public Const COMM_CONNECTED As Integer = 1
    Public Const COMM_BUSY As Integer = 2
    Public Const COMM_FAILED As Integer = 3
    Public Const COMM_CONNECTING As Integer = 4

    Public Const ACTION_GET_SHUTTER_STATUS As String = "S"
    Public Const ACTION_OPEN_UPPER_SHUTTER As String = "U"
    Public Const ACTION_CLOSE_UPPER_SHUTTER As String = "u"
    Public Const ACTION_VENT_UPPER_SHUTTER As String = "V"
    Public Const ACTION_OPEN_LOWER_SHUTTER As String = "L"
    Public Const ACTION_CLOSE_LOWER_SHUTTER As String = "l"
    Public Const ACTION_OPEN_BOTH_SHUTTER As String = "B"
    Public Const ACTION_CLOSE_BOTH_SHUTTER As String = "b"
    Public Const ACTION_IS_MOTOR_MOVING As String = "o"
    Public Const ACTION_ABORT_MOTORS As String = "A"
    Public Const ACTION_RECONFIG_PARAM As String = "C"

    Public Sub New(mytxtLog As TextBox, ipAddress As String, port As Integer)
        txtLog = mytxtLog
        CommStatus = COMM_NONE
        gConnectPort = gConnectPort
        gHostName = IPAddress
    End Sub

    Public Sub ConnectArduino()
        CommStatus = COMM_CONNECTING
        gClient = New TcpClient()
        'DescribeConnection("Before Connection")
        Try
            txtLog.AppendText(Environment.NewLine & "Begin Connecting")
            gConnectTimeoutObject.Reset()
            gClient.BeginConnect(gHostName, gConnectPort, New AsyncCallback(AddressOf onConnectCompleted), gClient)
        Catch ex As Exception
            txtLog.AppendText(Environment.NewLine & "   BeginConnect Failed. " & ex.Message)
        End Try

        If (gConnectTimeoutObject.WaitOne(4000, False)) Then
            ' Signal received
            If (gIsConnectionSuccessful) Then
                txtLog.AppendText(Environment.NewLine & "Connected! ")
                CommStatus = COMM_CONNECTED
                'DescribeConnection("After Connect")
            Else
                '   didn't work    
                txtLog.AppendText(Environment.NewLine & "Connection failed")
                CommStatus = COMM_FAILED
            End If
        Else
            ' Timed out
            gClient.Close()
            txtLog.AppendText(Environment.NewLine & "Connection timed out (4 sec)")
            CommStatus = COMM_FAILED
        End If
    End Sub

    ' This gets called when the connection is completed
    ' ar is the status of the async result
    Private Sub onConnectCompleted(ByVal ar As IAsyncResult)
        'Dim client As System.Net.Sockets.TcpClient = CType(ar.AsyncState, System.Net.Sockets.TcpClient)
        ' apparently cannot use txtLog since this is in a different thread?
        gIsConnectionSuccessful = False
        Try
            If (Not IsNothing(gClient.Client)) Then
                gClient.EndConnect(ar)
                gIsConnectionSuccessful = True
            End If
        Catch ex As Exception
            gIsConnectionSuccessful = False
        Finally
            gConnectTimeoutObject.Set()
        End Try

    End Sub

    Public Sub DisconnectClient()
        ' routine to disconnect socket connection from Server (Arduino)
        gClient.Close()
        txtLog.AppendText(Environment.NewLine & "DisconnectClient: Client disconnected")
        CommStatus = COMM_NONE

    End Sub
    ' RequestAction
    '   Sends the command code to the Arduino, retrieves the response string
    '   commands are like ACTION_xxx defined above
    '   Note that the return string may contain an error message; calling routines should check response
    '   for desired return information
    '   Note that some actions return a success code, but the action is not complete.
    '   The status string will show that the operation is still going on.
    Public Function RequestAction(cmd As String) As String
        Dim ret As String = ""
        If (cmd <> "") Then
            If (Not IsNothing(gClient.Client)) Then
                If (gClient.Connected) Then
                    ' Send the message
                    Dim clientStream As NetworkStream = gClient.GetStream()
                    If (SendMessage(clientStream, cmd)) Then
                        If (ReadReturn(clientStream, ret, cmd)) Then
                            ' Everything looks good
                            CommStatus = COMM_CONNECTED
                        Else
                            CommStatus = COMM_FAILED        ' the read failed
                            txtLog.AppendText(Environment.NewLine & "  Read failed. {" & ret & "}")
                        End If
                    Else
                            ' The Send failed
                            CommStatus = COMM_FAILED
                            ret = "Send Failed"
                    End If
                Else
                    txtLog.AppendText(Environment.NewLine & "Client is not connected")
                    CommStatus = COMM_FAILED
                    ret = "Client is not connected"
                End If
            Else
                txtLog.AppendText(Environment.NewLine & "Client is not connected")
                CommStatus = COMM_FAILED
                ret = "Client is not connected"
            End If
        End If

        RequestAction = ret
    End Function

    ' SendMessage
    ' sends the msg to the Arduino
    ' returns True if OK, False if it failed
    Private Function SendMessage(clientStream As NetworkStream, msg As String) As Boolean
        Dim buffer() As Byte = gEncoder.GetBytes(msg)
        Dim ret As Boolean = True
        If (msg <> ACTION_GET_SHUTTER_STATUS) Or (LogStatus) Then
            txtLog.AppendText(Environment.NewLine & "Sending msg " & msg)
        End If
        Try
            clientStream.Write(buffer, 0, buffer.Length)
            clientStream.Flush()
        Catch ex As System.IO.IOException
            txtLog.AppendText(Environment.NewLine & "Client write failed")
            txtLog.AppendText(Environment.NewLine & ex.Message)
            ret = False
        End Try
        SendMessage = ret
    End Function

    ' ReadReturn
    ' Reads the response from the Arduino from the command just sent.
    ' response is placed into responseData
    ' Returns True if everything is OK, False if there is an error
    Private Function ReadReturn(clientStream As NetworkStream, ByRef responseData As String, cmd As String) As Boolean
        Dim ret As Boolean = True

        If (Not gClient.Connected()) Then
            txtLog.AppendText(Environment.NewLine & "Client Read skipped since gClient is not connected")
            responseData = "Client Read skipped since gClient is not connected"
            ReadReturn = False
            Exit Function
        End If

        Dim sleepCount As Integer = 0
        While ((Not clientStream.DataAvailable) And (sleepCount < 8000))
            Thread.Sleep(100)              ' seems to need some time to get response
            sleepCount = sleepCount + 100
        End While

        ' Buffer to store the response bytes.
        Dim data(255) As Byte

        ' String to store the response ASCII representation.
        responseData = String.Empty
        If (sleepCount >= 8000) Then
            responseData = "Response Timed Out after 8 seconds"
            ret = False
        End If

        ' Read the first batch of the TcpServer response bytes.
        Dim blockCnt As Integer = 0
        While (clientStream.DataAvailable)
            Dim bytes As Int32 = clientStream.Read(data, 0, data.Length)
            responseData = responseData & System.Text.Encoding.ASCII.GetString(data, 0, bytes - 1)    ' bytes-1 to drop the null char
            blockCnt = blockCnt + 1
        End While

        If (cmd <> ACTION_GET_SHUTTER_STATUS) Or (LogStatus) Then
            txtLog.AppendText(Environment.NewLine & "From Server " & sleepCount.ToString & " sec. BlockCnt= " & blockCnt & " : " & responseData)
        End If
        ReadReturn = ret
    End Function

    ' Utility function to see what values are in the gClient
    Private Sub DescribeConnection()
        'txtLog.AppendText(Environment.NewLine & msg)
        If (IsNothing(gClient)) Then
            txtLog.AppendText(Environment.NewLine & "  Empty gClient")
        ElseIf (IsNothing(gClient.Client)) Then
            txtLog.AppendText(Environment.NewLine & "  Empty gClient.Client")
        Else
            txtLog.AppendText(Environment.NewLine & "  Connected " & gClient.Connected.ToString())
            txtLog.AppendText(Environment.NewLine & "  Client Connected " & gClient.Client.Connected.ToString())
            txtLog.AppendText(Environment.NewLine & "  Client Handle " & gClient.Client.Handle.ToString())
            txtLog.AppendText(Environment.NewLine & "  Client IsBound " & gClient.Client.IsBound.ToString())
            txtLog.AppendText(Environment.NewLine & "  Client RemoteEndPoint " & gClient.Client.RemoteEndPoint.ToString())
            txtLog.AppendText(Environment.NewLine & "  Client Ttl " & gClient.Client.Ttl.ToString())


        End If
    End Sub

    Public Sub SetOperationPending(pending As Boolean)
        If pending Then
            CommStatus = COMM_BUSY
            'Else
            'CommStatus = COMM_CONNECTED
        End If
    End Sub

    Public Sub SetLogStatus(val As Boolean)
        ' Turn on/off whether we want verbose logging; i.e., do we want status check messages displayed
        LogStatus = val
    End Sub

End Class
