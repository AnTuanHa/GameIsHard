Public Class PlayerBullet
    Inherits BaseEntity

    ' Store the angle in degrees in which the player's bullet image will rotate
    Dim angleDegrees As Double

    ' Initialize the player bullet
    Public Sub New(ByVal x As Integer, ByVal y As Integer)
        image = My.Resources.PlayerBullet
        position = New Vector(x, y)
        prevPosition = position
        rect = New Rectangle(position.ToPoint(), My.Resources.PlayerBullet.Size)
        collidingType = CollisionType.Collision_PlayerBullet
        speed = 10
    End Sub

    ' Update the player bullet
    Public Overrides Sub Update()
        MyBase.Update()

        ' Rotate the player's bullet every update
        angleDegrees += 2
        ' If the angle is greater than 360, reset it back to 0
        If angleDegrees >= 360 Then
            angleDegrees = 0
        End If
    End Sub

    ' Render the player's bullet
    Public Overrides Sub Render(ByRef e As System.Drawing.Graphics, ByVal interpolation As Double)
        ' Interpolate between the previous and current position
        drawPosition.X = CInt(prevPosition.X + ((position.X - prevPosition.X) * interpolation))
        drawPosition.Y = CInt(prevPosition.Y + ((position.Y - prevPosition.Y) * interpolation))

        ' Get the centered position of the player's bullet at render time
        Dim newCenteredPos As Point = New Point(drawPosition.X + GetWidth() / 2, drawPosition.Y + GetHeight() / 2)

        ' Move the origin of the graphics from (0,0) to where the center of the image is
        e.TranslateTransform(newCenteredPos.X, newCenteredPos.Y)

        ' Rotate the screen/buffer/matrix
        e.RotateTransform(angleDegrees)

        ' Must subtract the drawPosition because we moved the graphic's origin
        drawPosition.X -= newCenteredPos.X
        drawPosition.Y -= newCenteredPos.Y

        ' Draw image
        e.DrawImage(image, drawPosition.X, drawPosition.Y, rect.Width, rect.Height)

        ' Reset the transformations applied earlier to the screen/buffer
        e.ResetTransform()
    End Sub
End Class
