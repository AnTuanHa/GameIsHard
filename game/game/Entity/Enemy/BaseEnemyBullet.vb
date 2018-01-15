Public Class BaseEnemyBullet
    Inherits BaseEntity

    ' Store angle in degrees in which the enemy bullet
    ' should be rotated at when drawing it
    Private angleDegrees As Integer

    ' Initialize the Base Enemy Bullet
    Public Sub New(ByVal newImage As Bitmap, ByVal speed As Double)
        image = newImage
        rect = New Rectangle(position.ToPoint(), image.Size)
        collidingType = CollisionType.Collision_EnemyBullet
        Me.speed = speed
    End Sub

    ' Update the bullet
    Public Overrides Sub Update()
        ' Update the boundary box (rectangle) to the current position
        rect.Location = position.ToPoint()

        ' Update the previous position to the current position
        prevPosition = position

        ' Increment the velocity by its acceleration
        velocity += acceleration

        ' Get the magnitude of the velocity, if it's greater than
        ' the max speed we set the bullet at, we break the velocity
        ' into components such that they should have a magnitude of
        ' the max speed.
        Dim magnitude As Double = velocity.GetMagnitude()
        If magnitude > speed Then
            ' Convert the velocity into a unit vector and multiply it by
            ' the max speed we want it to be set at
            velocity.X = (velocity.X / magnitude) * speed
            velocity.Y = (velocity.Y / magnitude) * speed
        End If

        ' Update the position
        position += velocity
    End Sub

    ' Draw the enemy bullet
    Public Overrides Sub Render(ByRef e As System.Drawing.Graphics, ByVal interpolation As Double)
        ' Interpolate between the previous and current position
        drawPosition.X = CInt(prevPosition.X + ((position.X - prevPosition.X) * interpolation))
        drawPosition.Y = CInt(prevPosition.Y + ((position.Y - prevPosition.Y) * interpolation))

        ' Get the centered position of the enemy bullet at render time
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

        ' Reset the transformations we did earlier on the screen/buffer
        e.ResetTransform()
    End Sub

    ' Set the position of the enemy bullet
    ' This should only be used when creating a new bullet
    ' This is b/c we also set the previous position to the current position
    Public Overrides Sub SetPosition(ByVal x As Integer, ByVal y As Integer)
        MyBase.SetPosition(x, y)
        prevPosition = position
    End Sub

    ' Set the rotation in which the enemybullet should be drawn at
    Public Sub SetImageRotation(ByVal angle As Integer)
        angleDegrees = angle
    End Sub
End Class
