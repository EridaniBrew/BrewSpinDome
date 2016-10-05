' Logic to control the shutter.
' Low level communications are handled by ArduinoComm

Imports ASCOM
Imports ASCOM.DeviceInterface
Imports ASCOM.Utilities

Imports System.Timers
Imports System.Threading        ' to get Sleep function?



Public Class CShutterControl

    Private gMainDriver As ASCOM.DeviceInterface.IDomeV2
    Private gDriverID As String = ""
    Private gArduinoComm As ArduinoComm         ' Shutter Communications
    Private gTL As TraceLogger
    Private gLockObject As Object    ' syncing CommandString

    Friend Shared IPAddressProfileName As String = "IP Address" 'Constants used for Profile persistence
    Friend Shared listenPortProfileName As String = "Listen Port" 'Constants used for Profile persistence
    Friend Shared MotorTimerMsecProfileName As String = "MotorTimerMsec"
    Friend Shared VentTimeProfileName As String = "VentTime"
    Friend Shared UpperShutterTimeoutProfileName As String = "UpperShutterTimeout"
    Friend Shared LowerShutterTimeoutProfileName As String = "LowerShutterTimeout"
    Friend Shared UpperMotorSpeedProfileName As String = "UpperMotorSpeed"
    Friend Shared LowerMotorSpeedProfileName As String = "LowerMotorSpeed"
    Friend Shared ReverseUpperMotorProfileName As String = "ReverseUpperMotor"
    Friend Shared ReverseLowerMotorProfileName As String = "ReverseLowerMotor"

    Friend Shared IPAddressDefault As String = "192.168.1.30"
    Friend Shared listenPortDefault As String = "1552"
    Friend Shared MotorTimerMsecDefault As String = "500"
    Friend Shared VentTimeDefault As String = "3000"
    Friend Shared UpperShutterTimeoutDefault As String = "45000"
    Friend Shared LowerShutterTimeoutDefault As String = "45000"
    Friend Shared UpperMotorSpeedDefault As String = "1023"
    Friend Shared LowerMotorSpeedDefault As String = "1023"
    Friend Shared ReverseUpperMotorDefault As String = "False"
    Friend Shared ReverseLowerMotorDefault As String = "False"

    Friend Shared IPAddress As String ' Variables to hold the currrent device configuration
    Friend Shared listenPort As Long = 0
    Friend Shared MotorTimerMsec As Long = 0
    Friend Shared VentTime As Long = 0
    Friend Shared UpperShutterTimeout As Long = 0
    Friend Shared LowerShutterTimeout As Long = 0
    Friend Shared UpperMotorSpeed As Long = 0
    Friend Shared LowerMotorSpeed As Long = 0
    Friend Shared ReverseUpperMotor As Boolean = False
    Friend Shared ReverseLowerMotor As Boolean = False
    Private Const SHUTTER_POWERUP_SECS = 18     ' 20 seconds for powerup after home

    Public Const DOME_SUCCESS = "OK#"

    ' my driver globals           ===========================================================
    Private motorTimer As Timers.Timer                  ' timer to check when an active operation (opening, closing) is occuring
    Private gArduinoAvailable As Boolean = False           ' True - Arduino answered the ping
    Private gActiveOperation As String = ""             ' holds one of the above operations (ArduinoComm.ACTION_OPEN_BOTH_SHUTTER) for the motorTimer to know what to do
    Private gMotorTimeout As Long = 0                   ' How long to wait for return?
    Private gMotorOrigTimeout As Long = 0               ' Holds the original motorTimeout. gMotorTimeout ticks down

    ' values used during the Open or Close shutter operations
    Private gHoldAzimuth As Double = 0                      ' holds Dome position before Open or Close Shutter
    Private Const PARK_AZ_DELTA As Double = 3.0             ' if Azimuth this close to park, do not restore dome
    '                                                       ' position after open/close
    Private gOpenCloseStep As String = ""                   ' Holds the current step for opening/Closing shutters
    Private Const PHASE_HOMEFIRST = "HomeFirst"
    Private Const PHASE_WAIT_ARD_POWER = "WaitArdPower"
    Private Const PHASE_SHUTTER_MOVE = "ShutterMove"
    Private Const PHASE_RESTORE_DOME = "RestoreDome"

    ' This is a kludgy way to get a message back to the client program when the shutter
    ' open or close fails. When the shutterStatus goes to shutter error, this message is set.
    ' The message is retrieved with a special CommandString of "ShutterErrorMsg"
    Private gDomeShutterErrorMsg As String = ""

    ' Tracking how long the dome has been in the Parked position.
    ' The idea is: when an open/close is requested the dome must be parked, then wait 20 seconds to allow the arduino to
    ' to power up and get ready to receive commands. 
    ' I would like to skip  waiting the 20 seconds if the dome has already been homed long enough
    Private gHomedTimeValid As Boolean = False            ' indicates the home time stamp is appropriate
    Private gHomedTime As Date                     ' time of last home.
    Private gHomedAz As Double = 0                 ' Azimuth of Homed position

    ' Status information from Shutter Arduino
    Public gShutterDetail As CShutterDetails

    Private gDomeShutterState As ShutterState = ShutterState.shutterError    ' Variable to hold the open/closed status of the shutter

    Public Sub New(mainDriver As ASCOM.DeviceInterface.IDomeV2, driverID As String, tl As TraceLogger, lockobj As Object)
        gMainDriver = mainDriver
        gDriverID = driverID
        gTL = tl
        gLockObject = lockobj
        ReadProfile()

        motorTimer = New Timers.Timer
        motorTimer.Enabled = False
        motorTimer.Interval = MotorTimerMsec
        AddHandler motorTimer.Elapsed, New System.Timers.ElapsedEventHandler(AddressOf motorTimerCallback)

        gShutterDetail = New CShutterDetails

        'gArdComm = New ArduinoComm(IPAddress, listenPort, tl, gLockObject)
    End Sub

    Public Function AbortSlew() As String
        Dim errStr As String = ""
        Dim retS As String = ""

        ' Now Abort Shutter if it is running
        ' Is Arduino connected?
        ' gArduinoAvailable is true only when an open or close
        ' command is active. In this case we need to send the Abort command
        If (gArduinoAvailable And (motorTimer.Enabled)) Then
            retS = ShutterCommandString(ArduinoComm.ACTION_ABORT_MOTORS)     ' CommandString("A")
            If (retS <> DOME_SUCCESS) Then
                errStr = "AbortSlew failed: " & retS
            End If
            ' try and use the last status results to determine shutter state
            RetrieveStatus()
            If (gShutterDetail.UpperClosedSw And gShutterDetail.LowerClosedSw) Then
                gDomeShutterState = ShutterState.shutterClosed
            ElseIf (gShutterDetail.UpperOpenSw And gShutterDetail.LowerOpenSw) Then
                gDomeShutterState = ShutterState.shutterOpen
            Else
                gDomeShutterState = ShutterState.shutterError
            End If

            gOpenCloseStep = ""
            motorTimer.Enabled = False
            gMotorTimeout = 0
            gMotorOrigTimeout = 0
        Else
            errStr = "AbortSlew: Shutter not active"
        End If

        Return errStr
    End Function

    Public Function GetShutterState() As ShutterState
        Return gDomeShutterState
    End Function

    Public Function GetShutterVals() As String
        Dim s As String = "Status Not Available"

        ' check we are at home
        Dim ready As Boolean = False
        Try
            ready = ArduinoIsReady("GetShutterVals", False)
        Catch ex As Exception
            ' This exception occurs if the connection process fails (after waiting for power)
            gTL.LogMessage("Shutter:GetShutterVals:  ", "could not connect to Arduino, use last status")
            DisconnectFromArduino()
            Return s
        End Try
        If (gArduinoAvailable) Then
            RetrieveStatus()
            s = gShutterDetail.BuildStatusString()
            gTL.LogMessage("Shutter:GetShutterVals:  ", "ShutterArduino status is " & s)
        End If

        Return s
    End Function

    Public Function GetOpenCloseStep() As String
        Return gOpenCloseStep
    End Function

    ' True if shutter is in process of opening or closing (Shutter is moving)
    Public Function ShutterSlewing() As Boolean
        Dim mySlewing As Boolean = False
        If ((gDomeShutterState = ShutterState.shutterOpening) Or
            (gDomeShutterState = ShutterState.shutterClosing)) Then
            mySlewing = True
        End If
        Return mySlewing
    End Function

    ' Returns string for TL logging
    Public Function CloseShutter() As String
        Dim retS As String = ""
        ' If already closed, we are done
        ' Start the multi-step process handled in MotorTimer_tick
        ' first, park dome

        If (gDomeShutterState = ShutterState.shutterClosing) Then
            Return "Shutter already Closing"
        End If
        If (gDomeShutterState = ShutterState.shutterClosed) Then
            Return "Shutter already Closed"
        End If

        ' Start park operation 
        StartHome(ArduinoComm.ACTION_CLOSE_BOTH_SHUTTER)
        Return ("Closing shutter process started")
    End Function

    ' Returns string for TL logging
    Public Function OpenShutter() As String
        Dim retS As String = ""
        ' If already open, we are done; could be partly open, but assuming no Venting happening
        ' If already opening, we are done.
        ' We need to do this operation in several steps. Homing is first; other steps
        ' are done in motorTimer callback
        '   1) Save current Az val, Start Homing the dome. Set status values
        '      indicating open is in progress.
        '   2) Wait 20 sec for power up of Arduino
        '   3) Open Shutters
        '   4) Move back to original Azimuth
        '   5) set status to show complete

        If (gDomeShutterState = ShutterState.shutterOpening) Then
            Return "Shutter already opening"
        End If
        If (gDomeShutterState = ShutterState.shutterOpen) Then
            Return "Shutter already open"
        End If

        ' Start open shutter operation 
        StartHome(ArduinoComm.ACTION_OPEN_BOTH_SHUTTER)
        Return ("Opening shutter process started")
    End Function


    ' When dome is AtHome, start the clock.
    ' This allows the Open/Close operation to skip the waiting for PowerUp if the time has
    ' already elapsed on the clock
    Public Sub StartHomeClock(startStop As Boolean)
        If startStop Then
            ' start the clock unless already running
            If (Not gHomedTimeValid) Then
                gHomedTime = Now()
                gHomedTimeValid = True
                gHomedAz = gMainDriver.Azimuth
            End If
            SneakPeek()
        Else
            'clear the clock
            gHomedTimeValid = False
        End If
    End Sub

    Private Function ShutterCommandString(ByVal Command As String) As String
        Dim retS As String = "xx"

        'CheckConnected("ShutterCommandString")                 ' verify that we are connected
        ' it's a good idea to put all the low level communication with the device here,
        ' then all communication calls this function
        ' you need something to ensure that only one command is in progress at a time
        ' Temp - return miniStep status as special command
        If (Command = "miniStatus") Then
            retS = gOpenCloseStep
        ElseIf (Command = "ShutterErrorMsg") Then
            retS = gDomeShutterErrorMsg
        ElseIf (Command = "ParkTime") Then
            Dim s As String
            If (gHomedTimeValid) Then
                Dim secs As Long
                secs = DateDiff(DateInterval.Second, gHomedTime, Now())
                s = gHomedTimeValid.ToString() + " " & secs.ToString()
            Else
                s = gHomedTimeValid.ToString()
            End If
            retS = s
        Else
            retS = gArduinoComm.RequestAction(Command)
        End If


        Return retS
    End Function


    ' When the time goes off, check whether any current motor movements have completed
    Private Sub motorTimerCallback(ByVal sender As Object, ByVal e As ElapsedEventArgs)
        Dim retS As String = ""
        motorTimer.Enabled = False              ' avoid multiple interrupts

        gMotorTimeout -= motorTimer.Interval
        If (gMotorTimeout <= 0) Then
            ' operation has timed out
            'SetShutterError("Timeout for operation " & gActiveOperation)
            gTL.LogMessage("motorTimer Timeout", "!!Timeout of " & gMotorOrigTimeout & " for operation " & gActiveOperation)
            AbortSlew()
            Exit Sub
        End If

        Dim contMotor As Boolean = MovementProgress(gActiveOperation)
        If (contMotor) Then
            motorTimer.Enabled = True
        End If
    End Sub

    Private Function MovementProgress(cmd As String) As Boolean
        ' Checking the various phases of opening the shutter
        '  1. Home dome
        '  2. Wait for Arduino to power up
        '  3. Connect to Arduino/ send Open command
        '  4. Restore original Dome position
        '  5. Restore Slaved setting, disconnect Arduino

        Dim contMotor As Boolean = False         ' should we re-enable the motor timeout?
        Dim opening As String
        Dim finalState As ShutterState
        If (cmd = ArduinoComm.ACTION_OPEN_BOTH_SHUTTER) Then
            ' set up for opening shutter
            opening = "opening"
            finalState = ShutterState.shutterOpen
        ElseIf (cmd = ArduinoComm.ACTION_CLOSE_BOTH_SHUTTER) Then
            ' set up for closing shutter
            opening = "closing"
            finalState = ShutterState.shutterClosed
        Else
            opening = "Unknown"
            Throw New Exception("MovementProgress: Unrecognized command " & cmd)
        End If

        If (gOpenCloseStep = PHASE_HOMEFIRST) Then
            contMotor = True
            If (gMainDriver.AtHome) Then
                gTL.LogMessage("Shutter:MovementProgress:  ", "Dome homed, wait for Arduino to power up")
                gOpenCloseStep = PHASE_WAIT_ARD_POWER
                gMotorTimeout = 20000 + 10000
                gMotorOrigTimeout = gMotorTimeout
            End If

        ElseIf (gOpenCloseStep = PHASE_WAIT_ARD_POWER) Then
            contMotor = True
            gTL.LogMessage("Shutter:MovementProgress:  ", "Check ArduinoReady")
            Dim ready As Boolean = False
            Try
                ready = ArduinoIsReady(cmd, False)
            Catch ex As Exception
                ' This exception occurs if the connection process fails (after waiting for power)
                gTL.LogMessage("Shutter:MovementProgress:  ", "!!could not connect to Arduino")
                SetShutterError("Shutter " & opening & " Could not connect to Arduino")
                DisconnectFromArduino()
                gOpenCloseStep = ""
                gDomeShutterState = ShutterState.shutterError
                contMotor = False
                Return contMotor
            End Try
            If (gArduinoAvailable) Then
                gTL.LogMessage("Shutter:MovementProgress:  ", "ShutterArduino powered up and connected")
                gOpenCloseStep = PHASE_SHUTTER_MOVE
                gTL.LogMessage("Shutter:MovementProgress:  ", "ShutterArduino start " & opening & " shutters")
                ' Move the shutters
                Dim retS As String = ShutterCommandString(cmd)
                If (retS = ArduinoComm.DRIVER_SUCCESS) Then
                    StartMotorTimer(cmd, UpperShutterTimeout + LowerShutterTimeout)
                    gTL.LogMessage("Shutter:MovementProgress:  ", "   Timeout is " & UpperShutterTimeout + LowerShutterTimeout)
                Else
                    SetShutterError(opening & " cmd failed: " & retS)
                    gArduinoAvailable = False
                    gTL.LogMessage("Shutter:MovementProgress:  ", opening & " cmd failed: " & retS)
                    DisconnectFromArduino()
                    gOpenCloseStep = ""
                    gDomeShutterState = ShutterState.shutterError
                    contMotor = False
                End If
            Else
                gTL.LogMessage("Shutter:MovementProgress:  ", "Shutter Arduino not ready")
            End If

        ElseIf (gOpenCloseStep = PHASE_SHUTTER_MOVE) Then
            contMotor = True
            RetrieveStatus()
            If (((cmd = ArduinoComm.ACTION_OPEN_BOTH_SHUTTER) And (gShutterDetail.UpperOpenSw) And (gShutterDetail.LowerOpenSw)) Or
                ((cmd = ArduinoComm.ACTION_CLOSE_BOTH_SHUTTER) And (gShutterDetail.UpperClosedSw) And (gShutterDetail.LowerClosedSw))) Then
                'gTL.LogMessage("Shutter:MovementProgress:  ", opening & "Shutters complete. ")
                ' commented this section out - do not restore dome position
                ' after open/close completes
                'If (gMainDriver.CanSetAzimuth And (Math.Abs(gHoldAzimuth - gMainDriver.Azimuth) > PARK_AZ_DELTA)) Then
                '    gMainDriver.SlewToAzimuth(gHoldAzimuth)
                '    gOpenCloseStep = PHASE_RESTORE_DOME
                '    gMotorTimeout = 90000
                '    gMotorOrigTimeout = gMotorTimeout
                '    gTL.LogMessage("Shutter:MovementProgress: ", "Restoring dome azimuth")
                'Else
                ' already in restore dome position, we are done
                gDomeShutterState = finalState
                gTL.LogMessage("Shutter:MovementProgress: ", "Shutter " & opening & " complete, not restoring dome azimuth ")
                gOpenCloseStep = ""
                DisconnectFromArduino()
                contMotor = False
                'End If
            Else
                gTL.LogMessage("Shutter:MovementProgress: ", "Shutter still moving")
            End If

        ElseIf (gOpenCloseStep = PHASE_RESTORE_DOME) Then
            ' For now, this should not be happening
            contMotor = True
            If (Math.Abs(gMainDriver.Azimuth - gHoldAzimuth) <= 2) Then
                gDomeShutterState = finalState
                gTL.LogMessage("Shutter:MovementProgress: ", "Shutter " & opening & " complete, dome azimuth restored")
                DisconnectFromArduino()
                gOpenCloseStep = ""
                contMotor = False
            End If
        End If

        Return contMotor
    End Function

    Private Sub StartHome(op As String)
        ' op is ArduinoComm.ACTION_CLOSE_BOTH_SHUTTER or OPEN_BOTH
        ' Start the Home required before connecting to the Arduino
        ' Save current rotation settings so we can restore the dome after
        ' open/close
        gHoldAzimuth = gMainDriver.Azimuth

        gOpenCloseStep = PHASE_HOMEFIRST
        If (op = ArduinoComm.ACTION_OPEN_BOTH_SHUTTER) Then
            gDomeShutterState = ShutterState.shutterOpening
        Else
            gDomeShutterState = ShutterState.shutterClosing
        End If

        'TL.LogMessage("StartHome", "Homing before connecting to Arduino")
        If (Not gMainDriver.AtHome) Then
            gMainDriver.FindHome()
        End If
        StartMotorTimer(op, 90000)

    End Sub

    Private Sub StartMotorTimer(cmd As String, timeout As Long)
        ' Start the motor Timer to check for end of the command
        'TL.LogMessage("StartMotorTimer", "start timer for " & cmd)
        gMotorTimeout = timeout
        gMotorOrigTimeout = gMotorTimeout
        gActiveOperation = cmd
        motorTimer.Enabled = True
    End Sub

    Private Function ArduinoIsReady(cmd As String, skipHomeCheck As Boolean) As Boolean
        ' Checking to see if Arduino is ready for operation
        ' Criteria:
        '   Dome is at Home
        '   Home time has passed the powerup time needed for Arduino to be ready
        ' If already connected, return true
        ' else connect to Arduino.
        '
        ' skipHomeCheck = true => we already know we are at home, don't check
        ' returns true if Arduino is connected
        '          false if no success
        ' throws exception if connection failed

        Dim ready As Boolean = False
        If (Not skipHomeCheck) Then
            If (Not gMainDriver.AtHome) Then
                gTL.LogMessage("    ShutterControl.ArduinoIsReady", "Not at home")
                Return False      ' not at home
            End If
        End If

        ' this was to allow variable amounts of time
        'gArdPowerUpTime = gArdPowerUpTime - motorTimer.Interval
        'If (gArdPowerUpTime <= 0) Then
        If (gHomedTimeValid And (DateDiff(DateInterval.Second, gHomedTime, Now()) > SHUTTER_POWERUP_SECS)) Then
            ConnectToArduino(cmd)
            If (gArduinoAvailable) Then
                ready = True
                gTL.LogMessage("    ShutterControl.ArduinoIsReady", "Shutter Arduino connection succeeded")
            Else
                ready = False    ' could not connect
                gTL.LogMessage("    ShutterControl.ArduinoIsReady", "Shutter Arduino connection failed")
                Throw New Exception("Connection to Shutter Arduino failed")
            End If
        Else
            ready = False      ' not enough time to power up
            gTL.LogMessage("    ShutterControl.ArduinoIsReady", "Still powering up")
        End If

        Return ready
    End Function

    Private Sub ConnectToArduino(cmd As String)
        ' cmd is ArduinoComm.ACTION_OPEN_BOTH_SHUTTER or CLOSE
        ' At this point we should be Homed, and the Arduino has had time to
        ' to start up
        ' Save/set status info

        'gActiveOperation = cmd
        If (cmd = ArduinoComm.ACTION_OPEN_BOTH_SHUTTER) Then
            gDomeShutterState = ShutterState.shutterOpening
        ElseIf (cmd = ArduinoComm.ACTION_CLOSE_BOTH_SHUTTER) Then
            gDomeShutterState = ShutterState.shutterClosing
        End If

        If (IsNothing(gArduinoComm)) Then
            gTL.LogMessage("    ShutterControl.ConnectToArduino", "Allocating ArduinoComm")
            gArduinoComm = New ArduinoComm(IPAddress, listenPort, gTL, gLockObject)
        End If

        If ((gArduinoComm.CommStatus = ArduinoComm.COMM_FAILED) Or
            (gArduinoComm.CommStatus = ArduinoComm.COMM_NONE)) Then
            gTL.LogMessage("    ShutterControl.ConnectToArduino", "Connect to Arduino")
            gArduinoComm.ConnectArduino()
            If (gArduinoComm.CommStatus = ArduinoComm.COMM_CONNECTED) Then
                ' ReconfigureArduino()
                gArduinoAvailable = True
            Else
                gArduinoAvailable = False
            End If
            gTL.LogMessage("    ShutterControl.ConnectToArduino", "Connect result is " & gArduinoAvailable.ToString())

        End If

    End Sub

    Private Sub DisconnectFromArduino()
        gArduinoAvailable = False
        If (Not IsNothing(gArduinoComm)) Then
            'TL.LogMessage("DisconnectFromArduino", "Disconnecting Arduino ")
            gArduinoComm.DisconnectArduino()
            gArduinoComm = Nothing
        End If

    End Sub

    Private Sub ReconfigureArduino()
        ' Send commands to Arduino to set the various operating parameters: motor speed, direction, etc
        Dim ret As String
        Dim cmd As String

        ' For now, take out reconfigure
        Return

        ' Update Vent Time
        cmd = ArduinoComm.ACTION_RECONFIG_PARAM & ", " & "VentTime" & ", " & VentTime.ToString() & ", "
        ret = ShutterCommandString(cmd)
        If (ret <> ArduinoComm.DRIVER_SUCCESS) Then
            Throw New ASCOM.DriverException("Failed to reconfigure Vent Time with {" & cmd & "}" & " return:  " & ret)
        End If

        ' Update Reverse Upper Motor
        cmd = ArduinoComm.ACTION_RECONFIG_PARAM & "," & "ReverseUpper" & "," & ReverseUpperMotor.ToString() & ","
        ret = ShutterCommandString(cmd)
        If (ret <> ArduinoComm.DRIVER_SUCCESS) Then
            Throw New ASCOM.DriverException("Failed to reconfigure ReverseUpper with {" & cmd & "}" & " return: " & ret)
        End If

        ' Update Reverse Lower Motor
        cmd = ArduinoComm.ACTION_RECONFIG_PARAM & "," & "ReverseLower" & "," & ReverseLowerMotor.ToString() & ","
        ret = ShutterCommandString(cmd)
        If (ret <> ArduinoComm.DRIVER_SUCCESS) Then
            Throw New ASCOM.DriverException("Failed to reconfigure Reverse Lower with {" & cmd & "}" & " return: " & ret)
        End If

        ' Update Upper Motor Speed
        'cmd = ArduinoComm.ACTION_RECONFIG_PARAM & "," & "UpperSpeed" & "," & UpperMotorSpeed.ToString() & ","
        'ret = ShutterCommandString(cmd)
        'If (ret <> ArduinoComm.DRIVER_SUCCESS) Then
        'Throw New ASCOM.DriverException("Failed to reconfigure Upper Motor Speed with {" & cmd & "}" & " return: " & ret)
        'End If

        ' Update Lower Motor Speed
        'cmd = ArduinoComm.ACTION_RECONFIG_PARAM & "," & "LowerSpeed" & "," & LowerMotorSpeed.ToString() & ","
        'ret = ShutterCommandString(cmd)
        'If (ret <> ArduinoComm.DRIVER_SUCCESS) Then
        'Throw New ASCOM.DriverException("Failed to reconfigure Lower Motor Speed with {" & cmd & "}" & " return: " & ret)
        'End If


        ' Change the Timer values
        motorTimer.Interval = MotorTimerMsec

    End Sub


    Private Sub RetrieveStatus()
        ' Retrieve status info from the Arduino. 
        ' Issue status command, retrieve string, parse the string
        Dim statStr As String

        statStr = gArduinoComm.RequestAction(ArduinoComm.ACTION_GET_SHUTTER_STATUS)
        If (gShutterDetail.ParseStatusString(statStr)) Then
            gArduinoAvailable = True
            gArduinoComm.SetOperationPending(gShutterDetail.OpInProgress)
        Else
            'bad status string 
            gArduinoAvailable = False
        End If
        Return
    End Sub


    Private Sub ReadProfile()
        Dim driverID As String = gDriverID

        Using driverProfile As New Profile()
            driverProfile.DeviceType = "Dome"
            IPAddress = driverProfile.GetValue(driverID, IPAddressProfileName, String.Empty, IPAddressDefault)
            listenPort = driverProfile.GetValue(driverID, listenPortProfileName, String.Empty, listenPortDefault)
            MotorTimerMsec = driverProfile.GetValue(driverID, MotorTimerMsecProfileName, String.Empty, MotorTimerMsecDefault)
            VentTime = driverProfile.GetValue(driverID, VentTimeProfileName, String.Empty, VentTimeDefault)
            LowerShutterTimeout = driverProfile.GetValue(driverID, LowerShutterTimeoutProfileName, String.Empty, LowerShutterTimeoutDefault)
            UpperShutterTimeout = driverProfile.GetValue(driverID, UpperShutterTimeoutProfileName, String.Empty, UpperShutterTimeoutDefault)
            UpperMotorSpeed = driverProfile.GetValue(driverID, UpperMotorSpeedProfileName, String.Empty, UpperMotorSpeedDefault)
            LowerMotorSpeed = driverProfile.GetValue(driverID, LowerMotorSpeedProfileName, String.Empty, LowerMotorSpeedDefault)
            ReverseUpperMotor = Convert.ToBoolean(driverProfile.GetValue(driverID, ReverseUpperMotorProfileName, String.Empty, ReverseUpperMotorDefault))
            ReverseLowerMotor = Convert.ToBoolean(driverProfile.GetValue(driverID, ReverseLowerMotorProfileName, String.Empty, ReverseLowerMotorDefault))
        End Using
    End Sub

    Public Sub WriteProfile()
        Dim driverID As String = gDriverID

        Using driverProfile As New Profile()
            driverProfile.WriteValue(driverID, IPAddressProfileName, IPAddress.ToString())
            driverProfile.WriteValue(driverID, listenPortProfileName, listenPort.ToString())
            driverProfile.WriteValue(driverID, MotorTimerMsecProfileName, MotorTimerMsec.ToString())
            driverProfile.WriteValue(driverID, VentTimeProfileName, VentTime.ToString())
            driverProfile.WriteValue(driverID, UpperShutterTimeoutProfileName, UpperShutterTimeout.ToString())
            driverProfile.WriteValue(driverID, LowerShutterTimeoutProfileName, LowerShutterTimeout.ToString())
            'driverProfile.WriteValue(driverID, UpperMotorSpeedProfileName, UpperMotorSpeed.ToString())
            'driverProfile.WriteValue(driverID, LowerMotorSpeedProfileName, LowerMotorSpeed.ToString())
            driverProfile.WriteValue(driverID, ReverseUpperMotorProfileName, ReverseUpperMotor.ToString())
            driverProfile.WriteValue(driverID, ReverseLowerMotorProfileName, ReverseLowerMotor.ToString())

        End Using
    End Sub

    Private Sub SetShutterError(msg As String)
        gDomeShutterState = ShutterState.shutterError
        gDomeShutterErrorMsg = msg
    End Sub

    Private Sub SneakPeek()
        ' We are at home - sneak a peak at the shutter status
        ' so we can initialize the ShutterState from shutterError
        If (gDomeShutterState = ShutterState.shutterError) Then
            Dim ready As Boolean = False
            Try
                ready = ArduinoIsReady("SneakPeek", True)
            Catch ex As Exception
                ' This exception occurs if the connection process fails (after waiting for power)
                gTL.LogMessage("Shutter:SneakPeek:  ", "could not connect to Arduino")
                DisconnectFromArduino()
                Return
            End Try
            If (gArduinoAvailable) Then
                RetrieveStatus()
                If (gShutterDetail.UpperClosedSw And gShutterDetail.LowerClosedSw) Then
                    gDomeShutterState = ShutterState.shutterClosed
                ElseIf (gShutterDetail.UpperOpenSw And gShutterDetail.LowerOpenSw) Then
                    gDomeShutterState = ShutterState.shutterOpen
                End If
                DisconnectFromArduino()
            End If
        End If
    End Sub
End Class






' This class holds the shutter status information: switch states,
' voltage, temperature, etc
Public Class CShutterDetails
    ' Shutter switches. True is Closed, False is open
    Public UpperOpenSw As Boolean = False
    Public UpperClosedSw As Boolean = False
    Public LowerOpenSw As Boolean = False
    Public LowerClosedSw As Boolean = False
    Public Voltage As Double = 0.0
    Public UpperCurrent As Double = 0.0
    Public LowerCurrent As Double = 0.0
    Public Temperature As Double = 0.0
    Public OpInProgress As Boolean = False

    Public Sub New()


    End Sub

    Public Function ParseStatusString(s As String) As Boolean
        ' return true if string parses OK
        ' return false if problem
        Dim retCode As Boolean = False

        ' statStr should look like 
        ' "Status,%c,%c,%c,%c,%5d,%5d,%5d,%5d,%c", upperOpen, upperClosed, lowerOpen, lowerClosed, rawUpCurrent, rawLowCurrent, rawVoltage, rawTemperature, OpInProgress
        Dim statPieces As String() = s.Split(New Char() {","c})
        If (statPieces(0) = "Status") Then
            UpperOpenSw = (statPieces(1) = "C")
            UpperClosedSw = (statPieces(2) = "C")
            LowerOpenSw = (statPieces(3) = "C")
            LowerClosedSw = (statPieces(4) = "C")
            UpperCurrent = CDbl(statPieces(5)) / 30.72                     ' Found article indicating that current may be count / 30.72
            LowerCurrent = CDbl(statPieces(6)) / 30.72
            Voltage = CDbl(statPieces(7)) * (0.0163) - 0.4217          ' measured 4 voltages and corresponding counts
            ' Voltage divider is 10K and 22K
            Temperature = ((CDbl(statPieces(8)) * 4.75 / 1024 - 0.5) * 100) * 9 / 5 + 32    ' Vcc ,easured 4.75V
            OpInProgress = (statPieces(7) = "T")
            retCode = True
        End If

        Return retCode
    End Function

    Public Function BuildStatusString() As String
        ' build a string with the values in it, separated by pipe
        Dim s As String = ""
        s = "Status|" & UpperOpenSw.ToString() & "|" & UpperClosedSw.ToString() & "|" &
            LowerOpenSw.ToString() & "|" & LowerClosedSw.ToString() & "|" &
            Format(UpperCurrent, "0.0") & "|" & Format(LowerCurrent, "0.0") & "|" &
            Format(Voltage, "0.0") & "|" & Format(Temperature, "0") & "|" &
            OpInProgress.ToString()
        Return s
    End Function
End Class