Public Class Enemy
    Inherits BaseEntity

    ' Create the healthbar for the enemy
    Private healthBar As HealthBar

    ' Create predefined points of where the enemy should go
    ' Point 1 = top left corner. Point 2 = top right corner. Point 3 = center of the screen
    Private point1 As Vector
    Private point2 As Vector
    Private point3 As Vector

    ' Keep track of what point the enemy is going to
    Private pointToGoTo As Vector

    ' Keep track if the enemy is currently moving to a point
    Private isCurrentlyMovingToPoint As Boolean

    ' Keep track of the distance of the enemy's current position to the point he's going to
    Private distanceToPoint As Double

    ' Hold position timer for how long the enemy is going to stay at each point
    Private holdPositionTimer As Integer

    ' Stop all movement of the enemy (used to make him go to point 3 when he starts spamming the flower pattern)
    ' This keeps him from moving to any other points
    Private stopAllMovement As Boolean = False

    ' Initialize the enemy
    Public Sub New()
        image = My.Resources.Enemy
        rect = New Rectangle(position.ToPoint(), My.Resources.Enemy.Size)
        speed = 3
        collidingType = CollisionType.Collision_Enemy

        ' Initialize the healthbar, giving the enemy 1000 hp, setting it at the top of the screen
        ' with a width and height of 800 by 16 respectively
        healthBar = New HealthBar(1000, New Point(100, 16), 800, 16)

        ' Create the points of where the enemy will go to
        ' (top left corner, top right corner, and center of screen respectively)
        point1 = New Vector(128 - rect.Width / 2, 64)
        point2 = New Vector(864 - rect.Width / 2, 64)
        point3 = New Vector(Game.ClientSize.Width / 2 - rect.Width / 2, 200)

        ' Spawn the enemy outside of the display screen in the top left corner
        position = New Vector(0 - rect.Width, 0)
        prevPosition = position

        ' Make the enemy move to point 1 (top left corner)
        moveToPoint1()
    End Sub

    ' Render enemy
    Public Overrides Sub Render(ByRef e As System.Drawing.Graphics, ByVal interpolation As Double)
        MyBase.Render(e, interpolation)

        ' Render the healthbar of the enemy
        healthBar.Render(e)
    End Sub

    ' Update enemy logic
    Public Overrides Sub Update()
        MyBase.Update()

        ' If stopAllMovement is false, meaning that the enemy is not spamming
        ' the flower pattern and that the enemy's hp is not less than 25%
        ' We want the enemy to constantly move from point 1 to point 2 to point 3 and then back to point 1 constantly
        If stopAllMovement = False Then

            ' Check if the enemy is currently moving to a point
            If isCurrentlyMovingToPoint Then
                ' Determine the distance of the enemy from the point he's going to, to his current position
                Dim vectorToPoint As Vector = pointToGoTo - position
                distanceToPoint = vectorToPoint.GetMagnitude()

                ' Check if the distance is less than 5 (meaning that he's close to his destination)
                ' We don't use distanceToPoint = 0 because the distance will never be 0 since the enemy doesn't move
                ' at a speed of 1. The distances will always be off by a few pixels
                If distanceToPoint <= 5 Then
                    ' Stop moving the enemy
                    isCurrentlyMovingToPoint = False
                    SetVelocity(0, 0)
                    distanceToPoint = 0
                End If

                ' Otherwise, if the enemy is waiting at a point
            Else
                ' Increment the holdPosition timer by the game tick
                holdPositionTimer += Game.SKIP_TICKS

                ' If the enemy has waited 3 seconds at his destination
                If holdPositionTimer >= 3000 Then
                    ' Move the enemy to his next destination
                    If pointToGoTo = point1 Then
                        ' Move to point 2 if his current location is at point 1
                        moveToPoint2()
                    ElseIf pointToGoTo = point2 Then
                        ' Move to point 3 if his current location is at point 2
                        moveToPoint3()
                    ElseIf pointToGoTo = point3 Then
                        ' Move to piont 1 if his current location is at point 3
                        moveToPoint1()
                    End If

                    ' Reset the holdPosition timer
                    holdPositionTimer = 0
                End If
            End If

            ' If stopAllMovement is true, meaning that the enemy's health is less than 25%
            ' and is spamming the flower pattern, we want the enemy to stay at point 3
        Else
            ' Move the enemy to point 3
            moveToPoint3()

            ' Check if the enemy is still moving to point 3
            If isCurrentlyMovingToPoint Then
                ' Determine the distance of the enemy from the point he's going to, to his current position
                Dim vectorToPoint As Vector = pointToGoTo - position
                distanceToPoint = vectorToPoint.GetMagnitude()

                ' Check if the distance is less than 5 (meaning that he's close to his destination)
                ' We don't use distanceToPoint = 0 because the distance will never be 0 since the enemy doesn't move
                ' at a speed of 1. The distances will always be off by a few pixels
                If distanceToPoint < 5 Then
                    ' Stop moving the enemy
                    isCurrentlyMovingToPoint = False
                    SetVelocity(0, 0)
                    distanceToPoint = 0
                End If
            End If
        End If
    End Sub

    ' Make the enemy move to point 1
    Private Sub MoveToPoint1()
        ' Determine the vector from the enemy's current position to point 1
        Dim vectorToPoint1 As Vector = point1 - position
        ' Determine the angle in which the enemy must move at
        Dim angle As Double = Math.Atan2(vectorToPoint1.Y, vectorToPoint1.X)

        ' Determine the velocity vector the enemy must move at
        Dim vx As Double = Math.Cos(angle) * speed
        Dim vy As Double = Math.Sin(angle) * speed

        ' Set the enemy's velocity
        SetVelocity(vx, vy)

        ' Set the enemy's state to moving to a point and determine his distance
        isCurrentlyMovingToPoint = True
        pointToGoTo = point1
        distanceToPoint = vectorToPoint1.GetMagnitude()
    End Sub

    ' Make the enemy move to point 2
    Private Sub MoveToPoint2()
        ' Determine the vector from the enemy's current position to point 2
        Dim vectorToPoint2 As Vector = point2 - position
        ' Determine the angle in which the enemy must move at
        Dim angle As Double = Math.Atan2(vectorToPoint2.Y, vectorToPoint2.X)

        ' Determine the velocity vector the enemy must move at
        Dim vx As Double = Math.Cos(angle) * speed
        Dim vy As Double = Math.Sin(angle) * speed

        ' Set the enemy's velocity
        SetVelocity(vx, vy)

        ' Set the enemy's state to moving to a point and determine his distance
        isCurrentlyMovingToPoint = True
        pointToGoTo = point2
        distanceToPoint = vectorToPoint2.GetMagnitude()
    End Sub

    ' Make the enemy move to point 3
    Private Sub MoveToPoint3()
        ' Determine the vector from the enemy's current position to point 2
        Dim vectorToPoint3 As Vector = point3 - position
        ' Determine the angle in which the enemy must move at
        Dim angle As Double = Math.Atan2(vectorToPoint3.Y, vectorToPoint3.X)

        ' Determine the velocity vector the enemy must move at
        Dim vx As Double = Math.Cos(angle) * speed
        Dim vy As Double = Math.Sin(angle) * speed

        ' Set the enemy's velocity
        SetVelocity(vx, vy)

        ' Set the enemy's state to moving to a point and determine his distance
        isCurrentlyMovingToPoint = True
        pointToGoTo = point3
        distanceToPoint = vectorToPoint3.GetMagnitude()
    End Sub

    ' Make the enemy shoot in a circle depending on how many bullets specified in the function
    Public Sub ShootInCircle(ByVal bitmap As Bitmap, ByRef listOfEntities As List(Of BaseEntity), ByVal numOfBullets As Integer)
        ' Loop through the number of bullets we need to shoot evenly in a circle
        For counter As Integer = 0 To numOfBullets - 1
            ' Create the bullet and add it to the entity manager
            Dim enemyBullet As BaseEnemyBullet = New BaseEnemyBullet(bitmap, 3)
            enemyBullet.SetPosition(position.X + (rect.Width / 2) - (enemyBullet.GetWidth() / 2), position.Y + (rect.Height / 2) - (enemyBullet.GetHeight() / 2))
            listOfEntities.Add(enemyBullet)

            ' Determine the angle the bullet must go through
            ' To shoot in a circle evenly, we do 360 degrees divided by how many bullets we want to shoot
            ' multiplied by the current counter (the bullet # we're at) and convert it to radians
            Dim angle As Double = (counter * (360 / numOfBullets)) * (Math.PI / 180)

            ' Set the velocity the enemy bullet must move at to go at the correct angle at its specified speed
            Dim vx As Double = enemyBullet.GetSpeed() * Math.Cos(angle)
            Dim vy As Double = enemyBullet.GetSpeed() * Math.Sin(angle)
            enemyBullet.SetVelocity(vx, vy)
        Next
    End Sub

    ' Make the enemy shoot a series of bullets around the enemy (360 degrees)
    ' This is by rotating by 6 degrees every update tick
    Public Function ShootAroundEnemy() As EnemyBulletEmitter
        ' Determine the center of the enemy
        Dim centerOfEnemy As Vector = New Vector(position.X + (rect.Width / 2), position.Y + (rect.Height / 2))

        ' Create a bullet emitter
        Dim emitter As EnemyBulletEmitter

        ' Set the angle to 0 and the angleChangeSpeed to be 6 degrees
        Dim angle As Double = 0
        Dim angleChangeSpeed As Double = 6

        ' Initialize the emitter with a bullet speed of 3, spawn at the center of the enemy,
        ' Lasting for 2 seconds shooting 64 bullets
        emitter = New EnemyBulletEmitter(My.Resources.EnemyBulletPink, 3, centerOfEnemy, 2000, 64, angle, angleChangeSpeed)
        Return emitter
    End Function

    ' This will shoot a series of bullets in a spiral
    ' The spiral pattern has 6 lines of bullet spinning around
    Public Function ShootSpiral() As EnemyBulletEmitter
        ' Determine the center of the enemy
        Dim centerOfEnemy As Vector = New Vector(position.X + (rect.Width / 2), position.Y + (rect.Height / 2))

        ' Create a bullet emitter
        Dim emitter As EnemyBulletEmitter

        ' Set the angle to 0 and the angleChangeSpeed to be 57 degrees
        Dim angle As Double = 0
        Dim angleChangeSpeed As Double = 57

        ' Initialize the emitter with a bullet speed of 3, spawn at the center of the enemy,
        ' Lasting for 2 seconds shooting 200 bullets
        emitter = New EnemyBulletEmitter(My.Resources.EnemyBulletGreen, 3, centerOfEnemy, 2000, 200, angle, angleChangeSpeed)
        Return emitter
    End Function

    ' This will shoot a series of bullets in a spiral (version 2)
    ' The spiral pattern has 3 lines of bullets spinning around
    Public Function ShootSpiral2() As EnemyBulletEmitter
        ' Determine the center of the enemy
        Dim centerOfEnemy As Vector = New Vector(position.X + (rect.Width / 2), position.Y + (rect.Height / 2))

        ' Create a bullet emitter
        Dim emitter As EnemyBulletEmitter

        ' Set the angle to 0 and the angleChangeSpeed to be 114 degrees
        Dim angle As Double = 0
        Dim angleChangeSpeed As Double = 114

        ' Initialize the emitter with a bullet speed of 3, spawn at the center of the enemy,
        ' Lasting for 2 seconds shooting 128 bullets
        emitter = New EnemyBulletEmitter(My.Resources.EnemyBulletBlue, 3, centerOfEnemy, 2000, 128, angle, angleChangeSpeed)
        Return emitter
    End Function

    ' Shoot a beautiful flower pattern that looks like it has 6 petals
    Public Function ShootFlowerPattern() As List(Of EnemyBulletEmitter)
        ' Create a temp list of emitters
        Dim listOfBulletEmitters As List(Of EnemyBulletEmitter) = New List(Of EnemyBulletEmitter)

        ' Determine the center of the enemy
        Dim centerOfEnemy As Vector = New Vector(position.X + (rect.Width / 2), position.Y + (rect.Height / 2))

        ' Spawn 6 (0 to 5) emitters spinning clockwise
        For counter As Integer = 0 To 5
            ' Set the angle of the emitter to be evenly divided into 6 even portions
            Dim angle As Double = (counter * (360 / 6)) * (Math.PI / 180)

            ' Set the angle change speed to be 8 (clockwise)
            Dim angleChangeSpeed As Double = 8

            ' Create a bullet emitter
            Dim emitter As EnemyBulletEmitter

            ' Initialize the emitter with a bullet speed of 3, spawn at the center of the enemy,
            ' Lasting for 5 seconds shooting 32 bullets
            emitter = New EnemyBulletEmitter(My.Resources.EnemyBulletRed, 3, centerOfEnemy, 5000, 45, angle, angleChangeSpeed)
            listOfBulletEmitters.Add(emitter)
        Next

        ' Spawn 6 (0 to 5) emitters spinning counter-clockwise
        For counter As Integer = 0 To 5
            ' Set the angle of the emitter to be evenly divided into 6 even portions
            Dim angle As Double = (counter * (360 / 6)) * (Math.PI / 180)

            ' Set the angle change speed to be -8 (counter clockwise)
            Dim angleChangeSpeed As Double = -8

            ' Create a bullet emitter
            Dim emitter As EnemyBulletEmitter

            ' Initialize the emitter with a bullet speed of 3, spawn at the center of the enemy,
            ' Lasting for 5 seconds shooting 32 bullets
            emitter = New EnemyBulletEmitter(My.Resources.EnemyBulletRed, 3, centerOfEnemy, 5000, 32, angle, angleChangeSpeed)
            listOfBulletEmitters.Add(emitter)
        Next

        ' Return the list of bullet emitters we created
        Return listOfBulletEmitters
    End Function

    ' Shoot a series of bullets towards the player
    Public Function ShootToPlayer(ByRef playerPosition As Vector) As EnemyBulletEmitter
        ' Determine the center of the enemy
        Dim centerOfEnemy As Vector = New Vector(position.X + (rect.Width / 2), position.Y + (rect.Height / 2))

        ' Create a bullet emitter
        Dim emitter As EnemyBulletEmitter

        ' Determine the angle we need to shoot at from where the enemy is to the player's position
        Dim angle As Double = Math.Atan2(playerPosition.Y - position.Y, playerPosition.X - position.X)

        ' Initialize the emitter with a bullet speed of 5, spawn at the center of the enemy,
        ' Lasting for 2 seconds shooting 10 bullets, and an angle change speed of 0, since we don't want the bullet to change direction
        emitter = New EnemyBulletEmitter(My.Resources.EnemyBulletRoundedEdgeYellow, 5, centerOfEnemy, 2000, 10, angle, 0)
        Return emitter
    End Function

    ' Remove health
    Public Sub TakeDamage(ByVal damage As Integer)
        healthBar.RemoveHealth(damage)
    End Sub

    ' Return whether the enemy is moving to a point or not
    Public Function IsMovingToAPoint() As Boolean
        Return isCurrentlyMovingToPoint
    End Function

    ' Return the enemy's current health
    Public Function GetHealth() As Integer
        Return healthBar.GetCurrentHealth()
    End Function

    ' Return the enemy's current health percent
    Public Function GetHealthPercent() As Integer
        Return healthBar.GetCurrentHealthPercent()
    End Function

    ' Stops the enemy's movement and move him to point 3 (center of screen)
    ' Only use this when the enemy's hp is less than 25%
    Public Sub StopMovement()
        stopAllMovement = True
    End Sub

    ' Return the enemy's point he's going to
    Public Function GetPointGoingTo() As Vector
        Return pointToGoTo
    End Function

    ' Return Point 1 location (top left corner)
    Public Function GetPoint1() As Vector
        Return point1
    End Function

    ' Return Point 2 location (top right corner)
    Public Function GetPoint2() As Vector
        Return point2
    End Function

    ' Return Point 3 location (center of screen)
    Public Function GetPoint3() As Vector
        Return point3
    End Function
End Class