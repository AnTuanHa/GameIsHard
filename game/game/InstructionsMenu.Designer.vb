<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InstructionsMenu
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(InstructionsMenu))
        Me.returnToMainMenuButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'returnToMainMenuButton
        '
        Me.returnToMainMenuButton.Font = New System.Drawing.Font("Arial", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.returnToMainMenuButton.Location = New System.Drawing.Point(261, 312)
        Me.returnToMainMenuButton.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.returnToMainMenuButton.Name = "returnToMainMenuButton"
        Me.returnToMainMenuButton.Size = New System.Drawing.Size(127, 52)
        Me.returnToMainMenuButton.TabIndex = 1
        Me.returnToMainMenuButton.Text = "Main Menu"
        Me.returnToMainMenuButton.UseVisualStyleBackColor = True
        '
        'InstructionsMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.game.My.Resources.Resources.InstructionsMenu
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(634, 372)
        Me.Controls.Add(Me.returnToMainMenuButton)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.Name = "InstructionsMenu"
        Me.Text = "Instructions"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents returnToMainMenuButton As System.Windows.Forms.Button
End Class
