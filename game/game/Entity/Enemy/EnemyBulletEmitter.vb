Public Class EnemyBulletEmitter

    ' Store the image we want to use for the bullets that we're going to emit
    Private image As Bitmap

    ' Position of the emitter
    Private position As Vector

    ' Speed of the bullet that they will move at
    Private speed As Double

    ' Keep track of the interval to shoot the bullets at
    Private intervalToShootAt As Integer

    ' Bullet timer to know when it is time to shoot
    Private bulletTimer As Integer

    ' Keep track of how many bullets already shot and bullets to shoot
    ' This will tell us when the emitter is done its job and can dispose itself
    Private numofBulletsShot As Integer
    Private numOfBulletsToShoot As Integer

    ' Angle in radians to rotate the emission of bullets at
    Private angleRad As Double
    ' Speed of the angle that it will change at in degrees
    Private angleChangeSpeedDeg As Double

    ' Initialize the enemy bullet emitter
    Public Sub New(ByVal bitmap As Bitmap, ByVal speed As Double, ByVal spawnPos As Vector, ByVal msBeforeDecay As Integer, ByVal numberOfBulletsToShoot As Integer, ByVal angleRadians As Double, ByVal angleChangeSpeedDegrees As Double)
        image = bitmap
        Me.speed = speed
        position = spawnPos

        angleRad = angleRadians
        angleChangeSpeedDeg = angleChangeSpeedDegrees
        numofBulletsShot = 0
        numOfBulletsToShoot = numberOfBulletsToShoot

        ' Get the lifetime of the bullet emitter in terms of the game ticker
        Dim lifetime As Integer

        ' Since the game increments at fixed intervals, we must convert the time it takes
        ' for an update to happen into real life time
        lifetime = Game.SKIP_TICKS * Game.TICKS_PER_SECOND * (msBeforeDecay / 1000)

        ' Get the interval we must shoot at for X amount of bullets in Y amount of time
        intervalToShootAt = lifetime / numberOfBulletsToShoot

        ' Set the bulletTimer to be the interval to shoot at so we can shoot the bullet immediately at the start
        bulletTimer = intervalToShootAt
    End Sub

    Public Sub Update(ByRef listOfEntities As List(Of BaseEntity))
        ' Update bullet timer by the game ticker
        bulletTimer += Game.SKIP_TICKS

        ' If bulletTimerhas passed the time/interval to shoot at,
        ' then shoot a bullet
        If bulletTimer >= intervalToShootAt Then
            ' Create new enemy bullet and set its position
            Dim enemyBullet As BaseEnemyBullet = New BaseEnemyBullet(image, speed)
            enemyBullet.SetPosition(position.X - (enemyBullet.GetWidth() / 2), position.Y - (enemyBullet.GetHeight() / 2))
            listOfEntities.Add(enemyBullet)

            ' Update the angle the emission of bullets to shoot at
            ' by incrementing it by the change speed and converting it from
            ' degrees to radians
            angleRad += angleChangeSpeedDeg * (Math.PI / 180)

            ' Convert the angleRad into degrees
            ' Used for the transformation of the bullets
            Dim angleDegrees As Double = angleRad * (180 / Math.PI)

            ' Minus 90 degrees because our image is pointing down, not to the right
            angleDegrees -= 90

            ' If the angle is negative, change it into 0-359 format, not -180 and 180.
            If angleDegrees < 0 Then
                angleDegrees += 360
            End If

            ' Set the rotation of the bitmap
            enemyBullet.SetImageRotation(angleDegrees)

            ' Set the acceleration of the enemyBullet to be in the
            ' direction of the target
            Dim ax As Double = Math.Cos(angleRad) * 0.1
            Dim ay As Double = Math.Sin(angleRad) * 0.1
            enemyBullet.SetAcceleration(ax, ay)

            ' Reset the timer and increment how many bullets we've shot
            bulletTimer = 0
            numofBulletsShot += 1
        End If
    End Sub

    ' Return true or false whether or not we are done emitting our bullets
    Public Function IsDone() As Boolean
        ' Return true if we've shot the number of bullets we requested
        If numofBulletsShot >= numOfBulletsToShoot Then
            Return True
        End If

        ' Otherwise, return false
        Return False
    End Function
End Class
