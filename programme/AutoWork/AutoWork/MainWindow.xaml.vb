Imports System.Windows.Threading

Class MainWindow
    Private WithEvents timer As DispatcherTimer


    Private _milisecond As Integer



    Private Sub button1_Click(sender As Object, e As RoutedEventArgs) Handles button1.Click
        minute.Content = "00"
        seconds.Content = "00"
        miliseconds.Content = "000"
        playmedia.Position = New TimeSpan(0)
        playmedia.Play()
        _milisecond = 0
        timer.Start()
    End Sub

    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        timer = New DispatcherTimer
        timer.Interval = New TimeSpan(10000000)

    End Sub

    Private Sub dispatcherTimer_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles timer.Tick

        timer.Stop()
        _milisecond = _milisecond + 1
        seconds.Content = CType((_milisecond), Integer)
        timer.Start()
    End Sub




End Class
