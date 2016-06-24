Imports System.Reflection
Imports KskPlugInSharedObject
Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim result As ProcessResult = ExternalProcess.Start(System.IO.Path.Combine(My.Application.Info.DirectoryPath, "KskPlugInSharedObject.dll"), "KskPlugInSharedObject.TestType", "TestMethod", New ProcessData)
        MsgBox("result returned: " & result.ReturnedValues("data").data("done"))
    End Sub
End Class