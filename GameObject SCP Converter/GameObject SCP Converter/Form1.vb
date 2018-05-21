Imports System.Text.RegularExpressions

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim line As String = ""
        Dim rgx As New Regex("(?<=\[gameobj )\d+(?=\])", RegexOptions.IgnoreCase)
        Dim obj As gObject = Nothing
        Using sr As New IO.StreamReader("C:\gameobjects.scp")
            line = sr.ReadLine()
            While line IsNot Nothing

                If line <> vbNullString Then
                    If (rgx.IsMatch(line) = True) Then
                        obj = gObject.Create(rgx.Match(line).Value)
                    Else
                        If (obj IsNot Nothing) Then

                            parseLine(line, obj)

                        End If
                    End If
                End If

                line = sr.ReadLine()

            End While
        End Using

        For Each gobj As gObject In gObject.objLst
            Using sw As New IO.StreamWriter(My.Computer.FileSystem.SpecialDirectories.Desktop & "/gameobjects.sql", True)
                sw.WriteLine("REPLACE INTO gameobject_template (entry,type,displayId,name,faction,flags,size,data0,data1,data2,data3,data4,data5,data6,data7,data8,data9,data10,data11,data12) VALUES ('" & gobj.objId & "','" & gobj.type & "','" & gobj.modelId & "',""" & gobj.objName.Replace("""", """""") & """,'" & gobj.faction & "','" & gobj.flags & "','" & gobj.scale & "','" & gobj.data(0) & "','" & gobj.data(1) & "','" & gobj.data(2) & "','" & gobj.data(3) & "','" & gobj.data(4) & "','" & gobj.data(5) & "','" & gobj.data(6) & "','" & gobj.data(7) & "','" & gobj.data(8) & "','" & gobj.data(9) & "','" & gobj.data(10) & "','" & gobj.data(11) & "','" & gobj.data(12) & "');")
            End Using
        Next
    End Sub

    Private Sub parseLine(ByVal str As String, ByRef obj As gObject)

        If obj IsNot Nothing Then

            str = Split(str, "//")(0)  'Remove line comments
            str = str.Trim()  'Remove leading or trailing spaces

            Select Case True
                Case str.StartsWith("name=")
                    obj.objName = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("model=")
                    obj.modelId = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("faction=")
                    obj.faction = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("size=")
                    obj.scale = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("type=")
                    obj.type = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("flags=")
                    obj.flags = Val("&H" & str.Substring(str.IndexOf("=") + 1))

                Case str.StartsWith("sound0=")
                    obj.data(0) = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("sound1=")
                    Dim i As Long = str.Substring(str.IndexOf("=") + 1)
                    obj.data(1) = IIf(i < 0, 0, i)

                Case str.StartsWith("sound2=")
                    obj.data(2) = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("sound3=")
                    obj.data(3) = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("sound4=")
                    obj.data(4) = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("sound5=")
                    obj.data(5) = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("sound6=")
                    obj.data(6) = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("sound7=")
                    obj.data(7) = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("sound8=")
                    obj.data(8) = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("sound9=")
                    obj.data(9) = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("sound10=")
                    obj.data(10) = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("sound11=")
                    obj.data(11) = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("sound12=")
                    obj.data(12) = str.Substring(str.IndexOf("=") + 1)
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
    Private _data(12) As Long
    Public Property data() As Long()
        Get
            Return _data
        End Get
        Set(value As Long())
            _data = value
        End Set
    End Property


    Sub New()
        _id = 0
        _name = ""
        _modelId = 0
        _faction = 0
        _scale = 1.0
        _type = 0
        _flags = 0
        _data = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    End Sub

    Public Shared objLst As New List(Of gObject)
    Public Shared Function Create(ByVal id As Integer) As gObject
        Dim obj As New gObject
        obj._id = id

        objLst.Add(obj)

        Return obj
    End Function
End Class