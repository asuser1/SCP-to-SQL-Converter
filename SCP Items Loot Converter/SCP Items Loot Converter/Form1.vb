Imports System.Text.RegularExpressions

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim line As String = ""
        Dim rgx As New Regex("(?<=\[item )\d+(?=\])", RegexOptions.IgnoreCase)
        Dim itm As item = Nothing
        Using sr As New IO.StreamReader("C:\items.scp")
            line = sr.ReadLine()
            While line IsNot Nothing

                If line <> vbNullString Then
                    If (rgx.IsMatch(line) = True) Then
                        itm = item.Create(rgx.Match(line).Value)
                    Else
                        If (itm IsNot Nothing) Then

                            parseLine(line, itm)

                        End If
                    End If
                End If

                line = sr.ReadLine()

            End While
        End Using

        Using sw As New IO.StreamWriter(My.Computer.FileSystem.SpecialDirectories.Desktop & "/loot.sql", True)
            For Each itr As item In item.items
                For Each kvp As KeyValuePair(Of Integer, item.lootItem) In itr.getLoot
                    sw.WriteLine("REPLACE INTO item_loot_template VALUES ('" & itr.itemId & "','" & kvp.Key & "','" & kvp.Value.percentChance & "','0','" & kvp.Value.dropCount & "','" & kvp.Value.dropCount & "','0');")
                Next
            Next
        End Using
    End Sub

    Private Sub parseLine(ByVal str As String, ByRef itm As item)

        If itm IsNot Nothing Then

            str = Split(str, " // ")(0)  'Remove line comments
            str = str.Trim()  'Remove leading or trailing spaces

            If (str.StartsWith("loot=")) Then
                Dim spl() As String = Split(str.Substring(str.IndexOf("=") + 1))
                itm.addLoot(spl(0), spl(1))
            End If

        End If
    End Sub
End Class

Public Class item
    Public Structure lootItem
        Public percentChance As Decimal
        Public dropCount As Integer
        Sub New(ByVal pctChn As Decimal, ByVal drp As Integer)
            percentChance = pctChn
            dropCount = drp
        End Sub
    End Structure

    Private id As Integer
    Public Property itemId As Integer
        Get
            Return id
        End Get
        Set(value As Integer)
            id = value
        End Set
    End Property
    Private loot As Dictionary(Of Integer, lootItem)
    Public Sub addLoot(ByVal itmId As Integer, ByVal chance As Decimal)
        If (loot.ContainsKey(itmId) = True) Then
            loot.Item(itmId) = New lootItem(chance, loot.Item(itmId).dropCount + 1)
        Else
            Dim ltItm As New lootItem(chance, 1)
            loot.Add(itmId, ltItm)
        End If
    End Sub

    Public Function getLoot() As Dictionary(Of Integer, lootItem)
        Return loot
    End Function

    Private _count As Integer
    Public Property count As Integer
        Get
            Return _count
        End Get
        Set(value As Integer)
            _count = value
        End Set
    End Property

    Sub New()
        id = 0
        loot = New Dictionary(Of Integer, lootItem)
        _count = 1
    End Sub

    Public Shared items As New List(Of item)
    Public Shared Function Create(ByVal idNum As Integer) As item
        Dim i As New item
        i.id = idNum

        items.Add(i)

        Return i
    End Function
End Class
