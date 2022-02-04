Imports System.Data.SQLite
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Security


Module PublicStuff

    Public Declare Sub keybd_event Lib "user32" (ByVal bVk As Byte, ByVal bScan As Byte, ByVal dwFlags As Integer, ByVal dwExtraInfo As Integer)


    ' SHARED VAR 
    Public WOW_SESSIONS As New List(Of WoW_Session)
    Public Const CONSTRING As String = "Data Source=VanillaLauncher.db;Version=3;Pooling=True;Synchronous=Off;journal mode=Memory;foreign keys=true;;"
    Public IsMainFormLoaded As Boolean


    ' STORED PARAMETERS (From database)
    Public DB_VERSION As Integer
    Public VANILLA_EXE As String
    Public VANILLA_DIR As String
    Public BC_EXE As String
    Public BC_DIR As String
    Public TURTLE_EXE As String
    Public TURTLE_DIR As String
    Public WAIT_LOAD As Integer
    Public WAIT_LOGIN As Integer
    Public WAIT_DOWN As Integer
    Public ISCRYPT As Boolean
    Public LANG As String
    Public TRAY_DEFAULT_WINDOW As Integer
    Public TRAY_SHOW_FOLDER As Boolean
    Public TRAY_SHOW_KILL As Boolean
    Public TRAY_SHOW_SUB_WINDOWED As Boolean
    Public TRAY_SHOW_SUBSUB_WINDOWED As Boolean
    Public SHOW_TASKBAR As Boolean
    Public TRAY_SHOW_LINKS As Boolean
    Public CHAR_IMAGE As String
    Public GUILD_LINKS As Boolean
    Public GUILD_NAME As String
    Public SOFT_KILL As Boolean
    Public MONITORING As Boolean

    ' COMPUTED PARAMETERS
    Public TOTAL_RAM As Long
    Public SKEY As SecureString
    Public NBR_MONITORS As Integer
    Public SCREEN1_RATIO As Single
    Public SCREEN2_RATIO As Single

    ' EASTER EGG
    Public RAGNA_SPEECH As New List(Of String)(New String() {"TOO SOON! YOU HAVE AWAKENED ME TOO SOON, EXECUTUS!",
                                                             "WHAT IS THE MEANING OF THIS INTRUSION?",
                                                             "PAR LE FEU, SOYEZ PURIFIÉS !",
                                                             "GOÛTEZ AUX FLAMMES DE SULFURON !",
                                                             "MEURS, INSECTE !"
                                                             })


    Public Sub Get_Params()

        Dim objConn = New SQLiteConnection(CONSTRING)
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader

        Try

            Dim sql As String = "SELECT ParamName, ParamValue FROM param "

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = sql
                objReader = objCommand.ExecuteReader()

                While (objReader.Read())

                    Select Case objReader("ParamName")

                        Case "VANILLA_EXE"
                            VANILLA_EXE = objReader("ParamValue")

                        Case "BC_EXE"
                            BC_EXE = objReader("ParamValue")

                        Case "TURTLE_EXE"
                            TURTLE_EXE = objReader("ParamValue")

                        Case "Wait_Load"
                            WAIT_LOAD = CInt(objReader("ParamValue"))

                        Case "Wait_Login"
                            WAIT_LOGIN = CInt(objReader("ParamValue"))

                        Case "Wait_Down"
                            WAIT_DOWN = CInt(objReader("ParamValue"))

                        Case "IsCrypt"
                            ISCRYPT = CInt(objReader("ParamValue"))

                        Case "DB_Version"
                            DB_VERSION = CInt(objReader("ParamValue"))

                        Case "Default_Window"
                            TRAY_DEFAULT_WINDOW = CInt(objReader("ParamValue"))

                        Case "ShowFolder"
                            If CInt(objReader("ParamValue")) = 1 Then
                                TRAY_SHOW_FOLDER = True
                            Else
                                TRAY_SHOW_FOLDER = False
                            End If

                        Case "ShowLinks"
                            If CInt(objReader("ParamValue")) = 1 Then
                                TRAY_SHOW_LINKS = True
                            Else
                                TRAY_SHOW_LINKS = False
                            End If

                        Case "Char_Image"
                            CHAR_IMAGE = objReader("ParamValue")

                        Case "ShowKill"
                            If CInt(objReader("ParamValue")) = 1 Then
                                TRAY_SHOW_KILL = True
                            Else
                                TRAY_SHOW_KILL = False
                            End If

                        Case "ShowTaskBar"
                            If CInt(objReader("ParamValue")) = 1 Then
                                SHOW_TASKBAR = True
                            Else
                                SHOW_TASKBAR = False
                            End If

                        Case "GuildLinks"
                            If CInt(objReader("ParamValue")) = 1 Then
                                GUILD_LINKS = True
                            Else
                                GUILD_LINKS = False
                            End If

                        Case "GuildName"
                            GUILD_NAME = objReader("ParamValue")

                        Case "ShowSubWindowed"
                            If CInt(objReader("ParamValue")) = 1 Then
                                TRAY_SHOW_SUB_WINDOWED = True
                            Else
                                TRAY_SHOW_SUB_WINDOWED = False
                            End If

                        Case "ShowSubSubWindowed"
                            If CInt(objReader("ParamValue")) = 1 Then
                                TRAY_SHOW_SUBSUB_WINDOWED = True
                            Else
                                TRAY_SHOW_SUBSUB_WINDOWED = False
                            End If

                        Case "Monitoring"
                            If CInt(objReader("ParamValue")) = 1 Then
                                MONITORING = True
                            Else
                                MONITORING = False
                            End If

                        Case "SoftKill"
                            If CInt(objReader("ParamValue")) = 1 Then
                                SOFT_KILL = True
                            Else
                                SOFT_KILL = False
                            End If

                    End Select

                End While


                If File.Exists(VANILLA_EXE) Then
                    VANILLA_DIR = Path.GetDirectoryName(VANILLA_EXE)
                End If

                If File.Exists(BC_EXE) Then
                    BC_DIR = Path.GetDirectoryName(BC_EXE)
                End If

                If File.Exists(TURTLE_EXE) Then
                    TURTLE_DIR = Path.GetDirectoryName(TURTLE_EXE)
                End If

            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try

    End Sub


    Public Sub Start_Macro(ID As Integer)

        Dim objConn = New SQLiteConnection(CONSTRING)
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader
        Dim WaitTime As Integer
        Dim WS As WoW_Session

        Try


            If ID = 0 Then Exit Sub

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)

                objConn.Open()


                objCommand = objConn.CreateCommand()
                objCommand.CommandText = " SELECT s.*, COALESCE(s.CharID,0) AS CI FROM macro_step s  WHERE s.MacroID=" & ID & " ORDER BY s.StepOrder"
                objReader = objCommand.ExecuteReader()

                While (objReader.Read())

                    WaitTime = CInt(objReader("StepWait"))

                    WS = New WoW_Session

                    Select Case objReader("StepWindow")

                        Case "Windowed"
                            WS.Screen = 1
                            WS.IsWindowed = True

                        Case "Windowed1"
                            WS.Screen = 1
                            WS.IsWindowed = True

                        Case "Windowed2"
                            WS.Screen = 2
                            WS.IsWindowed = True

                        Case Else
                            WS.Screen = 1
                            WS.IsWindowed = False

                    End Select

                    If Not IsDBNull(objReader("StepX")) Then
                        WS.WindowX = CInt(objReader("StepX"))
                    End If

                    If Not IsDBNull(objReader("StepY")) Then
                        WS.WindowY = CInt(objReader("StepY"))
                    End If

                    If Not IsDBNull(objReader("StepW")) Then
                        WS.WindowW = CInt(objReader("StepW"))
                    End If

                    If Not IsDBNull(objReader("StepH")) Then
                        WS.WindowH = CInt(objReader("StepH"))
                    End If

                    If objReader("CI") = 0 Then
                        WS.Set_AccountID(CInt(objReader("AccountID")))
                    Else
                        WS.Set_CharID(CInt(objReader("CharID")))
                    End If

                    WS.Start_WoW()

                    Threading.Thread.Sleep(WaitTime)

                End While

            End Using


        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try


    End Sub





    Public Sub Kill_WoW_Process(ProcID)

        Dim proc As System.Diagnostics.Process
        Dim WS As WoW_Session
        Try

            If ProcID = 0 Then
                For Each WS In WOW_SESSIONS
                    proc = Process.GetProcessById(WS.ProcID)
                    If Strings.UCase(proc.ProcessName) = "WOW" Then

                        If SOFT_KILL Then
                            proc.CloseMainWindow()
                        Else
                            proc.Kill()
                        End If

                    End If
                Next
                WOW_SESSIONS.Clear()

            Else
                proc = Process.GetProcessById(ProcID)
                If Strings.UCase(proc.ProcessName) = "WOW" Then

                    If SOFT_KILL Then
                        proc.CloseMainWindow()
                    Else
                        proc.Kill()
                    End If

                    For Each WS In WOW_SESSIONS
                        If WS.ProcID = ProcID Then
                            WOW_SESSIONS.Remove(WS)
                            Exit For
                        End If
                    Next

                End If

            End If

        Catch ex As Exception

        End Try
    End Sub



    Public Sub Set_Param(strParam As String, strValue As String)


        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand

        Try
            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)

                objConn.Open()
                objCommand = objConn.CreateCommand()
                objCommand.CommandText = "UPDATE param SET ParamValue=$VAL WHERE ParamName=$PARAM"
                objCommand.Parameters.Add("$PARAM", DbType.String).Value = strParam
                objCommand.Parameters.Add("$VAL", DbType.String).Value = strValue
                objCommand.ExecuteNonQuery()

            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try


    End Sub





    Public Function PWD_Encrypt(plainText As String) As String

        Try
            Dim myPtr As IntPtr = Marshal.SecureStringToBSTR(SKEY)

            ' MessageBox.Show("key=" & Marshal.PtrToStringUni(myPtr))

            Dim wrapper As New Simple3Des(Marshal.PtrToStringUni(myPtr))
            Dim cipherText As String = wrapper.EncryptData(plainText)

            Return cipherText
        Catch ex As Exception
            MessageBox.Show("Can't encrypt password: " & ex.Message)
            Return ""
        End Try

    End Function

    Public Function PWD_Decrypt(plainText As String) As String

        Try
            Dim myPtr As IntPtr = Marshal.SecureStringToBSTR(SKEY)

            'MessageBox.Show("key=" & Marshal.PtrToStringUni(myPtr))

            Dim wrapper As New Simple3Des(Marshal.PtrToStringUni(myPtr))
            Dim cipherText As String = wrapper.DecryptData(plainText)

            Return cipherText
        Catch ex As Exception
            MessageBox.Show("Can't decrypt password: " & ex.Message)
            Return ""
        End Try

    End Function


    Public Function GetAllScreensResolution() As Point

        Try
            Dim ResX As Integer =
    (From scr As Screen In Screen.AllScreens Select scr.Bounds.Width).Sum

            Dim ResY As Integer =
                (From scr As Screen In Screen.AllScreens Select scr.Bounds.Height).Sum

            Return New Point(ResX, ResY)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try

    End Function

    Public Function GetPrimaryScreenResolution() As Point
        Try
            Return New Point(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height)
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try


    End Function


    Public Function GetPrimaryScreenWorkingArea() As Point
        Try
            Return New Point(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height)
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try

    End Function

    Public Function GetSecondaryScreenResolution() As Point
        Try
            Dim ResX As Integer =
    (From scr As Screen In Screen.AllScreens Select scr.Bounds.Width).Sum

            Dim ResY As Integer =
                (From scr As Screen In Screen.AllScreens Select scr.Bounds.Height).Sum

            Return New Point(ResX - Screen.PrimaryScreen.Bounds.Width, ResY - Screen.PrimaryScreen.Bounds.Height)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try


    End Function

    Public Function GetSecondaryScreenWorkingArea() As Point
        Try
            Dim ResX As Integer =
    (From scr As Screen In Screen.AllScreens Select scr.WorkingArea.Width).Sum

            Dim ResY As Integer =
                (From scr As Screen In Screen.AllScreens Select scr.WorkingArea.Height).Sum

            Return New Point(ResX - Screen.PrimaryScreen.WorkingArea.Width, ResY - Screen.PrimaryScreen.WorkingArea.Height)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try

    End Function



    Public Function GetPingMs(ByRef hostNameOrAddress As String)
        Dim ping As New System.Net.NetworkInformation.Ping
        Return ping.Send(hostNameOrAddress).RoundtripTime
    End Function



    Public Function Easter_Egg() As String

        Try
            ' Rand 1-100 
            Dim IntChance As Integer = CInt(Math.Ceiling(Rnd() * 100))

            If IntChance > RAGNA_SPEECH.Count Then
                Return ""
            Else
                Return RAGNA_SPEECH(IntChance - 1)
            End If

        Catch ex As Exception

        End Try
    End Function


    Public Function Image_Class(strClass As String) As Image

        Dim img As Image

        Try
            Select Case strClass
                Case "Death Knight"
                    img = My.Resources.class_deathknight

                Case "Demon Hunter"
                    img = My.Resources.class_demonhunter

                Case "Druid"
                    img = My.Resources.class_druid

                Case "Hunter"
                    img = My.Resources.class_hunter

                Case "Mage"
                    img = My.Resources.class_mage

                Case "Monk"
                    img = My.Resources.class_monk

                Case "Paladin"
                    img = My.Resources.class_paladin

                Case "Priest"
                    img = My.Resources.class_priest

                Case "Rogue"
                    img = My.Resources.class_rogue

                Case "Shaman"
                    img = My.Resources.class_shaman

                Case "Warlock"
                    img = My.Resources.class_warlock

                Case "Warrior"
                    img = My.Resources.class_warrior

                Case "Mule"
                    img = My.Resources.mule

                Case Else
                    img = My.Resources.question_mark
            End Select

            Return img

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
            Return My.Resources.question_mark
        End Try

    End Function


    Public Function Image_Race(strRace As String, strGender As String) As Image

        Dim img As Image

        Try

            Select Case strRace

                Case "Goblin"
                    If strGender = "M" Then
                        img = My.Resources.IconSmall_Goblin2_Male
                    Else
                        img = My.Resources.IconSmall_Goblin2_Female
                    End If

                Case "Blood Elf"
                    If strGender = "M" Then
                        img = My.Resources.IconSmall_BloodElf_Male
                    Else
                        img = My.Resources.IconSmall_BloodElf_Female
                    End If


                Case "Troll"
                    If strGender = "M" Then
                        img = My.Resources.IconSmall_Troll_Male
                    Else
                        img = My.Resources.IconSmall_Troll_Female
                    End If

                Case "Tauren"
                    If strGender = "M" Then
                        img = My.Resources.IconSmall_Tauren_Male
                    Else
                        img = My.Resources.IconSmall_Tauren_Female
                    End If

                Case "Undead"
                    If strGender = "M" Then
                        img = My.Resources.IconSmall_Undead_Male
                    Else
                        img = My.Resources.IconSmall_Undead_Female
                    End If

                Case "Orc"
                    If strGender = "M" Then
                        img = My.Resources.IconSmall_Orc_Male
                    Else
                        img = My.Resources.IconSmall_Orc_Female
                    End If

                Case "Pandaren"
                    If strGender = "M" Then
                        img = My.Resources.IconSmall_Pandaren_Male
                    Else
                        img = My.Resources.IconSmall_Pandaren_Female
                    End If

                Case "Worgen"
                    If strGender = "M" Then
                        img = My.Resources.IconSmall_Worgen2_Male
                    Else
                        img = My.Resources.IconSmall_Worgen2_Female
                    End If

                Case "Dranei"
                    If strGender = "M" Then
                        img = My.Resources.IconSmall_Draenei_Male
                    Else
                        img = My.Resources.IconSmall_Draenei_Female
                    End If

                Case "Gnome"
                    If strGender = "M" Then
                        img = My.Resources.IconSmall_Gnome_Male
                    Else
                        img = My.Resources.IconSmall_Gnome_Female
                    End If

                Case "Night Elf"
                    If strGender = "M" Then
                        img = My.Resources.IconSmall_NightElf_Male
                    Else
                        img = My.Resources.IconSmall_NightElf_Female
                    End If

                Case "Dwarf"
                    If strGender = "M" Then
                        img = My.Resources.IconSmall_Dwarf_Male
                    Else
                        img = My.Resources.IconSmall_Dwarf_Female
                    End If

                Case "Human"
                    If strGender = "M" Then
                        img = My.Resources.IconSmall_Human_Male
                    Else
                        img = My.Resources.IconSmall_Human_Female
                    End If

                Case "High Elf"
                    If strGender = "M" Then
                        img = My.Resources.IconSmall_BloodElf_Male
                    Else
                        img = My.Resources.IconSmall_BloodElf_Female
                    End If

                Case "Goblin"
                    If strGender = "M" Then
                        img = My.Resources.IconSmall_Goblin2_Male
                    Else
                        img = My.Resources.IconSmall_Goblin2_Female
                    End If


                Case Else
                    img = My.Resources.question_mark

            End Select

            Return img

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
            Return My.Resources.question_mark
        End Try



    End Function


    Public Function Image_Race_Big(strRace As String, strGender As String) As Image

        Dim img As Image

        Try

            Select Case strRace

                Case "Blood Elf"
                    If strGender = "M" Then
                        img = My.Resources.achievement_character_bloodelf_male
                    Else
                        img = My.Resources.achievement_character_bloodelf_female
                    End If

                Case "Troll"
                    If strGender = "M" Then
                        img = My.Resources.achievement_character_troll_male
                    Else
                        img = My.Resources.achievement_character_troll_female
                    End If

                Case "Tauren"
                    If strGender = "M" Then
                        img = My.Resources.achievement_character_tauren_male
                    Else
                        img = My.Resources.achievement_character_tauren_female
                    End If

                Case "Undead"
                    If strGender = "M" Then
                        img = My.Resources.achievement_character_undead_male
                    Else
                        img = My.Resources.achievement_character_undead_female
                    End If

                Case "Orc"
                    If strGender = "M" Then
                        img = My.Resources.achievement_character_orc_male
                    Else
                        img = My.Resources.achievement_character_orc_female
                    End If


                Case "Dranei"
                    If strGender = "M" Then
                        img = My.Resources.achievement_character_draenei_male
                    Else
                        img = My.Resources.achievement_character_draenei_female
                    End If

                Case "Gnome"
                    If strGender = "M" Then
                        img = My.Resources.achievement_character_gnome_male
                    Else
                        img = My.Resources.achievement_character_gnome_female
                    End If

                Case "Night Elf"
                    If strGender = "M" Then
                        img = My.Resources.achievement_character_nightelf_male
                    Else
                        img = My.Resources.achievement_character_nightelf_female
                    End If

                Case "Dwarf"
                    If strGender = "M" Then
                        img = My.Resources.achievement_character_dwarf_male
                    Else
                        img = My.Resources.achievement_character_dwarf_female
                    End If

                Case "Human"
                    If strGender = "M" Then
                        img = My.Resources.achievement_character_human_male
                    Else
                        img = My.Resources.achievement_character_human_female
                    End If

                Case "High Elf"
                    If strGender = "M" Then
                        img = My.Resources.highelf_male
                    Else
                        img = My.Resources.highelf_female
                    End If

                Case "Goblin"
                    If strGender = "M" Then
                        img = My.Resources.goblin_male
                    Else
                        img = My.Resources.goblin_female
                    End If

                Case Else
                    img = My.Resources.question_mark

            End Select

            Return img

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
            Return My.Resources.question_mark
        End Try



    End Function


End Module
