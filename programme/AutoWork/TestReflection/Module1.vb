Imports System.Reflection
Imports KskPlugInSharedObject

Module Module1

    Sub Main()
        Dim assemblyPath As String = Console.ReadLine()
        Dim currentAssembly As Assembly = Assembly.LoadFile(System.IO.Path.Combine(My.Application.Info.DirectoryPath, "KskPlugInSharedObject.dll"))
        For Each ccType As Type In currentAssembly.GetTypes
            Console.WriteLine(ccType.FullName)
        Next
        Dim currType As Type = currentAssembly.GetType("KskPlugInSharedObject.TestType")
        Dim method As MethodInfo = currType.GetMethod("TestMethod")
        Dim target As Object = New Object
        Dim result As ProcessData = method.Invoke(Nothing, {New ProcessData})
        Console.WriteLine("result returned: " & result.Data("done"))
        Console.Read()
    End Sub

End Module
