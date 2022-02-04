Imports System.IO

Public Class InitForm

    Shared Sub Main()
        Dim res1 As Point
        Dim res2 As Point

        Try

            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)


            '  LogIt("Starting...")


            ' First install
            If File.Exists("VanillaLauncher.db") Then
                ' LogIt("Database VanillaLauncher.db found.")
            Else
                'LogIt("Database not found, creating it.")
                CreateDatabase()
            End If

            ' Retrieve application parameters
            Get_Params()

            ' Change this if we deploy a new version !
            ' If DB_VERSION < 1 Then
            '  LogIt("Database version changed, upgrading it.")
            '  UpgradeDatabase()
            'End If

            TOTAL_RAM = My.Computer.Info.TotalPhysicalMemory
            'LogIt("Computer RAM : " & TOTAL_RAM)

            NBR_MONITORS = Screen.AllScreens.Length
            ' LogIt("Monitors detected : " & NBR_MONITORS)

            If NBR_MONITORS = 1 Then
                res1 = GetAllScreensResolution()
                SCREEN1_RATIO = res1.X / res1.Y
                'LogIt("Monitor Ratio : " & res1.X & " / " & res1.Y)
            ElseIf NBR_MONITORS = 2 Then
                res1 = GetPrimaryScreenResolution()
                res2 = GetSecondaryScreenResolution()
                SCREEN1_RATIO = res1.X / res1.Y
                SCREEN2_RATIO = res2.X / res2.Y
                '  LogIt("Monitor 1 Ratio : " & res1.X & " / " & res1.Y)
                ' LogIt("Monitor 2 Ratio : " & res2.X & " / " & res2.Y)
            Else ' more than 2 screens : unmanaged yet
                '  LogIt("You have more than 2 monitors on this computer, this is not managed by the application")
                res1 = GetAllScreensResolution()
                SCREEN1_RATIO = res1.X / res1.Y
            End If

            Application.Run(New MainForm)  'Specify the startup form
            InitForm.Dispose()


        Catch ex As Exception

        End Try
    End Sub


    Private Sub Init_form()


    End Sub

    Private Sub LogIt(strText As String)

        Try
            TextBoxLog.Text = TextBoxLog.Text & Format(Now, "hh:nn:ss") & " " & strText & vbCrLf
        Catch ex As Exception

        End Try
    End Sub
End Class