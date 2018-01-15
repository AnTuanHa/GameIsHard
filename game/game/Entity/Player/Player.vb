Public Class Player
    Inherits BaseEntity

    ' Store another image to draw under the player's hitbox
    Private underlay As Bitmap
    Private underlayDrawPosition As Point

    ' Store the slow speed when player holds shift
    Private slowSpeed As Integer

    ' Initialize the player
    Public Sub New()
        image = My.Resources.PlayerHitBox
        underlay = My.Resources.Player
        position = New Vector((Game.ClientSize.Width / 2) - (image.Width / 2), Game.Height - 128)
        prevPosition = position
        rect = New Rectangle(position.ToPoint(), My.Resources.PlayerHitBox.Size)
        speed = 5
        slowSpeed = 2
        collidingType = CollisionType.Collision_Player
    End Sub

    ' Render the player as well as its underlay
    Public Overrides Sub Render(ByRef e As System.Drawing.Graphics, ByVal interpolation As Double)
        ' Draw the underlay where the player is, but also center them
        underlayDrawPosition.X = CInt(prevPosition.X + (position.X - prevPosition.X) * interpolation) - underlay.Size.Width / 2 + image.Size.Width / 2
        underlayDrawPosition.Y = CInt(prevPosition.Y + (position.Y - prevPosition.Y) * interpolation) - underlay.Size.Height / 2 + image.Size.Height / 2

        ' Draw underlay first
        e.DrawImage(underlay, underlayDrawPosition.X, underlayDrawPosition.Y, underlay.Size.Width, underlay.Size.Height)

        ' Then draw the player's hitbox. Order matters
        MyBase.Render(e, interpolation)
    End Sub

    ' Update the player
    Public Overrides Sub Update()
        MyBase.Update()
    End Sub

    ' Return the slow speed
    Public Function GetSlowSpeed() As Integer
        Return slowSpeed
    End Function
End Class
