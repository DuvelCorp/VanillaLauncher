Imports System.Data.SQLite

Public Class PasswordForm
    Private Sub ButtonOK_Click(sender As Object, e As EventArgs) Handles ButtonOK.Click
        '    Dim objConn As SQLiteConnection
        '    Dim objCommand As SQLiteCommand
        '   Dim objReader As SQLiteDataReader
        '  Dim cnt As Integer

        Try

            If TextBoxKey.Text <> "" Then


                SKEY = New Security.SecureString

                For Each unChar As Char In TextBoxKey.Text
                    SKEY.AppendChar(unChar)
                Next

                SKEY.MakeReadOnly()
                ISCRYPT = True



                ' Exit if formload
                If Not IsMainFormLoaded Then
                    Exit Sub
                End If


                Set_Param("IsCrypt", "1")
                Call MainForm.MainForm_Reload()

                'objConn = New SQLiteConnection(CONSTRING & "New=True;")

                'Using (objConn)
                '    objConn.Open()

                '    ' Reset all passwords
                '    objCommand = objConn.CreateCommand()
                '    objCommand.CommandText = "UPDATE account SET AccountPassword='';"
                '    objCommand.ExecuteNonQuery()

                '    ' Count the number of passwords reset

                '    objCommand = objConn.CreateCommand()
                '    objCommand.CommandText = "SELECT changes() AS cnt FROM account"
                '    objReader = objCommand.ExecuteReader()

                '    While (objReader.Read())
                '        cnt = objReader("cnt")
                '    End While

                '    If cnt > 0 Then
                '        LogIt("The passwords have been reset on " & cnt & " account(s). You should reintroduce them !", Color.Blue)
                '    End If

                '    DataGridViewAccounts_Load()
                'End Using

            End If

            Me.Close()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click
        Try
            Me.Close()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub PasswordForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            TextBoxKey.Focus()
        Catch ex As Exception

        End Try


    End Sub
End Class