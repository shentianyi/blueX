Public Class ProcessData
    Private _uid As String
    Private _data As Hashtable

    Public Property UID As String
        Get
            Return _uid
        End Get
        Set(value As String)
            _uid = value
        End Set
    End Property

    Public Property Data As Hashtable
        Get
            If _data Is Nothing Then
                _data = New Hashtable
            End If
            Return _data
        End Get
        Set(value As Hashtable)
            _data = value
        End Set
    End Property
End Class
