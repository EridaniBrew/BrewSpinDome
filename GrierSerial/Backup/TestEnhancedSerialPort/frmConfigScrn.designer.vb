<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmConfigScrn
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConfigScrn))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.OddParity = New System.Windows.Forms.RadioButton
        Me.EvenParity = New System.Windows.Forms.RadioButton
        Me.NoParity = New System.Windows.Forms.RadioButton
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Data8 = New System.Windows.Forms.RadioButton
        Me.Data7 = New System.Windows.Forms.RadioButton
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.lstRate = New System.Windows.Forms.ListBox
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.lstDescription = New System.Windows.Forms.ListBox
        Me.lstCommPort = New System.Windows.Forms.ListBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.Stop2 = New System.Windows.Forms.RadioButton
        Me.Stop1 = New System.Windows.Forms.RadioButton
        Me.Watchdog = New System.Windows.Forms.CheckBox
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.OddParity)
        Me.GroupBox1.Controls.Add(Me.EvenParity)
        Me.GroupBox1.Controls.Add(Me.NoParity)
        Me.GroupBox1.Location = New System.Drawing.Point(111, 54)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(86, 86)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Parity"
        '
        'OddParity
        '
        Me.OddParity.AutoSize = True
        Me.OddParity.Location = New System.Drawing.Point(6, 65)
        Me.OddParity.Name = "OddParity"
        Me.OddParity.Size = New System.Drawing.Size(45, 17)
        Me.OddParity.TabIndex = 2
        Me.OddParity.TabStop = True
        Me.OddParity.Text = "Odd"
        Me.OddParity.UseVisualStyleBackColor = True
        '
        'EvenParity
        '
        Me.EvenParity.AutoSize = True
        Me.EvenParity.Location = New System.Drawing.Point(6, 42)
        Me.EvenParity.Name = "EvenParity"
        Me.EvenParity.Size = New System.Drawing.Size(50, 17)
        Me.EvenParity.TabIndex = 1
        Me.EvenParity.TabStop = True
        Me.EvenParity.Text = "Even"
        Me.EvenParity.UseVisualStyleBackColor = True
        '
        'NoParity
        '
        Me.NoParity.AutoSize = True
        Me.NoParity.Location = New System.Drawing.Point(6, 19)
        Me.NoParity.Name = "NoParity"
        Me.NoParity.Size = New System.Drawing.Size(51, 17)
        Me.NoParity.TabIndex = 0
        Me.NoParity.TabStop = True
        Me.NoParity.Text = "None"
        Me.NoParity.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Data8)
        Me.GroupBox2.Controls.Add(Me.Data7)
        Me.GroupBox2.Location = New System.Drawing.Point(110, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(86, 42)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Data Bits"
        '
        'Data8
        '
        Me.Data8.AutoSize = True
        Me.Data8.Location = New System.Drawing.Point(43, 19)
        Me.Data8.Name = "Data8"
        Me.Data8.Size = New System.Drawing.Size(31, 17)
        Me.Data8.TabIndex = 1
        Me.Data8.TabStop = True
        Me.Data8.Text = "8"
        Me.Data8.UseVisualStyleBackColor = True
        '
        'Data7
        '
        Me.Data7.AutoSize = True
        Me.Data7.Location = New System.Drawing.Point(6, 19)
        Me.Data7.Name = "Data7"
        Me.Data7.Size = New System.Drawing.Size(31, 17)
        Me.Data7.TabIndex = 0
        Me.Data7.TabStop = True
        Me.Data7.Text = "7"
        Me.Data7.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.lstRate)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(86, 160)
        Me.GroupBox3.TabIndex = 2
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Bit Rate"
        '
        'lstRate
        '
        Me.lstRate.FormattingEnabled = True
        Me.lstRate.Location = New System.Drawing.Point(11, 19)
        Me.lstRate.Name = "lstRate"
        Me.lstRate.Size = New System.Drawing.Size(63, 134)
        Me.lstRate.TabIndex = 0
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.lstDescription)
        Me.GroupBox4.Controls.Add(Me.lstCommPort)
        Me.GroupBox4.Location = New System.Drawing.Point(12, 188)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(400, 86)
        Me.GroupBox4.TabIndex = 3
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Serial Port"
        '
        'lstDescription
        '
        Me.lstDescription.FormattingEnabled = True
        Me.lstDescription.Location = New System.Drawing.Point(87, 20)
        Me.lstDescription.Name = "lstDescription"
        Me.lstDescription.Size = New System.Drawing.Size(307, 56)
        Me.lstDescription.TabIndex = 1
        '
        'lstCommPort
        '
        Me.lstCommPort.FormattingEnabled = True
        Me.lstCommPort.Location = New System.Drawing.Point(6, 20)
        Me.lstCommPort.Name = "lstCommPort"
        Me.lstCommPort.Size = New System.Drawing.Size(75, 56)
        Me.lstCommPort.Sorted = True
        Me.lstCommPort.TabIndex = 0
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(242, 76)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(83, 23)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "OK"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(242, 105)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(83, 23)
        Me.Button2.TabIndex = 5
        Me.Button2.Text = "Cancel"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.Stop2)
        Me.GroupBox5.Controls.Add(Me.Stop1)
        Me.GroupBox5.Location = New System.Drawing.Point(110, 146)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(87, 42)
        Me.GroupBox5.TabIndex = 6
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Stop Bits"
        '
        'Stop2
        '
        Me.Stop2.AutoSize = True
        Me.Stop2.Location = New System.Drawing.Point(43, 19)
        Me.Stop2.Name = "Stop2"
        Me.Stop2.Size = New System.Drawing.Size(31, 17)
        Me.Stop2.TabIndex = 1
        Me.Stop2.TabStop = True
        Me.Stop2.Text = "2"
        Me.Stop2.UseVisualStyleBackColor = True
        '
        'Stop1
        '
        Me.Stop1.AutoSize = True
        Me.Stop1.Location = New System.Drawing.Point(6, 19)
        Me.Stop1.Name = "Stop1"
        Me.Stop1.Size = New System.Drawing.Size(31, 17)
        Me.Stop1.TabIndex = 0
        Me.Stop1.TabStop = True
        Me.Stop1.Text = "1"
        Me.Stop1.UseVisualStyleBackColor = True
        '
        'Watchdog
        '
        Me.Watchdog.AutoSize = True
        Me.Watchdog.Location = New System.Drawing.Point(242, 165)
        Me.Watchdog.Name = "Watchdog"
        Me.Watchdog.Size = New System.Drawing.Size(165, 17)
        Me.Watchdog.TabIndex = 2
        Me.Watchdog.Text = "Enable 10 second Watchdog"
        Me.Watchdog.UseVisualStyleBackColor = True
        '
        'frmConfigScrn
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(418, 280)
        Me.Controls.Add(Me.Watchdog)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmConfigScrn"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "frmConfigScrn"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents OddParity As System.Windows.Forms.RadioButton
    Friend WithEvents EvenParity As System.Windows.Forms.RadioButton
    Friend WithEvents NoParity As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Data8 As System.Windows.Forms.RadioButton
    Friend WithEvents Data7 As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents lstRate As System.Windows.Forms.ListBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents lstCommPort As System.Windows.Forms.ListBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents Stop2 As System.Windows.Forms.RadioButton
    Friend WithEvents Stop1 As System.Windows.Forms.RadioButton
    Friend WithEvents lstDescription As System.Windows.Forms.ListBox
    Friend WithEvents Watchdog As System.Windows.Forms.CheckBox
End Class
