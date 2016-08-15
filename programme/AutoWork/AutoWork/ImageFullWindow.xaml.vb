Public Class ImageFullWindow
    Dim parentWindow As Working

    Public Sub New()
        InitializeComponent()
    End Sub


    Public Sub New(parentWindow As Working)
        InitializeComponent()
        Me.parentWindow = parentWindow
        Try
            Me.image.Source = Me.parentWindow.image_wi.Source
        Catch
        End Try
    End Sub
End Class
