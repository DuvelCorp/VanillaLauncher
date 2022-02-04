Imports System.Data.SQLite
Imports System.IO



Public Class TrayForm

    '  Public Sub New()

    '     InitializeComponent()
    '     TrayMenu.Renderer = New JDtoolStripmenuRenderer

    ' End Sub

    Private Sub TrayForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try
            TrayMenu.Show(Cursor.Position) 'Shows the Right click menu on the cursor position
            Me.Left = TrayMenu.Left + 1 'Puts the form behind the menu horizontally
            Me.Top = TrayMenu.Top + 1 'Puts the form behind the menu vertically

            Build_Menu()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try

    End Sub

    Private Sub TrayForm_Deactivate(sender As Object, e As EventArgs) Handles MyBase.Deactivate
        Try
            Me.Close() 'Closes the "trayform" Form and consequently every thing in it, including the "traymenu"
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try

    End Sub

    Private Sub Add_Windowed_Sub_Menu(ByRef ParentItem As ToolStripMenuItem, ID As Integer, Optional IsChar As Boolean = False)


        Dim FullScreenItem As ToolStripMenuItem
        Dim Windowed1Item As ToolStripMenuItem
        Dim Windowed2Item As ToolStripMenuItem

        Try

            FullScreenItem = New ToolStripMenuItem
            FullScreenItem.Text = "FullScreen"
            FullScreenItem.Tag = ID & ";Full"
            FullScreenItem.Image = My.Resources.Square_FullScreen

            If IsChar Then
                AddHandler FullScreenItem.Click, AddressOf CharMenu_Handler
            Else
                AddHandler FullScreenItem.Click, AddressOf AccountMenu_Handler
            End If

            ParentItem.DropDownItems.Add(FullScreenItem)

            Windowed1Item = New ToolStripMenuItem
            If NBR_MONITORS > 1 Then
                Windowed1Item.Text = "Screen 1"
            Else
                Windowed1Item.Text = "Windowed"
            End If
            Windowed1Item.Tag = ID & ";Screen1"
            Windowed1Item.Image = My.Resources.Square_Windowed
            If IsChar Then
                AddHandler Windowed1Item.Click, AddressOf CharMenu_Handler
            Else
                AddHandler Windowed1Item.Click, AddressOf AccountMenu_Handler
            End If
            ParentItem.DropDownItems.Add(Windowed1Item)

            If TRAY_SHOW_SUBSUB_WINDOWED Then
                Add_Windowed_Sub_Sub_Menu(Windowed1Item, ID, IsChar)
            End If

            If NBR_MONITORS > 1 Then
                Windowed2Item = New ToolStripMenuItem
                Windowed2Item.Text = "Screen 2"
                Windowed2Item.Tag = ID & ";Screen2"
                Windowed2Item.Image = My.Resources.Square_Windowed

                If IsChar Then
                    AddHandler Windowed2Item.Click, AddressOf CharMenu_Handler
                Else
                    AddHandler Windowed2Item.Click, AddressOf AccountMenu_Handler
                End If

                ParentItem.DropDownItems.Add(Windowed2Item)

                If TRAY_SHOW_SUBSUB_WINDOWED Then
                    Add_Windowed_Sub_Sub_Menu(Windowed2Item, ID, IsChar)
                End If

            End If


        Catch ex As Exception

        End Try


    End Sub


    Private Sub Add_Windowed_Sub_Sub_Menu(ByRef ParentItem As ToolStripMenuItem, ID As Integer, Optional IsChar As Boolean = False)


        Dim Item1 As ToolStripMenuItem
        Dim Item2 As ToolStripMenuItem
        Dim Item3 As ToolStripMenuItem
        Dim Item4 As ToolStripMenuItem


        Try

            Item1 = New ToolStripMenuItem
            Item1.Text = "Top Left"
            Item1.Tag = ParentItem.Tag & ";TopLeft"
            Item1.Image = My.Resources.Square_Arrow_Top_Left_128
            If IsChar Then
                AddHandler Item1.Click, AddressOf CharMenu_Handler
            Else
                AddHandler Item1.Click, AddressOf AccountMenu_Handler
            End If
            ParentItem.DropDownItems.Add(Item1)


            Item2 = New ToolStripMenuItem
            Item2.Text = "Top Right"
            Item2.Tag = ParentItem.Tag & ";TopRight"
            Item2.Image = My.Resources.Square_Arrow_Top_Right_128
            If IsChar Then
                AddHandler Item2.Click, AddressOf CharMenu_Handler
            Else
                AddHandler Item2.Click, AddressOf AccountMenu_Handler
            End If
            ParentItem.DropDownItems.Add(Item2)


            Item3 = New ToolStripMenuItem
            Item3.Text = "Bottom Left"
            Item3.Tag = ParentItem.Tag & ";BottomLeft"
            Item3.Image = My.Resources.Square_Arrow_Bottom_Left_128
            If IsChar Then
                AddHandler Item3.Click, AddressOf CharMenu_Handler
            Else
                AddHandler Item3.Click, AddressOf AccountMenu_Handler
            End If
            ParentItem.DropDownItems.Add(Item3)


            Item4 = New ToolStripMenuItem
            Item4.Text = "Bottom Right"
            Item4.Tag = ParentItem.Tag & ";BottomRight"
            Item4.Image = My.Resources.Square_Arrow_Bottom_Right_128
            If IsChar Then
                AddHandler Item4.Click, AddressOf CharMenu_Handler
            Else
                AddHandler Item4.Click, AddressOf AccountMenu_Handler
            End If
            ParentItem.DropDownItems.Add(Item4)


        Catch ex As Exception

        End Try


    End Sub


    Private Sub Build_Menu()

        Dim objConn = New SQLiteConnection(CONSTRING)

        Dim objCmdRealm As SQLiteCommand
        Dim objCmdAccount As SQLiteCommand
        Dim objCmdChar As SQLiteCommand
        Dim objCmdLink As SQLiteCommand
        Dim objCmdMacro As SQLiteCommand

        Dim objRealm As SQLiteDataReader
        Dim objAccount As SQLiteDataReader
        Dim objChar As SQLiteDataReader
        Dim objLink As SQLiteDataReader
        Dim objMacro As SQLiteDataReader

        Dim MacroRootItem As ToolStripMenuItem
        Dim RootItem As ToolStripMenuItem

        Dim LinkRootItem As ToolStripMenuItem
        Dim GuildRootItem As ToolStripMenuItem
        Dim ProcItem As ToolStripMenuItem
        Dim LinkItem As ToolStripMenuItem
        Dim GuildItem As ToolStripMenuItem
        Dim MacroItem As ToolStripMenuItem
        Dim FavItem As ToolStripMenuItem

        Dim RealmItem As ToolStripMenuItem
        Dim AccountItem As ToolStripMenuItem
        Dim CharItem As ToolStripMenuItem


        Dim p As Integer
        Dim f As Integer
        Dim fSep As Integer
        Dim r As Integer
        Dim i As Integer
        Dim j As Integer
        Dim g As Integer
        Dim Sep As ToolStripSeparator
        Dim sql As String


        Dim RealmName As String
        Dim RealmID As Integer
        Dim WoWversion As Integer


        Dim AccountID As Integer
        Dim AccountName As String
        Dim AccountPassword As String
        Dim AccountIsFav As Boolean

        Dim CharID As Integer
        Dim CharIndex As Integer
        Dim CharName As String
        Dim CharClass As String
        Dim CharRace As String
        Dim CharGender As String
        Dim CharFaction As String
        Dim CharColor As String
        Dim CharIsFav As Boolean

        Dim Proc As System.Diagnostics.Process
        Dim WS As WoW_Session

        Dim ProcName As String

        Try

            ' MenuWoW.DropDownItems.Clear()

            TrayMenu.Items.Clear()

            ' --------------------------------------------------------------------------------------
            ' MAIN LOOP : REALMS -> ACCOUNTS -> CHARS
            ' --------------------------------------------------------------------------------------


            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)

                objConn.Open()


                sql = " SELECT DISTINCT " &
                  "        S.ServerName, S.ServerRealmList,   " &
                  "        R.RealmName, R.RealmID,  " &
                  "        v.wowversionID, v.wowversionName     " &
                  " FROM account a INNER JOIN realm r ON A.RealmID=r.RealmID " &
                  " INNER JOIN server s on S.ServerID=r.ServerID " &
                  " INNER JOIN wowversion v on S.wowversionID=v.wowversionID " &
                  " ORDER BY v.wowversionID, s.ServerID, r.RealmName"

                objCmdRealm = objConn.CreateCommand()
                objCmdRealm.CommandText = sql
                objRealm = objCmdRealm.ExecuteReader()

                r = 0
                f = 0


                While (objRealm.Read())

                    RealmName = objRealm("RealmName")
                    RealmID = CInt(objRealm("RealmID"))
                    WoWversion = CInt(objRealm("wowversionID"))

                    RealmItem = New ToolStripMenuItem
                    RealmItem.Text = RealmName
                    RealmItem.Tag = RealmID
                    If WoWversion = 2 Then
                        RealmItem.Image = My.Resources.V2
                    Else
                        RealmItem.Image = My.Resources.wow_icon
                    End If

                    TrayMenu.Items.Insert(r, RealmItem)

                    r = r + 1


                    sql = " SELECT a.AccountID, a.AccountName, a.AccountPassword, a.AccountIsFav, a.AccountNote " &
                  " FROM account a " &
                  " WHERE RealmID=" & RealmID &
                  " ORDER BY a.AccountID"

                    objCmdAccount = objConn.CreateCommand()
                    objCmdAccount.CommandText = sql
                    objAccount = objCmdAccount.ExecuteReader()

                    i = 0

                    While (objAccount.Read())

                        AccountID = CInt(objAccount("AccountID"))
                        AccountName = objAccount("AccountName")
                        AccountPassword = objAccount("AccountPassword")

                        If IsDBNull(objAccount("AccountIsFav")) Then
                            AccountIsFav = False
                        Else
                            AccountIsFav = CBool(objAccount("AccountIsFav"))
                        End If

                        AccountItem = New ToolStripMenuItem
                        AccountItem.Name = "Account" & AccountID
                        AccountItem.Text = AccountName
                        AccountItem.Tag = AccountID
                        AccountItem.ToolTipText = objAccount("AccountNote")
                        AccountItem.Image = My.Resources.Key
                        '    AccountItem.BackColor = Color.SlateGray
                        AddHandler AccountItem.Click, AddressOf AccountMenu_Handler

                        RealmItem.DropDownItems.Insert(i, AccountItem)
                        i = i + 1

                        If TRAY_SHOW_SUB_WINDOWED Then
                            Add_Windowed_Sub_Menu(AccountItem, AccountID)
                        End If



                        If AccountIsFav Then

                            FavItem = New ToolStripMenuItem
                            FavItem.Text = AccountName
                            FavItem.Name = "AccountFav" & AccountID
                            FavItem.Tag = AccountID
                            FavItem.ToolTipText = objAccount("AccountNote")
                            FavItem.Image = My.Resources.Key
                            '    FavItem.BackColor = Color.SlateGray
                            AddHandler FavItem.Click, AddressOf AccountMenu_Handler
                            TrayMenu.Items.Insert(f, FavItem)

                            ' comment f++ to have the accounts at end of the fav list
                            'f = f + 1 
                            fSep = fSep + 1
                            r = r + 1


                            If TRAY_SHOW_SUB_WINDOWED Then
                                Add_Windowed_Sub_Menu(FavItem, AccountID)
                            End If


                        End If


                            sql = " SELECT a.AccountID, a.AccountName, a.AccountNote, " &
                              "        ra.RaceName, ra.RaceFaction, cl.ClassName, cl.ClassColor,  " &
                              "        c.CharID, c.CharName, C.CharIndex, C.CharGender, C.CharFaction , C.CharIsFav , C.CharNote " &
                              " FROM character c " &
                              " LEFT JOIN  account a ON c.AccountID=a.AccountID " &
                              " LEFT JOIN class cl ON c.CLassID=cl.ClassID " &
                              " LEFT JOIN race ra ON c.RaceID=ra.RaceID " &
                              " WHERE a.AccountID = " & AccountID &
                              " ORDER BY C.CharIndex"

                        objCmdChar = objConn.CreateCommand()
                        objCmdChar.CommandText = sql
                        objChar = objCmdChar.ExecuteReader()

                        j = 0

                        While (objChar.Read())

                            CharID = CInt(objChar("CharID"))
                            CharIndex = CInt(objChar("CharIndex"))
                            CharName = objChar("CharName")
                            CharIsFav = CBool(objChar("CharIsFav"))

                            If IsDBNull(objChar("ClassName")) Then
                                CharClass = ""
                            Else
                                CharClass = objChar("ClassName")
                            End If

                            If IsDBNull(objChar("RaceName")) Then
                                CharRace = ""
                            Else
                                CharRace = objChar("RaceName")
                            End If

                            CharGender = objChar("CharGender")
                            CharFaction = objChar("CharFaction")

                            If IsDBNull(objChar("ClassColor")) Then
                                CharColor = "AAAAAA"
                            Else
                                CharColor = objChar("ClassColor")
                            End If

                            CharItem = New ToolStripMenuItem
                            CharItem.Text = CharName
                            CharItem.Name = "Char" & CharID
                            CharItem.Tag = CharID
                            CharItem.ToolTipText = objChar("CharNote")

                            CharItem.ForeColor = ColorTranslator.FromHtml("#" & CharColor)
                            CharItem.BackColor = Color.Black


                            If CHAR_IMAGE = "Class" Then
                                CharItem.Image = Image_Class(CharClass)
                            Else
                                CharItem.Image = Image_Race(CharRace, CharGender)
                            End If

                            AddHandler CharItem.Click, AddressOf CharMenu_Handler
                            AccountItem.DropDownItems.Insert(j, CharItem)

                            j = j + 1

                            If TRAY_SHOW_SUB_WINDOWED Then

                                Add_Windowed_Sub_Menu(CharItem, CharID, True)

                            End If


                            If CharIsFav Then


                                FavItem = New ToolStripMenuItem
                                FavItem.Text = CharName
                                FavItem.Name = "CharFav" & CharID
                                FavItem.Tag = CharID
                                FavItem.ToolTipText = objChar("CharNote")
                                FavItem.ForeColor = ColorTranslator.FromHtml("#" & CharColor)
                                FavItem.BackColor = Color.Black

                                If CHAR_IMAGE = "Class" Then
                                    FavItem.Image = Image_Class(CharClass)
                                Else
                                    FavItem.Image = Image_Race(CharRace, CharGender)
                                End If

                                AddHandler FavItem.Click, AddressOf CharMenu_Handler
                                TrayMenu.Items.Insert(f, FavItem)


                                f = f + 1
                                fSep = fSep + 1
                                r = r + 1


                                If TRAY_SHOW_SUB_WINDOWED Then
                                    Add_Windowed_Sub_Menu(FavItem, CharID, True)
                                End If


                            End If


                        End While

                    End While

                End While


                ' --------------------------------------------------------------------------------------
                ' MACROS
                ' --------------------------------------------------------------------------------------

                MacroRootItem = New ToolStripMenuItem
                MacroRootItem.Text = "Macros"
                MacroRootItem.Image = My.Resources.play
                TrayMenu.Items.Insert(r, MacroRootItem)
                r = r + 1

                sql = " SELECT * FROM macro ;"
                objCmdMacro = objConn.CreateCommand()
                objCmdMacro.CommandText = sql
                objMacro = objCmdMacro.ExecuteReader()
                i = 0
                While (objMacro.Read())

                    MacroItem = New ToolStripMenuItem
                    MacroItem.Text = objMacro("MacroName")
                    MacroItem.Tag = objMacro("MacroID")
                    MacroItem.Image = My.Resources.play
                    MacroItem.ToolTipText = objMacro("MacroNote")
                    AddHandler MacroItem.Click, AddressOf MacroMenu_Handler
                    MacroRootItem.DropDownItems.Insert(i, MacroItem)
                    i = i + 1


                    If CBool(objMacro("MacroIsFav")) Then


                        FavItem = New ToolStripMenuItem
                        FavItem.Text = objMacro("MacroName")
                        FavItem.Tag = objMacro("MacroID")
                        FavItem.Image = My.Resources.play
                        FavItem.ToolTipText = objMacro("MacroNote")
                        AddHandler FavItem.Click, AddressOf MacroMenu_Handler
                        TrayMenu.Items.Insert(f, FavItem)
                        f = f + 1
                        fSep = fSep + 1
                        r = r + 1
                    End If

                End While

                If i = 0 Then
                    MacroRootItem.Dispose()
                    r = r - 1
                End If

                ' --------------------------------------------------------------------------------------
                '  At this point there's nothing more to Favorite
                ' --------------------------------------------------------------------------------------

                If f > 0 Then
                    Sep = New ToolStripSeparator
                    TrayMenu.Items.Insert(fSep, Sep)
                    'r = r + 1
                End If

                Sep = New ToolStripSeparator
                TrayMenu.Items.Insert(r, Sep)
                r = r + 1

                ' --------------------------------------------------------------------------------------
                ' LINKS
                ' --------------------------------------------------------------------------------------

                If TRAY_SHOW_LINKS Then

                    LinkRootItem = New ToolStripMenuItem
                    LinkRootItem.Text = "Links"
                    LinkRootItem.Image = My.Resources.url
                    TrayMenu.Items.Insert(r, LinkRootItem)
                    r = r + 1

                    GuildRootItem = New ToolStripMenuItem
                    GuildRootItem.Text = GUILD_NAME
                    GuildRootItem.Image = My.Resources.guild
                    TrayMenu.Items.Insert(r, GuildRootItem)
                    r = r + 1

                    sql = " SELECT LinkLabel, LinkURL, COALESCE(LinkImage,0) AS ImageIdx, COALESCE(IsGuild,0) AS Guild FROM weblink ORDER BY LinkOrder ;"
                    objCmdLink = objConn.CreateCommand()
                    objCmdLink.CommandText = sql
                    objLink = objCmdLink.ExecuteReader()
                    i = 0
                    g = 0
                    While objLink.Read()


                        If objLink("Guild") = "1" And GUILD_LINKS Then
                            GuildItem = New ToolStripMenuItem
                            GuildItem.Text = objLink("LinkLabel")
                            GuildItem.Tag = objLink("LinkURL")


                            If CInt(objLink("ImageIdx")) Then
                                GuildItem.Image = ImageListLinks.Images(CInt(objLink("ImageIdx")))
                            Else
                                GuildItem.Image = My.Resources.guild
                            End If

                            AddHandler GuildItem.Click, AddressOf LinkMenu_Handler
                            GuildRootItem.DropDownItems.Insert(g, GuildItem)
                            g = g + 1
                        Else
                            LinkItem = New ToolStripMenuItem
                            LinkItem.Text = objLink("LinkLabel")
                            LinkItem.Tag = objLink("LinkURL")
                            Dim foo As String
                            foo = objLink("ImageIdx")
                            If CInt(objLink("ImageIdx")) Then
                                LinkItem.Image = ImageListLinks.Images(CInt(objLink("ImageIdx")))
                            Else
                                LinkItem.Image = My.Resources.url
                            End If
                            AddHandler LinkItem.Click, AddressOf LinkMenu_Handler
                            LinkRootItem.DropDownItems.Insert(i, LinkItem)
                            i = i + 1
                        End If

                    End While

                    If i = 0 Then
                        LinkRootItem.Dispose()
                    Else
                        r = r - 1
                    End If

                    If g = 0 Then
                        GuildRootItem.Dispose()
                    Else
                        r = r - 1
                    End If

                End If


                ' --------------------------------------------------------------------------------------
                ' FOLDERS
                ' --------------------------------------------------------------------------------------
                If TRAY_SHOW_FOLDER Then

                    If File.Exists(VANILLA_EXE) Then
                        RootItem = New ToolStripMenuItem
                        RootItem.Name = "MenuFolderVanilla"
                        RootItem.Text = "WoW Folder"
                        RootItem.Image = My.Resources.folder
                        AddHandler RootItem.Click, AddressOf MenuFolderWoW_Click
                        TrayMenu.Items.Add(RootItem)
                    End If

                    If File.Exists(BC_EXE) Then
                        RootItem = New ToolStripMenuItem
                        RootItem.Name = "MenuFolderBC"
                        RootItem.Text = "BC Folder"
                        RootItem.Image = My.Resources.folder
                        AddHandler RootItem.Click, AddressOf MenuFolderBC_Click
                        TrayMenu.Items.Add(RootItem)

                    End If

                End If

                ' --------------------------------------------------------------------------------------
                '  KILL
                ' --------------------------------------------------------------------------------------

                If TRAY_SHOW_KILL Then
                    RootItem = New ToolStripMenuItem
                    RootItem.Name = "MenuKill"
                    RootItem.Text = "Kill WoW"
                    RootItem.Image = My.Resources.gun
                    AddHandler RootItem.Click, AddressOf MenuKill_Click
                    TrayMenu.Items.Add(RootItem)

                    If WOW_SESSIONS.Count = 0 Then
                        RootItem.Enabled = False
                        RootItem.ToolTipText = "Nothing to kill atm"
                    Else
                        RootItem.Enabled = True
                        RootItem.ToolTipText = WOW_SESSIONS.Count & " WoW process" & IIf(WOW_SESSIONS.Count > 1, "es", "")
                    End If

                    For Each WS In WOW_SESSIONS
                        Proc = Nothing

                        Try
                            Proc = Process.GetProcessById(WS.ProcID)
                        Catch ex As Exception

                        End Try

                        If IsNothing(Proc) Then

                            WOW_SESSIONS.Remove(WS)
                        Else
                            If Strings.UCase(Proc.ProcessName) = "WOW" Then

                                ProcName = (WS.SessionName)

                                ProcItem = New ToolStripMenuItem
                                ProcItem.Name = "Proc" & WS.ProcID
                                ProcItem.Text = ProcName
                                ProcItem.Tag = WS.ProcID

                                If WS.WoWversion = 2 Then
                                    ProcItem.Image = My.Resources.V2
                                Else
                                    ProcItem.Image = My.Resources.wow_icon
                                End If

                                '    AccountItem.BackColor = Color.SlateGray
                                AddHandler ProcItem.Click, AddressOf MenuKill_Click
                                RootItem.DropDownItems.Insert(p, ProcItem)

                            End If


                        End If


                        p = p + 1

                    Next

                End If

                ' --------------------------------------------------------------------------------------
                ' EXIT
                ' --------------------------------------------------------------------------------------

                RootItem = New ToolStripMenuItem
                RootItem.Name = "MenuExit"
                RootItem.Text = "Exit"
                RootItem.Image = My.Resources.Close_icon
                AddHandler RootItem.Click, AddressOf MenuExit_Click
                TrayMenu.Items.Add(RootItem)


            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        Finally

        End Try

    End Sub

    Private Sub MacroMenu_Handler(ByVal sender As Object, ByVal e As EventArgs)
        Dim ID As Integer
        Dim myItem As ToolStripMenuItem



        Try

            myItem = CType(sender, ToolStripMenuItem)

            ID = CInt(myItem.Tag)

            If ID = 0 Then Exit Sub

            Start_Macro(ID)


        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try

    End Sub




    Private Sub AccountMenu_Handler(ByVal sender As Object, ByVal e As EventArgs)
        Dim ID As Long
        Dim myItem As ToolStripMenuItem

        Dim params() As String

        Dim WS As WoW_Session

        Try

            myItem = CType(sender, ToolStripMenuItem)

            params = Split(myItem.Tag, ";")

            ID = CLng(params(0))

            If ID = 0 Then Exit Sub

            WS = New WoW_Session
            WS.Set_AccountID(ID)

            If params.Count > 1 Then

                Select Case params(1)

                    Case "Full"
                        WS.IsWindowed = False
                        WS.Screen = 1

                    Case "Screen1"
                        WS.Screen = 1
                        WS.IsWindowed = True

                    Case "Screen2"
                        WS.Screen = 2
                        WS.IsWindowed = True
                End Select

            End If

            If params.Count = 3 Then
                WS.Set_WoW_Window(params(2))
            Else
                WS.Set_WoW_Window()
            End If

            WS.Start_WoW()


        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try

    End Sub



    Private Sub CharMenu_Handler(ByVal sender As Object, ByVal e As EventArgs)
        Dim ID As Long
        Dim myItem As ToolStripMenuItem

        Dim params() As String

        Dim WS As WoW_Session

        Try

            myItem = CType(sender, ToolStripMenuItem)
            params = Split(myItem.Tag, ";")
            ID = CLng(params(0))

            If ID = 0 Then Exit Sub

            WS = New WoW_Session
            WS.Set_CharID(ID)

            If params.Count > 1 Then

                Select Case params(1)

                    Case "Full"
                        WS.IsWindowed = False
                        WS.Screen = 1

                    Case "Screen1"
                        WS.Screen = 1
                        WS.IsWindowed = True

                    Case "Screen2"
                        WS.Screen = 2
                        WS.IsWindowed = True
                End Select

            End If

            If params.Count = 3 Then
                WS.Set_WoW_Window(params(2))
            Else
                WS.Set_WoW_Window()
            End If

            WS.Start_WoW()


        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try

    End Sub


    Private Sub LinkMenu_Handler(ByVal sender As Object, ByVal e As EventArgs)
        Dim url As String
        Dim myItem As ToolStripMenuItem

        Try
            ' Extract the tag value from the item received.
            myItem = CType(sender, ToolStripMenuItem)
            url = myItem.Tag

            Process.Start(url)
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub MenuFolderBC_Click()
        Try
            Process.Start(BC_DIR)
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub MenuFolderWoW_Click()
        Try
            Process.Start(VANILLA_DIR)
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub MenuExit_Click()
        Application.Exit()


    End Sub

    Private Sub MenuKill_Click(ByVal sender As Object, ByVal e As EventArgs)


        Dim ID As Integer
        Dim myItem As ToolStripMenuItem
        Dim myItemParent As ToolStripMenuItem


        Try
            myItem = CType(sender, ToolStripMenuItem)
            ID = CInt(myItem.Tag)

            Kill_WoW_Process(ID)


            If ID Then
                myItemParent = myItem.OwnerItem

                If WOW_SESSIONS.Count Then
                    myItemParent.Enabled = True
                    myItemParent.ToolTipText = WOW_SESSIONS.Count & " WoW process" & IIf(WOW_SESSIONS.Count > 1, "es", "")
                Else
                    myItemParent.Enabled = False
                    myItemParent.ToolTipText = "Nothing to kill atm"
                End If
                myItem.Dispose()

            Else
                myItem.Enabled = False
                myItem.ToolTipText = "Nothing to kill atm"
            End If

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try


    End Sub



    'Public Class JDtoolStripmenuRenderer
    '    Inherit    s ToolStripProfessionalRenderer
    '    Protected Overloads Overrides Sub OnRenderMenuItemBackground(ByVal e As ToolStripItemRenderEventArgs)
    '        Try
    '            Dim rc As New Rectangle(Point.Empty, e.Item.Size)
    '            Dim c As Color

    '            Select Case Strings.Left(e.Item.Name, 3)

    '                Case "Cha"

    '                    c = IIf(e.Item.Selected, Color.DarkSlateGray, Color.Black)

    '                    Using brush As New SolidBrush(c)
    '                        e.Graphics.FillRectangle(brush, rc)
    '                    End Using

    '                Case "Acc"

    '                    c = IIf(e.Item.Selected, Color.DarkSlateGray, Color.DarkGray)

    '                    Using brush As New SolidBrush(c)
    '                        e.Graphics.FillRectangle(brush, rc)
    '                    End Using

    '                Case Else

    '                    c = IIf(e.Item.Selected, Color.FromKnownColor(KnownColor.MenuHighlight), Color.FromKnownColor(KnownColor.MenuBar))

    '                    Using brush As New SolidBrush(c)
    '                        e.Graphics.FillRectangle(brush, rc)
    '                    End Using


    '            End Select


    '        Catch ex As Exception

    '        End Try

    '    End Sub
    'End Class

End Class