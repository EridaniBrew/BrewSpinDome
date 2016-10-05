<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LogDialog
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
        Me.txtLog = New System.Windows.Forms.TextBox()
        Me.cbVerbose = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'txtLog
        '
        Me.txtLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLog.Location = New System.Drawing.Point(11, 39)
        Me.txtLog.Multiline = True
        Me.txtLog.Name = "txtLog"
        Me.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtLog.Size = New System.Drawing.Size(554, 372)
        Me.txtLog.TabIndex = 1
        '
        'cbVerbose
        '
        Me.cbVerbose.AutoSize = True
        Me.cbVerbose.Location = New System.Drawing.Point(12, 12)
        Me.cbVerbose.Name = "cbVerbose"
        Me.cbVerbose.Size = New System.Drawing.Size(187, 21)
        Me.cbVerbose.TabIndex = 2
        Me.cbVerbose.Text = "Include Status Messages"
        Me.cbVerbose.UseVisualStyleBackColor = True
        '
        'LogDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(578, 423)
        Me.ControlBox = False
        Me.Controls.Add(Me.cbVerbose)
        Me.Controls.Add(Me.txtLog)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LogDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "LogDialog"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtLog As System.Windows.Forms.TextBox
    Friend WithEvents cbVerbose As System.Windows.Forms.CheckBox

End Class
