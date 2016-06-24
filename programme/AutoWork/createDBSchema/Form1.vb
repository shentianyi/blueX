
Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim autowork As AutoWorkDataContext = New AutoWorkDataContext("Data Source=SVCNSKSK06;Initial Catalog=AutoWork;Persist Security Info=True;User ID=sa;Password=Leoni2000")
        If Not autowork.DatabaseExists Then
            autowork.CreateDatabase()
            MsgBox("完成")
        End If
    End Sub
End Class
