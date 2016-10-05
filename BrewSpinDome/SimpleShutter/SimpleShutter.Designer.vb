<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SimpleShutter
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SimpleShutter))
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tabOperation = New System.Windows.Forms.TabPage()
        Me.OperationsTableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel()
        Me.btnCloseBoth = New System.Windows.Forms.Button()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.btnOpenUpper = New System.Windows.Forms.Button()
        Me.btnOpenBoth = New System.Windows.Forms.Button()
        Me.picUpperLight = New System.Windows.Forms.PictureBox()
        Me.picHeartBeat = New System.Windows.Forms.PictureBox()
        Me.btnOpenLower = New System.Windows.Forms.Button()
        Me.btnCloseUpper = New System.Windows.Forms.Button()
        Me.btnVent = New System.Windows.Forms.Button()
        Me.btnCloseLower = New System.Windows.Forms.Button()
        Me.picLowerLight = New System.Windows.Forms.PictureBox()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.lblUpperCurrent = New System.Windows.Forms.Label()
        Me.lblLowerCurrent = New System.Windows.Forms.Label()
        Me.lblVoltage = New System.Windows.Forms.Label()
        Me.lblTemperature = New System.Windows.Forms.Label()
        Me.btnTrace = New System.Windows.Forms.Button()
        Me.btnAbortMotors = New System.Windows.Forms.Button()
        Me.tabOptions = New System.Windows.Forms.TabPage()
        Me.txtListenPort = New System.Windows.Forms.MaskedTextBox()
        Me.txtIPAddress = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblDriverInfo = New System.Windows.Forms.Label()
        Me.TabArduino = New System.Windows.Forms.TabPage()
        Me.lblConfigApply = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtLowerMotorSpeed = New System.Windows.Forms.MaskedTextBox()
        Me.txtUpperMotorSpeed = New System.Windows.Forms.MaskedTextBox()
        Me.txtMotorTimerMsec = New System.Windows.Forms.MaskedTextBox()
        Me.txtHeartbeatTimerMsec = New System.Windows.Forms.MaskedTextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtLowerShutterTimeout = New System.Windows.Forms.MaskedTextBox()
        Me.txtUpperShutterTimeout = New System.Windows.Forms.MaskedTextBox()
        Me.txtVentTime = New System.Windows.Forms.MaskedTextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.chkReverseLowerMotor = New System.Windows.Forms.CheckBox()
        Me.chkReverseUpperMotor = New System.Windows.Forms.CheckBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnArduinoApply = New System.Windows.Forms.Button()
        Me.toolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.motorTimer = New System.Windows.Forms.Timer(Me.components)
        Me.heartBeatTimer = New System.Windows.Forms.Timer(Me.components)
        Me.ConsoleTimer = New System.Windows.Forms.Timer(Me.components)
        Me.TabControl1.SuspendLayout()
        Me.tabOperation.SuspendLayout()
        Me.OperationsTableLayoutPanel.SuspendLayout()
        Me.TableLayoutPanel4.SuspendLayout()
        CType(Me.picUpperLight, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picHeartBeat, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picLowerLight, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.tabOptions.SuspendLayout()
        Me.TabArduino.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tabOperation)
        Me.TabControl1.Controls.Add(Me.tabOptions)
        Me.TabControl1.Controls.Add(Me.TabArduino)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(453, 350)
        Me.TabControl1.TabIndex = 8
        '
        'tabOperation
        '
        Me.tabOperation.Controls.Add(Me.OperationsTableLayoutPanel)
        Me.tabOperation.Location = New System.Drawing.Point(4, 25)
        Me.tabOperation.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.tabOperation.Name = "tabOperation"
        Me.tabOperation.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.tabOperation.Size = New System.Drawing.Size(445, 321)
        Me.tabOperation.TabIndex = 0
        Me.tabOperation.Text = "Shutter"
        Me.tabOperation.UseVisualStyleBackColor = True
        '
        'OperationsTableLayoutPanel
        '
        Me.OperationsTableLayoutPanel.ColumnCount = 4
        Me.OperationsTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.OperationsTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.OperationsTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.OperationsTableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.OperationsTableLayoutPanel.Controls.Add(Me.TableLayoutPanel4, 2, 0)
        Me.OperationsTableLayoutPanel.Controls.Add(Me.lblStatus, 0, 6)
        Me.OperationsTableLayoutPanel.Controls.Add(Me.btnOpenUpper, 1, 1)
        Me.OperationsTableLayoutPanel.Controls.Add(Me.btnOpenBoth, 1, 0)
        Me.OperationsTableLayoutPanel.Controls.Add(Me.picUpperLight, 0, 1)
        Me.OperationsTableLayoutPanel.Controls.Add(Me.picHeartBeat, 0, 0)
        Me.OperationsTableLayoutPanel.Controls.Add(Me.btnOpenLower, 1, 3)
        Me.OperationsTableLayoutPanel.Controls.Add(Me.btnCloseUpper, 1, 2)
        Me.OperationsTableLayoutPanel.Controls.Add(Me.btnVent, 2, 1)
        Me.OperationsTableLayoutPanel.Controls.Add(Me.btnCloseLower, 1, 4)
        Me.OperationsTableLayoutPanel.Controls.Add(Me.picLowerLight, 0, 3)
        Me.OperationsTableLayoutPanel.Controls.Add(Me.TableLayoutPanel2, 3, 1)
        Me.OperationsTableLayoutPanel.Controls.Add(Me.btnTrace, 3, 0)
        Me.OperationsTableLayoutPanel.Controls.Add(Me.btnAbortMotors, 3, 4)
        Me.OperationsTableLayoutPanel.Location = New System.Drawing.Point(23, 6)
        Me.OperationsTableLayoutPanel.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.OperationsTableLayoutPanel.Name = "OperationsTableLayoutPanel"
        Me.OperationsTableLayoutPanel.RowCount = 6
        Me.OperationsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39.0!))
        Me.OperationsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39.0!))
        Me.OperationsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39.0!))
        Me.OperationsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39.0!))
        Me.OperationsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39.0!))
        Me.OperationsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10.0!))
        Me.OperationsTableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33.0!))
        Me.OperationsTableLayoutPanel.Size = New System.Drawing.Size(419, 306)
        Me.OperationsTableLayoutPanel.TabIndex = 1
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.ColumnCount = 1
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel4.Controls.Add(Me.btnCloseBoth, 0, 0)
        Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(211, 2)
        Me.TableLayoutPanel4.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 1
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(98, 35)
        Me.TableLayoutPanel4.TabIndex = 11
        '
        'btnCloseBoth
        '
        Me.btnCloseBoth.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCloseBoth.Location = New System.Drawing.Point(3, 2)
        Me.btnCloseBoth.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnCloseBoth.Name = "btnCloseBoth"
        Me.btnCloseBoth.Size = New System.Drawing.Size(92, 31)
        Me.btnCloseBoth.TabIndex = 9
        Me.btnCloseBoth.Text = "Close Both"
        Me.btnCloseBoth.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OperationsTableLayoutPanel.SetColumnSpan(Me.lblStatus, 4)
        Me.lblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.Location = New System.Drawing.Point(3, 205)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(413, 34)
        Me.lblStatus.TabIndex = 2
        Me.lblStatus.Text = "Status"
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'btnOpenUpper
        '
        Me.btnOpenUpper.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOpenUpper.Location = New System.Drawing.Point(107, 41)
        Me.btnOpenUpper.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnOpenUpper.Name = "btnOpenUpper"
        Me.btnOpenUpper.Size = New System.Drawing.Size(98, 35)
        Me.btnOpenUpper.TabIndex = 0
        Me.btnOpenUpper.Text = "Open Upper"
        Me.btnOpenUpper.UseVisualStyleBackColor = True
        '
        'btnOpenBoth
        '
        Me.btnOpenBoth.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOpenBoth.BackColor = System.Drawing.Color.Transparent
        Me.btnOpenBoth.Location = New System.Drawing.Point(107, 2)
        Me.btnOpenBoth.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnOpenBoth.Name = "btnOpenBoth"
        Me.btnOpenBoth.Size = New System.Drawing.Size(98, 35)
        Me.btnOpenBoth.TabIndex = 8
        Me.btnOpenBoth.Text = "Open Both"
        Me.btnOpenBoth.UseVisualStyleBackColor = False
        '
        'picUpperLight
        '
        Me.picUpperLight.Image = Global.SimpleShutter.My.Resources.Resources.grayLightOff
        Me.picUpperLight.InitialImage = Global.SimpleShutter.My.Resources.Resources.YellowMidFrame1
        Me.picUpperLight.Location = New System.Drawing.Point(3, 41)
        Me.picUpperLight.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.picUpperLight.Name = "picUpperLight"
        Me.OperationsTableLayoutPanel.SetRowSpan(Me.picUpperLight, 2)
        Me.picUpperLight.Size = New System.Drawing.Size(98, 74)
        Me.picUpperLight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picUpperLight.TabIndex = 5
        Me.picUpperLight.TabStop = False
        Me.toolTip.SetToolTip(Me.picUpperLight, "Upper Shutter" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  Green - Closed" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  Blue - Open" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  Yellow - In between, partially " & _
        "open" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10))
        '
        'picHeartBeat
        '
        Me.picHeartBeat.ErrorImage = Global.SimpleShutter.My.Resources.Resources.lightOff
        Me.picHeartBeat.Image = Global.SimpleShutter.My.Resources.Resources.redLightOff
        Me.picHeartBeat.InitialImage = Global.SimpleShutter.My.Resources.Resources.yellowLightOff
        Me.picHeartBeat.Location = New System.Drawing.Point(3, 2)
        Me.picHeartBeat.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.picHeartBeat.Name = "picHeartBeat"
        Me.picHeartBeat.Size = New System.Drawing.Size(21, 22)
        Me.picHeartBeat.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.picHeartBeat.TabIndex = 12
        Me.picHeartBeat.TabStop = False
        Me.picHeartBeat.Tag = "Off"
        Me.toolTip.SetToolTip(Me.picHeartBeat, "Green when Arduino is connected;" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Red when comm failed" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Yellow while trying to co" & _
        "nnect" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Blue when motors are busy performing an operation")
        '
        'btnOpenLower
        '
        Me.btnOpenLower.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOpenLower.Location = New System.Drawing.Point(107, 119)
        Me.btnOpenLower.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnOpenLower.Name = "btnOpenLower"
        Me.btnOpenLower.Size = New System.Drawing.Size(98, 35)
        Me.btnOpenLower.TabIndex = 1
        Me.btnOpenLower.Text = "Open Lower"
        Me.btnOpenLower.UseVisualStyleBackColor = True
        '
        'btnCloseUpper
        '
        Me.btnCloseUpper.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCloseUpper.Location = New System.Drawing.Point(107, 80)
        Me.btnCloseUpper.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnCloseUpper.Name = "btnCloseUpper"
        Me.btnCloseUpper.Size = New System.Drawing.Size(98, 35)
        Me.btnCloseUpper.TabIndex = 3
        Me.btnCloseUpper.Text = "Close Upper"
        Me.btnCloseUpper.UseVisualStyleBackColor = True
        '
        'btnVent
        '
        Me.btnVent.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnVent.Location = New System.Drawing.Point(211, 41)
        Me.btnVent.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnVent.Name = "btnVent"
        Me.btnVent.Size = New System.Drawing.Size(98, 35)
        Me.btnVent.TabIndex = 7
        Me.btnVent.Text = "Vent Dome"
        Me.btnVent.UseVisualStyleBackColor = True
        '
        'btnCloseLower
        '
        Me.btnCloseLower.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCloseLower.Location = New System.Drawing.Point(107, 158)
        Me.btnCloseLower.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnCloseLower.Name = "btnCloseLower"
        Me.btnCloseLower.Size = New System.Drawing.Size(98, 35)
        Me.btnCloseLower.TabIndex = 2
        Me.btnCloseLower.Text = "Close Lower"
        Me.btnCloseLower.UseVisualStyleBackColor = True
        '
        'picLowerLight
        '
        Me.picLowerLight.Image = Global.SimpleShutter.My.Resources.Resources.grayLightOff
        Me.picLowerLight.Location = New System.Drawing.Point(3, 119)
        Me.picLowerLight.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.picLowerLight.Name = "picLowerLight"
        Me.OperationsTableLayoutPanel.SetRowSpan(Me.picLowerLight, 2)
        Me.picLowerLight.Size = New System.Drawing.Size(98, 74)
        Me.picLowerLight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picLowerLight.TabIndex = 7
        Me.picLowerLight.TabStop = False
        Me.toolTip.SetToolTip(Me.picLowerLight, "Lower Shutter" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  Green - Closed" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  Blue - Open" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "  Yellow - In between, partially " & _
        "open")
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.lblUpperCurrent, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.lblLowerCurrent, 0, 1)
        Me.TableLayoutPanel2.Controls.Add(Me.lblVoltage, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.lblTemperature, 1, 1)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(323, 41)
        Me.TableLayoutPanel2.Margin = New System.Windows.Forms.Padding(11, 2, 3, 2)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 2
        Me.OperationsTableLayoutPanel.SetRowSpan(Me.TableLayoutPanel2, 3)
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(93, 75)
        Me.TableLayoutPanel2.TabIndex = 5
        '
        'lblUpperCurrent
        '
        Me.lblUpperCurrent.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblUpperCurrent.AutoSize = True
        Me.lblUpperCurrent.Location = New System.Drawing.Point(3, 10)
        Me.lblUpperCurrent.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.lblUpperCurrent.Name = "lblUpperCurrent"
        Me.lblUpperCurrent.Size = New System.Drawing.Size(36, 17)
        Me.lblUpperCurrent.TabIndex = 16
        Me.lblUpperCurrent.Text = "0.00"
        Me.lblUpperCurrent.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.toolTip.SetToolTip(Me.lblUpperCurrent, "Upper Shutter motor current")
        '
        'lblLowerCurrent
        '
        Me.lblLowerCurrent.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblLowerCurrent.AutoSize = True
        Me.lblLowerCurrent.Location = New System.Drawing.Point(3, 47)
        Me.lblLowerCurrent.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.lblLowerCurrent.Name = "lblLowerCurrent"
        Me.lblLowerCurrent.Size = New System.Drawing.Size(36, 17)
        Me.lblLowerCurrent.TabIndex = 15
        Me.lblLowerCurrent.Text = "0.00"
        Me.lblLowerCurrent.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.toolTip.SetToolTip(Me.lblLowerCurrent, "Lower Shutter motor current")
        '
        'lblVoltage
        '
        Me.lblVoltage.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblVoltage.AutoSize = True
        Me.lblVoltage.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblVoltage.Location = New System.Drawing.Point(57, 10)
        Me.lblVoltage.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.lblVoltage.Name = "lblVoltage"
        Me.lblVoltage.Padding = New System.Windows.Forms.Padding(0, 0, 5, 0)
        Me.lblVoltage.Size = New System.Drawing.Size(42, 17)
        Me.lblVoltage.TabIndex = 18
        Me.lblVoltage.Text = "0.0V"
        Me.lblVoltage.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.toolTip.SetToolTip(Me.lblVoltage, "Power Supply Voltage")
        '
        'lblTemperature
        '
        Me.lblTemperature.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.lblTemperature.AutoSize = True
        Me.lblTemperature.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.lblTemperature.Location = New System.Drawing.Point(70, 47)
        Me.lblTemperature.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.lblTemperature.Name = "lblTemperature"
        Me.lblTemperature.Padding = New System.Windows.Forms.Padding(0, 0, 5, 0)
        Me.lblTemperature.Size = New System.Drawing.Size(29, 17)
        Me.lblTemperature.TabIndex = 19
        Me.lblTemperature.Text = "0F"
        Me.lblTemperature.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.toolTip.SetToolTip(Me.lblTemperature, "Temperature (deg F)")
        '
        'btnTrace
        '
        Me.btnTrace.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnTrace.AutoEllipsis = True
        Me.btnTrace.Location = New System.Drawing.Point(315, 2)
        Me.btnTrace.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnTrace.Name = "btnTrace"
        Me.btnTrace.Size = New System.Drawing.Size(101, 35)
        Me.btnTrace.TabIndex = 2
        Me.btnTrace.Text = "Trace..."
        Me.btnTrace.UseVisualStyleBackColor = True
        '
        'btnAbortMotors
        '
        Me.btnAbortMotors.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAbortMotors.AutoSize = True
        Me.btnAbortMotors.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.btnAbortMotors.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAbortMotors.ForeColor = System.Drawing.Color.Red
        Me.btnAbortMotors.Location = New System.Drawing.Point(315, 158)
        Me.btnAbortMotors.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnAbortMotors.Name = "btnAbortMotors"
        Me.btnAbortMotors.Size = New System.Drawing.Size(101, 35)
        Me.btnAbortMotors.TabIndex = 6
        Me.btnAbortMotors.Text = "Halt Motors"
        Me.btnAbortMotors.UseVisualStyleBackColor = True
        '
        'tabOptions
        '
        Me.tabOptions.Controls.Add(Me.txtListenPort)
        Me.tabOptions.Controls.Add(Me.txtIPAddress)
        Me.tabOptions.Controls.Add(Me.Label1)
        Me.tabOptions.Controls.Add(Me.lblDriverInfo)
        Me.tabOptions.Location = New System.Drawing.Point(4, 25)
        Me.tabOptions.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.tabOptions.Name = "tabOptions"
        Me.tabOptions.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.tabOptions.Size = New System.Drawing.Size(445, 321)
        Me.tabOptions.TabIndex = 1
        Me.tabOptions.Text = "Options"
        Me.tabOptions.UseVisualStyleBackColor = True
        '
        'txtListenPort
        '
        Me.txtListenPort.Location = New System.Drawing.Point(191, 44)
        Me.txtListenPort.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtListenPort.Mask = "00000"
        Me.txtListenPort.Name = "txtListenPort"
        Me.txtListenPort.Size = New System.Drawing.Size(72, 22)
        Me.txtListenPort.TabIndex = 12
        Me.txtListenPort.Text = "1552"
        Me.toolTip.SetToolTip(Me.txtListenPort, "Listen Port for TCP communication")
        Me.txtListenPort.ValidatingType = GetType(Integer)
        '
        'txtIPAddress
        '
        Me.txtIPAddress.Location = New System.Drawing.Point(191, 14)
        Me.txtIPAddress.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtIPAddress.Name = "txtIPAddress"
        Me.txtIPAddress.Size = New System.Drawing.Size(163, 22)
        Me.txtIPAddress.TabIndex = 10
        Me.txtIPAddress.Text = "192.168.1.30"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 47)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 17)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "Listen Port"
        '
        'lblDriverInfo
        '
        Me.lblDriverInfo.AutoSize = True
        Me.lblDriverInfo.Location = New System.Drawing.Point(8, 17)
        Me.lblDriverInfo.Name = "lblDriverInfo"
        Me.lblDriverInfo.Size = New System.Drawing.Size(150, 17)
        Me.lblDriverInfo.TabIndex = 7
        Me.lblDriverInfo.Text = "IP Address for Arduino"
        '
        'TabArduino
        '
        Me.TabArduino.Controls.Add(Me.lblConfigApply)
        Me.TabArduino.Controls.Add(Me.Label8)
        Me.TabArduino.Controls.Add(Me.Label7)
        Me.TabArduino.Controls.Add(Me.txtLowerMotorSpeed)
        Me.TabArduino.Controls.Add(Me.txtUpperMotorSpeed)
        Me.TabArduino.Controls.Add(Me.txtMotorTimerMsec)
        Me.TabArduino.Controls.Add(Me.txtHeartbeatTimerMsec)
        Me.TabArduino.Controls.Add(Me.Label6)
        Me.TabArduino.Controls.Add(Me.Label5)
        Me.TabArduino.Controls.Add(Me.txtLowerShutterTimeout)
        Me.TabArduino.Controls.Add(Me.txtUpperShutterTimeout)
        Me.TabArduino.Controls.Add(Me.txtVentTime)
        Me.TabArduino.Controls.Add(Me.Label4)
        Me.TabArduino.Controls.Add(Me.chkReverseLowerMotor)
        Me.TabArduino.Controls.Add(Me.chkReverseUpperMotor)
        Me.TabArduino.Controls.Add(Me.Label3)
        Me.TabArduino.Controls.Add(Me.Label2)
        Me.TabArduino.Controls.Add(Me.btnArduinoApply)
        Me.TabArduino.Location = New System.Drawing.Point(4, 25)
        Me.TabArduino.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabArduino.Name = "TabArduino"
        Me.TabArduino.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.TabArduino.Size = New System.Drawing.Size(445, 321)
        Me.TabArduino.TabIndex = 2
        Me.TabArduino.Text = "Configuration"
        Me.TabArduino.UseVisualStyleBackColor = True
        '
        'lblConfigApply
        '
        Me.lblConfigApply.AutoSize = True
        Me.lblConfigApply.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.lblConfigApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 13.8!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblConfigApply.ForeColor = System.Drawing.Color.ForestGreen
        Me.lblConfigApply.Location = New System.Drawing.Point(8, 257)
        Me.lblConfigApply.Name = "lblConfigApply"
        Me.lblConfigApply.Size = New System.Drawing.Size(20, 29)
        Me.lblConfigApply.TabIndex = 22
        Me.lblConfigApply.Text = " "
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(341, 130)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(96, 17)
        Me.Label8.TabIndex = 21
        Me.Label8.Text = "Lower Shutter"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(225, 130)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(97, 17)
        Me.Label7.TabIndex = 20
        Me.Label7.Text = "Upper Shutter"
        '
        'txtLowerMotorSpeed
        '
        Me.txtLowerMotorSpeed.Enabled = False
        Me.txtLowerMotorSpeed.Location = New System.Drawing.Point(344, 188)
        Me.txtLowerMotorSpeed.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtLowerMotorSpeed.Mask = "0000"
        Me.txtLowerMotorSpeed.Name = "txtLowerMotorSpeed"
        Me.txtLowerMotorSpeed.Size = New System.Drawing.Size(68, 22)
        Me.txtLowerMotorSpeed.TabIndex = 19
        Me.txtLowerMotorSpeed.Text = "1023"
        Me.txtLowerMotorSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.toolTip.SetToolTip(Me.txtLowerMotorSpeed, "Time to check running motor (msec)")
        '
        'txtUpperMotorSpeed
        '
        Me.txtUpperMotorSpeed.Enabled = False
        Me.txtUpperMotorSpeed.Location = New System.Drawing.Point(253, 188)
        Me.txtUpperMotorSpeed.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtUpperMotorSpeed.Mask = "0000"
        Me.txtUpperMotorSpeed.Name = "txtUpperMotorSpeed"
        Me.txtUpperMotorSpeed.Size = New System.Drawing.Size(68, 22)
        Me.txtUpperMotorSpeed.TabIndex = 18
        Me.txtUpperMotorSpeed.Text = "1023"
        Me.txtUpperMotorSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.toolTip.SetToolTip(Me.txtUpperMotorSpeed, "Time to check running motor (msec)")
        '
        'txtMotorTimerMsec
        '
        Me.txtMotorTimerMsec.Location = New System.Drawing.Point(269, 43)
        Me.txtMotorTimerMsec.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtMotorTimerMsec.Mask = "0000"
        Me.txtMotorTimerMsec.Name = "txtMotorTimerMsec"
        Me.txtMotorTimerMsec.Size = New System.Drawing.Size(68, 22)
        Me.txtMotorTimerMsec.TabIndex = 17
        Me.txtMotorTimerMsec.Text = "3000"
        Me.txtMotorTimerMsec.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.toolTip.SetToolTip(Me.txtMotorTimerMsec, "Time to check running motor (msec)")
        '
        'txtHeartbeatTimerMsec
        '
        Me.txtHeartbeatTimerMsec.Location = New System.Drawing.Point(269, 15)
        Me.txtHeartbeatTimerMsec.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtHeartbeatTimerMsec.Mask = "0000"
        Me.txtHeartbeatTimerMsec.Name = "txtHeartbeatTimerMsec"
        Me.txtHeartbeatTimerMsec.Size = New System.Drawing.Size(68, 22)
        Me.txtHeartbeatTimerMsec.TabIndex = 16
        Me.txtHeartbeatTimerMsec.Text = "3000"
        Me.txtHeartbeatTimerMsec.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.toolTip.SetToolTip(Me.txtHeartbeatTimerMsec, "Time to check status (msec)")
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(8, 46)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(221, 17)
        Me.Label6.TabIndex = 15
        Me.Label6.Text = "Check running motor timer (msec)"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(8, 18)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(158, 17)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = "Heartbeat Timer (msec)"
        '
        'txtLowerShutterTimeout
        '
        Me.txtLowerShutterTimeout.Location = New System.Drawing.Point(344, 160)
        Me.txtLowerShutterTimeout.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtLowerShutterTimeout.Mask = "000000"
        Me.txtLowerShutterTimeout.Name = "txtLowerShutterTimeout"
        Me.txtLowerShutterTimeout.Size = New System.Drawing.Size(68, 22)
        Me.txtLowerShutterTimeout.TabIndex = 13
        Me.txtLowerShutterTimeout.Text = "15000"
        Me.txtLowerShutterTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.toolTip.SetToolTip(Me.txtLowerShutterTimeout, "Timeout for Lower Shutter Motor")
        '
        'txtUpperShutterTimeout
        '
        Me.txtUpperShutterTimeout.Location = New System.Drawing.Point(253, 160)
        Me.txtUpperShutterTimeout.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtUpperShutterTimeout.Mask = "000000"
        Me.txtUpperShutterTimeout.Name = "txtUpperShutterTimeout"
        Me.txtUpperShutterTimeout.Size = New System.Drawing.Size(68, 22)
        Me.txtUpperShutterTimeout.TabIndex = 12
        Me.txtUpperShutterTimeout.Text = "45000"
        Me.txtUpperShutterTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.toolTip.SetToolTip(Me.txtUpperShutterTimeout, "Timeout for Upper Shutter Motor")
        '
        'txtVentTime
        '
        Me.txtVentTime.Location = New System.Drawing.Point(269, 71)
        Me.txtVentTime.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtVentTime.Mask = "000000"
        Me.txtVentTime.Name = "txtVentTime"
        Me.txtVentTime.Size = New System.Drawing.Size(68, 22)
        Me.txtVentTime.TabIndex = 11
        Me.txtVentTime.Text = "10000"
        Me.txtVentTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(8, 191)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(148, 17)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Motor Speed (0-1023)"
        '
        'chkReverseLowerMotor
        '
        Me.chkReverseLowerMotor.AutoSize = True
        Me.chkReverseLowerMotor.Location = New System.Drawing.Point(344, 217)
        Me.chkReverseLowerMotor.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.chkReverseLowerMotor.Name = "chkReverseLowerMotor"
        Me.chkReverseLowerMotor.Size = New System.Drawing.Size(87, 21)
        Me.chkReverseLowerMotor.TabIndex = 5
        Me.chkReverseLowerMotor.Text = "Reverse "
        Me.chkReverseLowerMotor.UseVisualStyleBackColor = True
        '
        'chkReverseUpperMotor
        '
        Me.chkReverseUpperMotor.AutoSize = True
        Me.chkReverseUpperMotor.Location = New System.Drawing.Point(253, 217)
        Me.chkReverseUpperMotor.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.chkReverseUpperMotor.Name = "chkReverseUpperMotor"
        Me.chkReverseUpperMotor.Size = New System.Drawing.Size(83, 21)
        Me.chkReverseUpperMotor.TabIndex = 4
        Me.chkReverseUpperMotor.Text = "Reverse"
        Me.chkReverseUpperMotor.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(8, 162)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(196, 17)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Shutter Motor Timeout (msec)"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 74)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(200, 17)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Time to run Vent Motor (msec)"
        '
        'btnArduinoApply
        '
        Me.btnArduinoApply.Location = New System.Drawing.Point(355, 18)
        Me.btnArduinoApply.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnArduinoApply.Name = "btnArduinoApply"
        Me.btnArduinoApply.Size = New System.Drawing.Size(76, 28)
        Me.btnArduinoApply.TabIndex = 0
        Me.btnArduinoApply.Text = "Apply"
        Me.btnArduinoApply.UseVisualStyleBackColor = True
        '
        'motorTimer
        '
        Me.motorTimer.Interval = 500
        '
        'heartBeatTimer
        '
        Me.heartBeatTimer.Interval = 3000
        '
        'ConsoleTimer
        '
        Me.ConsoleTimer.Interval = 5000
        '
        'SimpleShutter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(453, 350)
        Me.Controls.Add(Me.TabControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "SimpleShutter"
        Me.Text = "Brew Simple Shutter"
        Me.TabControl1.ResumeLayout(False)
        Me.tabOperation.ResumeLayout(False)
        Me.OperationsTableLayoutPanel.ResumeLayout(False)
        Me.OperationsTableLayoutPanel.PerformLayout()
        Me.TableLayoutPanel4.ResumeLayout(False)
        CType(Me.picUpperLight, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picHeartBeat, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picLowerLight, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.tabOptions.ResumeLayout(False)
        Me.tabOptions.PerformLayout()
        Me.TabArduino.ResumeLayout(False)
        Me.TabArduino.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabOptions As System.Windows.Forms.TabPage
    Friend WithEvents lblDriverInfo As System.Windows.Forms.Label
    Friend WithEvents tabOperation As System.Windows.Forms.TabPage
    Friend WithEvents OperationsTableLayoutPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents btnOpenBoth As System.Windows.Forms.Button
    Friend WithEvents btnCloseBoth As System.Windows.Forms.Button
    Friend WithEvents btnCloseLower As System.Windows.Forms.Button
    Friend WithEvents btnVent As System.Windows.Forms.Button
    Friend WithEvents picUpperLight As System.Windows.Forms.PictureBox
    Friend WithEvents TableLayoutPanel4 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents picLowerLight As System.Windows.Forms.PictureBox
    Friend WithEvents TableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents lblVoltage As System.Windows.Forms.Label
    Friend WithEvents lblUpperCurrent As System.Windows.Forms.Label
    Friend WithEvents lblLowerCurrent As System.Windows.Forms.Label
    Friend WithEvents picHeartBeat As System.Windows.Forms.PictureBox
    Friend WithEvents btnOpenUpper As System.Windows.Forms.Button
    Friend WithEvents btnCloseUpper As System.Windows.Forms.Button
    Friend WithEvents btnAbortMotors As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents btnOpenLower As System.Windows.Forms.Button
    Friend WithEvents txtIPAddress As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents toolTip As System.Windows.Forms.ToolTip
    Friend WithEvents motorTimer As System.Windows.Forms.Timer
    Friend WithEvents heartBeatTimer As System.Windows.Forms.Timer
    Friend WithEvents btnTrace As System.Windows.Forms.Button
    Friend WithEvents TabArduino As System.Windows.Forms.TabPage
    Friend WithEvents chkReverseLowerMotor As System.Windows.Forms.CheckBox
    Friend WithEvents chkReverseUpperMotor As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnArduinoApply As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblTemperature As System.Windows.Forms.Label
    Friend WithEvents txtVentTime As System.Windows.Forms.MaskedTextBox
    Friend WithEvents txtMotorTimerMsec As System.Windows.Forms.MaskedTextBox
    Friend WithEvents txtHeartbeatTimerMsec As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtLowerShutterTimeout As System.Windows.Forms.MaskedTextBox
    Friend WithEvents txtUpperShutterTimeout As System.Windows.Forms.MaskedTextBox
    Friend WithEvents txtListenPort As System.Windows.Forms.MaskedTextBox
    Friend WithEvents ConsoleTimer As System.Windows.Forms.Timer
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtLowerMotorSpeed As System.Windows.Forms.MaskedTextBox
    Friend WithEvents txtUpperMotorSpeed As System.Windows.Forms.MaskedTextBox
    Friend WithEvents lblConfigApply As System.Windows.Forms.Label

End Class
