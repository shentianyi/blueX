Imports Brilliantech.Framwork.Utils
Imports PLCLightCL.Enum
Imports PLCLightCL.Light

Public Class LightHelper
    Private _ramlightController As ILightController
    Private _canlightController As ILightController


    Public Sub New()
        '_ramlightController = New RamLightController(My.Settings.com)
        '_canlightController = New CanLightController(My.Settings.CanServerIP, My.Settings.CanServerPort, My.Settings.CanId)
        Me.Open()
    End Sub

    Public Sub Open()
        Try
            _ramlightController = New RamLightController(My.Settings.com)
            _canlightController = New CanLightController(My.Settings.CanServerIP, My.Settings.CanServerPort, My.Settings.CanId)
        Catch ex As Exception
            LogUtil.LogUtil.Logger.Error(ex.Message, ex)
        End Try
    End Sub


    Public Sub Play(cmdType As LightCmdType, workStaionId As String, Optional ByVal routeId As String = Nothing)
        Try
            Dim db As AutoWorkDataContext = New AutoWorkDataContext
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

            If cmdType = LightCmdType.ON Then
                If String.IsNullOrEmpty(routeId) = False Then

                    If routeLightDic.Keys.Contains("Ram") Then
                        _ramlightController.Play(LightCmdType.ON, StringListToInt(routeLightDic("Ram").Select(Function(c) c.labelAddr).ToList()))
                    End If

                    If routeLightDic.Keys.Contains("Can") Then
                        _canlightController.Play(LightCmdType.ON, StringListToInt(routeLightDic("Can").Select(Function(c) c.labelAddr).ToList()))
                    End If

                End If
            ElseIf cmdType = LightCmdType.ALL_OFF Then
                _ramlightController.Play(LightCmdType.ALL_OFF)

                If allLightDic.Keys.Contains("Can") Then
                    _canlightController.Play(LightCmdType.OFF, StringListToInt(allLightDic("Can").Select(Function(c) c.labelAddr).ToList()))
                End If

            ElseIf cmdType = LightCmdType.ALL_OFF_BEFORE_ON Then
                _ramlightController.Play(LightCmdType.ALL_OFF)
                If allLightDic.Keys.Contains("Can") Then
                    _canlightController.Play(LightCmdType.OFF, StringListToInt(allLightDic("Can").Select(Function(c) c.labelAddr).ToList()))
                End If


                If String.IsNullOrEmpty(routeId) = False Then

                    If routeLightDic.Keys.Contains("Ram") Then
                        _ramlightController.Play(LightCmdType.ON, StringListToInt(routeLightDic("Ram").Select(Function(c) c.labelAddr).ToList()))
                    End If

                    If routeLightDic.Keys.Contains("Can") Then
                        _canlightController.Play(LightCmdType.ON, StringListToInt(routeLightDic("Can").Select(Function(c) c.labelAddr).ToList()))
                    End If

                End If
            End If

        Catch ex As Exception
            LogUtil.LogUtil.Logger.Error(ex.Message, ex)
        End Try
    End Sub

    Public Sub Close()
        Try
            _ramlightController.Close()
            _canlightController.Close()
        Catch ex As Exception
            LogUtil.LogUtil.Logger.Error(ex.Message, ex)
        End Try
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
