Public Class Game

    ' Determine if the game is running or not
    Private isRunning As Boolean = True

    ' Fixed Time Step variables
    ' 
    ' Constantly updates the game at the specified tick rate, which is 60. By doing so,
    ' we decouple the rendering speed from the update speed. As a result, we render
    ' the objects as fast as we can while interpolating the previous positions to
    ' the current position. Additionally, we update the actual game logic at a constant rate of
    ' 60 ticks per second. Thus, if the game is run on a computer that can't render fast enough,
    ' the game objects will still be accurately updated because the update calls are not
    ' computationally taxing. Also, if the game is run on a computer that is really fast, the
    ' interpolation will make the movement really smooth.
    Private gameTicker As Stopwatch
    Public Const TICKS_PER_SECOND As Integer = 60
    Public Const SKIP_TICKS As Integer = 1000 / TICKS_PER_SECOND
    Private Const MAX_FRAMESKIP As Integer = 5
    Private next_game_tick As ULong = 0
    Private numOfLoops As Integer
    Private interpolation As Double

    ' Create the level
    Private level As Level

    ' Double buffering, allow game to be more smoother
    Public Sub New()
        MyBase.New()
        InitializeComponent()
        SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint, True)
        SetStyle(ControlStyles.ResizeRedraw, True)
    End Sub

    ' Runs this code when form is opened
    Private Sub Game_Load() Handles Me.Load
        ' Show and focus the form
        Me.Show()
        Me.Focus()
        
        ' Set the display size to be 1024x768 and center the game on the user's screen
        Me.ClientSize = New Size(1024, 768)
        Dim centerOfScreenX As Integer = Screen.PrimaryScreen.WorkingArea.Width / 2 - Me.Width / 2
        Dim centerOfScreenY As Integer = Screen.PrimaryScreen.WorkingArea.Height / 2 - Me.Height / 2
        Me.Location = New Point(centerOfScreenX, centerOfScreenY)

        ' Play background music
        If Not MainMenu.GetIsGameMuted() Then
            My.Computer.Audio.Play(My.Resources.gamemusic, AudioPlayMode.BackgroundLoop)
        End If

        ' Initialize the level
        level = New Level()

        ' Start fixed time-step game loop
        gameTicker = New Stopwatch()
        gameTicker.Start()
        StartGameLoop()
    End Sub

    Private Sub StartGameLoop()
        Do While isRunning
            ' Get events (mouse, keyboard, etc)
            SendKeys.Flush()

            ' Get the elapsed time in milliseconds after program has been ran
            Dim currentTime As ULong = gameTicker.ElapsedMilliseconds

            ' Keep track of how many loops that have been made in the while loop
            numOfLoops = 0

            ' Keep updating the game as long as the elapsed time is ahead of
            ' the next game tick (the time it should take for every frame)
            ' If we're falling behind more than 5 frames, then don't bother updating
            While currentTime > next_game_tick And numOfLoops < MAX_FRAMESKIP
                UpdateGame()

                ' Increase next_game_tick by the constant skip_ticks
                ' This is how the game keeps track of what time it should be at right now
                ' Hence the "Fixed Time Step" game loop we are using
                next_game_tick += SKIP_TICKS
                numOfLoops += 1
            End While

            ' Interpolation value, from 0 to 1, which is passed onto the rendering function
            ' The interpolation value is the value that we use to interpolate between the
            ' previous position to the current position
            interpolation = (currentTime + SKIP_TICKS - next_game_tick) / SKIP_TICKS

            ' Freezes (Suspend) the form from drawing anything at all
            ' Calls the Game_Paint function (Render Game) via the Invalidate function
            ' Finally, draw everything immediately onto the form
            ' This effectively stops any "flicker" or "tearing" on the screen when drawing things
            Me.SuspendLayout()
            Me.Invalidate()
            Me.ResumeLayout()
        Loop
    End Sub

    ' Update the game's logic
    Private Sub UpdateGame()
        level.Update()
    End Sub

    ' Render game
    Private Sub Game_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        e.Graphics.FillRectangle(Brushes.Black, Me.DisplayRectangle)
        level.Render(e.Graphics, interpolation)
    End Sub

    ' Close the game
    Private Sub Game_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' Stop playing sounds and show main menu
        My.Computer.Audio.Stop()
        gameTicker.Stop()
        isRunning = False
        MainMenu.Show()
    End Sub

    ' Handle KeyPress events
    Public Sub Game_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles Me.KeyPress
        ' Pause game if ESC is pressed
        If e.KeyChar = ChrW(Keys.Escape) Then
            TogglePause()
        End If
    End Sub

    ' Toggle between whether the game is paused or not
    Public Sub TogglePause()
        If isRunning Then
            isRunning = False
            gameTicker.Stop()
        Else
            isRunning = True
            gameTicker.Start()
            StartGameLoop()
        End If
    End Sub
End Class
