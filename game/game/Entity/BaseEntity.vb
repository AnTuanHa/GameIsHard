Public MustInherit Class BaseEntity
    Inherits BaseObject

    ' Image of the entity
    Protected image As Bitmap

    ' Position of the entity
    Protected position As Vector
    Protected prevPosition As Vector

    ' Movement vectors of the entity
    Protected velocity As Vector
    Protected acceleration As Vector

    ' Rendering position; in which we interpolate between the previous and current position
    Protected drawPosition As Point

    ' Speed in which the entity moves at
    Protected speed As Double

    ' Overridable function that draws the image to the screen
    Public Overridable Sub Render(ByRef e As Graphics, ByVal interpolation As Double)
        ' Interpolate between the previous and current position
        drawPosition.X = CInt(prevPosition.X + ((position.X - prevPosition.X) * interpolation))
        drawPosition.Y = CInt(prevPosition.Y + ((position.Y - prevPosition.Y) * interpolation))

        ' Draw image
        e.DrawImage(image, drawPosition.X, drawPosition.Y, rect.Width, rect.Height)
        e.ResetTransform()
    End Sub

    ' Overridable function that updates the rectangle to the position and the previous position
    Public Overridable Sub Update()
        ' Update the boundary box (rectangle) to the current position
        rect.Location = position.ToPoint()

        ' Update the previous position to the current position
        prevPosition = position

        ' Update the current position to the new position
        position += velocity
    End Sub

    ' Deletes and clears all the memory that the entity used
    Public Overrides Sub Dispose()
        rect = Nothing
        image = Nothing
        position = Nothing
        prevPosition = Nothing
        drawPosition = Nothing
        velocity = Nothing
    End Sub

    ' Gets the current position of the entity
    Public Function GetPosition() As Vector
        Return position
    End Function

    ' Set the entity's position
    Public Overridable Sub SetPosition(ByVal x As Integer, ByVal y As Integer)
        position.X = x
        position.Y = y
    End Sub

    ' Set the X position of the entity
    Public Overridable Sub SetPositionX(ByVal x As Integer)
        position.X = x
    End Sub

    ' Set the Y position of the entity
    Public Overridable Sub SetPositionY(ByVal y As Integer)
        position.Y = y
    End Sub

    ' Gets the velocity vector of the entity
    Public Function GetVelocity() As Vector
        Return velocity
    End Function

    ' Set the velocity of the entity
    Public Overridable Sub SetVelocity(ByVal x As Double, ByVal y As Double)
        velocity.X = x
        velocity.Y = y
    End Sub

    ' Set the X-component of the velocity
    Public Overridable Sub SetVelocityX(ByVal x As Double)
        velocity.X = x
    End Sub

    ' Set the Y-component of the velocity
    Public Overridable Sub SetVelocityY(ByVal y As Double)
        velocity.Y = y
    End Sub

    ' Get the entity's speed
    Public Function GetSpeed() As Double
        Return speed
    End Function

    ' Get the entity's acceleration vector
    Public Function GetAcceleration() As Vector
        Return acceleration
    End Function

    ' Set the acceleration of the entity
    Public Overridable Sub SetAcceleration(ByVal x As Double, ByVal y As Double)
        acceleration.X = x
        acceleration.Y = y
    End Sub

    ' Set the X-component of the acceleration
    Public Overridable Sub SetAccelerationX(ByVal x As Double)
        acceleration.X = x
    End Sub

    ' Set the Y-component of the acceleration
    Public Overridable Sub SetAccelerationY(ByVal y As Double)
        acceleration.Y = y
    End Sub
End Class
