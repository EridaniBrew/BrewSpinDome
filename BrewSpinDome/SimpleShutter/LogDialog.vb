Imports System.Windows.Forms

Public Class LogDialog

    Private Sub cbVerbose_CheckedChanged(sender As Object, e As EventArgs) Handles cbVerbose.CheckedChanged
        Dim mainform As SimpleShutter
        mainform = cbVerbose.Tag
        If (Not IsNothing(mainform)) Then
            mainform.SetLogStatus(cbVerbose.Checked)
        End If
    End Sub

End Class
