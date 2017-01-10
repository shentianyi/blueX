Imports System.IO
Imports System.Media
Imports System.Timers
Imports System.Windows.Threading

Public Class MsgDialog

    Private Shared ReadOnly infoImage As String = Path.Combine(My.Application.Info.DirectoryPath, "msg\\Info.png")
    Private Shared ReadOnly warningImage As String = Path.Combine(My.Application.Info.DirectoryPath, "msg\\Warning.png")
    Private Shared ReadOnly successImage As String = Path.Combine(My.Application.Info.DirectoryPath, "msg\\ok.png")
    Private Shared ReadOnly errorImage As String = Path.Combine(My.Application.Info.DirectoryPath, "msg\\Error.png")

    Private focusIdleIndicator As Boolean = False
    Private currentLevel As MsgLevel
    Private autoCloseTimer As System.Timers.Timer

    Private Sub setClickMode()
        Me.Button_no.ClickMode = ClickMode.Press
        Me.Button_yes.ClickMode = ClickMode.Press
        Me.Button_no.IsDefault = True
        Me.Button_yes.IsDefault = True
    End Sub


    Public Sub New(ByVal level As MsgLevel, ByVal msg As String, Optional ByVal setIdle As Boolean = False, Optional ByVal autoCloseTime As Integer = 0)
        InitializeComponent()
        Me.AssignPicture(level)
        Me.AssignText(msg)
        focusIdleIndicator = setIdle
        currentLevel = level
        setClickMode()
        If autoCloseTime > 0 Then
            Me.autoCloseTimer = New System.Timers.Timer
            Me.autoCloseTimer.Interval = autoCloseTime * 1000
            AddHandler autoCloseTimer.Elapsed, AddressOf HandleTimer

            'timer.Elapsed = Async ( sender, e ) => await HandleTimer()
            autoCloseTimer.Enabled = True
        End If
    End Sub





    Private Sub HandleTimer(sender As Object, e As ElapsedEventArgs)

        'Try
        '    Me.DialogResult = True
        'Catch ex As Exception

        'End Try

        'Me.Close()
        'Me.autoCloseTimer.Enabled = False
        '  Me.Dispatcher.Invoke(DispatcherPriority.Normal, New System.Windows.Threading.TimerDispatcherDelegate(AddressOf Button_yes_Click))
        Try
            Dispatcher.Invoke(Sub()
                                  Try
                                      Close()
                                  Catch ex As Exception

                                  End Try

                              End Sub)
        Catch ex As Exception

        End Try

        '  Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, {Me.Close()})

    End Sub

    Public Sub New(ByVal level As MsgLevel, ByVal msg() As String, Optional ByVal setIdle As Boolean = False)
        InitializeComponent()
        Me.AssignPicture(level)
        Me.AssignText(ArrayToString(msg))
        focusIdleIndicator = setIdle
        currentLevel = level
        setClickMode()
    End Sub

    Public Sub AssignPicture(ByVal level As MsgLevel)
        Dim imagePath As String = ""
        Try
            Select Case level
                Case MsgLevel.Info
                    imagePath = infoImage
                    Me.Background = Brushes.LightBlue
                    Exit Select
                Case MsgLevel.Mistake
                    imagePath = errorImage
                    Exit Select
                Case MsgLevel.Successful
                    imagePath = successImage
                    Exit Select
                Case MsgLevel.Warning
                    imagePath = warningImage
                    ' Me.Background = Brushes.Yellow
                    Me.Background = New SolidColorBrush(ColorConverter.ConvertFromString(My.Settings.SameWIMsgWindowColor))
                    Exit Select
            End Select
            Me.Indicator.Source = New BitmapImage(New Uri(imagePath, UriKind.Absolute))
        Catch ex As Exception

        End Try


    End Sub

    Public Shared Function CMsgDlg(ByVal level As MsgLevel, ByVal msg As String, Optional ByVal setIdle As Boolean = False, Optional ByVal Owner As Window = Nothing, Optional ByVal autoCloseTime As Integer = 0) As MsgDialog
        Dim returned As MsgDialog = New MsgDialog(level, msg, setIdle, autoCloseTime)
        If Owner IsNot Nothing Then
            returned.Owner = Owner
        End If
        Return returned
    End Function



    Private Sub AssignText(ByVal msg As String)
        Me.TextBox_Message.Text = msg

    End Sub

    Public Shared Function ArrayToString(ByVal str() As String)
        Dim combined As String = ""
        If str IsNot Nothing Then
            If str.Length > 0 Then
                For Each st As String In str
                    combined = combined & vbCrLf & st
                Next
            End If

        End If
        Return combined.TrimStart(New Char() {vbCrLf})
    End Function

    Private Sub Button_yes_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_yes.Click

        e.Handled = True
        Try
            Me.DialogResult = True
        Catch ex As Exception

        End Try

        Me.Close()

    End Sub





    Private Sub Button_no_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button_no.Click
        e.Handled = True
        Try
            Me.DialogResult = False
        Catch ex As Exception

        End Try

        Me.Close()
    End Sub

    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If Me.focusIdleIndicator = True Then
            Me.TextBox_Message.Focus()
        Else
            Me.Button_no.Focus()
        End If

        System.Threading.ThreadPool.QueueUserWorkItem(New System.Threading.WaitCallback(AddressOf PlaySound), Me.currentLevel)
    End Sub

    Public Shared Sub PlaySound(ByVal level As Object)

        Try
            Dim player As SoundPlayer = New System.Media.SoundPlayer


            Select Case CType(level, Integer)
                Case MsgLevel.Mistake
                    player.SoundLocation = System.IO.Path.Combine(My.Application.Info.DirectoryPath, "msg\\alarm.wav")
                Case MsgLevel.Info
                    player.SoundLocation = System.IO.Path.Combine(My.Application.Info.DirectoryPath, "msg\\info.wav")
                Case MsgLevel.Successful
                    player.SoundLocation = System.IO.Path.Combine(My.Application.Info.DirectoryPath, "msg\\info.wav")
                Case MsgLevel.Warning
                    player.SoundLocation = System.IO.Path.Combine(My.Application.Info.DirectoryPath, "msg\\alarm.wav")
            End Select
            player.LoadAsync()
            player.PlaySync()
        Catch ex As Exception

        End Try



    End Sub

    Private Sub Window_Closing(sender As Object, e As ComponentModel.CancelEventArgs)
        If autoCloseTimer IsNot Nothing Then
            autoCloseTimer.Stop()
            autoCloseTimer = Nothing
        End If
    End Sub

    'Private Sub Button_no_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Button_no.KeyUp

    '    If e.Key = Key.Enter Then

    '        Try
    '            Me.DialogResult = False
    '        Catch ex As Exception

    '        End Try
    '        e.Handled = True
    '        Me.Close()
    '    End If

    'End Sub

    'Private Sub Button_yes_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Button_yes.KeyUp
    '    If e.Key = Key.Enter Then
    '        Try
    '            Me.DialogResult = True
    '        Catch ex As Exception

    '        End Try
    '        e.Handled = True
    '        Me.Close()
    '    End If


    'End Sub
End Class
