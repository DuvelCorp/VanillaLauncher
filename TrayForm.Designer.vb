<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TrayForm
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
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

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TrayForm))
        Me.TrayMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.MenuFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuLinks = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImageListLinks = New System.Windows.Forms.ImageList(Me.components)
        Me.TrayMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'TrayMenu
        '
        Me.TrayMenu.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.TrayMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuFolder, Me.MenuExit, Me.MenuLinks})
        Me.TrayMenu.Name = "TrayMenu"
        Me.TrayMenu.Size = New System.Drawing.Size(112, 82)
        '
        'MenuFolder
        '
        Me.MenuFolder.Image = My.Resources.Resources.folder
        Me.MenuFolder.Name = "MenuFolder"
        Me.MenuFolder.Size = New System.Drawing.Size(111, 26)
        Me.MenuFolder.Text = "Folder"
        '
        'MenuExit
        '
        Me.MenuExit.Image = My.Resources.Resources.Close_icon
        Me.MenuExit.Name = "MenuExit"
        Me.MenuExit.Size = New System.Drawing.Size(111, 26)
        Me.MenuExit.Text = "Exit"
        '
        'MenuLinks
        '
        Me.MenuLinks.Image = CType(resources.GetObject("MenuLinks.Image"), System.Drawing.Image)
        Me.MenuLinks.Name = "MenuLinks"
        Me.MenuLinks.Size = New System.Drawing.Size(111, 26)
        Me.MenuLinks.Text = "Links"
        '
        'ImageListLinks
        '
        Me.ImageListLinks.ImageStream = CType(resources.GetObject("ImageListLinks.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageListLinks.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageListLinks.Images.SetKeyName(0, "anonymous.png")
        Me.ImageListLinks.Images.SetKeyName(1, "axe.png")
        Me.ImageListLinks.Images.SetKeyName(2, "bat.png")
        Me.ImageListLinks.Images.SetKeyName(3, "blizz.gif")
        Me.ImageListLinks.Images.SetKeyName(4, "calendar.png")
        Me.ImageListLinks.Images.SetKeyName(5, "chrome.png")
        Me.ImageListLinks.Images.SetKeyName(6, "crown.png")
        Me.ImageListLinks.Images.SetKeyName(7, "cup.png")
        Me.ImageListLinks.Images.SetKeyName(8, "email.png")
        Me.ImageListLinks.Images.SetKeyName(9, "fb.png")
        Me.ImageListLinks.Images.SetKeyName(10, "fire.png")
        Me.ImageListLinks.Images.SetKeyName(11, "firefox.png")
        Me.ImageListLinks.Images.SetKeyName(12, "forum.png")
        Me.ImageListLinks.Images.SetKeyName(13, "globe.png")
        Me.ImageListLinks.Images.SetKeyName(14, "google.png")
        Me.ImageListLinks.Images.SetKeyName(15, "graph.png")
        Me.ImageListLinks.Images.SetKeyName(16, "gun.png")
        Me.ImageListLinks.Images.SetKeyName(17, "ham.png")
        Me.ImageListLinks.Images.SetKeyName(18, "helm.png")
        Me.ImageListLinks.Images.SetKeyName(19, "icon_forum.png")
        Me.ImageListLinks.Images.SetKeyName(20, "instagram.png")
        Me.ImageListLinks.Images.SetKeyName(21, "moneybag.png")
        Me.ImageListLinks.Images.SetKeyName(22, "nuclear.png")
        Me.ImageListLinks.Images.SetKeyName(23, "pinter.png")
        Me.ImageListLinks.Images.SetKeyName(24, "pray.png")
        Me.ImageListLinks.Images.SetKeyName(25, "safari.png")
        Me.ImageListLinks.Images.SetKeyName(26, "skull.png")
        Me.ImageListLinks.Images.SetKeyName(27, "star.png")
        Me.ImageListLinks.Images.SetKeyName(28, "steam.png")
        Me.ImageListLinks.Images.SetKeyName(29, "talk.png")
        Me.ImageListLinks.Images.SetKeyName(30, "target.png")
        Me.ImageListLinks.Images.SetKeyName(31, "twitch.png")
        Me.ImageListLinks.Images.SetKeyName(32, "twitter.png")
        Me.ImageListLinks.Images.SetKeyName(33, "wow.png")
        Me.ImageListLinks.Images.SetKeyName(34, "write.png")
        Me.ImageListLinks.Images.SetKeyName(35, "youtube.png")
        '
        'TrayForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(212, 206)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Name = "TrayForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "VanillaLauncher"
        Me.TopMost = True
        Me.TrayMenu.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TrayMenu As ContextMenuStrip
    Friend WithEvents MenuLinks As ToolStripMenuItem
    Friend WithEvents MenuFolder As ToolStripMenuItem
    Friend WithEvents MenuExit As ToolStripMenuItem
    Friend WithEvents ImageListLinks As ImageList
End Class
