Imports System.Threading
Imports System.Resources

' brew  1.1.0   Initial version
' brew  2.0.0   Re-arranged buttons, icons. Changed shutter icons from LEDs to animated GIFs
' brew  2.0.1   Added ability to turn off Status messages in Trace log screen
' brew  2.0.2   Changes to connection logic: 
'                 a) One set of connection code instead of two.
'                 b) disable heartBeatTimer while connection and configuration are in progress
'                 c) firstTimeStatus = true after connection is established to get correct shutter icons
'       2.1.0   Removed configuration of motor speed. I always run with 1023 anyway.
'       2.1.1   Changed voltage scaling to match board rebuild. Matched to linear fit of 4 readings instead of 1 reading.

Public Class SimpleShutter
    ' Switch status on shutters
    Public upperOpenSw As Boolean = False          ' True means it is on/closed
    Public upperClosedSw As Boolean = False
    Public lowerOpenSw As Boolean = False
    Public lowerClosedSw As Boolean = False
    Public upperCurrent As Double = 0.0
    Public lowerCurrent As Double = 0.0
    Public powerVoltage As Double = 0.0
    Public statTemperature As Double = 0.0
    Public operationInProgress As Boolean = False

    Public firstTimeStatus As Boolean = True        ' flag to set initial shutter state

    Private gTimeout As Long                    ' Shutter should open or close in this time
    Private gUpperTimeout As Long = 45000         ' timeout for the open/close of upper shutter
    Private gLowerTimeout As Long = 15000         ' timeout for the open/close of lower shutter
    
    Private Const OP_OPEN_BOTH As String = "OpenBoth"
    Private Const OP_CLOSE_BOTH As String = "CloseBoth"
    Private Const OP_OPEN_UPPER As String = "OpenUpper"
    Private Const OP_CLOSE_UPPER As String = "CloseUpper"
    Private Const OP_OPEN_LOWER As String = "OpenLower"
    Private Const OP_CLOSE_LOWER As String = "CloseLower"
    Private Const OP_VENT As String = "Vent"
    Private gActiveOperation As String              ' holds one of the above operations for the motorTimer to know what to do

    Private gArduinoAvailable As Boolean = False    ' HeartBeat checks whether Arduino is communicating

    Private gArdComm As ArdComm                 ' Comm class to Arduino WiFi

    Private Const DRIVER_SUCCESS As String = "OK"

    ' Status message colors
    Private GoodColor As System.Drawing.Color = Color.ForestGreen
    Private ErrorColor As System.Drawing.Color = Color.Red
    Private RunningColor As System.Drawing.Color = Color.DarkTurquoise

    Private gHeartBeatImages(5) As Image

    ' These variables implement making the program run in Console mode, run from the command line
    Dim gConsoleOp As String
    Private Const CON_OP_OPEN As String = "OPEN"
    Private Const CON_OP_CLOSE As String = "CLOSE"

    Dim gConsoleState As Integer
    Private Const CON_CONNECTING As Integer = 0
    Private Const CON_OPENING As Integer = 1
    Private Const CON_CLOSING As Integer = 2
    Private Const CON_WHATEVER As Integer = 3


    ' Get shutter switch status using the Action GetShutterStatus
    Public Sub RetrieveStatus()
        Dim statStr As String
        
        statStr = gArdComm.RequestAction(ArdComm.ACTION_GET_SHUTTER_STATUS)
            ' statStr should look like 
        ' "Status,%c,%c,%c,%c,%5d,%5d,%5d,%5d,%c", upperOpen, upperClosed, lowerOpen, lowerClosed, rawUpCurrent, rawLowCurrent, rawVoltage, rawTemperature, OpInProgress
        Dim statPieces As String() = statStr.Split(New Char() {","c})
        If (statPieces(0) = "Status") Then
            upperOpenSw = (statPieces(1) = "C")
            upperClosedSw = (statPieces(2) = "C")
            lowerOpenSw = (statPieces(3) = "C")
            lowerClosedSw = (statPieces(4) = "C")
            upperCurrent = CDbl(statPieces(5)) / 30.72                     ' Found article indicating that current may be count / 30.72
            lowerCurrent = CDbl(statPieces(6)) / 30.72
            powerVoltage = CDbl(statPieces(7)) * (0.0163) - 0.4217          ' measured 4 voltages and corresponding counts
            ' Voltage divider is 10K and 22K
            statTemperature = ((CDbl(statPieces(8)) * 4.75 / 1024 - 0.5) * 100) * 9 / 5 + 32    ' Vcc ,easured 4.75V
            operationInProgress = (statPieces(9) = "T")
            gArduinoAvailable = True
            gArdComm.SetOperationPending(operationInProgress)
        Else
            'bad status string 
            gArduinoAvailable = False
        End If

    End Sub

    ' Configure the various buttons on the screen (Enable/Disable) based on the Shutter Switch Status
    '  If Arduino is not communicating, disable all buttons
    Private Sub SetOperationsState()
        RetrieveStatus()
        If (gArduinoAvailable) Then
            If (firstTimeStatus) Then
                InitializeShutterLights()
                firstTimeStatus = False
            End If
            

            lblUpperCurrent.Text = upperCurrent.ToString("0.00")
            lblLowerCurrent.Text = lowerCurrent.ToString("0.00")
            lblVoltage.Text = powerVoltage.ToString("0.0") & " V"
            lblTemperature.Text = statTemperature.ToString("0") & " F"

            If (Not operationInProgress) Then
                ' Now enable/disable buttons
                EnableButton(btnAbortMotors, True)
                '                                           Upper      Lower
                If (upperOpenSw) Then                       'Open
                    EnableButton(btnCloseUpper, True)
                    EnableButton(btnVent, False)
                    EnableButton(btnOpenUpper, False)
                    EnableButton(btnCloseBoth, True)
                    If (lowerOpenSw) Then                   'Open       open
                        EnableButton(btnCloseLower, True)
                        EnableButton(btnOpenLower, False)
                        EnableButton(btnOpenBoth, False)
                        EnableButton(btnCloseUpper, False)
                    ElseIf (lowerClosedSw) Then             'Open       closed
                        EnableButton(btnOpenBoth, True)
                        EnableButton(btnCloseLower, False)
                        EnableButton(btnOpenLower, True)
                    Else   ' lower in between               ' Open      between
                        EnableButton(btnOpenBoth, True)
                        EnableButton(btnCloseLower, True)
                        EnableButton(btnOpenLower, True)
                    End If
                ElseIf (upperClosedSw) Then                 'Closed
                    EnableButton(btnCloseUpper, False)
                    EnableButton(btnVent, True)
                    EnableButton(btnOpenUpper, True)
                    EnableButton(btnOpenBoth, True)
                    EnableButton(btnCloseLower, False)
                    EnableButton(btnOpenLower, False)
                    EnableButton(btnCloseBoth, False)
                Else                                        'between
                    EnableButton(btnCloseUpper, True)
                    EnableButton(btnVent, True)
                    EnableButton(btnOpenUpper, True)
                    EnableButton(btnCloseBoth, True)
                    EnableButton(btnOpenBoth, True)
                    If (lowerOpenSw) Then                   'between     open
                        EnableButton(btnCloseLower, True)
                        EnableButton(btnOpenLower, False)
                        EnableButton(btnCloseUpper, False)
                    ElseIf (lowerClosedSw) Then             'between     closed
                        EnableButton(btnCloseLower, False)
                        EnableButton(btnOpenLower, True)
                    Else   ' lower in between               ' between    between
                        EnableButton(btnCloseLower, True)
                        EnableButton(btnOpenLower, True)
                        EnableButton(btnCloseUpper, False)
                    End If
                End If
            Else
                ' motor operation is in progress
                If (motorTimer.Tag <> OP_CLOSE_UPPER) Then EnableButton(btnCloseUpper, False)
                If (motorTimer.Tag <> OP_VENT) Then EnableButton(btnVent, False)
                If (motorTimer.Tag <> OP_OPEN_UPPER) Then EnableButton(btnOpenUpper, False)
                If (motorTimer.Tag <> OP_CLOSE_BOTH) Then EnableButton(btnCloseBoth, False)
                If (motorTimer.Tag <> OP_OPEN_BOTH) Then EnableButton(btnOpenBoth, False)
                If (motorTimer.Tag <> OP_CLOSE_LOWER) Then EnableButton(btnCloseLower, False)
                If (motorTimer.Tag <> OP_OPEN_LOWER) Then EnableButton(btnOpenLower, False)
            End If
        Else
            ' Arduino shows no heartbeat. Disable buttons
            picUpperLight.Image = My.Resources.ResourceManager.GetObject("grayLightOff")
            picLowerLight.Image = My.Resources.ResourceManager.GetObject("grayLightOff")
            EnableButton(btnCloseUpper, False)
            EnableButton(btnVent, False)
            EnableButton(btnOpenUpper, False)
            EnableButton(btnCloseBoth, False)
            EnableButton(btnOpenBoth, False)
            EnableButton(btnCloseLower, False)
            EnableButton(btnOpenLower, False)
        End If

    End Sub

    ' When button is enabled, make the background palegreen. 
    ' When disabled make the background transparent
    Private Sub EnableButton(button As Button, newState As Boolean)
        If (newState) Then
            button.BackColor = Color.PaleGreen
        Else
            button.BackColor = Color.Transparent
        End If
        button.Enabled = newState
    End Sub

    ' Set the desired image for the shutter 
    ' This only happens once, first time status is read. 
    ' After that, images are changed when operations are started or completed
    Private Sub InitializeShutterLights()
        If (upperOpenSw) Then
            picUpperLight.Image = My.Resources.ResourceManager.GetObject("Open")
        ElseIf (upperClosedSw) Then
            picUpperLight.Image = My.Resources.ResourceManager.GetObject("Closed")
        Else
            picUpperLight.Image = My.Resources.ResourceManager.GetObject("Between")
        End If
        If (lowerOpenSw) Then
            picLowerLight.Image = My.Resources.ResourceManager.GetObject("Open")
        ElseIf (lowerClosedSw) Then
            picLowerLight.Image = My.Resources.ResourceManager.GetObject("Closed")
        Else
            picLowerLight.Image = My.Resources.ResourceManager.GetObject("Between")
        End If
    End Sub

    Private Sub SetMotorTimeout(timeoutInMsec As Long, tag As String)
        gTimeout = timeoutInMsec
        motorTimer.Tag = tag
        motorTimer.Enabled = True
    End Sub

    ' Open both the upper and lower shutters
    ' This will use the standard driver open call
    ' motorTimer is used to check when shutter positioning is achieved
    Private Sub btnOpenBoth_Click(sender As Object, e As EventArgs) Handles btnOpenBoth.Click
        Cursor = Cursors.WaitCursor

        EnableButton(sender, False)
        sender.BackColor = RunningColor
        SetStatusMsg("Opening Shutters", GoodColor)
        picUpperLight.Image = My.Resources.ResourceManager.GetObject("Opening")
        picUpperLight.Tag = "opening"
        picLowerLight.Tag = "waiting"
        Dim statStr As String = gArdComm.RequestAction(ArdComm.ACTION_OPEN_BOTH_SHUTTER)
        If (statStr = DRIVER_SUCCESS) Then
            SetMotorTimeout(gUpperTimeout + gLowerTimeout, OP_OPEN_BOTH)
        Else
            SetStatusMsg("Open Both Shutters operation Failed: " + statStr, ErrorColor)
        End If
        SetOperationsState()

        Cursor = Cursors.Default

    End Sub

    Private Sub btnCloseBoth_Click(sender As Object, e As EventArgs) Handles btnCloseBoth.Click
        Cursor = Cursors.WaitCursor

        EnableButton(sender, False)
        sender.BackColor = RunningColor
        SetStatusMsg("Closing Shutters", GoodColor)
        picLowerLight.Image = My.Resources.ResourceManager.GetObject("Closing")
        picUpperLight.Tag = "waiting"
        picLowerLight.Tag = "closing"
        Dim statStr As String = gArdComm.RequestAction(ArdComm.ACTION_CLOSE_BOTH_SHUTTER)
        If (statStr = DRIVER_SUCCESS) Then
            SetMotorTimeout(gUpperTimeout + gLowerTimeout, OP_CLOSE_BOTH)
        Else
            SetStatusMsg("Close Both Shutters operation Failed: " + statStr, ErrorColor)
        End If
        SetOperationsState()
        Cursor = Cursors.Default
    End Sub

    ' Similar to the btnAbort routine
    Private Sub HaltMotors()
        Dim statStr As String

        statStr = gArdComm.RequestAction(ArdComm.ACTION_ABORT_MOTORS)
        If (statStr = DRIVER_SUCCESS) Then
            operationInProgress = False
            'StopMotorTimer("Motors Halted", ErrorColor)
        Else
            SetStatusMsg("Halt Failed: " & statStr, ErrorColor)
        End If
    End Sub

    Private Sub btnAbortMotors_Click(sender As Object, e As EventArgs) Handles btnAbortMotors.Click
        Dim statStr As String
        Cursor = Cursors.WaitCursor
        EnableButton(sender, False)
        sender.BackColor = RunningColor

        SetStatusMsg("Halting Motor Movement", ErrorColor)
        statStr = gArdComm.RequestAction(ArdComm.ACTION_ABORT_MOTORS)
        If (statStr = DRIVER_SUCCESS) Then
            operationInProgress = False
            StopMotorTimer("Motors Halted", ErrorColor)
        Else
            SetStatusMsg("Halt Failed: " & statStr, ErrorColor)
        End If
        firstTimeStatus = True          ' trigger setting shutter icons based on status
        SetOperationsState()
        Cursor = Cursors.Default
    End Sub

    ' when a motor operation starts, it uses this callback to check whether it has completed (or timed out with gTimeout)
    ' the Tag on the timer holds the active operation being timed (see OP_OPEN_BOTH and associated constants)
    Private Sub motorTimer_Tick(sender As Object, e As EventArgs) Handles motorTimer.Tick
        gTimeout -= motorTimer.Interval
        If (gTimeout <= 0) Then
            ' operation has timed out
            StopMotorTimer("Operation timed out", ErrorColor)
            HaltMotors()
            Exit Sub
        End If

        RetrieveStatus()
        picHeartBeat.Image = gHeartBeatImages(gArdComm.CommStatus)
        Select Case (motorTimer.Tag)
            Case OP_OPEN_BOTH
                If ((upperOpenSw) And (picUpperLight.Tag = "opening")) Then
                    picUpperLight.Tag = "done"
                    picLowerLight.Tag = "opening"
                    picUpperLight.Image = My.Resources.ResourceManager.GetObject("Open")
                    picLowerLight.Image = My.Resources.ResourceManager.GetObject("Opening")
                End If
                If ((lowerOpenSw) And (picLowerLight.Tag = "opening")) Then
                    picLowerLight.Tag = "done"
                    picLowerLight.Image = My.Resources.ResourceManager.GetObject("Open")
                End If
                If ((upperOpenSw) And (lowerOpenSw)) Then
                    picUpperLight.Image = My.Resources.ResourceManager.GetObject("Open")
                    picLowerLight.Image = My.Resources.ResourceManager.GetObject("Open")
                    picUpperLight.Tag = "done"
                    picLowerLight.Tag = "done"
                    StopMotorTimer("Shutters are Open", GoodColor)
                End If
            Case OP_CLOSE_BOTH
                If ((lowerClosedSw) And (picLowerLight.Tag = "closing")) Then
                    picLowerLight.Tag = "done"
                    picUpperLight.Tag = "closing"
                    picLowerLight.Image = My.Resources.ResourceManager.GetObject("Closed")
                    picUpperLight.Image = My.Resources.ResourceManager.GetObject("Closing")
                End If
                If ((upperClosedSw) And (picUpperLight.Tag = "closing")) Then
                    picUpperLight.Tag = "done"
                    picUpperLight.Image = My.Resources.ResourceManager.GetObject("Closed")
                End If
                If ((upperClosedSw) And (lowerClosedSw)) Then
                    picUpperLight.Image = My.Resources.ResourceManager.GetObject("Closed")
                    picLowerLight.Image = My.Resources.ResourceManager.GetObject("Closed")
                    picUpperLight.Tag = "done"
                    picLowerLight.Tag = "done"
                    StopMotorTimer("Shutters are Closed", GoodColor)
                End If
            Case OP_OPEN_UPPER
                If (upperOpenSw) Then
                    picUpperLight.Image = My.Resources.ResourceManager.GetObject("Open")
                    StopMotorTimer("Upper Shutter is Open", GoodColor)
                End If
            Case OP_CLOSE_UPPER
                If (upperClosedSw) Then
                    picUpperLight.Image = My.Resources.ResourceManager.GetObject("Closed")
                    StopMotorTimer("Upper Shutter is Closed", GoodColor)
                End If
            Case OP_OPEN_LOWER
                If (lowerOpenSw) Then
                    picLowerLight.Image = My.Resources.ResourceManager.GetObject("Open")
                    StopMotorTimer("Lower Shutter is Open", GoodColor)
                End If
            Case OP_CLOSE_LOWER
                If (lowerClosedSw) Then
                    picLowerLight.Image = My.Resources.ResourceManager.GetObject("Closed")
                    StopMotorTimer("Lower Shutter is Closed", GoodColor)
                End If
            Case OP_VENT
                If (Not operationInProgress) Then
                    picUpperLight.Image = My.Resources.ResourceManager.GetObject("Between")
                    StopMotorTimer("Upper Shutter Vented", GoodColor)
                End If
            Case Else
        End Select
        If (motorTimer.Enabled = False) Then
            SetOperationsState()                    ' we turned off timer, so state has changed.
        End If

    End Sub

    Private Sub StopMotorTimer(s As String, textColor As Drawing.Color)
        motorTimer.Enabled = False      ' stop the timer
        SetStatusMsg(s, textColor)
    End Sub


    Private Sub btnOpenUpper_Click(sender As Object, e As EventArgs) Handles btnOpenUpper.Click
        Cursor = Cursors.WaitCursor
        EnableButton(sender, False)
        sender.BackColor = RunningColor
        SetStatusMsg("Opening Upper Shutter", GoodColor)
        picUpperLight.Image = My.Resources.ResourceManager.GetObject("Opening")
        Dim statStr As String = gArdComm.RequestAction(ArdComm.ACTION_OPEN_UPPER_SHUTTER)
        If (statStr = DRIVER_SUCCESS) Then
            SetMotorTimeout(gUpperTimeout, OP_OPEN_UPPER)
        Else
            SetStatusMsg("Upper Shutter Open operation Failed: " + statStr, ErrorColor)
        End If
        SetOperationsState()
        Cursor = Cursors.Default
    End Sub

    Private Sub btnCloseUpper_Click(sender As Object, e As EventArgs) Handles btnCloseUpper.Click
        Cursor = Cursors.WaitCursor
        EnableButton(sender, False)
        sender.BackColor = RunningColor
        SetStatusMsg("Closing Upper Shutter", GoodColor)
        picUpperLight.Image = My.Resources.ResourceManager.GetObject("Closing")
        Dim statStr As String = gArdComm.RequestAction(ArdComm.ACTION_CLOSE_UPPER_SHUTTER)
        If (statStr = DRIVER_SUCCESS) Then
            SetMotorTimeout(gUpperTimeout, OP_CLOSE_UPPER)
        Else
            SetStatusMsg("Upper Shutter Close operation Failed: " + statStr, ErrorColor)
        End If
        SetOperationsState()

        Cursor = Cursors.Default
    End Sub

    Private Sub btnOpenLower_Click(sender As Object, e As EventArgs) Handles btnOpenLower.Click
        Cursor = Cursors.WaitCursor
        EnableButton(sender, False)
        sender.BackColor = RunningColor
        SetStatusMsg("Opening Lower Shutter", GoodColor)
        picLowerLight.Image = My.Resources.ResourceManager.GetObject("Opening")
        Dim statStr As String = gArdComm.RequestAction(ArdComm.ACTION_OPEN_LOWER_SHUTTER)
        If (statStr = DRIVER_SUCCESS) Then
            SetMotorTimeout(gLowerTimeout, OP_OPEN_LOWER)
        Else
            SetStatusMsg("Lower Shutter Close operation Failed: " + statStr, ErrorColor)
            sender.BackColor = ErrorColor
        End If
        SetOperationsState()
        Cursor = Cursors.Default

    End Sub

    Private Sub btnCloseLower_Click(sender As Object, e As EventArgs) Handles btnCloseLower.Click
        Cursor = Cursors.WaitCursor
        EnableButton(sender, False)
        sender.BackColor = RunningColor
        SetStatusMsg("Closing Lower Shutter", GoodColor)
        picLowerLight.Image = My.Resources.ResourceManager.GetObject("Closing")
        Dim statStr As String = gArdComm.RequestAction(ArdComm.ACTION_CLOSE_LOWER_SHUTTER)
        If (statStr = DRIVER_SUCCESS) Then
            SetMotorTimeout(gLowerTimeout, OP_CLOSE_LOWER)
        Else
            SetStatusMsg("Lower Shutter Close operation Failed: " + statStr, ErrorColor)
        End If
        SetOperationsState()
        Cursor = Cursors.Default
    End Sub

    Private Sub btnVent_Click(sender As Object, e As EventArgs) Handles btnVent.Click
        Dim statStr As String

        Cursor = Cursors.WaitCursor
        EnableButton(sender, False)
        sender.BackColor = RunningColor
        
        statStr = gArdComm.RequestAction(ArdComm.ACTION_VENT_UPPER_SHUTTER)
        picUpperLight.Image = My.Resources.ResourceManager.GetObject("Opening")
        If (statStr = DRIVER_SUCCESS) Then
            gTimeout = gUpperTimeout
            motorTimer.Tag = OP_VENT
            SetStatusMsg("Venting Upper Shutter", GoodColor)
            motorTimer.Enabled = True
        Else
            SetStatusMsg("Vent operation failed: " + statStr, ErrorColor)
        End If
        SetOperationsState()

        Cursor = Cursors.Default
    End Sub

    Private Sub SetStatusMsg(s As String, textColor As System.Drawing.Color)
        lblStatus.Text = s
        lblStatus.ForeColor = textColor
        System.Windows.Forms.Application.DoEvents()
    End Sub



    ' Checking whether the Arduino is available (the driver pings the Arduino)
    ' Set the light based on the results
    Private Sub heartBeatTimer_Tick(sender As Object, e As EventArgs) Handles heartBeatTimer.Tick
        If (IsNothing(gArdComm)) Then
            gArdComm = New ArdComm(LogDialog.txtLog, txtIPAddress.Text, CInt(txtListenPort.Text))
            gArdComm.CommStatus = ArdComm.COMM_FAILED
        End If

        If (gArdComm.CommStatus = ArdComm.COMM_FAILED) Then
            ' Need to retry connecting
            picHeartBeat.Image = gHeartBeatImages(ArdComm.COMM_CONNECTING)
            heartBeatTimer.Enabled = False
            System.Windows.Forms.Application.DoEvents()
            gArdComm.ConnectArduino()
            If (gArdComm.CommStatus = ArdComm.COMM_CONNECTED) Then
                picHeartBeat.Image = gHeartBeatImages(ArdComm.COMM_BUSY)
                System.Windows.Forms.Application.DoEvents()
                ReconfigureArduino()
                firstTimeStatus = True
            End If
        End If
        picHeartBeat.Image = gHeartBeatImages(gArdComm.CommStatus)

        If (gArdComm.CommStatus = ArdComm.COMM_CONNECTED) Then
            btnArduinoApply.Enabled = True
        Else
            btnArduinoApply.Enabled = False
        End If
        heartBeatTimer.Enabled = True
        SetOperationsState()
        
    End Sub


    ' Pop up/down the Trace Dialog
    Private Sub btnTrace_Click(sender As Object, e As EventArgs) Handles btnTrace.Click
        If (LogDialog.Visible) Then
            LogDialog.Hide()
        Else
            LogDialog.Show()
        End If

    End Sub

    Private Sub RestoreSettings()
        txtVentTime.Text = My.Settings.VentTime.ToString()
        txtUpperShutterTimeout.Text = My.Settings.UpperShutterTimeout.ToString()
        txtLowerShutterTimeout.Text = My.Settings.LowerShutterTimeout.ToString()
        txtUpperMotorSpeed.Text = My.Settings.UpperMotorSpeed.ToString()
        txtLowerMotorSpeed.Text = My.Settings.LowerMotorSpeed.ToString()
        txtHeartbeatTimerMsec.Text = My.Settings.HeartbeatTimeMsec.ToString()
        txtMotorTimerMsec.Text = My.Settings.MotorCheckTimeMsec.ToString()
        chkReverseUpperMotor.Checked = My.Settings.ReverseUpperMotor
        chkReverseLowerMotor.Checked = My.Settings.ReverseLowerMotor
        LogDialog.Location = New Point(My.Settings.LogDialogLeft, My.Settings.LogDialogTop)
        LogDialog.Width = My.Settings.LogDialogWidth
        LogDialog.Height = My.Settings.LogDialogHeight
        LogDialog.cbVerbose.Checked = My.Settings.LogVerbose
        Me.Location = New Point(My.Settings.FormLeft, My.Settings.FormTop)
    End Sub

    Private Sub SimpleShutter_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        SaveSettings()
        gArdComm.DisconnectClient()
    End Sub

    Private Sub SaveSettings()
        My.Settings.FormLeft = Me.Location.X
        My.Settings.FormTop = Me.Location.Y
        My.Settings.LogDialogLeft = LogDialog.Location.X
        My.Settings.LogDialogTop = LogDialog.Location.Y
        My.Settings.LogDialogWidth = LogDialog.Width
        My.Settings.LogDialogHeight = LogDialog.Height
        My.Settings.LogVerbose = LogDialog.cbVerbose.Checked

        My.Settings.VentTime = CLng(txtVentTime.Text)
        My.Settings.UpperShutterTimeout = CLng(txtUpperShutterTimeout.Text)
        My.Settings.LowerShutterTimeout = CLng(txtLowerShutterTimeout.Text)
        My.Settings.UpperMotorSpeed = CInt(txtUpperMotorSpeed.Text)
        My.Settings.LowerMotorSpeed = CInt(txtLowerMotorSpeed.Text)
        My.Settings.HeartbeatTimeMsec = CLng(txtHeartbeatTimerMsec.Text)
        My.Settings.MotorCheckTimeMsec = CLng(txtMotorTimerMsec.Text)
        My.Settings.ReverseUpperMotor = chkReverseUpperMotor.Checked
        My.Settings.ReverseLowerMotor = chkReverseLowerMotor.Checked

        My.Settings.Save()
    End Sub

    Private Sub SimpleShutter_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Restore saved settings
        RestoreSettings()
        Dim FileVer As String = FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion
        Me.Text = Me.Text & " Version " & FileVer
        heartBeatTimer.Enabled = True

        ' Set up HeartBeat Images
        ' These need to be in the order of values in gArcComm.CommStatus
        gHeartBeatImages(0) = My.Resources.ResourceManager.GetObject("grayLightOff")        'COMM_NONE
        gHeartBeatImages(1) = My.Resources.ResourceManager.GetObject("lightOn")             'COMM_CONNECTED
        gHeartBeatImages(2) = My.Resources.ResourceManager.GetObject("blueLightOn")         'COMM_BUSY
        gHeartBeatImages(3) = My.Resources.ResourceManager.GetObject("redLightOn")          'COMM_FAILED
        gHeartBeatImages(4) = My.Resources.ResourceManager.GetObject("yellowLightOn")       'COMM_CONNECTING
        
        ' Do we have run string arguments? Start Console Mode
        If (My.Application.CommandLineArgs.Count > 0) Then
            gConsoleOp = My.Application.CommandLineArgs.Item(0).ToUpper()
            gConsoleState = CON_CONNECTING
            ConsoleTimer.Enabled = True
        End If

        LogDialog.cbVerbose.Tag = Me
    End Sub

    Private Sub ReconfigureArduino()
        Dim ret As String
        Dim cmd As String

        SetStatusMsg("Configuring Parameters", GoodColor)
        System.Windows.Forms.Application.DoEvents()
        ' Update Vent Time
        cmd = ArdComm.ACTION_RECONFIG_PARAM & "," & "VentTime" & "," & txtVentTime.Text & ","
        ret = gArdComm.RequestAction(cmd)
        If (ret <> DRIVER_SUCCESS) Then
            LogMsg("Failed to reconfigure Vent Time with {" & cmd & "}")
        End If

        ' Update Reverse Upper Motor
        cmd = ArdComm.ACTION_RECONFIG_PARAM & "," & "ReverseUpper" & "," & chkReverseUpperMotor.Checked.ToString() & ","
        ret = gArdComm.RequestAction(cmd)
        If (ret <> DRIVER_SUCCESS) Then
            LogMsg("Failed to reconfigure ReverseUpper with {" & cmd & "}")
        End If

        ' Update Reverse Lower Motor
        cmd = ArdComm.ACTION_RECONFIG_PARAM & "," & "ReverseLower" & "," & chkReverseLowerMotor.Checked.ToString() & ","
        ret = gArdComm.RequestAction(cmd)
        If (ret <> DRIVER_SUCCESS) Then
            LogMsg("Failed to reconfigure Reverse Lower with {" & cmd & "}")
        End If

        ' Update Upper Motor Speed
        'cmd = ArdComm.ACTION_RECONFIG_PARAM & "," & "UpperSpeed" & "," & txtUpperMotorSpeed.Text & ","
        'ret = gArdComm.RequestAction(cmd)
        'If (ret <> DRIVER_SUCCESS) Then
        ' LogMsg("Failed to reconfigure Upper Motor Speed with {" & cmd & "}")
        'End If

        ' Update Lower Motor Speed
        'cmd = ArdComm.ACTION_RECONFIG_PARAM & "," & "LowerSpeed" & "," & txtLowerMotorSpeed.Text & ","
        'ret = gArdComm.RequestAction(cmd)
        'If (ret <> DRIVER_SUCCESS) Then
        'LogMsg("Failed to reconfigure Lower Motor Speed with {" & cmd & "}")
        'End If

        ' Shutter Timeouts are in this prog, not in Arduino
        gUpperTimeout = CLng(txtUpperShutterTimeout.Text)
        gLowerTimeout = CLng(txtLowerShutterTimeout.Text)

        ' Change the Timer values
        heartBeatTimer.Interval = CLng(txtHeartbeatTimerMsec.Text)
        motorTimer.Interval = CLng(txtMotorTimerMsec.Text)
        gArdComm.SetLogStatus(LogDialog.cbVerbose.Checked)

        SetStatusMsg("Ready", GoodColor)

    End Sub

    ' We have made changes to the Arduino settings, need to send the Configure commands to the Arduino
    Private Sub btnArduinoApply_Click(sender As Object, e As EventArgs) Handles btnArduinoApply.Click
        lblConfigApply.Text = "Applying Configuration Changes"
        Cursor = Cursors.WaitCursor
        If (gArdComm.CommStatus = ArdComm.COMM_FAILED) Then
            ' Cannot reconfigure
            MsgBox("Arduino is not connected. Cannot reconfigure")
        Else
            ReconfigureArduino()
        End If
        Cursor = Cursors.Default
        lblConfigApply.Text = "Configuration Complete"

    End Sub


    ' When running in Console mode, this timer does the various actions as though we clicked on the button
    ' gConsoleStatus holds the current status for this operation.
    ' gConsoleOp is either "open" or "close"
    Private Sub ConsoleTimer_Tick(sender As Object, e As EventArgs) Handles ConsoleTimer.Tick
        If (gConsoleState = CON_CONNECTING) Then
            If (gArduinoAvailable) Then
                If gConsoleOp = CON_OP_OPEN Then
                    ' start opening the shutters
                    gConsoleState = CON_OPENING
                    btnOpenBoth_Click(btnOpenBoth, Nothing)
                ElseIf gConsoleOp = CON_OP_CLOSE Then
                    ' start closing the shutters
                    gConsoleState = CON_CLOSING
                    btnCloseBoth_Click(btnCloseBoth, Nothing)
                End If
                Exit Sub
            End If
        End If

        ' in Opening state
        If (gConsoleState = CON_OPENING) Then
            If (Not operationInProgress) Then
                ' Opening must be done
                ' exit the program
                Environment.ExitCode = ShutterPosition()
                Me.Close()
                Exit Sub
            End If
        End If

        ' in Closing state
        If (gConsoleState = CON_CLOSING) Then
            If (Not operationInProgress) Then
                ' Closing must be done
                ' exit the program
                Environment.ExitCode = ShutterPosition()
                Me.Close()
                Exit Sub
            End If
        End If

    End Sub


    Private Sub TabArduino_Leave(sender As Object, e As EventArgs) Handles TabArduino.Leave
        lblConfigApply.Text = " "

    End Sub

    Function ShutterPosition() As Integer
        ' Return 1 if both shutters are open
        ' Return 2 if both shutters are closed
        ' Return 3 otherwise
        Dim retcode As Integer = 0

        If (upperClosedSw And lowerClosedSw) Then
            retcode = 2
        ElseIf (upperOpenSw And lowerOpenSw) Then
            retcode = 1
        Else
            retcode = 3
        End If
        ShutterPosition = retcode
    End Function

    Sub LogMsg(s As String)
        LogDialog.txtLog.AppendText(Environment.NewLine & s)
    End Sub

    Public Sub SetLogStatus(val As Boolean)
        gArdComm.SetLogStatus(val)
    End Sub
End Class
