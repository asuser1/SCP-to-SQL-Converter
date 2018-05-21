Imports System.Text.RegularExpressions

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim line As String = ""
        Dim rgx As New Regex("(?<=\[creature )\d+(?=\])", RegexOptions.IgnoreCase)
        Dim ctr As creature = Nothing
        Using sr As New IO.StreamReader("C:\creatures.scp")
            line = sr.ReadLine()
            While line IsNot Nothing

                If line <> vbNullString Then
                    If (rgx.IsMatch(line) = True) Then
                        ctr = creature.Create(rgx.Match(line).Value)
                    Else
                        If (ctr IsNot Nothing) Then

                            parseLine(line, ctr)

                        End If
                    End If
                End If

                line = sr.ReadLine()

            End While
        End Using

        For Each cre As creature In creature.ctrLst
            Using sw As New IO.StreamWriter(My.Computer.FileSystem.SpecialDirectories.Desktop & "/creatures.sql", True)
                If cre.equip(0) <> 0 Or cre.equip(1) <> 0 Or cre.equip(2) <> 0 Then
                    sw.WriteLine("REPLACE INTO creature_equip_template VALUES ('" & cre.ctrId & "','" & cre.equip(0) & "','" & cre.equip(1) & "','" & cre.equip(2) & "');")
                    sw.WriteLine("UPDATE creature_template SET EquipmentTemplateId = " & cre.ctrId & " WHERE entry = " & cre.ctrId & ";")
                End If
            End Using
        Next
    End Sub

    Private Sub parseLine(ByVal str As String, ByRef ctr As creature)

        If ctr IsNot Nothing Then

            str = Split(str, "//")(0)  'Remove line comments
            str = str.Trim()  'Remove leading or trailing spaces

            Select Case True
                Case str.StartsWith("equipmodel=0")
                    Dim spl() As String = Split(str.Substring(str.IndexOf("=") + 1))
                    ctr.equip(0) = spl(1)

                Case str.StartsWith("equipmodel=1")
                    Dim spl() As String = Split(str.Substring(str.IndexOf("=") + 1))
                    ctr.equip(1) = spl(1)

                Case str.StartsWith("equipmodel=2")
                    Dim spl() As String = Split(str.Substring(str.IndexOf("=") + 1))
                    ctr.equip(2) = spl(1)
            End Select
        End If
    End Sub
End Class

Public Class creature

    Private _id As Integer
    Public Property ctrId As Integer
        Get
            Return _id
        End Get
        Set(value As Integer)
            _id = value
        End Set
    End Property
    Private _equip(2) As Integer
    Public Property equip As Integer()
        Get
            Return _equip
        End Get
        Set(value As Integer())
            _equip = value
        End Set
    End Property



    Sub New()
        _id = 0
        _equip = {0, 0, 0}
    End Sub

    Public Shared ctrLst As New List(Of creature)
    Public Shared Function Create(ByVal id As Integer) As creature
        Dim ctr As New creature
        ctr._id = id

        ctrLst.Add(ctr)

        Return ctr
    End Function
End Class
