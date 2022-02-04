Imports System.Data.SQLite
Imports System.IO
Imports System.Drawing
Imports System.ComponentModel

Public Class MainForm

    Private LastChar As Integer
    Private LastAccount As Integer
    Private LastRealm As Integer
    Private LastServer As Integer
    Private LastLink As Integer
    Private LastMacro As Integer
    Private LastStep As Integer



    ' #####################################################################################################################"
#Region "Form_Mgt"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load



        Try


            TSSlog.Text = "Welcome ! "
            TSSversion.Text = "VanillaLauncher 2.0"

            StatusStrip1.LayoutStyle = ToolStripLayoutStyle.StackWithOverflow

            TSSlog.Alignment = ToolStripItemAlignment.Right
            TSSdbVersion.Alignment = ToolStripItemAlignment.Right
            TSSversion.Alignment = ToolStripItemAlignment.Right
            TSSbuttonKey.Alignment = ToolStripItemAlignment.Right



            ' ----------------------- All good ------------------------------------

            TextBoxWoWExe.Text = VANILLA_EXE
            TextBoxBCExe.Text = BC_EXE
            TextBoxTurtlexe.Text = TURTLE_EXE
            TextBoxWaitLaunch.Text = WAIT_LOAD
            TextBoxWaitLogin.Text = WAIT_LOGIN
            TextBoxWaitDown.Text = WAIT_DOWN

            ComboBoxConfigTrayCharIcon.SelectedText = CHAR_IMAGE
            ComboBoxConfigTrayCharIcon.Text = CHAR_IMAGE


            If TRAY_SHOW_FOLDER Then
                CheckBoxCOnfigTrayFolders.Checked = True
            Else
                CheckBoxCOnfigTrayFolders.Checked = False
            End If

            If TRAY_SHOW_KILL Then
                CheckBoxCOnfigTrayKill.Checked = True
            Else
                CheckBoxCOnfigTrayFolders.Checked = False
            End If

            If TRAY_SHOW_LINKS Then
                CheckBoxCOnfigTrayLinks.Checked = True
            Else
                CheckBoxCOnfigTrayLinks.Checked = False
            End If

            If SHOW_TASKBAR Then
                CheckBoxConfigShowTaskBar.Checked = True
            Else
                CheckBoxConfigShowTaskBar.Checked = False
            End If

            If TRAY_SHOW_SUB_WINDOWED Then
                CheckBoxCOnfigTraySub.Checked = True
            Else
                CheckBoxCOnfigTraySub.Checked = False
            End If

            If TRAY_SHOW_SUBSUB_WINDOWED Then
                CheckBoxCOnfigTraySubSub.Checked = True
            Else
                CheckBoxCOnfigTraySubSub.Checked = False
            End If


            If SOFT_KILL Then
                RadioButtonKillModeHard.Checked = False
                RadioButtonKillModeSoft.Checked = True
            Else
                RadioButtonKillModeHard.Checked = True
                RadioButtonKillModeSoft.Checked = False
            End If

            If MONITORING Then
                CheckBoxMonitoring.Checked = True

            Else
                CheckBoxMonitoring.Checked = False

            End If


            If VANILLA_EXE = "" And BC_EXE = "" Then
                LogIt("You should locate your WoW.exe file(s) in the settings !", Color.DarkRed)
            End If

            TextBoxSettingsGuildName.Text = GUILD_NAME
            LabelOverviewGuildName.Text = GUILD_NAME

            If GUILD_LINKS Then
                CheckBoxSettingsGuildLinks.Checked = True
            Else
                CheckBoxSettingsGuildLinks.Checked = False
            End If

            ComboBoxStepWindow.Items.Clear()

            ComboBoxStepWindow.Items.Add("Full Screen")
            ComboBoxStepWindow.SelectedText = "Full Screen"
            ComboBoxStepWindow.Text = "Full Screen"
            If NBR_MONITORS = 2 Then
                ComboBoxStepWindow.Items.Add("Windowed Screen 1")
                ComboBoxStepWindow.Items.Add("Windowed Screen 2")
            Else
                ComboBoxStepWindow.Items.Add("Windowed")
            End If

            'Tooltips
            Dim tt As New ToolTip()
            tt.SetToolTip(PictureBoxOverviewVersion, "Click to open folder")
            tt.SetToolTip(LabelOverviewServerAddress, "Click to Ping")
            tt.SetToolTip(LabelOverviewServerPing, "Click to Ping")
            tt.SetToolTip(LabelOverviewServerName, "Click to open Website")


            ' One-shot loads
            ComboBoxConfigTrayAction_Load()
            ComboBoxCharClass_Load()
            ComboBoxCharRace_Load()
            ComboBoxServerWoWversion_Load()
            Load_Links_Icons()

            ListViewOverviewGuild_Load()
            ListViewOverviewLinks_Load()
            ListViewOverviewMacros_Load()

            MainForm_Reload()

            ' TrayIcon.BalloonTipText = "Application Minimized."
            '  TrayIcon.BalloonTipTitle = "VanillaLauncher"

            ComboBoxConfigTrayAction.SelectedValue = TRAY_DEFAULT_WINDOW

            TextBoxHelp.Text = My.Resources.TextFileHelp.ToString


            TSSdbVersion.Text = "DB Version " & DB_VERSION

            IsMainFormLoaded = True

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try

    End Sub


    Public Sub MainForm_Reload()
        Try

            If ISCRYPT Then
                LabelPassWarning.Visible = False
            Else
                LabelPassWarning.Visible = True
            End If

            TreeViewLauncher_Load()
            Overview_Init()

            DataGridViewServers_Load()
            ComboBoxRealmServer_Load()

            DataGridViewRealms_Load()
            ComboBoxAccountRealm_Load()

            DataGridViewAccounts_Load()
            ComboBoxCharAccount_Load()

            DataGridViewChar_Load()

            DataGridViewLinks_Load()

            ComboBoxStepMacro_Load()
            ComboBoxStepAccount_Load()

            DataGridViewMacros_Load()
            DataGridViewSteps_Load()

            'ComboBoxStepChar_Load()



        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub


    Private Sub MainForm_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        Try


            If WindowState = FormWindowState.Minimized Then

                If SHOW_TASKBAR Then
                    ShowInTaskbar = True
                Else
                    ShowInTaskbar = False
                End If

                Dim lol As String
                lol = Easter_Egg()

                TrayIcon.Visible = True

                If lol <> "" Then

                    TrayIcon.BalloonTipText = lol
                    TrayIcon.ShowBalloonTip(2)
                End If



            ElseIf WindowState = FormWindowState.Normal Or WindowState = FormWindowState.Maximized Then
                TabPageMain.BackColor = Color.FromArgb(255, 64, 64, 64)
                TabControl1.SelectedIndex = 0
                Overview_Init()

                If WOW_SESSIONS.Count Then
                    Dim WS As WoW_Session

                    WS = WOW_SESSIONS.Last


                    If WS.CharID = 0 Then
                        Overview_Account(WS.AccountID)
                    Else
                        Overview_Char(WS.CharID)
                    End If
                End If

                Check_Processes()

            End If
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try

    End Sub

    Private Sub TSSbuttonKey_ButtonClick(sender As Object, e As EventArgs) Handles TSSbuttonKey.ButtonClick
        Try
            PasswordForm.Show()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub MainForm_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        Try

            If ISCRYPT Then
                TSSbuttonKey.Visible = True
                CheckBoxCrypt.Checked = True
                LabelPassWarning.Visible = False
            Else
                TSSbuttonKey.Visible = False
                CheckBoxCrypt.Checked = False
                LabelPassWarning.Visible = True
            End If

        Catch ex As Exception


        End Try
    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles TrayIcon.MouseDoubleClick
        Try
            ShowInTaskbar = True
            TrayIcon.Visible = False
            WindowState = FormWindowState.Normal
        Catch ex As Exception

        End Try

    End Sub

    Private Sub TrayIcon_MouseClick(sender As Object, e As MouseEventArgs) Handles TrayIcon.MouseClick
        Try

            If e.Button = Windows.Forms.MouseButtons.Right Then 'Checks if the pressed button is the Right Mouse

                TrayForm.Show() 'Shows the Form that is the parent of "traymenu"
                TrayForm.Activate() 'Set the Form to "Active", that means that that will be the "selected" window
                TrayForm.Width = 1 'Set the Form width to 1 pixel, that is needed because later we will set it behind the "traymenu"
                TrayForm.Height = 1 'Set the Form Height to 1 pixel, for the same reason as above
            End If

        Catch ex As Exception
            'MessageBox.Show("Error: " & ex.Message)
        End Try



    End Sub

    Private Sub Controls_Color_Init()

        Try

            ComboBoxServerWoWversion.BackColor = DefaultBackColor
            TextBoxServerName.BackColor = DefaultBackColor
            TextBoxServerRealmList.BackColor = DefaultBackColor

            ComboBoxRealmServer.BackColor = DefaultBackColor
            TextBoxRealmName.BackColor = DefaultBackColor
            ComboBoxRealmType.BackColor = DefaultBackColor
            ComboBoxRealmLang.BackColor = DefaultBackColor

            ComboBoxAccountRealm.BackColor = DefaultBackColor
            TextBoxAccountName.BackColor = DefaultBackColor
            TextBoxAccountPassword.BackColor = DefaultBackColor
            TextBoxAccountNote.BackColor = DefaultBackColor

            ComboBoxCharAccount.BackColor = DefaultBackColor
            TextBoxCharName.BackColor = DefaultBackColor
            TextBoxCharIndex.BackColor = DefaultBackColor
            ComboBoxCharClass.BackColor = DefaultBackColor
            ComboBoxCharRace.BackColor = DefaultBackColor
            TextBoxCharLevel.BackColor = DefaultBackColor
            TextBoxCharNote.BackColor = DefaultBackColor

            TextBoxMacroName.BackColor = DefaultBackColor
            ComboBoxStepMacro.BackColor = DefaultBackColor
            ComboBoxStepAccount.BackColor = DefaultBackColor


        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try

    End Sub


    Private Sub LogIt(strLog As String, Optional col As Color = Nothing)

        Try

            If col = Nothing Then
                col = Color.Black
            End If

            TSSlog.Text = strLog
            TSSlog.ForeColor = col

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try

    End Sub

    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged
        Try

            Select Case TabControl1.SelectedTab.Name.ToString

                Case TabPageMain.Name.ToString
                    If MONITORING Then
                        Timer1.Enabled = True
                    Else
                        Timer1.Enabled = False
                    End If

                Case Else
                    Timer1.Enabled = False

            End Select


        Catch ex As Exception

        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            If MONITORING Then Check_Processes()

        Catch ex As Exception

        End Try
    End Sub


#End Region


    ' #####################################################################################################################"
#Region "Char_Mgt"

    Private Sub DataGridViewChar_Load()

        Dim conn = New SQLiteConnection(CONSTRING)

        Try
            Using (conn)
                conn.Open()

                Dim sql As String = " SELECT  c.*, ra.RaceName, ra.RaceFaction, cl.ClassName, cl.ClassColor, R.RealmID, R.RealmName, a.AccountName  " &
                                    " FROM character c " &
                                    " LEFT JOIN class cl ON c.CLassID=cl.ClassID " &
                                    " LEFT JOIN race ra ON c.RaceID=ra.RaceID " &
                                    " INNER JOIN account a ON c.AccountID=a.AccountID " &
                                    " INNER JOIN realm r ON r.RealmID=a.RealmID " &
                                    " INNER JOIN server s on S.ServerID=r.ServerID " &
                                    " ORDER BY r.RealmName, a.AccountName, c.CharIndex"

                Dim cmdDataGrid As SQLiteCommand = New SQLiteCommand(sql, conn)

                Dim da As New SQLiteDataAdapter
                da.SelectCommand = cmdDataGrid
                Dim dt As New DataTable
                da.Fill(dt)


                With DataGridViewChar

                    .DataSource = Nothing
                    .AutoGenerateColumns = False
                    .ReadOnly = True
                    .AllowUserToAddRows = False
                    .ColumnCount = 14

                    .Columns(0).Name = "AccountID"
                    .Columns(0).DataPropertyName = "AccountID"
                    .Columns(0).Visible = False

                    .Columns(1).Name = "CharID"
                    .Columns(1).DataPropertyName = "CharID"
                    .Columns(1).Visible = False

                    .Columns(2).Name = "Realm"
                    .Columns(2).DataPropertyName = "RealmName"

                    .Columns(3).Name = "Account"
                    .Columns(3).DataPropertyName = "AccountName"

                    .Columns(4).Name = "Character"
                    .Columns(4).DataPropertyName = "CharName"

                    .Columns(5).Name = "ClassColor"
                    .Columns(5).DataPropertyName = "ClassColor"
                    .Columns(5).Visible = False

                    .Columns(6).Name = "Class"
                    .Columns(6).DataPropertyName = "ClassName"
                    .Columns(6).Visible = False

                    .Columns(7).Name = "CharRace"
                    .Columns(7).DataPropertyName = "RaceName"
                    .Columns(7).Visible = False

                    .Columns(8).Name = "CharFaction"
                    .Columns(8).DataPropertyName = "CharFaction"
                    .Columns(8).Visible = False

                    .Columns(9).Name = "CharGender"
                    .Columns(9).DataPropertyName = "CharGender"
                    .Columns(9).Visible = False

                    .Columns(10).Name = "Level"
                    .Columns(10).DataPropertyName = "CharLevel"

                    .Columns(11).Name = "Note"
                    .Columns(11).DataPropertyName = "CharNote"

                    .Columns(12).Name = "Index"
                    .Columns(12).DataPropertyName = "CharIndex"

                    .Columns(13).Name = "CharIsFav"
                    .Columns(13).DataPropertyName = "CharIsFav"
                    .Columns(13).Visible = False


                    Dim imgFaction As New DataGridViewImageColumn()
                    imgFaction.Image = My.Resources.question_mark
                    imgFaction.HeaderText = "Faction"
                    imgFaction.Width = 8
                    imgFaction.ImageLayout = DataGridViewImageCellLayout.Zoom
                    imgFaction.Name = "imgFaction"
                    .Columns.Add(imgFaction)


                    Dim imgClass As New DataGridViewImageColumn()
                    imgClass.Image = My.Resources.question_mark
                    imgClass.HeaderText = "Class"
                    imgClass.Width = 8
                    imgClass.ImageLayout = DataGridViewImageCellLayout.Zoom
                    imgClass.Name = "imgClass"
                    .Columns.Add(imgClass)

                    Dim imgRace As New DataGridViewImageColumn()
                    imgRace.Image = My.Resources.question_mark
                    imgRace.HeaderText = "Race"
                    imgRace.Width = 8
                    imgRace.ImageLayout = DataGridViewImageCellLayout.Normal
                    imgRace.Name = "imgRace"
                    .Columns.Add(imgRace)


                    Dim imgFav As New DataGridViewImageColumn()
                    imgFav.Image = My.Resources.question_mark
                    imgFav.HeaderText = "Fav"
                    imgFav.Width = 8
                    imgFav.ImageLayout = DataGridViewImageCellLayout.Normal
                    imgFav.Name = "imgFav"
                    .Columns.Add(imgFav)


                    .DataSource = dt
                    Dim readerDataGrid As SQLiteDataReader = cmdDataGrid.ExecuteReader()

                    For Each row As DataGridViewRow In .Rows
                        If row.Cells("CharID").Value = LastChar Then
                            .CurrentCell = row.Cells(2)
                            Exit For
                        End If
                    Next

                End With




            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try

    End Sub

    Private Sub DataGridViewChar_RowEnter(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewChar.RowEnter

        Dim objConn = New SQLiteConnection(CONSTRING)
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader
        Dim ID As Integer

        Try
            ID = DataGridViewChar.Item(1, e.RowIndex).Value

            If ID = 0 Then Exit Sub


            Dim sql As String = " SELECT  * FROM character WHERE CharID = " & ID

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = sql
                objReader = objCommand.ExecuteReader()

                While (objReader.Read())

                    LabelCharID.Text = ID

                    ComboBoxCharAccount.SelectedValue = objReader("AccountID")
                    TextBoxCharName.Text = objReader("CharName")
                    TextBoxCharIndex.Text = objReader("CharIndex")

                    If objReader("CharFaction") = "A" Then
                        RadioButtonCharAlliance.Checked = True
                        RadioButtonCharHorde.Checked = False
                    Else
                        RadioButtonCharAlliance.Checked = False
                        RadioButtonCharHorde.Checked = True
                    End If

                    If objReader("CharGender") = "M" Then
                        RadioButtonCharMale.Checked = True
                        RadioButtonCharFemale.Checked = False
                    Else
                        RadioButtonCharMale.Checked = False
                        RadioButtonCharFemale.Checked = True
                    End If

                    ComboBoxCharClass.SelectedValue = objReader("ClassID")
                    ComboBoxCharRace.SelectedValue = objReader("RaceID")

                    TextBoxCharLevel.Text = objReader("CharLevel")
                    TextBoxCharNote.Text = objReader("CharNote")


                    If CBool(objReader("CharIsFav")) = True Then
                        CheckBoxCharIsFav.Checked = True
                    Else
                        CheckBoxCharIsFav.Checked = False
                    End If

                End While

            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub


    Private Sub ButtonCharAdd_Click(sender As Object, e As EventArgs) Handles ButtonCharAdd.Click
        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader
        Dim ID As Long
        Dim strGender As String
        Dim strFaction As String

        Try

            Controls_Color_Init()

            If ComboBoxCharAccount.SelectedValue = "" Or ComboBoxCharAccount.SelectedValue = 0 Then
                ComboBoxCharAccount.BackColor = Color.IndianRed
                Exit Sub
            End If

            If TextBoxCharName.Text = "" Then
                TextBoxCharName.BackColor = Color.IndianRed
                Exit Sub
            End If


            If ComboBoxCharClass.SelectedValue = "" Or ComboBoxCharClass.SelectedValue = 0 Then
                ComboBoxCharClass.BackColor = Color.IndianRed
                Exit Sub
            End If

            If ComboBoxCharRace.SelectedValue = "" Or ComboBoxCharRace.SelectedValue = 0 Then
                ComboBoxCharRace.BackColor = Color.IndianRed
                Exit Sub
            End If

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "SELECT MAX(CharID) as ID FROM character"
                objReader = objCommand.ExecuteReader()
                While (objReader.Read())
                    If IsDBNull(objReader("ID")) Then
                        ID = 1
                    Else
                        ID = CLng(objReader("ID")) + 1
                    End If

                End While
                objReader.Close()


                If RadioButtonCharMale.Checked Then
                    strGender = "M"
                Else
                    strGender = "F"
                End If

                If RadioButtonCharAlliance.Checked Then
                    strFaction = "A"
                Else
                    strFaction = "H"
                End If


                If Not IsNumeric((TextBoxCharIndex.Text)) Then
                    TextBoxCharIndex.Text = 1
                End If

                If Not IsNumeric((TextBoxCharLevel.Text)) Then
                    TextBoxCharLevel.Text = 1
                End If


                objCommand.CommandText = "INSERT INTO character (CharID, AccountID, CharIndex, CharName, CharFaction, CharLevel, RaceID, ClassID, CharGender, CharNote, CharIsFav) VALUES ($CharID, $AccountID, $CharIndex, $CharName, $CharFaction, $CharLevel, $RaceID, $ClassID, $CharGender, $CharNote, $CharIsFav);"
                objCommand.Parameters.Add("$CharID", DbType.Int32).Value = ID
                objCommand.Parameters.Add("$AccountID", DbType.Int32).Value = ComboBoxCharAccount.SelectedValue
                objCommand.Parameters.Add("$CharIndex", DbType.Int32).Value = CInt(TextBoxCharIndex.Text)
                objCommand.Parameters.Add("$CharLevel", DbType.Int32).Value = CInt(TextBoxCharLevel.Text)
                objCommand.Parameters.Add("$CharName", DbType.String).Value = TextBoxCharName.Text
                objCommand.Parameters.Add("$RaceID", DbType.Int32).Value = ComboBoxCharRace.SelectedValue
                objCommand.Parameters.Add("$ClassID", DbType.Int32).Value = ComboBoxCharClass.SelectedValue
                objCommand.Parameters.Add("$CharGender", DbType.String).Value = strGender
                objCommand.Parameters.Add("$CharFaction", DbType.String).Value = strFaction
                objCommand.Parameters.Add("$CharNote", DbType.String).Value = TextBoxCharNote.Text
                objCommand.Parameters.Add("$CharIsFav", DbType.Boolean).Value = CheckBoxCharIsFav.Checked

                objCommand.ExecuteNonQuery()

                LastChar = ID
                MainForm_Reload()
            End Using

            LogIt("Character " & TextBoxCharName.Text & " Added", Color.DarkGreen)

        Catch ex As Exception
            LogIt("Character creation failed ! ", Color.Red)
            MessageBox.Show("Error: " & ex.Message)

        End Try
    End Sub

    Private Sub ButtonCharSave_Click(sender As Object, e As EventArgs) Handles ButtonCharSave.Click
        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim ID As Long
        Dim strGender As String
        Dim strFaction As String

        Try
            ID = CInt(LabelCharID.Text)

            If ID = 0 Then Exit Sub

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                If RadioButtonCharMale.Checked Then
                    strGender = "M"
                Else
                    strGender = "F"
                End If

                If RadioButtonCharAlliance.Checked Then
                    strFaction = "A"
                Else
                    strFaction = "H"
                End If

                objCommand.CommandText = "UPDATE character SET AccountID=$AccountID, CharIndex=$CharIndex, CharIsFav=$CharIsFav, CharName=$CharName, CharFaction=$CharFaction, CharLevel=$CharLevel, RaceID=$RaceID, ClassID=$ClassID, CharGender=$CharGender, CharNote=$CharNote WHERE CharID=$CharID;"
                objCommand.Parameters.Add("$CharID", DbType.Int32).Value = ID
                objCommand.Parameters.Add("$AccountID", DbType.Int32).Value = ComboBoxCharAccount.SelectedValue
                objCommand.Parameters.Add("$CharIndex", DbType.Int32).Value = CInt(TextBoxCharIndex.Text)
                objCommand.Parameters.Add("$CharLevel", DbType.Int32).Value = CInt(TextBoxCharLevel.Text)
                objCommand.Parameters.Add("$CharName", DbType.String).Value = TextBoxCharName.Text
                objCommand.Parameters.Add("$RaceID", DbType.Int32).Value = ComboBoxCharRace.SelectedValue
                objCommand.Parameters.Add("$ClassID", DbType.Int32).Value = ComboBoxCharClass.SelectedValue
                objCommand.Parameters.Add("$CharGender", DbType.String).Value = strGender
                objCommand.Parameters.Add("$CharFaction", DbType.String).Value = strFaction
                objCommand.Parameters.Add("$CharNote", DbType.String).Value = TextBoxCharNote.Text
                objCommand.Parameters.Add("$CharIsFav", DbType.Boolean).Value = CheckBoxCharIsFav.Checked

                objCommand.ExecuteNonQuery()

                LastChar = ID
                MainForm_Reload()
            End Using

            LogIt("Character " & TextBoxCharName.Text & " modified", Color.DarkGreen)

        Catch ex As Exception
            LogIt("Modification failed ! ", Color.Red)
            MessageBox.Show("Error: " & ex.Message)

        End Try
    End Sub

    Private Sub ButtonCharDel_Click(sender As Object, e As EventArgs) Handles ButtonCharDel.Click
        Dim ID As Long

        Try
            ID = CInt(LabelCharID.Text)
            If ID = 0 Then Exit Sub

            DB_Delete_Char(ID)

            MainForm_Reload()

            LogIt("Character deleted", Color.DarkGreen)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try
    End Sub




    Private Sub ComboBoxCharClass_Load()

        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader
        Dim dt As New DataTable
        Dim strFaction As String

        Try

            If RadioButtonCharAlliance.Checked Then
                strFaction = "A"
            Else
                strFaction = "H"
            End If

            dt.Columns.Add("ID")
            dt.Columns.Add("Name")
            'dt.Rows.Add("", "")

            dt.Rows.Add(0, "")

            objConn = New SQLiteConnection(CONSTRING)

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "SELECT ClassID, ClassName FROM class WHERE WoWversionID<=3  ORDER BY ClassName"
                objReader = objCommand.ExecuteReader()
                While (objReader.Read())
                    dt.Rows.Add(objReader("ClassID"), objReader("ClassName"))
                End While

                ComboBoxCharClass.DataSource = dt

                ComboBoxCharClass.ValueMember = "ID"
                ComboBoxCharClass.DisplayMember = "Name"
                ComboBoxCharClass.DropDownStyle = ComboBoxStyle.DropDown
            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try
    End Sub


    Private Sub ComboBoxCharRace_Load()

        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader
        Dim dt As New DataTable
        Dim strFaction As String

        Try

            If RadioButtonCharAlliance.Checked Then
                strFaction = "A"
            Else
                strFaction = "H"
            End If


            dt.Columns.Add("ID")
            dt.Columns.Add("Name")
            'dt.Rows.Add("", "")

            dt.Rows.Add(0, "")

            objConn = New SQLiteConnection(CONSTRING)

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "SELECT RaceID, RaceName FROM race WHERE WoWversionID<=3 AND RaceFaction='" & strFaction & "' ORDER BY RaceName"
                objReader = objCommand.ExecuteReader()
                While (objReader.Read())
                    dt.Rows.Add(objReader("RaceID"), objReader("RaceName"))
                End While

                ComboBoxCharRace.DataSource = dt

                ComboBoxCharRace.ValueMember = "ID"
                ComboBoxCharRace.DisplayMember = "Name"
                ComboBoxCharRace.DropDownStyle = ComboBoxStyle.DropDown
            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try
    End Sub

    Private Sub ComboBoxCharAccount_Load()

        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader
        Dim dt As New DataTable

        Try

            dt.Columns.Add("ID")
            dt.Columns.Add("Name")
            'dt.Rows.Add("", "")

            dt.Rows.Add(0, "")

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "SELECT  (a.AccountName || ' of ' ||  R.RealmName) AS Name, a.AccountID AS ID FROM account a INNER JOIN realm r ON r.RealmID=a.RealmID INNER JOIN server s on S.ServerID=r.ServerID ORDER BY r.RealmName, a.AccountName"
                objReader = objCommand.ExecuteReader()
                While (objReader.Read())
                    dt.Rows.Add(objReader("ID"), objReader("Name"))
                End While

                ComboBoxCharAccount.DataSource = dt

                ComboBoxCharAccount.ValueMember = "ID"
                ComboBoxCharAccount.DisplayMember = "Name"
                ComboBoxCharAccount.DropDownStyle = ComboBoxStyle.DropDown
            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try
    End Sub


    Private Sub GroupBoxCharFaction_Validated(sender As Object, e As EventArgs)
        ComboBoxCharRace_Load()
    End Sub



    Private Sub DataGridViewChar_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridViewChar.CellFormatting

        Try

            If e.RowIndex < 0 Then Exit Sub

            Select Case e.ColumnIndex

                Case 4 ' Character Name (to color)
                    DataGridViewChar.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.ForeColor = ColorTranslator.FromHtml("#" & DataGridViewChar.Rows(e.RowIndex).Cells(5).Value)

                Case 14 'Faction image (faction name in 8)
                    Select Case DataGridViewChar.Rows(e.RowIndex).Cells(8).Value
                        Case "A"
                            e.Value = My.Resources.Alliance_32
                        Case "H"
                            e.Value = My.Resources.Horde_32
                    End Select


                Case 15 'Class image (class name in 6)

                    e.Value = Image_Class(DataGridViewChar.Rows(e.RowIndex).Cells(6).Value)


                Case 16 'Race image (race name in 7 , gender in 9)

                    e.Value = Image_Race(DataGridViewChar.Rows(e.RowIndex).Cells(7).Value, DataGridViewChar.Rows(e.RowIndex).Cells(9).Value)


                Case 17 ' Fav image (Fav value in 13)
                    Select Case CBool(DataGridViewChar.Rows(e.RowIndex).Cells(13).Value)
                        Case False
                            e.Value = New Bitmap(1, 1)
                        Case True
                            e.Value = My.Resources.favorite

                    End Select

            End Select

        Catch ex As Exception

        End Try
    End Sub

    Private Sub Set_Class_Image()
        Try
            If ComboBoxCharClass.Text <> "" Then
                PictureBoxCharClass.Image = Image_Class(ComboBoxCharClass.Text)
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub Set_Race_Image()
        Try
            If ComboBoxCharRace.Text <> "" Then
                If RadioButtonCharFemale.Checked Then
                    PictureBoxCharRace.Image = Image_Race(ComboBoxCharRace.Text, "F")
                Else
                    PictureBoxCharRace.Image = Image_Race(ComboBoxCharRace.Text, "M")
                End If
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub ComboBoxCharRace_SelectedValueChanged(sender As Object, e As EventArgs) Handles ComboBoxCharRace.SelectedValueChanged
        Set_Race_Image()
    End Sub

    Private Sub ComboBoxCharClass_SelectedValueChanged(sender As Object, e As EventArgs) Handles ComboBoxCharClass.SelectedValueChanged
        Set_Class_Image()
    End Sub

    Private Sub RadioButtonCharMale_CheckedChanged(sender As Object, e As EventArgs)
        Set_Race_Image()
    End Sub

    Private Sub RadioButtonCharFemale_CheckedChanged(sender As Object, e As EventArgs)
        Set_Race_Image()
    End Sub

#End Region


    ' #####################################################################################################################"
#Region "Account_Mgt"


    Private Sub DataGridViewAccounts_Load()

        Dim conn = New SQLiteConnection(CONSTRING)

        Try


            Using (conn)
                conn.Open()

                Dim sql As String = "SELECT  R.RealmID, R.RealmName, a.AccountID, A.*  FROM account a INNER JOIN realm r ON r.RealmID=a.RealmID INNER JOIN server s on S.ServerID=r.ServerID"

                Dim cmdDataGrid As SQLiteCommand = New SQLiteCommand(sql, conn)

                Dim da As New SQLiteDataAdapter
                da.SelectCommand = cmdDataGrid
                Dim dt As New DataTable
                da.Fill(dt)



                With DataGridViewAccounts

                    .DataSource = Nothing
                    .AutoGenerateColumns = False
                    .ReadOnly = True
                    .AllowUserToAddRows = False
                    .ColumnCount = 12

                    .Columns(0).Name = "ID"
                    .Columns(0).DataPropertyName = "AccountID"
                    .Columns(0).Visible = False

                    .Columns(1).Name = "Realm"
                    .Columns(1).DataPropertyName = "RealmName"

                    .Columns(2).Name = "Account"
                    .Columns(2).DataPropertyName = "AccountName"


                    .Columns(3).DataPropertyName = "AccountPassword"

                    If ISCRYPT Then
                        .Columns(3).Name = "Encrypted Password"
                        .Columns(3).Visible = True
                    Else
                        .Columns(3).Name = "Password"
                        If CheckBoxShowPasswords.Checked Then
                            .Columns(3).Visible = True
                        Else
                            .Columns(3).Visible = False
                        End If
                    End If




                    .Columns(4).Name = "RealmID"
                    .Columns(4).DataPropertyName = "RealmID"
                    .Columns(4).Visible = False

                    .Columns(5).Name = "Note"
                    .Columns(5).DataPropertyName = "AccountNote"

                    .Columns(6).Name = "IsCustom"
                    .Columns(6).DataPropertyName = "AccountIsCustom"
                    .Columns(6).Visible = False

                    .Columns(7).Name = "Pos X"
                    .Columns(7).DataPropertyName = "AccountX"
                    .Columns(7).Visible = False

                    .Columns(8).Name = "Pos Y"
                    .Columns(8).DataPropertyName = "AccountY"
                    .Columns(8).Visible = False

                    .Columns(9).Name = "Width"
                    .Columns(9).DataPropertyName = "AccountW"
                    .Columns(9).Visible = False

                    .Columns(10).Name = "Height"
                    .Columns(10).DataPropertyName = "AccountH"
                    .Columns(10).Visible = False

                    .Columns(11).Name = "IsFav"
                    .Columns(11).DataPropertyName = "AccountIsFav"
                    .Columns(11).Visible = False

                    Dim imgCustom As New DataGridViewImageColumn()
                    imgCustom.Image = My.Resources.custom
                    imgCustom.HeaderText = "Custom Window?"
                    imgCustom.Width = 8
                    imgCustom.ImageLayout = DataGridViewImageCellLayout.Zoom
                    imgCustom.Name = "imgCustom"
                    .Columns.Add(imgCustom)
                    .Columns(12).Visible = False

                    Dim imgFav As New DataGridViewImageColumn()
                    imgFav.Image = My.Resources.question_mark
                    imgFav.HeaderText = "Fav"
                    imgFav.Width = 8
                    imgFav.ImageLayout = DataGridViewImageCellLayout.Zoom
                    imgFav.Name = "imgFav"
                    .Columns.Add(imgFav)


                    .DataSource = dt

                    For Each row As DataGridViewRow In .Rows
                        If row.Cells("ID").Value = LastAccount Then
                            .CurrentCell = row.Cells(1)
                            Exit For
                        End If
                    Next

                End With

                Dim readerDataGrid As SQLiteDataReader = cmdDataGrid.ExecuteReader()


            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally

        End Try

    End Sub


    Private Sub CheckBoxShowPasswords_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxShowPasswords.CheckedChanged

        Dim ID As Integer

        Try
            If CheckBoxShowPasswords.Checked Then
                TextBoxAccountPassword.UseSystemPasswordChar = False
            Else
                TextBoxAccountPassword.UseSystemPasswordChar = True
            End If

            If LabelAccountID.Text = "" Or LabelAccountID.Text = ".." Then
                ID = 0
            Else
                ID = CInt(LabelAccountID.Text)
            End If

            LastAccount = ID
            DataGridViewAccounts_Load()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub ComboBoxAccountRealm_Load()

        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader
        Dim dt As New DataTable

        Try

            dt.Columns.Add("ID")
            dt.Columns.Add("Name")

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "SELECT r.RealmID, r.RealmName, s.ServerID, s.ServerName FROM realm r INNER JOIN server s on r.ServerID=s.ServerID ORDER BY r.RealmName"
                objReader = objCommand.ExecuteReader()
                While (objReader.Read())
                    dt.Rows.Add(objReader("RealmID"), (objReader("RealmName") & " of " & objReader("ServerName")))
                End While

                ComboBoxAccountRealm.DataSource = dt

                ComboBoxAccountRealm.ValueMember = "ID"
                ComboBoxAccountRealm.DisplayMember = "Name"
                ComboBoxAccountRealm.DropDownStyle = ComboBoxStyle.DropDown
            End Using

        Catch ex As Exception

        End Try
    End Sub

    Private Sub DataGridViewAccounts_RowEnter(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewAccounts.RowEnter

        Try
            LabelAccountID.Text = DataGridViewAccounts.Item(0, e.RowIndex).Value
            ComboBoxAccountRealm.SelectedValue = DataGridViewAccounts.Item(4, e.RowIndex).Value
            TextBoxAccountName.Text = DataGridViewAccounts.Item(2, e.RowIndex).Value
            TextBoxAccountNote.Text = DataGridViewAccounts.Item(5, e.RowIndex).Value

            If CheckBoxShowPasswords.Checked Then
                TextBoxAccountPassword.UseSystemPasswordChar = False
            Else
                TextBoxAccountPassword.UseSystemPasswordChar = True
            End If

            If ISCRYPT Then
                If DataGridViewAccounts.Item(3, e.RowIndex).Value = "" Then
                    TextBoxAccountPassword.Text = ""
                Else
                    Dim cipher As String = PWD_Decrypt(DataGridViewAccounts.Item(3, e.RowIndex).Value)
                    TextBoxAccountPassword.Text = cipher
                End If
            Else
                TextBoxAccountPassword.Text = DataGridViewAccounts.Item(3, e.RowIndex).Value
            End If


        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try


    End Sub


    Private Sub CheckBoxAccountCustom_CheckedChanged(sender As Object, e As EventArgs)


    End Sub

    Private Sub ButtonAccountAdd_Click(sender As Object, e As EventArgs) Handles ButtonAccountAdd.Click

        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader
        Dim ID As Long


        Try

            Controls_Color_Init()

            If ComboBoxAccountRealm.SelectedValue = "" Or ComboBoxAccountRealm.SelectedValue = 0 Then
                ComboBoxAccountRealm.BackColor = Color.IndianRed
                Exit Sub
            End If

            If TextBoxAccountName.Text = "" Then
                TextBoxAccountName.BackColor = Color.IndianRed
                Exit Sub
            End If
            If TextBoxAccountPassword.Text = "" Then
                TextBoxAccountName.BackColor = Color.IndianRed
                Exit Sub
            End If

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "SELECT MAX(accountID) as ID FROM Account"
                objReader = objCommand.ExecuteReader()
                While (objReader.Read())
                    If IsDBNull(objReader("ID")) Then
                        ID = 1
                    Else
                        ID = CLng(objReader("ID")) + 1
                    End If

                End While
                objReader.Close()

                objCommand.CommandText = "INSERT INTO Account (accountID, RealmID, AccountName, AccountPassword, AccountNote, AccountIsFav) " &
                                         " VALUES ($AccountID, $RealmID, $AccountName, $AccountPassword, $AccountNote,$AccountIsFav) "
                objCommand.Parameters.Add("$RealmID", DbType.Int32).Value = ComboBoxAccountRealm.SelectedValue
                objCommand.Parameters.Add("$AccountName", DbType.String).Value = TextBoxAccountName.Text
                objCommand.Parameters.Add("$AccountNote", DbType.String).Value = TextBoxAccountNote.Text
                objCommand.Parameters.Add("$AccountID", DbType.Int32).Value = ID
                objCommand.Parameters.Add("$AccountIsFav", DbType.Boolean).Value = IIf(CheckBoxAccountIsFav.Checked, 1, 0)



                If ISCRYPT Then
                    Dim cipher As String = PWD_Encrypt(TextBoxAccountPassword.Text)
                    objCommand.Parameters.Add("$AccountPassword", DbType.String).Value = cipher
                Else
                    objCommand.Parameters.Add("$AccountPassword", DbType.String).Value = TextBoxAccountPassword.Text
                End If


                objCommand.ExecuteNonQuery()

                LastAccount = ID
                MainForm_Reload()
            End Using

            LogIt("Account " & TextBoxAccountName.Text & " Added", Color.DarkGreen)

        Catch ex As Exception
            LogIt("Account creation failed ! ", Color.Red)
            MessageBox.Show("Error: " & ex.Message)

        End Try
    End Sub

    Private Sub ButtonAccountSave_Click(sender As Object, e As EventArgs) Handles ButtonAccountSave.Click

        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim ID As Long

        Try

            ID = CInt(LabelAccountID.Text)

            If ID = 0 Then Exit Sub

            If TextBoxAccountName.Text = "" Then
                TextBoxAccountName.BackColor = Color.IndianRed
                Exit Sub
            End If
            If TextBoxAccountPassword.Text = "" Then
                TextBoxAccountName.BackColor = Color.IndianRed
                Exit Sub
            End If

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "UPDATE Account SET RealmID=$RealmID, AccountName=$AccountName, AccountPassword=$AccountPassword , AccountIsFav=$AccountIsFav, AccountNote=$AccountNote WHERE AccountID=$AccountID ;"
                objCommand.Parameters.Add("$RealmID", DbType.Int32).Value = ComboBoxAccountRealm.SelectedValue
                objCommand.Parameters.Add("$AccountName", DbType.String).Value = TextBoxAccountName.Text
                objCommand.Parameters.Add("$AccountNote", DbType.String).Value = TextBoxAccountNote.Text
                objCommand.Parameters.Add("$AccountID", DbType.Int32).Value = ID

                objCommand.Parameters.Add("$AccountIsFav", DbType.Boolean).Value = CheckBoxAccountIsFav.Checked


                If ISCRYPT Then
                    Dim cipher As String = PWD_Encrypt(TextBoxAccountPassword.Text)
                    objCommand.Parameters.Add("$AccountPassword", DbType.String).Value = cipher
                Else
                    objCommand.Parameters.Add("$AccountPassword", DbType.String).Value = TextBoxAccountPassword.Text
                End If

                objCommand.ExecuteNonQuery()

                LastAccount = ID
                MainForm_Reload()

            End Using

            LogIt("Account " & TextBoxAccountName.Text & " modified", Color.DarkGreen)

        Catch ex As Exception
            LogIt("Modification failed ! ", Color.Red)
            MessageBox.Show("Error: " & ex.Message)

        End Try
    End Sub


    Private Sub ButtonAccountDel_Click(sender As Object, e As EventArgs) Handles ButtonAccountDel.Click

        Dim ID As Long

        Try
            ID = CInt(LabelAccountID.Text)
            If ID = 0 Then Exit Sub

            DB_Delete_Account(ID)

            MainForm_Reload()

            LogIt("Account deleted", Color.DarkGreen)
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try
    End Sub


    Private Sub DataGridViewAccounts_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridViewAccounts.CellFormatting
        Try

            If e.RowIndex < 0 Then Exit Sub

            Select Case e.ColumnIndex


                Case 12 ' IsCUstom image (IsCustom in 6)
                    Select Case DataGridViewAccounts.Rows(e.RowIndex).Cells(6).Value
                        Case 0
                            e.Value = New Bitmap(1, 1)
                        Case 1

                    End Select

                Case 13 ' Fav image (IsFav in 11)
                    Select Case CBool(DataGridViewAccounts.Rows(e.RowIndex).Cells(11).Value)
                        Case False
                            e.Value = New Bitmap(1, 1)
                        Case True
                            e.Value = My.Resources.favorite

                    End Select

            End Select

        Catch ex As Exception

        End Try
    End Sub


#End Region


    ' #####################################################################################################################"
#Region "Realm_Mgt"

    Private Sub DataGridViewRealms_Load()

        Dim conn = New SQLiteConnection(CONSTRING)

        Try
            Using (conn)
                conn.Open()

                Dim sql As String = "SELECT R.*, S.ServerID, S.ServerName, V.WoWversionID, V.WoWversionName FROM realm R INNER JOIN server S ON R.ServerID = S.ServerID INNER JOIN wowversion V ON S.WoWversionID=V.WoWversionID ORDER BY V.WoWversionID, S.ServerID, R.RealmName"

                Dim cmdDataGrid As SQLiteCommand = New SQLiteCommand(sql, conn)

                Dim da As New SQLiteDataAdapter
                da.SelectCommand = cmdDataGrid
                Dim dt As New DataTable
                da.Fill(dt)

                With DataGridViewRealms

                    .DataSource = Nothing

                    .AutoGenerateColumns = False
                    .ReadOnly = True
                    .AllowUserToAddRows = False
                    .ColumnCount = 8

                    .Columns(0).Name = "VersionID"
                    .Columns(0).DataPropertyName = "WoWversionName"
                    .Columns(0).Visible = False

                    .Columns(1).Name = "Version"
                    .Columns(1).DataPropertyName = "WoWversionName"

                    .Columns(2).Name = "ServerID"
                    .Columns(2).DataPropertyName = "ServerID"
                    .Columns(2).Visible = False

                    .Columns(3).Name = "Server"
                    .Columns(3).DataPropertyName = "ServerName"

                    .Columns(4).Name = "RealmID"
                    .Columns(4).DataPropertyName = "RealmID"
                    .Columns(4).Visible = False

                    .Columns(5).Name = "Realm"
                    .Columns(5).DataPropertyName = "RealmName"

                    .Columns(6).Name = "Type"
                    .Columns(6).DataPropertyName = "RealmType"

                    .Columns(7).Name = "Lang"
                    .Columns(7).DataPropertyName = "RealmLang"

                    .DataSource = dt
                    Dim readerDataGrid As SQLiteDataReader = cmdDataGrid.ExecuteReader()

                    For Each row As DataGridViewRow In .Rows
                        If row.Cells("RealmID").Value = LastRealm Then
                            .CurrentCell = row.Cells(5)
                            Exit For
                        End If
                    Next

                End With


            End Using

        Catch ex As Exception
            MsgBox(ex.ToString())

        Finally

        End Try

    End Sub

    Private Sub DataGridViewRealms_RowEnter(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewRealms.RowEnter

        Try
            LabelRealmID.Text = DataGridViewRealms.Item(4, e.RowIndex).Value
            ComboBoxRealmServer.SelectedValue = DataGridViewRealms.Item(2, e.RowIndex).Value
            TextBoxRealmName.Text = DataGridViewRealms.Item(5, e.RowIndex).Value
            ComboBoxRealmType.Text = DataGridViewRealms.Item(6, e.RowIndex).Value
            ComboBoxRealmLang.Text = DataGridViewRealms.Item(7, e.RowIndex).Value

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub


    Private Sub ComboBoxRealmServer_Load()

        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader
        Dim dt As New DataTable

        Try

            dt.Columns.Add("ID")
            dt.Columns.Add("Name")
            'dt.Rows.Add("", "")

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "SELECT ServerID, ServerName FROM server ORDER BY ServerID"
                objReader = objCommand.ExecuteReader()
                While (objReader.Read())
                    dt.Rows.Add(objReader("ServerID"), objReader("ServerName"))
                End While

                ComboBoxRealmServer.DataSource = dt

                ComboBoxRealmServer.ValueMember = "ID"
                ComboBoxRealmServer.DisplayMember = "Name"
                ComboBoxRealmServer.DropDownStyle = ComboBoxStyle.DropDown
            End Using

        Catch ex As Exception

        End Try
    End Sub


    Private Sub ButtonRealmAdd_Click(sender As Object, e As EventArgs) Handles ButtonRealmAdd.Click
        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader
        Dim ID As Long


        Try
            Controls_Color_Init()

            If ComboBoxRealmServer.SelectedValue = "" Or ComboBoxRealmServer.SelectedValue = 0 Then
                ComboBoxRealmServer.BackColor = Color.IndianRed
                Exit Sub
            End If


            If TextBoxRealmName.Text = "" Then
                TextBoxRealmName.BackColor = Color.IndianRed
                Exit Sub
            End If

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "SELECT MAX(RealmID) as ID FROM realm"
                objReader = objCommand.ExecuteReader()
                While (objReader.Read())
                    If IsDBNull(objReader("ID")) Then
                        ID = 1
                    Else
                        ID = CLng(objReader("ID")) + 1
                    End If

                End While
                objReader.Close()


                objCommand.CommandText = "INSERT INTO realm (RealmID, ServerID, RealmName, RealmType, RealmLang) " &
                                         " VALUES ($RealmID, $ServerID, $RealmName, $RealmType, $RealmLang)  "

                objCommand.Parameters.Add("$RealmID", DbType.Int32).Value = ID
                objCommand.Parameters.Add("$ServerID", DbType.Int32).Value = ComboBoxRealmServer.SelectedValue
                objCommand.Parameters.Add("$RealmName", DbType.String).Value = TextBoxRealmName.Text
                objCommand.Parameters.Add("$RealmType", DbType.String).Value = ComboBoxRealmType.Text
                objCommand.Parameters.Add("$RealmLang", DbType.String).Value = ComboBoxRealmLang.Text

                objCommand.ExecuteNonQuery()

                LastRealm = ID
                MainForm_Reload()

            End Using

            LogIt("Realm " & TextBoxRealmName.Text & " Added", Color.DarkGreen)

        Catch ex As Exception
            LogIt("Realm creation failed ! ", Color.Red)
            MessageBox.Show("Error: " & ex.Message)

        End Try
    End Sub

    Private Sub ButtonRealmSave_Click(sender As Object, e As EventArgs) Handles ButtonRealmSave.Click
        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim ID As Long

        Try
            ID = CInt(LabelRealmID.Text)

            If ID = 0 Then Exit Sub

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "UPDATE realm SET ServerID=$ServerID, RealmName=$RealmName, RealmType=$RealmType, RealmLang=$RealmLang WHERE RealmID=$RealmID;"

                objCommand.Parameters.Add("$RealmID", DbType.Int32).Value = ID
                objCommand.Parameters.Add("$ServerID", DbType.Int32).Value = ComboBoxRealmServer.SelectedValue
                objCommand.Parameters.Add("$RealmName", DbType.String).Value = TextBoxRealmName.Text
                objCommand.Parameters.Add("$RealmType", DbType.String).Value = ComboBoxRealmType.Text
                objCommand.Parameters.Add("$RealmLang", DbType.String).Value = ComboBoxRealmLang.Text

                objCommand.ExecuteNonQuery()

                LastRealm = ID
                MainForm_Reload()


            End Using

            LogIt("Realm " & TextBoxRealmName.Text & " modified", Color.DarkGreen)

        Catch ex As Exception
            LogIt("Modification failed ! ", Color.Red)
            MessageBox.Show("Error: " & ex.Message)

        End Try
    End Sub

    Private Sub ButtonRealmDel_Click(sender As Object, e As EventArgs) Handles ButtonRealmDel.Click
        Dim ID As Long

        Try
            ID = CInt(LabelRealmID.Text)
            If ID = 0 Then Exit Sub

            DB_Delete_Realm(ID)

            MainForm_Reload()

            LogIt("Realm deleted", Color.DarkGreen)
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try
    End Sub


#End Region


    ' #####################################################################################################################"
#Region "Server_Mgt"

    Private Sub DataGridViewServers_Load()

        Dim conn = New SQLiteConnection(CONSTRING)

        Try
            Using (conn)
                conn.Open()

                Dim sql As String = "SELECT S.*, V.WoWversionName FROM server S INNER JOIN wowversion V ON S.WoWversionID=V.WoWversionID ORDER BY WoWversionID, ServerID"

                Dim cmdDataGrid As SQLiteCommand = New SQLiteCommand(sql, conn)

                Dim da As New SQLiteDataAdapter
                da.SelectCommand = cmdDataGrid
                Dim dt As New DataTable
                da.Fill(dt)

                With DataGridViewServers

                    .DataSource = Nothing

                    .AutoGenerateColumns = False
                    .ReadOnly = True
                    .AllowUserToAddRows = False
                    .ColumnCount = 5

                    .Columns(0).Name = "VersionID"
                    .Columns(0).DataPropertyName = "WoWversionID"
                    .Columns(0).Visible = False

                    .Columns(1).Name = "Version"
                    .Columns(1).DataPropertyName = "WoWversionName"

                    .Columns(2).Name = "ServerID"
                    .Columns(2).DataPropertyName = "ServerID"
                    .Columns(2).Visible = False

                    .Columns(3).Name = "Server"
                    .Columns(3).DataPropertyName = "ServerName"

                    .Columns(4).Name = "RealmList"
                    .Columns(4).DataPropertyName = "ServerRealmList"

                    .DataSource = dt
                    Dim readerDataGrid As SQLiteDataReader = cmdDataGrid.ExecuteReader()


                    For Each row As DataGridViewRow In .Rows
                        If row.Cells("ServerID").Value = LastServer Then
                            .CurrentCell = row.Cells(3)
                            Exit For
                        End If
                    Next

                End With


            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try

    End Sub

    Private Sub DataGridViewServers_RowEnter(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewServers.RowEnter

        Try
            LabelServerID.Text = DataGridViewServers.Item(2, e.RowIndex).Value
            ComboBoxServerWoWversion.SelectedValue = DataGridViewServers.Item(0, e.RowIndex).Value
            TextBoxServerName.Text = DataGridViewServers.Item(3, e.RowIndex).Value
            TextBoxServerRealmList.Text = DataGridViewServers.Item(4, e.RowIndex).Value

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub


    Private Sub ComboBoxServerWoWversion_Load()

        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader
        Dim dt As New DataTable

        Try

            dt.Columns.Add("ID")
            dt.Columns.Add("Name")
            'dt.Rows.Add("", "")

            objConn = New SQLiteConnection(CONSTRING)

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "SELECT WoWversionID, WoWversionName FROM wowversion WHERE WoWversionActive=1 ORDER BY WoWversionID"
                objReader = objCommand.ExecuteReader()
                While (objReader.Read())
                    dt.Rows.Add(objReader("WoWversionID"), objReader("WoWversionName"))
                End While

                ComboBoxServerWoWversion.DataSource = dt

                ComboBoxServerWoWversion.ValueMember = "ID"
                ComboBoxServerWoWversion.DisplayMember = "Name"
                ComboBoxServerWoWversion.DropDownStyle = ComboBoxStyle.DropDown
            End Using

        Catch ex As Exception

        End Try
    End Sub


    Private Sub ButtonServerAdd_Click(sender As Object, e As EventArgs) Handles ButtonServerAdd.Click
        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader
        Dim ID As Long


        Try


            Controls_Color_Init()

            If ComboBoxServerWoWversion.SelectedValue = "" Or ComboBoxServerWoWversion.SelectedValue = 0 Then
                ComboBoxServerWoWversion.BackColor = Color.IndianRed
                Exit Sub
            End If

            If TextBoxServerName.Text = "" Then
                TextBoxServerRealmList.BackColor = Color.IndianRed
                Exit Sub
            End If

            If TextBoxServerRealmList.Text = "" Then
                TextBoxServerRealmList.BackColor = Color.IndianRed
                Exit Sub
            End If

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "SELECT MAX(ServerID) as ID FROM server"
                objReader = objCommand.ExecuteReader()
                While (objReader.Read())
                    If IsDBNull(objReader("ID")) Then
                        ID = 1
                    Else
                        ID = CLng(objReader("ID")) + 1
                    End If

                End While
                objReader.Close()


                objCommand.CommandText = "INSERT INTO server (ServerID, WoWversionID, ServerName, ServerRealmList) VALUES " &
                                         " ($ServerID, $WoWversionID, $ServerName, $ServerRealmList) "

                objCommand.Parameters.Add("$ServerID", DbType.Int32).Value = ID
                objCommand.Parameters.Add("$WoWversionID", DbType.Int32).Value = ComboBoxServerWoWversion.SelectedValue
                objCommand.Parameters.Add("$ServerName", DbType.String).Value = TextBoxServerName.Text
                objCommand.Parameters.Add("$ServerRealmList", DbType.String).Value = TextBoxServerRealmList.Text


                objCommand.ExecuteNonQuery()

                LastServer = ID

                MainForm_Reload()

            End Using

            LogIt("Server " & TextBoxServerName.Text & " Added", Color.DarkGreen)

        Catch ex As Exception
            LogIt("Server creation failed ! ", Color.Red)
            MessageBox.Show("Error: " & ex.Message)

        End Try
    End Sub

    Private Sub ButtonServerSave_Click(sender As Object, e As EventArgs) Handles ButtonServerSave.Click
        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim ID As Long

        Try
            ID = CInt(LabelServerID.Text)

            If ID = 0 Then Exit Sub

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "UPDATE server SET WoWversionID=$WoWversionID, ServerName=$ServerName, ServerRealmList=$ServerRealmList WHERE ServerID=$ServerID;"

                objCommand.Parameters.Add("$ServerID", DbType.Int32).Value = ID
                objCommand.Parameters.Add("$WoWversionID", DbType.Int32).Value = ComboBoxServerWoWversion.SelectedValue
                objCommand.Parameters.Add("$ServerName", DbType.String).Value = TextBoxServerName.Text
                objCommand.Parameters.Add("$ServerRealmList", DbType.String).Value = TextBoxServerRealmList.Text

                objCommand.ExecuteNonQuery()

                LastServer = ID
                MainForm_Reload()
            End Using

            LogIt("Server " & TextBoxServerName.Text & " modified", Color.DarkGreen)

        Catch ex As Exception
            LogIt("Modification failed ! ", Color.Red)
            MessageBox.Show("Error: " & ex.Message)

        End Try
    End Sub

    Private Sub ButtonServerDel_Click(sender As Object, e As EventArgs) Handles ButtonServerDel.Click
        Dim ID As Long

        Try
            ID = CInt(LabelServerID.Text)
            If ID = 0 Then Exit Sub

            DB_Delete_Server(ID)

            MainForm_Reload()

            LogIt("Server deleted", Color.DarkGreen)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try
    End Sub

#End Region


    ' #####################################################################################################################"
#Region "Links_Mgt"

    Private Sub DataGridViewLinks_Load()

        Dim conn = New SQLiteConnection(CONSTRING)

        Try
            Using (conn)
                conn.Open()

                Dim sql As String = "SELECT * FROM weblink ORDER BY IsGuild, LinkOrder"

                Dim cmdDataGrid As SQLiteCommand = New SQLiteCommand(sql, conn)

                Dim da As New SQLiteDataAdapter
                da.SelectCommand = cmdDataGrid
                Dim dt As New DataTable
                da.Fill(dt)

                With DataGridViewLinks

                    .DataSource = Nothing

                    .AutoGenerateColumns = False
                    .ReadOnly = True
                    .AllowUserToAddRows = False
                    .ColumnCount = 5

                    .Columns(0).Name = "LinkID"
                    .Columns(0).DataPropertyName = "LinkID"
                    .Columns(0).Visible = False

                    .Columns(1).Name = "Order"
                    .Columns(1).DataPropertyName = "LinkOrder"

                    .Columns(2).Name = "Label"
                    .Columns(2).DataPropertyName = "LinkLabel"

                    .Columns(3).Name = "Guild ?"
                    .Columns(3).DataPropertyName = "IsGuild"
                    .Columns(3).Visible = True

                    .Columns(4).Name = "ImageIdx"
                    .Columns(4).DataPropertyName = "LinkImage"
                    .Columns(4).Visible = False


                    ' 5
                    Dim colurl As New DataGridViewLinkColumn()
                    colurl.DataPropertyName = "LinkURL"
                    colurl.Name = "URL"
                    .Columns.Add(colurl)

                    ' 6
                    Dim imgLink As New DataGridViewImageColumn()
                    imgLink.Image = ImageListLinks.Images(13)
                    imgLink.HeaderText = "Icon"
                    imgLink.Width = 8
                    imgLink.ImageLayout = DataGridViewImageCellLayout.Zoom
                    imgLink.Name = "imgLink"
                    .Columns.Add(imgLink)



                    .DataSource = dt
                    Dim readerDataGrid As SQLiteDataReader = cmdDataGrid.ExecuteReader()

                    For Each row As DataGridViewRow In .Rows
                        If row.Cells("LinkID").Value = LastLink Then
                            .CurrentCell = row.Cells(1)
                            Exit For
                        End If
                    Next

                End With


            End Using

        Catch ex As Exception
            MsgBox(ex.ToString())

        End Try

    End Sub


    Private Sub DataGridViewLinks_RowEnter(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewLinks.RowEnter
        Dim ImageIdx As Integer

        Try
            LabelLinkID.Text = DataGridViewLinks.Item(0, e.RowIndex).Value
            TextBoxLinkOrder.Text = DataGridViewLinks.Item(1, e.RowIndex).Value

            If IsDBNull(DataGridViewLinks.Item(2, e.RowIndex).Value) Then
                TextBoxLinkLabel.Text = ""
            Else
                TextBoxLinkLabel.Text = DataGridViewLinks.Item(2, e.RowIndex).Value
            End If

            If IsDBNull(DataGridViewLinks.Item(5, e.RowIndex).Value) Then
                TextBoxLinkURL.Text = ""
            Else
                TextBoxLinkURL.Text = DataGridViewLinks.Item(5, e.RowIndex).Value
            End If


            If IsDBNull(DataGridViewLinks.Item(3, e.RowIndex).Value) Then
                CheckBoxLinksIsGuild.Checked = False
            Else
                CheckBoxLinksIsGuild.Checked = CBool(DataGridViewLinks.Item(3, e.RowIndex).Value)
            End If

            If IsDBNull(DataGridViewLinks.Item(4, e.RowIndex).Value) Or Not IsNumeric(DataGridViewLinks.Item(4, e.RowIndex).Value) Then
                ImageIdx = 14
            Else
                ImageIdx = CInt(DataGridViewLinks.Item(4, e.RowIndex).Value)
            End If

            ListViewLinksIcons.Items(ImageIdx).Focused = True
            ListViewLinksIcons.Items(ImageIdx).Selected = True
            ListViewLinksIcons.EnsureVisible(ImageIdx)


        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub


    Private Sub DataGridViewLinks_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridViewLinks.CellFormatting

        Try

            If e.RowIndex < 0 Then Exit Sub

            Select Case e.ColumnIndex

                Case 6 ' Icon image (image index in 4)

                    If IsDBNull(DataGridViewLinks.Rows(e.RowIndex).Cells(4).Value) Or Not IsNumeric(DataGridViewLinks.Rows(e.RowIndex).Cells(4).Value) Then
                        e.Value = ImageListLinks.Images(13)
                    Else
                        e.Value = ImageListLinks.Images(CInt(DataGridViewLinks.Rows(e.RowIndex).Cells(4).Value))
                    End If


            End Select

        Catch ex As Exception

        End Try
    End Sub



    Private Sub ButtonLinkAdd_Click(sender As Object, e As EventArgs) Handles ButtonLinkAdd.Click

        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader
        Dim ID As Long

        Dim ImageIdx As Integer


        Try

            Controls_Color_Init()


            If TextBoxLinkLabel.Text = "" Then
                TextBoxLinkLabel.BackColor = Color.IndianRed
                Exit Sub
            End If

            If TextBoxLinkURL.Text = "" Then
                TextBoxLinkURL.BackColor = Color.IndianRed
                Exit Sub
            End If

            If Not IsNumeric((TextBoxLinkOrder.Text)) Then
                TextBoxLinkOrder.Text = 1
            End If


            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "SELECT MAX(LinkID) as ID FROM weblink"
                objReader = objCommand.ExecuteReader()
                While (objReader.Read())
                    If IsDBNull(objReader("ID")) Then
                        ID = 1
                    Else
                        ID = CLng(objReader("ID")) + 1
                    End If

                End While
                objReader.Close()


                If ListViewLinksIcons.SelectedIndices(0) > 0 Then
                    ImageIdx = ListViewLinksIcons.SelectedIndices(0)
                Else
                    ImageIdx = 14
                End If

                objCommand.CommandText = "INSERT INTO weblink (LinkID, LinkLabel, LinkURL, LinkOrder, IsGuild, LinkImage) VALUES " &
                                         " ($LinkID, $LinkLabel, $LinkURL, $LinkOrder,$IsGuild, $LinkImage) "

                objCommand.Parameters.Add("$LinkID", DbType.Int32).Value = ID
                objCommand.Parameters.Add("$LinkLabel", DbType.String).Value = TextBoxLinkLabel.Text
                objCommand.Parameters.Add("$LinkURL", DbType.String).Value = TextBoxLinkURL.Text
                objCommand.Parameters.Add("$LinkOrder", DbType.Int32).Value = TextBoxLinkOrder.Text
                objCommand.Parameters.Add("$IsGuild", DbType.Boolean).Value = CheckBoxLinksIsGuild.Checked
                objCommand.Parameters.Add("$LinkImage", DbType.Int32).Value = ImageIdx

                objCommand.ExecuteNonQuery()

                LastLink = ID
                DataGridViewLinks_Load()
                ListViewOverviewGuild_Load()
                ListViewOverviewLinks_Load()

            End Using

            LogIt("Link " & TextBoxLinkLabel.Text & " Added", Color.DarkGreen)

        Catch ex As Exception
            LogIt("Link creation failed ! ", Color.Red)
            MessageBox.Show("Error: " & ex.Message)

        End Try

    End Sub

    Private Sub ButtonLinkSave_Click(sender As Object, e As EventArgs) Handles ButtonLinkSave.Click
        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim ID As Long
        Dim ImageIdx As Integer

        Try
            ID = CInt(LabelLinkID.Text)

            If ID = 0 Then Exit Sub

            If Not IsNumeric((TextBoxLinkOrder.Text)) Then
                TextBoxLinkOrder.Text = 1
            End If


            If ListViewLinksIcons.SelectedIndices(0) > 0 Then
                ImageIdx = ListViewLinksIcons.SelectedIndices(0)
            Else
                ImageIdx = 14
            End If

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "UPDATE weblink SET LinkLabel=$LinkLabel, LinkURL=$LinkURL, LinkOrder=$LinkOrder, IsGuild=$IsGuild, LinkImage=$LinkImage  WHERE LinkID=$LinkID;"

                objCommand.Parameters.Add("$LinkID", DbType.Int32).Value = ID
                objCommand.Parameters.Add("$LinkLabel", DbType.String).Value = TextBoxLinkLabel.Text
                objCommand.Parameters.Add("$LinkURL", DbType.String).Value = TextBoxLinkURL.Text
                objCommand.Parameters.Add("$LinkOrder", DbType.Int32).Value = TextBoxLinkOrder.Text
                objCommand.Parameters.Add("$IsGuild", DbType.Boolean).Value = CheckBoxLinksIsGuild.Checked
                objCommand.Parameters.Add("$LinkImage", DbType.Int32).Value = ImageIdx

                objCommand.ExecuteNonQuery()

                LastLink = ID
                DataGridViewLinks_Load()
                ListViewOverviewGuild_Load()
                ListViewOverviewLinks_Load()
            End Using

            LogIt("Link " & TextBoxLinkLabel.Text & " modified", Color.DarkGreen)

        Catch ex As Exception
            LogIt("Modification failed ! ", Color.Red)
            MessageBox.Show("Error: " & ex.Message)

        End Try
    End Sub

    Private Sub ButtonLinkDel_Click(sender As Object, e As EventArgs) Handles ButtonLinkDel.Click
        Dim ID As Long

        Try
            ID = CInt(LabelLinkID.Text)
            If ID = 0 Then Exit Sub

            DB_Delete_Link(ID)

            DataGridViewLinks_Load()
            ListViewOverviewGuild_Load()
            ListViewOverviewLinks_Load()

            LogIt("Link deleted", Color.DarkGreen)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try
    End Sub

    Private Sub Load_Links_Icons()

        Try
            Dim i As Integer

            For i = 0 To ImageListLinks.Images.Count

                ListViewLinksIcons.Items.Add("", i)

            Next


        Catch ex As Exception

        End Try

    End Sub

#End Region


    ' #####################################################################################################################"
#Region "Overview_Mgt"

    Private Sub TreeViewLauncher_Load()

        Dim conn = New SQLiteConnection(CONSTRING)
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader

        Dim NodeVersion As TreeNode
        Dim NodeServer As TreeNode
        Dim NodeRealm As TreeNode
        Dim NodeAccount As TreeNode
        Dim NodeChar As TreeNode

        Dim oldVersion As Integer
        Dim oldServer As Integer
        Dim oldRealm As Integer
        Dim oldAccount As Integer

        Dim cntServers As Integer
        Dim cntRealms As Integer
        Dim cntAccounts As Integer
        Dim cntChars As Integer

        Dim sql As String
        Dim where As String

        Try


            Using (conn)
                conn.Open()

                If CheckBoxFavsOnly.Checked Then
                    where = " WHERE a.AccountIsFav=1 or c.CharIsFav=1 "
                End If


                sql = " SELECT DISTINCT a.AccountID, a.AccountName, a.AccountIsFav,   " &
                  "        ra.RaceName, ra.RaceFaction, cl.ClassName, cl.ClassColor,  " &
                  "        c.CharID, c.CharName, C.CharIndex, C.CharGender, C.CharFaction,  " &
                  "         S.ServerID, S.ServerName, S.ServerRealmList,   " &
                  "        R.RealmName, R.RealmID,  " &
                  "        v.wowversionID, v.wowversionName     " &
                  " FROM account a " &
                  " LEFT JOIN character c ON a.AccountID=c.AccountID " &
                  " LEFT JOIN class cl ON c.CLassID=cl.ClassID " &
                  " LEFT JOIN race ra ON c.RaceID=ra.RaceID " &
                  " INNER JOIN realm r ON A.RealmID=r.RealmID " &
                  " INNER JOIN server s on S.ServerID=r.ServerID " &
                  " INNER JOIN wowversion v on S.wowversionID=v.wowversionID " &
                  where &
                  " ORDER BY v.wowversionID, s.ServerName, r.RealmName, a.AccountName, c.CharName"


                objCommand = conn.CreateCommand()
                objCommand.CommandText = sql
                objReader = objCommand.ExecuteReader()


                TreeViewLauncher.Nodes.Clear()

                TreeViewLauncher.ImageList = ImageListTV

                While (objReader.Read())



                    If oldVersion <> CInt(objReader("wowversionID")) And ComboView.Text = "FULL" Then

                        NodeVersion = New TreeNode()
                        NodeVersion.Tag = CInt(objReader("wowversionID"))
                        NodeVersion.Name = "NodeVersion_" & CInt(objReader("wowversionID"))
                        NodeVersion.Text = objReader("wowversionname")
                        NodeVersion.SelectedImageKey = "V" & CInt(objReader("wowversionID"))
                        NodeVersion.ImageKey = "V" & CInt(objReader("wowversionID"))

                        oldVersion = CInt(objReader("wowversionID"))

                        TreeViewLauncher.Nodes.Add(NodeVersion)
                    End If



                    If oldServer <> CInt(objReader("ServerID")) Then

                        NodeServer = New TreeNode()
                        NodeServer.Tag = CInt(objReader("ServerID"))
                        NodeServer.Name = "NodeServer_" & CInt(objReader("ServerID"))
                        NodeServer.Text = objReader("ServerName")
                        NodeServer.ImageKey = "server"
                        NodeServer.SelectedImageKey = "server"

                        oldServer = CInt(objReader("ServerID"))
                        cntServers = cntServers + 1

                        If ComboView.Text = "FULL" Then
                            NodeVersion.Nodes.Add(NodeServer)
                        Else
                            If ComboView.Text = "Server" Then TreeViewLauncher.Nodes.Add(NodeServer)
                        End If

                    End If



                    If oldRealm <> CInt(objReader("RealmID")) Then
                        NodeRealm = New TreeNode()
                        NodeRealm.Tag = CInt(objReader("RealmID"))
                        NodeRealm.Name = "NodeRealm_" & CInt(objReader("RealmID"))
                        NodeRealm.Text = objReader("RealmName")
                        NodeRealm.ImageKey = "realm"
                        NodeRealm.SelectedImageKey = "realm"

                        oldRealm = CInt(objReader("RealmID"))
                        cntRealms = cntRealms + 1

                        If ComboView.Text = "FULL" Or ComboView.Text = "Server" Then
                            NodeServer.Nodes.Add(NodeRealm)
                        Else
                            If ComboView.Text = "Realm" Then TreeViewLauncher.Nodes.Add(NodeRealm)
                        End If

                    End If



                    If oldAccount <> CInt(objReader("AccountID")) Then
                        NodeAccount = New TreeNode()
                        NodeAccount.Tag = CInt(objReader("AccountID"))
                        NodeAccount.Name = "NodeAccount_" & CInt(objReader("AccountID"))

                        If ComboView.Text = "Account" Then
                            NodeAccount.Text = objReader("AccountName") & " (" & objReader("RealmName") & ")"
                        Else
                            NodeAccount.Text = objReader("AccountName")
                        End If


                        NodeAccount.ImageKey = "account"
                        NodeAccount.SelectedImageKey = "account"

                        oldAccount = CInt(objReader("AccountID"))
                        cntAccounts = cntAccounts + 1

                        If ComboView.Text = "FULL" Or ComboView.Text = "Server" Or ComboView.Text = "Realm" Then
                            NodeRealm.Nodes.Add(NodeAccount)
                        Else
                            If ComboView.Text = "Account" Then TreeViewLauncher.Nodes.Add(NodeAccount)
                        End If
                    End If

                    If Not IsDBNull(objReader("CharID")) Then

                        NodeChar = New TreeNode()
                        NodeChar.Tag = CInt(objReader("CharID"))
                        NodeChar.Name = "NodeChar_" & CInt(objReader("CharID"))


                        If ComboView.Text = "Character" Then
                            NodeChar.Text = objReader("CharName") & " (" & objReader("RealmName") & ")"
                        Else
                            NodeChar.Text = objReader("CharName")
                        End If

                        NodeChar.ImageKey = "F" & (objReader("CharFaction"))
                        NodeChar.SelectedImageKey = "F" & (objReader("CharFaction"))
                        ' NodeChar.ImageKey = "IconSmall_" & objReader("RaceName") & "_" & IIf(objReader("CharGender") = "M", "Male", "Female") & ".gif"
                        ' NodeChar.SelectedImageKey = "IconSmall_" & objReader("RaceName") & "_" & IIf(objReader("CharGender") = "M", "Male", "Female") & ".gif"
                        NodeChar.ForeColor = ColorTranslator.FromHtml("#" & (objReader("ClassColor")))

                        cntChars = cntChars + 1

                        If ComboView.Text = "FULL" Or ComboView.Text = "Server" Or ComboView.Text = "Realm" Or ComboView.Text = "Account" Then
                            NodeAccount.Nodes.Add(NodeChar)
                        Else
                            TreeViewLauncher.Nodes.Add(NodeChar)
                        End If

                    End If

                End While

                If CheckExpand.Checked Then TreeViewLauncher.ExpandAll()
                'LinkLabelExpandTV.Visible = True

                TSSchars.Text = cntChars
                TSSservers.Text = cntServers
                TSSrealms.Text = cntRealms
                TSSaccounts.Text = cntAccounts

            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        Finally

        End Try

    End Sub


    Public Sub Overview_Init()

        Try

            PictureBoxOverviewVersion.Visible = False
            LabelOverviewServerName.Text = ""
            LabelOverviewServerAddress.Text = ""
            LabelOverviewServerPing.Text = ""
            LabelOverviewServerMS.Text = ""
            LabelOverviewServerPing.ForeColor = Color.DarkGray
            LabelOverviewServerAddress.ForeColor = Color.DarkGray


            LabelOverviewRealmName.Text = ""
            LabelOverviewRealmType.Text = ""
            LabelOverviewRealmLang.Text = ""

            LabelOverviewAccountID.Text = ""
            LabelOverviewAccountName.Text = ""
            LabelOverviewAccountNote.Text = ""
            PictureBoxOverviewAccountFullscreen.Visible = False
            PictureBoxOverviewAccountScreen1.Visible = False
            PictureBoxOverviewAccountScreen1A.Visible = False
            PictureBoxOverviewAccountScreen1B.Visible = False
            PictureBoxOverviewAccountScreen1C.Visible = False
            PictureBoxOverviewAccountScreen1D.Visible = False
            PictureBoxOverviewAccountScreen2.Visible = False
            PictureBoxOverviewAccountScreen2A.Visible = False
            PictureBoxOverviewAccountScreen2B.Visible = False
            PictureBoxOverviewAccountScreen2C.Visible = False
            PictureBoxOverviewAccountScreen2D.Visible = False
            LabelOverviewAccountFullscreen.Visible = False
            LabelOverviewAccountScreen1.Visible = False
            LabelOverviewAccountScreen2.Visible = False


            LabelOverviewCharID.Text = ""
            LabelOverviewCharIndex.Text = ""
            LabelOverviewCharName.Text = ""
            LabelOverviewCharNote.Text = ""
            PictureBoxOverviewCharClass.Visible = False
            PictureBoxOverviewCharRace.Visible = False
            PictureBoxOverviewCharFaction.Visible = False
            PictureBoxOverviewCharFullscreen.Visible = False
            PictureBoxOverviewCharScreen1.Visible = False
            PictureBoxOverviewCharScreen1A.Visible = False
            PictureBoxOverviewCharScreen1B.Visible = False
            PictureBoxOverviewCharScreen1C.Visible = False
            PictureBoxOverviewCharScreen1D.Visible = False
            PictureBoxOverviewCharScreen2.Visible = False
            PictureBoxOverviewCharScreen2A.Visible = False
            PictureBoxOverviewCharScreen2B.Visible = False
            PictureBoxOverviewCharScreen2C.Visible = False
            PictureBoxOverviewCharScreen2D.Visible = False
            LabelOverviewCharFullscreen.Visible = False
            LabelOverviewCharScreen1.Visible = False
            LabelOverviewCharScreen2.Visible = False


            LabelOverviewProcessID.Text = ""
            LabelOverviewProcessText.Text = ""
            LabelOverviewProcessStarted.Text = ""
            LabelOverviewProcessKill.Visible = False
            LabelOverviewProcessSelect.Visible = False
            PictureBoxOverviewProcessKill.Visible = False
            PictureBoxOverviewProcessSelect.Visible = False

            LabelOverviewSessionsCount.Text = ""


        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try


    End Sub

    Private Sub Overview_Version(ID As Integer)


        Try

            If ID = 0 Then Exit Sub

            Select Case ID
                Case 2
                    PictureBoxOverviewVersion.Image = My.Resources.V2_large
                    PictureBoxOverviewVersion.Tag = "V2"
                Case 3
                    PictureBoxOverviewVersion.Image = My.Resources.V3_large
                    PictureBoxOverviewVersion.Tag = "V3"
                Case Else
                    PictureBoxOverviewVersion.Image = My.Resources.V1_large
                    PictureBoxOverviewVersion.Tag = "V1"
            End Select

            PictureBoxOverviewVersion.Visible = True

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub


    Private Sub Overview_Server(ID As Integer)

        Dim conn = New SQLiteConnection(CONSTRING)
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader
        Dim sql As String

        Try

            If ID = 0 Then Exit Sub

            Using (conn)
                conn.Open()

                sql = " SELECT *  FROM server WHERE ServerID=" & ID

                objCommand = conn.CreateCommand()
                objCommand.CommandText = sql
                objReader = objCommand.ExecuteReader()

                While (objReader.Read())

                    Select Case (objReader("WoWversionID"))
                        Case 2
                            PictureBoxOverviewVersion.Image = My.Resources.V2_large
                            PictureBoxOverviewVersion.Tag = "V2"
                        Case 3
                            PictureBoxOverviewVersion.Image = My.Resources.V3_large
                            PictureBoxOverviewVersion.Tag = "V3"
                        Case Else
                            PictureBoxOverviewVersion.Image = My.Resources.V1_large
                            PictureBoxOverviewVersion.Tag = "V1"
                    End Select

                    PictureBoxOverviewVersion.Visible = True

                    LabelOverviewServerName.Text = objReader("ServerName")
                    LabelOverviewServerAddress.Text = objReader("ServerRealmList")

                End While

            End Using

            Ping_Server()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub Overview_Realm(ID As Integer)

        Dim conn = New SQLiteConnection(CONSTRING)
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader
        Dim sql As String

        Try

            If ID = 0 Then Exit Sub

            Using (conn)
                conn.Open()

                sql = " SELECT s.ServerName, s.ServerRealmList, s.WoWversionID,  " &
                      "        r.RealmName, r.RealmType, r.RealmLang  " &
                      " FROM realm r " &
                      "  INNER JOIN server s ON s.ServerID=r.ServerID " &
                      " WHERE r.RealmID=" & ID

                objCommand = conn.CreateCommand()
                objCommand.CommandText = sql
                objReader = objCommand.ExecuteReader()

                While (objReader.Read())

                    ' Server 
                    Select Case (objReader("WoWversionID"))
                        Case 2
                            PictureBoxOverviewVersion.Image = My.Resources.V2_large
                            PictureBoxOverviewVersion.Tag = "V2"
                        Case 3
                            PictureBoxOverviewVersion.Image = My.Resources.V3_large
                            PictureBoxOverviewVersion.Tag = "V3"
                        Case Else
                            PictureBoxOverviewVersion.Image = My.Resources.V1_large
                            PictureBoxOverviewVersion.Tag = "V1"
                    End Select

                    PictureBoxOverviewVersion.Visible = True

                    LabelOverviewServerName.Text = objReader("ServerName")
                    LabelOverviewServerAddress.Text = objReader("ServerRealmList")

                    ' Realm
                    LabelOverviewRealmName.Text = objReader("RealmName")
                    LabelOverviewRealmType.Text = objReader("RealmType")
                    LabelOverviewRealmLang.Text = objReader("RealmLang")

                End While


            End Using

            Ping_Server()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub


    Public Sub Overview_Account(ID As Integer)

        Dim conn = New SQLiteConnection(CONSTRING)
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader
        Dim sql As String
        Dim i As Integer

        Try

            If ID = 0 Then Exit Sub

            Using (conn)
                conn.Open()

                sql = " SELECT DISTINCT a.AccountID, a.AccountName,   " &
                  "        ra.RaceName, ra.RaceFaction, cl.ClassName, cl.ClassColor,  " &
                  "        c.CharID, c.CharName, C.CharIndex, C.CharGender, C.CharFaction,  " &
                  "         S.ServerID, S.ServerName, S.ServerRealmList,   " &
                  "        R.RealmName, R.RealmID,  " &
                  "        v.wowversionID, v.wowversionName     " &
                  " FROM account a " &
                  " LEFT JOIN character c ON a.AccountID=c.AccountID " &
                  " LEFT JOIN class cl ON c.CLassID=cl.ClassID " &
                  " LEFT JOIN race ra ON c.RaceID=ra.RaceID " &
                  " INNER JOIN realm r ON A.RealmID=r.RealmID " &
                  " INNER JOIN server s on S.ServerID=r.ServerID " &
                  " INNER JOIN wowversion v on S.wowversionID=v.wowversionID " &
                  " ORDER BY v.wowversionID, s.ServerName, r.RealmName, a.AccountName, c.CharName"


                sql = " SELECT s.ServerName, s.ServerRealmList, s.WoWversionID,  " &
                      "        r.RealmName, r.RealmType, r.RealmLang,  " &
                      "       a.AccountID, a.AccountName, a.AccountNote " &
                      " FROM account a " &
                      "  INNER JOIN realm r ON r.RealmID=a.RealmID " &
                      "  INNER JOIN server s ON s.ServerID=r.ServerID " &
                      " WHERE a.AccountID=" & ID


                objCommand = conn.CreateCommand()
                objCommand.CommandText = sql
                objReader = objCommand.ExecuteReader()

                While (objReader.Read())

                    ' Server 
                    Select Case (objReader("WoWversionID"))
                        Case 2
                            PictureBoxOverviewVersion.Image = My.Resources.V2_large
                            PictureBoxOverviewVersion.Tag = "V2"
                        Case 3
                            PictureBoxOverviewVersion.Image = My.Resources.V3_large
                            PictureBoxOverviewVersion.Tag = "V3"
                        Case Else
                            PictureBoxOverviewVersion.Image = My.Resources.V1_large
                            PictureBoxOverviewVersion.Tag = "V1"
                    End Select


                    PictureBoxOverviewVersion.Visible = True

                    LabelOverviewServerName.Text = objReader("ServerName")
                    LabelOverviewServerAddress.Text = objReader("ServerRealmList")

                    ' Realm
                    LabelOverviewRealmName.Text = objReader("RealmName")
                    LabelOverviewRealmType.Text = objReader("RealmType")
                    LabelOverviewRealmLang.Text = objReader("RealmLang")

                    ' Account

                    LabelOverviewAccountID.Text = objReader("AccountID")
                    LabelOverviewAccountName.Text = objReader("AccountName")
                    LabelOverviewAccountNote.Text = objReader("AccountNote")

                    PictureBoxOverviewAccountFullscreen.Visible = True
                    LabelOverviewAccountFullscreen.Visible = True

                    PictureBoxOverviewAccountScreen1.Visible = True
                    PictureBoxOverviewAccountScreen1A.Visible = True
                    PictureBoxOverviewAccountScreen1B.Visible = True
                    PictureBoxOverviewAccountScreen1C.Visible = True
                    PictureBoxOverviewAccountScreen1D.Visible = True
                    LabelOverviewAccountScreen1.Visible = True

                    If NBR_MONITORS > 1 Then
                        PictureBoxOverviewAccountScreen2.Visible = True
                        PictureBoxOverviewAccountScreen2A.Visible = True
                        PictureBoxOverviewAccountScreen2B.Visible = True
                        PictureBoxOverviewAccountScreen2C.Visible = True
                        PictureBoxOverviewAccountScreen2D.Visible = True
                        LabelOverviewAccountScreen2.Visible = True
                    End If

                End While


            End Using

            Check_Processes()

            Ping_Server()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub


    Public Sub Overview_Char(ID As Integer)

        Dim conn = New SQLiteConnection(CONSTRING)
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader
        Dim sql As String

        Try

            If ID = 0 Then Exit Sub

            Using (conn)
                conn.Open()


                sql = " SELECT s.ServerName, s.ServerRealmList, s.WoWversionID,  " &
                      "        r.RealmName, r.RealmType, r.RealmLang,  " &
                      "        a.AccountID, a.AccountName, a.AccountNote, " &
                      "        ra.RaceName, ra.RaceFaction, cl.ClassName, cl.ClassColor,  " &
                      "        c.CharID, c.CharName, c.CharIndex,  c.CharNote, C.CharLevel, C.CharGender, C.CharFaction  " &
                      " FROM character c " &
                      " INNER JOIN account a ON a.AccountID=c.AccountID " &
                      " LEFT JOIN class cl ON c.CLassID=cl.ClassID " &
                      " LEFT JOIN race ra ON c.RaceID=ra.RaceID " &
                      "  INNER JOIN realm r ON r.RealmID=a.RealmID " &
                      "  INNER JOIN server s ON s.ServerID=r.ServerID " &
                      " WHERE c.CharID=" & ID


                objCommand = conn.CreateCommand()
                objCommand.CommandText = sql
                objReader = objCommand.ExecuteReader()

                While (objReader.Read())

                    ' Server 
                    Select Case (objReader("WoWversionID"))
                        Case 2
                            PictureBoxOverviewVersion.Image = My.Resources.V2_large
                        Case 3
                            PictureBoxOverviewVersion.Image = My.Resources.V3_large
                        Case Else
                            PictureBoxOverviewVersion.Image = My.Resources.V1_large
                    End Select

                    PictureBoxOverviewVersion.Visible = True

                    LabelOverviewServerName.Text = objReader("ServerName")
                    LabelOverviewServerAddress.Text = objReader("ServerRealmList")

                    ' Realm
                    LabelOverviewRealmName.Text = objReader("RealmName")
                    LabelOverviewRealmType.Text = objReader("RealmType")
                    LabelOverviewRealmLang.Text = objReader("RealmLang")

                    ' Account

                    LabelOverviewAccountID.Text = objReader("AccountID")
                    LabelOverviewAccountName.Text = objReader("AccountName")
                    LabelOverviewAccountNote.Text = objReader("AccountNote")

                    PictureBoxOverviewAccountFullscreen.Visible = True
                    LabelOverviewAccountFullscreen.Visible = True

                    PictureBoxOverviewAccountScreen1.Visible = True
                    PictureBoxOverviewAccountScreen1A.Visible = True
                    PictureBoxOverviewAccountScreen1B.Visible = True
                    PictureBoxOverviewAccountScreen1C.Visible = True
                    PictureBoxOverviewAccountScreen1D.Visible = True
                    LabelOverviewAccountScreen1.Visible = True

                    If NBR_MONITORS > 1 Then
                        PictureBoxOverviewAccountScreen2.Visible = True
                        PictureBoxOverviewAccountScreen2A.Visible = True
                        PictureBoxOverviewAccountScreen2B.Visible = True
                        PictureBoxOverviewAccountScreen2C.Visible = True
                        PictureBoxOverviewAccountScreen2D.Visible = True
                        LabelOverviewAccountScreen2.Visible = True
                        LabelOverviewAccountScreen1.Text = "Windowed Screen 1"
                    Else
                        LabelOverviewAccountScreen1.Text = "Windowed"
                    End If


                    ' Character
                    LabelOverviewCharID.Text = objReader("CharID")
                    LabelOverviewCharName.Text = objReader("CharName")
                    LabelOverviewCharNote.Text = objReader("CharNote")
                    LabelOverviewCharIndex.Text = objReader("CharIndex")

                    If Not IsDBNull(objReader("RaceFaction")) Then
                        PictureBoxOverviewCharFaction.Visible = True
                        If objReader("RaceFaction") = "H" Then
                            PictureBoxOverviewCharFaction.Image = My.Resources.Horde_Banner
                        Else
                            PictureBoxOverviewCharFaction.Image = My.Resources.Alliance_Banner
                        End If

                    End If

                    If Not IsDBNull(objReader("RaceName")) And Not IsDBNull(objReader("CharGender")) Then
                        PictureBoxOverviewCharRace.Visible = True
                        PictureBoxOverviewCharRace.Image = Image_Race_Big(objReader("RaceName"), objReader("CharGender"))
                    End If

                    If Not IsDBNull(objReader("ClassName")) Then
                        PictureBoxOverviewCharClass.Visible = True
                        PictureBoxOverviewCharClass.Image = Image_Class(objReader("ClassName"))
                        LabelOverviewCharName.ForeColor = ColorTranslator.FromHtml("#" & (objReader("ClassColor")))
                    Else
                        LabelOverviewCharName.ForeColor = DefaultForeColor
                    End If

                    PictureBoxOverviewCharFullscreen.Visible = True
                    LabelOverviewCharFullscreen.Visible = True

                    PictureBoxOverviewCharScreen1.Visible = True
                    PictureBoxOverviewCharScreen1A.Visible = True
                    PictureBoxOverviewCharScreen1B.Visible = True
                    PictureBoxOverviewCharScreen1C.Visible = True
                    PictureBoxOverviewCharScreen1D.Visible = True
                    LabelOverviewCharScreen1.Visible = True

                    If NBR_MONITORS > 1 Then
                        PictureBoxOverviewCharScreen2.Visible = True
                        PictureBoxOverviewCharScreen2A.Visible = True
                        PictureBoxOverviewCharScreen2B.Visible = True
                        PictureBoxOverviewCharScreen2C.Visible = True
                        PictureBoxOverviewCharScreen2D.Visible = True
                        LabelOverviewCharScreen2.Visible = True
                        LabelOverviewCharScreen1.Text = "Windowed Screen 1"
                    Else
                        LabelOverviewCharScreen1.Text = "Windowed"
                    End If


                End While


            End Using

            Check_Processes()

            Ping_Server()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub





    Private Function ASCII_Pbar(Val As Long, Tot As Long, Optional Unit As String = "") As String
        Try
            Dim intRatio As Integer
            Dim i As Integer
            Dim strBar As String = ""
            Dim chrFilled As Char = "█"

            Dim chrEmpty As Char = "▐"

            If Val > 0 Then
                intRatio = (Math.Ceiling(Val / Tot * 100))
            End If


            For i = 1 To 10
                If i <= Math.Ceiling(intRatio / 10) Then
                    strBar = strBar & chrFilled
                Else
                    strBar = strBar & chrEmpty
                End If

            Next



            If Unit <> "" Then
                strBar = strBar & " " & Val & "" & LCase(Unit)
            End If

            Return strBar

        Catch ex As Exception

        End Try
    End Function



    Public Sub Check_Processes()

        Dim i As Integer
        Dim Proc As Process
        Dim foo As Integer
        Dim WS As WoW_Session
        Dim strCurrentProcID As Integer
        Dim ProcName As String
        Dim ProcMemory As Long
        Dim ProcUsage As String
        Dim strMemory As String
        Dim strCPU As String

        Dim IsCurrentAccountStarted As Boolean

        Try


            With ListViewSessions
                .Items.Clear()
                '  .Columns.Add("Proc")
                '  .Columns.Add("Account")
                '  .Columns.Add("Version")
                ' .Columns.Add("Name")
            End With

            ' Cleaning collections if a window was killed by the user
            i = 0
            For Each WS In WOW_SESSIONS
                i = i + 1
                ProcName = ""
                Try
                    Proc = Process.GetProcessById(WS.ProcID)
                    ProcName = Proc.ProcessName.ToString
                Catch ex As Exception

                Finally
                    If IsNothing(Proc) Or UCase(ProcName) <> "WOW" Then
                        WOW_SESSIONS.Remove(WS)
                    End If
                End Try
            Next


            LabelOverviewSessionsCount.Text = WOW_SESSIONS.Count


            If WOW_SESSIONS.Count Then
                LabelOverviewSessionsCount.ForeColor = Color.Lime
                PictureBoxOverViewKillAll.Visible = True
                LabelOverViewKillAll.Visible = True

                i = 0

                For Each WS In WOW_SESSIONS
                    i = i + 1
                    If IsNumeric(LabelOverviewAccountID.Text) Then
                        If WS.AccountID = LabelOverviewAccountID.Text Then
                            IsCurrentAccountStarted = True
                            strCurrentProcID = WS.ProcID
                        End If
                    End If

                    Proc = Process.GetProcessById(WS.ProcID)

                    strMemory = ASCII_Pbar((Proc.PrivateMemorySize64 / 1024 / 1024), (TOTAL_RAM / 1024 / 1024), "MB")
                    strCPU = ASCII_Pbar(Math.Round(WS.GetCPU), 100)
                    ' WS.Reset_Perf_Counter()

                    ListViewSessions.Items.Add(New ListViewItem({WS.WoWversion - 1, WS.ProcID, WS.RealmName, WS.AccountName, WS.CharName, strCPU, strMemory}))

                    Try
                        foo = WS.WoWversion - 1
                        ListViewSessions.Items(i - 1).ImageIndex = foo
                    Catch ex As Exception
                        'MessageBox.Show("Error: " & ex.Message)
                    End Try


                Next

            Else
                PictureBoxOverViewKillAll.Visible = False
                LabelOverViewKillAll.Visible = False
                LabelOverviewSessionsCount.ForeColor = Color.Gray

            End If


            If IsCurrentAccountStarted Then

                LabelOverviewProcessID.Text = strCurrentProcID
                LabelOverviewProcessText.Text = "Process " & strCurrentProcID
                LabelOverviewProcessStarted.Text = "Started"
                LabelOverviewProcessStarted.ForeColor = Color.Lime
                LabelOverviewProcessKill.Visible = True
                LabelOverviewProcessSelect.Visible = True
                PictureBoxOverviewProcessKill.Visible = True
                PictureBoxOverviewProcessSelect.Visible = True


                PictureBoxOverviewCharFullscreen.Visible = False
                PictureBoxOverviewCharScreen1.Visible = False
                PictureBoxOverviewCharScreen1A.Visible = False
                PictureBoxOverviewCharScreen1B.Visible = False
                PictureBoxOverviewCharScreen1C.Visible = False
                PictureBoxOverviewCharScreen1D.Visible = False
                PictureBoxOverviewCharScreen2.Visible = False
                PictureBoxOverviewCharScreen2A.Visible = False
                PictureBoxOverviewCharScreen2B.Visible = False
                PictureBoxOverviewCharScreen2C.Visible = False
                PictureBoxOverviewCharScreen2D.Visible = False
                LabelOverviewCharFullscreen.Visible = False
                LabelOverviewCharScreen1.Visible = False
                LabelOverviewCharScreen2.Visible = False
                PictureBoxOverviewAccountFullscreen.Visible = False
                PictureBoxOverviewAccountScreen1.Visible = False
                PictureBoxOverviewAccountScreen1A.Visible = False
                PictureBoxOverviewAccountScreen1B.Visible = False
                PictureBoxOverviewAccountScreen1C.Visible = False
                PictureBoxOverviewAccountScreen1D.Visible = False
                PictureBoxOverviewAccountScreen2.Visible = False
                PictureBoxOverviewAccountScreen2A.Visible = False
                PictureBoxOverviewAccountScreen2B.Visible = False
                PictureBoxOverviewAccountScreen2C.Visible = False
                PictureBoxOverviewAccountScreen2D.Visible = False
                LabelOverviewAccountFullscreen.Visible = False
                LabelOverviewAccountScreen1.Visible = False
                LabelOverviewAccountScreen2.Visible = False

            Else
                LabelOverviewProcessID.Text = ""
                LabelOverviewProcessStarted.Text = ""
                LabelOverviewProcessText.Text = ""
                LabelOverviewProcessKill.Visible = False
                LabelOverviewProcessSelect.Visible = False
                PictureBoxOverviewProcessKill.Visible = False
                PictureBoxOverviewProcessSelect.Visible = False

                If IsNumeric(LabelOverviewAccountID.Text) Then
                    If CInt(LabelOverviewAccountID.Text) Then
                        PictureBoxOverviewAccountFullscreen.Visible = True
                        PictureBoxOverviewAccountScreen1.Visible = True
                        PictureBoxOverviewAccountScreen1A.Visible = True
                        PictureBoxOverviewAccountScreen1B.Visible = True
                        PictureBoxOverviewAccountScreen1C.Visible = True
                        PictureBoxOverviewAccountScreen1D.Visible = True
                        LabelOverviewAccountFullscreen.Visible = True
                        LabelOverviewAccountScreen1.Visible = True

                        If NBR_MONITORS > 1 Then
                            PictureBoxOverviewAccountScreen2.Visible = True
                            PictureBoxOverviewAccountScreen2A.Visible = True
                            PictureBoxOverviewAccountScreen2B.Visible = True
                            PictureBoxOverviewAccountScreen2C.Visible = True
                            PictureBoxOverviewAccountScreen2D.Visible = True
                            LabelOverviewAccountScreen2.Visible = True
                        End If
                    End If

                End If

                If IsNumeric(LabelOverviewCharID.Text) Then
                    If CInt(LabelOverviewCharID.Text) Then
                        PictureBoxOverviewCharFullscreen.Visible = True
                        PictureBoxOverviewCharScreen1.Visible = True
                        PictureBoxOverviewCharScreen1A.Visible = True
                        PictureBoxOverviewCharScreen1B.Visible = True
                        PictureBoxOverviewCharScreen1C.Visible = True
                        PictureBoxOverviewCharScreen1D.Visible = True
                        LabelOverviewCharFullscreen.Visible = True
                        LabelOverviewCharScreen1.Visible = True

                        If NBR_MONITORS > 1 Then
                            PictureBoxOverviewCharScreen2.Visible = True
                            PictureBoxOverviewCharScreen2A.Visible = True
                            PictureBoxOverviewCharScreen2B.Visible = True
                            PictureBoxOverviewCharScreen2C.Visible = True
                            PictureBoxOverviewCharScreen2D.Visible = True
                            LabelOverviewCharScreen2.Visible = True
                        End If
                    End If

                End If



            End If



        Catch ex As Exception
            'MessageBox.Show("Error: " & ex.Message)
        End Try

    End Sub


    Private Sub Ping_Server()
        Dim server As String

        Try
            If LabelOverviewServerAddress.Text = "" Then
                Exit Sub
            Else
                server = LabelOverviewServerAddress.Text
            End If
            LabelOverviewServerPing.Text = ""
            LabelOverviewServerMS.Text = "Ping..."

            Me.Refresh()


            If My.Computer.Network.Ping(server) Then
                LabelOverviewServerAddress.ForeColor = Color.Lime
                LabelOverviewServerPing.ForeColor = Color.Lime
                LabelOverviewServerPing.Text = GetPingMs(server)
                LabelOverviewServerMS.Text = "ms"

            Else
                LabelOverviewServerAddress.ForeColor = Color.Red
                LabelOverviewServerPing.ForeColor = Color.Red
                LabelOverviewServerPing.Text = ":-("
            End If
        Catch

        Finally
            ' LogIt("Can't ping " & server, Color.Red)
        End Try


    End Sub


    Private Sub ListViewSessions_click(sender As Object, e As EventArgs) Handles ListViewSessions.Click
        Try
            Dim selectedItem As ListViewItem
            Dim ProcID As Integer
            Dim WS As WoW_Session

            If ListViewSessions.SelectedIndices.Count = 0 Then Exit Sub

            Overview_Init()


            selectedItem = ListViewSessions.SelectedItems(0)

            ' Accountid = CInt(selectedItem.SubItems(2).Text)
            ProcID = CInt(selectedItem.SubItems(1).Text)

            For Each WS In WOW_SESSIONS

                If WS.ProcID = ProcID Then

                    If WS.CharID = 0 Then
                        Overview_Account(WS.AccountID)
                    Else
                        Overview_Char(WS.CharID)
                    End If

                    Exit For
                End If

            Next

        Catch ex As Exception

        End Try
    End Sub



    Private Sub ListViewOverviewGuild_Click(sender As Object, e As EventArgs) Handles ListViewOverviewGuild.Click
        Try
            Dim LinkURL As String
            Dim selectedItem As ListViewItem

            If ListViewOverviewGuild.SelectedIndices.Count = 0 Then Exit Sub

            selectedItem = ListViewOverviewGuild.SelectedItems(0)

            LinkURL = selectedItem.SubItems(2).Text


            If Strings.Left(LinkURL, 4) = "http" Then
                Process.Start(LinkURL)
            End If


        Catch ex As Exception

        End Try
    End Sub

    Private Sub ListViewOverviewLinks_Click(sender As Object, e As EventArgs) Handles ListViewOverviewLinks.Click
        Try
            Dim LinkURL As String
            Dim selectedItem As ListViewItem

            If ListViewOverviewLinks.SelectedIndices.Count = 0 Then Exit Sub

            selectedItem = ListViewOverviewLinks.SelectedItems(0)

            LinkURL = selectedItem.SubItems(2).Text


            If Strings.Left(LinkURL, 4) = "http" Then
                Process.Start(LinkURL)
            End If


        Catch ex As Exception

        End Try
    End Sub

    Private Sub ListViewOverviewMacros_Click(sender As Object, e As EventArgs) Handles ListViewOverviewMacros.Click
        Try
            Dim MacroID As Integer
            Dim selectedItem As ListViewItem



            If ListViewOverviewMacros.SelectedIndices.Count = 0 Then Exit Sub

            selectedItem = ListViewOverviewMacros.SelectedItems(0)

            MacroID = CInt(selectedItem.SubItems(2).Text)


            If MacroID Then
                Start_Macro(MacroID)
            End If


        Catch ex As Exception

        End Try
    End Sub


    Private Sub PictureBoxOverViewKillAll_MouseClick(sender As Object, e As MouseEventArgs) Handles PictureBoxOverViewKillAll.MouseClick
        Try


            Kill_WoW_Process(0)

            Check_Processes()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub PictureBoxOverviewProcessKill_Click(sender As Object, e As EventArgs) Handles PictureBoxOverviewProcessKill.Click

        Try

            If Not IsNumeric(LabelOverviewProcessID.Text) Then Exit Sub

            Kill_WoW_Process(CInt((LabelOverviewProcessID.Text)))

            Check_Processes()

        Catch ex As Exception

        End Try

    End Sub


    Private Sub PictureBoxOverviewProcessSelect_Click(sender As Object, e As EventArgs) Handles PictureBoxOverviewProcessSelect.Click
        Try

            If Not IsNumeric(LabelOverviewProcessID.Text) Then Exit Sub

            Dim p As Process = Process.GetProcessById(LabelOverviewProcessID.Text)
            If p IsNot Nothing Then
                SetForegroundWindow(p.MainWindowHandle)
                ShowWindow(p.MainWindowHandle, SW_SHOWNORMAL)
                ShowWindow(p.MainWindowHandle, SW_RESTORE)
            End If

        Catch ex As Exception

        End Try
    End Sub



    Private Sub TreeViewLauncher_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles TreeViewLauncher.NodeMouseClick

        Dim mynode As TreeNode
        Dim ID As Integer

        Try

            Overview_Init()

            mynode = CType(e.Node, TreeNode)

            Select Case Strings.Left(mynode.Name.ToString, 8)
                Case "NodeChar"
                    ID = CInt(mynode.Tag)
                    Overview_Char(ID)
                    'If MONITORING Then Timer1.Enabled = True

                Case "NodeAcco"
                    ID = CInt(mynode.Tag)
                    Overview_Account(ID)
                   ' If MONITORING Then Timer1.Enabled = True

                Case "NodeReal"
                    ID = CInt(mynode.Tag)
                    Overview_Realm(ID)

                Case "NodeServ"
                    ID = CInt(mynode.Tag)
                    Overview_Server(ID)

                Case "NodeVers"
                    ID = CInt(mynode.Tag)
                    Overview_Version(ID)

                Case Else 'nothing

            End Select

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try

    End Sub


    Private Sub TreeViewLauncher_NodeMouseDoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles TreeViewLauncher.NodeMouseDoubleClick

        'Dim mynode As TreeNode
        'Dim ID As Integer

        'Try
        '    mynode = CType(e.Node, TreeNode)

        '    Select Case Strings.Left(mynode.Name.ToString, 8)
        '        Case "NodeChar"
        '            ID = CInt(mynode.Tag)
        '            CharNode_StartWoW(ID)

        '        Case "NodeAcco"
        '            ID = CInt(mynode.Tag)
        '            AccountNode_StartWoW(ID)

        '        Case Else 'nothing

        '    End Select

        'Catch ex As Exception
        '    MessageBox.Show("Error: " & ex.Message)
        'End Try
    End Sub


    Private Sub AccountNode_StartWoW(ID As Integer)

        Dim WS As WoW_Session
        Try

            If ID = 0 Then Exit Sub

            WS = New WoW_Session

            WS.Set_WoW_Window()
            WS.Set_AccountID(ID)

            Timer1.Enabled = False
            WS.Start_WoW()
            If MONITORING Then Timer1.Enabled = True

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try

    End Sub



    Private Sub CharNode_StartWoW(ID As Integer)

        Dim WS As WoW_Session

        Try

            If ID = 0 Then Exit Sub

            WS = New WoW_Session

            WS.Set_WoW_Window()

            Timer1.Enabled = False
            WS.Start_WoW()
            If MONITORING Then Timer1.Enabled = True

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try

    End Sub


    Private Sub LaunchButtonGreen_MouseEnter(sender As Object, e As EventArgs) Handles PictureBoxOverviewAccountScreen2B.MouseEnter, PictureBoxOverviewAccountScreen1.MouseEnter, PictureBoxOverviewAccountFullscreen.MouseEnter, PictureBoxOverviewCharFullscreen.MouseEnter, PictureBoxOverviewCharScreen1.MouseEnter, PictureBoxOverviewCharScreen1C.MouseEnter, PictureBoxOverviewCharScreen1D.MouseEnter, PictureBoxOverviewCharScreen1A.MouseEnter, PictureBoxOverviewCharScreen1B.MouseEnter, PictureBoxOverviewCharScreen2.MouseEnter, PictureBoxOverviewCharScreen2C.MouseEnter, PictureBoxOverviewCharScreen2D.MouseEnter, PictureBoxOverviewCharScreen2A.MouseEnter, PictureBoxOverviewCharScreen2B.MouseEnter, PictureBoxOverviewAccountScreen2.MouseEnter, PictureBoxOverviewAccountScreen2C.MouseEnter, PictureBoxOverviewAccountScreen2D.MouseEnter, PictureBoxOverviewAccountScreen1A.MouseEnter, PictureBoxOverviewAccountScreen1B.MouseEnter, PictureBoxOverviewAccountScreen1D.MouseEnter, PictureBoxOverviewAccountScreen1C.MouseEnter, PictureBoxOverviewAccountScreen2A.MouseEnter, PictureBoxOverviewProcessSelect.MouseEnter, PictureBoxOverviewProcessKill.MouseEnter, PictureBoxOverViewKillAll.MouseEnter

        Try
            Dim myPB As PictureBox
            myPB = sender
            myPB.BackColor = Color.GreenYellow
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try



    End Sub

    Private Sub LaunchButtonRed_MouseEnter(sender As Object, e As EventArgs) Handles PictureBoxOverviewProcessKill.MouseEnter, PictureBoxOverViewKillAll.MouseEnter

        Try
            Dim myPB As PictureBox
            myPB = sender
            myPB.BackColor = Color.DarkRed
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try



    End Sub


    Private Sub LaunchButton_MouseLeave(sender As Object, e As EventArgs) Handles PictureBoxOverviewAccountScreen2B.MouseLeave, PictureBoxOverviewAccountScreen1.MouseLeave, PictureBoxOverviewAccountFullscreen.MouseLeave, PictureBoxOverviewCharFullscreen.MouseLeave, PictureBoxOverviewCharScreen1.MouseLeave, PictureBoxOverviewCharScreen1C.MouseLeave, PictureBoxOverviewCharScreen1D.MouseLeave, PictureBoxOverviewCharScreen1A.MouseLeave, PictureBoxOverviewCharScreen1B.MouseLeave, PictureBoxOverviewCharScreen2.MouseLeave, PictureBoxOverviewCharScreen2C.MouseLeave, PictureBoxOverviewCharScreen2D.MouseLeave, PictureBoxOverviewCharScreen2A.MouseLeave, PictureBoxOverviewCharScreen2B.MouseLeave, PictureBoxOverviewAccountScreen2.MouseLeave, PictureBoxOverviewAccountScreen2C.MouseLeave, PictureBoxOverviewAccountScreen2D.MouseLeave, PictureBoxOverviewAccountScreen1A.MouseLeave, PictureBoxOverviewAccountScreen1B.MouseLeave, PictureBoxOverviewAccountScreen1D.MouseLeave, PictureBoxOverviewAccountScreen1C.MouseLeave, PictureBoxOverviewAccountScreen2A.MouseLeave, PictureBoxOverviewProcessKill.MouseLeave, PictureBoxOverviewProcessSelect.MouseLeave, PictureBoxOverViewKillAll.MouseLeave

        Try
            Dim myPB As PictureBox
            myPB = sender
            myPB.BackColor = Color.White
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub LaunchButton_Click(sender As Object, e As EventArgs) Handles PictureBoxOverviewCharScreen2D.Click, PictureBoxOverviewCharScreen2C.Click, PictureBoxOverviewCharScreen2B.Click, PictureBoxOverviewCharScreen2A.Click, PictureBoxOverviewCharScreen2.Click, PictureBoxOverviewCharScreen1D.Click, PictureBoxOverviewCharScreen1C.Click, PictureBoxOverviewCharScreen1B.Click, PictureBoxOverviewCharScreen1A.Click, PictureBoxOverviewCharScreen1.Click, PictureBoxOverviewCharFullscreen.Click, PictureBoxOverviewAccountScreen2D.Click, PictureBoxOverviewAccountScreen2C.Click, PictureBoxOverviewAccountScreen2B.Click, PictureBoxOverviewAccountScreen2A.Click, PictureBoxOverviewAccountScreen2.Click, PictureBoxOverviewAccountScreen1D.Click, PictureBoxOverviewAccountScreen1C.Click, PictureBoxOverviewAccountScreen1B.Click, PictureBoxOverviewAccountScreen1A.Click, PictureBoxOverviewAccountScreen1.Click, PictureBoxOverviewAccountFullscreen.Click

        Dim tags(3) As String
        Dim WS As WoW_Session

        Dim AccountID As Integer
        Dim CharID As Integer

        Try

            If Not IsNumeric(LabelOverviewAccountID.Text) Then Exit Sub

            AccountID = CInt(LabelOverviewAccountID.Text)

            WS = New WoW_Session

            tags = Split(sender.Tag, ";")


            If tags(0) = "C" Then

                If IsNumeric(LabelOverviewCharID.Text) Then
                    CharID = CInt(LabelOverviewCharID.Text)
                End If
            End If


            If Strings.Left(tags(1), 1) = "F" Then
                WS.IsWindowed = False
            Else
                WS.IsWindowed = True
            End If

            Select Case tags(1)
                Case "FS"
                    WS.Screen = 1
                Case "S1"
                    WS.Screen = 1
                Case "S2"
                    WS.Screen = 2
                Case Else
                    WS.Screen = 1
            End Select

            If UBound(tags) = 2 Then

                Select Case tags(2)
                    Case "A"
                        WS.Set_WoW_Window("TopLeft")
                    Case "B"
                        WS.Set_WoW_Window("TopRight")
                    Case "C"
                        WS.Set_WoW_Window("BottomLeft")
                    Case "D"
                        WS.Set_WoW_Window("BottomRight")
                    Case Else
                        WS.Set_WoW_Window()
                End Select
            Else
                WS.Set_WoW_Window()
            End If

            If CharID Then
                WS.Set_CharID(CharID)
            Else
                WS.Set_AccountID(AccountID)
            End If

            Timer1.Enabled = False
            WS.Start_WoW()
            If MONITORING Then Timer1.Enabled = True

            Check_Processes()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try

    End Sub



    Private Sub LabelOverviewServerName_Click(sender As Object, e As EventArgs) Handles LabelOverviewServerName.Click
        Dim Server As String
        Dim URL As String
        Dim i As Integer
        Try
            If LabelOverviewServerAddress.Text <> "" Then

                Server = LabelOverviewServerAddress.Text

                i = Strings.InStr(Server, ".")

                URL = "http://" & Strings.Mid(Server, i + 1)

                Process.Start(URL)


            End If


        Catch ex As Exception

        End Try
    End Sub

    Private Sub LabelOverviewServerAddress_Click(sender As Object, e As EventArgs) Handles LabelOverviewServerAddress.Click
        Try
            Ping_Server()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub PictureBoxOverviewVersion_Click(sender As Object, e As EventArgs) Handles PictureBoxOverviewVersion.Click
        Try
            Select Case PictureBoxOverviewVersion.Tag
                Case 2
                    Process.Start(BC_DIR)
                Case 3
                    Process.Start(TURTLE_DIR)
                Case Else
                    Process.Start(VANILLA_DIR)
            End Select

        Catch ex As Exception

        End Try
    End Sub

    Private Sub LabelOverviewServerPing_Click(sender As Object, e As EventArgs) Handles LabelOverviewServerPing.Click
        Try
            Ping_Server()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub PictureBoxAppMin_Click(sender As Object, e As EventArgs) Handles PictureBoxAppMin.Click
        Try
            Me.WindowState = FormWindowState.Minimized
        Catch ex As Exception

        End Try
    End Sub

    Private Sub PictureBoxAppExit_Click(sender As Object, e As EventArgs) Handles PictureBoxAppExit.Click
        Try
            Application.Exit()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub PictureBoxAppMin_MouseEnter(sender As Object, e As EventArgs) Handles PictureBoxAppMin.MouseEnter
        Try
            TabPageMain.BackColor = Color.DarkBlue
        Catch ex As Exception

        End Try

    End Sub

    Private Sub PictureBoxAppExit_MouseEnter(sender As Object, e As EventArgs) Handles PictureBoxAppExit.MouseEnter
        Try
            TabPageMain.BackColor = Color.DarkRed
        Catch ex As Exception

        End Try

    End Sub

    Private Sub PictureBoxAppExit_MouseLeave(sender As Object, e As EventArgs) Handles PictureBoxAppExit.MouseLeave
        Try
            TabPageMain.BackColor = Color.FromArgb(255, 64, 64, 64)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub PictureBoxAppMin_MouseLeave(sender As Object, e As EventArgs) Handles PictureBoxAppMin.MouseLeave
        Try
            TabPageMain.BackColor = Color.FromArgb(255, 64, 64, 64)
        Catch ex As Exception

        End Try
    End Sub


    Private Sub ListViewOverviewGuild_Load()

        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader
        Dim LinkLabel As String
        Dim LinkURL As String
        Dim ImageIdx As Integer
        Dim i As Integer

        Try

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            ListViewOverviewGuild.Items.Clear()


            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "SELECT LinkLabel, LinkURL, LinkImage FROM weblink WHERE IsGuild=1 ORDER BY LinkOrder"
                objReader = objCommand.ExecuteReader()

                While (objReader.Read())

                    i = i + 1
                    LinkLabel = objReader("LinkLabel")
                    LinkURL = objReader("LinkURL")

                    If Not IsNumeric(objReader("LinkImage")) Or IsDBNull(objReader("LinkImage")) Then
                        ImageIdx = 13
                    Else
                        ImageIdx = CInt(objReader("LinkImage"))
                    End If

                    ListViewOverviewGuild.Items.Add(New ListViewItem({"", LinkLabel, LinkURL}))

                    ListViewOverviewGuild.Items(i - 1).ImageIndex = ImageIdx
                    ListViewOverviewGuild.Items(i - 1).ToolTipText = LinkURL

                End While



            End Using


        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try


    End Sub


    Private Sub ListViewOverviewLinks_Load()

        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader
        Dim LinkLabel As String
        Dim LinkURL As String
        Dim ImageIdx As Integer
        Dim i As Integer

        Try

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            ListViewOverviewLinks.Items.Clear()


            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "SELECT LinkLabel, LinkURL, LinkImage FROM weblink WHERE (IsGuild=0 OR IsGuild IS NULL) ORDER BY LinkOrder"
                objReader = objCommand.ExecuteReader()

                While (objReader.Read())

                    i = i + 1
                    LinkLabel = objReader("LinkLabel")
                    LinkURL = objReader("LinkURL")

                    If Not IsNumeric(objReader("LinkImage")) Or IsDBNull(objReader("LinkImage")) Then
                        ImageIdx = 13
                    Else
                        ImageIdx = CInt(objReader("LinkImage"))
                    End If

                    ListViewOverviewLinks.Items.Add(New ListViewItem({"", LinkLabel, LinkURL}))

                    ListViewOverviewLinks.Items(i - 1).ImageIndex = ImageIdx
                    ListViewOverviewLinks.Items(i - 1).ToolTipText = LinkURL

                End While



            End Using


        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try


    End Sub


    Private Sub ListViewOverviewMacros_Load()

        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader
        Dim MacroID As Integer
        Dim MacroName As String
        Dim MacroNote As String
        Dim i As Integer

        Try

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            ListViewOverviewMacros.Items.Clear()


            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "SELECT MacroID, MacroIsFav, MacroName, MacroNote FROM macro "
                objReader = objCommand.ExecuteReader()

                While (objReader.Read())

                    i = i + 1
                    MacroID = CInt(objReader("MacroID"))
                    MacroName = objReader("MacroName")
                    MacroNote = objReader("MacroNote")

                    ListViewOverviewMacros.Items.Add(New ListViewItem({"", MacroName, MacroID}))
                    ListViewOverviewMacros.Items(i - 1).ImageIndex = 0
                    ListViewOverviewMacros.Items(i - 1).ToolTipText = MacroNote


                End While



            End Using


        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try


    End Sub

    Private Sub ListViewSessions_MouseEnter(sender As Object, e As EventArgs) Handles ListViewSessions.MouseEnter
        Try
            Me.Timer1.Enabled = False
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ListViewSessions_MouseLeave(sender As Object, e As EventArgs) Handles ListViewSessions.MouseLeave
        Try
            If MONITORING Then Me.Timer1.Enabled = True
        Catch ex As Exception

        End Try
    End Sub

#End Region


    ' #####################################################################################################################"
#Region "Macro_mgt"


    Private Sub DataGridViewMacros_Load()

        Dim conn = New SQLiteConnection(CONSTRING)

        Try
            Using (conn)
                conn.Open()

                Dim sql As String = "SELECT * FROM macro ORDER BY MacroID"

                Dim cmdDataGrid As SQLiteCommand = New SQLiteCommand(sql, conn)

                Dim da As New SQLiteDataAdapter
                da.SelectCommand = cmdDataGrid
                Dim dt As New DataTable
                da.Fill(dt)

                With DataGridViewMacros

                    .DataSource = Nothing
                    .AutoGenerateColumns = False
                    .ReadOnly = True
                    .AllowUserToAddRows = False
                    .ColumnCount = 4

                    .Columns(0).Name = "ID"
                    .Columns(0).DataPropertyName = "MacroID"
                    .Columns(0).Visible = False

                    .Columns(1).Name = "Name"
                    .Columns(1).DataPropertyName = "MacroName"

                    .Columns(2).Name = "Note"
                    .Columns(2).DataPropertyName = "MacroNote"

                    .Columns(3).Name = "IsFav"
                    .Columns(3).DataPropertyName = "MacroIsFav"
                    .Columns(3).Visible = False

                    Dim imgFav As New DataGridViewImageColumn()
                    imgFav.Image = My.Resources.question_mark
                    imgFav.HeaderText = "Fav"
                    imgFav.Width = 8
                    imgFav.ImageLayout = DataGridViewImageCellLayout.Zoom
                    imgFav.Name = "imgFav"
                    .Columns.Add(imgFav)

                    .DataSource = dt

                    For Each row As DataGridViewRow In .Rows
                        If row.Cells("ID").Value = LastMacro Then
                            .CurrentCell = row.Cells(1)
                            Exit For
                        End If
                    Next

                End With


            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try

    End Sub


    Private Sub DataGridViewMacros_RowEnter(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewMacros.RowEnter

        Try

            LastMacro = DataGridViewMacros.Item(0, e.RowIndex).Value

            LabelMacroID.Text = DataGridViewMacros.Item(0, e.RowIndex).Value
            TextBoxMacroName.Text = DataGridViewMacros.Item(1, e.RowIndex).Value
            TextBoxMacroNote.Text = DataGridViewMacros.Item(2, e.RowIndex).Value


            If CBool(DataGridViewMacros.Item(3, e.RowIndex).Value) = True Then
                CheckBoxMacroIsFav.Checked = True
            Else
                CheckBoxMacroIsFav.Checked = False
            End If

            If RadioButtonShowStepLink.Checked Then
                DataGridViewSteps_Load()

            End If


        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub


    Private Sub DataGridViewMacros_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridViewMacros.CellFormatting
        Try

            If e.RowIndex < 0 Then Exit Sub

            Select Case e.ColumnIndex

                Case 4 ' Fav image (IsFav in 3)
                    Select Case CBool(DataGridViewMacros.Rows(e.RowIndex).Cells(3).Value)
                        Case False
                            e.Value = New Bitmap(1, 1)
                        Case True
                            e.Value = My.Resources.favorite

                    End Select

            End Select

        Catch ex As Exception

        End Try
    End Sub


    Private Sub ButtonMacroAdd_Click(sender As Object, e As EventArgs) Handles ButtonMacroAdd.Click
        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader
        Dim ID As Long


        Try


            Controls_Color_Init()

            If TextBoxMacroName.Text = "" Then
                TextBoxMacroName.BackColor = Color.IndianRed
                Exit Sub
            End If

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "SELECT MAX(MacroID) as ID FROM macro"
                objReader = objCommand.ExecuteReader()
                While (objReader.Read())
                    If IsDBNull(objReader("ID")) Then
                        ID = 1
                    Else
                        ID = CLng(objReader("ID")) + 1
                    End If

                End While
                objReader.Close()


                objCommand.CommandText = "INSERT INTO macro (MacroID, MacroIsFav, MacroName, MacroNote) VALUES " &
                                         " ($MacroID, $MacroIsFav, $MacroName, $MacroNote) "

                objCommand.Parameters.Add("$MacroID", DbType.Int32).Value = ID
                objCommand.Parameters.Add("$MacroName", DbType.String).Value = TextBoxMacroName.Text
                objCommand.Parameters.Add("$MacroNote", DbType.String).Value = TextBoxMacroNote.Text
                objCommand.Parameters.Add("$MacroIsFav", DbType.Boolean).Value = CheckBoxMacroIsFav.Checked


                objCommand.ExecuteNonQuery()

                LastMacro = ID

                ComboBoxStepMacro_Load()
                DataGridViewMacros_Load()
                ListViewOverviewMacros_Load()


            End Using

            LogIt("Macro " & TextBoxMacroName.Text & " Added", Color.DarkGreen)

        Catch ex As Exception
            LogIt("Server creation failed ! ", Color.Red)
            MessageBox.Show("Error: " & ex.Message)

        End Try
    End Sub

    Private Sub ButtonMacroSave_Click(sender As Object, e As EventArgs) Handles ButtonMacroSave.Click
        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim ID As Long

        Try

            Controls_Color_Init()

            If TextBoxMacroName.Text = "" Then
                TextBoxMacroName.BackColor = Color.IndianRed
                Exit Sub
            End If

            ID = CInt(LabelMacroID.Text)

            If ID = 0 Then Exit Sub

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "UPDATE macro SET MacroName=$MacroName, MacroNote=$MacroNote, MacroIsFav=$MacroIsFav WHERE MacroID=$MacroID;"

                objCommand.Parameters.Add("$MacroID", DbType.Int32).Value = ID
                objCommand.Parameters.Add("$MacroName", DbType.String).Value = TextBoxMacroName.Text
                objCommand.Parameters.Add("$MacroNote", DbType.String).Value = TextBoxMacroNote.Text
                objCommand.Parameters.Add("$MacroIsFav", DbType.Boolean).Value = CheckBoxMacroIsFav.Checked

                objCommand.ExecuteNonQuery()

                LastMacro = ID
                ComboBoxStepMacro_Load()
                DataGridViewMacros_Load()
                ListViewOverviewMacros_Load()
            End Using

            LogIt("Macro " & TextBoxMacroName.Text & " modified", Color.DarkGreen)

        Catch ex As Exception
            LogIt("Modification failed ! ", Color.Red)
            MessageBox.Show("Error: " & ex.Message)

        End Try
    End Sub

    Private Sub ButtonMacroDel_Click(sender As Object, e As EventArgs) Handles ButtonMacroDel.Click
        Dim ID As Long

        Try
            ID = CInt(LabelMacroID.Text)
            If ID = 0 Then Exit Sub

            DB_Delete_Macro(ID)

            ComboBoxStepMacro_Load()
            DataGridViewMacros_Load()
            ListViewOverviewMacros_Load()

            LogIt("Macro deleted", Color.DarkGreen)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try
    End Sub




#End Region

    ' #####################################################################################################################"
#Region "Step_mgt"



    Private Sub DataGridViewSteps_Load()

        Dim conn = New SQLiteConnection(CONSTRING)
        Dim strWhere As String = ""

        Try
            Using (conn)
                conn.Open()


                If RadioButtonShowStepLink.Checked And LastMacro Then
                    strWhere = " WHERE s.MacroID=" & LastMacro & " "
                End If

                Dim sql As String = " SELECT s.*, m.MacroName, c.CharName, (a.AccountName || ' of ' || r.RealmName) AS AccountLabel  " &
                                    " FROM macro_step s " &
                                    " LEFT JOIN macro m ON s.MacroID=m.MacroID " &
                                    " LEFT JOIN account a ON s.AccountID=a.AccountID " &
                                    " LEFT JOIN character c ON s.CharID=c.CharID " &
                                    " LEFT JOIN realm r ON a.RealmID=r.RealmID " &
                                    strWhere &
                                    " ORDER BY s.MacroID, s.StepOrder"

                Dim cmdDataGrid As SQLiteCommand = New SQLiteCommand(sql, conn)

                Dim da As New SQLiteDataAdapter
                da.SelectCommand = cmdDataGrid
                Dim dt As New DataTable
                da.Fill(dt)

                With DataGridViewSteps

                    .DataSource = Nothing
                    .AutoGenerateColumns = False
                    .ReadOnly = True
                    .AllowUserToAddRows = False
                    .ColumnCount = 14

                    .Columns(0).Name = "ID"
                    .Columns(0).DataPropertyName = "StepID"
                    '   .Columns(0).Visible = False

                    .Columns(1).Name = "MacroID"
                    .Columns(1).DataPropertyName = "MacroID"
                    .Columns(1).Visible = False

                    .Columns(2).Name = "Macro"
                    .Columns(2).DataPropertyName = "MacroName"

                    .Columns(3).Name = "Order"
                    .Columns(3).DataPropertyName = "StepOrder"

                    .Columns(4).Name = "Wait"
                    .Columns(4).DataPropertyName = "StepWait"

                    .Columns(5).Name = "AccountID"
                    .Columns(5).DataPropertyName = "AccountID"
                    .Columns(5).Visible = False

                    .Columns(6).Name = "Account"
                    .Columns(6).DataPropertyName = "AccountLabel"


                    .Columns(7).Name = "CharID"
                    .Columns(7).DataPropertyName = "CharID"
                    .Columns(7).Visible = False

                    .Columns(8).Name = "Character"
                    .Columns(8).DataPropertyName = "CharName"

                    .Columns(9).Name = "StepWindow"
                    .Columns(9).DataPropertyName = "StepWindow"

                    .Columns(10).Name = "Left"
                    .Columns(10).DataPropertyName = "StepX"

                    .Columns(11).Name = "Top"
                    .Columns(11).DataPropertyName = "StepY"

                    .Columns(12).Name = "Width"
                    .Columns(12).DataPropertyName = "StepW"

                    .Columns(13).Name = "Height"
                    .Columns(13).DataPropertyName = "StepH"

                    .DataSource = dt

                    For Each row As DataGridViewRow In .Rows
                        If row.Cells("ID").Value = LastStep Then
                            .CurrentCell = row.Cells(2)
                            Exit For
                        End If
                    Next

                End With


            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try

    End Sub

    Private Sub DataGridViewSteps_RowEnter(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridViewSteps.RowEnter

        Try
            LabelStepID.Text = DataGridViewSteps.Item(0, e.RowIndex).Value
            ComboBoxStepMacro.SelectedValue = DataGridViewSteps.Item(1, e.RowIndex).Value
            TextBoxStepOrder.Text = DataGridViewSteps.Item(3, e.RowIndex).Value
            TextBoxStepWait.Text = DataGridViewSteps.Item(4, e.RowIndex).Value
            ComboBoxStepAccount.SelectedValue = DataGridViewSteps.Item(5, e.RowIndex).Value
            ComboBoxStepChar.SelectedValue = DataGridViewSteps.Item(7, e.RowIndex).Value

            Select Case DataGridViewSteps.Item(9, e.RowIndex).Value
                Case "Windowed"
                    ComboBoxStepWindow.SelectedValue = "Windowed"
                    ComboBoxStepWindow.Text = "Windowed"

                Case "Windowed1"
                    ComboBoxStepWindow.SelectedValue = "Windowed Screen 1"
                    ComboBoxStepWindow.Text = "Windowed Screen 1"

                Case "Windowed2"
                    ComboBoxStepWindow.SelectedValue = "Windowed Screen 2"
                    ComboBoxStepWindow.Text = "Windowed Screen 2"

                Case Else
                    ComboBoxStepWindow.SelectedValue = "Full Screen"
                    ComboBoxStepWindow.Text = "Full Screen"

            End Select

            If ComboBoxStepWindow.SelectedValue = "Full Screen" Then
                TextBoxStepW.Enabled = False
                TextBoxStepH.Enabled = False
                TextBoxStepX.Enabled = False
                TextBoxStepY.Enabled = False
            Else
                TextBoxStepW.Enabled = True
                TextBoxStepH.Enabled = True
                TextBoxStepX.Enabled = True
                TextBoxStepY.Enabled = True
            End If



            If IsDBNull(DataGridViewSteps.Item(10, e.RowIndex).Value) Then
                TextBoxStepX.Text = ""
            Else
                TextBoxStepX.Text = DataGridViewSteps.Item(10, e.RowIndex).Value
            End If

            If IsDBNull(DataGridViewSteps.Item(11, e.RowIndex).Value) Then
                TextBoxStepY.Text = ""
            Else
                TextBoxStepY.Text = DataGridViewSteps.Item(11, e.RowIndex).Value
            End If


            If IsDBNull(DataGridViewSteps.Item(12, e.RowIndex).Value) Then
                TextBoxStepW.Text = ""
            Else
                TextBoxStepW.Text = DataGridViewSteps.Item(12, e.RowIndex).Value
            End If

            If IsDBNull(DataGridViewSteps.Item(13, e.RowIndex).Value) Then
                TextBoxStepH.Text = ""
            Else
                TextBoxStepH.Text = DataGridViewSteps.Item(13, e.RowIndex).Value
            End If



        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub



    Private Sub ComboBoxStepMacro_Load()

        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader
        Dim dt As New DataTable

        Try

            dt.Columns.Add("ID")
            dt.Columns.Add("Name")
            'dt.Rows.Add("", "")

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "SELECT MacroID AS ID, MacroName AS Name FROM macro ORDER BY MacroID"
                objReader = objCommand.ExecuteReader()
                While (objReader.Read())
                    dt.Rows.Add(objReader("ID"), objReader("Name"))
                End While

                ComboBoxStepMacro.DataSource = dt

                ComboBoxStepMacro.ValueMember = "ID"
                ComboBoxStepMacro.DisplayMember = "Name"
                ComboBoxStepMacro.DropDownStyle = ComboBoxStyle.DropDown
            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try
    End Sub

    Private Sub ComboBoxStepAccount_Load()

        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader
        Dim dt As New DataTable

        Try

            dt.Columns.Add("ID")
            dt.Columns.Add("Name")
            'dt.Rows.Add("", "")

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "SELECT  (a.AccountName || ' of ' ||  R.RealmName) AS Name, a.AccountID AS ID FROM account a INNER JOIN realm r ON r.RealmID=a.RealmID INNER JOIN server s on S.ServerID=r.ServerID ORDER BY r.RealmName, a.AccountName"
                objReader = objCommand.ExecuteReader()
                While (objReader.Read())
                    dt.Rows.Add(objReader("ID"), objReader("Name"))
                End While

                ComboBoxStepAccount.DataSource = dt

                ComboBoxStepAccount.ValueMember = "ID"
                ComboBoxStepAccount.DisplayMember = "Name"
                ComboBoxStepAccount.DropDownStyle = ComboBoxStyle.DropDown
            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try
    End Sub

    Private Sub ComboBoxStepChar_Load(intAccountID As Integer)

        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader
        Dim dt As New DataTable

        Try

            If intAccountID = 0 Then Exit Sub

            dt.Columns.Add("ID")
            dt.Columns.Add("Name")
            'dt.Rows.Add("", "")

            dt.Rows.Add(0, "")

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "SELECT  CharName AS Name, CharID AS ID FROM character WHERE AccountID=" & intAccountID & " ORDER BY CharName"
                objReader = objCommand.ExecuteReader()
                While (objReader.Read())
                    dt.Rows.Add(objReader("ID"), objReader("Name"))
                End While

                ComboBoxStepChar.DataSource = dt

                ComboBoxStepChar.ValueMember = "ID"
                ComboBoxStepChar.DisplayMember = "Name"
                ComboBoxStepChar.DropDownStyle = ComboBoxStyle.DropDown
            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try
    End Sub


    Private Sub ComboBoxStepWindow_SelectedValueChanged(sender As Object, e As EventArgs) Handles ComboBoxStepWindow.SelectedValueChanged

        Try

            Try
                If ComboBoxStepWindow.Text = "Full Screen" Then

                    TextBoxStepW.Enabled = False
                    TextBoxStepH.Enabled = False
                    TextBoxStepX.Enabled = False
                    TextBoxStepY.Enabled = False
                Else
                    TextBoxStepW.Enabled = True
                    TextBoxStepH.Enabled = True
                    TextBoxStepX.Enabled = True
                    TextBoxStepY.Enabled = True
                End If

            Catch ex As Exception

            End Try

        Catch ex As Exception

        End Try
    End Sub


    Private Sub ButtonStepAdd_Click(sender As Object, e As EventArgs) Handles ButtonStepAdd.Click
        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim objReader As SQLiteDataReader
        Dim ID As Long


        Try

            Controls_Color_Init()

            If ComboBoxStepMacro.SelectedValue = "" Then
                ComboBoxStepMacro.BackColor = Color.IndianRed
                Exit Sub
            End If

            If ComboBoxStepAccount.SelectedValue = "" Then
                ComboBoxStepAccount.BackColor = Color.IndianRed
                Exit Sub
            End If

            If Not IsNumeric((TextBoxStepOrder.Text)) Then
                TextBoxStepOrder.Text = 1
            End If

            If Not IsNumeric((TextBoxStepWait.Text)) Then
                TextBoxStepWait.Text = 1
            End If


            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "SELECT MAX(StepID) as ID FROM macro_step"
                objReader = objCommand.ExecuteReader()
                While (objReader.Read())
                    If IsDBNull(objReader("ID")) Then
                        ID = 1
                    Else
                        ID = CLng(objReader("ID")) + 1
                    End If

                End While
                objReader.Close()


                objCommand.CommandText = "INSERT INTO macro_step (StepID, MacroID , AccountID , CharID , StepOrder , StepWait , StepWindow , StepX , StepY ,  StepW , StepH  ) VALUES " &
                                         " ( $StepID, $MacroID , $AccountID , $CharID , $StepOrder , $StepWait , $StepWindow , $StepX , $StepY ,  $StepW , $StepH  ) "

                objCommand.Parameters.Add("$StepID", DbType.Int32).Value = ID
                objCommand.Parameters.Add("$MacroID", DbType.Int32).Value = CInt(ComboBoxStepMacro.SelectedValue)
                objCommand.Parameters.Add("$AccountID", DbType.Int32).Value = CInt(ComboBoxStepAccount.SelectedValue)

                If ComboBoxStepChar.SelectedValue = 0 Then
                    objCommand.Parameters.Add("$CharID", DbType.Int32).Value = DBNull.Value
                Else
                    objCommand.Parameters.Add("$CharID", DbType.Int32).Value = CInt(ComboBoxStepChar.SelectedValue)
                End If


                objCommand.Parameters.Add("$StepOrder", DbType.Int32).Value = CInt(TextBoxStepOrder.Text)
                objCommand.Parameters.Add("$StepWait", DbType.Int32).Value = CInt(TextBoxStepWait.Text)

                Select Case ComboBoxStepWindow.Text
                    Case "Windowed"

                        objCommand.Parameters.Add("$StepWindow", DbType.String).Value = "Windowed"
                    Case "Windowed Screen 1"
                        objCommand.Parameters.Add("$StepWindow", DbType.String).Value = "Windowed1"

                    Case "Windowed Screen 2"
                        objCommand.Parameters.Add("$StepWindow", DbType.String).Value = "Windowed2"

                    Case Else
                        objCommand.Parameters.Add("$StepWindow", DbType.String).Value = "FullScreen"

                End Select




                If TextBoxStepX.Text = "" Then
                    objCommand.Parameters.Add("$StepX", DbType.Int32).Value = DBNull.Value
                Else
                    objCommand.Parameters.Add("$StepX", DbType.Int32).Value = CInt(TextBoxStepX.Text)
                End If

                If TextBoxStepY.Text = "" Then
                    objCommand.Parameters.Add("$StepY", DbType.Int32).Value = DBNull.Value
                Else
                    objCommand.Parameters.Add("$StepY", DbType.Int32).Value = CInt(TextBoxStepY.Text)
                End If

                If TextBoxStepW.Text = "" Then
                    objCommand.Parameters.Add("$StepW", DbType.Int32).Value = DBNull.Value
                Else
                    objCommand.Parameters.Add("$StepW", DbType.Int32).Value = CInt(TextBoxStepW.Text)
                End If

                If TextBoxStepH.Text = "" Then
                    objCommand.Parameters.Add("$StepH", DbType.Int32).Value = DBNull.Value
                Else
                    objCommand.Parameters.Add("$StepH", DbType.Int32).Value = CInt(TextBoxStepH.Text)
                End If


                objCommand.ExecuteNonQuery()

                LastStep = ID

                DataGridViewSteps_Load()

            End Using

            LogIt("Added step to macro " & TextBoxMacroName.Text, Color.DarkGreen)

        Catch ex As Exception
            LogIt("Step creation failed ! ", Color.Red)
            MessageBox.Show("Error: " & ex.Message)

        End Try

    End Sub


    Private Sub ButtonStepSave_Click(sender As Object, e As EventArgs) Handles ButtonStepSave.Click

        Dim objConn As SQLiteConnection
        Dim objCommand As SQLiteCommand
        Dim ID As Long

        Try

            Controls_Color_Init()

            If ComboBoxStepMacro.SelectedValue = "" Then
                ComboBoxStepMacro.BackColor = Color.IndianRed
                Exit Sub
            End If

            If ComboBoxStepAccount.SelectedValue = "" Then
                ComboBoxStepAccount.BackColor = Color.IndianRed
                Exit Sub
            End If



            If Not IsNumeric((TextBoxStepOrder.Text)) Then
                TextBoxStepOrder.Text = 1
            End If

            If Not IsNumeric((TextBoxStepWait.Text)) Then
                TextBoxStepWait.Text = 1
            End If

            ID = CInt(LabelStepID.Text)

            If ID = 0 Then Exit Sub

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)
                objConn.Open()
                objCommand = objConn.CreateCommand()

                objCommand.CommandText = "UPDATE macro_step SET MacroID=$MacroID , AccountID=$AccountID , CharID=$CharID , StepOrder=$StepOrder , StepWait=$StepWait , StepWindow=$StepWindow , StepX=$StepX , StepY=$StepY ,  StepW=$StepW , StepH=$StepH  WHERE StepID=$StepID;"

                objCommand.Parameters.Add("$StepID", DbType.Int32).Value = ID
                objCommand.Parameters.Add("$MacroID", DbType.Int32).Value = CInt(ComboBoxStepMacro.SelectedValue)
                objCommand.Parameters.Add("$AccountID", DbType.Int32).Value = CInt(ComboBoxStepAccount.SelectedValue)

                If ComboBoxStepChar.SelectedValue = 0 Then
                    objCommand.Parameters.Add("$CharID", DbType.Int32).Value = DBNull.Value
                Else
                    objCommand.Parameters.Add("$CharID", DbType.Int32).Value = CInt(ComboBoxStepChar.SelectedValue)
                End If


                objCommand.Parameters.Add("$StepOrder", DbType.Int32).Value = CInt(TextBoxStepOrder.Text)
                objCommand.Parameters.Add("$StepWait", DbType.Int32).Value = CInt(TextBoxStepWait.Text)

                Select Case ComboBoxStepWindow.Text
                    Case "Windowed"

                        objCommand.Parameters.Add("$StepWindow", DbType.String).Value = "Windowed"
                    Case "Windowed Screen 1"
                        objCommand.Parameters.Add("$StepWindow", DbType.String).Value = "Windowed1"

                    Case "Windowed Screen 2"
                        objCommand.Parameters.Add("$StepWindow", DbType.String).Value = "Windowed2"

                    Case Else
                        objCommand.Parameters.Add("$StepWindow", DbType.String).Value = "FullScreen"

                End Select

                If TextBoxStepX.Text = "" Then
                    objCommand.Parameters.Add("$StepX", DbType.Int32).Value = DBNull.Value
                Else
                    objCommand.Parameters.Add("$StepX", DbType.Int32).Value = CInt(TextBoxStepX.Text)
                End If

                If TextBoxStepY.Text = "" Then
                    objCommand.Parameters.Add("$StepY", DbType.Int32).Value = DBNull.Value
                Else
                    objCommand.Parameters.Add("$StepY", DbType.Int32).Value = CInt(TextBoxStepY.Text)
                End If

                If TextBoxStepW.Text = "" Then
                    objCommand.Parameters.Add("$StepW", DbType.Int32).Value = DBNull.Value
                Else
                    objCommand.Parameters.Add("$StepW", DbType.Int32).Value = CInt(TextBoxStepW.Text)
                End If

                If TextBoxStepH.Text = "" Then
                    objCommand.Parameters.Add("$StepH", DbType.Int32).Value = DBNull.Value
                Else
                    objCommand.Parameters.Add("$StepH", DbType.Int32).Value = CInt(TextBoxStepH.Text)
                End If

                objCommand.ExecuteNonQuery()

                LastStep = ID
                DataGridViewSteps_Load()

            End Using

            LogIt("Step modified for macro " & TextBoxMacroName.Text & " ", Color.DarkGreen)

        Catch ex As Exception
            LogIt("Modification failed ! ", Color.Red)
            MessageBox.Show("Error: " & ex.Message)

        End Try

    End Sub


    Private Sub ButtonStepDel_Click(sender As Object, e As EventArgs) Handles ButtonStepDel.Click
        Dim ID As Long

        Try
            ID = CInt(LabelMacroID.Text)
            If ID = 0 Then Exit Sub

            DB_Delete_Step(ID)

            MainForm_Reload()

            LogIt("Step deleted", Color.DarkGreen)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try
    End Sub

    Private Sub ComboBoxStepAccount_SelectedValueChanged(sender As Object, e As EventArgs) Handles ComboBoxStepAccount.SelectedValueChanged
        Try

            ComboBoxStepChar_Load(ComboBoxStepAccount.SelectedValue)
        Catch ex As Exception
            'MessageBox.Show("Error: " & ex.Message)
        End Try

    End Sub

    Private Sub RadioButtonShowStepAll_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonShowStepAll.CheckedChanged
        Try
            If IsMainFormLoaded Then DataGridViewSteps_Load()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub RadioButtonShowStepLink_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonShowStepLink.CheckedChanged
        Try
            If IsMainFormLoaded Then DataGridViewSteps_Load()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub ButtonMacroTest_Click(sender As Object, e As EventArgs) Handles ButtonMacroTest.Click
        Try
            Start_Macro(CInt(LabelMacroID.Text))
        Catch ex As Exception

        End Try

    End Sub

    Private Sub TextBoxStepW_TextChanged(sender As Object, e As EventArgs) Handles TextBoxStepW.Validated
        Try
            Exit Sub

            If Val(TextBoxStepW.Text) Then

                If ComboBoxStepWindow.Text = "Windowed Screen 2" Then
                    TextBoxStepH.Text = Math.Round(Val(TextBoxStepW.Text) / SCREEN2_RATIO)
                Else
                    TextBoxStepH.Text = Math.Round(Val(TextBoxStepW.Text) / SCREEN1_RATIO)

                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub TextBoxStepH_TextChanged(sender As Object, e As EventArgs) Handles TextBoxStepH.Validated
        Try

            Exit Sub

            If Val(TextBoxStepH.Text) Then

                If ComboBoxStepWindow.Text = "Windowed Screen 2" Then
                    TextBoxStepW.Text = Math.Round(Val(TextBoxStepH.Text) * SCREEN2_RATIO)
                Else
                    TextBoxStepW.Text = Math.Round(Val(TextBoxStepH.Text) * SCREEN1_RATIO)

                End If
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub ComboBoxStepTemplate_SelectedValueChanged(sender As Object, e As EventArgs) Handles ComboBoxStepTemplate.SelectedValueChanged

        Dim WW As WoW_Session

        Dim Template As String

        Try

            WW = New WoW_Session

            If ComboBoxStepWindow.Text = "Full Screen" Then
                WW.IsWindowed = False
            Else
                WW.IsWindowed = True
            End If


            If ComboBoxStepWindow.Text = "Windowed Screen 2" Then
                WW.Screen = 2
            Else
                WW.Screen = 1
            End If

            Select Case ComboBoxStepTemplate.Text

                Case "1/4 Top Left"
                    Template = "TopLeft"

                Case "1/4 Top Right"
                    Template = "TopRight"

                Case "1/4 Bottom Left"
                    Template = "BottomLeft"


                Case "1/4 Bottom Right"
                    Template = "BottomRight"

                Case Else

            End Select

            WW.Set_WoW_Window(Template)

            TextBoxStepX.Text = WW.WindowX
            TextBoxStepY.Text = WW.WindowY
            TextBoxStepW.Text = WW.WindowW
            TextBoxStepH.Text = WW.WindowH

            WW = Nothing

        Catch ex As Exception

        End Try


    End Sub


#End Region

    ' #####################################################################################################################"
#Region "Settings_Mgt"


    Private Sub ButtonWoWExe_Click(sender As Object, e As EventArgs) Handles ButtonWoWExe.Click
        Try
            Using dialog As New OpenFileDialog
                If dialog.ShowDialog() <> DialogResult.OK Then Return
                TextBoxWoWExe.Text = dialog.FileName
                SetWoWExe()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try

    End Sub

    Private Sub ButtonBCExe_Click(sender As Object, e As EventArgs) Handles ButtonBCExe.Click
        Try

            Using dialog As New OpenFileDialog
                If dialog.ShowDialog() <> DialogResult.OK Then Return
                TextBoxBCExe.Text = dialog.FileName
                SetWoWExe()
            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try

    End Sub

    Private Sub ButtonTurtleExe_Click(sender As Object, e As EventArgs) Handles ButtonTurtleExe.Click
        Try

            Using dialog As New OpenFileDialog
                If dialog.ShowDialog() <> DialogResult.OK Then Return
                TextBoxTurtlexe.Text = dialog.FileName
                SetWoWExe()
            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub



    Private Sub SetWoWExe()

        Dim OK1 As Boolean
        Dim OK2 As Boolean
        Dim OK3 As Boolean

        Try

            'If TextBoxWoWExe.Text = "" And TextBoxBCExe.Text = "" Then
            ' strMessage = "Your WoW.exe file is not located! Please do this in the Settings."
            ' MessageBox.Show(strMessage)
            'LogIt(strMessage, Color.DarkRed)
            ' Exit Sub
            ' End If

            If TextBoxWoWExe.Text <> "" Then

                VANILLA_EXE = TextBoxWoWExe.Text

                If File.Exists(VANILLA_EXE) Then
                    Set_Param("VANILLA_EXE", VANILLA_EXE)
                    VANILLA_DIR = Path.GetDirectoryName(VANILLA_EXE)
                    OK1 = True
                Else
                    LogIt("Vanilla EXE path seems invalid !", Color.DarkRed)
                End If

            End If

            If TextBoxBCExe.Text <> "" Then

                BC_EXE = TextBoxBCExe.Text

                If File.Exists(BC_EXE) Then
                    Set_Param("BC_EXE", BC_EXE)
                    BC_DIR = Path.GetDirectoryName(BC_EXE)
                    OK2 = True
                Else
                    LogIt("BC EXE path seems invalid !", Color.DarkRed)
                End If

            End If

            If TextBoxTurtlexe.Text <> "" Then

                TURTLE_EXE = TextBoxTurtlexe.Text

                If File.Exists(TURTLE_EXE) Then
                    Set_Param("TURTLE_EXE", TURTLE_EXE)
                    TURTLE_DIR = Path.GetDirectoryName(TURTLE_EXE)
                    OK3 = True
                Else
                    LogIt("Turtle EXE path seems invalid !", Color.DarkRed)
                End If

            End If



            If OK1 Or OK2 Or ok3 Then
                LogIt("Game path defined", Color.DarkGreen)
            End If

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try

    End Sub

    Private Sub TextBoxWoWExe_TextChanged(sender As Object, e As EventArgs) Handles TextBoxWoWExe.Validated
        Try
            SetWoWExe()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub TextBoxWait_TextChanged(sender As Object, e As EventArgs) Handles TextBoxWaitLaunch.Validated
        Dim intVal As Integer

        Try

            intVal = CInt(TextBoxWaitLaunch.Text)

            If intVal Then

            Else
                intVal = 3000
                TextBoxWaitLaunch.Text = intVal
            End If

            WAIT_LOAD = intVal
            Set_Param("Wait_Load", WAIT_LOAD)
            LogIt("Waiting time defined to " & WAIT_LOAD / 1000 & " seconds", Color.DarkGreen)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try


    End Sub

    Private Sub TextBoxWaitLogin_TextChanged(sender As Object, e As EventArgs) Handles TextBoxWaitLogin.Validated
        Dim intVal As Integer

        Try

            intVal = CInt(TextBoxWaitLogin.Text)

            If intVal Then

            Else
                intVal = 3000
                TextBoxWaitLogin.Text = intVal
            End If

            WAIT_LOGIN = intVal
            Set_Param("Wait_Login", WAIT_LOGIN)
            LogIt("Waiting time defined to " & WAIT_LOAD / 1000 & " seconds", Color.DarkGreen)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try


    End Sub


    Private Sub CheckBoxCrypt_Validated(sender As Object, e As EventArgs) Handles CheckBoxCrypt.CheckedChanged


        Try

            If CheckBoxCrypt.Checked Then
                TSSbuttonKey.Visible = True
                PasswordForm.Show()

            Else
                ISCRYPT = False
                Set_Param("IsCrypt", "0")
                ' LogIt("Passwords won't be encrypted")
                TSSbuttonKey.Visible = False
                LabelPassWarning.Visible = True

            End If

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try


    End Sub

    Private Sub TextBoxSettingsGuildName_Validated(sender As Object, e As EventArgs) Handles TextBoxSettingsGuildName.Validated

        Try
            If Not IsMainFormLoaded Then Exit Sub
            GUILD_NAME = TextBoxSettingsGuildName.Text
            Set_Param("GuildName", GUILD_NAME)
            LabelOverviewGuildName.Text = GUILD_NAME
            LogIt("Guild Name changed", Color.DarkGreen)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub CheckBoxSettingsGuildLinks_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxSettingsGuildLinks.CheckedChanged


        Try
            If Not IsMainFormLoaded Then Exit Sub
            If CheckBoxSettingsGuildLinks.Checked Then
                GUILD_LINKS = True
                Set_Param("GuildLinks", "1")
            Else
                GUILD_LINKS = False
                Set_Param("GuildLinks", "0")
            End If

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try


    End Sub


    Private Sub ComboBoxConfigTrayAction_Load()

        Try

            Dim comboSource As New Dictionary(Of Integer, String)()
            comboSource.Add(0, "Full Screen")

            If NBR_MONITORS = 1 Then
                comboSource.Add(1, "Windowed")
            Else
                comboSource.Add(1, "Windowed Screen 1")
                comboSource.Add(2, "Windowed Screen 2")
            End If

            ComboBoxConfigTrayAction.DataSource = New BindingSource(comboSource, Nothing)
            ComboBoxConfigTrayAction.DisplayMember = "Value"
            ComboBoxConfigTrayAction.ValueMember = "Key"

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try
    End Sub

    Private Sub ComboBoxConfigTrayAction_SelectedValueChanged(sender As Object, e As EventArgs) Handles ComboBoxConfigTrayAction.SelectedValueChanged
        Dim intVal As Integer

        Try
            If Not IsMainFormLoaded Then Exit Sub

            intVal = DirectCast(ComboBoxConfigTrayAction.SelectedItem, KeyValuePair(Of Integer, String)).Key

            TRAY_DEFAULT_WINDOW = intVal
            Set_Param("Default_Window", TRAY_DEFAULT_WINDOW)
            ' LogIt("Waiting time defined to " & WAIT_LOAD / 1000 & " seconds", Color.DarkGreen)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub ComboBoxConfigTrayCharIcon_SelectedValueChanged(sender As Object, e As EventArgs) Handles ComboBoxConfigTrayCharIcon.SelectedValueChanged
        Dim strIcon As String

        Try
            If Not IsMainFormLoaded Then Exit Sub

            strIcon = CStr(ComboBoxConfigTrayCharIcon.Text)

            Select Case strIcon
                Case "Race"
                Case "Class"

                Case Else
                    strIcon = "Race"
                    ComboBoxConfigTrayCharIcon.Text = "Race"

            End Select

            CHAR_IMAGE = strIcon

            Set_Param("Char_Image", CHAR_IMAGE)
            ' LogIt("Waiting time defined to " & WAIT_LOAD / 1000 & " seconds", Color.DarkGreen)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub




    Private Sub CheckBoxCOnfigTrayKill_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxCOnfigTrayKill.CheckedChanged
        Try
            If Not IsMainFormLoaded Then Exit Sub
            If CheckBoxCOnfigTrayKill.Checked Then
                TRAY_SHOW_KILL = True
                Set_Param("ShowKill", "1")
            Else
                TRAY_SHOW_KILL = False
                Set_Param("ShowKill", "0")
            End If

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub CheckBoxConfigShowTaskBar_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxConfigShowTaskBar.CheckedChanged
        Try
            If Not IsMainFormLoaded Then Exit Sub
            If CheckBoxConfigShowTaskBar.Checked Then
                SHOW_TASKBAR = True
                Set_Param("ShowTaskBar", "1")
            Else
                SHOW_TASKBAR = False
                Set_Param("ShowTaskBar", "0")
            End If

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub


    Private Sub CheckBoxCOnfigTrayFolders_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxCOnfigTrayFolders.CheckedChanged
        Try
            If Not IsMainFormLoaded Then Exit Sub
            If CheckBoxCOnfigTrayFolders.Checked Then
                TRAY_SHOW_FOLDER = True
                Set_Param("ShowFolder", "1")
            Else
                TRAY_SHOW_FOLDER = False
                Set_Param("ShowFolder", "0")
            End If

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub


    Private Sub CheckBoxCOnfigTraySub_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxCOnfigTraySub.CheckedChanged
        Try
            If Not IsMainFormLoaded Then Exit Sub
            If CheckBoxCOnfigTraySub.Checked Then
                TRAY_SHOW_SUB_WINDOWED = True
                Set_Param("ShowSubWindowed", "1")
            Else
                TRAY_SHOW_SUB_WINDOWED = False
                Set_Param("ShowSubWindowed", "0")
            End If

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub CheckBoxCOnfigTraySubSub_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxCOnfigTraySubSub.CheckedChanged
        Try
            If Not IsMainFormLoaded Then Exit Sub
            If CheckBoxCOnfigTraySubSub.Checked Then
                TRAY_SHOW_SUBSUB_WINDOWED = True
                Set_Param("ShowSubSubWindowed", "1")
            Else
                TRAY_SHOW_SUBSUB_WINDOWED = False
                Set_Param("ShowSubSubWindowed", "0")
            End If

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub


    Private Sub CheckBoxCOnfigTrayLinks_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxCOnfigTrayLinks.CheckedChanged
        Try
            If Not IsMainFormLoaded Then Exit Sub
            If CheckBoxCOnfigTrayLinks.Checked Then
                TRAY_SHOW_LINKS = True
                Set_Param("ShowLinks", "1")
            Else
                TRAY_SHOW_LINKS = False
                Set_Param("ShowLinks", "0")
            End If

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub






    Private Sub RadioButtonKillMode_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonKillModeHard.CheckedChanged, RadioButtonKillModeSoft.CheckedChanged
        Try
            If Not IsMainFormLoaded Then Exit Sub
            If RadioButtonKillModeSoft.Checked Then
                SOFT_KILL = True
                Set_Param("SoftKill", "1")
            Else
                SOFT_KILL = False
                Set_Param("SoftKill", "0")
            End If

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub CheckBoxMonitoring_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxMonitoring.CheckedChanged
        Try
            If Not IsMainFormLoaded Then Exit Sub
            If CheckBoxMonitoring.Checked Then
                MONITORING = True
                Set_Param("Monitoring", "1")
            Else
                MONITORING = False
                Set_Param("Monitoring", "0")
            End If

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub


    Private Sub LinkLabelExpandTV_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        Try
            TreeViewLauncher.ExpandAll()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub Label31_Click(sender As Object, e As EventArgs) Handles Label31.Click

    End Sub

    Private Sub TextBoxWaitDown_Validated(sender As Object, e As EventArgs) Handles TextBoxWaitDown.Validated
        Dim intVal As Integer

        Try

            intVal = CInt(TextBoxWaitDown.Text)

            If intVal Then

            Else
                intVal = 200
                TextBoxWaitDown.Text = intVal
            End If

            WAIT_DOWN = intVal
            Set_Param("Wait_Down", WAIT_DOWN)
            LogIt("Waiting time defined to " & WAIT_DOWN / 1000 & " seconds", Color.DarkGreen)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub



    Private Sub LinkLabelJambon_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        Try
            System.Diagnostics.Process.Start("http://jambonetdragon.xooit.be/index.php")
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub LinkLabelRaidstats_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs)
        Try
            System.Diagnostics.Process.Start("http://realmplayers.com/RaidStats/RaidList.aspx?realm=NG&Guild=Jambon%20et%20Dragon")
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub PictureBoxPaypal_Click(sender As Object, e As EventArgs) Handles PictureBoxPaypal.Click
        Try
            System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=AJUA3SKJEGEQC&lc=US&item_name=VanillaLauncher&currency_code=EUR&bn=PP%2dDonationsBF%3abtn_donateCC_LG%2egif%3aNonHosted")
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub RadioButtonCharAlliance_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonCharHorde.CheckedChanged, RadioButtonCharAlliance.CheckedChanged
        ComboBoxCharClass_Load()
        ComboBoxCharRace_Load()
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) 

    End Sub

    Private Sub TableLayoutPanel1_Paint(sender As Object, e As PaintEventArgs) Handles TableLayoutPanel1.Paint

    End Sub

    Private Sub Label40_Click(sender As Object, e As EventArgs) Handles Label40.Click

    End Sub

    Private Sub TextBox2_TextChanged(sender As Object, e As EventArgs) Handles TextBox2.TextChanged

    End Sub

    Private Sub LabelPassWarning_Click(sender As Object, e As EventArgs) Handles LabelPassWarning.Click

    End Sub

    Private Sub ComboView_SelectedValueChanged(sender As Object, e As EventArgs) Handles ComboView.SelectedValueChanged
        TreeViewLauncher_Load()
    End Sub

    Private Sub CheckBoxFavsOnly_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxFavsOnly.CheckedChanged
        TreeViewLauncher_Load()
    End Sub

    Private Sub CheckExpand_CheckedChanged(sender As Object, e As EventArgs) Handles CheckExpand.CheckedChanged
        TreeViewLauncher_Load()
    End Sub



#End Region


End Class
