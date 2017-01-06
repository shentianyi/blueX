
Imports AutoWork.MsgLevel
Imports AutoWork.MsgDialog

Partial Public Class Login
    Inherits Window
    Public Sub New()
        InitializeComponent()
    End Sub

    Public Shared session As StaffSession



    Private Sub confirm()
        If String.IsNullOrEmpty(textBox_operator.Text) = False And String.IsNullOrEmpty(textBox_station.Text) = False Then
            '验证StaffID和work station 是否存在
            Dim db As AutoWorkDataContext = New AutoWorkDataContext
            Dim workstation As WorkStation = db.WorkStations.SingleOrDefault(Function(c) String.Compare(c.workstationId, textBox_station.Text, True) = 0)
            If workstation Is Nothing Then
                MsgBox("扫描入的操作台" & textBox_station.Text & "不存在")
                init()
                Exit Sub
            End If

            StaffSession.Login(textBox_operator.Text, textBox_station.Text)
            StaffSession.GetInstance.WorkStation = workstation

            '' 
            init()

            Dim orderWindow As New WaitOrder()
            orderWindow.Show()

            Me.Close()

        Else
            MsgBox("请输入用户名和操作台号")
        End If
    End Sub

    Private Sub button_confirm_Click(sender As Object, e As RoutedEventArgs) Handles button_confirm.Click
        confirm()
    End Sub
    Private Sub init()
        textBox_operator.Text = ""
        textBox_station.Text = ""
        textBox_station.Focus()
    End Sub

    Private Sub Login_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        '   InitializeComponent()
        init()
    End Sub

    Private Sub textBox_operator_keyup(sender As Object, e As KeyEventArgs) Handles textBox_operator.KeyUp
        If e.Key = Key.Enter Then
            confirm()

        End If
    End Sub

    Private Sub textBox_operator_TextChanged(sender As Object, e As TextChangedEventArgs) Handles textBox_operator.TextChanged

    End Sub

    Private Sub textBox_station_PreviewTextInput(sender As Object, e As TextCompositionEventArgs) Handles textBox_station.PreviewTextInput

    End Sub

    Private Sub textBox_station_keyup(sender As Object, e As KeyEventArgs) Handles textBox_station.KeyUp
        If e.Key = Key.Enter Then
            textBox_operator.Focus()
        End If
    End Sub

    Private Sub textBox_station_TextChanged(sender As Object, e As TextChangedEventArgs) Handles textBox_station.TextChanged

    End Sub

    Private Sub button_Click(sender As Object, e As RoutedEventArgs) Handles button.Click
        Dim full As ImageFullWindow = New ImageFullWindow(Me)
        full.Show()
    End Sub

    Private Sub button1_Click(sender As Object, e As RoutedEventArgs) Handles button1.Click
        Dim msg As String = "上一次作业指导书为:123" & ", 本次为：" & "456"
        CMsgDlg(MsgLevel.Warning, msg, True, Nothing, My.Settings.SameWIColseTime).ShowDialog()
    End Sub
End Class
