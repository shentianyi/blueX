Imports System.ComponentModel
Public Class RoutineInstructionViewModel
    Implements INotifyPropertyChanged

    'current routine sequence
    ' parts list data
    ' video url
    ' staff
    ' workinstruction ID
    ' normal time
    ' time consumption
    ' order number
    ' customer
    ' delivery schedule
    ' emergent

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
End Class
