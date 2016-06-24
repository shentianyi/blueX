Imports System.ComponentModel

Public Class WorkInstructionViewModel
    Implements INotifyPropertyChanged

    Private _workinstruction As WorkInstruction
    Public Property WorkInstruction As WorkInstruction
        Get
            Return _workinstruction
        End Get
        Set(value As WorkInstruction)
            _workinstruction = value
        End Set
    End Property

    'Private _normalHour As Integer
    'Public Property NormalHour As Integer
    '    Get
    '        If Routines IsNot Nothing Then
    '            _normalHour = (From s In _routines Select s.normalTime).Sum()
    '        End If

    '        Return _normalHour
    '    End Get
    '    Set(value As Integer)
    '        _normalHour = value
    '    End Set
    'End Property

    'Private _routines As List(Of RoutineOnWorkStation)
    'Public Property Routines As List(Of)
    '    Get
    '        If _routines Is Nothing Then
    '            _routines = New List(Of Routine)
    '        End If
    '        Return _routines
    '    End Get
    '    Set(value As List(Of Routine))
    '        _routines = value
    '    End Set
    'End Property

    Private _currentRoutine As Routine
    Public Property CurrentRoutine As Routine
        Get
            Return _currentRoutine
        End Get
        Set(value As Routine)
            _currentRoutine = value
        End Set
    End Property

    Private _currentStep As Integer
    Public Property CurrentStep As Integer
        Get
            Return _currentStep
        End Get
        Set(value As Integer)
            _currentStep = value
        End Set
    End Property


    Private _currentRoutineNormalHour As Integer

    Public Property CurrentRoutineNormalHour As Integer
        Get
            Return _currentRoutineNormalHour
        End Get
        Set(value As Integer)
            _currentRoutineNormalHour = value
        End Set

    End Property

    Private _currentRoutineActualHour As Integer
    Public Property CurrentRoutineActualHour As Integer
        Get
            Return _currentRoutineActualHour
        End Get
        Set(value As Integer)
            _currentRoutineActualHour = value
        End Set
    End Property


    Public Sub NextRoutine()

    End Sub

    Public Sub PreviousRoutine()

    End Sub




    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged



End Class
