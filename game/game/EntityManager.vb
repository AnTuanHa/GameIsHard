Public Class EntityManager

    ' Keep track of all the entities in the game
    Private listOfEntities As List(Of BaseEntity)

    ' Use quadtree for efficient collision detection
    Private quadTree As QuadTree

    ' Initialize the Entity Manager
    Public Sub New()
        quadTree = New QuadTree(0, New Rectangle(0, 0, Game.ClientSize.Width, Game.ClientSize.Height))
        listOfEntities = New List(Of BaseEntity)
    End Sub

    ' Add an entity to the listOfEntities
    Public Sub AddEntity(ByRef entity As BaseEntity)
        listOfEntities.Add(entity)
    End Sub

    ' Update the entity manager
    Public Sub Update()
        ' Clear the quadtree
        quadTree.Clear()

        ' Loop through all of the entities in the list
        For Each entity As BaseEntity In listOfEntities
            ' Update the entity
            entity.Update()

            ' Insert the entity into the quadtree for collision detection
            quadTree.Insert(entity)
        Next
    End Sub

    ' Render all the entities in the list
    Public Sub Render(ByRef e As System.Drawing.Graphics, ByVal interpolation As Double)
        ' Render the quadtree nodes (used for debugging purposes only)
        'quadTree.RenderNodes(e)

        ' Loop through all of the entities in the list
        For Each entity As BaseEntity In listOfEntities
            ' Render them onto the screen
            entity.Render(e, interpolation)
        Next
    End Sub

    ' Check the collisions between each entity
    Public Sub CheckCollision()
        ' Keep track of all of the entities that need to be removed
        Dim removedEntities As List(Of BaseEntity) = New List(Of BaseEntity)

        ' Loop backwards in the whole entity list
        For i As Integer = listOfEntities.Count - 1 To 0 Step -1


            ' Collision detection for player
            If listOfEntities(i).GetCollisionType() = BaseEntity.CollisionType.Collision_Player Then
                ' Create a temporary variable to be the datatype: Player
                Dim player As Player = listOfEntities(i)

                ' Get a list of objects that are in the same node as the player
                Dim returnObjects As List(Of BaseObject) = New List(Of BaseObject)
                quadTree.Retrieve(returnObjects, player.GetRectangle())

                ' Loop backwards through all of the objects that are in the same node as the play
                For j As Integer = returnObjects.Count - 1 To 0 Step -1
                    ' Create a temporary variable to be the datatype: BaseObject
                    ' Making it easier to refer to the object
                    Dim obj As BaseObject = returnObjects(j)

                    ' Check if the object we are checking is an EnemyBullet
                    If obj.GetCollisionType() = BaseObject.CollisionType.Collision_EnemyBullet Then
                        ' Create a temporary variable to be the datatype: EnemyBullet
                        Dim enemyBullet As BaseEnemyBullet = obj

                        ' AABB (rectangle vs rectangle) collision detection
                        If player.GetRectangle().IntersectsWith(enemyBullet.GetRectangle()) Then
                            ' Circle vs Circle collision detection
                            If CheckForCircleToCircleCollision(player, enemyBullet) Then
                                ' Add bullet to the list of entities that need to be removed
                                If Not MainMenu.GetIsGameMuted() Then
                                    My.Computer.Audio.Play(My.Resources.LoseSound, AudioPlayMode.Background)
                                End If

                                MsgBox("Loser! You're a loser! Are you feeling sorry for yourself? Well you should be, because you are dirt! You make me sick you big baby. Baby want a bottle? A big dirt bottle?", MsgBoxStyle.OkOnly, "Game is Lost")
                                Game.Close()
                                removedEntities.Add(enemyBullet)
                            End If
                        End If
                    End If
                Next
            End If


            ' Collision detection for player's bullet
            If listOfEntities(i).GetCollisionType() = BaseEntity.CollisionType.Collision_PlayerBullet Then
                ' Create temporary variable to refer the player's bullet
                Dim playerBullet As PlayerBullet = listOfEntities(i)

                ' Get a list of objects that are within the same node as the player's bullet
                Dim returnObjects As List(Of BaseObject) = New List(Of BaseObject)
                quadTree.Retrieve(returnObjects, playerBullet.GetRectangle())

                ' Loop through the objects in the same node as the player's bullet
                For Each obj As BaseObject In returnObjects
                    ' Check if the entities in the same node is the Enemy
                    If obj.GetCollisionType() = BaseObject.CollisionType.Collision_Enemy Then
                        ' Create temp variable to refer the enemy
                        Dim enemy As Enemy = obj

                        ' AABB (rectangle vs rectangle) collision detection
                        If playerBullet.GetRectangle().IntersectsWith(enemy.GetRectangle()) Then
                            enemy.TakeDamage(5)
                            removedEntities.Add(playerBullet)

                            If enemy.GetHealth <= 0 Then
                                If Not MainMenu.GetIsGameMuted() Then
                                    My.Computer.Audio.Play(My.Resources.WinSound, AudioPlayMode.Background)
                                End If
                                MsgBox("You win!", MsgBoxStyle.OkOnly, "Game is won")
                                Game.Close()
                            End If
                        End If
                    End If
                Next
            End If


            ' Check if any entity is outside of the screen
            If CheckOutsideBoundary(listOfEntities(i)) Then
                ' Check if the entity is the player
                If listOfEntities(i).GetCollisionType() = BaseEntity.CollisionType.Collision_Player Then
                    ' Create temporary variable to easily refer to the player
                    Dim player As Player = listOfEntities(i)
                    Dim width As Integer = player.GetWidth()
                    Dim height As Integer = player.GetHeight()

                    ' Set the player's position to the edge of the screen if they are outside of the screen
                    If player.GetPosition().X < 0 Then
                        player.SetPosition(0, player.GetPosition().Y)
                    ElseIf player.GetPosition().X + width > Game.ClientSize.Width Then
                        player.SetPosition(Game.ClientSize.Width - width, player.GetPosition().Y)
                    End If

                    ' Set the player's position to be the edge of the screen if they are outside the screen
                    If player.GetPosition().Y < 0 Then
                        player.SetPosition(player.GetPosition().X, 0)
                    ElseIf player.GetPosition().Y + height > Game.ClientSize.Height Then
                        player.SetPosition(player.GetPosition().X, Game.ClientSize.Height - height)
                    End If

                    ' Check if the entity is a bullet
                ElseIf listOfEntities(i).GetCollisionType() = BaseEntity.CollisionType.Collision_EnemyBullet Or _
                        listOfEntities(i).GetCollisionType() = BaseEntity.CollisionType.Collision_PlayerBullet Then

                    ' Check if the bullet is COMPLETELY outside of the screen
                    If CheckOutsideBoundaryCompletely(listOfEntities(i)) Then
                        ' Add the bullet to the list of entites that need to be removed
                        removedEntities.Add(listOfEntities(i))
                    End If
                End If
            End If
        Next

        ' Loop backwards through all of the entities
        For i As Integer = listOfEntities.Count - 1 To 0 Step -1
            ' Loop backwards through all of the entities that need to be removed
            For j As Integer = removedEntities.Count - 1 To 0 Step -1
                ' Check if the entity that needs to be removed is the one in the EntityManager list
                ' We simply check their position as a poor man's way of checking if we are referring to the same entity
                If listOfEntities(i).GetPosition() = removedEntities(j).GetPosition() Then
                    ' Remove the entity in the removed entity list
                    removedEntities.RemoveAt(j)

                    ' Clear the memory used up by the entity
                    listOfEntities(i).Dispose()

                    ' Remove the entity in the list
                    listOfEntities.RemoveAt(i)
                End If
            Next
        Next
    End Sub

    ' Check if the entity is outside the display screen
    Private Function CheckOutsideBoundary(ByVal entity As BaseEntity) As Boolean
        Dim pos As Vector = entity.GetPosition()
        Dim width As Integer = entity.GetWidth()
        Dim height As Integer = entity.GetHeight()

        ' Return true if the edges of an entity is outside of the boundary at all
        If pos.X < 0 Or _
                pos.X + width > Game.ClientSize.Width Or _
                pos.Y < 0 Or _
                pos.Y + height > Game.ClientSize.Height Then
            Return True
        End If

        ' Otherwise, return false
        Return False
    End Function

    ' Check if the entity is completely outside of the display screen
    Private Function CheckOutsideBoundaryCompletely(ByVal entity As BaseEntity) As Boolean
        Dim pos As Vector = entity.GetPosition()
        Dim width As Integer = entity.GetWidth()
        Dim height As Integer = entity.GetHeight()

        ' Return true if the whole entity is outside of the boundary
        If pos.X + width < 0 Or _
                pos.X > Game.ClientSize.Width Or _
                pos.Y + height < 0 Or _
                pos.Y > Game.ClientSize.Height Then
            Return True
        End If

        ' Otherwise, return false
        Return False
    End Function

    ' Circle vs Circle collision detection
    Private Function CheckForCircleToCircleCollision(ByVal obj1 As BaseEntity, ByVal obj2 As BaseEntity)
        ' Get the object's position
        Dim pos1 As Vector = obj1.GetPosition()
        Dim pos2 As Vector = obj2.GetPosition()

        ' Get the radius's of the objects by their widths and dividing by 2 (height can be used too)
        Dim radius1 As Double = obj1.GetWidth() / 2
        Dim radius2 As Double = obj2.GetWidth() / 2

        ' Move the position of the object to the center (making calculations easier)
        pos1.X += radius1
        pos1.Y += radius1

        pos2.X += radius2
        pos2.Y += radius2

        ' Get the vector between the object1 and object2
        Dim dx As Double = pos1.X - pos2.X
        Dim dy As Double = pos1.Y - pos2.Y

        ' Get their distance via pythagorean's theorem
        Dim distance As Double = Math.Sqrt((dx * dx) + (dy * dy))

        ' Check if the distance is less than the radius of obj1 and obj2
        ' If true, we return true b/c a collision occurred
        If distance < radius1 + radius2 Then
            Return True
        End If

        ' Otherwise, return false b/c no collision occurred
        Return False
    End Function

    ' Return the entity list used by the manager
    Public Function GetEntityList() As List(Of BaseEntity)
        Return listOfEntities
    End Function
End Class
