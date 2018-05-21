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
                sw.WriteLine("REPLACE INTO creature_template VALUES ('" & cre.ctrId & "',""" & cre.ctrName.Replace("""", """""") & """,""" & cre.ctrGuild.Replace("""", """""") & """,'" & cre.minLvl & "','" & cre.maxLvl & "','" & cre.modelId(0) & "','" & cre.modelId(1) & "','" & cre.modelId(2) & "','" & cre.modelId(3) & "','" & cre.faction & "','" & cre.faction & "','" & cre.scale & "','" & cre.family & "','" & cre.type & "','" & 0 & "','1','0','" & cre.npcFlags & "','" & Math.Abs(cre.flags) & "','0','0','0','1','" & cre.speed & "','" & 0 & "','" & cre.rank & "','1','1','1','0','1','1','" & cre.health & "','" & cre.maxHealth & "','" & cre.mana & "','" & cre.maxMana & "','" & cre.minDmg & "','" & cre.maxDmg & "','" & cre.minDmg & "','" & cre.maxDmg & "','" & 0 & "','" & cre.attackPhy & "','" & cre.attackRan & "','" & 0 & "','" & 0 & "','" & 0 & "','" & Math.Abs(cre.moneyMin) & "','" & Math.Abs(cre.moneyMax) & "','" & cre.lootId & "','" & 0 & "','" & 0 & "','" & 0 & "','" & 0 & "','" & 0 & "','" & cre.getResist(0) & "','" & cre.getResist(1) & "','" & cre.getResist(2) & "','" & cre.getResist(3) & "','" & cre.getResist(4) & "','" & cre.getResist(5) & "','" & 0 & "','" & 0 & "','" & cre.trainerType & "','" & 0 & "','" & cre.trainerClass & "','" & 0 & "','" & 0 & "','" & 0 & "','" & 0 & "','" & 0 & "','" & cre.civilian & "','" & "');")
            End Using
        Next
    End Sub

    Private Sub parseLine(ByVal str As String, ByRef ctr As creature)

        If ctr IsNot Nothing Then

            str = Split(str, "//")(0)  'Remove line comments
            str = str.Trim()  'Remove leading or trailing spaces

            Select Case True
                Case str.StartsWith("name=")
                    ctr.ctrName = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("guild=")
                    ctr.ctrGuild = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("level=")
                    Dim spl() As String = Split(str.Substring(str.IndexOf("=") + 1), IIf(str.Contains(" "), " ", ".."))
                    If (spl.Length > 1) Then
                        ctr.minLvl = spl(0)
                        ctr.maxLvl = spl(1)
                    Else
                        ctr.minLvl = spl(0)
                        ctr.maxLvl = spl(0)
                    End If

                Case str.StartsWith("model=")
                    Dim spl() As String = Split(str.Substring(str.IndexOf("=") + 1), ",")
                    Select Case True
                        Case spl.Length = 1
                            ctr.modelId(0) = spl(0)

                        Case spl.Length = 2
                            ctr.modelId(0) = spl(0)
                            ctr.modelId(1) = spl(1)

                        Case spl.Length = 3
                            ctr.modelId(0) = spl(0)
                            ctr.modelId(1) = spl(1)
                            ctr.modelId(2) = spl(2)

                        Case spl.Length = 4
                            ctr.modelId(0) = spl(0)
                            ctr.modelId(1) = spl(1)
                            ctr.modelId(2) = spl(2)
                            ctr.modelId(3) = spl(3)
                    End Select

                Case str.StartsWith("faction=")
                    ctr.faction = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("size=")
                    ctr.scale = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("family=")
                    ctr.family = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("type=")
                    ctr.type = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("npcflags=")
                    ctr.npcFlags = Val("&H" & str.Substring(str.IndexOf("=") + 1))

                Case str.StartsWith("flags1=")
                    ctr.flags = Val("&H" & str.Substring(str.IndexOf("=") + 1))

                Case str.StartsWith("speed=")
                    ctr.speed = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("elite=")
                    ctr.rank = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("maxhealth=")
                    Dim spl() As String = Split(str.Substring(str.IndexOf("=") + 1), "..")
                    If (spl.Length > 1) Then
                        ctr.health = spl(0)
                        ctr.maxHealth = spl(1)
                    Else
                        ctr.health = str.Substring(str.IndexOf("=") + 1)
                        ctr.maxHealth = str.Substring(str.IndexOf("=") + 1)
                    End If


                Case str.StartsWith("maxmana=")
                    Dim spl() As String = Split(str.Substring(str.IndexOf("=") + 1), "..")
                    If (spl.Length > 1) Then
                        ctr.mana = spl(0)
                        ctr.maxMana = spl(1)
                    Else
                        ctr.mana = spl(0)
                        ctr.maxMana = spl(0)
                    End If

                Case str.StartsWith("damage=")
                    Dim spl() As String = Split(str.Substring(str.IndexOf("=") + 1))
                    If (spl.Length > 1) Then
                        ctr.minDmg = spl(0)
                        ctr.maxDmg = spl(1)
                    Else
                        ctr.minDmg = spl(0)
                        ctr.maxDmg = spl(0)
                    End If

                Case str.StartsWith("attack=")
                    Dim spl() As String = Split(str.Substring(str.IndexOf("=") + 1))
                    If (spl.Length > 1) Then
                        ctr.attackRan = spl(0)
                        ctr.attackPhy = spl(1)
                    Else
                        ctr.attackRan = spl(0)
                        ctr.attackPhy = spl(0)
                    End If

                Case str.StartsWith("money=")
                    Dim spl() As String = Split(str.Substring(str.IndexOf("=") + 1), IIf(str.Contains(" "), " ", ".."))
                    If (spl.Length > 1) Then
                        ctr.moneyMin = spl(0)
                        ctr.moneyMax = spl(1)
                    Else
                        ctr.moneyMin = spl(0)
                        ctr.moneyMax = spl(0)
                    End If

                Case str.StartsWith("loottemplate=")
                    ctr.lootId = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("resist=#SPELL_SCHOOL_HOLY ")
                    ctr.setResist(0, Split(str.Substring(str.IndexOf("=") + 1))(1))

                Case str.StartsWith("resist=#SPELL_SCHOOL_FIRE ")
                    ctr.setResist(1, Split(str.Substring(str.IndexOf("=") + 1))(1))

                Case str.StartsWith("resist=#SPELL_SCHOOL_NATURE ")
                    ctr.setResist(2, Split(str.Substring(str.IndexOf("=") + 1))(1))

                Case str.StartsWith("resist=#SPELL_SCHOOL_FROST ")
                    ctr.setResist(3, Split(str.Substring(str.IndexOf("=") + 1))(1))

                Case str.StartsWith("resist=#SPELL_SCHOOL_SHADOW ")
                    ctr.setResist(4, Split(str.Substring(str.IndexOf("=") + 1))(1))

                Case str.StartsWith("resist=#SPELL_SCHOOL_ARCANE ")
                    ctr.setResist(5, Split(str.Substring(str.IndexOf("=") + 1))(1))

                Case str.StartsWith("trainer_type=")
                    ctr.trainerType = str.Substring(str.IndexOf("=") + 1)

                Case str.StartsWith("npcjobs=")
                    Dim spl() As String = Split(str.Substring(str.IndexOf("=") + 1))
                    For Each st As String In spl
                        If (st.Contains("trainer") = True) Then
                            If (st.Equals("trainer") = False) Then
                                ctr.trainerClass = st.Replace("trainer", "")
                            End If
                        End If
                    Next

                Case str.StartsWith("civilian=")
                    ctr.civilian = str.Substring(str.IndexOf("=") + 1)
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
    Private _name As String
    Public Property ctrName As String
        Get
            Return _name
        End Get
        Set(value As String)
            _name = value
        End Set
    End Property
    Private _subName As String
    Public Property ctrGuild() As String
        Get
            Return _subName
        End Get
        Set(ByVal value As String)
            _subName = value
        End Set
    End Property
    Private _minLevel As Integer
    Public Property minLvl As Integer
        Get
            Return _minLevel
        End Get
        Set(ByVal value As Integer)
            _minLevel = value
        End Set
    End Property
    Private _maxLevel As Integer
    Public Property maxLvl As Integer
        Get
            Return _maxLevel
        End Get
        Set(ByVal value As Integer)
            _maxLevel = value
        End Set
    End Property
    Private _modelId(3) As Integer
    Public Property modelId() As Integer()
        Get
            Return _modelId
        End Get
        Set(ByVal value As Integer())
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
    Private _family As Integer
    Public Property family() As Integer
        Get
            Return _family
        End Get
        Set(ByVal value As Integer)
            _family = value
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
    Private _npcFlags As Integer
    Public Property npcFlags() As Integer
        Get
            Return _npcFlags
        End Get
        Set(ByVal value As Integer)
            _npcFlags = value
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
    Private _speed As Decimal
    Public Property speed() As Decimal
        Get
            Return _speed
        End Get
        Set(ByVal value As Decimal)
            _speed = value
        End Set
    End Property
    Private _rank As Integer
    Public Property rank() As Integer
        Get
            Return _rank
        End Get
        Set(ByVal value As Integer)
            _rank = value
        End Set
    End Property
    Private _health As Int64
    Public Property health() As Int64
        Get
            Return _health
        End Get
        Set(ByVal value As Int64)
            _health = value
        End Set
    End Property
    Private _maxHealth As Int64
    Public Property maxHealth As Int64
        Get
            Return _maxHealth
        End Get
        Set(value As Int64)
            _maxHealth = value
        End Set
    End Property
    Private _mana As Integer
    Public Property mana() As Integer
        Get
            Return _mana
        End Get
        Set(ByVal value As Integer)
            _mana = value
        End Set
    End Property
    Private _maxMana As Integer
    Public Property maxMana() As Integer
        Get
            Return _maxMana
        End Get
        Set(ByVal value As Integer)
            _maxMana = value
        End Set
    End Property
    Private _minDmg As Decimal
    Public Property minDmg() As Decimal
        Get
            Return _minDmg
        End Get
        Set(ByVal value As Decimal)
            _minDmg = value
        End Set
    End Property
    Private _maxDmg As Decimal
    Public Property maxDmg() As Decimal
        Get
            Return _maxDmg
        End Get
        Set(ByVal value As Decimal)
            _maxDmg = value
        End Set
    End Property
    Private _attackPhy As Integer
    Public Property attackPhy() As Integer
        Get
            Return _attackPhy
        End Get
        Set(ByVal value As Integer)
            _attackPhy = value
        End Set
    End Property
    Private _attackRan As Integer
    Public Property attackRan() As Integer
        Get
            Return _attackRan
        End Get
        Set(ByVal value As Integer)
            _attackRan = value
        End Set
    End Property
    Private _moneyMin As Integer
    Public Property moneyMin() As Integer
        Get
            Return _moneyMin
        End Get
        Set(ByVal value As Integer)
            _moneyMin = value
        End Set
    End Property
    Private _moneyMax As Integer
    Public Property moneyMax() As Integer
        Get
            Return _moneyMax
        End Get
        Set(ByVal value As Integer)
            _moneyMax = value
        End Set
    End Property
    Private _lootId As Integer
    Public Property lootId() As Integer
        Get
            Return _lootId
        End Get
        Set(ByVal value As Integer)
            _lootId = value
        End Set
    End Property
    Private _resist(5) As Integer
    Public Function getResist(ByVal index As Integer) As Integer
        Return _resist(index)
    End Function

    Public Sub setResist(ByVal index As Integer, ByVal value As Integer)
        _resist(index) = value
    End Sub
    Private _trainerType As Integer
    Public Property trainerType() As Integer
        Get
            Return _trainerType
        End Get
        Set(ByVal value As Integer)
            _trainerType = value
        End Set
    End Property
    Private _trainerClass As Integer
    Public Property trainerClass() As Integer
        Get
            Return _trainerClass
        End Get
        Set(ByVal value As Integer)
            _trainerClass = value
        End Set
    End Property
    Private _civilian As Integer
    Public Property civilian() As Integer
        Get
            Return _civilian
        End Get
        Set(ByVal value As Integer)
            _civilian = value
        End Set
    End Property


    Sub New()
        _id = 0
        _name = ""
        _subName = ""
        _minLevel = 0
        _maxLevel = 0
        _modelId = {0, 0, 0, 0}
        _faction = 0
        _scale = 1.0
        _family = 0
        _type = 0
        _npcFlags = 0
        _flags = 0
        _speed = 1.14286
        _rank = 0
        _health = 0
        _maxHealth = 0
        _mana = 0
        _maxMana = 0
        _minDmg = 0
        _maxDmg = 0
        _attackPhy = 0
        _attackRan = 0
        _moneyMin = 0
        _moneyMax = 0
        _lootId = 0
        _resist = {0, 0, 0, 0, 0, 0}
        _trainerType = 0
        _trainerClass = 0
        _civilian = 0
    End Sub

    Public Shared ctrLst As New List(Of creature)
    Public Shared Function Create(ByVal id As Integer) As creature
        Dim ctr As New creature
        ctr._id = id

        ctrLst.Add(ctr)

        Return ctr
    End Function
End Class