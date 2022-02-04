Imports System.Runtime.InteropServices
Imports System.Diagnostics


Module WindowMgt


    Public Const SW_SHOWNORMAL As Integer = 1
    Public Const SW_SHOWMAXIMIZED As Integer = 3
    Public Const SW_RESTORE As Integer = 9

    <DllImport("user32.dll")>
    Public Function MoveWindow(ByVal hWnd As IntPtr, ByVal x As Integer, ByVal y As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal bRepaint As Boolean) As Boolean
    End Function

    <DllImport("user32.dll")>
    Public Function SetForegroundWindow(ByVal hWnd As IntPtr) As Boolean
    End Function

    <DllImport("user32.dll")>
    Public Function ShowWindow(ByVal hWnd As IntPtr, nCmdShow As Integer) As Boolean
    End Function


    <DllImport("user32.dll", SetLastError:=True)>
    Private Function GetWindowRect(ByVal hWnd As IntPtr, <Out()> ByRef lpRect As RECT) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <StructLayoutAttribute(LayoutKind.Sequential)>
    Private Structure RECT
        Public left As Integer
        Public top As Integer
        Public right As Integer
        Public bottom As Integer
    End Structure

    Public Function OpenApp(ByVal appName As String, Optional IsWindowed As Boolean = False, Optional intWait As Integer = 3000, Optional x As Integer = 0, Optional y As Integer = 0, Optional w As Integer = 0, Optional h As Integer = 0) As Long

        Dim proc As New System.Diagnostics.Process

        proc.StartInfo.FileName = appName
        proc.Start()

        proc.WaitForInputIdle(intWait)
        Try
            If SetForegroundWindow(proc.MainWindowHandle) And IsWindowed Then 'check if there is handle to the window

                Dim rec As RECT 'structure to get size and location 
                If Not proc.MainWindowHandle.Equals(IntPtr.Zero) Then

                    If GetWindowRect(proc.MainWindowHandle, rec) Then

                        If h = 0 Then h = rec.bottom - rec.top
                        If w = 0 Then w = rec.right - rec.left

                        If x = 0 Then x = CInt(SystemInformation.WorkingArea.Width / 3)
                        If y = 0 Then y = CInt(SystemInformation.WorkingArea.Height / 3)

                        'call MoveWindow ApI to move the windows
                        ' Threading.Thread.Sleep(10)
                        MoveWindow(proc.MainWindowHandle, x, y, w, h, True)
                    End If

                End If
            End If

            Return proc.Id

        Catch ex As Exception

        End Try
    End Function


End Module
