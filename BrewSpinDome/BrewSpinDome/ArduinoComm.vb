Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Linq
Imports System.Net
Imports System.Net.Sockets
Imports System.Text
Imports System.Windows.Forms
Imports System.Threading
Imports ASCOM.Utilities

' This class contains the code necessary to communicate through TCP socket to the Arduino computer
' using the cc3000 WiFi shield
Public Class ArduinoComm
    Private gConnectPort As Integer = 1552
    Private gHostName As String = "192.168.1.30"
    Private TL As TraceLogger ' Private variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
    Private gLockObject As Object           ' syncing write/reads

    Private myMessage As String = ""
    Private gClient As TcpClient
    'Private serverEndPoint As New IPEndPoint(IPAddress.Parse("192.168.1.30"), 1552)
    Private gEncoder As New ASCIIEncoding()

    Private gRestartConnect As Boolean = False
    Private gConnectTimeoutObject As New ManualResetEvent(False)
    Private gIsConnectionSuccessful As Boolean = False

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

    Public Const DRIVER_SUCCESS As String = "OK"            ' Arduino indicates success

    Public Sub New(ipAddress As String, port As Integer, mytl As TraceLogger, lockobj As Object)
        CommStatus = COMM_NONE
        gConnectPort = gConnectPort
        gHostName = ipAddress
        gLockObject = lockobj
        TL = mytl
    End Sub

    Public Sub ConnectArduino()
        CommStatus = COMM_CONNECTING
        'If (IsNothing(gClient)) Then
        gClient = New TcpClient()
        'End If

        SyncLock gLockObject
            Try
                TL.LogMessage("  ArduinoComm-ConnectArduino", "Begin Connecting")
                gConnectTimeoutObject.Reset()
                gClient.BeginConnect(gHostName, gConnectPort, New AsyncCallback(AddressOf onConnectCompleted), gClient)
            Catch ex As Exception
                TL.LogMessage("  ArduinoComm-ConnectArduino", "!!BeginConnect Failed. " & ex.Message)
            End Try

            If (gConnectTimeoutObject.WaitOne(5000, False)) Then
                ' Signal received
                If (gIsConnectionSuccessful) Then
                    TL.LogMessage("  ArduinoComm-ConnectArduino", "Connected ")
                    CommStatus = COMM_CONNECTED
                Else
                    '   didn't work    
                    TL.LogMessage("  ArduinoComm-ConnectArduino", "!!Connection failed")
                    CommStatus = COMM_FAILED
                End If
            Else
                ' Timed out
                gClient.Close()
                TL.LogMessage("  ArduinoComm-ConnectArduino", "!!Connection timed out")
                CommStatus = COMM_FAILED
            End If
        End SyncLock
    End Sub

    Public Sub DisconnectArduino()
        gClient.Close()
        TL.LogMessage("  ArduinoComm-DisConnectArduino", "Connection Closed")
        CommStatus = COMM_NONE
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
                    SyncLock gLockObject
                        Dim clientStream As NetworkStream = gClient.GetStream()
                        TL.LogMessage("  ArduinoComm-RequestAction", "  Send cmd " & cmd)
                        If (SendMessage(clientStream, cmd)) Then
                            If (ReadReturn(clientStream, ret, cmd)) Then
                                ' Everything looks good
                                CommStatus = COMM_CONNECTED
                                TL.LogMessage("  ArduinoComm-RequestAction", "  Returned " & ret)
                            Else
                                CommStatus = COMM_FAILED        ' the read failed
                                TL.LogMessage("  ArduinoComm-RequestAction", "  !!Read failed. " & ret)
                            End If
                        Else
                            ' The Send failed
                            CommStatus = COMM_FAILED
                            TL.LogMessage("  ArduinoComm-RequestAction", "  !!Send Failed for " & cmd)
                            ret = "Send Failed"
                        End If
                    End SyncLock
                Else    ' gClient not connected
                    TL.LogMessage("  ArduinoComm-RequestAction", "!!Client is not connected")
                    CommStatus = COMM_FAILED
                    ret = "Client is not connected"
                End If
            Else      ' gClient is Nothing
                TL.LogMessage("  ArduinoComm-RequestAction", "!!Client is not connected")
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
        TL.LogMessage("  ArduinoComm-SendMessage", "Sending msg " & msg)
        Try
            clientStream.Write(buffer, 0, buffer.Length)
            clientStream.Flush()
        Catch ex As System.IO.IOException
            TL.LogMessage("  ArduinoComm-SendMessage", "!!Client write failed")
            TL.LogMessage("  ArduinoComm-SendMessage", ex.Message)
            ret = False
        Catch ex As Exception
            ' When the shutter finishes opening/closing, it disconnects
            ' from the Arduino. This causes the read command to crash
            ' if it is reading a status command, for example
            TL.LogMessage("  ArduinoComm-SendMessage", "Arduino disconnected " & ex.Message)
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
            TL.LogMessage("  ArduinoComm-ReadReturn", "Client Read skipped since gClient is not connected")
            responseData = "Client Read skipped since gClient is not connected"
            ReadReturn = False
            Exit Function
        End If

        Dim sleepCount As Integer = 0
        Try
            While ((Not clientStream.DataAvailable) And (sleepCount < 3000))
                Thread.Sleep(100)              ' seems to need some time to get response
                sleepCount = sleepCount + 100
            End While
        Catch ex As Exception
            ' When the shutter finishes opening/closing, it disconnects
            ' from the Arduino. This causes the read command to crash
            ' if it is reading a status command, for example
            responseData = "Arduino disconnected for cmd: " & cmd
            Return False
        End Try

        ' Buffer to store the response bytes.
        Dim data(255) As Byte

        ' String to store the response ASCII representation.
        responseData = String.Empty
        If (sleepCount >= 3000) Then
            responseData = "Response Timed Out for cmd: " & cmd
            Return False
        End If

        ' Read the first batch of the TcpServer response bytes.
        While (clientStream.DataAvailable)
            Dim bytes As Int32 = clientStream.Read(data, 0, data.Length)
            responseData = responseData & System.Text.Encoding.ASCII.GetString(data, 0, bytes - 1)    ' bytes-1 to drop the null char
        End While

        TL.LogMessage("  ArduinoComm-ReadReturn", "From Server (msec) " & sleepCount.ToString & "for cmd: " & cmd & ": " & responseData)
        ReadReturn = ret
    End Function


    Public Sub SetOperationPending(pending As Boolean)
        If pending Then
            CommStatus = COMM_BUSY
            'Else
            'CommStatus = COMM_CONNECTED
        End If
    End Sub

End Class
