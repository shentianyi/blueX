Imports KskPlugInSharedObject

Public Class TestType
    Public Function TestMethod(data As ProcessData) As ProcessData
        data.Data("done") = "This is processed by the reflected method"
        Return data
    End Function
End Class
