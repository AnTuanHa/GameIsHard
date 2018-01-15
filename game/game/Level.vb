Public Class Level

    ' Declare a function that allows us to get the user's keyboard state,
    ' check whether a key is pressed down or not
    Private Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As Integer) As Integer

    ' Background image
    Private backgroundImage As Bitmap

    ' Create 2 positions so we can create a seamless scrolling background
    Private backgroundImagePos As Point
    Private backgroundImagePos2 As Point

    ' Create a manager to manage the entities in the game
    Private entityManager As EntityManager

    ' Create the player
    Private player As Player

    ' Keeps track of how many bullets we want to shoot within a certain interval
    Private playerBulletTimer As Integer

    ' Create the boss/enemy
    Private enemy As Enemy

    ' Create a list of enemy bullet emitters
    Private listOfEnemyBulletEmitters As List(Of EnemyBulletEmitter)

    ' Timers for the enemy bullet patterns
    Private shootInCircleTimer As Integer
    Private shootToPlayerTimer As Integer
    Private shootFlowerPatternTimer As Integer

    ' Initialize the level
    Public Sub New()
        ' Set background image
        backgroundImage = My.Resources.Background

        ' Put background image at 0,0
        backgroundImagePos = New Point(0, 0)
        ' Put another background image above it
        backgroundImagePos2 = New Point(0, -768)

        entityManager = New EntityManager()
        player = New Player()
        enemy = New Enemy()

        ' Add the player and enemy to the entity manager
        entityManager.AddEntity(player)
        entityManager.AddEntity(enemy)

        listOfEnemyBulletEmitters = New List(Of EnemyBulletEmitter)

        ' Set the timer's to the interval we want to shoot them at
        ' We don't set it to 0 because we want the enemy to shoot his abilities
        ' immediately. We don't want to wait for the timer to tick
        ' while he waits at his destination
        shootInCircleTimer = 3000
        shootToPlayerTimer = 200
        shootFlowerPatternTimer = 5000
    End Sub

    ' Update the level
    Public Sub Update()
        backgroundImagePos.Y += 1
        backgroundImagePos2.Y += 1
        If backgroundImagePos.Y >= 768 Then
            backgroundImagePos.Y = -768
        End If
        If backgroundImagePos2.Y >= 768 Then
            backgroundImagePos2.Y = -768
        End If

        ' Update the entities in the manager
        entityManager.Update()

        ' Stop the player's movement
        player.SetVelocityX(0)
        player.SetVelocityY(0)

        ' Check if the Up, Left, Right or Down arrow key is pressed
        ' If true, set the player's velocity in its corresponding direction by using a unit vector
        If GetAsyncKeyState(Keys.Up) Then
            player.SetVelocityY(-1)
        End If
        If GetAsyncKeyState(Keys.Left) Then
            player.SetVelocityX(-1)
        End If
        If GetAsyncKeyState(Keys.Right) Then
            player.SetVelocityX(1)
        End If
        If GetAsyncKeyState(Keys.Down) Then
            player.SetVelocityY(1)
        End If

        ' Increment the bullet timer by the game time tick
        playerBulletTimer += Game.SKIP_TICKS

        ' Check if the Z key is pressed
        If GetAsyncKeyState(Keys.Z) Then
            ' If the player bullet timer is greater than 100 milliseconds
            If playerBulletTimer >= 100 Then
                ' Create the player bullet, spawn it at the player
                ' Add it to the entity manager and set its speed
                Dim playerBullet As PlayerBullet
                playerBullet = New PlayerBullet(player.GetPosition.X - player.GetWidth() / 2, player.GetPosition.Y - My.Resources.Player.Size.Height)
                entityManager.AddEntity(playerBullet)
                playerBullet.SetVelocityY(-playerBullet.GetSpeed())

                ' Reset the bullet timer
                playerBulletTimer = 0
            End If
        End If

        ' Determine the player's magnitude/speed
        Dim magnitude As Double = player.GetVelocity().GetMagnitude()
        ' If the player's magnitude is not 0 (they are moving)
        If magnitude <> 0 Then
            ' Set the player's velocity to a unit vector
            ' By setting the player's velocity to a unit vector, this effectively
            ' stops the player from moving faster diagonally
            player.SetVelocityX(player.GetVelocity().X / magnitude)
            player.SetVelocityY(player.GetVelocity().Y / magnitude)
        End If

        ' If the shift key is pressed, we want the player to move at a slow speed
        If GetAsyncKeyState(Keys.ShiftKey) Then
            ' Multiply the player's unit vector velocity by the speed we want them to move at
            player.SetVelocityX(player.GetVelocity().X * player.GetSlowSpeed())
            player.SetVelocityY(player.GetVelocity().Y * player.GetSlowSpeed())

            ' Otherwise, we want to move the player at its normal speed
        Else
            ' Multiply the player's unit vector velocity by the speed we want them to move at
            player.SetVelocityX(player.GetVelocity().X * player.GetSpeed())
            player.SetVelocityY(player.GetVelocity().Y * player.GetSpeed())
        End If

        ' Update the enemy's timer to shoot in a circle
        shootInCircleTimer += Game.SKIP_TICKS
        ' If the enemy's health is between 75% to 100%
        If enemy.GetHealthPercent() >= 75 Then
            ' Make the enemy shoot a series of bullets towards the player while they are moving to a point
            If enemy.IsMovingToAPoint() Then
                ' Make sure we don't spawn more than 5 emitters
                If listOfEnemyBulletEmitters.Count < 5 Then
                    ' Increment the shoot to player timer
                    shootToPlayerTimer += Game.SKIP_TICKS

                    ' If the shootToPlayer timer is greater than 100 milliseconds, we call the function
                    ' to shoot a series of bullets towards the player
                    If shootToPlayerTimer >= 100 Then
                        listOfEnemyBulletEmitters.Add(enemy.ShootToPlayer(player.GetPosition()))

                        ' Reset the timer
                        shootToPlayerTimer = 0
                    End If
                End If

                ' Otherwise, if the enemy is waiting at a point
            Else
                ' Shoot bullets around the enemy in a circle after 3 seconds of waiting at the point
                If shootInCircleTimer >= 3000 Then
                    listOfEnemyBulletEmitters.Add(enemy.ShootAroundEnemy())

                    ' Reset the timer
                    shootInCircleTimer = 0
                End If
            End If

            ' Otherwise, if the enemy's health is 50% to 75%
        ElseIf enemy.GetHealthPercent() >= 50 Then
            ' If the enemy is NOT moving to a point (waiting at a point)
            If Not enemy.IsMovingToAPoint() Then

                ' Shoot in a circle after 3 seconds
                If shootInCircleTimer >= 3000 Then
                    ' Make the enemy shoot in a spiral
                    listOfEnemyBulletEmitters.Add(enemy.ShootSpiral())
                    ' Make the enemy shoot in a circle around him as well as make him shoot 64 bullets
                    enemy.ShootInCircle(My.Resources.EnemyBulletCircleRed, entityManager.GetEntityList(), 64)

                    ' Reset the timer
                    shootInCircleTimer = 0
                End If
            End If

            ' Otherwise, if the enemy's health is 25% to 50%
        ElseIf enemy.GetHealthPercent() >= 25 Then

            ' If the enemy is NOT moving to a point (waiting at a point)
            If Not enemy.IsMovingToAPoint() Then
                ' Shoot in a circle after 3 seconds
                If shootInCircleTimer >= 3000 Then
                    ' Make the enemy shoot in a spiral (version 2)
                    listOfEnemyBulletEmitters.Add(enemy.ShootSpiral2())

                    ' Reset the timer
                    shootInCircleTimer = 0
                End If
            End If

            ' Otherwise, if the enemy's health is below 25%
        ElseIf enemy.GetHealthPercent() < 25 Then
            ' Update the flower pattern timer by the game tick time
            shootFlowerPatternTimer += Game.SKIP_TICKS

            ' Make sure we stop the enemy's movement and make him move to point 3 (in the middle of the screen)
            enemy.StopMovement()

            ' Check if the enemy has stopped moving as well as the point he's moving to is point 3 (in the middle of the screen)
            If enemy.GetVelocity().GetMagnitude() = 0 And enemy.GetPointGoingTo() = enemy.GetPoint3() Then
                ' Shoot in a circle around the enemy every 1 second
                If shootInCircleTimer >= 1000 Then
                    ' Shoot 45 bullets around the enemy every 1 second
                    enemy.ShootInCircle(My.Resources.EnemyBulletCircleOrange, entityManager.GetEntityList(), 44)

                    ' Reset the timer
                    shootInCircleTimer = 0
                End If

                ' Shoot the flower pattern every 5 seconds
                ' (this continually shoots in a flower pattern b/c the flower pattern lasts 5 second)
                If shootFlowerPatternTimer >= 5000 Then
                    ' Shoot the flower pattern
                    listOfEnemyBulletEmitters.AddRange(enemy.ShootFlowerPattern())

                    ' Reset the timer
                    shootFlowerPatternTimer = 0
                End If
            End If
        End If


        ' Update all the bullet emitters in the game, and if they are done emitting, remove them
        For i As Integer = listOfEnemyBulletEmitters.Count - 1 To 0 Step -1
            ' Update emitter
            listOfEnemyBulletEmitters(i).Update(entityManager.GetEntityList())

            ' Remove emitter if it is done shooting
            If listOfEnemyBulletEmitters(i).IsDone() Then
                listOfEnemyBulletEmitters.RemoveAt(i)
            End If
        Next

        ' Check the collision between the entities in the game
        entityManager.CheckCollision()
    End Sub

    ' Render the level
    Public Sub Render(ByRef e As System.Drawing.Graphics, ByVal interpolation As Double)
        ' Draw the backgrounds
        e.DrawImage(backgroundImage, backgroundImagePos.X, backgroundImagePos.Y, 1024, 768)
        e.DrawImage(backgroundImage, backgroundImagePos2.X, backgroundImagePos2.Y, 1024, 768)

        ' Draw entities
        entityManager.Render(e, interpolation)
    End Sub
End Class
