Imports System.Reflection
Imports System.Text.RegularExpressions
Imports KskPlugInSharedObject
Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim tr As Regex = New Regex("^TC\w+")
        Dim kr As Regex = New Regex("^4([a-zA-Z0-9]{9})(PR|EN)$")
        If tr.IsMatch(textBox_ordernr.Text) And kr.IsMatch(textBox_ksknr.Text) Then

            MsgBox("OK")

        Else
            MsgBox("请扫描正确的小车号和KSK号")

        End If

        'Dim result As ProcessResult = ExternalProcess.Start(System.IO.Path.Combine(My.Application.Info.DirectoryPath, "KskPlugInSharedObject.dll"), "KskPlugInSharedObject.TestType", "TestMethod", New ProcessData)
        'MsgBox("result returned: " & result.ReturnedValues("data").data("done"))
    End Sub
End Class