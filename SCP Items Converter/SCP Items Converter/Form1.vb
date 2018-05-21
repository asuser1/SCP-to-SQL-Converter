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

        For Each itr As item In item.lstItm
            Using sw As New IO.StreamWriter(My.Computer.FileSystem.SpecialDirectories.Desktop & "/items.sql", True)
                sw.WriteLine("REPLACE INTO item_template VALUES ('" & itr.entry & "','" & itr.itemClass & "','" & itr.itemSubClass & "',""" & itr.itemName & """,'" & itr.itemModelId & "','" & itr.itemQuality & "','" & itr.itemFlags & "','1','" & itr.itemBuyPrice & "','" & itr.itemSellPrice & "','" & itr.itemInvType & "','" & itr.itemClasses & "','" & itr.itemRaces & "','" & itr.itemLevel & "','" & itr.itemReqLevel & "','" & itr.itemReqSkill & "','" & itr.itemRankSkill & "','" & itr.itemReqSpell & "','" & itr.itemPvpRank & "','0','0','0','" & itr.itemMaxCount & "','" & itr.itemStackable & "','" & itr.itemContSlots & "','" & itr.getBonusStr & itr.getDmgMin(0) & "','" & itr.getDmgMax(0) & "','" & itr.getDmgType(0) & "','" & itr.getDmgMin(1) & "','" & itr.getDmgMax(1) & "','" & itr.getDmgType(1) & "','" & itr.getDmgMin(2) & "','" & itr.getDmgMax(2) & "','" & itr.getDmgType(2) & "','" & itr.getDmgMin(3) & "','" & itr.getDmgMax(3) & "','" & itr.getDmgType(3) & "','" & itr.getDmgMin(4) & "','" & itr.getDmgMax(4) & "','" & itr.getDmgType(4) & "','" & Math.Abs(itr.getRes(0)) & "','" & Math.Abs(itr.getRes(1)) & "','" & Math.Abs(itr.getRes(2)) & "','" & Math.Abs(itr.getRes(3)) & "','" & Math.Abs(itr.getRes(4)) & "','" & Math.Abs(itr.getRes(5)) & "','" & Math.Abs(itr.getRes(6)) & "','" & itr.itemDelay & "','" & itr.itemAmmo & "','0','" & itr.spl(0) & "','" & itr.splTrig(0) & "','" & itr.splChrg(0) & "','0','" & itr.splCd(0) & "','" & itr.splCat(0) & "','" & itr.splCatCd(0) & "','" & itr.spl(1) & "','" & itr.splTrig(1) & "','" & itr.splChrg(1) & "','0','" & itr.splCd(1) & "','" & itr.splCat(1) & "','" & itr.splCatCd(1) & "','" & itr.spl(2) & "','" & itr.splTrig(2) & "','" & itr.splChrg(2) & "','0','" & itr.splCd(2) & "','" & itr.splCat(2) & "','" & itr.splCatCd(2) & "','" & itr.spl(3) & "','" & itr.splTrig(3) & "','" & itr.splChrg(3) & "','0','" & itr.splCd(3) & "','" & itr.splCat(3) & "','" & itr.splCatCd(3) & "','" & itr.spl(4) & "','" & itr.splTrig(4) & "','" & itr.splChrg(4) & "','0','" & itr.splCd(4) & "','" & itr.splCat(4) & "','" & itr.splCatCd(4) & "','" & itr.itemBonding & "',""" & itr.itemDescription & """,'" & itr.itemPageText & "','" & itr.itemLang & "','" & itr.itemPageMaterial & "','" & itr.itemStartsQuest & "','" & itr.itemLockId & "','" & itr.itemMaterial & "','" & itr.itemSheath & "','" & 0 & "','" & itr.itemBlock & "','" & itr.itemSet & "','" & Math.Abs(itr.itemDurability) & "','" & 0 & "','" & 0 & "','" & 0 & "','" & 0 & "','" & 0 & "','" & 0 & "','" & 0 & "','" & 0 & "','" & Math.Abs(itr.itemExtraFlags) & "');")
            End Using
        Next
    End Sub

    Private Sub parseLine(ByVal str As String, ByRef itm As item)

        If itm IsNot Nothing Then

            str = Split(str, " // ")(0)  'Remove line comments
            str = str.Trim()  'Remove leading or trailing spaces

            Select Case True

                Case str.StartsWith("name=")
                    itm.itemName = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("description=")
                    itm.itemDescription = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("class=")
                    itm.itemClass = str.Substring(str.IndexOf("=") + 1)

                Case Regex.IsMatch(str, "(?<=subclass=)\d+")
                    itm.itemSubClass = Regex.Match(str, "(?<=subclass=)\d+").Value

                Case Regex.IsMatch(str, "(?<=inventorytype=)\d+")
                    itm.itemInvType = Regex.Match(str, "(?<=inventorytype=)\d+").Value

                Case Regex.IsMatch(str, "(?<=reqlevel=)\d+")
                    itm.itemReqLevel = Regex.Match(str, "(?<=reqlevel=)\d+").Value

                Case Regex.IsMatch(str, "(?<=quality=)\d+")
                    itm.itemQuality = Regex.Match(str, "(?<=quality=)\d+").Value

                Case Regex.IsMatch(str, "(?<=delay=)\d+")
                    itm.itemDelay = Regex.Match(str, "(?<=delay=)\d+").Value

                Case Regex.IsMatch(str, "(?<=durability=)\d+")
                    itm.itemDurability = Regex.Match(str, "(?<=durability=)\d+").Value

                Case Regex.IsMatch(str, "(?<=damage=)(\s|\d)+")
                    Dim spl() As String = Split(Regex.Match(str, "(?<=damage=)(\s|\d)+").Value, " ")
                    itm.setDmg(spl(0), spl(1), spl(2))

                Case Regex.IsMatch(str, "(?<=resistance1=)\d+")
                    itm.setResistance(0, Regex.Match(str, "(?<=resistance1=)\d+").Value)

                Case Regex.IsMatch(str, "(?<=resistance2=)\d+")
                    itm.setResistance(1, Regex.Match(str, "(?<=resistance2=)\d+").Value)

                Case Regex.IsMatch(str, "(?<=resistance3=)\d+")
                    itm.setResistance(2, Regex.Match(str, "(?<=resistance3=)\d+").Value)

                Case Regex.IsMatch(str, "(?<=resistance4=)\d+")
                    itm.setResistance(3, Regex.Match(str, "(?<=resistance4=)\d+").Value)

                Case Regex.IsMatch(str, "(?<=resistance5=)\d+")
                    itm.setResistance(4, Regex.Match(str, "(?<=resistance5=)\d+").Value)

                Case Regex.IsMatch(str, "(?<=resistance6=)\d+")
                    itm.setResistance(5, Regex.Match(str, "(?<=resistance6=)\d+").Value)

                Case Regex.IsMatch(str, "(?<=resistance7=)\d+")
                    itm.setResistance(6, Regex.Match(str, "(?<=resistance7=)\d+").Value)

                Case Regex.IsMatch(str, "(?<=bonus=0\s)(\d|-)+")
                    itm.setBonus(0, Regex.Match(str, "(?<=bonus=0\s)(\d|-)+").Value)

                Case Regex.IsMatch(str, "(?<=bonus=1\s)(\d|-)+")
                    itm.setBonus(1, Regex.Match(str, "(?<=bonus=1\s)(\d|-)+").Value)

                Case Regex.IsMatch(str, "(?<=bonus=2\s)(\d|-)+")
                    itm.setBonus(2, Regex.Match(str, "(?<=bonus=2\s)(\d|-)+").Value)

                Case Regex.IsMatch(str, "(?<=bonus=3\s)(\d|-)+")
                    itm.setBonus(3, Regex.Match(str, "(?<=bonus=3\s)(\d|-)+").Value)

                Case Regex.IsMatch(str, "(?<=bonus=4\s)(\d|-)+")
                    itm.setBonus(4, Regex.Match(str, "(?<=bonus=4\s)(\d|-)+").Value)

                Case Regex.IsMatch(str, "(?<=bonus=5\s)(\d|-)+")
                    itm.setBonus(5, Regex.Match(str, "(?<=bonus=5\s)(\d|-)+").Value)

                Case Regex.IsMatch(str, "(?<=bonus=6\s)(\d|-)+")
                    itm.setBonus(6, Regex.Match(str, "(?<=bonus=6\s)(\d|-)+").Value)

                Case Regex.IsMatch(str, "(?<=bonus=7\s)(\d|-)+")
                    itm.setBonus(7, Regex.Match(str, "(?<=bonus=7\s)(\d|-)+").Value)

                Case Regex.IsMatch(str, "(?<=buyprice=)\d+")
                    itm.itemBuyPrice = Regex.Match(str, "(?<=buyprice=)\d+").Value

                Case Regex.IsMatch(str, "(?<=sellprice=)\d+")
                    itm.itemSellPrice = Regex.Match(str, "(?<=sellprice=)\d+").Value

                Case Regex.IsMatch(str, "(?<=bonding=)\d+")
                    itm.itemBonding = Regex.Match(str, "(?<=bonding=)\d+").Value

                Case Regex.IsMatch(str, "(?<=classes=)(\d|\w)+")
                    itm.itemClasses = Val("&H" & Regex.Match(str, "(?<=classes=)(\d|\w)+").Value)

                Case Regex.IsMatch(str, "(?<=model=)\d+")
                    itm.itemModelId = Regex.Match(str, "(?<=model=)\d+").Value

                Case Regex.IsMatch(str, "(?<=flags=)\d+")
                    itm.itemFlags = Regex.Match(str, "(?<=flags=)\d+").Value

                Case str.StartsWith("level")
                    itm.itemLevel = str.Substring(str.IndexOf("=") + 1)

                Case Regex.IsMatch(str, "(?<=races=)(\d|\w)+")
                    itm.itemRaces = Val("&H" & Regex.Match(str, "(?<=races=)(\d|\w)+").Value)

                Case Regex.IsMatch(str, "(?<=stackable=)\d+")
                    itm.itemStackable = Regex.Match(str, "(?<=stackable=)\d+").Value

                Case Regex.IsMatch(str, "(?<=block=)\d+")
                    itm.itemBlock = Regex.Match(str, "(?<=block=)\d+").Value

                Case Regex.IsMatch(str, "(?<=maxcount=)\d+")
                    itm.itemMaxCount = Regex.Match(str, "(?<=maxcount=)\d+").Value

                Case str.StartsWith("material=")
                    itm.itemMaterial = str.Substring(str.IndexOf("=") + 1)

                Case Regex.IsMatch(str, "(?<=pagematerial=)-?\d+")
                    itm.itemPageMaterial = Regex.Match(str, "(?<=pagematerial=)-?\d+").Value

                Case Regex.IsMatch(str, "(?<=pagetext=)\d+")
                    itm.itemPageText = Regex.Match(str, "(?<=pagetext=)\d+").Value

                Case Regex.IsMatch(str, "(?<=sheath=)\d+")
                    itm.itemSheath = Regex.Match(str, "(?<=sheath=)\d+").Value

                Case Regex.IsMatch(str, "(?<=spell=)(\d|\s|-)+")
                    Dim spl() As String = Split(Regex.Match(str, "(?<=spell=)(\d|\s|-)+").Value, " ")
                    If (spl.Length = 1) Then
                        itm.setSpell(spl(0), 0, 0, -1, 0, -1)
                    Else
                        itm.setSpell(spl(0), spl(1), spl(2), spl(3), spl(4), spl(5))
                    End If

                Case Regex.IsMatch(str, "(?<=set=)\d+")
                    itm.itemSet = Regex.Match(str, "(?<=set=)\d+").Value

                Case Regex.IsMatch(str, "(?<=language=)\d+")
                    itm.itemLang = Regex.Match(str, "(?<=language=)\d+").Value

                Case Regex.IsMatch(str, "(?<=extra=)(\d|-)+")
                    itm.itemExtraFlags = Regex.Match(str, "(?<=extra=)(\d|-)+").Value

                Case Regex.IsMatch(str, "(?<=containerslots=)\d+")
                    itm.itemContSlots = Regex.Match(str, "(?<=containerslots=)\d+").Value

                Case Regex.IsMatch(str, "(?<=ammotype=)\d+")
                    itm.itemAmmo = Regex.Match(str, "(?<=ammotype=)\d+").Value

                Case Regex.IsMatch(str, "(?<=skill=)\d+")
                    itm.itemReqSkill = Regex.Match(str, "(?<=skill=)\d+").Value

                Case Regex.IsMatch(str, "(?<=skillrank=)\d+")
                    itm.itemRankSkill = Regex.Match(str, "(?<=skillrank=)\d+").Value

                Case Regex.IsMatch(str, "(?<=spellreq=)\d+")
                    itm.itemReqSpell = Regex.Match(str, "(?<=spellreq=)\d+").Value

                Case Regex.IsMatch(str, "(?<=startquest=)\d+")
                    itm.itemStartsQuest = Regex.Match(str, "(?<=startquest=)\d+").Value

                Case Regex.IsMatch(str, "(?<=pvprankreq=)\d+")
                    itm.itemPvpRank = Regex.Match(str, "(?<=pvprankreq=)\d+").Value

                Case Regex.IsMatch(str, "(?<=lockid=)\d+")
                    itm.itemLockId = Regex.Match(str, "(?<=lockid=)\d+").Value

                Case Regex.IsMatch(str, "(?<=loot=)(\d|\s|\.)+")
                    Dim spl() As String = Split(Regex.Match(str, "(?<=loot=)(\d|\s|\.)+").Value, " ")
                    itm.addLoot(spl(0), spl(1))
            End Select

        End If

    End Sub
End Class

Public Class item
    Private id As Integer
    Public Property entry As Integer
        Get
            Return id
        End Get
        Set(value As Integer)
            id = value
        End Set
    End Property
    Private name As String
    Private desc As String
    Public Property itemDescription As String
        Get
            Return desc
        End Get
        Set(value As String)
            desc = value
        End Set
    End Property
    Private itmClass As Integer
    Private subClass As Integer
    Public Property itemSubClass As Integer
        Get
            Return subClass
        End Get
        Set(value As Integer)
            subClass = value
        End Set
    End Property
    Private inventoryType As Integer
    Public Property itemInvType As Integer
        Get
            Return inventoryType
        End Get
        Set(value As Integer)
            inventoryType = value
        End Set
    End Property
    Private reqLevel As Integer
    Public Property itemReqLevel As Integer
        Get
            Return reqLevel
        End Get
        Set(value As Integer)
            reqLevel = value
        End Set
    End Property
    Private quality As Integer
    Public Property itemQuality As Integer
        Get
            Return quality
        End Get
        Set(value As Integer)
            quality = value
        End Set
    End Property
    Private delay As Integer
    Public Property itemDelay As Integer
        Get
            Return delay
        End Get
        Set(value As Integer)
            delay = value
        End Set
    End Property
    Private durability As Integer
    Public Property itemDurability As Integer
        Get
            Return durability
        End Get
        Set(value As Integer)
            durability = value
        End Set
    End Property
    Private dmgMin(6) As Int64
    Private dmgMax(6) As Int64
    Private dmgType(6) As Integer
    Public Sub setDmg(ByVal minDmg As Int64, ByVal maxDmg As Int64, ByVal type As Integer)
        Dim idx As Integer = Array.IndexOf(dmgMax, Convert.ToInt64(0))
        dmgMin(idx) = minDmg
        dmgMax(idx) = maxDmg
        dmgType(idx) = type
    End Sub

    Public Function getDmgMin(ByVal index As Integer) As Integer
        Return dmgMin(index)
    End Function

    Public Function getDmgMax(ByVal index As Integer) As Integer
        Return dmgMax(index)
    End Function

    Public Function getDmgType(ByVal index As Integer) As Integer
        Return dmgType(index)
    End Function
    Private resistance(6) As Integer
    Public Sub setResistance(ByVal index As Integer, ByVal val As Integer)
        resistance(index) = val
    End Sub

    Public Function getRes(ByVal index As Integer) As Integer
        Return resistance(index)
    End Function
    Private bonus As Dictionary(Of Integer, Integer)
    Public Sub setBonus(ByVal index As Integer, ByVal val As Integer)
        bonus.Add(index, val)
    End Sub

    Public Function getBonus(ByVal index As Integer)
        Return bonus(index)
    End Function
    Private buyPrice As String
    Public Property itemBuyPrice As String
        Get
            Return buyPrice
        End Get
        Set(value As String)
            buyPrice = value
        End Set
    End Property
    Private sellPrice As String
    Public Property itemSellPrice As String
        Get
            Return sellPrice
        End Get
        Set(value As String)
            sellPrice = value
        End Set
    End Property
    Private bonding As Integer
    Public Property itemBonding As Integer
        Get
            Return bonding
        End Get
        Set(value As Integer)
            bonding = value
        End Set
    End Property
    Private classes As Integer
    Public Property itemClasses As Integer
        Get
            Return classes
        End Get
        Set(value As Integer)
            If value <> 0 Then
                classes = value
            End If
        End Set
    End Property
    Private modelId As Integer
    Public Property itemModelId As Integer
        Get
            Return modelId
        End Get
        Set(value As Integer)
            modelId = value
        End Set
    End Property
    Private flags As Integer
    Public Property itemFlags As Integer
        Get
            Return flags
        End Get
        Set(value As Integer)
            flags = value
        End Set
    End Property
    Private level As Integer
    Public Property itemLevel As Integer
        Get
            Return level
        End Get
        Set(value As Integer)
            level = value
        End Set
    End Property
    Private races As Integer
    Public Property itemRaces As Integer
        Get
            Return races
        End Get
        Set(value As Integer)
            If value <> 0 Then
                races = value
            End If
        End Set
    End Property
    Private stackable As Integer
    Public Property itemStackable As Integer
        Get
            Return stackable
        End Get
        Set(value As Integer)
            stackable = value
        End Set
    End Property
    Private block As Integer
    Public Property itemBlock As Integer
        Get
            Return block
        End Get
        Set(value As Integer)
            block = value
        End Set
    End Property
    Private maxcount As Integer
    Public Property itemMaxCount As Integer
        Get
            Return maxcount
        End Get
        Set(value As Integer)
            maxcount = value
        End Set
    End Property
    Private material As Integer
    Public Property itemMaterial As Integer
        Get
            Return material
        End Get
        Set(value As Integer)
            material = value
        End Set
    End Property
    Private pageMaterial As Integer
    Public Property itemPageMaterial As Integer
        Get
            Return pageMaterial
        End Get
        Set(value As Integer)
            pageMaterial = value
        End Set
    End Property
    Private pageText As Integer
    Public Property itemPageText As Integer
        Get
            Return pageText
        End Get
        Set(value As Integer)
            pageText = value
        End Set
    End Property
    Private sheath As Integer
    Public Property itemSheath As Integer
        Get
            Return sheath
        End Get
        Set(value As Integer)
            sheath = value
        End Set
    End Property
    Private spell(4) As Integer
    Public Function spl(ByVal index As Integer) As Integer
        Return spell(index)
    End Function
    Private spellTrig(4) As Integer
    Public Function splTrig(ByVal index As Integer) As Integer
        Return spellTrig(index)
    End Function
    Private spellCharges(4) As Integer
    Public Function splChrg(ByVal index As Integer) As Integer
        Return spellCharges(index)
    End Function
    Private spellCooldown(4) As Integer
    Public Function splCd(ByVal index As Integer) As Integer
        Return spellCooldown(index)
    End Function
    Private spellCategory(4) As Integer
    Public Function splCat(ByVal index As Integer) As Integer
        Return spellCategory(index)
    End Function
    Private spellCatCooldown(4) As Integer
    Public Function splCatCd(ByVal index As Integer) As Integer
        Return spellCatCooldown(index)
    End Function
    Public Sub setSpell(ByVal val As Integer, ByVal trig As Integer, ByVal charge As Integer, ByVal cool As Integer, ByVal cat As Integer, ByVal catCool As Integer)
        Dim index As Integer = Array.IndexOf(spell, 0)
        spell(index) = val
        spellTrig(index) = trig
        spellCharges(index) = charge
        spellCooldown(index) = cool
        spellCategory(index) = cat
        spellCatCooldown(index) = catCool
    End Sub
    Private itmSet As Integer
    Public Property itemSet As Integer
        Get
            Return itmSet
        End Get
        Set(value As Integer)
            itmSet = value
        End Set
    End Property
    Private lang As Integer
    Public Property itemLang As Integer
        Get
            Return lang
        End Get
        Set(value As Integer)
            lang = value
        End Set
    End Property
    Private extraFlags As Integer
    Public Property itemExtraFlags() As Integer
        Get
            Return extraFlags
        End Get
        Set(ByVal value As Integer)
            extraFlags = value
        End Set
    End Property
    Private containerSlots As Integer
    Public Property itemContSlots As Integer
        Get
            Return containerSlots
        End Get
        Set(value As Integer)
            containerSlots = value
        End Set
    End Property
    Private ammoType As Integer
    Public Property itemAmmo As Integer
        Get
            Return ammoType
        End Get
        Set(value As Integer)
            ammoType = value
        End Set
    End Property
    Private startsQuest As Integer
    Public Property itemStartsQuest As Integer
        Get
            Return startsQuest
        End Get
        Set(value As Integer)
            startsQuest = value
        End Set
    End Property
    Private reqSkill As Integer
    Public Property itemReqSkill As Integer
        Get
            Return reqSkill
        End Get
        Set(value As Integer)
            reqSkill = value
        End Set
    End Property
    Private rankSkill As Integer
    Public Property itemRankSkill As Integer
        Get
            Return rankSkill
        End Get
        Set(value As Integer)
            rankSkill = value
        End Set
    End Property
    Private reqSpell As Integer
    Public Property itemReqSpell As Integer
        Get
            Return reqSpell
        End Get
        Set(value As Integer)
            reqSpell = value
        End Set
    End Property
    Private pvpRank As Integer
    Public Property itemPvpRank As Integer
        Get
            Return pvpRank
        End Get
        Set(value As Integer)
            pvpRank = value
        End Set
    End Property
    Private lockId As Integer
    Public Property itemLockId As Integer
        Get
            Return lockId
        End Get
        Set(value As Integer)
            lockId = value
        End Set
    End Property
    Private loot As List(Of KeyValuePair(Of Integer, Decimal))
    Public Sub addLoot(ByVal itm As Integer, ByVal chance As Decimal)
        loot.Add(New KeyValuePair(Of Integer, Decimal)(itm, chance))
    End Sub
    Public Shared lstItm As New List(Of item)

    Sub New()
        id = 0
        name = ""
        desc = ""
        itmClass = 0
        subClass = 0
        inventoryType = 0
        reqLevel = 0
        quality = 0
        delay = 0
        durability = 0
        dmgMin = {0, 0, 0, 0, 0, 0, 0}
        dmgMax = {0, 0, 0, 0, 0, 0, 0}
        dmgType = {0, 0, 0, 0, 0, 0, 0}
        resistance = {0, 0, 0, 0, 0, 0, 0}
        bonus = New Dictionary(Of Integer, Integer)
        buyPrice = 0
        sellPrice = 0
        bonding = 0
        classes = -1
        modelId = 0
        level = 0
        flags = 0
        block = 0
        races = -1
        stackable = 0
        maxcount = 0
        material = 0
        pageMaterial = 0
        pageText = 0
        sheath = 0
        spell = {0, 0, 0, 0, 0}
        spellTrig = {0, 0, 0, 0, 0}
        spellCharges = {0, 0, 0, 0, 0}
        spellCooldown = {-1, -1, -1, -1, -1}
        spellCategory = {0, 0, 0, 0, 0}
        spellCatCooldown = {-1, -1, -1, -1, -1}
        itmSet = 0
        lang = 0
        extraFlags = 0
        containerSlots = 0
        ammoType = 0
        startsQuest = 0
        reqSkill = 0
        rankSkill = 0
        reqSpell = 0
        pvpRank = 0
        lockId = 0
        loot = New List(Of KeyValuePair(Of Integer, Decimal))
    End Sub

    Public Property itemName
        Get
            Return name
        End Get
        Set(value)
            name = value
        End Set
    End Property

    Public Property itemClass
        Get
            Return itmClass
        End Get
        Set(value)
            itmClass = value
        End Set
    End Property

    Public Function getBonusStr() As String
        Dim rtnStr As String = ""
        Dim i As Integer = 0
        Dim bonusArr(20) As Integer
        bonusArr = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}

        If bonus.Count > 0 Then
            For Each int As Integer In bonus.Keys
                bonusArr(i) = int
                bonusArr(i + 1) = bonus.Item(int)
                i += 2
            Next
        End If

        For Each num As Integer In bonusArr
            rtnStr &= num & "','"
        Next

        Return rtnStr
    End Function

    Public Shared Function Create(ByVal id As Integer) As item
        Dim itm As New item
        itm.id = id

        lstItm.Add(itm)

        Return itm
    End Function
End Class