Public Class ProcessResult
    Private _resultCode As Integer = -9999
    Private _msg As List(Of String)
    Private _errors As Dictionary(Of String, String)
    Private _returnedValues As Hashtable

    Public Property ResultCode As Integer
        Get
            Return _resultCode
        End Get
        Set(value As Integer)
            _resultCode = value
        End Set
    End Property

    Public Property Msgs As List(Of String)
        Get
            If _msg Is Nothing Then
                _msg = New List(Of String)
            End If
            Return _msg
        End Get
        Set(value As List(Of String))
            _msg = value
        End Set
    End Property

    Public Property Errors As Dictionary(Of String, String)
        Get
            If _errors Is Nothing Then
                _errors = New Dictionary(Of String, String)
            End If
            Return _errors
        End Get
        Set(value As Dictionary(Of String, String))
            _errors = value
        End Set
    End Property

    Public Property ReturnedValues As Hashtable
        Get
            If _returnedValues Is Nothing Then
                _returnedValues = New Hashtable
            End If
            Return _returnedValues
        End Get
        Set(value As Hashtable)
            _returnedValues = value
        End Set
    End Property


End Class
