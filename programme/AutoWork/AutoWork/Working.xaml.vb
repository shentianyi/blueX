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
    'Private _lightController As ILightController
    Private _lightHelper As LightHelper

    Private WithEvents timer As DispatcherTimer
    Private _isprepareTime = True
    Private _prepareTime = 0
    Private _header As HeadMessage
    Private _lepsWiId As String

    Dim images As List(Of String) = New List(Of String)
    Dim currentImageIndex As Integer = 0


    Public Sub New(headMsg As PLCLightCL.Model.HeadMessage, workInstructionId As String)

        ' 此调用是设计器所必需的。
        InitializeComponent()
        timer = New DispatcherTimer
        timer.Interval = New TimeSpan(10000000)

        Try
            _orderNr = headMsg.KSK
            _lepsWiId = workInstructionId
            Dim dc As DataContext = New DataContext(GlobalConfigs.DbConnStr)
            Dim awWI As LepsWorkInstructionOnAW = dc.Context.GetTable(Of LepsWorkInstructionOnAW).Where(Function(l) l.workstationId.Equals(StaffSession.GetInstance.WorkStation.workstationId) And l.lepsWI.Equals(_lepsWiId)).FirstOrDefault
            If awWI IsNot Nothing Then
                _wi = _svc.GetWIforOrderOnWorkStation(awWI.awWI)
                _wiRoutines = _svc.GetRoutinesofWi(_wi.id)
                _wiRoutines = (From a In _wiRoutines Order By a.sequence).ToList
                _currentSeq = _wiRoutines(0).sequence
                _header = headMsg
            Else
                MsgBox("没有维护LEPS与AutoWork的作业指导书对应关系, Leps作业指导书是:" & _lepsWiId, MsgBoxStyle.Critical)
            End If
        Catch ex As Exception
            MsgBox("开始流程失败，请通知管理人员" & ex.ToString, MsgBoxStyle.Critical)
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
                    '_lightController.Play(LightCmdType.ALL_OFF)
                    _lightHelper.Play(LightCmdType.ALL_OFF, StaffSession.GetInstance.WorkStation.workstationId)
                Catch ex As Exception

                End Try

                _currentSeq = seq
                _currentRoutine = _wiRoutines(seq)
                BindData()
            End If
        End If


    End Sub

    Private Sub BindData()
        ' Dim labels As List(Of String) = _svc.GetLabelAddressForRoutineOnWorkStation(_currentRoutine.routineId, StaffSession.GetInstance.StationID)
        ' Dim intLabels As List(Of Integer) = New List(Of Integer)
        'For Each strId As String In labels
        'intLabels.Add(CType(strId, Integer))
        'Next
        Try
            ' _lightController.Play(LightCmdType.ALL_OFF_BEFORE_ON, intLabels)
            _lightHelper.Play(LightCmdType.ALL_OFF_BEFORE_ON, StaffSession.GetInstance.StationID, _currentRoutine.routineId)
        Catch ex As Exception

        End Try

        Me.label_steps.Content = String.Format("Step {0} / {1}", _currentSeq + 1, _wiRoutines.Count)
        Me.label_normalhour.Content = _currentRoutine.normalTime
        Me.images = _currentRoutine.picture.Split(",").ToList
        Me.currentImageIndex = 0
        '    Dim imgPath As String = System.IO.Path.Combine(My.Application.Info.DirectoryPath, "Routines\Img\" & Me.images.First())

        Me.next_image_button.Visibility = Visibility.Collapsed
        Me.prev_image_button.Visibility = Visibility.Collapsed


        Dim videoPath As String = System.IO.Path.Combine(My.Application.Info.DirectoryPath, "Routines\Video\" & _currentRoutine.video)
        Try
            Me.image_wi.Source = New BitmapImage(New Uri(getImagePath(Me.images.First()), UriKind.RelativeOrAbsolute))
        Catch
        End Try
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
        Me.next_image_button.Visibility = Visibility.Visible
        Me.prev_image_button.Visibility = Visibility.Visible


        Me.SetImageNextPrevVisi()
        Me.button_hide.Focus()
    End Sub

    Private Sub button_video_Click(sender As Object, e As RoutedEventArgs) Handles button_video.Click
        Me.image_wi.Visibility = Visibility.Collapsed
        Me.next_image_button.Visibility = Visibility.Collapsed
        Me.prev_image_button.Visibility = Visibility.Collapsed

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
                    lepsCl.AKBasicModule(StaffSession.GetInstance.WorkStation.prodLine, StaffSession.GetInstance.WorkStation.lepsWorkstation, _header.KSK, _lepsWiId)
                    lepsCl.CompleteHarness(_header.Board, StaffSession.GetInstance.WorkStation.lepsWorkstation, _header.KSK)
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
            ' _lightController = New RamLightController(My.Settings.com)
            _lightHelper = New LightHelper

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
        If My.Settings.SingleWorkPlace Then
            Dim waitOrder As New WaitOrder
            waitOrder.Show()
        Else
            Dim login As New Login
            login.Show()
        End If

        Try
            '_lightController.Play(LightCmdType.ALL_OFF)
            '_lightController.Close()
            _lightHelper.Play(LightCmdType.ALL_OFF, StaffSession.GetInstance.StationID)
            _lightHelper.Close()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub image_button_MouseUp(sender As Object, e As MouseButtonEventArgs)
        Dim l As Label = sender
        If (l.Name.Equals("next_image_button")) Then

            currentImageIndex += 1

        Else

            currentImageIndex -= 1
        End If

        SetImageNextPrevVisi()
        '  Dim imgPath As String = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Routines\Img\" & images(currentImageIndex))
        Try
            Me.image_wi.Source = New BitmapImage(New Uri(getImagePath(images(currentImageIndex)), UriKind.RelativeOrAbsolute))
        Catch
        End Try
    End Sub

    Private Sub SetImageNextPrevVisi()

        If currentImageIndex <= 0 Then

            currentImageIndex = 0
            prev_image_button.Visibility = Visibility.Collapsed
            next_image_button.Visibility = Visibility.Collapsed
            If (images.Count > 1) Then

                next_image_button.Visibility = Visibility.Visible

            End If

        ElseIf (currentImageIndex >= images.Count - 1) Then

            currentImageIndex = images.Count - 1
            next_image_button.Visibility = Visibility.Collapsed
            prev_image_button.Visibility = Visibility.Collapsed
            If (images.Count > 1) Then
                prev_image_button.Visibility = Visibility.Visible
            End If

        ElseIf (currentImageIndex < images.Count) Then

            If (images.Count > 1) Then

                next_image_button.Visibility = Visibility.Visible
                prev_image_button.Visibility = Visibility.Visible
            End If
        End If
    End Sub

    Private Sub image_wi_MouseUp(sender As Object, e As MouseButtonEventArgs) Handles image_wi.MouseUp
        Try
            Dim full As ImageFullWindow = New ImageFullWindow(Me)
            full.ShowDialog()
        Catch ex As Exception

        End Try
    End Sub

    Private Function getImagePath(filename As String) As String
        Dim path As String = String.Empty
        If (My.Settings.RemoteImage) Then
            path = My.Settings.FTPServer & filename
        Else
            path = System.IO.Path.Combine(My.Application.Info.DirectoryPath, "Routines\Img\" & filename)
        End If
        Return path
    End Function
End Class
