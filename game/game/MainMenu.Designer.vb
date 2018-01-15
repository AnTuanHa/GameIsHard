<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainMenu
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainMenu))
        Me.startGameButton = New System.Windows.Forms.Button()
        Me.quitGameButton = New System.Windows.Forms.Button()
        Me.muteGameCheckBox = New System.Windows.Forms.CheckBox()
        Me.instructionsButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'startGameButton
        '
        Me.startGameButton.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.startGameButton.Location = New System.Drawing.Point(258, 189)
        Me.startGameButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.startGameButton.Name = "startGameButton"
        Me.startGameButton.Size = New System.Drawing.Size(127, 52)
        Me.startGameButton.TabIndex = 0
        Me.startGameButton.Text = "Start Game"
        Me.startGameButton.UseVisualStyleBackColor = True
        '
        'quitGameButton
        '
        Me.quitGameButton.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.quitGameButton.Location = New System.Drawing.Point(258, 307)
        Me.quitGameButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.quitGameButton.Name = "quitGameButton"
        Me.quitGameButton.Size = New System.Drawing.Size(127, 52)
        Me.quitGameButton.TabIndex = 1
        Me.quitGameButton.Text = "Quit Game"
        Me.quitGameButton.UseVisualStyleBackColor = True
        '
        'muteGameCheckBox
        '
        Me.muteGameCheckBox.AutoSize = True
        Me.muteGameCheckBox.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.muteGameCheckBox.Location = New System.Drawing.Point(258, 151)
        Me.muteGameCheckBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.muteGameCheckBox.Name = "muteGameCheckBox"
        Me.muteGameCheckBox.Size = New System.Drawing.Size(127, 26)
        Me.muteGameCheckBox.TabIndex = 2
        Me.muteGameCheckBox.Text = "Mute Game"
        Me.muteGameCheckBox.UseVisualStyleBackColor = True
        '
        'instructionsButton
        '
        Me.instructionsButton.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.instructionsButton.Location = New System.Drawing.Point(258, 248)
        Me.instructionsButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.instructionsButton.Name = "instructionsButton"
        Me.instructionsButton.Size = New System.Drawing.Size(127, 52)
        Me.instructionsButton.TabIndex = 3
        Me.instructionsButton.Text = "Instructions"
        Me.instructionsButton.UseVisualStyleBackColor = True
        '
        'MainMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.game.My.Resources.Resources.MainMenu
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(634, 372)
        Me.Controls.Add(Me.instructionsButton)
        Me.Controls.Add(Me.muteGameCheckBox)
        Me.Controls.Add(Me.quitGameButton)
        Me.Controls.Add(Me.startGameButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.Name = "MainMenu"
        Me.Text = "Game is Hard"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents startGameButton As System.Windows.Forms.Button
    Friend WithEvents quitGameButton As System.Windows.Forms.Button
    Friend WithEvents muteGameCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents instructionsButton As System.Windows.Forms.Button
End Class
