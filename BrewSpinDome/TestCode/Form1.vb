Imports System.Text.RegularExpressions


Public Class Form1
    Private Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
        Dim s As String = txtIn.Text

        ' should return string like POS 235#
        Dim rgx As Regex = New Regex("\w*\s*\d+#")
        If (rgx.IsMatch(s)) Then
            ' rebuild string correctly
            rgx = New Regex("\w*\s*(\d+#)")
            s = rgx.Replace(s, "POS $1")
            lblOut.Text = s
        Else
            ' cannot use string
            lblOut.Text = "Cannot use string " & s
        End If
    End Sub
End Class
