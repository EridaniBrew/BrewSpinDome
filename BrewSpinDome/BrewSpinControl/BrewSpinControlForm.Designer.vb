<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BrewSpinControlForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BrewSpinControlForm))
        Me.buttonConnect = New System.Windows.Forms.Button()
        Me.btnGoToAz = New System.Windows.Forms.Button()
        Me.lblCurrentAz = New System.Windows.Forms.Label()
        Me.btnHome = New System.Windows.Forms.Button()
        Me.btnPark = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.picArduinoAvail = New System.Windows.Forms.PictureBox()
        Me.lblTemperature = New System.Windows.Forms.Label()
        Me.lblPowerVoltage = New System.Windows.Forms.Label()
        Me.picLowerClosedSw = New System.Windows.Forms.PictureBox()
        Me.picLowerOpenSw = New System.Windows.Forms.PictureBox()
        Me.picUpperClosedSw = New System.Windows.Forms.PictureBox()
        Me.picUpperOpenSw = New System.Windows.Forms.PictureBox()
        Me.lblShutterStatus = New System.Windows.Forms.Label()
        Me.btnCloseShutter = New System.Windows.Forms.Button()
        Me.btnOpenShutter = New System.Windows.Forms.Button()
        Me.picShutter = New System.Windows.Forms.PictureBox()
        Me.lblBlockPos = New System.Windows.Forms.Label()
        Me.picPark = New System.Windows.Forms.PictureBox()
        Me.picSlewing = New System.Windows.Forms.PictureBox()
        Me.picHome = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnSlewWest = New System.Windows.Forms.Button()
        Me.btnSlewEast = New System.Windows.Forms.Button()
        Me.btnHalt = New System.Windows.Forms.Button()
        Me.btnTest = New System.Windows.Forms.Button()
        Me.txtTargetAz = New System.Windows.Forms.TextBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.picCountTics = New System.Windows.Forms.PictureBox()
        Me.lblParkAz = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblOffsetAz = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.buttonChoose = New System.Windows.Forms.Button()
        Me.txtDesiredAz = New System.Windows.Forms.TextBox()
        Me.btnSyncInstr = New System.Windows.Forms.Button()
        Me.btnSync = New System.Windows.Forms.Button()
        Me.lblBlockCount = New System.Windows.Forms.Label()
        Me.btnCountBlocks = New System.Windows.Forms.Button()
        Me.btnSetPark = New System.Windows.Forms.Button()
        Me.labelDriverId = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.CheckShutterTimer = New System.Windows.Forms.Timer(Me.components)
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.picArduinoAvail, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picLowerClosedSw, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picLowerOpenSw, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picUpperClosedSw, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picUpperOpenSw, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picShutter, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picPark, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picSlewing, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picHome, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        CType(Me.picCountTics, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'buttonConnect
        '
        Me.buttonConnect.Location = New System.Drawing.Point(305, 6)
        Me.buttonConnect.Name = "buttonConnect"
        Me.buttonConnect.Size = New System.Drawing.Size(72, 23)
        Me.buttonConnect.TabIndex = 4
        Me.buttonConnect.Text = "Connect"
        Me.buttonConnect.UseVisualStyleBackColor = True
        '
        'btnGoToAz
        '
        Me.btnGoToAz.Location = New System.Drawing.Point(12, 53)
        Me.btnGoToAz.Name = "btnGoToAz"
        Me.btnGoToAz.Size = New System.Drawing.Size(51, 23)
        Me.btnGoToAz.TabIndex = 6
        Me.btnGoToAz.Text = "Go To"
        Me.ToolTip1.SetToolTip(Me.btnGoToAz, "Rotate dome to the target Azimuth")
        Me.btnGoToAz.UseVisualStyleBackColor = True
        '
        'lblCurrentAz
        '
        Me.lblCurrentAz.AutoSize = True
        Me.lblCurrentAz.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentAz.Location = New System.Drawing.Point(65, 11)
        Me.lblCurrentAz.Name = "lblCurrentAz"
        Me.lblCurrentAz.Size = New System.Drawing.Size(19, 20)
        Me.lblCurrentAz.TabIndex = 8
        Me.lblCurrentAz.Text = "0"
        Me.ToolTip1.SetToolTip(Me.lblCurrentAz, "Current Azimuth")
        '
        'btnHome
        '
        Me.btnHome.Location = New System.Drawing.Point(69, 98)
        Me.btnHome.Name = "btnHome"
        Me.btnHome.Size = New System.Drawing.Size(57, 23)
        Me.btnHome.TabIndex = 9
        Me.btnHome.Text = "Home"
        Me.btnHome.UseVisualStyleBackColor = True
        '
        'btnPark
        '
        Me.btnPark.Location = New System.Drawing.Point(69, 145)
        Me.btnPark.Name = "btnPark"
        Me.btnPark.Size = New System.Drawing.Size(57, 23)
        Me.btnPark.TabIndex = 10
        Me.btnPark.Text = "Park"
        Me.btnPark.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Top
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.MinimumSize = New System.Drawing.Size(483, 176)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(483, 240)
        Me.TabControl1.TabIndex = 11
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.SystemColors.Window
        Me.TabPage1.Controls.Add(Me.picArduinoAvail)
        Me.TabPage1.Controls.Add(Me.lblTemperature)
        Me.TabPage1.Controls.Add(Me.lblPowerVoltage)
        Me.TabPage1.Controls.Add(Me.picLowerClosedSw)
        Me.TabPage1.Controls.Add(Me.picLowerOpenSw)
        Me.TabPage1.Controls.Add(Me.picUpperClosedSw)
        Me.TabPage1.Controls.Add(Me.picUpperOpenSw)
        Me.TabPage1.Controls.Add(Me.lblShutterStatus)
        Me.TabPage1.Controls.Add(Me.btnCloseShutter)
        Me.TabPage1.Controls.Add(Me.btnOpenShutter)
        Me.TabPage1.Controls.Add(Me.picShutter)
        Me.TabPage1.Controls.Add(Me.lblBlockPos)
        Me.TabPage1.Controls.Add(Me.picPark)
        Me.TabPage1.Controls.Add(Me.picSlewing)
        Me.TabPage1.Controls.Add(Me.picHome)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Controls.Add(Me.btnSlewWest)
        Me.TabPage1.Controls.Add(Me.btnSlewEast)
        Me.TabPage1.Controls.Add(Me.btnHalt)
        Me.TabPage1.Controls.Add(Me.btnTest)
        Me.TabPage1.Controls.Add(Me.txtTargetAz)
        Me.TabPage1.Controls.Add(Me.btnPark)
        Me.TabPage1.Controls.Add(Me.buttonConnect)
        Me.TabPage1.Controls.Add(Me.btnHome)
        Me.TabPage1.Controls.Add(Me.lblCurrentAz)
        Me.TabPage1.Controls.Add(Me.btnGoToAz)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(475, 214)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Operation"
        '
        'picArduinoAvail
        '
        Me.picArduinoAvail.ErrorImage = Global.ASCOM.BrewSpin.My.Resources.Resources.greenLightOff
        Me.picArduinoAvail.Image = Global.ASCOM.BrewSpin.My.Resources.Resources.grayLightOff
        Me.picArduinoAvail.InitialImage = Nothing
        Me.picArduinoAvail.Location = New System.Drawing.Point(367, 76)
        Me.picArduinoAvail.Name = "picArduinoAvail"
        Me.picArduinoAvail.Size = New System.Drawing.Size(16, 16)
        Me.picArduinoAvail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picArduinoAvail.TabIndex = 32
        Me.picArduinoAvail.TabStop = False
        Me.ToolTip1.SetToolTip(Me.picArduinoAvail, "Arduino Connection Status")
        '
        'lblTemperature
        '
        Me.lblTemperature.AutoSize = True
        Me.lblTemperature.Location = New System.Drawing.Point(272, 183)
        Me.lblTemperature.Name = "lblTemperature"
        Me.lblTemperature.Size = New System.Drawing.Size(13, 13)
        Me.lblTemperature.TabIndex = 31
        Me.lblTemperature.Text = "0"
        Me.ToolTip1.SetToolTip(Me.lblTemperature, "Shutter Controller Temperature (F)")
        '
        'lblPowerVoltage
        '
        Me.lblPowerVoltage.AutoSize = True
        Me.lblPowerVoltage.Location = New System.Drawing.Point(230, 183)
        Me.lblPowerVoltage.Name = "lblPowerVoltage"
        Me.lblPowerVoltage.Size = New System.Drawing.Size(13, 13)
        Me.lblPowerVoltage.TabIndex = 30
        Me.lblPowerVoltage.Text = "0"
        Me.ToolTip1.SetToolTip(Me.lblPowerVoltage, "Power Supply Voltage")
        '
        'picLowerClosedSw
        '
        Me.picLowerClosedSw.Image = Global.ASCOM.BrewSpin.My.Resources.Resources.grayLightOff
        Me.picLowerClosedSw.InitialImage = Nothing
        Me.picLowerClosedSw.Location = New System.Drawing.Point(367, 169)
        Me.picLowerClosedSw.Name = "picLowerClosedSw"
        Me.picLowerClosedSw.Size = New System.Drawing.Size(16, 16)
        Me.picLowerClosedSw.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLowerClosedSw.TabIndex = 29
        Me.picLowerClosedSw.TabStop = False
        Me.ToolTip1.SetToolTip(Me.picLowerClosedSw, "Lower Shutter Closed Switch")
        '
        'picLowerOpenSw
        '
        Me.picLowerOpenSw.Image = Global.ASCOM.BrewSpin.My.Resources.Resources.grayLightOff
        Me.picLowerOpenSw.InitialImage = Nothing
        Me.picLowerOpenSw.Location = New System.Drawing.Point(367, 145)
        Me.picLowerOpenSw.Name = "picLowerOpenSw"
        Me.picLowerOpenSw.Size = New System.Drawing.Size(16, 16)
        Me.picLowerOpenSw.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picLowerOpenSw.TabIndex = 28
        Me.picLowerOpenSw.TabStop = False
        Me.ToolTip1.SetToolTip(Me.picLowerOpenSw, "Lower Shutter Open Switch")
        '
        'picUpperClosedSw
        '
        Me.picUpperClosedSw.Image = Global.ASCOM.BrewSpin.My.Resources.Resources.grayLightOff
        Me.picUpperClosedSw.InitialImage = Nothing
        Me.picUpperClosedSw.Location = New System.Drawing.Point(367, 123)
        Me.picUpperClosedSw.Name = "picUpperClosedSw"
        Me.picUpperClosedSw.Size = New System.Drawing.Size(16, 16)
        Me.picUpperClosedSw.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picUpperClosedSw.TabIndex = 27
        Me.picUpperClosedSw.TabStop = False
        Me.ToolTip1.SetToolTip(Me.picUpperClosedSw, "Upper Shutter Closed Switch")
        '
        'picUpperOpenSw
        '
        Me.picUpperOpenSw.Image = Global.ASCOM.BrewSpin.My.Resources.Resources.grayLightOff
        Me.picUpperOpenSw.InitialImage = Nothing
        Me.picUpperOpenSw.Location = New System.Drawing.Point(367, 98)
        Me.picUpperOpenSw.Name = "picUpperOpenSw"
        Me.picUpperOpenSw.Size = New System.Drawing.Size(16, 16)
        Me.picUpperOpenSw.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picUpperOpenSw.TabIndex = 26
        Me.picUpperOpenSw.TabStop = False
        Me.ToolTip1.SetToolTip(Me.picUpperOpenSw, "Upper Shutter Open Switch")
        '
        'lblShutterStatus
        '
        Me.lblShutterStatus.AutoSize = True
        Me.lblShutterStatus.Location = New System.Drawing.Point(272, 160)
        Me.lblShutterStatus.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblShutterStatus.Name = "lblShutterStatus"
        Me.lblShutterStatus.Size = New System.Drawing.Size(35, 13)
        Me.lblShutterStatus.TabIndex = 25
        Me.lblShutterStatus.Text = "status"
        '
        'btnCloseShutter
        '
        Me.btnCloseShutter.Location = New System.Drawing.Point(274, 127)
        Me.btnCloseShutter.Name = "btnCloseShutter"
        Me.btnCloseShutter.Size = New System.Drawing.Size(86, 23)
        Me.btnCloseShutter.TabIndex = 24
        Me.btnCloseShutter.Text = "Close Shutter"
        Me.btnCloseShutter.UseVisualStyleBackColor = True
        '
        'btnOpenShutter
        '
        Me.btnOpenShutter.Location = New System.Drawing.Point(274, 98)
        Me.btnOpenShutter.Name = "btnOpenShutter"
        Me.btnOpenShutter.Size = New System.Drawing.Size(86, 23)
        Me.btnOpenShutter.TabIndex = 23
        Me.btnOpenShutter.Text = "Open Shutter"
        Me.btnOpenShutter.UseVisualStyleBackColor = True
        '
        'picShutter
        '
        Me.picShutter.Image = Global.ASCOM.BrewSpin.My.Resources.Resources.grayLightOff
        Me.picShutter.InitialImage = Nothing
        Me.picShutter.Location = New System.Drawing.Point(216, 89)
        Me.picShutter.Name = "picShutter"
        Me.picShutter.Size = New System.Drawing.Size(41, 41)
        Me.picShutter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picShutter.TabIndex = 21
        Me.picShutter.TabStop = False
        Me.ToolTip1.SetToolTip(Me.picShutter, "At Home")
        '
        'lblBlockPos
        '
        Me.lblBlockPos.AutoSize = True
        Me.lblBlockPos.Location = New System.Drawing.Point(138, 16)
        Me.lblBlockPos.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblBlockPos.Name = "lblBlockPos"
        Me.lblBlockPos.Size = New System.Drawing.Size(13, 13)
        Me.lblBlockPos.TabIndex = 20
        Me.lblBlockPos.Text = "0"
        '
        'picPark
        '
        Me.picPark.Image = Global.ASCOM.BrewSpin.My.Resources.Resources.grayLightOff
        Me.picPark.InitialImage = Nothing
        Me.picPark.Location = New System.Drawing.Point(12, 136)
        Me.picPark.Name = "picPark"
        Me.picPark.Size = New System.Drawing.Size(41, 41)
        Me.picPark.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picPark.TabIndex = 19
        Me.picPark.TabStop = False
        Me.ToolTip1.SetToolTip(Me.picPark, "At Park")
        '
        'picSlewing
        '
        Me.picSlewing.Image = Global.ASCOM.BrewSpin.My.Resources.Resources.grayLightOff
        Me.picSlewing.InitialImage = Nothing
        Me.picSlewing.Location = New System.Drawing.Point(194, 6)
        Me.picSlewing.Name = "picSlewing"
        Me.picSlewing.Size = New System.Drawing.Size(41, 41)
        Me.picSlewing.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picSlewing.TabIndex = 18
        Me.picSlewing.TabStop = False
        Me.ToolTip1.SetToolTip(Me.picSlewing, "Slewing Active")
        Me.picSlewing.Visible = False
        '
        'picHome
        '
        Me.picHome.Image = Global.ASCOM.BrewSpin.My.Resources.Resources.grayLightOff
        Me.picHome.InitialImage = Nothing
        Me.picHome.Location = New System.Drawing.Point(12, 89)
        Me.picHome.Name = "picHome"
        Me.picHome.Size = New System.Drawing.Size(41, 41)
        Me.picHome.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picHome.TabIndex = 17
        Me.picHome.TabStop = False
        Me.ToolTip1.SetToolTip(Me.picHome, "At Home")
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(44, 13)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Cur Pos"
        '
        'btnSlewWest
        '
        Me.btnSlewWest.Location = New System.Drawing.Point(104, 183)
        Me.btnSlewWest.Name = "btnSlewWest"
        Me.btnSlewWest.Size = New System.Drawing.Size(75, 23)
        Me.btnSlewWest.TabIndex = 15
        Me.btnSlewWest.Text = "Slew West"
        Me.btnSlewWest.UseVisualStyleBackColor = True
        '
        'btnSlewEast
        '
        Me.btnSlewEast.Location = New System.Drawing.Point(12, 183)
        Me.btnSlewEast.Name = "btnSlewEast"
        Me.btnSlewEast.Size = New System.Drawing.Size(75, 23)
        Me.btnSlewEast.TabIndex = 14
        Me.btnSlewEast.Text = "Slew East"
        Me.btnSlewEast.UseVisualStyleBackColor = True
        '
        'btnHalt
        '
        Me.btnHalt.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnHalt.ForeColor = System.Drawing.Color.Red
        Me.btnHalt.Location = New System.Drawing.Point(170, 53)
        Me.btnHalt.Name = "btnHalt"
        Me.btnHalt.Size = New System.Drawing.Size(87, 26)
        Me.btnHalt.TabIndex = 13
        Me.btnHalt.Text = "Halt"
        Me.btnHalt.UseVisualStyleBackColor = True
        '
        'btnTest
        '
        Me.btnTest.Location = New System.Drawing.Point(305, 50)
        Me.btnTest.Name = "btnTest"
        Me.btnTest.Size = New System.Drawing.Size(75, 23)
        Me.btnTest.TabIndex = 12
        Me.btnTest.Text = "Test"
        Me.btnTest.UseVisualStyleBackColor = True
        '
        'txtTargetAz
        '
        Me.txtTargetAz.Location = New System.Drawing.Point(69, 53)
        Me.txtTargetAz.Name = "txtTargetAz"
        Me.txtTargetAz.Size = New System.Drawing.Size(46, 20)
        Me.txtTargetAz.TabIndex = 11
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.picCountTics)
        Me.TabPage2.Controls.Add(Me.lblParkAz)
        Me.TabPage2.Controls.Add(Me.Label5)
        Me.TabPage2.Controls.Add(Me.Label3)
        Me.TabPage2.Controls.Add(Me.Label4)
        Me.TabPage2.Controls.Add(Me.lblOffsetAz)
        Me.TabPage2.Controls.Add(Me.Label2)
        Me.TabPage2.Controls.Add(Me.buttonChoose)
        Me.TabPage2.Controls.Add(Me.txtDesiredAz)
        Me.TabPage2.Controls.Add(Me.btnSyncInstr)
        Me.TabPage2.Controls.Add(Me.btnSync)
        Me.TabPage2.Controls.Add(Me.lblBlockCount)
        Me.TabPage2.Controls.Add(Me.btnCountBlocks)
        Me.TabPage2.Controls.Add(Me.btnSetPark)
        Me.TabPage2.Controls.Add(Me.labelDriverId)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(475, 214)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Configuration"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'picCountTics
        '
        Me.picCountTics.Image = Global.ASCOM.BrewSpin.My.Resources.Resources.Blaulicht3
        Me.picCountTics.InitialImage = Nothing
        Me.picCountTics.Location = New System.Drawing.Point(90, 32)
        Me.picCountTics.Name = "picCountTics"
        Me.picCountTics.Size = New System.Drawing.Size(41, 41)
        Me.picCountTics.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picCountTics.TabIndex = 19
        Me.picCountTics.TabStop = False
        Me.ToolTip1.SetToolTip(Me.picCountTics, "Slewing Active")
        Me.picCountTics.Visible = False
        '
        'lblParkAz
        '
        Me.lblParkAz.AutoSize = True
        Me.lblParkAz.Location = New System.Drawing.Point(344, 57)
        Me.lblParkAz.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblParkAz.Name = "lblParkAz"
        Me.lblParkAz.Size = New System.Drawing.Size(13, 13)
        Me.lblParkAz.TabIndex = 13
        Me.lblParkAz.Text = "0"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(220, 57)
        Me.Label5.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(109, 13)
        Me.Label5.TabIndex = 12
        Me.Label5.Text = "Current Park Position:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.HotTrack
        Me.Label3.Location = New System.Drawing.Point(98, 3)
        Me.Label3.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(106, 20)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "Utility Tasks"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(2, 105)
        Me.Label4.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(155, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Telescope Requested Azimuth:"
        '
        'lblOffsetAz
        '
        Me.lblOffsetAz.AutoSize = True
        Me.lblOffsetAz.Location = New System.Drawing.Point(344, 105)
        Me.lblOffsetAz.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.lblOffsetAz.Name = "lblOffsetAz"
        Me.lblOffsetAz.Size = New System.Drawing.Size(13, 13)
        Me.lblOffsetAz.TabIndex = 9
        Me.lblOffsetAz.Text = "0"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(220, 105)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(115, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Current Azimuth Offset:"
        '
        'buttonChoose
        '
        Me.buttonChoose.Location = New System.Drawing.Point(10, 175)
        Me.buttonChoose.Name = "buttonChoose"
        Me.buttonChoose.Size = New System.Drawing.Size(72, 23)
        Me.buttonChoose.TabIndex = 7
        Me.buttonChoose.Text = "Choose..."
        Me.buttonChoose.UseVisualStyleBackColor = True
        '
        'txtDesiredAz
        '
        Me.txtDesiredAz.Location = New System.Drawing.Point(164, 102)
        Me.txtDesiredAz.Name = "txtDesiredAz"
        Me.txtDesiredAz.Size = New System.Drawing.Size(47, 20)
        Me.txtDesiredAz.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.txtDesiredAz, "Enter the telescope generated Azimuth for the dome (See Instructions)")
        '
        'btnSyncInstr
        '
        Me.btnSyncInstr.BackColor = System.Drawing.Color.Transparent
        Me.btnSyncInstr.Location = New System.Drawing.Point(222, 127)
        Me.btnSyncInstr.Name = "btnSyncInstr"
        Me.btnSyncInstr.Size = New System.Drawing.Size(100, 22)
        Me.btnSyncInstr.TabIndex = 4
        Me.btnSyncInstr.Text = "Sync Instructions"
        Me.ToolTip1.SetToolTip(Me.btnSyncInstr, "Instructions for performing the Sync Position process")
        Me.btnSyncInstr.UseVisualStyleBackColor = False
        '
        'btnSync
        '
        Me.btnSync.BackColor = System.Drawing.Color.Transparent
        Me.btnSync.Location = New System.Drawing.Point(101, 127)
        Me.btnSync.Name = "btnSync"
        Me.btnSync.Size = New System.Drawing.Size(89, 22)
        Me.btnSync.TabIndex = 3
        Me.btnSync.Text = "Sync Position"
        Me.ToolTip1.SetToolTip(Me.btnSync, "Sync the Telescope and dome positions." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Click Sync Instructions for how to do t" &
        "his process.")
        Me.btnSync.UseVisualStyleBackColor = False
        '
        'lblBlockCount
        '
        Me.lblBlockCount.AutoSize = True
        Me.lblBlockCount.Location = New System.Drawing.Point(8, 57)
        Me.lblBlockCount.Name = "lblBlockCount"
        Me.lblBlockCount.Size = New System.Drawing.Size(13, 13)
        Me.lblBlockCount.TabIndex = 2
        Me.lblBlockCount.Text = "0"
        Me.ToolTip1.SetToolTip(Me.lblBlockCount, "Current value for Total Tics per dome rotation")
        '
        'btnCountBlocks
        '
        Me.btnCountBlocks.BackColor = System.Drawing.Color.Transparent
        Me.btnCountBlocks.Location = New System.Drawing.Point(10, 32)
        Me.btnCountBlocks.Name = "btnCountBlocks"
        Me.btnCountBlocks.Size = New System.Drawing.Size(74, 22)
        Me.btnCountBlocks.TabIndex = 1
        Me.btnCountBlocks.Text = "Count Tics"
        Me.ToolTip1.SetToolTip(Me.btnCountBlocks, resources.GetString("btnCountBlocks.ToolTip"))
        Me.btnCountBlocks.UseVisualStyleBackColor = False
        '
        'btnSetPark
        '
        Me.btnSetPark.Location = New System.Drawing.Point(222, 32)
        Me.btnSetPark.Name = "btnSetPark"
        Me.btnSetPark.Size = New System.Drawing.Size(74, 22)
        Me.btnSetPark.TabIndex = 0
        Me.btnSetPark.Text = "SetPark"
        Me.ToolTip1.SetToolTip(Me.btnSetPark, "Set the current Dome position as the Park position")
        Me.btnSetPark.UseVisualStyleBackColor = True
        '
        'labelDriverId
        '
        Me.labelDriverId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.labelDriverId.Cursor = System.Windows.Forms.Cursors.Default
        Me.labelDriverId.DataBindings.Add(New System.Windows.Forms.Binding("Text", Global.ASCOM.BrewSpin.My.MySettings.Default, "DriverId", True, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged))
        Me.labelDriverId.ForeColor = System.Drawing.SystemColors.WindowText
        Me.labelDriverId.Location = New System.Drawing.Point(88, 177)
        Me.labelDriverId.Name = "labelDriverId"
        Me.labelDriverId.Size = New System.Drawing.Size(291, 21)
        Me.labelDriverId.TabIndex = 6
        Me.labelDriverId.Text = Global.ASCOM.BrewSpin.My.MySettings.Default.DriverId
        Me.labelDriverId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'txtLog
        '
        Me.txtLog.BackColor = System.Drawing.SystemColors.Window
        Me.txtLog.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtLog.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLog.Location = New System.Drawing.Point(0, 240)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(399, 203)
        Me.txtLog.TabIndex = 11
        '
        'Timer1
        '
        Me.Timer1.Interval = 500
        '
        'Panel1
        '
        Me.Panel1.AutoSize = True
        Me.Panel1.Controls.Add(Me.txtLog)
        Me.Panel1.Controls.Add(Me.TabControl1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(399, 443)
        Me.Panel1.TabIndex = 13
        '
        'CheckShutterTimer
        '
        Me.CheckShutterTimer.Interval = 4000
        '
        'BrewSpinControlForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Window
        Me.ClientSize = New System.Drawing.Size(399, 443)
        Me.Controls.Add(Me.Panel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "BrewSpinControlForm"
        Me.Text = "BrewSpin Control"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        CType(Me.picArduinoAvail, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picLowerClosedSw, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picLowerOpenSw, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picUpperClosedSw, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picUpperOpenSw, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picShutter, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picPark, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picSlewing, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picHome, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage2.PerformLayout()
        CType(Me.picCountTics, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents buttonConnect As System.Windows.Forms.Button
    Friend WithEvents btnGoToAz As System.Windows.Forms.Button
    Friend WithEvents lblCurrentAz As System.Windows.Forms.Label
    Friend WithEvents btnHome As System.Windows.Forms.Button
    Friend WithEvents btnPark As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents lblBlockCount As System.Windows.Forms.Label
    Friend WithEvents btnCountBlocks As System.Windows.Forms.Button
    Friend WithEvents btnSetPark As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents txtLog As System.Windows.Forms.TextBox
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents txtTargetAz As System.Windows.Forms.TextBox
    Friend WithEvents btnSyncInstr As System.Windows.Forms.Button
    Friend WithEvents btnSync As System.Windows.Forms.Button
    Friend WithEvents txtDesiredAz As System.Windows.Forms.TextBox
    Friend WithEvents btnTest As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnHalt As System.Windows.Forms.Button
    Private WithEvents buttonChoose As System.Windows.Forms.Button
    Private WithEvents labelDriverId As System.Windows.Forms.Label
    Friend WithEvents picHome As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnSlewWest As System.Windows.Forms.Button
    Friend WithEvents btnSlewEast As System.Windows.Forms.Button
    Friend WithEvents picSlewing As System.Windows.Forms.PictureBox
    Friend WithEvents picPark As System.Windows.Forms.PictureBox
    Friend WithEvents Label4 As Label
    Friend WithEvents lblOffsetAz As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents lblParkAz As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents lblBlockPos As Label
    Friend WithEvents picCountTics As PictureBox
    Friend WithEvents btnCloseShutter As Button
    Friend WithEvents btnOpenShutter As Button
    Friend WithEvents picShutter As PictureBox
    Friend WithEvents lblShutterStatus As Label
    Friend WithEvents CheckShutterTimer As Timer
    Friend WithEvents picLowerClosedSw As PictureBox
    Friend WithEvents picLowerOpenSw As PictureBox
    Friend WithEvents picUpperClosedSw As PictureBox
    Friend WithEvents picUpperOpenSw As PictureBox
    Friend WithEvents lblTemperature As Label
    Friend WithEvents lblPowerVoltage As Label
    Friend WithEvents picArduinoAvail As PictureBox
End Class
