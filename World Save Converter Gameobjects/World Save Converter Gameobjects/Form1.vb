Imports System.Text.RegularExpressions

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim line As String = ""
        Dim obj As gObject = Nothing
        Using sr As New IO.StreamReader("C:\world.save")
            line = sr.ReadLine()
            While line IsNot Nothing

                If line <> vbNullString Then
                    If (line = "[OBJECT]") Then
                        obj = gObject.Create()
                    Else
                        If (obj IsNot Nothing) Then

                            parseLine(line, obj)

                        End If
                    End If
                End If

                line = sr.ReadLine()

            End While
        End Using

        Dim guid As Integer = 1
        For Each gobj As gObject In gObject.objLst
            If (gobj.type <> 5) Then
                Continue For
            End If

            Using sw As New IO.StreamWriter(My.Computer.FileSystem.SpecialDirectories.Desktop & "/gameobject.sql", True)
                sw.WriteLine("REPLACE INTO gameobject VALUES('" & guid & "','" & gobj.objId & "','" & gobj.map & "','" & gobj.xyz(0) & "','" & gobj.xyz(1) & "','" & gobj.xyz(2) & "','" & gobj.xyz(3) & "','" & gobj.rot(0) & "','" & gobj.rot(1) & "','" & gobj.rot(2) & "','" & gobj.rot(3) & "','" & obj.spawntime & "','100','1');")
            End Using
            guid += 1
        Next
    End Sub

    Private Sub parseLine(ByVal str As String, ByRef obj As gObject)

        If obj IsNot Nothing Then

            str = Split(str, "//")(0)  'Remove line comments
            str = str.Trim()  'Remove leading or trailing spaces

            Select Case True
                Case str.StartsWith("ENTRY=")
                    obj.objId = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("TYPE=")
                    obj.type = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("SPAWNTIME=")
                    obj.spawntime = Split(str.Substring(str.IndexOf("=") + 1))(0)

                Case str.StartsWith("MAP=")
                    obj.map = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("XYZ=")
                    Dim i() As String = Split(str.Substring(str.IndexOf("=") + 1))
                    obj.xyz(0) = i(0)
                    obj.xyz(1) = i(1)
                    obj.xyz(2) = i(2)
                    obj.xyz(3) = i(3)

                Case str.StartsWith("ROTATION=")
                    Dim i() As String = Split(str.Substring(str.IndexOf("=") + 1))
                    obj.rot = {i(0), i(1), i(2), i(3)}
            End Select
        End If
    End Sub
End Class

Public Class gObject

    Private _id As Integer
    Public Property objId As Integer
        Get
            Return _id
        End Get
        Set(value As Integer)
            _id = value
        End Set
    End Property
    Private _type As Integer
    Public Property type() As Integer
        Get
            Return _type
        End Get
        Set(ByVal value As Integer)
            _type = value
        End Set
    End Property
    Private _spawntime As Integer
    Public Property spawntime() As Long
        Get
            Return _spawntime
        End Get
        Set(value As Long)
            _spawntime = value
        End Set
    End Property
    Private _map As Integer
    Public Property map As Integer
        Get
            Return _map
        End Get
        Set(value As Integer)
            _map = value
        End Set
    End Property
    Private _xyz(3) As Decimal
    Public Property xyz() As Decimal()
        Get
            Return _xyz
        End Get
        Set(value As Decimal())
            _xyz = value
        End Set
    End Property
    Private _rot(3) As Decimal
    Public Property rot() As Decimal()
        Get
            Return _rot
        End Get
        Set(value As Decimal())
            _rot = value
        End Set
    End Property

    Sub New()
        _id = 0
        _type = 0
        _spawntime = 900
        _map = 0
        _xyz = {0, 0, 0, 0}
        _rot = {0, 0, 0, 0}
    End Sub

    Public Shared objLst As New List(Of gObject)
    Public Shared Function Create() As gObject
        Dim obj As New gObject

        objLst.Add(obj)

        Return obj
    End Function
End Class