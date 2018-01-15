Public MustInherit Class BaseObject

    ' Collision types for each object
    Enum CollisionType As Integer
        Collision_Player
        Collision_PlayerBullet
        Collision_Enemy
        Collision_EnemyBullet
    End Enum

    ' Collision type of the object
    Protected collidingType As CollisionType

    ' Rectangle of the object
    Protected rect As Rectangle

    ' Return the width of the object
    Public Function GetWidth() As Integer
        Return rect.Width
    End Function

    ' Return the height of the object
    Public Function GetHeight() As Integer
        Return rect.Height
    End Function

    ' Return the rectangle of the object
    Public Function GetRectangle() As Rectangle
        Return rect
    End Function

    ' Return the collision type of the object
    Public Function GetCollisionType() As CollisionType
        Return collidingType
    End Function

    ' Stub function used for BaseEntity and its subclasses
    Public Overridable Sub Dispose()

    End Sub
End Class
