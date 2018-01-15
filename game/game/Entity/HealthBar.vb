Public Class HealthBar

    ' Max health
    Private maxHealth As Integer

    ' Current health
    Private currentHealth As Integer

    ' Max Health Rectangle (Red bar)
    Private maxHealthRect As Rectangle

    ' Current health rectangle (green bar)
    Private currentHealthRect As Rectangle

    ' Health bar's width and height
    Private healthBarWidth
    Private healthBarHeight

    ' Initialize the health bar
    Public Sub New(ByVal maxHealth As Integer, ByVal position As Point, ByVal width As Integer, ByVal height As Integer)
        ' Set the max and current health
        Me.maxHealth = maxHealth
        currentHealth = maxHealth

        ' Set the width and height of the health bar's rectangle
        healthBarWidth = width
        healthBarHeight = height

        ' Set the max health rectangle
        maxHealthRect = New Rectangle(position.X, position.Y, healthBarWidth, healthBarHeight)

        ' Set the current health rectangle to be the same size as maxHealthRect
        currentHealthRect = maxHealthRect
    End Sub

    ' Remove health in the healthbar
    Public Sub RemoveHealth(ByVal dmg As Integer)
        ' Decrease the current health by the damage
        currentHealth -= dmg

        ' Get the factor in which we scale the healthbar rectangle by
        ' This is because our hitpoints may be different to the size of the
        ' health bar's width
        Dim factor As Double = currentHealth / maxHealth
        currentHealthRect.Width = factor * healthBarWidth
    End Sub

    ' Return the current health in the healthbar
    Public Function GetCurrentHealth() As Integer
        Return currentHealth
    End Function

    ' Return the health percent in the healthbar
    Public Function GetCurrentHealthPercent() As Integer
        Dim hpPercent As Double = (currentHealth / maxHealth) * 100
        Return hpPercent
    End Function

    ' Render the healthbars
    Public Sub Render(ByVal e As System.Drawing.Graphics)
        e.FillRectangle(Brushes.Red, maxHealthRect)
        e.FillRectangle(Brushes.Green, currentHealthRect)
    End Sub
End Class
