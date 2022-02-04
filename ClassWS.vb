Imports System.Data.SQLite
Imports System.IO

Public Class WoW_Session

    Private _SessionName As String

    Private _WoWversionID As Integer

    Private _ServerID As Integer
    Private _ServerName As String
    Private _ServerAddress As String

    Private _RealmID As Integer
    Private _RealmName As String

    Private _AccountID As Integer
    Private _AccountName As String
    Private _AccountPassword As String

    Private _CharID As Integer
    Private _CharIndex As Integer
    Private _CharName As String
    Private _CharClass As String
    Private _CharRace As String
    Private _CharFaction As String
    Private _CharColor As String
    Private _CharLevel As Integer

    Private _Screen As Integer
    Private _IsWindowed As Boolean

    Private _ProcID As Integer

    Private _Process As Process
    Private _PerfCnt As PerformanceCounter
    Private _startSample As CounterSample

    Private _X As Integer
    Private _Y As Integer
    Private _W As Integer
    Private _H As Integer


    Public Sub New()

        Select Case TRAY_DEFAULT_WINDOW
            Case 1
                Me.Screen = 1
                Me.IsWindowed = True

            Case 2
                Me.Screen = 2
                Me.IsWindowed = True

            Case Else
                Me.Screen = 1
                Me.IsWindowed = False

        End Select

    End Sub

    Public Sub Dispose()
        If (Not (Me._PerfCnt) Is Nothing) Then
            Me._PerfCnt.Dispose()
        End If

    End Sub


    Public ReadOnly Property AccountID() As Integer
        Get
            Return _AccountID
        End Get


    End Property



    Public ReadOnly Property CharID() As Integer
        Get
            Return _CharID
        End Get

    End Property


    Public Property Screen() As Integer
        Get
            Return _Screen
        End Get

        Set(ByVal Value As Integer)
            _Screen = Value
        End Set

    End Property

    Public Property IsWindowed() As Boolean
        Get
            Return _IsWindowed
        End Get

        Set(ByVal Value As Boolean)
            _IsWindowed = Value
        End Set

    End Property



    Public Property WindowX() As Integer
        Get
            Return _X
        End Get

        Set(ByVal Value As Integer)
            _X = Value
        End Set

    End Property


    Public Property WindowY() As Integer
        Get
            Return _Y
        End Get

        Set(ByVal Value As Integer)
            _Y = Value
        End Set

    End Property

    Public Property WindowW() As Integer
        Get
            Return _W
        End Get

        Set(ByVal Value As Integer)
            _W = Value
        End Set

    End Property

    Public Property WindowH() As Integer
        Get
            Return _H
        End Get

        Set(ByVal Value As Integer)
            _H = Value
        End Set

    End Property

    Public ReadOnly Property SessionName() As String
        Get
            Return _SessionName
        End Get

        'Set(ByVal Value As String)
        '    wSessionName = Value
        'End Set

    End Property

    Public ReadOnly Property WoWversion() As Integer
        Get
            Return _WoWversionID
        End Get

        'Set(ByVal Value As Integer)
        '    wWoWversionID = Value
        'End Set

    End Property

    Public ReadOnly Property ProcID() As Integer
        Get
            Return _ProcID
        End Get

        ' read-only
        'Set(ByVal Value As Integer)
        '    wprocid = Value
        'End Set

    End Property

    Public ReadOnly Property RealmName() As String
        Get
            Return _RealmName
        End Get
    End Property

    Public ReadOnly Property AccountName() As String
        Get
            Return _AccountName
        End Get
    End Property

    Public ReadOnly Property CharName() As String
        Get
            Return _CharName
        End Get
    End Property


    Public Sub Start_WoW()

        Dim i As Integer
        Dim strEXE As String

        Try

            Cursor.Current = Cursors.WaitCursor

            If Me._AccountID = 0 Then Exit Sub

            If MainForm.WindowState = FormWindowState.Maximized Or MainForm.WindowState = FormWindowState.Normal Then
                MainForm.Timer1.Enabled = False
                MainForm.Overview_Init()

                If Me._CharID = 0 Then
                    MainForm.Overview_Account(Me._AccountID)
                Else
                    MainForm.Overview_Char(Me._CharID)
                End If
                MainForm.TabPageMain.BackColor = Color.DarkOrange
                MainForm.LabelOverviewProcessStarted.Text = "Starting..."
                MainForm.LabelOverviewProcessStarted.ForeColor = Color.DarkOrange
                MainForm.Refresh()
            End If

            Select Case Me._WoWversionID
                Case 2
                    strEXE = BC_EXE
                Case 3
                    strEXE = TURTLE_EXE
                Case Else
                    strEXE = VANILLA_EXE
            End Select


            If strEXE = "" Then
                MessageBox.Show("You have not located your WoW.exe ! Do that it the settings.")
                Exit Sub
            End If


            Me.Set_WoW_Config()
            Me.Set_WoW_RealmList()


            Me._ProcID = OpenApp(strEXE, Me._IsWindowed, WAIT_LOAD, Me._X, Me._Y, Me._W, Me._H)
            Me._Process = Process.GetProcessById(Me._ProcID)

            Me._SessionName = Me._RealmName & " -> " & Me._AccountName
            Me.Set_Perf_Counter()

            WOW_SESSIONS.Add(Me)


            If MainForm.WindowState = FormWindowState.Maximized Or MainForm.WindowState = FormWindowState.Normal Then
                MainForm.Check_Processes()
            End If


            ' Remove CAPS LOCK 


            Threading.Thread.Sleep(WAIT_LOAD)
            Me.SendKeys(Me._AccountName)
            Me.SendKeys("{TAB}")

            If ISCRYPT Then
                Me.SendKeys(PWD_Decrypt(Me._AccountPassword))
            Else
                Me.SendKeys(Me._AccountPassword)
            End If

            Me.SendKeys("{ENTER}")

            If Me._CharIndex > 0 Then

                Threading.Thread.Sleep(WAIT_LOGIN)

                For i = 1 To Me._CharIndex - 1
                    Me.SendKeys("{DOWN}")
                    Threading.Thread.Sleep(WAIT_DOWN)
                Next i
                Me.SendKeys("{ENTER}")

            End If


        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        Finally
            Cursor.Current = Cursors.Arrow
            If MainForm.WindowState = FormWindowState.Maximized Or MainForm.WindowState = FormWindowState.Normal Then
                MainForm.TabPageMain.BackColor = Color.FromArgb(255, 64, 64, 64)
                If MONITORING Then
                    MainForm.Timer1.Enabled = True
                Else
                    MainForm.Timer1.Enabled = False
                End If
            End If

        End Try

    End Sub


    Private Sub SendKeys(strKey As String)
        Try
            SetForegroundWindow(Me._Process.MainWindowHandle)
            ShowWindow(Me._Process.MainWindowHandle, SW_SHOWNORMAL)
            ShowWindow(Me._Process.MainWindowHandle, SW_RESTORE)

            If Control.IsKeyLocked(Keys.CapsLock) Then
                Call keybd_event(System.Windows.Forms.Keys.CapsLock, &H14, 1, 0)
                Call keybd_event(System.Windows.Forms.Keys.CapsLock, &H14, 3, 0)
            End If

            My.Computer.Keyboard.SendKeys(strKey, True)

        Catch ex As Exception

        End Try

    End Sub

    Public Sub Set_AccountID(ID As Integer)
        Try
            Dim objConn = New SQLiteConnection(CONSTRING)
            Dim objCommand As SQLiteCommand
            Dim objReader As SQLiteDataReader
            Dim sql As String

            If ID = 0 Then Exit Sub

            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)

                objConn.Open()

                sql = " SELECT A.AccountName, A.AccountPassword, " &
              "        S.ServerID, S.ServerName, S.ServerRealmList,   " &
              "        R.RealmName, R.RealmID,  " &
              "        v.WoWversionID, v.WoWversionName     " &
              " FROM account a INNER JOIN realm r ON A.RealmID=r.RealmID " &
              " INNER JOIN server s on S.ServerID=r.ServerID " &
              " INNER JOIN wowversion v on S.wowversionID=v.wowversionID " &
              " WHERE a.AccountID = " & ID


                objCommand = objConn.CreateCommand()
                objCommand.CommandText = sql
                objReader = objCommand.ExecuteReader()

                While (objReader.Read())
                    _AccountID = ID
                    _AccountName = objReader("AccountName")
                    _AccountPassword = objReader("AccountPassword")
                    _RealmID = CInt(objReader("RealmID"))
                    _RealmName = objReader("RealmName")
                    _ServerID = CInt(objReader("ServerID"))
                    _ServerName = objReader("ServerName")
                    _ServerAddress = objReader("ServerRealmList")
                    _WoWversionID = CInt(objReader("WowversionID"))

                End While

            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try

    End Sub


    Public Sub Set_CharID(ID As Integer)
        Try
            Dim objConn = New SQLiteConnection(CONSTRING)
            Dim objCommand As SQLiteCommand
            Dim objReader As SQLiteDataReader
            Dim sql As String

            If ID = 0 Then Exit Sub


            objConn = New SQLiteConnection(CONSTRING & "New=True;")

            Using (objConn)

                objConn.Open()

                sql = " SELECT s.ServerID, s.ServerName, s.ServerRealmList, s.WoWversionID,  " &
                      "        r.RealmID, r.RealmName, r.RealmType, r.RealmLang,  " &
                      "        a.AccountID, a.AccountName, a.AccountPassword, a.AccountNote, " &
                      "        ra.RaceName, ra.RaceFaction, cl.ClassName, cl.ClassColor,  " &
                      "        c.CharID, c.CharName, c.CharIndex,  c.CharNote, C.CharLevel, C.CharGender, C.CharFaction  " &
                      " FROM character c " &
                      " INNER JOIN account a ON a.AccountID=c.AccountID " &
                      " LEFT JOIN class cl ON c.CLassID=cl.ClassID " &
                      " LEFT JOIN race ra ON c.RaceID=ra.RaceID " &
                      "  INNER JOIN realm r ON r.RealmID=a.RealmID " &
                      "  INNER JOIN server s ON s.ServerID=r.ServerID " &
                      " WHERE c.CharID=" & ID

                objCommand = objConn.CreateCommand()
                objCommand.CommandText = sql
                objReader = objCommand.ExecuteReader()

                While (objReader.Read())
                    _CharID = ID
                    _CharName = objReader("CharName")
                    _CharIndex = objReader("CharIndex")
                    _CharLevel = objReader("CharLevel")
                    _CharColor = objReader("ClassColor")
                    _CharRace = objReader("RaceName")
                    _CharClass = objReader("ClassName")
                    _CharFaction = objReader("RaceFaction")

                    _AccountID = objReader("AccountID")
                    _AccountName = objReader("AccountName")
                    _AccountPassword = objReader("AccountPassword")
                    _RealmID = CInt(objReader("RealmID"))
                    _RealmName = objReader("RealmName")
                    _ServerID = CInt(objReader("ServerID"))
                    _ServerName = objReader("ServerName")
                    _ServerAddress = objReader("ServerRealmList")
                    _WoWversionID = CInt(objReader("WowversionID"))

                End While

            End Using


        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub


    Private Sub Set_Perf_Counter()
        Try
            Dim instancename As String = Me.GetProcessInstanceName()
            Me._PerfCnt = New PerformanceCounter("Process", "% Processor Time", instancename, True)
            Me.Reset_Perf_Counter()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try


    End Sub


    ''' Resets the internal counter. All subsequent calls to GetCpuUtilization() will 
    ''' be relative to the point in time when you called ResetCounter(). This 
    ''' method can be call as often as necessary to get a new baseline for 
    ''' CPU utilization measurements.
    Public Sub Reset_Perf_Counter()
        Try
            Me._startSample = Me._PerfCnt.NextSample
            'Me._PerfCnt.BeginInit()
        Catch ex As Exception

        End Try

    End Sub



    Public Function GetCPU() As Double
        Try

            Return Me._PerfCnt.NextValue

            'Dim diffValue As Double = (curr.RawValue - Me._startSample.RawValue)
            'Dim diffTimestamp As Double = (curr.TimeStamp100nSec - Me._startSample.TimeStamp100nSec)
            'Dim usage As Double = ((diffValue / diffTimestamp) _
            '            * 100)
            'Return usage

        Catch ex As Exception

        End Try

    End Function



    Private Function GetProcessInstanceName() As String
        Dim cat As PerformanceCounterCategory = New PerformanceCounterCategory("Process")
        Dim instances() As String = cat.GetInstanceNames
        For Each instance As String In instances
            Dim cnt As PerformanceCounter = New PerformanceCounter("Process", "ID Process", instance, True)
            Dim val As Integer = CType(cnt.RawValue, Integer)
            If (val = Me._ProcID) Then
                Return instance
            End If

        Next
        Throw New Exception(("Could not find performance counter " + "instance name for current process. This is truly strange ..."))
    End Function


    Public Sub Set_WoW_Window(Optional Template As String = "")

        Dim res1 As Point
        Dim res2 As Point

        Try

            res1 = GetPrimaryScreenWorkingArea()
            res2 = GetSecondaryScreenWorkingArea()

            If Me._Screen = 2 Then
                Me._X = res1.X + 1
                Me._Y = 1
                Me._H = res1.Y
            Else
                Me._X = 1
                Me._Y = 1
                Me._H = res1.Y
            End If


            If Me._IsWindowed And Template <> "" Then

                Select Case Template

                    Case "TopLeft"

                        If Me._Screen = 2 Then
                            Me._X = res1.X + 1
                            Me._Y = 1
                            Me._H = Math.Floor(res2.Y / 2)
                        Else
                            Me._X = 1
                            Me._Y = 1
                            Me._H = Math.Floor(res1.Y / 2)
                        End If

                    Case "TopRight"
                        Me._H = res1.Y / 2

                        If Me._Screen = 2 Then
                            Me._X = res1.X + 1 + Math.Floor(res2.X / 2)
                            Me._Y = 1
                            Me._H = Math.Floor(res2.Y / 2)
                        Else
                            Me._X = 1 + Math.Floor(res1.X / 2)
                            Me._Y = 1
                            Me._H = Math.Floor(res1.Y / 2)
                        End If

                    Case "BottomLeft"

                        If Me._Screen = 2 Then
                            Me._X = res1.X + 1
                            Me._Y = 1 + Math.Floor(res2.Y / 2)
                            Me._H = Math.Floor(res2.Y / 2)
                        Else
                            Me._X = 1
                            Me._Y = 1 + Math.Floor(res1.Y / 2)
                            Me._H = Math.Floor(res1.Y / 2)
                        End If


                    Case "BottomRight"
                        If Me._Screen = 2 Then
                            Me._X = res1.X + 1 + Math.Floor(res2.X / 2)
                            Me._Y = 1 + Math.Floor(res2.Y / 2)
                            Me._H = Math.Floor(res2.Y / 2)
                        Else
                            Me._X = 1 + Math.Floor(res1.X / 2)
                            Me._Y = 1 + Math.Floor(res1.Y / 2)
                            Me._H = Math.Floor(res1.Y / 2)
                        End If

                    Case Else


                End Select

                If Me._Screen = 2 Then
                    Me._W = Me._H * SCREEN2_RATIO
                Else
                    Me._W = Me._H * SCREEN1_RATIO
                End If


            End If

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)

        End Try


    End Sub


    Private Sub Set_WoW_Config()

        Dim LineIN As String
        Dim LineOUT As String
        Dim Reader As StreamReader
        Dim Writer As StreamWriter

        Dim nbLines As Integer
        Dim WrittenFile As System.IO.FileInfo

        Dim InitialFilePath As String
        Dim NewFilePath As String

        Dim IsMaximizeFOund As Boolean
        Dim IsWindowFound As Boolean
        Dim IsCharIndexFound As Boolean
        Dim IsRealmNameFound As Boolean
        Dim IsRealmListFound As Boolean

        Dim MaximizeVal As Integer

        Dim strGamePath As String

        Const DQ As String = Chr(34)

        Try

            Select Case Me._WoWversionID
                Case 2
                    strGamePath = BC_DIR
                Case 3
                    strGamePath = TURTLE_DIR
                Case Else
                    strGamePath = VANILLA_DIR
            End Select

            LineOUT = ""

            InitialFilePath = strGamePath & "\WTF\Config.wtf"
            NewFilePath = "WoW_COnfig_Temp.wtf"

            WrittenFile = New System.IO.FileInfo(NewFilePath)

            If File.Exists(NewFilePath) Then
                WrittenFile.Attributes = IO.FileAttributes.Normal
            End If

            Reader = New StreamReader(InitialFilePath)
            Writer = New StreamWriter(NewFilePath, False)


            Do
                LineIN = Reader.ReadLine()

                LineOUT = LineIN

                If Strings.Left(LineIN, 13) = "SET realmList" Then
                    IsRealmListFound = True
                    LineOUT = "SET realmList " & DQ & Me._ServerAddress & DQ
                End If

                If Strings.Left(LineIN, 13) = "SET realmName" Then
                    IsRealmNameFound = True
                    LineOUT = "SET realmName " & DQ & Me._RealmName & DQ
                End If

                If Strings.Left(LineIN, 14) = "SET gxMaximize" Then

                    IsMaximizeFOund = True
                    MaximizeVal = CInt(Strings.Mid(17, 1))
                    If Me._IsWindowed Then
                        LineOUT = "SET gxMaximize ""0"""
                    Else
                        LineOUT = "SET gxMaximize ""1"""
                    End If
                End If

                If Strings.Left(LineIN, 12) = "SET gxWindow" Then
                    IsWindowFound = True
                    MaximizeVal = CInt(Strings.Mid(15, 1))
                    LineOUT = "SET gxWindow ""1"""
                End If


                If Me._CharID Then
                    If Strings.Left(LineIN, 22) = "SET lastCharacterIndex" Then
                        IsCharIndexFound = True
                        LineOUT = "SET lastCharacterIndex ""0"""
                    End If
                End If


                Writer.Write(LineOUT & vbCrLf)

                nbLines = nbLines + 1

            Loop Until LineIN Is Nothing

            If Not IsRealmListFound Then
                LineOUT = "SET realmList " & DQ & Me._ServerAddress & DQ
                Writer.Write(LineOUT & vbCrLf)
            End If

            If Not IsRealmNameFound Then
                LineOUT = "SET realmName " & DQ & Me._ServerName & DQ
                Writer.Write(LineOUT & vbCrLf)
            End If


            If Not IsMaximizeFOund Then
                If Me._IsWindowed Then
                    LineOUT = "SET gxMaximize ""0"""
                Else
                    LineOUT = "SET gxMaximize ""1"""
                End If
                Writer.Write(LineOUT & vbCrLf)
            End If

            If Not IsWindowFound Then
                '   If IsWindowed Then
                ' LineOUT = "SET gxWindow ""0"""
                '  Else
                LineOUT = "SET gxWindow ""1"""
                'End If
                Writer.Write(LineOUT & vbCrLf)
            End If

            If Me._CharID And Not IsCharIndexFound Then
                LineOUT = "SET lastCharacterIndex ""0"""
                Writer.Write(LineOUT & vbCrLf)
            End If

            Reader.Close()

            Writer.Close()

            FileCopy(NewFilePath, InitialFilePath)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try

    End Sub




    Private Sub Set_WoW_RealmList()

        Dim Writer As StreamWriter
        Dim WrittenFile As System.IO.FileInfo

        Dim InitialFilePath As String
        Dim NewFilePath As String

        Dim strGamePath As String

        Try

            Select Case Me._WoWversionID
                Case 2
                    strGamePath = BC_DIR
                Case 3
                    strGamePath = TURTLE_DIR
                Case Else
                    strGamePath = VANILLA_DIR
            End Select

            InitialFilePath = strGamePath & "\realmlist.wtf"
            NewFilePath = "realmlist_Temp.wtf"

            WrittenFile = New System.IO.FileInfo(NewFilePath)

            If File.Exists(NewFilePath) Then
                WrittenFile.Attributes = IO.FileAttributes.Normal
            End If

            Writer = New StreamWriter(NewFilePath, False)

            Writer.Write("SET realmList " & Me._ServerAddress & vbCrLf)

            Writer.Close()

            FileCopy(NewFilePath, InitialFilePath)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try

    End Sub






End Class
