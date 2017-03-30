<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.MovingDown = New System.Windows.Forms.Timer(Me.components)
        Me.OpRegel = New System.Windows.Forms.Label()
        Me.OpScore = New System.Windows.Forms.Label()
        Me.HowToPlay = New System.Windows.Forms.Label()
        Me.RestartKnop = New System.Windows.Forms.Label()
        Me.PauseKnop = New System.Windows.Forms.Label()
        Me.OpHighscore = New System.Windows.Forms.Label()
        Me.JokerKnop = New System.Windows.Forms.Label()
        Me.JokerTimer = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'MovingDown
        '
        Me.MovingDown.Enabled = True
        Me.MovingDown.Interval = 1000
        '
        'OpRegel
        '
        Me.OpRegel.BackColor = System.Drawing.SystemColors.ControlDark
        Me.OpRegel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OpRegel.Location = New System.Drawing.Point(300, 80)
        Me.OpRegel.Name = "OpRegel"
        Me.OpRegel.Size = New System.Drawing.Size(100, 25)
        Me.OpRegel.TabIndex = 0
        Me.OpRegel.Text = "Hier komt welke regel we op zitten."
        '
        'OpScore
        '
        Me.OpScore.BackColor = System.Drawing.SystemColors.ControlDark
        Me.OpScore.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OpScore.Location = New System.Drawing.Point(300, 20)
        Me.OpScore.Name = "OpScore"
        Me.OpScore.Size = New System.Drawing.Size(100, 25)
        Me.OpScore.TabIndex = 1
        Me.OpScore.Text = "Score is: 0" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'HowToPlay
        '
        Me.HowToPlay.BackColor = System.Drawing.SystemColors.ControlDark
        Me.HowToPlay.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HowToPlay.Location = New System.Drawing.Point(300, 120)
        Me.HowToPlay.Name = "HowToPlay"
        Me.HowToPlay.Size = New System.Drawing.Size(100, 225)
        Me.HowToPlay.TabIndex = 2
        Me.HowToPlay.Text = "How to Play:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "-| Keydown:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "[right] to move right" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "[left] to move left" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "[up] to " &
    "rotate" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "-| Hold:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "[space] to speed up"
        '
        'RestartKnop
        '
        Me.RestartKnop.BackColor = System.Drawing.SystemColors.ControlDark
        Me.RestartKnop.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RestartKnop.Location = New System.Drawing.Point(300, 420)
        Me.RestartKnop.Name = "RestartKnop"
        Me.RestartKnop.Size = New System.Drawing.Size(100, 20)
        Me.RestartKnop.TabIndex = 3
        Me.RestartKnop.Text = "* Restart *" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.RestartKnop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PauseKnop
        '
        Me.PauseKnop.BackColor = System.Drawing.SystemColors.ControlDark
        Me.PauseKnop.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PauseKnop.Location = New System.Drawing.Point(300, 395)
        Me.PauseKnop.Name = "PauseKnop"
        Me.PauseKnop.Size = New System.Drawing.Size(100, 20)
        Me.PauseKnop.TabIndex = 4
        Me.PauseKnop.Text = "* Pause *" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.PauseKnop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'OpHighscore
        '
        Me.OpHighscore.BackColor = System.Drawing.SystemColors.ControlDark
        Me.OpHighscore.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OpHighscore.Location = New System.Drawing.Point(300, 50)
        Me.OpHighscore.Name = "OpHighscore"
        Me.OpHighscore.Size = New System.Drawing.Size(100, 25)
        Me.OpHighscore.TabIndex = 5
        Me.OpHighscore.Text = "Highscore: " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'JokerKnop
        '
        Me.JokerKnop.BackColor = System.Drawing.SystemColors.ControlDark
        Me.JokerKnop.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.JokerKnop.Location = New System.Drawing.Point(300, 370)
        Me.JokerKnop.Name = "JokerKnop"
        Me.JokerKnop.Size = New System.Drawing.Size(100, 20)
        Me.JokerKnop.TabIndex = 6
        Me.JokerKnop.Text = "* Joker *" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.JokerKnop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'JokerTimer
        '
        Me.JokerTimer.Interval = 1000
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.ClientSize = New System.Drawing.Size(447, 535)
        Me.Controls.Add(Me.JokerKnop)
        Me.Controls.Add(Me.OpHighscore)
        Me.Controls.Add(Me.PauseKnop)
        Me.Controls.Add(Me.RestartKnop)
        Me.Controls.Add(Me.HowToPlay)
        Me.Controls.Add(Me.OpScore)
        Me.Controls.Add(Me.OpRegel)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents MovingDown As Timer
    Friend WithEvents OpRegel As Label
    Friend WithEvents OpScore As Label
    Friend WithEvents HowToPlay As Label
    Friend WithEvents RestartKnop As Label
    Friend WithEvents PauseKnop As Label
    Friend WithEvents OpHighscore As Label
    Friend WithEvents JokerKnop As Label
    Friend WithEvents JokerTimer As Timer
End Class
