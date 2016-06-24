Public NotInheritable Class ELabelController
    Shared m_instance As ELabelController
    Shared ReadOnly padlock As New Object()
    Private Sub New()
    End Sub

    Public Shared ReadOnly Property Instance() As ELabelController
        Get
            SyncLock padlock
                If m_instance Is Nothing Then
                    m_instance = New ELabelController()
                End If
                Return m_instance
            End SyncLock
        End Get
    End Property

    Public Shared Function TurnOnLabel(labelId As String) As Integer


    End Function

    Public Shared Function TurnOffLabel(labelId As String) As Integer

    End Function

    Public Shared Function TurnOfAll() As Integer

    End Function
End Class
