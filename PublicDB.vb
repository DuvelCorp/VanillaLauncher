Imports System.Data.SQLite

Module PublicDB


    Public Sub UpgradeDatabase()

        Try

            Dim objConn As SQLiteConnection
            Dim objCommand As SQLiteCommand

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)

                objConn.Open()

                ' -----------------------------------------------------------------------------------

                ' Just for upcoming upgragdes if necessary 

                If DB_VERSION < 1 Then


                End If


            End Using


        Catch ex As Exception

            MessageBox.Show("Error: " & ex.Message)
        End Try


    End Sub


    Public Sub CreateDatabase()

        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand

        Try
            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)

                objConn.Open()

                ' Structure creation

                objCommand = objConn.CreateCommand()
                objCommand.CommandText = "CREATE TABLE param (ParamID integer primary key, ParamName varchar(50), ParamValue varchar(200));"
                objCommand.ExecuteNonQuery()

                objCommand.CommandText = "CREATE TABLE wowversion (WoWversionID integer primary key, WoWversionName varchar(30), WoWversionActive boolean, WoWversionExe varchar(300));"
                objCommand.ExecuteNonQuery()

                objCommand.CommandText = "CREATE TABLE server (ServerID integer primary key, WoWversionID integer, ServerName varchar(50), ServerRealmList varchar(100));"
                objCommand.ExecuteNonQuery()

                objCommand.CommandText = "CREATE TABLE realm (RealmID integer primary key, ServerID integer REFERENCES server(ServerID) ON DELETE CASCADE, RealmName varchar(50), RealmType varchar(6), RealmLang char(2));"
                objCommand.ExecuteNonQuery()

                objCommand.CommandText = "CREATE TABLE account (AccountID integer primary key, RealmID integer REFERENCES realm(RealmID) ON DELETE CASCADE, AccountName varchar(50), AccountPassword varchar(50), AccountNote varchar(100), AccountIsFav boolean, AccountIsCustom boolean, AccountX integer, AccountY integer, AccountW integer, AccountH integer);"
                objCommand.ExecuteNonQuery()

                objCommand.CommandText = "CREATE TABLE character (CharID integer primary key, AccountID integer REFERENCES account(AccountID) ON DELETE CASCADE, CharIndex integer, CharIsFav boolean, CharName varchar(30),  CharFaction char(1), CharLevel integer, RaceID integer, ClassID integer, CharGender char(1), CharNote varchar(100));"
                objCommand.ExecuteNonQuery()

                objCommand.CommandText = "CREATE TABLE class (ClassID integer primary key, WoWversionID integer, ClassName varchar(30), ClassColor char(6));"
                objCommand.ExecuteNonQuery()

                objCommand.CommandText = "CREATE TABLE race (RaceID integer primary key, WoWversionID integer, RaceFaction char(1), RaceName varchar(30));"
                objCommand.ExecuteNonQuery()

                objCommand.CommandText = "CREATE TABLE weblink (LinkID integer primary key, LinkOrder integer, LinkLabel varchar(30), LinkURL varchar(500), LinkImage varchar(500), IsGuild boolean);"
                objCommand.ExecuteNonQuery()

                objCommand = objConn.CreateCommand()
                objCommand.CommandText = "CREATE TABLE macro (MacroID integer primary key, MacroIsFav boolean, MacroName varchar(50), MacroNote varchar(200));"
                objCommand.ExecuteNonQuery()

                objCommand = objConn.CreateCommand()
                objCommand.CommandText = "CREATE TABLE macro_step (StepID integer primary key, MacroID integer REFERENCES macro(MacroID) ON DELETE CASCADE, AccountID integer, CharID integer, StepOrder integer, StepWait integer, StepWindow varchar(10), StepX integer, StepY integer,  StepW integer, StepH integer );"
                objCommand.ExecuteNonQuery()



                ' Data creation


                objCommand.CommandText = "INSERT INTO server (ServerID, WoWversionID, ServerName, ServerRealmList) VALUES " &
                                        " (1,3,'Turtle WoW','logon.turtle-wow.org'),              " &
                                        " (2,1,'Nostalgeek','auth.nostalgeek-serveur.com'),              " &
                                        " (3,2,'The Geek Crusade','auth.thegeekcrusade-serveur.com'),    " &
                                        " (4,1,'Darrowshire','ptr.darrowshire.com'),     " &
                                        " (5,1,'Elysium Project','logon.elysium-project.org');     "
                objCommand.ExecuteNonQuery()

                objCommand.CommandText = "INSERT INTO realm (RealmID, ServerID, RealmName, RealmType, RealmLang) VALUES " &
                                        " (1,1,'Turtle WoW','RP','EN'),      " &
                                        " (2,2,'Nostalgeek 1.12','PVP','FR'),      " &
                                        " (3,3,'The Geek Crusade BC','PVP','FR'), " &
                                        " (4,4,'Zul''Mashar','PVP','EN'), " &
                                        " (5,5,'Nighthaven','PVP','EN'); "
                objCommand.ExecuteNonQuery()



                objCommand.CommandText = "INSERT INTO param (ParamID, ParamName, ParamValue) VALUES " &
                                         " (1, 'VANILLA_EXE', '') , " &
                                         " (2, 'BC_EXE', '') , " &
                                         " (3, 'Wait_Load', '3000') , " &
                                         " (4, 'Wait_Login', '2000') , " &
                                         " (5, 'Wait_Down', '200') , " &
                                         " (6, 'IsCrypt', '0') , " &
                                         " (7, 'Default_Window', '0') , " &
                                         " (8, 'ShowFolder', '1') , " &
                                         " (9, 'ShowLinks', '1') , " &
                                         " (10, 'TURTLE_EXE', '') , " &
                                         " (11, 'Char_Image', 'Race') , " &
                                         " (12, 'ShowKill', '1') , " &
                                         " (13, 'ShowTaskBar', '1') , " &
                                         " (14, 'GuildLinks', '1') , " &
                                         " (15, 'GuildName', 'My Guild') , " &
                                         " (16, 'ShowSubWindowed', '1') , " &
                                         " (17, 'ShowSubSubWindowed', '0') , " &
                                         " (18, 'DB_Version', '1') , " &
                                         " (19, 'Monitoring', '1') , " &
                                         " (20, 'SoftKill', '1') ; "


                objCommand.ExecuteNonQuery()

                objCommand.CommandText = "INSERT INTO wowversion (WoWversionID, WoWversionName, WoWVersionActive) VALUES " &
                                         "(1, 'Vanilla', 1), " &
                                         "(2, 'Burning Crusade', 1), " &
                                         "(3, 'Turtle WoW', 1); "
                objCommand.ExecuteNonQuery()



                objCommand.CommandText = "INSERT INTO race (RaceID, WoWversionID, RaceFaction, RaceName) VALUES " &
                                         "(1,1,'A','Human'), " &
                                         "(2,1,'A','Dwarf'), " &
                                         "(3,1,'A','Night Elf'), " &
                                         "(4,1,'A','Gnome'), " &
                                         "(5,2,'A','Dranei'), " &
                                         "(7,1,'H','Orc'), " &
                                         "(8,1,'H','Undead'), " &
                                         "(9,1,'H','Tauren'), " &
                                         "(10,1,'H','Troll'), " &
                                         "(11,2,'H','Blood Elf'), " &
                                         "(12,3,'H','Goblin'), " &
                                         "(13,3,'A','High Elf'); "
                objCommand.ExecuteNonQuery()

                objCommand.CommandText = "INSERT INTO class (ClassID, WoWversionID, ClassName, ClassColor) VALUES " &
                                         "(13, 1, 'Mule', '999999'), " &
                                         "(1, 1, 'Druid', 'FF7D0A'), " &
                                         "(2, 1, 'Hunter', 'ABD473'), " &
                                         "(3, 1, 'Mage', '69CCF0'), " &
                                         "(4, 1, 'Paladin', 'F58CBA'), " &
                                         "(5, 1, 'Priest', 'FFFFFF'), " &
                                         "(6, 1, 'Rogue', 'FFF569'), " &
                                         "(7, 1, 'Shaman', '0070DE'), " &
                                         "(8, 1, 'Warlock','9482C9'), " &
                                         "(9, 1, 'Warrior', 'C79C6E'); "
                objCommand.ExecuteNonQuery()





                objCommand.CommandText = "INSERT INTO weblink (LinkID, IsGuild, LinkOrder, LinkLabel, LinkURL, LinkImage) VALUES " &
                                         " (1, 0, 0, 'WowHead Classic', 'https://classic.wowhead.com/','13') , " &
                                         " (2, 0, 2, 'Nostalgeek', 'https://www.nostalgeek-serveur.com','13') , " &
                                         " (3, 0, 3, 'The Geek Crusade', 'https://www.thegeekcrusade-serveur.com/','13') , " &
                                         " (4, 0, 4, 'Turtle WoW', 'https://turtle-wow.org','13') , " &
                                         " (5, 0, 5, 'Darrowshire', 'https://darrowshire.com/','13') , " &
                                         " (6, 0, 6, 'Elysium Project', 'https://elysium-project.org/','13') , " &
                                         " (7, 0, 1, 'Shagu Add-ons', 'https://shagu.org/','13') ; "

                objCommand.ExecuteNonQuery()


                'DB_VERSION = 1
                'CHAR_IMAGE = "Race"
                'TRAY_SHOW_KILL = True
                'TRAY_SHOW_SUB_WINDOWED = True
                'TRAY_SHOW_SUBSUB_WINDOWED = True
                'SHOW_TASKBAR = False
                'GUILD_LINKS = True
                'GUILD_NAME = "My Guild"




            End Using

        Catch ex As Exception
            MsgBox(Err.Description, MsgBoxStyle.Exclamation, "Error " & Err.Number)
        End Try

    End Sub



    ' Regrouping all DELETE here in case referential integrity doest work

    Public Sub DB_Delete_Char(ID As Long)


        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand

        Try

            If ID = 0 Then Exit Sub

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()
                objCommand.CommandText = "DELETE FROM character WHERE CharID=$ID"
                objCommand.Parameters.Add("$ID", DbType.Int32).Value = ID
                objCommand.ExecuteNonQuery()

            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try


    End Sub


    Public Sub DB_Delete_Account(ID As Long)


        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand

        Try

            If ID = 0 Then Exit Sub

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "DELETE FROM Account WHERE AccountID=$AccountID"
                objCommand.Parameters.Add("$AccountID", DbType.Int32).Value = ID
                objCommand.ExecuteNonQuery()

            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try


    End Sub


    Public Sub DB_Delete_Server(ID As Long)


        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand

        Try

            If ID = 0 Then Exit Sub

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "DELETE FROM server WHERE ServerID=$ServerID"
                objCommand.Parameters.Add("$ServerID", DbType.Int32).Value = ID
                objCommand.ExecuteNonQuery()

            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try


    End Sub


    Public Sub DB_Delete_Realm(ID As Long)


        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand

        Try

            If ID = 0 Then Exit Sub

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "DELETE FROM realm WHERE RealmID=$RealmID"
                objCommand.Parameters.Add("$RealmID", DbType.Int32).Value = ID
                objCommand.ExecuteNonQuery()

            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try


    End Sub

    Public Sub DB_Delete_Macro(ID As Long)


        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand

        Try

            If ID = 0 Then Exit Sub

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "DELETE FROM macro WHERE MacroID=$ID"
                objCommand.Parameters.Add("$ID", DbType.Int32).Value = ID
                objCommand.ExecuteNonQuery()

            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try


    End Sub

    Public Sub DB_Delete_Step(ID As Long)


        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand

        Try

            If ID = 0 Then Exit Sub

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "DELETE FROM macro_step WHERE StepID=$ID"
                objCommand.Parameters.Add("$ID", DbType.Int32).Value = ID
                objCommand.ExecuteNonQuery()

            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try


    End Sub



    Public Sub DB_Delete_Link(ID As Long)


        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand

        Try

            If ID = 0 Then Exit Sub

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "DELETE FROM weblink WHERE LinkID=$LinkID"
                objCommand.Parameters.Add("$LinkID", DbType.Int32).Value = ID
                objCommand.ExecuteNonQuery()

            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try


    End Sub


End Module
