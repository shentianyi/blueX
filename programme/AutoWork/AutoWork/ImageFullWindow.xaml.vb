Public Class ImageFullWindow
    Dim parentWindow As Working
    Dim parentL As Login

    Public Sub New()
        InitializeComponent()
    End Sub

    Public Sub New(parentWindow As Login)
        InitializeComponent()
        Me.parentL = parentWindow
        Try
            Me.image.Source = Me.parentL.image_wi.Source
        Catch
        End Try
    End Sub


    Public Sub New(parentWindow As Working)
        InitializeComponent()
        Me.parentWindow = parentWindow
        Try
            Me.image.Source = Me.parentWindow.image_wi.Source
        Catch
        End Try
    End Sub

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        Me.Close()
    End Sub
End Class
