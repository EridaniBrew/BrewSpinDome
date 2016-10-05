<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TestEnhanced
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
        Me.components = New System.ComponentModel.Container
        Me.txtTerm = New System.Windows.Forms.TextBox
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.CommPortToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.PortOpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.SettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ClearScreeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ClearScreenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.HexDisplayToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.ChooseTerminalFontToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Tx = New System.Windows.Forms.Label
        Me.Rx = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.FontDialog1 = New System.Windows.Forms.FontDialog
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SerialPort1 = New System.IO.Ports.SerialPort(Me.components)
        Me.MenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtTerm
        '
        Me.txtTerm.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtTerm.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTerm.Location = New System.Drawing.Point(0, 27)
        Me.txtTerm.Multiline = True
        Me.txtTerm.Name = "txtTerm"
        Me.txtTerm.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtTerm.Size = New System.Drawing.Size(685, 342)
        Me.txtTerm.TabIndex = 1
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.CommPortToolStripMenuItem, Me.ClearScreeToolStripMenuItem, Me.AboutToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(685, 24)
        Me.MenuStrip1.TabIndex = 3
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(92, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'CommPortToolStripMenuItem
        '
        Me.CommPortToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PortOpenToolStripMenuItem, Me.ToolStripSeparator1, Me.SettingsToolStripMenuItem})
        Me.CommPortToolStripMenuItem.Name = "CommPortToolStripMenuItem"
        Me.CommPortToolStripMenuItem.Size = New System.Drawing.Size(136, 20)
        Me.CommPortToolStripMenuItem.Text = "Communications Port"
        '
        'PortOpenToolStripMenuItem
        '
        Me.PortOpenToolStripMenuItem.Name = "PortOpenToolStripMenuItem"
        Me.PortOpenToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.PortOpenToolStripMenuItem.Text = "Port Open"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(149, 6)
        '
        'SettingsToolStripMenuItem
        '
        Me.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem"
        Me.SettingsToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.SettingsToolStripMenuItem.Text = "Settings"
        '
        'ClearScreeToolStripMenuItem
        '
        Me.ClearScreeToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ClearScreenToolStripMenuItem, Me.ToolStripSeparator2, Me.HexDisplayToolStripMenuItem, Me.ToolStripSeparator3, Me.ChooseTerminalFontToolStripMenuItem})
        Me.ClearScreeToolStripMenuItem.Name = "ClearScreeToolStripMenuItem"
        Me.ClearScreeToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.ClearScreeToolStripMenuItem.Text = "View"
        '
        'ClearScreenToolStripMenuItem
        '
        Me.ClearScreenToolStripMenuItem.Name = "ClearScreenToolStripMenuItem"
        Me.ClearScreenToolStripMenuItem.Size = New System.Drawing.Size(232, 22)
        Me.ClearScreenToolStripMenuItem.Text = "ClearScreen"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(229, 6)
        '
        'HexDisplayToolStripMenuItem
        '
        Me.HexDisplayToolStripMenuItem.Name = "HexDisplayToolStripMenuItem"
        Me.HexDisplayToolStripMenuItem.Size = New System.Drawing.Size(232, 22)
        Me.HexDisplayToolStripMenuItem.Text = "Hex Display"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(229, 6)
        '
        'ChooseTerminalFontToolStripMenuItem
        '
        Me.ChooseTerminalFontToolStripMenuItem.Name = "ChooseTerminalFontToolStripMenuItem"
        Me.ChooseTerminalFontToolStripMenuItem.Size = New System.Drawing.Size(232, 22)
        Me.ChooseTerminalFontToolStripMenuItem.Text = "Choose Terminal Display Font"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(52, 20)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.Tx)
        Me.Panel1.Controls.Add(Me.Rx)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Location = New System.Drawing.Point(0, 366)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(685, 39)
        Me.Panel1.TabIndex = 4
        '
        'Tx
        '
        Me.Tx.AutoSize = True
        Me.Tx.BackColor = System.Drawing.Color.Red
        Me.Tx.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Tx.Location = New System.Drawing.Point(174, 10)
        Me.Tx.Name = "Tx"
        Me.Tx.Size = New System.Drawing.Size(21, 15)
        Me.Tx.TabIndex = 5
        Me.Tx.Text = "Tx"
        '
        'Rx
        '
        Me.Rx.AutoSize = True
        Me.Rx.BackColor = System.Drawing.Color.Red
        Me.Rx.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Rx.Location = New System.Drawing.Point(201, 10)
        Me.Rx.Name = "Rx"
        Me.Rx.Size = New System.Drawing.Size(22, 15)
        Me.Rx.TabIndex = 4
        Me.Rx.Text = "Rx"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Red
        Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label4.Location = New System.Drawing.Point(137, 10)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(31, 15)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Ring"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Red
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label3.Location = New System.Drawing.Point(101, 10)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(32, 15)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "DCD"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Red
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label2.Location = New System.Drawing.Point(67, 10)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(30, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "CTS"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Red
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label1.Location = New System.Drawing.Point(31, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(32, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "DSR"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(168, 117)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "SendData"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(168, 163)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 6
        Me.Button2.Text = "SendID"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        '
        'SerialPort1
        '
        '
        'TestEnhanced
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(685, 402)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Controls.Add(Me.txtTerm)
        Me.Name = "TestEnhanced"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtTerm As System.Windows.Forms.TextBox
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CommPortToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PortOpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents SettingsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ClearScreeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ClearScreenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HexDisplayToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ChooseTerminalFontToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FontDialog1 As System.Windows.Forms.FontDialog
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Tx As System.Windows.Forms.Label
    Friend WithEvents Rx As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents SerialPort1 As System.IO.Ports.SerialPort

End Class
