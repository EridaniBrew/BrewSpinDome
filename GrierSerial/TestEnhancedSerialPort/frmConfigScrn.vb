'Copyright (C) Richard L. Grier, 2007-2011
Imports System.Text
Imports System.Management
Public Class frmConfigScrn
    '***********  Communication Settings Configuration Form

    Private NewData As Int32
    Private NewStop As Int32
    Private NewParity As Int32
    '
    '--- No parity option button
    '
    Private Sub NoParity_CheckedChanged(ByVal eventSender _
    As System.Object, ByVal eventArgs As System.EventArgs) _
                            Handles NoParity.CheckedChanged
        If eventSender.Checked Then
            NewParity = TestEnhanced.SerialPort.Parity.None
        End If
    End Sub
    '
    '--- Odd parity option button
    '
    Private Sub OddParity_CheckedChanged(ByVal eventSender _
    As System.Object, ByVal eventArgs As System.EventArgs) _
                            Handles OddParity.CheckedChanged
        If eventSender.Checked Then
            NewParity = TestEnhanced.SerialPort.Parity.Odd
        End If
    End Sub
    '
    '--- Even parity option button
    '
    Private Sub EvenParity_CheckedChanged(ByVal eventSender _
    As System.Object, ByVal eventArgs As System.EventArgs) _
                            Handles EvenParity.CheckedChanged
        If eventSender.Checked Then
            NewParity = TestEnhanced.SerialPort.Parity.Even
        End If
    End Sub

    '--- Initialize and display configuration form
    '
    Private Sub frmConfigScrn_Load(ByVal eventSender As  _
    System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Dim I As Short
        'change all references from TestEnhanced.SerialPort to
        'TestEnhanced.SerialPort1 if using the built-in SerialPort control

        Dim PortNames() As String = TestEnhanced.SerialPort.GetPortNames()
        lstCommPort.Items.Clear()
        For I = PortNames.Length - 1 To 0 Step -1
            lstCommPort.Items.Add(ConvertUnicode2ASCII(PortNames(I)))
        Next

        '''''''''//
        ''comment out the following block if you are using the built in serial port object
        Dim PortDescriptions() As String = TestEnhanced.SerialPort.PortDescriptions
        lstDescription.Items.Clear()
        For I = 0 To PortDescriptions.Length - 1
            lstDescription.Items.Add(PortDescriptions(I))
        Next
        '''''''''//

        With lstRate
            .Items.Clear()
            .Items.Add("300")
            .Items.Add("1200")
            .Items.Add("2400")
            .Items.Add("4800")
            .Items.Add("9600")
            .Items.Add("19200")
            .Items.Add("38400")
            .Items.Add("57600")
            .Items.Add("115200")
        End With

        With TestEnhanced.SerialPort
            '--- Get current port
            lstCommPort.SelectedIndex = lstCommPort.FindString(.PortName)
            If lstCommPort.SelectedIndex = -1 Then lstCommPort.SelectedIndex = 0

            '--- Get current rate
            Select Case .BaudRate 'select rate
                Case 300 'set active baud
                    lstRate.SelectedIndex = 0
                Case 1200
                    lstRate.SelectedIndex = 1
                Case 2400
                    lstRate.SelectedIndex = 2
                Case 4800
                    lstRate.SelectedIndex = 3
                Case 9600
                    lstRate.SelectedIndex = 4
                Case 19200
                    lstRate.SelectedIndex = 5
                Case 38400
                    lstRate.SelectedIndex = 6
                Case 57600
                    lstRate.SelectedIndex = 7
                Case 115200
                    lstRate.SelectedIndex = 8
                Case Else
                    lstRate.SelectedIndex = 4
            End Select

            '--- Get current parity
            NewParity = .Parity
            Select Case .Parity
                Case .Parity.None   'set active parity
                    NoParity.Checked = True 'option button
                Case .Parity.Even
                    EvenParity.Checked = True
                Case .Parity.Odd
                    OddParity.Checked = True
            End Select

            '--- Get data bits
            NewData = .DataBits
            Select Case .DataBits 'select data bits
                Case 7 'set active choice
                    Data7.Checked = True 'option button
                Case 8
                    Data8.Checked = True
            End Select

            '--- Get stop bits
            NewStop = .StopBits
            Select Case .StopBits  'select stop bits
                Case .StopBits.One 'set active choice
                    Stop1.Checked = True 'option button
                Case .StopBits.Two
                    Stop2.Checked = True
            End Select
        End With
    End Sub
    '
    '--- 1 stop bit option button
    '
    Private Sub Stop1_CheckedChanged(ByVal eventSender _
    As System.Object, ByVal eventArgs As System.EventArgs) _
                            Handles Stop1.CheckedChanged
        If eventSender.Checked Then
            NewStop = TestEnhanced.SerialPort.StopBits.One
        End If
    End Sub
    '
    '--- 2 stop bits option button
    '
    Private Sub Stop2_CheckedChanged(ByVal eventSender As  _
    System.Object, ByVal eventArgs As System.EventArgs) _
                            Handles Stop2.CheckedChanged
        If eventSender.Checked Then
            NewStop = TestEnhanced.SerialPort.StopBits.Two
        End If
    End Sub
    '
    '--- 8 data bits option button
    '
    Private Sub Data8_CheckedChanged(ByVal sender As  _
        System.Object, ByVal e As System.EventArgs) _
        Handles Data8.CheckedChanged
        NewData = 8
    End Sub
    '
    '--- 7 data bits option button
    '
    Private Sub Data7_CheckedChanged(ByVal sender As  _
        System.Object, ByVal e As System.EventArgs) _
        Handles Data7.CheckedChanged
        NewData = 7
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e _
    As System.EventArgs) Handles Button1.Click
        '
        '--- Ok button actions
        '
        Dim OldPort As String
        Dim PortOpen As Boolean

        With TestEnhanced.SerialPort
            OldPort = .PortName
            PortOpen = .IsOpen
            If PortOpen = True Then .Close()
            .BaudRate = Val(lstRate.Text)
            .Parity = NewParity
            .DataBits = NewData
            .StopBits = NewStop
            .PortName = lstCommPort.SelectedItem
            'set new port number
            If PortOpen = True Then
                Try
                    .Open()
                Catch Ex As Exception
                    MsgBox(Err.Description)
                Finally
                    If .IsOpen = False Then
                        MsgBox("Selected port could not be opened", MsgBoxStyle.Exclamation)
                        .PortName = OldPort
                    End If
                End Try
            End If
            If Watchdog.Checked Then
                .WatchdogTimeout = 10000
                .WatchdogReopenTime = 30000
                .EnableWatchdog = True
            Else
                .EnableWatchdog = False
            End If
        End With
        Me.Close() 'remove configuration form
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e _
    As System.EventArgs) Handles Button2.Click
        '--- Cancel button actions
        '
        '
        Me.Close()
    End Sub

    Private Function ConvertUnicode2ASCII(ByVal UString As String) As String
        Dim ascii As Encoding = Encoding.ASCII
        Dim [unicode] As Encoding = Encoding.Unicode
        'the string returned from a PortNames call may be unicode.  Use the
        'following block to convert to ASCII
        Dim unicodeBytes As Byte() = [unicode].GetBytes(UString)
        Dim asciiBytes As Byte() = Encoding.Convert([unicode], ascii, unicodeBytes)
        Dim asciiChars(ascii.GetCharCount(asciiBytes, 0, asciiBytes.Length)) As Char
        ascii.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0)
        Dim asciiString As New String(asciiChars)
        asciiString = asciiString.Trim(Chr(0), "c")  'this seems to be needed for some Bluetooth adapters?
        Return asciiString.Trim(Chr(0), "?")
        'then trim the trailing null and extraneous ? character
    End Function

    Private Sub lstCommPort_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstCommPort.Click
        'lstDescription.SelectedIndex = lstCommPort.SelectedIndex
    End Sub

    Private Sub lstDescription_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstDescription.Click
        lstCommPort.SelectedIndex = lstDescription.SelectedIndex
    End Sub
End Class