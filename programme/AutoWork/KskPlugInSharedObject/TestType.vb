Public Class TestType

    Public Function TestMethod(data As ProcessData) As ProcessResult
        Dim result As ProcessResult = New ProcessResult
        data.Data("done") = "This is processed by the reflected method"
        result.ReturnedValues.Add("data", data)
        Return result
    End Function
End Class
