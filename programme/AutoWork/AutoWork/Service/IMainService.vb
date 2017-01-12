Public Interface IMainService
    Function GetStaff(staffId As String) As Staff
    Function GetOrder(orderId As String) As Order
    Function GetWIforOrderOnWorkStation(workStation As String) As WorkInstruction

    Function GetRoutinesofWi(workInstructionId As String) As List(Of RoutineOnWorkInstructionDetails)
    Function GetPartOfRoutine(routineId As String) As List(Of PartOnRoutine)

    Function GetLabelAddressForPartsOnWorkStation(parts As List(Of String), workstationId As String) As List(Of ELabelOnForPartOnWorkstation)


    Function GetLabelAddressForRoutineOnWorkStation(routineId As String, workstationId As String) As List(Of String)

    Function GetLabelsForRoutineOnWorkStation(routineId As String, workstationId As String) As List(Of ELabelOnForPartOnWorkstation)

End Interface
