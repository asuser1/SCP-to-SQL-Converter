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
            If (gobj.type <> 3) Then
                Continue For
            End If

            Using sw As New IO.StreamWriter(My.Computer.FileSystem.SpecialDirectories.Desktop & "/creatures.sql", True)
                While gobj.objNum > 0
                    sw.WriteLine("REPLACE INTO creature VALUES('" & guid & "','" & gobj.objId & "','" & gobj.map & "','" & gobj.modelId & "','0','" & gobj.xyz(0) & "','" & gobj.xyz(1) & "','" & gobj.xyz(2) & "','" & gobj.xyz(3) & "','" & gobj.spawntime & "','" & gobj.spawndist & "','0','" & gobj.health & "','" & gobj.power & "','0','" & IIf(gobj.spawndist > 0, 1, 0) & "');")
                    gobj.objNum -= 1
                    guid += 1
                End While
            End Using
        Next
    End Sub

    Private Sub parseLine(ByVal str As String, ByRef obj As gObject)

        If obj IsNot Nothing Then

            str = Split(str, "//")(0)  'Remove line comments
            str = str.Trim()  'Remove leading or trailing spaces

            Select Case True
                Case str.StartsWith("LINK=")
                    gObject.objLst.Remove(obj)
                    obj = Nothing

                Case str.StartsWith("ENTRY=")
                    obj.objId = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("MODEL=")
                    obj.modelId = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("LEVEL=")
                    obj.level = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("FACTION=")
                    obj.faction = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("SIZE=")
                    obj.scale = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("TYPE=")
                    obj.type = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("FLAGS=")
                    obj.flags = Val("&H" & str.Substring(str.IndexOf("=") + 1))

                Case str.StartsWith("MAXHEALTH=")
                    obj.health = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("MAXPOWER=")
                    obj.power = Split(str.Substring(str.IndexOf("=") + 1))(0)

                Case str.StartsWith("SPAWNTIME=")
                    obj.spawntime = Split(str.Substring(str.IndexOf("=") + 1))(0)

                Case str.StartsWith("SPAWNDIST=")
                    obj.spawndist = Split(str.Substring(str.IndexOf("=") + 1))(1)

                Case str.StartsWith("MAP=")
                    obj.map = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("XYZ=")
                    Dim i() As String = Split(str.Substring(str.IndexOf("=") + 1))
                    obj.xyz(0) = i(0)
                    obj.xyz(1) = i(1)
                    obj.xyz(2) = i(2)
                    obj.xyz(3) = i(3)

                Case str.StartsWith("SPAWN=")
                    Dim i() As String = Split(str.Substring(str.IndexOf("=") + 1))
                    obj.objId = i(0)
                    obj.objNum = i(1)
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
    Private _num As Integer
    Public Property objNum As Integer
        Get
            Return _num
        End Get
        Set(value As Integer)
            _num = value
        End Set
    End Property
    Private _name As String
    Public Property objName As String
        Get
            Return _name
        End Get
        Set(value As String)
            _name = value
        End Set
    End Property
    Private _modelId As Integer
    Public Property modelId() As Integer
        Get
            Return _modelId
        End Get
        Set(ByVal value As Integer)
            _modelId = value
        End Set
    End Property
    Private _faction As Integer
    Public Property faction() As Integer
        Get
            Return _faction
        End Get
        Set(ByVal value As Integer)
            _faction = value
        End Set
    End Property
    Private _scale As Decimal
    Public Property scale() As Decimal
        Get
            Return _scale
        End Get
        Set(ByVal value As Decimal)
            _scale = value
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
    Private _flags As Long
    Public Property flags() As Long
        Get
            Return _flags
        End Get
        Set(ByVal value As Long)
            _flags = value
        End Set
    End Property
    Private _level As Integer
    Public Property level() As Integer
        Get
            Return _level
        End Get
        Set(value As Integer)
            _level = value
        End Set
    End Property
    Private _health As Long
    Public Property health() As Long
        Get
            Return _health
        End Get
        Set(value As Long)
            _health = value
        End Set
    End Property
    Private _power As Long
    Public Property power() As Long
        Get
            Return _power
        End Get
        Set(value As Long)
            _power = value
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
    Private _spawndist As Decimal
    Public Property spawndist As Decimal
        Get
            Return _spawndist
        End Get
        Set(value As Decimal)
            _spawndist = value
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


    Sub New()
        _id = 0
        _num = 1
        _name = ""
        _modelId = 0
        _faction = 0
        _scale = 1.0
        _type = 0
        _flags = 0
        _level = 0
        _health = 0
        _power = 0
        _spawntime = 0
        _spawndist = 0
        _map = 0
        _xyz = {0, 0, 0, 0}
    End Sub

    Public Shared objLst As New List(Of gObject)
    Public Shared Function Create() As gObject
        Dim obj As New gObject

        objLst.Add(obj)

        Return obj
    End Function
End Class