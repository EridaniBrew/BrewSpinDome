'tabs=4
' --------------------------------------------------------------------------------
'
' ASCOM Dome driver for BrewSpin
'
' Description:	Driver for my Arduino based dome driver. 
'				This driver uses the Arduino to control Foster Systems hardware
'				to do Dome rotation. My dome is an Exploradome I 8 foot dome.
'
' Implements:	ASCOM Dome interface version: 1.0
' Author:		(Brew) Robert Brewington <eridanibrew@gmail.com>
'
' Edit Log:
'
' Date			Who	Vers	Description
' -----------	---	-----	-------------------------------------------------------
' dd-mmm-yyyy	XXX	6.0.0	Initial edit, from Dome template
' 28-Apr-2016   brew 6.0.1  Rework code for better logging information
'                           "Internal" functions to distinguish external calls from clients
'                           Use GetPos malformed string - Microsoft seems to drop the occasional character
' 16-Sep-2016   brew 6.0.2  Change how Arduino is connected. Add GET_PARAM option to retrieve Arduino
'                           Status string with switch values, voltage, etc
' 02-Oct-2016   brew 6.1.0  Added GETVALS to allow retrieval of status. Not working well yet.
'                           Removed Ping, back to 20 sec Home Timer.
' ---------------------------------------------------------------------------------
'
'
' Your driver's ID is ASCOM.BrewSpin.Dome
'
' The Guid attribute sets the CLSID for ASCOM.DeviceName.Dome
' The ClassInterface/None addribute prevents an empty interface called
' _Dome from being created and used as the [default] interface
'

' This definition is used to select code that's only applicable for one device type
#Const Device = "Dome"

#Const ImplementShutter = True          ' Use to include shutter code

Imports ASCOM
Imports ASCOM.Astrometry
Imports ASCOM.Astrometry.AstroUtils
Imports ASCOM.DeviceInterface
Imports ASCOM.Utilities

Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading        ' to get Sleep function?
Imports System.Timers
Imports System.Text.RegularExpressions

<Guid("f4d84a25-ab66-4f7d-9d4e-10f6e16fe531")>
<ClassInterface(ClassInterfaceType.None)>
Public Class Dome

    ' The Guid attribute sets the CLSID for ASCOM.BrewSpin.Dome
    ' The ClassInterface/None addribute prevents an empty interface called
    ' _BrewSpin from being created and used as the [default] interface

    Implements IDomeV2

    '
    ' Driver ID and descriptive string that shows in the Chooser
    '
    Friend Shared driverID As String = "ASCOM.BrewSpin.Dome"
    Private Shared driverDescription As String = "BrewSpin Dome"

    Friend Shared comPortProfileName As String = "COM Port" 'Constants used for Profile persistence
    Friend Shared traceStateProfileName As String = "Trace Level"
    Friend Shared ParkAzProfileName As String = "Park Azimuth Position"
    Friend Shared offsetAzProfileName As String = "Offset Azimuth"
    Friend Shared AzEpsilonProfileName As String = "Azimuth Epsilon"
    Friend Shared totalBlocksProfileName As String = "Total Blocks"

    Friend Shared comPortDefault As String = "COM10"
    Friend Shared traceStateDefault As String = "True"
    Friend Shared ParkAzDefault As String = "90.0"
    Friend Shared offsetAzDefault As String = "-70.0"
    Friend Shared AzEpsilonDefault As String = "2.0"
    Friend Shared totalBlocksDefault As String = "490"

    Friend Shared comPort As String ' Variables to hold the currrent device configuration
    Friend Shared traceState As Boolean
    Friend Shared ParkAz As Double       ' Park Azimuth position
    Friend Shared offsetAz As Double     ' offset for Azimuth position to align with telescope
    Friend Shared AzEpsilon As Double    ' accuracy for Azimuth readings
    Friend Shared totalBlocks As Integer    ' number of blocks in one dome rotation

    Private gReadTimeoutObject As New ManualResetEvent(False)        ' used for Reading Serial port Async



    Private connectedState As Boolean ' Private variable to hold the connected state
    Private utilities As Util ' Private variable to hold an ASCOM Utilities object
    Private astroUtilities As AstroUtils ' Private variable to hold an AstroUtils object to provide the Range method
    Private TL As TraceLogger ' Private variable to hold the trace logger object (creates a diagnostic log file with information that you specify)

    Friend WithEvents SerialPort1 As IO.Ports.SerialPort     ' used to talk to Arduino

    ' Special Actions we can do - not normally in Dome drivers
    Private Const ACTION_SLEW = "Dome:Slew"             ' Slews forever
    Private Const ACTION_GETVAL = "Dome:GetVal"             ' Retrieve Test value
    Private Const ACTION_GET_PARAM = "Dome:GetParam"             ' Retrieve Specified value
    Private Const ACTION_UPDATE_TOTALBLOCKS = "Dome:UpdateTotalBlocks"

    ' Shutter Operation values
    Private gShutter As CShutterControl                  ' the class for shutter manipulation

    Private LockObject As Object        ' used for locking Write/Read in CommandString & ShutterCommandString

    '
    ' Constructor - Must be public for COM registration!
    '
    Public Sub New()

        ReadProfile() ' Read device configuration from the ASCOM Profile store
        TL = New TraceLogger("", "BrewSpin")
        TL.Enabled = traceState
        TL.LogMessage("Dome", "Starting initialisation")

        connectedState = False ' Initialise connected to false
        utilities = New Util() ' Initialise util object
        astroUtilities = New AstroUtils 'Initialise new astro utiliites object
        If (IsNothing(SerialPort1)) Then
            SerialPort1 = New System.IO.Ports.SerialPort
            SerialPort1.BaudRate = 9600
            SerialPort1.Handshake = IO.Ports.Handshake.None
        End If

        LockObject = New Object
        gShutter = New CShutterControl(Me, driverID, TL, LockObject)

        TL.LogMessage("Dome", "Completed initialisation")
    End Sub

    '
    ' PUBLIC COM INTERFACE IDomeV2 IMPLEMENTATION
    '

#Region "Common properties and methods"
    ''' <summary>
    ''' Displays the Setup Dialog form.
    ''' If the user clicks the OK button to dismiss the form, then
    ''' the new settings are saved, otherwise the old values are reloaded.
    ''' THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
    ''' </summary>
    Public Sub SetupDialog() Implements IDomeV2.SetupDialog
        ' consider only showing the setup dialog if not connected
        ' or call a different dialog if connected
        If IsConnected Then
            System.Windows.Forms.MessageBox.Show("Already connected, just press OK")
        End If

        Using F As SetupDialogForm = New SetupDialogForm()
            Dim result As System.Windows.Forms.DialogResult = F.ShowDialog()
            If result = DialogResult.OK Then
                WriteProfile() ' Persist device configuration values to the ASCOM Profile store
            End If
        End Using
    End Sub

    Public ReadOnly Property SupportedActions() As ArrayList Implements IDomeV2.SupportedActions
        Get
            Dim myList As ArrayList = New ArrayList
            TLSpace("SupportedActions")
            TL.LogMessage("SupportedActions Get", "Returning arraylist")
            myList.Add(ACTION_SLEW)
            myList.Add(ACTION_GETVAL)
            myList.Add(ACTION_GET_PARAM)
            myList.Add(ACTION_UPDATE_TOTALBLOCKS)

            Return myList
        End Get
    End Property

    Public Function Action(ByVal ActionName As String, ByVal ActionParameters As String) As String Implements IDomeV2.Action
        Dim retS As String

        Select Case (ActionName)
            Case ACTION_SLEW
                retS = ActionSlew(ActionParameters)

            Case ACTION_GETVAL
                retS = ActionGetVal()
            Case ACTION_GET_PARAM
                retS = ActionGetParam(ActionParameters)
            Case ACTION_UPDATE_TOTALBLOCKS
                retS = ActionUpdateTotalBlocks()

            Case Else
                retS = "Action " & ActionName & " is not supported by this driver"
        End Select
        Return retS
    End Function

    Public Sub CommandBlind(ByVal Command As String, Optional ByVal Raw As Boolean = False) Implements IDomeV2.CommandBlind
        CheckConnected("CommandBlind")
        ' Call CommandString and return as soon as it finishes
        Me.CommandString(Command, Raw)
        ' or
        Throw New MethodNotImplementedException("CommandBlind")
    End Sub

    Public Function CommandBool(ByVal Command As String, Optional ByVal Raw As Boolean = False) As Boolean _
        Implements IDomeV2.CommandBool
        CheckConnected("CommandBool")
        Dim ret As String = CommandString(Command, Raw)
        ' TODO decode the return string and return true or false
        ' or
        Throw New MethodNotImplementedException("CommandBool")
    End Function

    Private Function gBufferContains(buf() As Byte, c As Byte) As Boolean
        ' Search bytearray for char
        Dim i As Integer
        For i = 0 To buf.Length - 1
            If (buf(i) = c) Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub ClearGbuffer(buf() As Byte, len As Integer)
        ' Clear gBuffer
        Dim i As Integer
        For i = 0 To len - 1
            buf(i) = 0
        Next
    End Sub

    Private gTotalBytesRead As Integer = 0

    Private gRotationCommReadDone As Boolean = False
    Private gRotationCommString As String = ""

    Public Function CommandString(ByVal Command As String, Optional ByVal Raw As Boolean = False) As String _
        Implements IDomeV2.CommandString
        Dim s As String = ""
        Dim inpCount As Integer = 0

        CheckConnected("    CommandString")
        ' it's a good idea to put all the low level communication with the device here,
        ' then all communication calls this function
        ' you need something to ensure that only one command is in progress at a time
        SyncLock LockObject
            SerialPort1.WriteLine(Command)
            TL.LogMessage("  CommandString", "Writing command " & Command)
            Dim officialResponse As Boolean = False
            While (Not officialResponse)
                Try
                    s = SerialPort1.ReadLine()
                Catch ex As Exception
                    TL.LogMessage("  CommandString", "!!readLine failed after command " & Command & " " & ex.Message)
                    'Throw New System.Exception("CommandString: readLine failed after command " & Command & " " & ex.Message)
                End Try

                If (s.Contains("#")) Then
                    officialResponse = True
                    TL.LogMessage("  CommandString", "Received response " & s)
                    s = s.Replace(vbCr, "")
                    s = s.Replace(vbLf, "")
                Else
                    TL.LogMessage("  CommandString", "Arduino Trace " & Command & ": " & s)
                End If
            End While
        End SyncLock
        CommandString = s
    End Function


    Public Property Connected() As Boolean Implements IDomeV2.Connected
        Get
            TL.LogMessage("  Connected Get", IsConnected.ToString())
            Return IsConnected
        End Get
        Set(value As Boolean)
            TLSpace("Connected")
            TL.LogMessage("Connected Set", value.ToString())
            If value = IsConnected Then
                Return
            End If

            If value Then
                connectedState = True
                TL.LogMessage("  Connected Set True", "Connecting to port " + comPort)
                OpenSerialPort()
                Dim rets As String = CommandString("SETTOTALBLOCKS " & totalBlocks & "#")
                If (rets <> CShutterControl.DOME_SUCCESS) Then
                    TL.LogMessage("  Connected Set True", "!!Error setting SetTotalBlocks to " + totalBlocks.ToString())
                    Throw New Exception("Connected Set: failed SetTotalBlocks command with  " & totalBlocks.ToString())
                End If
            Else
                connectedState = False
                TL.LogMessage("  Connected Set False", "Disconnecting from port " + comPort)
                CloseSerialPort()
            End If
        End Set
    End Property


    Private Sub OpenSerialPort()
        CloseSerialPort()

        On Error Resume Next
        SerialPort1.PortName = comPort
        SerialPort1.BaudRate = 9600
        SerialPort1.ReadTimeout = 2000
        SerialPort1.WriteTimeout = 2000
        If (Err.Number > 0) Then
            TL.LogMessage("  OpenSerialPort", "!!" & Err.Description)
            Throw New ASCOM.InvalidValueException("OpenSerialPort invalid port " & comPort & ". Error: " & Err.Description)
        End If
        On Error Resume Next
        SerialPort1.Open()
        If (Err.Number > 0) Then
            TL.LogMessage("  OpenSerialPort", "!!" & Err.Description)
            Throw New ASCOM.InvalidValueException("OpenSerialPort failed to open port " & comPort & ". Error: " & Err.Description)
        Else
            TL.LogMessage("  OpenSerialPort", "Comm Port " & SerialPort1.PortName & " has been opened")
        End If
    End Sub

    Private Sub CloseSerialPort()
        If ((Not IsNothing(SerialPort1)) And SerialPort1.IsOpen) Then
            SerialPort1.Close()
        End If
    End Sub

    Public ReadOnly Property Description As String Implements IDomeV2.Description
        Get
            ' this pattern seems to be needed to allow a public property to return a private field
            Dim d As String = driverDescription
            TL.LogMessage("Description Get", d)
            Return d
        End Get
    End Property

    Public ReadOnly Property DriverInfo As String Implements IDomeV2.DriverInfo
        Get
            Dim m_version As Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version
            ' TODO customise this driver description
            Dim s_driverInfo As String = "BrewSpin driver for Arduino/Foster combo. Version: " + m_version.Major.ToString() + "." + m_version.Minor.ToString()
            TL.LogMessage("DriverInfo Get", s_driverInfo)
            Return s_driverInfo
        End Get
    End Property

    Public ReadOnly Property DriverVersion() As String Implements IDomeV2.DriverVersion
        Get
            ' Get our own assembly and report its version number
            TL.LogMessage("DriverVersion Get", Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString(2))
            Return Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString(2)
        End Get
    End Property

    Public ReadOnly Property InterfaceVersion() As Short Implements IDomeV2.InterfaceVersion
        Get
            TL.LogMessage("InterfaceVersion Get", "2")
            Return 2
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IDomeV2.Name
        Get
            Dim s_name As String = "BrewSpin"
            TL.LogMessage("Name Get", s_name)
            Return s_name
        End Get
    End Property

    Public Sub Dispose() Implements IDomeV2.Dispose
        ' Clean up the tracelogger and util objects
        TL.Enabled = False
        TL.Dispose()
        TL = Nothing
        utilities.Dispose()
        utilities = Nothing
        astroUtilities.Dispose()
        astroUtilities = Nothing
        gShutter = Nothing
    End Sub

#End Region

#Region "IDome Implementation"

    Public Sub AbortSlew() Implements IDomeV2.AbortSlew
        ' This is a mandatory parameter but we have no action to take in this simple driver
        CheckConnected("  RotArduino is not connected")
        TLSpace("AbortSlew")

        Dim s As String = CommandString("ABORT#")
        If (s <> CShutterControl.DOME_SUCCESS) Then
            TL.LogMessage("AbortSlew", "   !!Invalid Abort rotation response " & s)
        Else
            TL.LogMessage("AbortSlew", "Abort Rotation Completed with response " & s)
        End If

        s = gShutter.AbortSlew()
        TL.LogMessage("AbortSlew", "Shutter Completed with response " & s)
    End Sub

    Public ReadOnly Property Altitude() As Double Implements IDomeV2.Altitude
        Get
            TL.LogMessage("Altitude Get", "!!Not implemented")
            Throw New ASCOM.PropertyNotImplementedException("Altitude", False)
        End Get
    End Property

    Public ReadOnly Property AtHome() As Boolean Implements IDomeV2.AtHome
        Get
            CheckConnected("  RotArduino is not connected")
            TLSpace("AtHome")

            Dim homeflag As Boolean = False
            Dim rets As String
            rets = CommandString("ISHOME#")
            If (rets = "TRUE#") Then
                homeflag = True
            ElseIf (rets <> "FALSE#") Then
                TL.LogMessage("AtHome", "   !!Invalid ISHOME response " & rets)
            End If
            TL.LogMessage("AtHome", homeflag.ToString())

            gShutter.StartHomeClock(homeflag)
            AtHome = homeflag
        End Get
    End Property

    Public ReadOnly Property AtPark() As Boolean Implements IDomeV2.AtPark
        Get
            CheckConnected("  RotArduino is not connected")
            TLSpace("AtPark")

            Dim block As Integer = GetBlockPos()
            Dim curAz As Double = BlockToAzScope(block)
            Dim isPark As Boolean = False
            If (Math.Abs(curAz - ParkAz) < AzEpsilon) Then
                isPark = True
            End If
            AtPark = isPark
        End Get
    End Property

    ' Reading Azimuth of Dome
    Public ReadOnly Property Azimuth() As Double Implements IDomeV2.Azimuth
        Get
            CheckConnected("  RotArduino is not connected")
            TLSpace("Azimuth")

            Azimuth = internalAzimuth()
        End Get
    End Property

    Private Function internalAzimuth() As Double
        Dim block As Integer = GetBlockPos()
        Dim curAz As Double = BlockToAzScope(block)
        internalAzimuth = curAz
    End Function

    Public ReadOnly Property CanFindHome() As Boolean Implements IDomeV2.CanFindHome
        Get
            TL.LogMessage("CanFindHome Get", True.ToString())
            Return True
        End Get
    End Property

    Public ReadOnly Property CanPark() As Boolean Implements IDomeV2.CanPark
        Get
            TL.LogMessage("CanPark Get", True.ToString())
            Return True
        End Get
    End Property

    Public ReadOnly Property CanSetAltitude() As Boolean Implements IDomeV2.CanSetAltitude
        Get
            TL.LogMessage("CanSetAltitude Get", False.ToString())
            Return False
        End Get
    End Property

    Public ReadOnly Property CanSetAzimuth() As Boolean Implements IDomeV2.CanSetAzimuth
        Get
            TL.LogMessage("CanSetAzimuth Get", True.ToString())
            Return True
        End Get
    End Property

    Public ReadOnly Property CanSetPark() As Boolean Implements IDomeV2.CanSetPark
        Get
            TL.LogMessage("CanSetPark Get", True.ToString())
            Return True
        End Get
    End Property

    Public ReadOnly Property CanSetShutter() As Boolean Implements IDomeV2.CanSetShutter
        Get
            TL.LogMessage("CanSetShutter Get", True.ToString())
            Return True
        End Get
    End Property

    Public ReadOnly Property CanSlave() As Boolean Implements IDomeV2.CanSlave
        Get
            TL.LogMessage("CanSlave Get", True.ToString())
            Return True
        End Get
    End Property

    Public ReadOnly Property CanSyncAzimuth() As Boolean Implements IDomeV2.CanSyncAzimuth
        Get
            TL.LogMessage("CanSyncAzimuth Get", True.ToString())
            Return True
        End Get
    End Property

    Public Sub CloseShutter() Implements IDomeV2.CloseShutter
        CheckConnected("  RotArduino is not connected")
        TLSpace("CloseShutter")
        Dim s As String = gShutter.CloseShutter()
        TL.LogMessage("CloseShutter", s)
    End Sub

    Public Sub FindHome() Implements IDomeV2.FindHome
        CheckConnected("  RotArduino is not connected")
        TLSpace("FindHome")

        Dim rets As String = CommandString("SLEWTO 0 W#")
        If (rets <> CShutterControl.DOME_SUCCESS) Then
            TL.LogMessage("FindHome", "!!Command failed error " & rets)
            'Throw New ASCOM.DriverException("FindHome command failed. error " & rets)
        Else
            TL.LogMessage("FindHome", "started ")
        End If
    End Sub

    Public Sub OpenShutter() Implements IDomeV2.OpenShutter
        CheckConnected("  RotArduino is not connected")
        TLSpace("OpenShutter")
        Dim s As String = gShutter.OpenShutter()
        TL.LogMessage("OpenShutter", s)
    End Sub

    Public Sub Park() Implements IDomeV2.Park
        TLSpace("Park")
        internalSlewToAzimuth(ParkAz)
    End Sub

    Public Sub SetPark() Implements IDomeV2.SetPark
        CheckConnected("RotArduino is not connected")
        TLSpace("SetPark")
        ParkAz = internalAzimuth()

        TL.LogMessage("SetPark", "Park position set to " & ParkAz.ToString())
        ' Save to profile
        Using driverProfile As New Profile()
            driverProfile.DeviceType = "Dome"
            driverProfile.WriteValue(driverID, ParkAzProfileName, ParkAz.ToString())
        End Using

    End Sub

    Public ReadOnly Property ShutterStatus() As ShutterState Implements IDomeV2.ShutterStatus
        Get
            TLSpace("ShutterStatus")
            Dim shutState As ShutterState = gShutter.GetShutterState()
            TL.LogMessage("ShutterStatus", shutState.ToString())
            Return shutState
        End Get
    End Property

    Public Property Slaved() As Boolean Implements IDomeV2.Slaved
        Get
            TL.LogMessage("Slaved Get", False.ToString())
            Return False
        End Get
        Set(value As Boolean)
            TL.LogMessage("Slaved Set", "!!not implemented")
            Throw New ASCOM.PropertyNotImplementedException("Slaved", True)
        End Set
    End Property

    Public Sub SlewToAltitude(Altitude As Double) Implements IDomeV2.SlewToAltitude
        TL.LogMessage("SlewToAltitude", "!!Not implemented")
        Throw New ASCOM.MethodNotImplementedException("SlewToAltitude")
    End Sub

    ' Azimuth is converted to blocks. Direction is determined by shortest route from current block position.
    ' Special Case -
    '     Azimuth = 9999    means SLEWTO 9999 W#
    '     Azimuth = -9999   means SLEWTO 9999 E#
    ' These slew continuously in the desired direction
    Public Sub SlewToAzimuth(Azimuth As Double) Implements IDomeV2.SlewToAzimuth
        CheckConnected("  RotArduino is not connected")
        TLSpace("SlewToAzimuth")
        internalSlewToAzimuth(Azimuth)
    End Sub

    Private Sub internalSlewToAzimuth(Azimuth As Double)
        ' Validate Azimuth. Must be 0 to 360, or 9999, or -9999
        If (((Azimuth >= 0) And (Azimuth < 360)) Or ((Azimuth = 9999) Or (Azimuth = -9999))) Then
            ' OK
        Else
            TL.LogMessage("SlewToAzimuth", "!!invalid azimuth " & Azimuth.ToString())
            Exit Sub
            'Throw New ASCOM.DriverException("SlewToAzimuth invalid Az " & Azimuth.ToString())
        End If
        Dim newBlock As Integer = AzScopeToBlock(Azimuth)
        Dim rets As String
        Dim dir As String = "E"
        ' retrieve current position so we can figure out which direction to go
        Dim curBlock As Integer = GetBlockPos()
        'Dim curAz As Double = BlockToAzScope(curBlock)

        ' We might already be there
        If (Math.Abs(curBlock - newBlock) < 1) Then
            TL.LogMessage("SlewToAzimuth", "Already at target block position ")
            Return
        End If

        If (Math.Abs(Azimuth) = 9999) Then
            newBlock = 9999
            If (Azimuth > 0) Then
                dir = "W"
            End If
        Else
            dir = DetermineSlewDir(newBlock, curBlock)
        End If

        ' Build the command
        Dim cmd As String = "SLEWTO " & newBlock.ToString() & " " & dir & "#"

        rets = CommandString(cmd)
        If (rets <> CShutterControl.DOME_SUCCESS) Then
            TL.LogMessage("SlewToAzimuth", "!!failed set to Az " & Azimuth.ToString() & " block " & newBlock.ToString())
            Exit Sub
            'Throw New ASCOM.DriverException("SlewToAzimuth failed set to Az " & Azimuth.ToString() & " block " & newBlock.ToString() & "cmd " & cmd)
        End If

        gShutter.StartHomeClock(False)
    End Sub

    Public ReadOnly Property Slewing() As Boolean Implements IDomeV2.Slewing
        ' I am not using shutter operation as part of slewing definition
        Get
            TLSpace("Slewing")
            ' Check if shutters are moving.
            ' If not, check if Rotation is moving
            Dim mySlewing As Boolean = False
            If (gShutter.ShutterSlewing()) Then
                mySlewing = True
            Else
                Dim rets As String = CommandString("ISSLEWING#")
                If (rets = "TRUE#") Then
                    mySlewing = True
                ElseIf (rets = "FALSE#") Then
                    mySlewing = False
                Else
                    TL.LogMessage("Slewing Get", "   !!Invalid ISSLEWING response " & rets)
                    mySlewing = False
                End If
            End If

            TL.LogMessage("Slewing Get", mySlewing.ToString())
            Slewing = mySlewing
        End Get
    End Property

    Public Sub SyncToAzimuth(Azimuth As Double) Implements IDomeV2.SyncToAzimuth
        ' The given azimuth should correspond to the current block position
        ' The offset required is given by
        ' off = blkPos * 360 / totalBlocks  - azimuth
        ' Use this to set the variable offsetAz, which is stored in profile
        TLSpace("SyncToAzimuth")
        TL.LogMessage("SyncToAzimuth", "with azimuth " & Azimuth.ToString())
        If (Azimuth < 0 Or Azimuth >= 360) Then
            TL.LogMessage("SyncToAzimuth", "!!Invalid azimuth " & Azimuth.ToString())
            Exit Sub
            'Throw New ASCOM.DriverException("SlewToAltitude failed, invalid Azimuth " & Azimuth.ToString())
        End If
        If (totalBlocks = 0) Then
            TL.LogMessage("SyncToAzimuth", "!!totalBlocks is 0 ")
            Exit Sub
            'Throw New ASCOM.DriverException("SlewToAltitude failed, totalBlocks is zero")
        End If

        Dim blkPos As Integer = GetBlockPos()
        offsetAz = (blkPos * 360 / totalBlocks) - Azimuth
        TL.LogMessage("SyncToAzimuth", "New offsetAz is " & offsetAz.ToString())

        ' Save in profile
        Using driverProfile As New Profile()
            driverProfile.DeviceType = "Dome"
            driverProfile.WriteValue(driverID, offsetAzProfileName, offsetAz.ToString())
        End Using

    End Sub

#End Region

#Region "Private properties and methods"
    ' here are some useful properties and methods that used as required
    ' to help with

#Region "ASCOM Registration"

    Private Shared Sub RegUnregASCOM(ByVal bRegister As Boolean)

        Using P As New Profile() With {.DeviceType = "Dome"}
            If bRegister Then
                P.Register(driverID, driverDescription)
            Else
                P.Unregister(driverID)
            End If
        End Using

    End Sub

    <ComRegisterFunction()>
    Public Shared Sub RegisterASCOM(ByVal T As Type)

        RegUnregASCOM(True)

    End Sub

    <ComUnregisterFunction()>
    Public Shared Sub UnregisterASCOM(ByVal T As Type)

        RegUnregASCOM(False)

    End Sub

#End Region

    ''' <summary>
    ''' Returns true if there is a valid connection to the driver hardware
    ''' </summary>
    Private ReadOnly Property IsConnected As Boolean
        Get
            ' TODO check that the driver hardware connection exists and is connected to the hardware
            Return connectedState
        End Get
    End Property

    ''' <summary>
    ''' Use this function to throw an exception if we aren't connected to the hardware
    ''' </summary>
    ''' <param name="message"></param>
    Private Sub CheckConnected(ByVal message As String)
        If Not IsConnected Then
            Throw New NotConnectedException(message)
        End If
    End Sub

    ''' <summary>
    ''' Read the device configuration from the ASCOM Profile store
    ''' </summary>
    Friend Sub ReadProfile()
        Using driverProfile As New Profile()
            driverProfile.DeviceType = "Dome"
            traceState = Convert.ToBoolean(driverProfile.GetValue(driverID, traceStateProfileName, String.Empty, traceStateDefault))
            comPort = driverProfile.GetValue(driverID, comPortProfileName, String.Empty, comPortDefault)
            ParkAz = Convert.ToDouble(driverProfile.GetValue(driverID, ParkAzProfileName, String.Empty, ParkAzDefault))
            offsetAz = Convert.ToDouble(driverProfile.GetValue(driverID, offsetAzProfileName, String.Empty, offsetAzDefault))
            AzEpsilon = Convert.ToDouble(driverProfile.GetValue(driverID, AzEpsilonProfileName, String.Empty, AzEpsilonDefault))
            totalBlocks = Convert.ToInt32(driverProfile.GetValue(driverID, totalBlocksProfileName, String.Empty, totalBlocksDefault))

        End Using

    End Sub

    ''' <summary>
    ''' Write the device configuration to the  ASCOM  Profile store
    ''' </summary>
    Friend Sub WriteProfile()
        Using driverProfile As New Profile()
            driverProfile.DeviceType = "Dome"
            driverProfile.WriteValue(driverID, traceStateProfileName, traceState.ToString())
            driverProfile.WriteValue(driverID, comPortProfileName, comPort.ToString())
            driverProfile.WriteValue(driverID, ParkAzProfileName, ParkAz.ToString())
            driverProfile.WriteValue(driverID, offsetAzProfileName, offsetAz.ToString())
            driverProfile.WriteValue(driverID, AzEpsilonProfileName, AzEpsilon.ToString())
            driverProfile.WriteValue(driverID, totalBlocksProfileName, totalBlocks.ToString())

        End Using
        gShutter.WriteProfile()
    End Sub

    ' GetBlockPos retrieves the current block position from Arduino
    ' returned string is like POS nnn#
    ' return -1 if messed up somehow
    Private Function GetBlockPos() As Integer
        Dim s As String = ""
        Dim pos As Integer = 0

        s = CommandString("GETPOS#")
        ' should return string like POS 235#
        If ((s.Length >= 3) And (s.Substring(0, 3) <> "POS")) Then
            TL.LogMessage("    GetBlockPos", "!!Arduino reports bad string " & s)
            ' Can we use the malformed string?
            Dim rgx As Regex = New Regex("\w*\s*\d+#")
            If (rgx.IsMatch(s)) Then
                ' rebuild string correctly
                rgx = New Regex("\w*\s*(\d+#)")
                s = rgx.Replace(s, "POS $1")
                TL.LogMessage("    GetBlockPos", "!!Malformed string rebuilt as " & s)
            Else
                ' cannot use string
                TL.LogMessage("    GetBlockPos", "!!Cannot use string " & s)
            End If
        End If

        If (s.Substring(0, 3) = "POS") Then
            s = s.Replace("#", "")   ' remove the #
            pos = CInt(s.Substring(4))
            TL.LogMessage("    GetBlockPos", "Arduino reports " & s & " position is " & pos.ToString())
        End If
        GetBlockPos = pos
    End Function

    Private Function DetermineSlewDir(newBlock As Integer, curBlock As Integer) As String
        ' Which way to turn? Possible situations: note that from Home sensor, blocks increment going to the left (as dome goes west)
        '  H = home position, C is current block position, N is new block position. Assume totalBlocks = 500
        '  20   15     0    475   470                                            WestBlkMove N-C   EastBlkMove C - N
        '              H     C     N        C = 475, N = 470  Should go East     -5 => 495         5
        '              H     N     C        C = 470, N = 475  Should go West      5                -5 => 495  
        '   C     N    H                    C = 20,  N = 15   Should go East     -5 => 495         5
        '         C    H     N              C = 15,  N = 475  Should go East      460              -460 => 40
        '         N    H     C              C = 475, N = 15   Should go West     -460 => 40        460
        '         C    H     N              C = 15,  N = 475  Should go East      460              -460 => 40
        Dim dir As String = "E"

        Dim westBlkMove As Integer = newBlock - curBlock
        If (westBlkMove < 0) Then westBlkMove = westBlkMove + totalBlocks
        Dim eastBlkMove As Integer = curBlock - newBlock
        If (eastBlkMove < 0) Then eastBlkMove = eastBlkMove + totalBlocks

        If (eastBlkMove > westBlkMove) Then dir = "W"
        Return dir
    End Function


    ' Azimuth offset calculations
    ' We have two azumuth coordinates: 
    '       the azimuth requested by the scope (azScope)
    '       the azimuth seen by the Arduino, based on Home position (asArduino)
    ' Example:
    '       Scope says to slew to Az = 90. The Arduino slews to an actual angle of 110, since the Home sensor is at an angle of 20 degrees.
    '       So, the offset is at 20 degrees  offsetAz = 20
    '       azArduino = AzScope + offsetAz
    '       azScope = azArduino - offsetAz
    ' Note that offsetAz can be negative

    ' Convert Block number to Azimuth.
    ' Accounts for the offsetAz
    Private Function BlockToAzScope(block As Integer) As Double
        Dim az As Double = 0
        If (totalBlocks <= 0) Then
            totalBlocks = 600
            TL.LogMessage("    BlockToAzScope", "!!zero totalBlocks. Set to 600")
        End If

        az = block * 360.0 / totalBlocks    ' azArduino
        az = az - offsetAz
        If (az < 0) Then
            az = az + 360.0
        ElseIf (az >= 360) Then
            az = az - 360.0
        End If
        TL.LogMessage("    BlockToAzScope", "block " & block.ToString() & " gives az " & az.ToString())
        BlockToAzScope = az
    End Function

    ' Convert Azimuth to  Block number.
    ' Accounts for the offsetAz
    Private Function AzScopeToBlock(azScope As Double) As Integer
        Dim azArd As Double = azScope + offsetAz
        If (azArd >= 360) Then
            azArd = azArd - 360.0
        ElseIf (azArd < 0) Then
            azArd = azArd + 360.0
        End If
        Dim blk As Integer = azArd / 360.0 * totalBlocks
        TL.LogMessage("    AzScopeToBlock", "az " & azScope.ToString() & " gives block " & blk.ToString())
        AzScopeToBlock = blk
    End Function

    Private Function IsSlewing() As Boolean
        Dim s As String = ""
        Dim res As Boolean = False

        Try
            s = CommandString("IsSlewing#")
        Catch ex As Exception
            TL.LogMessage("  IsSlewing", "!!IsSlewing command failed with response {" & s & "}" & vbCrLf & "err mesg " & ex.Message)
            Return False
        End Try
        If (s = "TRUE#") Then
            res = True
        ElseIf (s <> "FALSE#") Then
            TL.LogMessage("  IsSlewing", "   !!Invalid IsSlewing response " & s)
        End If

        If (Not res) Then      ' if dome is not slewing, maybe shutter is
            res = gShutter.ShutterSlewing()
        End If
        TL.LogMessage("  IsSlewing", "IsSlewing is " & res.ToString())
        Return res
    End Function

    Private Function ActionSlew(dir As String) As String
        CheckConnected("  RotArduino is not connected")

        TL.LogMessage("ActionSlew", "Plannning to slew forever to the " & dir)
        Dim rets As String = CommandString("SLEWTO 9999 " & dir & "#")
        If (rets <> CShutterControl.DOME_SUCCESS) Then
            TL.LogMessage("ActionSlew", "!!Command failed error " & rets)
            'Throw New ASCOM.DriverException("ActionSlew command failed. error " & rets)
        Else
            TL.LogMessage("ActionSlew", "started " & dir)
        End If

        Return rets
    End Function

    Private Function ActionGetVal() As String
        CheckConnected("RotArduino is not connected")

        TL.LogMessage("ActionGetVal", "starting command")
        Dim rets As String = CommandString("GETVAL#")
        TL.LogMessage("ActionGetVal", "retrieved " & rets)

        Return rets
    End Function

    Private Function ActionGetParam(parmName As String) As String
        CheckConnected("  RotArduino is not connected")
        Dim rets As String = ""
        Select Case (UCase(parmName))
            Case "OFFSETAZ"
                rets = Format(offsetAz, "0.0")
            Case "PARKAZ"
                rets = Format(ParkAz, "0.0")
            Case "BLKPOS"
                Dim blkPos As String = CommandString("GETPOS#")
                rets = blkPos
            Case "TOTALBLOCKS"
                rets = Format(totalBlocks, "0")
            Case "OPENCLOSESTEP"
                rets = gShutter.GetOpenCloseStep()
            Case "SHUTTERVALS"
                rets = gShutter.GetShutterVals()

            Case Else
                rets = "Parameter " & parmName & " is not supported"
        End Select

        TL.LogMessage("  ActionGetParam", parmName & " retrieved " & rets)

        Return rets
    End Function

    Private Function ActionUpdateTotalBlocks() As String
        ' CountBlocks process has updated TotalBlocks in the Arduino.
        ' Retrieve the value, update the value in the driver, update the Profile, 
        ' and Return info in the form Count nnn#
        Dim s As String = CommandString("GETTOTALBLOCKS#")

        If ((s.Length > 14) And (s.Substring(0, 11) = "TOTALBLOCKS")) Then
            Dim sCount As String = s.Replace("#", "")
            sCount = sCount.Replace("TOTALBLOCKS", "")         ' remove the COUNT
            Dim cnt As Integer
            If (Integer.TryParse(sCount, cnt)) Then
                TL.LogMessage("ActionUpdateTotalBlocks", "CountBlocks completed with " & cnt.ToString())
                ' save as TotalBlocks parameter
                totalBlocks = cnt
                ' Save in profile
                Using driverProfile As New Profile()
                    driverProfile.DeviceType = "Dome"
                    driverProfile.WriteValue(driverID, totalBlocksProfileName, totalBlocks.ToString())
                End Using
                TL.LogMessage("ActionUpdateTotalBlocks", "TotalBlocks updated with " & totalBlocks.ToString())
                s = totalBlocks.ToString()    ' return the new value
            Else
                TL.LogMessage("ActionUpdateTotalBlocks", "!!CountBlocks ended with bad count " & s)
            End If
        Else
            TL.LogMessage("ActionUpdateTotalBlocks", "!!GetTotalBlocks did not return Count: " & s)
        End If
        Return s
    End Function

    Private Sub TLSpace(cmdName As String)
        TL.LogMessage(" ", "==========================================  " & cmdName)
    End Sub





#End Region

End Class
