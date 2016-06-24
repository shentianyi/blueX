Public Class Rework
    Private Sub combox_reason_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles combox_reason.SelectionChanged

    End Sub

    Private Sub button_Copy_Click(sender As Object, e As RoutedEventArgs) Handles button_Copy.Click
        Me.DialogResult = False
        Me.Close()
    End Sub

    Private Sub button_Click(sender As Object, e As RoutedEventArgs) Handles button.Click
        Me.DialogResult = True
        Me.Close()
    End Sub
End Class
