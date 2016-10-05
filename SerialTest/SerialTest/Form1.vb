Imports System.Threading        ' to get Sleep function?

Public Class Form1

    Private Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        Dim s As String = ""

        ' open port
        SerialPort1.Open()
        SerialPort1.WriteLine(txtInput.Text)
        txtOutput.Text = "Sent text " & txtInput.Text
        Dim officialResponse As Boolean = False
        Thread.Sleep(1000)
        Try
            s = SerialPort1.ReadLine
        Catch ex As Exception
            txtOutput.AppendText(vbCrLf & "Read timed out")
        End Try
        txtOutput.AppendText(vbCrLf & s)
        SerialPort1.Close()
    End Sub
End Class
