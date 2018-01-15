Public Class MainMenu

    ' Determine if the game is muted or not
    Private gameMuted As Boolean

    ' Hide this form and show the game
    Private Sub startGameButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles startGameButton.Click
        Me.Hide()
        Game.Show()
    End Sub

    ' Quit/close the form
    Private Sub quitGameButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles quitGameButton.Click
        Me.Close()
    End Sub

    ' Hide this form and show instructions menu
    Private Sub instructionsButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles instructionsButton.Click
        Me.Hide()
        InstructionsMenu.Show()
    End Sub

    ' Get if the game has been muted
    Public Function GetIsGameMuted() As Boolean
        Return gameMuted
    End Function

    ' Mute all sounds
    Private Sub muteGameCheckBox_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles muteGameCheckBox.CheckedChanged
        ' Set gameMuted to true if the mute checkbox has been checked
        If muteGameCheckBox.Checked Then
            gameMuted = True

            ' Stop playing any sounds
            My.Computer.Audio.Stop()
        Else
            ' Otherwise, if it has been unchecked, set it to false
            gameMuted = False
        End If
    End Sub

    Private Sub MainMenu_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ' Set the display size to be 640x400 and center the game on the user's screen
        Me.ClientSize = New Size(640, 400)
        Dim centerOfScreenX As Integer = Screen.PrimaryScreen.WorkingArea.Width / 2 - Me.Width / 2
        Dim centerOfScreenY As Integer = Screen.PrimaryScreen.WorkingArea.Height / 2 - Me.Height / 2
        Me.Location = New Point(centerOfScreenX, centerOfScreenY)
    End Sub
End Class