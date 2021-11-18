<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainUserForm
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainUserForm))
        Me.ListBoxQueue = New System.Windows.Forms.ListBox()
        Me.LabelQueue = New System.Windows.Forms.Label()
        Me.ButtonStart = New System.Windows.Forms.Button()
        Me.ButtonRefresh = New System.Windows.Forms.Button()
        Me.ButtonAddQueue = New System.Windows.Forms.Button()
        Me.ButtonRemove = New System.Windows.Forms.Button()
        Me.ButtonSettings = New System.Windows.Forms.Button()
        Me.ButtonMoveRobot = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.ButtonClose = New System.Windows.Forms.Button()
        Me.PanelToolbar = New System.Windows.Forms.Panel()
        Me.ButtonMinimize = New System.Windows.Forms.Button()
        Me.LabelClock = New System.Windows.Forms.Label()
        Me.LabelToolbar = New System.Windows.Forms.Label()
        Me.TimerClock = New System.Windows.Forms.Timer(Me.components)
        Me.PictureBoxCaution = New System.Windows.Forms.PictureBox()
        Me.LabelCaution = New System.Windows.Forms.Label()
        Me.AxMBActX1 = New AxMB3Lib4.AxMBActX()
        Me.bcgWorkMain = New System.ComponentModel.BackgroundWorker()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelToolbar.SuspendLayout()
        CType(Me.PictureBoxCaution, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AxMBActX1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ListBoxQueue
        '
        Me.ListBoxQueue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ListBoxQueue.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListBoxQueue.FormattingEnabled = True
        Me.ListBoxQueue.ItemHeight = 15
        Me.ListBoxQueue.Location = New System.Drawing.Point(1457, 91)
        Me.ListBoxQueue.Name = "ListBoxQueue"
        Me.ListBoxQueue.Size = New System.Drawing.Size(435, 874)
        Me.ListBoxQueue.TabIndex = 0
        '
        'LabelQueue
        '
        Me.LabelQueue.AutoSize = True
        Me.LabelQueue.Font = New System.Drawing.Font("Calibri", 20.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelQueue.Location = New System.Drawing.Point(1451, 55)
        Me.LabelQueue.Name = "LabelQueue"
        Me.LabelQueue.Size = New System.Drawing.Size(188, 33)
        Me.LabelQueue.TabIndex = 1
        Me.LabelQueue.Text = "Engraver Queue"
        '
        'ButtonStart
        '
        Me.ButtonStart.BackColor = System.Drawing.Color.FromArgb(CType(CType(150, Byte), Integer), CType(CType(150, Byte), Integer), CType(CType(150, Byte), Integer))
        Me.ButtonStart.FlatAppearance.BorderColor = System.Drawing.Color.DimGray
        Me.ButtonStart.FlatAppearance.BorderSize = 2
        Me.ButtonStart.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray
        Me.ButtonStart.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver
        Me.ButtonStart.Font = New System.Drawing.Font("Calibri", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonStart.ForeColor = System.Drawing.SystemColors.Control
        Me.ButtonStart.Location = New System.Drawing.Point(723, 111)
        Me.ButtonStart.Name = "ButtonStart"
        Me.ButtonStart.Size = New System.Drawing.Size(285, 59)
        Me.ButtonStart.TabIndex = 2
        Me.ButtonStart.Text = "Start Engraving"
        Me.ButtonStart.UseVisualStyleBackColor = False
        '
        'ButtonRefresh
        '
        Me.ButtonRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButtonRefresh.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer))
        Me.ButtonRefresh.FlatAppearance.BorderSize = 0
        Me.ButtonRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(75, Byte), Integer), CType(CType(75, Byte), Integer), CType(CType(75, Byte), Integer))
        Me.ButtonRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(175, Byte), Integer), CType(CType(175, Byte), Integer), CType(CType(175, Byte), Integer))
        Me.ButtonRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonRefresh.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonRefresh.ForeColor = System.Drawing.SystemColors.Control
        Me.ButtonRefresh.Location = New System.Drawing.Point(1457, 971)
        Me.ButtonRefresh.Name = "ButtonRefresh"
        Me.ButtonRefresh.Size = New System.Drawing.Size(161, 38)
        Me.ButtonRefresh.TabIndex = 3
        Me.ButtonRefresh.Text = "Refresh Queue"
        Me.ButtonRefresh.UseVisualStyleBackColor = False
        '
        'ButtonAddQueue
        '
        Me.ButtonAddQueue.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButtonAddQueue.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer))
        Me.ButtonAddQueue.FlatAppearance.BorderSize = 0
        Me.ButtonAddQueue.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(75, Byte), Integer), CType(CType(75, Byte), Integer), CType(CType(75, Byte), Integer))
        Me.ButtonAddQueue.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(175, Byte), Integer), CType(CType(175, Byte), Integer), CType(CType(175, Byte), Integer))
        Me.ButtonAddQueue.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAddQueue.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAddQueue.ForeColor = System.Drawing.SystemColors.Control
        Me.ButtonAddQueue.Location = New System.Drawing.Point(1792, 972)
        Me.ButtonAddQueue.Name = "ButtonAddQueue"
        Me.ButtonAddQueue.Size = New System.Drawing.Size(100, 38)
        Me.ButtonAddQueue.TabIndex = 4
        Me.ButtonAddQueue.Text = "Add to Queue"
        Me.ButtonAddQueue.UseVisualStyleBackColor = False
        '
        'ButtonRemove
        '
        Me.ButtonRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButtonRemove.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer))
        Me.ButtonRemove.FlatAppearance.BorderSize = 0
        Me.ButtonRemove.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(75, Byte), Integer), CType(CType(75, Byte), Integer), CType(CType(75, Byte), Integer))
        Me.ButtonRemove.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(175, Byte), Integer), CType(CType(175, Byte), Integer), CType(CType(175, Byte), Integer))
        Me.ButtonRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonRemove.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonRemove.ForeColor = System.Drawing.SystemColors.Control
        Me.ButtonRemove.Location = New System.Drawing.Point(1686, 972)
        Me.ButtonRemove.Name = "ButtonRemove"
        Me.ButtonRemove.Size = New System.Drawing.Size(100, 38)
        Me.ButtonRemove.TabIndex = 5
        Me.ButtonRemove.Text = "Remove from Queue"
        Me.ButtonRemove.UseVisualStyleBackColor = False
        '
        'ButtonSettings
        '
        Me.ButtonSettings.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButtonSettings.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer))
        Me.ButtonSettings.FlatAppearance.BorderSize = 0
        Me.ButtonSettings.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(75, Byte), Integer), CType(CType(75, Byte), Integer), CType(CType(75, Byte), Integer))
        Me.ButtonSettings.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(175, Byte), Integer), CType(CType(175, Byte), Integer), CType(CType(175, Byte), Integer))
        Me.ButtonSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonSettings.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonSettings.ForeColor = System.Drawing.SystemColors.Control
        Me.ButtonSettings.Location = New System.Drawing.Point(18, 921)
        Me.ButtonSettings.Name = "ButtonSettings"
        Me.ButtonSettings.Size = New System.Drawing.Size(189, 34)
        Me.ButtonSettings.TabIndex = 6
        Me.ButtonSettings.Text = "Settings"
        Me.ButtonSettings.UseVisualStyleBackColor = False
        '
        'ButtonMoveRobot
        '
        Me.ButtonMoveRobot.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ButtonMoveRobot.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer))
        Me.ButtonMoveRobot.FlatAppearance.BorderSize = 0
        Me.ButtonMoveRobot.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(75, Byte), Integer), CType(CType(75, Byte), Integer), CType(CType(75, Byte), Integer))
        Me.ButtonMoveRobot.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(175, Byte), Integer), CType(CType(175, Byte), Integer), CType(CType(175, Byte), Integer))
        Me.ButtonMoveRobot.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonMoveRobot.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonMoveRobot.ForeColor = System.Drawing.SystemColors.Control
        Me.ButtonMoveRobot.Location = New System.Drawing.Point(18, 961)
        Me.ButtonMoveRobot.Name = "ButtonMoveRobot"
        Me.ButtonMoveRobot.Size = New System.Drawing.Size(189, 36)
        Me.ButtonMoveRobot.TabIndex = 7
        Me.ButtonMoveRobot.Text = "Move Robot"
        Me.ButtonMoveRobot.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.SystemColors.ControlDark
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.PictureBox1)
        Me.Panel1.Controls.Add(Me.ButtonMoveRobot)
        Me.Panel1.Controls.Add(Me.ButtonSettings)
        Me.Panel1.Location = New System.Drawing.Point(-7, -8)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(207, 1544)
        Me.Panel1.TabIndex = 8
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.Control
        Me.Label1.Location = New System.Drawing.Point(36, 190)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(127, 23)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "UR10 Engraver"
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(19, -11)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(209, 198)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBox1.TabIndex = 8
        Me.PictureBox1.TabStop = False
        '
        'ButtonClose
        '
        Me.ButtonClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonClose.BackColor = System.Drawing.Color.Transparent
        Me.ButtonClose.FlatAppearance.BorderSize = 0
        Me.ButtonClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ButtonClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red
        Me.ButtonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonClose.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonClose.ForeColor = System.Drawing.SystemColors.Control
        Me.ButtonClose.Location = New System.Drawing.Point(1885, 8)
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.Size = New System.Drawing.Size(30, 30)
        Me.ButtonClose.TabIndex = 10
        Me.ButtonClose.Text = "X"
        Me.ButtonClose.UseVisualStyleBackColor = False
        '
        'PanelToolbar
        '
        Me.PanelToolbar.BackColor = System.Drawing.SystemColors.ControlDarkDark
        Me.PanelToolbar.Controls.Add(Me.ButtonMinimize)
        Me.PanelToolbar.Controls.Add(Me.LabelClock)
        Me.PanelToolbar.Controls.Add(Me.LabelToolbar)
        Me.PanelToolbar.Controls.Add(Me.ButtonClose)
        Me.PanelToolbar.Location = New System.Drawing.Point(-7, -5)
        Me.PanelToolbar.Name = "PanelToolbar"
        Me.PanelToolbar.Size = New System.Drawing.Size(1949, 39)
        Me.PanelToolbar.TabIndex = 11
        '
        'ButtonMinimize
        '
        Me.ButtonMinimize.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonMinimize.BackColor = System.Drawing.Color.Transparent
        Me.ButtonMinimize.FlatAppearance.BorderSize = 0
        Me.ButtonMinimize.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ButtonMinimize.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(100, Byte), Integer))
        Me.ButtonMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonMinimize.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonMinimize.ForeColor = System.Drawing.SystemColors.Control
        Me.ButtonMinimize.Location = New System.Drawing.Point(1826, 6)
        Me.ButtonMinimize.Name = "ButtonMinimize"
        Me.ButtonMinimize.Size = New System.Drawing.Size(26, 30)
        Me.ButtonMinimize.TabIndex = 14
        Me.ButtonMinimize.Text = "_"
        Me.ButtonMinimize.UseVisualStyleBackColor = False
        '
        'LabelClock
        '
        Me.LabelClock.AutoSize = True
        Me.LabelClock.BackColor = System.Drawing.Color.Transparent
        Me.LabelClock.Font = New System.Drawing.Font("Calibri", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelClock.ForeColor = System.Drawing.SystemColors.Control
        Me.LabelClock.Location = New System.Drawing.Point(834, 14)
        Me.LabelClock.Name = "LabelClock"
        Me.LabelClock.Size = New System.Drawing.Size(79, 19)
        Me.LabelClock.TabIndex = 13
        Me.LabelClock.Text = "LabelClock"
        '
        'LabelToolbar
        '
        Me.LabelToolbar.AutoSize = True
        Me.LabelToolbar.BackColor = System.Drawing.Color.Transparent
        Me.LabelToolbar.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelToolbar.ForeColor = System.Drawing.SystemColors.Control
        Me.LabelToolbar.Location = New System.Drawing.Point(19, 11)
        Me.LabelToolbar.Name = "LabelToolbar"
        Me.LabelToolbar.Size = New System.Drawing.Size(228, 23)
        Me.LabelToolbar.TabIndex = 12
        Me.LabelToolbar.Text = "UR10 Engraving Automation"
        '
        'TimerClock
        '
        '
        'PictureBoxCaution
        '
        Me.PictureBoxCaution.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.PictureBoxCaution.Image = CType(resources.GetObject("PictureBoxCaution.Image"), System.Drawing.Image)
        Me.PictureBoxCaution.Location = New System.Drawing.Point(783, 201)
        Me.PictureBoxCaution.Name = "PictureBoxCaution"
        Me.PictureBoxCaution.Size = New System.Drawing.Size(150, 150)
        Me.PictureBoxCaution.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PictureBoxCaution.TabIndex = 12
        Me.PictureBoxCaution.TabStop = False
        '
        'LabelCaution
        '
        Me.LabelCaution.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.LabelCaution.AutoSize = True
        Me.LabelCaution.Font = New System.Drawing.Font("Calibri", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelCaution.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelCaution.Location = New System.Drawing.Point(730, 354)
        Me.LabelCaution.Name = "LabelCaution"
        Me.LabelCaution.Size = New System.Drawing.Size(278, 29)
        Me.LabelCaution.TabIndex = 13
        Me.LabelCaution.Text = "Caution:  Robot is Running"
        '
        'AxMBActX1
        '
        Me.AxMBActX1.Enabled = True
        Me.AxMBActX1.Location = New System.Drawing.Point(492, 465)
        Me.AxMBActX1.Name = "AxMBActX1"
        Me.AxMBActX1.OcxState = CType(resources.GetObject("AxMBActX1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxMBActX1.Size = New System.Drawing.Size(818, 544)
        Me.AxMBActX1.TabIndex = 14
        '
        'bcgWorkMain
        '
        '
        'MainUserForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1920, 1061)
        Me.Controls.Add(Me.AxMBActX1)
        Me.Controls.Add(Me.LabelCaution)
        Me.Controls.Add(Me.PictureBoxCaution)
        Me.Controls.Add(Me.PanelToolbar)
        Me.Controls.Add(Me.ListBoxQueue)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ButtonRemove)
        Me.Controls.Add(Me.ButtonAddQueue)
        Me.Controls.Add(Me.ButtonRefresh)
        Me.Controls.Add(Me.ButtonStart)
        Me.Controls.Add(Me.LabelQueue)
        Me.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "MainUserForm"
        Me.Text = "EAV"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelToolbar.ResumeLayout(False)
        Me.PanelToolbar.PerformLayout()
        CType(Me.PictureBoxCaution, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AxMBActX1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ListBoxQueue As System.Windows.Forms.ListBox
    Friend WithEvents LabelQueue As System.Windows.Forms.Label
    Friend WithEvents ButtonStart As System.Windows.Forms.Button
    Friend WithEvents ButtonRefresh As System.Windows.Forms.Button
    Friend WithEvents ButtonAddQueue As System.Windows.Forms.Button
    Friend WithEvents ButtonRemove As System.Windows.Forms.Button
    Friend WithEvents ButtonSettings As System.Windows.Forms.Button
    Friend WithEvents ButtonMoveRobot As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ButtonClose As System.Windows.Forms.Button
    Friend WithEvents PanelToolbar As System.Windows.Forms.Panel
    Friend WithEvents LabelToolbar As System.Windows.Forms.Label
    Friend WithEvents TimerClock As System.Windows.Forms.Timer
    Friend WithEvents LabelClock As System.Windows.Forms.Label
    Friend WithEvents ButtonMinimize As System.Windows.Forms.Button
    Friend WithEvents PictureBoxCaution As System.Windows.Forms.PictureBox
    Friend WithEvents LabelCaution As System.Windows.Forms.Label
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents AxMBActX1 As AxMB3Lib4.AxMBActX
    Friend WithEvents bcgWorkMain As System.ComponentModel.BackgroundWorker

End Class
