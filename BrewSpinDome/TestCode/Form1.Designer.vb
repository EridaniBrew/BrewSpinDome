<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.txtIn = New System.Windows.Forms.TextBox()
        Me.lblOut = New System.Windows.Forms.Label()
        Me.btnGo = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'txtIn
        '
        Me.txtIn.Location = New System.Drawing.Point(6, 33)
        Me.txtIn.Name = "txtIn"
        Me.txtIn.Size = New System.Drawing.Size(271, 20)
        Me.txtIn.TabIndex = 0
        '
        'lblOut
        '
        Me.lblOut.AutoSize = True
        Me.lblOut.Location = New System.Drawing.Point(6, 87)
        Me.lblOut.Name = "lblOut"
        Me.lblOut.Size = New System.Drawing.Size(39, 13)
        Me.lblOut.TabIndex = 1
        Me.lblOut.Text = "Label1"
        '
        'btnGo
        '
        Me.btnGo.Location = New System.Drawing.Point(30, 125)
        Me.btnGo.Name = "btnGo"
        Me.btnGo.Size = New System.Drawing.Size(62, 31)
        Me.btnGo.TabIndex = 2
        Me.btnGo.Text = "Go"
        Me.btnGo.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 261)
        Me.Controls.Add(Me.btnGo)
        Me.Controls.Add(Me.lblOut)
        Me.Controls.Add(Me.txtIn)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtIn As TextBox
    Friend WithEvents lblOut As Label
    Friend WithEvents btnGo As Button
End Class
