Public Class LackProcessDataException
    Inherits Exception

    Public Sub New()
        MyBase.New("ProcessData缺少参数")
    End Sub

End Class
