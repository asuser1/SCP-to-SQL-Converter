Imports System.Text.RegularExpressions

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim line As String = ""
        Dim rgx As New Regex("(?<=\[loottemplate )\d+(?=\])", RegexOptions.IgnoreCase)
        Dim itm As item = Nothing
        Using sr As New IO.StreamReader("C:\loottemplates.scp")
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

        parseCreatures()

        Using sw As New IO.StreamWriter(My.Computer.FileSystem.SpecialDirectories.Desktop & "/loot.sql", True)
            For Each itr As item In item.items
                For Each kvp As KeyValuePair(Of Integer, lootItem) In itr.getLoot
                    sw.WriteLine("REPLACE INTO creature_loot_template VALUES ('" & itr.itemId & "','" & kvp.Key & "','" & kvp.Value.percentChance & "','0','" & kvp.Value.dropCount & "','" & kvp.Value.dropCount & "','0');")
                Next
            Next

            For Each creatur As creature In creature.items
                If (creatur.lootTemplate = 0) Then
                    Continue For
                End If

                sw.WriteLine("UPDATE creature_template SET LootId = '" & creatur.lootTemplate & "' WHERE Entry = '" & creatur.creatureId & "';")
            Next
        End Using
    End Sub

    Private Sub parseCreatures()
        Dim line As String = ""
        Dim rgx As New Regex("(?<=\[creature )\d+(?=\])", RegexOptions.IgnoreCase)
        Dim itm As creature = Nothing
        Using sr As New IO.StreamReader("C:\creatures.scp")
            line = sr.ReadLine()
            While line IsNot Nothing

                If line <> vbNullString Then
                    If (rgx.IsMatch(line) = True) Then
                        itm = creature.Create(rgx.Match(line).Value)
                    Else
                        If (itm IsNot Nothing) Then

                            parseLineC(line, itm)

                        End If
                    End If
                End If

                line = sr.ReadLine()

            End While
        End Using

        For Each creat As creature In creature.items
            If creat.lootTemplate = 0 Then
                Continue For
            End If

            If (creat.hasLoot = True And creat.lootTemplate <> creat.creatureId) Then
                Dim newTemplate As item = item.Create(creat.creatureId)
                Dim ctrTmpl As Dictionary(Of Integer, lootItem) = getLootTemplate(creat.lootTemplate)
                If (ctrTmpl.Count > 0) Then
                    newTemplate.addLootTemplate(getLootTemplate(creat.lootTemplate))
                    creat.lootTemplate = creat.creatureId
                Else
                    creat.lootTemplate = 0
                End If
            End If
        Next
    End Sub

    Private Function getLootTemplate(ByVal lootTempla As Integer) As Dictionary(Of Integer, lootItem)
        Dim rtn As New Dictionary(Of Integer, lootItem)
        For Each i As item In item.items
            If i.itemId = lootTempla Then
                rtn = i.getLoot
            End If
        Next
        Return rtn
    End Function

    Private Sub parseLine(ByVal str As String, ByRef itm As item)

        If itm IsNot Nothing Then

            str = Split(str, " // ")(0)  'Remove line comments
            str = str.Trim()  'Remove leading or trailing spaces
            str = Regex.Replace(str, "\s{2,}", " ") 'If line is incorrectly spaced, correct it

            If (str.StartsWith("loot=")) Then
                Dim spl() As String = Split(str.Substring(str.IndexOf("=") + 1))
                itm.addLoot(spl(0), spl(1))
            End If

        End If
    End Sub

    Private Sub parseLineC(ByVal str As String, ByRef itm As creature)

        If itm IsNot Nothing Then

            str = Split(str, " // ")(0)  'Remove line comments
            str = str.Trim()  'Remove leading or trailing spaces
            str = Regex.Replace(str, "\s{2,}", " ") 'If line is incorrectly spaced, correct it

            If (str.StartsWith("loot=")) Then
                If itm.hasLoot = False Then
                    itm.hasLoot = True
                End If
            End If

            If (str.StartsWith("loottemplate=")) Then
                itm.lootTemplate = str.Substring(str.IndexOf("=") + 1)
            End If

        End If
    End Sub
End Class

Public Class item

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

    Public Sub addLootTemplate(ByVal ltTmp As Dictionary(Of Integer, lootItem))
        For Each imp As KeyValuePair(Of Integer, lootItem) In ltTmp
            If (loot.Keys.Contains(imp.Key) = False) Then
                loot.Add(imp.Key, imp.Value)
            Else
                loot.Item(imp.Key) = New lootItem(loot.Values(imp.Key).percentChance, loot.Values(imp.Key).dropCount + imp.Value.dropCount)
            End If
        Next
    End Sub

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

Public Structure lootItem
    Public percentChance As Decimal
    Public dropCount As Integer
    Sub New(ByVal pctChn As Decimal, ByVal drp As Integer)
        percentChance = pctChn
        dropCount = drp
    End Sub
End Structure

Public Class creature

    Private id As Integer
    Public Property creatureId As Integer
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

    Public lootTemplate As Integer

    Public hasLoot As Integer

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
        hasLoot = False
        lootTemplate = 0
    End Sub

    Public Shared items As New List(Of creature)
    Public Shared Function Create(ByVal idNum As Integer) As creature
        Dim i As New creature
        i.id = idNum

        items.Add(i)

        Return i
    End Function
End Class