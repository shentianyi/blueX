Imports System.Threading
Imports System.Windows.Threading
Imports PLCLightCL
Imports PLCLightCL.Enum
Imports PLCLightCL.Light
Imports PLCLightCL.Model
Imports Repository




Public Class Working

    Private _orderNr As String
    Private _wi As WorkInstruction
    Private _wiRoutines As List(Of RoutineOnWorkInstructionDetails)
    Private _currentSeq As Integer = 0
    Private _svc As MainService = New MainService
    Private _currentRoutine As RoutineOnWorkInstructionDetails
    Private _lightController As ILightController
    Private WithEvents timer As DispatcherTimer
    Private _isprepareTime = True
    Private _prepareTime = 0
    Private _header As HeadMessage

    Public Sub New(headMsg As PLCLightCL.Model.HeadMessage, workInstructionId As String)

        ' 此调用是设计器所必需的。
        InitializeComponent()
        timer = New DispatcherTimer
        timer.Interval = New TimeSpan(10000000)

        Try
            _orderNr = headMsg.KSK
            _wi = _svc.GetWIforOrderOnWorkStation(workInstructionId)
            _wiRoutines = _svc.GetRoutinesofWi(_wi.id)
            _wiRoutines = (From a In _wiRoutines Order By a.sequence).ToList
            _currentSeq = _wiRoutines(0).sequence
            _header = headMsg
        Catch ex As Exception
            MsgBox("开始流程失败，请通知管理人员", MsgBoxStyle.Critical)
        End Try


    End Sub

    Private Sub MoveToStep(ByVal seq As Integer)

        If seq < _currentSeq Then

        Else
            Me.label_actual.Content = 0
            timeConsume = 0
            _prepareTime = 0
            _isprepareTime = True
        End If


        If Me._wiRoutines IsNot Nothing Then
            If _wiRoutines.Count > seq Then
                Try
                    _lightController.Play(LightCmdType.ALL_OFF)
                Catch ex As Exception

                End Try

                _currentSeq = seq
                _currentRoutine = _wiRoutines(seq)
                BindData()
            End If
        End If


    End Sub

    Private Sub BindData()
        Dim labels As List(Of String) = _svc.GetLabelAddressForRoutineOnWorkStation(_currentRoutine.routineId, StaffSession.GetInstance.StationID)
        Dim intLabels As List(Of Integer) = New List(Of Integer)
        For Each strId As String In labels
            intLabels.Add(CType(strId, Integer))
        Next
        Try
            _lightController.Play(LightCmdType.ALL_OFF_BEFORE_ON, intLabels)
        Catch ex As Exception

        End Try

        Me.label_steps.Content = String.Format("Step {0} / {1}", _currentSeq + 1, _wiRoutines.Count)
        Me.label_normalhour.Content = _currentRoutine.normalTime
        Dim imgPath As String = System.IO.Path.Combine(My.Application.Info.DirectoryPath, "Routines\Img\" & _currentRoutine.picture)
        Dim videoPath As String = System.IO.Path.Combine(My.Application.Info.DirectoryPath, "Routines\Video\" & _currentRoutine.video)

        Me.image_wi.Source = New BitmapImage(New Uri(imgPath, UriKind.RelativeOrAbsolute))
        Me.mediaplay.Source = New Uri(videoPath, UriKind.Absolute)
        mediaplay.Play()
        timer.Start()

    End Sub

    Private Sub button_nextroutine_Click(sender As Object, e As RoutedEventArgs) Handles button_nextroutine.Click
        If _currentSeq < _wiRoutines.Count - 1 Then
            MoveToStep(_currentSeq + 1)
        End If
        Me.button_hide.Focus()
    End Sub

    Private Sub button_previousroutine_Click(sender As Object, e As RoutedEventArgs) Handles button_previousroutine.Click
        If _currentSeq > 0 Then
            MoveToStep(_currentSeq - 1)
        End If
        Me.button_hide.Focus()
    End Sub

    Private Sub button_text_Click(sender As Object, e As RoutedEventArgs) Handles button_text.Click
        Me.mediaplay.Visibility = Visibility.Collapsed
        Me.image_wi.Visibility = Visibility.Visible
        Me.button_hide.Focus()
    End Sub

    Private Sub button_video_Click(sender As Object, e As RoutedEventArgs) Handles button_video.Click
        Me.image_wi.Visibility = Visibility.Collapsed
        Me.mediaplay.Visibility = Visibility.Visible
        mediaplay.Position = New TimeSpan(0)
        mediaplay.Play()
        Me.button_hide.Focus()
    End Sub

    Private Sub button_finish_Click(sender As Object, e As RoutedEventArgs) Handles button_finish.Click
        If _currentRoutine.sequence <> _wiRoutines.Count - 1 Then
            MsgBox("流程还未结束", MsgBoxStyle.Information)
            Me.button_hide.Focus()
        Else
            Try
                Dim orderrepo As Repository(Of Order) = New Repository(Of [Order])(New DataContext(GlobalConfigs.DbConnStr))
                Dim order As Order = orderrepo.Single(Function(c) String.Compare(c.orderId, _header.KSK, True) = 0)
                order.status = OrderStatus.Finish
                orderrepo.SaveAll()
                ''根据预设，是否要跟LEPS交互
                If StaffSession.GetInstance.WorkStation.needEnd = True Then
                    Dim lepsCl As LEPSController = New LEPSController(GlobalConfigs.LepsDb)
                    lepsCl.AKBasicModule(StaffSession.GetInstance.WorkStation.prodLine, StaffSession.GetInstance.WorkStation.lepsWorkstation, _header.KSK, _wi.id)
                    lepsCl.CompleteHarness(_header.Board, StaffSession.GetInstance.StationID, _header.KSK)
                End If

                MsgBox("流程结束", MsgBoxStyle.Information)
            Catch ex As Exception
                MsgBox("与LEPS通讯时发生错误", MsgBoxStyle.Critical)
            End Try

            Me.Close()
        End If
    End Sub

    Private timeConsume = 0
    Private Sub timer_Tick(sender As Object, e As EventArgs) Handles timer.Tick
        timer.Stop()
        If _isprepareTime = True Then
            _prepareTime = _prepareTime + 1
            If _prepareTime > 4 Then
                _isprepareTime = False
            End If
        Else
            timeConsume = timeConsume + 1
            label_actual.Content = timeConsume
        End If

        timer.Start()
    End Sub

    Private Sub Working_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Try
            _lightController = New RamLightController(My.Settings.com)
        Catch ex As Exception

        End Try
        Me.label_cartNr.Content = _header.Board
        Me.label_ordernr.Content = _header.KSK
        Me.laebl_wiid.Content = _wi.id
        Me.station_name.Content = StaffSession.GetInstance.StationID
        MoveToStep(0)
    End Sub

    Private Sub Working_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.PreviewKeyDown

        If e.Key = Key.Space Then
            MoveToStep(_currentSeq + 1)
        End If
    End Sub

    Private Sub button_rework_Click(sender As Object, e As RoutedEventArgs) Handles button_rework.Click
        Dim rework As New Rework
        If rework.ShowDialog = True Then
            Me.Close()
        End If
    End Sub


    Private Sub Window_Closing(sender As Object, e As ComponentModel.CancelEventArgs)
        Try
            _lightController.Play(LightCmdType.ALL_OFF)
            _lightController.Close()
        Catch ex As Exception

        End Try
    End Sub
End Class
