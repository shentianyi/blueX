Imports System.Reflection

Public Class ExternalProcess
    Public Sub New()
        'please do not set construction here
    End Sub

    Public Shared Function Start(assemblyFile As String, type As String, methodName As String, data As ProcessData) As ProcessResult
        Dim result As ProcessResult
        Dim currentAssembly As Assembly = Assembly.LoadFile(assemblyFile)
        Dim processType As Type = currentAssembly.GetType(type)
        Dim method As MethodInfo = processType.GetMethod(methodName)
        result = method.Invoke(Activator.CreateInstance(processType), {data})
        Return result
    End Function

End Class
