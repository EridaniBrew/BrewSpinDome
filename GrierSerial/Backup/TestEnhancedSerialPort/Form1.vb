'Copyright (c) Richard L. Grier, 2010-2011
Imports System.Text
Imports System.Threading
Imports System.IO.Ports

'change all references from SerialPort to
'SerialPort1 if using the built-in SerialPort control
'and comment out all code that uses the Enhanced SerialPort Watchdog function

Public Class TestEnhanced
    Public WithEvents SerialPort As New EnhancedSerialPort.SerialPort
    Private Shared m_FormDefInstance As TestEnhanced
    Private Shared m_InitializingDefInstance As Boolean
    Private Shared SerialPortClosing As Boolean
    Private Shared ReceiveBuffer As String = ""
    Private Shared Data2Display As New StringBuilder(4096)
    Private Shared DispData As New DispClass
    'Private Shared LEDState As New LEDState

    Public Shared Property DefInstance() As TestEnhanced
        Get
            If m_FormDefInstance Is Nothing OrElse _
                        m_FormDefInstance.IsDisposed Then
                m_InitializingDefInstance = True
                m_FormDefInstance = New TestEnhanced
                m_InitializingDefInstance = False
            End If
            DefInstance = m_FormDefInstance
        End Get
        Set(ByVal Value As TestEnhanced)
            m_FormDefInstance = Value
        End Set
    End Property

    Private Sub txtTerm_KeyPress(ByVal sender As Object, ByVal e As  _
    System.Windows.Forms.KeyPressEventArgs) Handles txtTerm.KeyPress
        Dim KeyAscii As Int32 = Asc(e.KeyChar)
        With SerialPort
            If .IsOpen = True Then
                .Write(Chr(KeyAscii))
                DefInstance.Tx.BeginInvoke(New ChangeLED(AddressOf ChangeLEDStateWithTimeout), True)
            End If
        End With
        e.Handled = True
        Dim command() As Byte = {2, 1, 1, 1, 2, 3, 3}
    End Sub

    Public Delegate Sub ChangeLED(ByVal TxRx As Boolean)

    Private Shared Sub ChangeLEDStateWithTimeout(ByVal TxRx As Boolean)
        If TxRx = True Then    'Change TxLED
            DefInstance.Tx.BackColor = Color.Green
            DefInstance.Timer1.Enabled = True
        Else
            'Change RxLED
            DefInstance.Rx.BackColor = Color.Green
            DefInstance.Timer1.Enabled = True
        End If
    End Sub

    Private Delegate Sub DisplayData2(ByVal ReceiveBuffer As String)
    'This delegate object marshals receive data from the receive thread context DataReceived to the Windows Form STAThread context
    Class DispClass
        Public Sub Disp(ByVal ReceiveBuffer As String)
            With DefInstance.txtTerm
                If .SelectionStart > 0 AndAlso _
                    .Text.Substring(.SelectionStart - 1) <> vbLf Then
                    ReceiveBuffer = ReceiveBuffer.Replace(vbLf, vbCr)
                ElseIf .SelectionStart > 0 Then
                    ReceiveBuffer = ReceiveBuffer.Replace(vbLf, "")
                End If
                ReceiveBuffer = ReceiveBuffer.Replace(vbCr & vbCr, vbCr)
                ReceiveBuffer = ReceiveBuffer.Replace(vbCr, vbCrLf)
                If Len(ReceiveBuffer) = 1 Then
                    If InStr(ReceiveBuffer, Chr(8)) > 0 Then
                        If (.Text.Length > 0) Then .Text = _
                            .Text.Remove(.Text.Length - 1, 1)
                    End If
                Else
                    .SelectionStart = .Text.Length
                    .SelectedText = ReceiveBuffer
                End If
                If .Text.Length > 4096 Then
                    .Text = Mid(.Text, 2048)
                    If Mid(.Text, 1) = vbLf Then _
                            .Text = Mid(.Text, 2)
                End If
                ReceiveBuffer = ""
            End With
        End Sub
    End Class

    Private Shared Sub DisplayData1(ByVal sender As Object, ByVal e As EventArgs)
        'This event handler marshals receive data from the receive thread context DataReceived to the Windows Form STAThread context
        'ReceiveBuffer has class scope
        With DefInstance.txtTerm
            If .SelectionStart > 0 AndAlso _
                .Text.Substring(.SelectionStart - 1) <> vbLf Then
                ReceiveBuffer = ReceiveBuffer.Replace(vbLf, vbCr)
            ElseIf .SelectionStart > 0 Then
                ReceiveBuffer = ReceiveBuffer.Replace(vbLf, "")
            End If
            ReceiveBuffer = ReceiveBuffer.Replace(vbCr & vbCr, vbCr)
            ReceiveBuffer = ReceiveBuffer.Replace(vbCr, vbCrLf)
            If Len(ReceiveBuffer) = 1 Then
                If InStr(ReceiveBuffer, Chr(8)) > 0 Then
                    If (.Text.Length > 0) Then .Text = _
                        .Text.Remove(.Text.Length - 1, 1)
                End If
            Else
                .SelectionStart = .Text.Length
                .SelectedText = ReceiveBuffer
            End If
            If .Text.Length > 4096 Then
                .Text = Mid(.Text, 2048)
                If Mid(.Text, 1) = vbLf Then _
                        .Text = Mid(.Text, 2)
            End If
        End With
    End Sub

    Public Delegate Sub DisplayData(ByVal Buffer As String)
    'This delegate routine marshals receive data from the receive thread context DataReceived to the Windows Form STAThread context
    Private Shared Sub Display(ByVal Buffer As String)
        With DefInstance.txtTerm
            If .SelectionStart > 0 AndAlso _
                .Text.Substring(.SelectionStart - 1) <> vbLf Then
                Buffer = Buffer.Replace(vbLf, vbCr)
            ElseIf .SelectionStart > 0 Then
                Buffer = Buffer.Replace(vbLf, "")
            End If
            Buffer = Buffer.Replace(vbCr & vbCr, vbCr)
            Buffer = Buffer.Replace(vbCr, vbCrLf)
            .SelectionStart = .Text.Length
            If Buffer.Substring(0, 1) = vbBack Then
                'handle a backspace character (simple-minded - might be enhanced)
                .Text = .Text.Substring(0, .Text.Length - 1)   'erase the last character
                .SelectedText = Buffer.Substring(1)
            Else
                .SelectedText = Buffer
            End If
        End With
    End Sub

    Private Sub TestEnhanced_FormClosing(ByVal sender As Object, ByVal e As  _
    System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        With SerialPort
            If .IsOpen Then
                .DiscardInBuffer()
                .Close()
            End If
        End With
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SerialPort = New EnhancedSerialPort.SerialPort
        With SerialPort
            Debug.Print(SerialPort.WriteBufferSize)
            .DTREnable = True
            .RTSEnable = True
            '.Handshake = IO.Ports.Handshake.RequestToSend
            .ReadBufferSize = 4096
            .ReceivedBytesThreshold = 1
        End With
        DefInstance = Me
    End Sub

    Private Sub ExitToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub PortOpenToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PortOpenToolStripMenuItem.Click
        Dim ex As Exception
        With SerialPort
            If .IsOpen = False Then
                Try
                    .Open()
                Catch ex
                End Try
            Else
                Try
                    SerialPortClosing = True
                    .Close()
                Catch ex
                End Try
            End If
            If .IsOpen = True Then
                PortOpenToolStripMenuItem.Checked = True
                Me.Text = "TestEnhanced using port: " & _
                                    SerialPort.PortName
                .ReceivedBytesThreshold = 1
                SerialPortClosing = False
            Else
                PortOpenToolStripMenuItem.Checked = False
                Me.Text = "TestEnhanced not running"
                SerialPortClosing = True
            End If
        End With
    End Sub

    Private Sub SettingsToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingsToolStripMenuItem.Click
        frmConfigScrn.ShowDialog()
        Me.Text = "TestEnhanced using port: " & _
                    SerialPort.PortName
    End Sub

    Private Sub ClearScreenToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearScreenToolStripMenuItem.Click
        txtTerm.Text = ""
    End Sub

    Private Sub HexDisplayToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HexDisplayToolStripMenuItem.Click
        With HexDisplayToolStripMenuItem
            .Checked = Not .Checked
        End With
    End Sub

    Private Sub AboutToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        MsgBox("TestEnhanced is a simple terminal emulator that illustrates" & _
        vbCr & "an EnhancedSerialPort class -- with watchdog function." & vbCr _
        & "The Watchdog is designed to reconnect a serial port, if that connection is lost." _
        & vbCr & "Such as might happen if a USB serial adapter is removed then restored." & vbCr & vbCr & _
        "Copyright (c) 2006-2010 by Richard L. Grier.")
    End Sub

    Private Sub SerialPort_DataReceived(ByVal BytesAvailable As Integer) Handles SerialPort.DataReceived
        If DefInstance.HexDisplayToolStripMenuItem.Checked = False Then
            ReceiveBuffer = SerialPort.ReadExisting
        Else
            Dim Buffer(0 To BytesAvailable - 1) As Byte
            Dim Text2Display As String = ""
            SerialPort.Read(Buffer, 0, BytesAvailable)
            For I As Integer = 0 To Buffer.Length - 1
                Text2Display += Buffer(I).ToString("X2") & " "
            Next
            ReceiveBuffer = Text2Display
        End If
        If ReceiveBuffer <> "" Then
            'EACH OF THE FOLLOWING MARSHAL DATA TO THE UI, IN SLIGHTLY DIFFERENT WAYS
            'Dim DisplayData As DisplayData = AddressOf DispData.Disp
            'DefInstance.BeginInvoke(DisplayData, ReceiveBuffer)
            'DefInstance.BeginInvoke(New EventHandler(AddressOf DisplayData1))
            DefInstance.txtTerm.BeginInvoke(New DisplayData(AddressOf Display), ReceiveBuffer)
        End If
        DefInstance.Tx.BeginInvoke(New ChangeLED(AddressOf ChangeLEDStateWithTimeout), False)
    End Sub

    Private Sub SerialPort_ErrorReceived(ByVal e As System.IO.Ports.SerialErrorReceivedEventArgs) Handles SerialPort.ErrorReceived
        Debug.WriteLine(e.EventType.ToString)
    End Sub

    Private Sub SerialPort_PinChanged(ByVal e As System.IO.Ports.SerialPinChangedEventArgs) Handles SerialPort.PinChanged
        If System.IO.Ports.SerialPinChange.DsrChanged = SerialPinChange.DsrChanged Then
            If SerialPort.DSRHolding = True Then
                Label1.BackColor = Color.Green
            ElseIf SerialPort.DSRHolding = False Then
                Label1.BackColor = Color.Red
            End If
        End If
        If System.IO.Ports.SerialPinChange.DsrChanged = SerialPinChange.CtsChanged Then
            If SerialPort.CTSHolding = True Then
                Label2.BackColor = Color.Green
            ElseIf SerialPort.DSRHolding = False Then
                Label2.BackColor = Color.Red
            End If
        End If
        If System.IO.Ports.SerialPinChange.DsrChanged = SerialPinChange.CDChanged Then
            If SerialPort.CDHolding = True Then
                Label3.BackColor = Color.Green
            ElseIf SerialPort.DSRHolding = False Then
                Label3.BackColor = Color.Red
            End If
        End If
        If System.IO.Ports.SerialPinChange.DsrChanged = SerialPinChange.Ring Then
            Label4.BackColor = Color.CornflowerBlue
            Thread.Sleep(100)
            Label4.BackColor = Color.Red
        End If
    End Sub

    Private Sub SerialPort_Watchdog(ByVal Flag As EnhancedSerialPort.SerialPort.WatchdogFlags) Handles SerialPort.Watchdog
        Debug.Print(Flag.ToString)
    End Sub

    Private Sub ChooseTerminalFontToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChooseTerminalFontToolStripMenuItem.Click
        FontDialog1.ShowDialog()
        txtTerm.Font = FontDialog1.Font
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If SerialPort.IsOpen Then
            Dim Buffer As String = Chr(2) & "111222333444555666777888999000" & vbCrLf & Chr(3)
            SerialPort.Write(Buffer)
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If SerialPort.IsOpen Then
            Dim Buffer As String = "12345678"
            SerialPort.Write(Buffer)
        End If
    End Sub

    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Timer1.Enabled = False
        If Tx.BackColor = Color.Green Then
            Tx.BackColor = Color.Red
        ElseIf Rx.BackColor = Color.Green Then
            Rx.BackColor = Color.Red
        End If
    End Sub

    Private Sub SerialPort1_DataReceived(ByVal sender As Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        'Suppose that you want to use the built-in SerialPort control (System.IO.Ports.SerialPort)
        'this code is equivalent to the EnhancedSerialPort DataReceived code
        Dim BytesAvailable As Integer = SerialPort1.BytesToRead
        If DefInstance.HexDisplayToolStripMenuItem.Checked = False Then
            ReceiveBuffer = SerialPort1.ReadExisting
        Else
            Dim Buffer(0 To BytesAvailable - 1) As Byte
            Dim Text2Display As String = ""
            SerialPort1.Read(Buffer, 0, BytesAvailable)
            For I As Integer = 0 To Buffer.Length - 1
                Text2Display += Buffer(I).ToString("X2") & " "
            Next
            ReceiveBuffer = Text2Display
        End If
        If ReceiveBuffer <> "" Then
            'EACH OF THE FOLLOWING MARSHAL DATA TO THE UI, IN SLIGHTLY DIFFERENT WAYS
            'Dim DisplayData As DisplayData = AddressOf DispData.Disp
            'DefInstance.BeginInvoke(DisplayData, ReceiveBuffer)
            'DefInstance.BeginInvoke(New EventHandler(AddressOf DisplayData1))
            DefInstance.txtTerm.BeginInvoke(New DisplayData(AddressOf Display), ReceiveBuffer)
        End If
        DefInstance.Tx.BeginInvoke(New ChangeLED(AddressOf ChangeLEDStateWithTimeout), False)
    End Sub
End Class
