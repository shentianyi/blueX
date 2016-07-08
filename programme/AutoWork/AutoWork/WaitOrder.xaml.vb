Imports PLCLightCL
Imports KskPlugInSharedObject

Public Class WaitOrder
    Private Sub textBox_ordernr_PreviewKeyUp(sender As Object, e As KeyEventArgs) Handles textBox_ordernr.PreviewKeyUp
        If e.Key = Key.Enter Then
            Me.textBox_ksknr.Focus()
        End If
    End Sub


    Public Sub CheckStatus()
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
            headMsg = lepsIntef.StartAndGetHarnessByBoard(Me.textBox_ordernr.Text, StaffSession.GetInstance.StationID)
            ''如果在第二站位此处出错，请修改StartAndGetHarnessByBoard为一个只获取信息的方法
            If headMsg Is Nothing Or headMsg.ProcessStatus <> 64 Then
                MsgBox("与LEPS交互错误")
            Else
                Dim wis As List(Of String) = lepsIntef.GetBasicModule(StaffSession.GetInstance.StationID, headMsg.KSK)
                If wis Is Nothing Or wis.Count = 0 Then
                    MsgBox("找不到作业指导书")
                Else
                    If order Is Nothing Then
                        db.Orders.InsertOnSubmit(New Order With {.status = OrderStatus.Open, .orderId = headMsg.KSK, .workstationId = StaffSession.GetInstance.StationID})
                    Else
                        Dim db1 As AutoWorkDataContext = New AutoWorkDataContext(GlobalConfigs.DbConnStr)
                        Dim neworder As Order = db1.Orders.SingleOrDefault(Function(c) String.Compare(c.orderId, Me.textBox_ksknr.Text) = 0)

                        neworder.workstationId = StaffSession.GetInstance.WorkStation.workstationId
                        neworder.status = OrderStatus.Open
                        db1.SubmitChanges()
                    End If
                    db.SubmitChanges()
                    Dim working As Working = New Working(headMsg, wis(0))
                    working.ShowDialog()
                End If
            End If
        Catch ex As Exception
            MsgBox("出现未知错误: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub


    Private Sub init()
        Me.textBox_ordernr.Text = ""
        Me.textBox_ksknr.Text = ""
        Me.textBox_ordernr.Focus()

    End Sub
    Private Sub textBox_ordernr_TextChanged(sender As Object, e As TextChangedEventArgs) Handles textBox_ordernr.TextChanged

    End Sub

    Private Sub WaitOrder_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Me.station_name.Content = StaffSession.GetInstance.StationID
        label_staffid.Content = StaffSession.GetInstance.StaffId
        init()
    End Sub

    Private Sub textBox_ksknr_PreviewKeyUp(sender As Object, e As KeyEventArgs) Handles textBox_ksknr.PreviewKeyUp

        If e.Key = Key.Enter Then
            If String.IsNullOrEmpty(textBox_ordernr.Text) = True Or String.IsNullOrEmpty(textBox_ksknr.Text) = True Then
                MsgBox("请扫描入KSK号和小车号")
                init()
            Else
                CheckStatus()
                init()
            End If
        End If

    End Sub
End Class
