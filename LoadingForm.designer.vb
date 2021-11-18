<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoadingForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LoadingForm))
        Me.ProgressBarLoading = New System.Windows.Forms.ProgressBar()
        Me.LabelLoading = New System.Windows.Forms.Label()
        Me.AxMBActX1 = New AxMB3Lib4.AxMBActX()
        CType(Me.AxMBActX1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ProgressBarLoading
        '
        Me.ProgressBarLoading.ForeColor = System.Drawing.Color.SteelBlue
        Me.ProgressBarLoading.Location = New System.Drawing.Point(17, 409)
        Me.ProgressBarLoading.Name = "ProgressBarLoading"
        Me.ProgressBarLoading.Size = New System.Drawing.Size(710, 27)
        Me.ProgressBarLoading.Step = 5
        Me.ProgressBarLoading.TabIndex = 0
        '
        'LabelLoading
        '
        Me.LabelLoading.AutoSize = True
        Me.LabelLoading.BackColor = System.Drawing.Color.Transparent
        Me.LabelLoading.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelLoading.ForeColor = System.Drawing.SystemColors.ControlDark
        Me.LabelLoading.Location = New System.Drawing.Point(12, 380)
        Me.LabelLoading.Name = "LabelLoading"
        Me.LabelLoading.Size = New System.Drawing.Size(94, 26)
        Me.LabelLoading.TabIndex = 2
        Me.LabelLoading.Text = "Loading..."
        '
        'AxMBActX1
        '
        Me.AxMBActX1.Enabled = True
        Me.AxMBActX1.Location = New System.Drawing.Point(17, 12)
        Me.AxMBActX1.Name = "AxMBActX1"
        Me.AxMBActX1.OcxState = CType(resources.GetObject("AxMBActX1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxMBActX1.Size = New System.Drawing.Size(696, 365)
        Me.AxMBActX1.TabIndex = 3
        '
        'LoadingForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(739, 448)
        Me.Controls.Add(Me.AxMBActX1)
        Me.Controls.Add(Me.ProgressBarLoading)
        Me.Controls.Add(Me.LabelLoading)
        Me.Enabled = False
        Me.ForeColor = System.Drawing.SystemColors.ControlDark
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "LoadingForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "RECV"
        CType(Me.AxMBActX1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ProgressBarLoading As System.Windows.Forms.ProgressBar
    Friend WithEvents LabelLoading As System.Windows.Forms.Label
    Friend WithEvents AxMBActX1 As AxMB3Lib4.AxMBActX
End Class
