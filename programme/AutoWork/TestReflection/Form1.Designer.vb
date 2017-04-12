<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。  
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.textBox_ordernr = New System.Windows.Forms.TextBox()
        Me.textBox_ksknr = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(146, 237)
        Me.Button1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(112, 34)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'textBox_ordernr
        '
        Me.textBox_ordernr.Location = New System.Drawing.Point(77, 66)
        Me.textBox_ordernr.Name = "textBox_ordernr"
        Me.textBox_ordernr.Size = New System.Drawing.Size(290, 28)
        Me.textBox_ordernr.TabIndex = 1
        '
        'textBox_ksknr
        '
        Me.textBox_ksknr.Location = New System.Drawing.Point(77, 119)
        Me.textBox_ksknr.Name = "textBox_ksknr"
        Me.textBox_ksknr.Size = New System.Drawing.Size(290, 28)
        Me.textBox_ksknr.TabIndex = 2
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 18.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(435, 393)
        Me.Controls.Add(Me.textBox_ksknr)
        Me.Controls.Add(Me.textBox_ordernr)
        Me.Controls.Add(Me.Button1)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button1 As Windows.Forms.Button
    Friend WithEvents textBox_ordernr As Windows.Forms.TextBox
    Friend WithEvents textBox_ksknr As Windows.Forms.TextBox
End Class
