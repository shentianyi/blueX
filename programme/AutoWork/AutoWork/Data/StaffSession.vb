Public Class StaffSession
    Private Shared instance As StaffSession

    Private _isLogin As Boolean = False
    Private _workstation As WorkStation
    Public Property WorkStation As WorkStation
        Get
            Return _workstation
        End Get
        Set(value As WorkStation)
            _workstation = value
        End Set
    End Property
    Public Property IsLogin As Boolean
        Get
            Return _isLogin
        End Get
        Set(value As Boolean)
            _isLogin = value
        End Set
    End Property


    Public Shared Sub LogOut()
        Dim Session As StaffSession = StaffSession.GetInstance
        Session.StaffId = Nothing
        Session.StationID = Nothing
        Session.IsLogin = False
        Session.WorkStation = Nothing

    End Sub


    Public Shared Sub Login(ByVal usrName As String, ByVal workstation As String)
        Dim Session As StaffSession = StaffSession.GetInstance
        Session.StationID = workstation
        Session.StaffId = usrName
        Session.IsLogin = True
        Session.StartTime = Now
    End Sub

    Private Sub New()

    End Sub

    Private Shared threadLocker As New Object
    Public Shared Function GetInstance() As StaffSession
        If instance Is Nothing Then
            SyncLock threadLocker
                If instance Is Nothing Then
                    instance = New StaffSession
                End If
            End SyncLock
        End If
        Return instance
    End Function

    Private _staffId As String
    Public Property StaffId As String
        Get
            Return _staffId
        End Get
        Set(value As String)
            _staffId = value
        End Set
    End Property

    Private _stationId As String
    Public Property StationID As String
        Get
            Return _stationId
        End Get
        Set(value As String)
            _stationId = value
        End Set
    End Property

    Private _startTime As DateTime
    Public Property StartTime As DateTime
        Get
            Return _startTime

        End Get
        Set(value As DateTime)
            _startTime = value
        End Set
    End Property

End Class
