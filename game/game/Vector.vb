Public Structure Vector
    ' Store the X and Y component of the 2D vector
    Public X As Double
    Public Y As Double

    ' Initialize the vector
    Public Sub New(ByVal x As Double, ByVal y As Double)
        Me.X = x
        Me.Y = y
    End Sub

    ' Add a vector with a vector and return the result
    Public Shared Operator +(ByVal vector1 As Vector, ByVal vector2 As Vector) As Vector
        Return New Vector(vector1.X + vector2.X, vector1.Y + vector2.Y)
    End Operator

    ' Subtract a vector with a vector and return the result
    Public Shared Operator -(ByVal vector1 As Vector, ByVal vector2 As Vector) As Vector
        Return New Vector(vector1.X - vector2.X, vector1.Y - vector2.Y)
    End Operator

    ' Return true if 2 vectors are equal, otherwise return true
    Public Shared Operator =(ByVal vector1 As Vector, ByVal vector2 As Vector) As Boolean
        If vector1.X = vector2.X And vector1.Y = vector2.Y Then
            Return True
        End If

        Return False
    End Operator

    ' Return false if the 2 vectors are equal, otherwise return true
    ' This is the opposite of the equal operator
    Public Shared Operator <>(ByVal vector1 As Vector, ByVal vector2 As Vector) As Boolean
        If vector1.X = vector2.X And vector1.Y = vector2.Y Then
            Return False
        End If

        Return True
    End Operator

    ' Convert vector to a Point
    ' This is used to pass a vector datatype into VB's functions,
    ' since they only accept the datatype Point
    ' Converting a vector to a point will lose precision
    ' This is because a vector has components of the double datatype
    ' whereas a Point has components of the Integer datatype
    Public Function ToPoint() As Point
        Return New Point(X, Y)
    End Function

    ' Return the magnitude of the vector
    Public Function GetMagnitude() As Double
        Return Math.Sqrt(X * X + Y * Y)
    End Function
End Structure