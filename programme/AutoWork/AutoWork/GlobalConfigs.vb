Public Class GlobalConfigs


    Private Shared _dbConnStr As String = My.Settings.database
    Private Shared _lepsConnstr As String = My.Settings.lepsdb
    Public Shared ReadOnly Property DbConnStr As String
        Get
            Return _dbConnStr
        End Get
    End Property

    Public Shared ReadOnly Property LepsDb As String
        Get
            Return _lepsConnstr
        End Get
    End Property




End Class
