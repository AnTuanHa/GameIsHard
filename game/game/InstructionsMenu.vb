Public Class InstructionsMenu

    ' Hide this form and show main menu
    Private Sub returnToMainMenuButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles returnToMainMenuButton.Click
        Me.Hide()
        MainMenu.Show()
    End Sub

    Private Sub InstructionsMenu_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ' Set the display size to be 640x400 and center the game on the user's screen
        Me.ClientSize = New Size(640, 400)
        Dim centerOfScreenX As Integer = Screen.PrimaryScreen.WorkingArea.Width / 2 - Me.Width / 2
        Dim centerOfScreenY As Integer = Screen.PrimaryScreen.WorkingArea.Height / 2 - Me.Height / 2
        Me.Location = New Point(centerOfScreenX, centerOfScreenY)
    End Sub
End Class