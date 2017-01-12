Imports AutoWork
Imports Repository

Public Class MainService
    Implements IMainService


    Public Function GetLabelAddressForPartsOnWorkStation(parts As List(Of String), workstationId As String) As List(Of ELabelOnForPartOnWorkstation) Implements IMainService.GetLabelAddressForPartsOnWorkStation
        Dim repo As Repository(Of ELabelOnForPartOnWorkstation) = New Repository(Of ELabelOnForPartOnWorkstation)(New DataContext(GlobalConfigs.DbConnStr))
        Return repo.FindAll(Function(c) parts.Contains(c.partNr) And String.Equals(c.workstationId, workstationId, StringComparison.InvariantCultureIgnoreCase)).ToList
    End Function

    Public Function GetLabelAddressForRoutineOnWorkStation(routineId As String, workstationId As String) As List(Of String) Implements IMainService.GetLabelAddressForRoutineOnWorkStation
        Dim result As List(Of String) = New List(Of String)
        Dim parts As List(Of PartOnRoutine) = GetPartOfRoutine(routineId)
        Dim partstr As List(Of String)
        If parts IsNot Nothing Then
            partstr = (From pas In parts Select pas.partnr).ToList
        End If
        If partstr IsNot Nothing Then
            Dim elabels As List(Of ELabelOnForPartOnWorkstation) = GetLabelAddressForPartsOnWorkStation(partstr, workstationId)
            If elabels IsNot Nothing Then
                result = (From labels In elabels Select labels.labelAddr).ToList
            End If
        End If
        Return result
    End Function

    Public Function GetLabelsForRoutineOnWorkStation(routineId As String, workstationId As String) As List(Of ELabelOnForPartOnWorkstation) Implements IMainService.GetLabelsForRoutineOnWorkStation
        Dim result As List(Of String) = New List(Of String)
        Dim parts As List(Of PartOnRoutine) = GetPartOfRoutine(routineId)
        Dim partstr As List(Of String)
        If parts IsNot Nothing Then
            partstr = (From pas In parts Select pas.partnr).ToList
        End If
        If partstr IsNot Nothing Then
            Dim elabels As List(Of ELabelOnForPartOnWorkstation) = GetLabelAddressForPartsOnWorkStation(partstr, workstationId)
            'If elabels IsNot Nothing Then
            '    result = (From labels In elabels Select labels.labelAddr).ToList
            'End If
            Return elabels
        End If
        Return Nothing
    End Function

    Public Function GetOrder(orderId As String) As Order Implements IMainService.GetOrder
        Dim repo As Repository(Of Order) = New Repository(Of Order)(New DataContext(GlobalConfigs.DbConnStr))
        Return repo.Single(Function(c) String.Equals(c.orderId, orderId, StringComparison.InvariantCultureIgnoreCase))
    End Function

    Public Function GetPartOfRoutine(routineId As String) As List(Of PartOnRoutine) Implements IMainService.GetPartOfRoutine
        Dim repo As Repository(Of PartOnRoutine) = New Repository(Of PartOnRoutine)(New DataContext(GlobalConfigs.DbConnStr))
        Return repo.FindAll(Function(c) String.Equals(c.routineId, routineId, StringComparison.InvariantCultureIgnoreCase)).ToList
    End Function

    Public Function GetRoutinesofWi(workInstructionId As String) As List(Of RoutineOnWorkInstructionDetails) Implements IMainService.GetRoutinesofWi
        Dim repo As Repository(Of RoutineOnWorkInstructionDetails) = New Repository(Of RoutineOnWorkInstructionDetails)(New DataContext(GlobalConfigs.DbConnStr))
        Return repo.FindAll(Function(c) String.Compare(c.workInstructionId, workInstructionId, True) = 0).ToList
    End Function

    Public Function GetStaff(staffId As String) As Staff Implements IMainService.GetStaff
        Dim repo As Repository(Of Staff) = New Repository(Of Staff)(New DataContext(GlobalConfigs.DbConnStr))
        Return repo.Single(Function(c) String.Equals(c.id, staffId, StringComparison.InvariantCultureIgnoreCase))
    End Function

    Public Function GetWIforOrderOnWorkStation(workStation As String) As WorkInstruction Implements IMainService.GetWIforOrderOnWorkStation
        Dim repo As Repository(Of WorkInstruction) = New Repository(Of WorkInstruction)(New DataContext(GlobalConfigs.DbConnStr))
        Return repo.Single(Function(c) String.Compare(workStation, c.id) = 0)
    End Function



End Class
