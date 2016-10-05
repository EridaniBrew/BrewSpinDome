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
        Me.components = New System.ComponentModel.Container()
        Me.txtInput = New System.Windows.Forms.TextBox()
        Me.txtOutput = New System.Windows.Forms.TextBox()
        Me.btnSend = New System.Windows.Forms.Button()
        Me.SerialPort1 = New System.IO.Ports.SerialPort(Me.components)
        Me.SuspendLayout()
        '
        'txtInput
        '
        Me.txtInput.Location = New System.Drawing.Point(12, 12)
        Me.txtInput.Name = "txtInput"
        Me.txtInput.Size = New System.Drawing.Size(199, 20)
        Me.txtInput.TabIndex = 0
        Me.txtInput.Text = "getpos#"
        '
        'txtOutput
        '
        Me.txtOutput.Location = New System.Drawing.Point(12, 62)
        Me.txtOutput.Multiline = True
        Me.txtOutput.Name = "txtOutput"
        Me.txtOutput.Size = New System.Drawing.Size(265, 180)
        Me.txtOutput.TabIndex = 1
        '
        'btnSend
        '
        Me.btnSend.Location = New System.Drawing.Point(219, 10)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.Size = New System.Drawing.Size(58, 21)
        Me.btnSend.TabIndex = 2
        Me.btnSend.Text = "Send"
        Me.btnSend.UseVisualStyleBackColor = True
        '
        'SerialPort1
        '
        Me.SerialPort1.PortName = "COM10"
        Me.SerialPort1.ReadTimeout = 1000
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 261)
        Me.Controls.Add(Me.btnSend)
        Me.Controls.Add(Me.txtOutput)
        Me.Controls.Add(Me.txtInput)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtInput As System.Windows.Forms.TextBox
    Friend WithEvents txtOutput As System.Windows.Forms.TextBox
    Friend WithEvents btnSend As System.Windows.Forms.Button
    Friend WithEvents SerialPort1 As System.IO.Ports.SerialPort

End Class
