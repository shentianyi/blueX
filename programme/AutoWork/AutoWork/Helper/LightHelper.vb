Imports Brilliantech.Framwork.Utils
Imports PLCLightCL.Enum
Imports PLCLightCL.Light

Public Class LightHelper
    Private _ramlightController As ILightController
    Private _canlightController As ILightController


    Public Sub New()

        'LogUtil.LogUtil.Logger.Info("Play Start")
        '_ramlightController = New RamLightController(My.Settings.com)
        '_canlightController = New CanLightController(My.Settings.CanServerIP, My.Settings.CanServerPort, My.Settings.CanId)
        Me.Open()
        'LogUtil.LogUtil.Logger.Info("Play End")

    End Sub

    ' Private locker As Object = New Object
    Public Sub Open()
        '   SyncLock locker
        Try
                If _ramlightController Is Nothing Then
                    _ramlightController = New RamLightController(My.Settings.com)
                End If
                ' _canlightController = New CanLightController(My.Settings.CanServerIP, My.Settings.CanServerPort, My.Settings.CanId)
            Catch ex As Exception
                LogUtil.LogUtil.Logger.Error(ex.Message, ex)
            End Try

            Try
                If _canlightController Is Nothing Then
                    '_ramlightController = New RamLightController(My.Settings.com)
                    _canlightController = New CanLightController(My.Settings.CanServerIP, My.Settings.CanServerPort, My.Settings.CanId)
                End If
            Catch ex As Exception
                LogUtil.LogUtil.Logger.Error(ex.Message, ex)
            End Try

        ' End SyncLock

    End Sub

    Private cmdType As LightCmdType
    Private workStaionId As String
    Private routeId As String = Nothing

    '  Dim beginInvokeThreads As List(Of Threading.Thread) = New List(Of System.Threading.Thread)

    Public Sub Play(cmdType As LightCmdType, workStaionId As String, Optional ByVal routeId As String = Nothing)
        Me.cmdType = cmdType
        Me.workStaionId = workStaionId
        Me.routeId = routeId

        'Dim beginInvokeThread = New System.Threading.Thread(AddressOf DoPlay)
        'beginInvokeThreads.Add(beginInvokeThread)
        'beginInvokeThread.IsBackground = True
        'beginInvokeThread.Start()
        Me.DoPlay()
    End Sub


    Public Sub DoPlay()
        '  SyncLock locker
        Try
                LogUtil.LogUtil.Logger.Info("Play Start")
                ' Me.Open()
                Dim db As AutoWorkDataContext = New AutoWorkDataContext(My.Settings.database)
                Dim allLights = db.ELabelOnForPartOnWorkstation.Where(Function(c) c.workstationId = workStaionId).ToList
                Dim allLightDic As Dictionary(Of String, List(Of ELabelOnForPartOnWorkstation)) = New Dictionary(Of String, List(Of ELabelOnForPartOnWorkstation))
                For Each i As ELabelOnForPartOnWorkstation In allLights
                    If allLightDic.Keys.Contains(i.controlType) Then
                        If allLightDic(i.controlType) Is Nothing Then
                            allLightDic(i.controlType) = New List(Of ELabelOnForPartOnWorkstation)
                            allLightDic(i.controlType).Add(i)
                        Else
                            allLightDic(i.controlType).Add(i)
                        End If
                    Else
                        allLightDic(i.controlType) = New List(Of ELabelOnForPartOnWorkstation)
                        allLightDic(i.controlType).Add(i)
                    End If
                Next

                Dim ms As MainService = New MainService

                Dim routeLights As List(Of ELabelOnForPartOnWorkstation) = New List(Of ELabelOnForPartOnWorkstation)
                Dim routeLightDic As Dictionary(Of String, List(Of ELabelOnForPartOnWorkstation)) = New Dictionary(Of String, List(Of ELabelOnForPartOnWorkstation))

                If String.IsNullOrEmpty(routeId) = False Then
                    routeLights = ms.GetLabelsForRoutineOnWorkStation(routeId, workStaionId)
                    If routeLights IsNot Nothing Then
                        For Each i As ELabelOnForPartOnWorkstation In routeLights
                            If routeLightDic.Keys.Contains(i.controlType) Then
                                If routeLightDic(i.controlType) Is Nothing Then
                                    routeLightDic(i.controlType) = New List(Of ELabelOnForPartOnWorkstation)
                                    routeLightDic(i.controlType).Add(i)
                                Else
                                    routeLightDic(i.controlType).Add(i)
                                End If
                            Else
                                routeLightDic(i.controlType) = New List(Of ELabelOnForPartOnWorkstation)
                                routeLightDic(i.controlType).Add(i)
                            End If
                        Next
                    End If

                End If

                If cmdType = LightCmdType.ON Then
                    If String.IsNullOrEmpty(routeId) = False Then

                        Try
                            If routeLightDic.Keys.Contains("RAM") Then
                                _ramlightController.Play(LightCmdType.ON, StringListToInt(routeLightDic("RAM").Select(Function(c) c.labelAddr).ToList()))
                            End If

                        Catch ex As Exception
                            LogUtil.LogUtil.Logger.Error(ex.Message, ex)
                        End Try

                        Try
                            If routeLightDic.Keys.Contains("CAN") Then
                                _canlightController.Play(LightCmdType.ON, StringListToInt(routeLightDic("CAN").Select(Function(c) c.labelAddr).ToList()))
                            End If


                        Catch ex As Exception
                            LogUtil.LogUtil.Logger.Error(ex.Message, ex)
                        End Try
                    End If
                ElseIf cmdType = LightCmdType.ALL_OFF Then

                    Try
                        _ramlightController.Play(LightCmdType.ALL_OFF)

                    Catch ex As Exception
                        LogUtil.LogUtil.Logger.Error(ex.Message, ex)
                    End Try

                    Try
                        If allLightDic.Keys.Contains("CAN") Then
                            _canlightController.Play(LightCmdType.OFF, StringListToInt(allLightDic("CAN").Select(Function(c) c.labelAddr).ToList()))
                        End If

                    Catch ex As Exception
                        LogUtil.LogUtil.Logger.Error(ex.Message, ex)
                    End Try
                ElseIf cmdType = LightCmdType.ALL_OFF_BEFORE_ON Then
                    Try
                        _ramlightController.Play(LightCmdType.ALL_OFF)

                    Catch ex As Exception
                        LogUtil.LogUtil.Logger.Error(ex.Message, ex)
                    End Try


                    Try
                        If allLightDic.Keys.Contains("CAN") Then
                            _canlightController.Play(LightCmdType.OFF, StringListToInt(allLightDic("CAN").Select(Function(c) c.labelAddr).ToList()))
                        End If

                    Catch ex As Exception
                        LogUtil.LogUtil.Logger.Error(ex.Message, ex)
                    End Try
                    If String.IsNullOrEmpty(routeId) = False Then

                        Try
                            If routeLightDic.Keys.Contains("RAM") Then
                                _ramlightController.Play(LightCmdType.ON, StringListToInt(routeLightDic("RAM").Select(Function(c) c.labelAddr).ToList()))
                            End If
                        Catch ex As Exception
                            LogUtil.LogUtil.Logger.Error(ex.Message, ex)
                        End Try

                        Try
                            If routeLightDic.Keys.Contains("CAN") Then
                                _canlightController.Play(LightCmdType.ON, StringListToInt(routeLightDic("CAN").Select(Function(c) c.labelAddr).ToList()))
                            End If
                        Catch ex As Exception
                            LogUtil.LogUtil.Logger.Error(ex.Message, ex)
                        End Try
                    End If
                End If
                LogUtil.LogUtil.Logger.Info("Play End")

                Me.Close()

                ' beginInvokeThread.Abort()
                '  beginInvokeThread = Nothing
            Catch ex As Exception
                LogUtil.LogUtil.Logger.Error(ex.Message, ex)
            Finally
                'beginInvokeThread.Abort()
                'beginInvokeThread = Nothing
            End Try
     '   End SyncLock
    End Sub


    Public Sub Close()
        Try
            _ramlightController.Close()
            _ramlightController = Nothing
            '_canlightController.Close()
        Catch ex As Exception
            LogUtil.LogUtil.Logger.Error(ex.Message, ex)
        End Try

        Try
            '_ramlightController.Close()
            _canlightController.Close()
            _canlightController = Nothing
        Catch ex As Exception
            LogUtil.LogUtil.Logger.Error(ex.Message, ex)
        End Try

        'Try
        '    For Each t As System.Threading.Thread In beginInvokeThreads
        '        '     t.Abort()
        '        '    t = Nothing

        '    Next
        'Catch ex As Exception
        '    LogUtil.LogUtil.Logger.Error(ex.Message, ex)
        'End Try

    End Sub

    Public Function StringListToInt(l As List(Of String)) As List(Of Integer)
        Dim intLabels As List(Of Integer) = New List(Of Integer)
        For Each strId As String In l
            If String.IsNullOrEmpty(strId) = False Then
                intLabels.Add(CType(strId, Integer))
            End If
        Next
        Return intLabels
    End Function
End Class
