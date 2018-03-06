Imports PLCLightCL
Imports KskPlugInSharedObject
Imports System.ComponentModel
Imports AutoWork.MsgLevel
Imports AutoWork.MsgDialog
Imports System.Text.RegularExpressions

Partial Public Class WaitOrder
    Inherits Window
    Public Sub New()
        InitializeComponent()
    End Sub



    Dim goLogin As Boolean = True
    Private Sub textBox_ordernr_PreviewKeyUp(sender As Object, e As KeyEventArgs) Handles textBox_ordernr.PreviewKeyUp
        If e.Key = Key.Enter Then
            Me.textBox_ksknr.Focus()
        End If
    End Sub


    Public Sub CheckStatus()
        'Me.checkPlug(1)


        Dim lepsIntef As LEPSController = New LEPSController(GlobalConfigs.LepsDb)
        Dim db As AutoWorkDataContext = New AutoWorkDataContext(My.Settings.database)

        Dim order As Order = db.Orders.SingleOrDefault(Function(c) String.Compare(c.orderId, Me.textBox_ksknr.Text, True) = 0)
        If order Is Nothing Then
            If StaffSession.GetInstance.WorkStation.seq <> 1 Then
                MsgBox("订单应该在从第一个工作台开始", MsgBoxStyle.Critical)
                Exit Sub
            End If

        Else
            If order.status = OrderStatus.Finish And order.WorkStation.seq + 1 <> StaffSession.GetInstance.WorkStation.seq Then
                MsgBox("订单的下一个站位应该是" & order.WorkStation.seq + 1 & "号站位", MsgBoxStyle.Critical)
                Exit Sub
            ElseIf order.status = OrderStatus.Open Then

                If order.WorkStation.seq <> StaffSession.GetInstance.WorkStation.seq Then
                    MsgBox("订单已经在站位" & order.workstationId & "上开始")
                    Exit Sub
                Else
                    MsgBox("订单已经在此站位开始，可能因为断电等原因中断过，将自动进行开始")
                End If
            End If
        End If

        Try

            Dim headMsg As Model.HeadMessage
            headMsg = lepsIntef.StartAndGetHarnessByBoard(Me.textBox_ordernr.Text, StaffSession.GetInstance.WorkStation.lepsWorkstation)
            ''如果在第二站位此处出错，请修改StartAndGetHarnessByBoard为一个只获取信息的方法
            If headMsg Is Nothing Or headMsg.ProcessStatus <> 64 Then
                MsgBox("与LEPS交互错误")
            Else
                If textBox_ksknr.Text.Equals(headMsg.KSK) = False Then
                    MsgBox("扫描KSK号与LEPS订单号不一致！请重新扫描！")
                    Return
                End If

                Dim wis As List(Of String) = lepsIntef.GetBasicModule(StaffSession.GetInstance.WorkStation.lepsWorkstation, headMsg.KSK)
                If wis Is Nothing Or wis.Count = 0 Then
                    MsgBox("找不到作业指导书")
                Else
                    If order Is Nothing Then
                        db.Orders.InsertOnSubmit(New Order With {.status = OrderStatus.Open, .orderId = headMsg.KSK, .workstationId = StaffSession.GetInstance.StationID})
                    Else
                        Dim db1 As AutoWorkDataContext = New AutoWorkDataContext(My.Settings.database)
                        Dim neworder As Order = db1.Orders.SingleOrDefault(Function(c) String.Compare(c.orderId, Me.textBox_ksknr.Text) = 0)

                        neworder.workstationId = StaffSession.GetInstance.WorkStation.workstationId
                        neworder.status = OrderStatus.Open
                        db1.SubmitChanges()
                    End If
                    db.SubmitChanges()

                    Dim msg1 As String = "LEPS作业指导书" & String.Join(";", wis.ToArray)
                    'MsgBox("LEPS作业指导书" & String.Join(";", wis.ToArray), MsgBoxStyle.Information)
                    CMsgDlg(MsgLevel.Info, msg1, True, Nothing, My.Settings.SameWIColseTime).ShowDialog()
                    '判断指导书是否重复
                    If String.IsNullOrEmpty(My.Settings.LastLeps) Then
                        My.Settings.LastLeps = wis(0)
                    Else
                        If Not My.Settings.LastLeps.Equals(wis(0)) Then
                            Dim msg As String = "上一次作业指导书为:" & My.Settings.LastLeps + ", 本次为：" & wis(0)
                            ' MessageBox.Show("上一次", MsgBoxStyle.Information)
                            My.Settings.LastLeps = wis(0)
                            If My.Settings.IsShowSameWI = True Then
                                CMsgDlg(MsgLevel.Warning, msg, True, Nothing).ShowDialog()
                            End If

                        End If

                    End If


                    Dim working As Working = New Working(headMsg, wis(0))
                    goLogin = False

                    '2018-03-06 Charlot
                    Me.checkPlug(1)
                    Me.Close()
                    working.Show()

                    'working.ShowDialog()
                End If
            End If
        Catch ex As Exception
            MsgBox("出现未知错误: " & ex.Message, MsgBoxStyle.Critical)
        End Try

        'Dim headMsg As Model.HeadMessage = New Model.HeadMessage
        'headMsg.Board = "a"
        'headMsg.KSK = "ffff"

        'Dim working As Working = New Working(headMsg, "WI-0101")
        'goLogin = False
        'Me.Close()
        'working.Show()
    End Sub



    Private Sub init()
        Me.textBox_ordernr.Text = ""
        Me.textBox_ksknr.Text = ""
        Me.textBox_ordernr.Focus()


        '2018-03-6 Charlot
        Me.checkPlug(0)
    End Sub




    Private Sub textBox_ordernr_TextChanged(sender As Object, e As TextChangedEventArgs) Handles textBox_ordernr.TextChanged

    End Sub

    'Private Sub WaitOrder_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

    'End Sub

    Private Sub textBox_ksknr_PreviewKeyUp(sender As Object, e As KeyEventArgs) Handles textBox_ksknr.PreviewKeyUp

        If e.Key = Key.Enter Then

            '2018-03-06 Charlot
            If My.Settings.WorkstationType.Equals("AW9") Then
                If String.IsNullOrEmpty(textBox_testLabelNr.Text) = True Then
                    MsgBox("请扫描入电测标签号")
                    init()
                    Return
                Else

                    If Me.isAw9Match(textBox_testLabelNr.Text) = False Then
                        'Me.checkPlug(1)

                        CMsgDlg(MsgLevel.Mistake, "电测标签号格式错误，请确认", True, Nothing).ShowDialog()
                        init()
                        Return

                    End If
                End If
            End If

            If String.IsNullOrEmpty(textBox_ordernr.Text) = True Or String.IsNullOrEmpty(textBox_ksknr.Text) = True Then
                MsgBox("请扫描入小车号和KSK号")
                init()
            Else
                Me.orderCheck()
            End If
        End If

    End Sub


    Private Sub orderCheck()
        Dim tr As Regex = New Regex("^(TC|TM)\w+")
        Dim kr As Regex = New Regex("^4([a-zA-Z0-9]{9})(PR|EN)$")
        If tr.IsMatch(textBox_ordernr.Text) And kr.IsMatch(textBox_ksknr.Text) Then
            CheckStatus()
            init()
        Else
            CMsgDlg(MsgLevel.Mistake, "请扫描正确的小车号和KSK号", True, Nothing).ShowDialog()

            ' MsgBox("请扫描正确的小车号和KSK号")
            init()
        End If
    End Sub


    Private Sub WaitOrder_Closing(sender As Object, e As CancelEventArgs)
        If goLogin Then
            Dim login As New Login
            login.Show()
        End If
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs)
        Me.station_name.Content = StaffSession.GetInstance.StationID
        label_staffid.Content = StaffSession.GetInstance.StaffId
        init()
    End Sub





    '2018-03-06 Charlot
    '' 开始
    Private Sub checkPlug(stage As Integer)
        ''stage 为0是开始
        ''stage 为1是扫描完

        If My.Settings.WorkstationType.Equals("AW9") Then
            If stage = 0 Then
                ' 电测标签验证
                Me.aw9TestSP.Visibility = Visibility.Visible
                aw9TestSP.Margin = New Thickness(10, 16, 0, 0)
                carSP.Margin = New Thickness(10, 36, 0, 0)
                kskSP.Margin = New Thickness(10, 36, 0, 0)

                Me.textBox_testLabelNr.Focus()
                Me.textBox_testLabelNr.Text = String.Empty
            ElseIf stage = 1 Then
                Try
                    ' 写入电测扫描记录
                    Dim db As AutoWorkDataContext = New AutoWorkDataContext(My.Settings.database)
                    Dim record As AW9Record = New AW9Record
                    record.labelContent = textBox_testLabelNr.Text
                    record.staffNr = StaffSession.GetInstance.StaffId
                    record.orderNr = textBox_ksknr.Text
                    record.workstationNr = StaffSession.GetInstance.StationID
                    record.carNr = textBox_ordernr.Text

                    record.createdAt = Date.Now
                    record.updatedAt = Date.Now

                    db.AW9Record.InsertOnSubmit(record)
                    db.SubmitChanges()
                Catch e As Exception
                    CMsgDlg(MsgLevel.Mistake, "电测保存错误:" & e.Message, True, Nothing).ShowDialog()
                End Try
            End If
        Else
                Me.aw9TestSP.Visibility = Visibility.Collapsed
        End If


    End Sub


    Private Function isAw9Match(str As String) As Boolean
        Dim tr As Regex = New Regex(My.Settings.aw9TestLabelRegex)
        Return tr.IsMatch(str)
    End Function

    Private Sub textBox_testLabelNr_PreviewKeyUp(sender As Object, e As KeyEventArgs) Handles textBox_testLabelNr.PreviewKeyUp
        If e.Key = Key.Enter Then
            Me.textBox_ordernr.Focus()
        End If
    End Sub
    '' 结束
End Class
