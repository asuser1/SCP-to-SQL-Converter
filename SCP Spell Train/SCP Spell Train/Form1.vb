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

        Using sw As New IO.StreamWriter(My.Computer.FileSystem.SpecialDirectories.Desktop & "/train.sql", True)
            For Each cre As creature In creature.ctrLst
                If cre.getTrain.Count > 0 Then
                    sw.WriteLine("UPDATE creature_template SET TrainerTemplateId = '" & cre.ctrId & "' WHERE Entry = '" & cre.ctrId & "';")
                End If

                For Each spl As Integer In cre.getTrain
                    sw.WriteLine("REPLACE INTO npc_trainer VALUES ('" & cre.ctrId & "','" & spl & "','0','0','0','0');")
                Next
            Next
        End Using
    End Sub

    Private Sub parseLine(ByVal str As String, ByRef ctr As creature)

        If ctr IsNot Nothing Then

            str = Split(str, "//")(0)  'Remove line comments
            str = str.Trim()  'Remove leading or trailing spaces

            Select Case True
                Case str.StartsWith("train=")
                    ctr.addSpell(str.Substring(str.IndexOf("=") + 1))
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
    Private trains As New List(Of Integer)
    Public Function getTrain() As List(Of Integer)
        Return trains
    End Function

    Sub addSpell(ByVal itm As Integer)
        trains.Add(itm)
    End Sub

    Sub New()
        _id = 0
    End Sub

    Public Shared ctrLst As New List(Of creature)
    Public Shared Function Create(ByVal id As Integer) As creature
        Dim ctr As New creature
        ctr._id = id

        ctrLst.Add(ctr)

        Return ctr
    End Function
End Class