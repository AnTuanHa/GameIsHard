Public Class QuadTree
    ' Max amount of objects we can have in a node until we have to split
    Private Const MAX_OBJECTS As Integer = 10

    ' Max amount of times we can split
    Private Const MAX_LEVEL As Integer = 5

    ' The current level of the QuadTree (tells us how many times we've split)
    Private level As Integer

    ' List of objects in the node
    Private objects As List(Of BaseObject)

    ' The boundary box in which the node takes up
    Private bounds As Rectangle

    ' Each QuadTree has 4 nodes
    Private nodes(3) As QuadTree

    Public Sub New(ByVal level As Integer, ByVal boundaryBox As Rectangle)
        Me.level = level
        objects = New List(Of BaseObject)
        bounds = boundaryBox
    End Sub

    ' Clear the quadtree
    Public Sub Clear()
        ' Clear all the objects in the list of the current node
        objects.Clear()

        ' Loop through all of the subnodes in the current node
        For i As Integer = 0 To nodes.Length - 1
            ' Make sure the subnode is not null
            If Not IsNothing(nodes(i)) Then
                ' Clear the nodes within the subnodes (calling this function again)
                ' This effectively clears all the nodes recursively
                nodes(i).Clear()

                ' Make the node null
                nodes(i) = Nothing
            End If
        Next
    End Sub

    ' Splits the node into 4 subnodes
    Private Sub Split()
        ' Get the position, width, and height to split the node into 4 subnodes
        Dim subWidth As Integer = CInt(bounds.Width / 2)
        Dim subHeight As Integer = CInt(bounds.Height / 2)
        Dim x As Integer = CInt(bounds.X)
        Dim y As Integer = CInt(bounds.Y)

        ' Create the new subnodes into 4 quadrants, starting from the
        ' top right, top left, bottom left and then bottom right
        nodes(0) = New QuadTree(level + 1, New Rectangle(x + subWidth, y, subWidth, subHeight))
        nodes(1) = New QuadTree(level + 1, New Rectangle(x, y, subWidth, subHeight))
        nodes(2) = New QuadTree(level + 1, New Rectangle(x, y + subHeight, subWidth, subHeight))
        nodes(3) = New QuadTree(level + 1, New Rectangle(x + subWidth, y + subHeight, subWidth, subHeight))
    End Sub

    ' Render the nodes (for debugging purposes)
    Public Sub RenderNodes(ByRef e As Graphics)
        ' Draw the current node's rectangle
        e.DrawRectangle(Pens.White, bounds)

        ' Display how many objects are in the current node
        e.DrawString(objects.Count.ToString(), New Font("Arial", 9), Brushes.White, bounds.Location.X + bounds.Width / 2, bounds.Location.Y + bounds.Height / 2)

        ' Loop through all of the subnodes in the current node
        For i As Integer = 0 To nodes.Length - 1
            ' Make sure the subnode is not null
            If Not IsNothing(nodes(i)) Then
                ' Call this function to render the nodes within the subnodes
                nodes(i).RenderNodes(e)
            End If
        Next
    End Sub

    ' Determine which node the object belongs to.
    ' -1 means that the object cannot completely fit within a child node
    ' and is therefore part of the parent node
    Private Function GetIndex(ByVal rect As Rectangle) As Integer
        ' Defaultly set the index to be -1 so that it is part of the parent node
        Dim index As Integer = -1

        ' Get the vertical and horizontal midpoint of the current node
        Dim verticalMidpoint As Double = bounds.X + (bounds.Width / 2)
        Dim horizontalMidpoint As Double = bounds.Y + (bounds.Height / 2)

        ' Determine whether the object can completely fit within the top quadrants
        Dim topQuadrant As Boolean = (rect.Y < horizontalMidpoint) And (rect.Y + rect.Height < horizontalMidpoint)
        ' Determine whether the object can completely fit within the bottom quadrants
        Dim bottomQuadrant As Boolean = (rect.Y > horizontalMidpoint)

        ' Determine whether the object can completely fit within the left quadrants
        If (rect.X < verticalMidpoint) And (rect.X + rect.Width < verticalMidpoint) Then
            If topQuadrant Then
                ' Object is in the top left quadrant
                index = 1
            ElseIf bottomQuadrant Then
                ' Object is in the bottom left quadrant
                index = 2
            End If

            ' Determine whether the object can completely fit within the right quadrants
        ElseIf (rect.X > verticalMidpoint) Then
            If topQuadrant Then
                ' Object is in the top right quadrant
                index = 0
            ElseIf bottomQuadrant Then
                ' Object is in the bottom right quadrant
                index = 3
            End If
        End If

        ' Return the index
        Return index
    End Function

    ' Insert the object into the quadtree. If the node
    ' exceeds the capacity, it will split and add all
    ' objects to their corresponding nodes.
    Public Sub Insert(ByVal obj As BaseObject)
        ' Check if the first node is not null
        ' The node could've been any other node
        ' This checks if we have splitted quadtree into nodes yet
        If Not IsNothing(nodes(0)) Then
            ' Get the index of where the object should be placed (0, 1, 2, or 3)
            Dim index As Integer = GetIndex(obj.GetRectangle())

            ' If the index is not equal to -1, then insert it
            ' to the corresponding subnode.
            ' An index of -1 would mean that it does not fit within
            ' any of the nodes (it could be in several nodes)
            ' Thus, -1 would mean that it should be placed within
            ' the parent node
            If index <> -1 Then
                ' Insert into the corresponding subnode
                ' (calling this function again)
                nodes(index).Insert(obj)

                ' Exits the function
                Return
            End If
        End If

        ' This is the code run if the index is equal to -1
        ' Meaning that it should be put into the parent node
        ' Also, this is the code run if the object is in its correct node

        ' Add the object to the list of objects for the current node
        objects.Add(obj)

        ' Check if there are too many objects in the list and make sure
        ' that we are not passed the maximum amount of splitting.
        ' If so, we should split and add the objects to its corresponding subnode
        If objects.Count > MAX_OBJECTS And level < MAX_LEVEL Then
            ' Check if the nodes are null, which means that if the nodes are
            ' null, then we haven't split the quadtrees into nodes yet.
            If IsNothing(nodes(0)) Then
                ' Split and create its corresponding subnodes
                Split()
            End If

            ' Loop through all of the objects in the list and add them to
            ' their corresponding subnode
            Dim i As Integer = 0
            While i < objects.Count
                ' Get the index of where the object should go
                Dim index As Integer = GetIndex(objects(i).GetRectangle())

                ' If the index is either 0, 1, 2, or 3, then we
                ' insert the object into its corresponding subnode
                If index <> -1 Then
                    ' Insert the object into its corresponding subnode
                    nodes(index).Insert(objects(i))
                    ' Remove the object in the parent node
                    objects.RemoveAt(i)

                    ' Otherwise, we skip it and leave it in the parent node
                Else
                    i += 1
                End If
            End While
        End If
    End Sub

    ' Get a list of objects that are in the same node as the pRect given
    Public Sub Retrieve(ByRef returnObjects As List(Of BaseObject), ByVal rect As Rectangle)
        ' Get the index of where the requested object is
        Dim index As Integer = GetIndex(rect)

        ' If the index that we got is 0, 1, 2, or 3, then we must
        ' call this function again to go down the subnodes until we are
        ' at the smallest node the requested object can fit.
        ' Also, make sure the subnodes of the current node is
        ' not null. Which if it is, then the requested object
        ' is in the correct node
        If Not index = -1 And Not IsNothing(nodes(0)) Then
            nodes(index).Retrieve(returnObjects, rect)
        End If

        ' Finally, return all of the objects in the same node
        ' as requested object
        returnObjects.AddRange(objects)
    End Sub
End Class
