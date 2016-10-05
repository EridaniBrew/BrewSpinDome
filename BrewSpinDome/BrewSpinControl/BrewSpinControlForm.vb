
'==================================================
' BrewSprinControl
' Program to exercise the Dome using the BrewSpin driver. Uses the same
' driver calls as regular ASCOM program.
'
' Ver    Name    Change
' 1.1    Brew    Halting now resets the running icon images
'                Disconnect resets the various widgets
'                Halt resets the button text on the Slew East/West buttons
' 1.2    Brew    Added logic to display shutter switches, voltage. Still not working yet.

Public Class BrewSpinControlForm

    Private driver As ASCOM.DriverAccess.Dome

    Private busyColor As Color = Color.MediumSeaGreen

    ' Keep track of driver capabilities so we dont keep sending "Can" requests
    Private DriverCanSetAzimuth As Boolean = False
    Private DriverCanFindHome As Boolean = False
    Private DriverCanPark As Boolean = False
    Private DriverCanSetPark As Boolean = False
    Private DriverCanSyncAzimuth As Boolean = False

    ' Special Actions we can do - not normally in Dome drivers
    Private Const ACTION_SLEW = "Dome:Slew"
    Private Const ACTION_GETVAL = "Dome:GetVal"
    Private Const ACTION_GET_PARAM = "Dome:GetParam"
    Private Const ACTION_UPDATE_TOTALBLOCKS = "Dome:UpdateTotalBlocks"

    Private LBL_COUNT_TICS As String = "Count Tics"
    Private LBL_HALT_COUNT_TICS As String = "Stop Count"
    Private LBL_SLEW_EAST As String = "Slew East"
    Private LBL_SLEW_WEST As String = "Slew West"
    Private LBL_HALT_SLEW As String = "Stop Slew"

    ' used for shutter status readings
    Dim gUpperOpenSw As Boolean = False
    Dim gUpperClosedSw As Boolean = False
    Dim gLowerOpenSw As Boolean = False
    Dim gLowerClosedSw As Boolean = False
    Dim gUpperCurrent As Double = 0.0
    Dim gLowerCurrent As Double = 0.0
    Dim gPowerVoltage As Double = 0.0
    Dim gStatTemperature As Double = 0.0



    ''' <summary>
    ''' This event is where the driver is choosen. The device ID will be saved in the settings.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
    Private Sub buttonChoose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonChoose.Click
        My.Settings.DriverId = ASCOM.DriverAccess.Dome.Choose(My.Settings.DriverId)
        SetUIState()
    End Sub

    ''' <summary>
    ''' Connects to the device to be tested.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
    Private Sub buttonConnect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonConnect.Click
        If (IsConnected) Then
            Try
                driver.Connected = False
                buttonConnect.Text = "Connect"
                SetUIState()
                SetStatusInfo()
                LogMsg("Disconnect completed")
                CheckShutterTimer.Enabled = False
            Catch ex As Exception
                MsgBox("Disconnect failed: " & ex.Message, vbOK, "Error")
                Return
            End Try
        Else
            driver = New ASCOM.DriverAccess.Dome(My.Settings.DriverId)
            Try
                driver.Connected = True
                buttonConnect.Text = "Disconnect"
                CheckShutterTimer.Enabled = True
            Catch ex As System.Exception
                MsgBox("Connect failed: " & ex.Message, vbOK, "Error")
                Return
            End Try
            WhatCanDriverDo()
            SetUIState()
            SetStatusInfo()
            LogMsg("Connect completed")
            lblOffsetAz.Text = driver.Action(ACTION_GET_PARAM, "OffsetAz")
            lblParkAz.Text = driver.Action(ACTION_GET_PARAM, "ParkAz")
            lblBlockCount.Text = driver.Action(ACTION_GET_PARAM, "TotalBlocks")
        End If
    End Sub

    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If IsConnected Then
            driver.Connected = False
        End If
        ' the settings are saved automatically when this application is closed.
    End Sub

    Private Sub WhatCanDriverDo()
        DriverCanSetAzimuth = driver.CanSetAzimuth
        DriverCanFindHome = driver.CanFindHome
        DriverCanPark = driver.CanPark
        DriverCanSetPark = driver.CanSetPark
        DriverCanSyncAzimuth = driver.CanSyncAzimuth

    End Sub
    ''' <summary>
    ''' Sets the state of the UI depending on the device state
    ''' </summary>
    Private Sub SetUIState()
        buttonConnect.Enabled = Not String.IsNullOrEmpty(My.Settings.DriverId)
        buttonChoose.Enabled = Not IsConnected
        'buttonConnect.Text = IIf(IsConnected, "Disconnect", "Connect")

        ' enable the buttons
        btnGoToAz.Enabled = False
        btnHome.Enabled = False
        btnPark.Enabled = False
        btnSetPark.Enabled = False
        btnSlewEast.Enabled = False
        btnSlewWest.Enabled = False
        btnHome.Enabled = False
        btnCountBlocks.Enabled = False
        btnSync.Enabled = False
        btnHalt.Enabled = False
        btnOpenShutter.Enabled = False
        btnCloseShutter.Enabled = False
        If (IsConnected) Then
            btnHalt.Enabled = True
            btnOpenShutter.Enabled = True
            btnCloseShutter.Enabled = True
            If (Not driver.Slewing) Then
                If (DriverCanSetAzimuth) Then
                    btnGoToAz.Enabled = True
                End If
                If (DriverCanFindHome) Then
                    btnHome.Enabled = True
                End If
                If (DriverCanPark) Then
                    btnPark.Enabled = True
                End If
                If (DriverCanSetPark) Then
                    btnSetPark.Enabled = True
                End If
                If (DriverCanSyncAzimuth) Then
                    btnSync.Enabled = True
                End If
                btnCountBlocks.Enabled = True
                btnSlewWest.Enabled = True
                btnSlewEast.Enabled = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' Gets a value indicating whether this instance is connected.
    ''' </summary>
    ''' <value>
    ''' 
    ''' <c>true</c> if this instance is connected; otherwise, <c>false</c>.
    ''' 
    ''' </value>
    Private ReadOnly Property IsConnected() As Boolean
        Get
            If Me.driver Is Nothing Then Return False
            Return driver.Connected
        End Get
    End Property


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim m_version As Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version
        Dim s_driverInfo As String = "BrewSpin Control " + m_version.Major.ToString() + "." + m_version.Minor.ToString()
        Me.Text = s_driverInfo

        If (My.Settings.DriverId = "") Then
            My.Settings.DriverId = "ASCOM.BrewSpin.Dome"
        End If
        labelDriverId.Text = My.Settings.DriverId
        SetUIState()
        SetStatusInfo()
    End Sub

    Private Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        If (DriverCanFindHome) Then
            Try
                driver.FindHome()
            Catch ex As Exception
                MsgBox("FindHome command failed: " & ex.Message, vbOK, "Error")
                Return
            End Try

            btnHome.BackColor = busyColor
            LogMsg("Starting Home rotation")
            Timer1.Tag = "Home"
            Timer1.Enabled = True
        Else
            MsgBox("Driver does not support FindHome", vbOK, "Error")
        End If
    End Sub

    Private Sub btnPark_Click(sender As Object, e As EventArgs) Handles btnPark.Click
        If (DriverCanPark) Then
            Try
                driver.Park()
            Catch ex As Exception
                MsgBox("Park command failed: " & ex.Message, vbOK, "Error")
                Return
            End Try

            btnPark.BackColor = busyColor
            LogMsg("Starting rotation to Park")
            Timer1.Tag = "Park"
            Timer1.Enabled = True
        Else
            MsgBox("Driver does not support Park", vbOK, "Error")
        End If

    End Sub

    Private Sub btnGoToAz_Click(sender As Object, e As EventArgs) Handles btnGoToAz.Click
        Dim az As Double = -1.0

        If (Not Double.TryParse(txtTargetAz.Text, az)) Then
            MsgBox("Invalid target azimuth value " & txtTargetAz.Text, vbOK, "Error")
            Exit Sub
        End If

        If (DriverCanSetAzimuth) Then
            Try
                driver.SlewToAzimuth(az)
            Catch ex As Exception
                MsgBox("SlewToAzimuth command failed: " & ex.Message, vbOK, "Error")
                Exit Sub
            End Try

            btnGoToAz.BackColor = busyColor
            LogMsg("Starting rotation to Azimuth " & az.ToString())
            Timer1.Tag = "SlewToAzimuth"
            Timer1.Enabled = True
        Else
            MsgBox("Driver does not support SetAzimuth", vbOK, "Error")
        End If

    End Sub

    Private Sub btnHalt_Click(sender As Object, e As EventArgs) Handles btnHalt.Click
        LogMsg("Halting motor")
        Try
            driver.AbortSlew()
        Catch ex As Exception
            MsgBox("Halt command failed: " & ex.Message, vbOK, "Error")
            Return
        End Try
        LogMsg("Halt command completed")
        Timer1.Enabled = False

        ' need to clear busy color
        btnPark.BackColor = buttonChoose.BackColor
        btnHome.BackColor = buttonChoose.BackColor
        btnCountBlocks.BackColor = buttonChoose.BackColor
        btnSlewEast.BackColor = buttonChoose.BackColor
        btnSlewWest.BackColor = buttonChoose.BackColor
        btnGoToAz.BackColor = buttonChoose.BackColor
        btnOpenShutter.BackColor = buttonChoose.BackColor
        btnCloseShutter.BackColor = buttonChoose.BackColor
        btnSlewEast.Text = LBL_SLEW_EAST
        btnSlewWest.Text = LBL_SLEW_WEST
        picSlewing.Visible = False
        UpdateImage(picShutter, "grayLightOff", "Shutter State Unknown")

        SetUIState()
        SetStatusInfo()
    End Sub



    Private Sub btnSetPark_Click(sender As Object, e As EventArgs) Handles btnSetPark.Click
        Try
            driver.SetPark()
        Catch ex As Exception
            MsgBox("SetPark command failed: " & ex.Message, vbOK, "Error")
            Return
        End Try
        LogMsg("SetPark completed ")
        lblParkAz.Text = driver.Action(ACTION_GET_PARAM, "ParkAz")
        SetUIState()
        SetStatusInfo()
    End Sub

    Private Sub btnCountBlocks_Click(sender As Object, e As EventArgs) Handles btnCountBlocks.Click
        Dim s As String = ""
        ' Are we already running? Then do abort
        If (picCountTics.Visible) Then
            btnHalt_Click(sender, e)
            picCountTics.Visible = False
            btnCountBlocks.Text = LBL_COUNT_TICS
            Exit Sub
        End If

        ' Turn on the running image
        picCountTics.Visible = True
        Try
            s = driver.CommandString("COUNTBLOCKS#", True)
        Catch ex As Exception
            MsgBox("CountBlocks command failed: " & ex.Message, vbOK, "Error")
            Return
        End Try

        If (s = "OK#") Then
            LogMsg("CountBlocks has started")
            btnCountBlocks.BackColor = busyColor
            btnCountBlocks.Text = LBL_HALT_COUNT_TICS
            Timer1.Tag = "CountBlocks"
            Timer1.Enabled = True
        Else
            MsgBox("CountBlocks failed with " & s, vbOK, "Error")
        End If
    End Sub

    ' Just puts instructions on how to use the Sync button into the log
    Private Sub btnSyncInstr_Click(sender As Object, e As EventArgs) Handles btnSyncInstr.Click
        LogMsg("=============================================")
        LogMsg("                 Sync Position Instructions")
        LogMsg("")
        LogMsg("1. Connect the telescope to the dome driver BrewSpin (for example, using ACP). Scope must be properly configured with pivot point information.")
        LogMsg("")
        LogMsg("2. Slew telescope to some position. For example, pick a star low in the South, very close to the meridian. After the slew note the target azimuth position; as an example, perhaps the telescope requests a dome azimuth of 112.")
        LogMsg("")
        LogMsg("   ACP shows the target azimuth in the Dome Control popup window. ASCOM Dome Control shows the target in the informational area.")
        LogMsg("")
        LogMsg("   Note that the Dome azimuth is very different from the target star azimuth. The dome azimuth accounts for the odd positions required to get the scope to match up with the dome slit.")
        LogMsg("")
        LogMsg("3. Disconnect the telescope from BrewSpin. Do not home or park the scope. Leave it pointed at the target star.")
        LogMsg("")
        LogMsg("4. Connect BrewSpinControl To the dome driver BrewSpin.")
        LogMsg("")
        LogMsg("5. Adjust the dome until it is aligned with the telescope. ")
        LogMsg("")
        LogMsg("   Use the Slew East/West buttons or the GoTo Azimuth button.")
        LogMsg("")
        LogMsg("   For example, perhaps dome azimuth 125 aligns with the telescope requested azimuth of 112.")
        LogMsg("")
        LogMsg("6. In the box on the left enter the telescope value (112). Click the Sync Position button. The dome positioning is now adjusted to match the telescope position of 112.")
        LogMsg("")
    End Sub

    Private Sub btnSync_Click(sender As Object, e As EventArgs) Handles btnSync.Click
        Dim desiredAz As Double
        If (Double.TryParse(txtDesiredAz.Text, desiredAz)) Then
            Try
                driver.SyncToAzimuth(desiredAz)
            Catch ex As Exception
                MsgBox("SyncToAzimuth command failed: " & ex.Message, vbOK, "Error")
                Return
            End Try
            LogMsg("Sync completed ")
            lblOffsetAz.Text = driver.Action(ACTION_GET_PARAM, "OffsetAz")   ' get driver offset
        Else
            MsgBox("Invalid Desired Azimuth " & txtDesiredAz.Text & " Enter the telescope azimuth value", vbOK, "Error")
        End If


    End Sub

    ' Set up the status information for when the driver is not connected
    Private Sub DefaultStatusInfo()
        lblCurrentAz.Text = "0.0"
        lblBlockPos.Text = ""

        picHome.Image = My.Resources.ResourceManager.GetObject("grayLightOff")
        picPark.Image = My.Resources.ResourceManager.GetObject("grayLightOff")
        picSlewing.Image = My.Resources.ResourceManager.GetObject("grayLightOff")
        picSlewing.Visible = False

        ' Shutter things
        lblShutterStatus.Text = ""
        UpdateImage(picShutter, "grayLightOff", "Shutter State Unknown")
    End Sub

    Private Sub SetStatusInfo()
        If (IsNothing(driver)) Then
            DefaultStatusInfo()
            Exit Sub
        End If
        If (Not driver.Connected) Then
            DefaultStatusInfo()
            Exit Sub
        End If

        Dim az As Double = driver.Azimuth
        lblCurrentAz.Text = Format(az, "0.0")
        lblBlockPos.Text = driver.Action(ACTION_GET_PARAM, "blkPos")
        If (driver.AtHome) Then
            picHome.Image = My.Resources.ResourceManager.GetObject("lightOn")
        Else
            picHome.Image = My.Resources.ResourceManager.GetObject("grayLightOff")
        End If
        If (driver.AtPark) Then
            picPark.Image = My.Resources.ResourceManager.GetObject("lightOn")
        Else
            picPark.Image = My.Resources.ResourceManager.GetObject("grayLightOff")
        End If
        If (driver.Slewing) Then
            picSlewing.Image = My.Resources.ResourceManager.GetObject("Blaulicht3")
            picSlewing.Visible = True
        Else
            picSlewing.Image = My.Resources.ResourceManager.GetObject("grayLightOff")
            picSlewing.Visible = False
        End If

        ' Shutter things
        Dim shutterStatus As String = driver.ShutterStatus.ToString()
        lblShutterStatus.Text = shutterStatus
        Dim shutterStep As String = driver.Action("Dome:GetParam", "OPENCLOSESTEP")
        If (shutterStatus = "shutterOpen") Then
            UpdateImage(picShutter, "Open", "Shutter is Open")
        ElseIf (shutterStatus = "shutterClosed") Then
            UpdateImage(picShutter, "Closed", "Shutter is Closed")
        ElseIf (shutterStatus = "shutterOpening") Then
            UpdateImage(picShutter, "Opening", "Shutter is Opening")
        ElseIf (shutterStatus = "shutterClosing") Then
            UpdateImage(picShutter, "Closing", "Shutter is Closing")
        ElseIf (shutterStatus = "shutterError") Then
            UpdateImage(picShutter, "redLightOn", "Shutter State Unknown")
        End If
        If (shutterStep = "HomeFirst") Then
            UpdateImage(picShutter, "Home Green", "Homing the dome")
        ElseIf (shutterStep = "WaitArdPower") Then
            UpdateImage(picShutter, "quick", "Powering up Shutter")
        ElseIf (shutterStep = "RestoreDome") Then
            UpdateImage(picShutter, "animdome", "Restoring dome position")
        End If

        ' brew buggy SetShutterSwitches()
    End Sub

    Private Sub UpdateImage(pic As PictureBox, name As String, tooltip As String)
        If (pic.Tag <> name) Then
            pic.Image = My.Resources.ResourceManager.GetObject(name)
            pic.Tag = name
            ToolTip1.SetToolTip(pic, tooltip)
        End If
    End Sub

    Private Sub UpdateShutterLights(red As String, green As String, lastKnown As Boolean)
        ' Display the shutter lights.
        ' If Arduino is not communicating, use redLightOff and lightOff for
        ' the lights.
        ' If communicating, use redLightOn and lightOn
        ' lastKnown = false means this is a real check of the switches
        '          true means we are basing the readings on the last known value;
        '               the dome has rotated so we can't communicate with shutter
        Dim lastS As String = ""
        If (lastKnown) Then
            lastS = "[Last Known Value]:"
        End If
        If (gUpperOpenSw) Then
            UpdateImage(picUpperOpenSw, red, lastS & "Upper OpenSwitch On")
        Else
            UpdateImage(picUpperOpenSw, "grayLightOff", lastS & "Upper OpenSwitch Off")
        End If
        If (gUpperClosedSw) Then
            UpdateImage(picUpperClosedSw, green, lastS & "Upper ClosedSwitch On")
        Else
            UpdateImage(picUpperClosedSw, "grayLightOff", lastS & "Upper ClosedSwitch Off")
        End If
        If (gLowerOpenSw) Then
            UpdateImage(picLowerOpenSw, red, lastS & "Lower OpenSwitch On")
        Else
            UpdateImage(picLowerOpenSw, "grayLightOff", lastS & "Lower OpenSwitch Off")
        End If
        If (gLowerClosedSw) Then
            UpdateImage(picLowerClosedSw, green, lastS & "Lower ClosedSwitch On")
        Else
            UpdateImage(picLowerClosedSw, "grayLightOff", lastS & "Lower ClosedSwitch Off")
        End If

    End Sub

    Private Sub SetShutterSwitches()
        'shutterRet is "OK#" or "Status Not Available"
        ' GetParam SHUTTERVALS should return string with pipe separated values
        Dim shutterRet As String = driver.Action("Dome:GetParam", "SHUTTERVALS")
        Dim statPieces() As String
        statPieces = shutterRet.Split("|")
        If (statPieces(0) = "Status") Then
            gUpperOpenSw = Convert.ToBoolean(statPieces(1))
            gUpperClosedSw = Convert.ToBoolean(statPieces(2))
            gLowerOpenSw = Convert.ToBoolean(statPieces(3))
            gLowerClosedSw = Convert.ToBoolean(statPieces(4))
            gUpperCurrent = CDbl(statPieces(5))
            gLowerCurrent = CDbl(statPieces(6))
            gPowerVoltage = CDbl(statPieces(7))
            gStatTemperature = CDbl(statPieces(8))
            UpdateImage(picArduinoAvail, "lightOn", "Arduino Communicating")
            UpdateShutterLights("redLightOn", "lightOn", False)
        Else
            UpdateImage(picArduinoAvail, "redLightOn", "Arduino Not Communicating")
            UpdateShutterLights("redLightOff", "greenLightOff", True)
        End If

        lblPowerVoltage.Text = Format(gPowerVoltage, "0.0")
        lblTemperature.Text = Format(gStatTemperature, "0")
    End Sub

    Private Sub LogMsg(s As String)
        txtLog.AppendText(vbCrLf & s)
    End Sub

    ' Timer to check for completion of various tasks
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        If (Timer1.Tag = "Home") Then
            If (driver.AtHome) Then
                ' we are done
                Timer1.Enabled = False
                btnHome.BackColor = buttonChoose.BackColor
                LogMsg("Home completed")
            End If

        ElseIf (Timer1.Tag = "Park") Then
            If (driver.AtPark) Then
                ' we have arrived
                Timer1.Enabled = False
                btnPark.BackColor = buttonChoose.BackColor
                LogMsg("Park completed")
            End If

        ElseIf (Timer1.Tag = "SlewToAzimuth") Then
            If (Not driver.Slewing) Then
                ' we have arrived
                Timer1.Enabled = False
                btnGoToAz.BackColor = buttonChoose.BackColor
                LogMsg("SlewToAzimuth completed")
            End If

        ElseIf (Timer1.Tag = "CountBlocks") Then
            If (Not driver.Slewing) Then
                ' we have arrived
                Timer1.Enabled = False
                picCountTics.Visible = False
                btnCountBlocks.Text = LBL_COUNT_TICS
                btnCountBlocks.BackColor = buttonChoose.BackColor
                LogMsg("CountBlocks completed. Updating TotalBlocks In driver.")
                Dim s As String = ""
                Try
                    s = driver.Action(ACTION_UPDATE_TOTALBLOCKS, "")
                Catch ex As Exception
                    MsgBox("Action UPDATETOTALBLOCKS failed " & s, vbOK, "Error")
                    Return
                End Try
                ' s now has the count
                LogMsg("CountBlocks completed With " & s)
                lblBlockCount.Text = s
            End If

        ElseIf (Timer1.Tag = "OpenShutter") Then
            If (Not driver.Slewing) Then
                ' we have arrived
                Timer1.Enabled = False
                btnOpenShutter.BackColor = buttonChoose.BackColor
                picShutter.Image = My.Resources.ResourceManager.GetObject("Open")
                LogMsg("Open Shutter completed")
            End If

        ElseIf (Timer1.Tag = "CloseShutter") Then
            If (Not driver.Slewing) Then
                ' we have arrived
                Timer1.Enabled = False
                btnCloseShutter.BackColor = buttonChoose.BackColor
                picShutter.Image = My.Resources.ResourceManager.GetObject("Closed")
                LogMsg("Close Shutter completed")
            End If

        End If

        SetStatusInfo()
    End Sub

    Private Sub test2()

    End Sub

    Private Sub btnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click
        If (CheckShutterTimer.Enabled) Then
            CheckShutterTimer.Enabled = False
        Else
            CheckShutterTimer.Enabled = True
        End If
    End Sub

    Private Sub btnSlewEast_Click(sender As Object, e As EventArgs) Handles btnSlewEast.Click
        If (Timer1.Enabled) Then
            ' already slewing, so issue abort
            btnHalt_Click(sender, e)
            btnSlewEast.Text = LBL_SLEW_EAST
            Exit Sub
        End If

        If (DriverCanSetAzimuth) Then
            Try
                driver.SlewToAzimuth(-9999)
            Catch ex As Exception
                MsgBox("SlewToAzimuth command failed: " & ex.Message, vbOK, "Error")
                Exit Sub
            End Try
        Else
            MsgBox("Driver does not support SetAzimuth", vbOK, "Error")
            Exit Sub
        End If

        btnSlewEast.BackColor = busyColor
        btnSlewEast.Text = LBL_HALT_SLEW
        LogMsg("Slewing East")
        Timer1.Tag = "SlewEast"
        Timer1.Enabled = True      ' need to set timer so Status info gets updated
    End Sub

    Private Sub btnSlewWest_Click(sender As Object, e As EventArgs) Handles btnSlewWest.Click
        If (Timer1.Enabled) Then
            ' already slewing, so issue abort
            btnHalt_Click(sender, e)
            btnSlewWest.Text = LBL_SLEW_WEST
            Exit Sub
        End If

        If (DriverCanSetAzimuth) Then
            Try
                driver.SlewToAzimuth(9999)
            Catch ex As Exception
                MsgBox("SlewToAzimuth command failed: " & ex.Message, vbOK, "Error")
                Exit Sub
            End Try
        Else
            MsgBox("Driver does not support SetAzimuth", vbOK, "Error")
            Exit Sub
        End If

        btnSlewWest.BackColor = busyColor
        btnSlewWest.Text = LBL_HALT_SLEW
        LogMsg("Slewing West")
        Timer1.Tag = "SlewWest"
        Timer1.Enabled = True      ' need to set timer so Status info gets updated

    End Sub

    Private Sub btnOpenShutter_Click(sender As Object, e As EventArgs) Handles btnOpenShutter.Click
        LogMsg("Start Opening Shutter")
        driver.OpenShutter()            ' start opening
        Timer1.Tag = "OpenShutter"
        Timer1.Enabled = True      ' need to set timer so Status info gets updated
        picShutter.Image = My.Resources.ResourceManager.GetObject("Opening")
        btnOpenShutter.BackColor = busyColor
    End Sub

    Private Sub btnCloseShutter_Click(sender As Object, e As EventArgs) Handles btnCloseShutter.Click
        LogMsg("Start Closing Shutter")
        driver.CloseShutter()
        Timer1.Tag = "CloseShutter"
        Timer1.Enabled = True      ' need to set timer so Status info gets updated
        picShutter.Image = My.Resources.ResourceManager.GetObject("Closing")
        btnCloseShutter.BackColor = busyColor
    End Sub

    ' Checking shutter status info every 4 seconds
    Private Sub CheckShutterTimer_Tick(sender As Object, e As EventArgs) Handles CheckShutterTimer.Tick
        'Try
        '    h = driver.AtHome
        'Catch ex As Exception
        '    LogMsg("AtHome failed: " & ex.Message)
        'End Try
        SetStatusInfo()
    End Sub
End Class
