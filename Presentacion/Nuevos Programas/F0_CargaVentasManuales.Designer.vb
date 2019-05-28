<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class F0_CargaVentasManuales
    Inherits Modelos.ModeloF00


    'Form reemplaza a Dispose para limpiar la lista de componentes.
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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(F0_CargaVentasManuales))
        Me.btCargarArchivo = New DevComponents.DotNetBar.ButtonX()
        Me.btCargarDatos = New DevComponents.DotNetBar.ButtonX()
        Me.btSalir = New DevComponents.DotNetBar.ButtonX()
        Me.GroupPanel1 = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.grMigracion = New Janus.Windows.GridEX.GridEX()
        Me.PanelSuperior.SuspendLayout()
        Me.PanelInferior.SuspendLayout()
        CType(Me.BubbleBarUsuario, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelToolBar1.SuspendLayout()
        Me.PanelToolBar2.SuspendLayout()
        Me.PanelPrincipal.SuspendLayout()
        Me.PanelUsuario.SuspendLayout()
        Me.PanelNavegacion.SuspendLayout()
        Me.MPanelUserAct.SuspendLayout()
        CType(Me.MEP, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelContent.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.MSuperTabControlPanel1.SuspendLayout()
        CType(Me.MSuperTabControl, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MSuperTabControl.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupPanel1.SuspendLayout()
        CType(Me.grMigracion, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelSuperior
        '
        Me.PanelSuperior.Controls.Add(Me.btSalir)
        Me.PanelSuperior.Controls.Add(Me.btCargarDatos)
        Me.PanelSuperior.Controls.Add(Me.btCargarArchivo)
        Me.PanelSuperior.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelSuperior.Style.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(150, Byte), Integer))
        Me.PanelSuperior.Style.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(150, Byte), Integer))
        Me.PanelSuperior.Style.BackgroundImage = CType(resources.GetObject("PanelSuperior.Style.BackgroundImage"), System.Drawing.Image)
        Me.PanelSuperior.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelSuperior.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.PanelSuperior.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelSuperior.Style.GradientAngle = 90
        Me.PanelSuperior.StyleMouseDown.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(150, Byte), Integer))
        Me.PanelSuperior.StyleMouseDown.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(150, Byte), Integer))
        Me.PanelSuperior.StyleMouseOver.BackColor1.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(150, Byte), Integer))
        Me.PanelSuperior.StyleMouseOver.BackColor2.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(45, Byte), Integer), CType(CType(150, Byte), Integer))
        Me.PanelSuperior.StyleMouseOver.BackgroundImage = CType(resources.GetObject("PanelSuperior.StyleMouseOver.BackgroundImage"), System.Drawing.Image)
        Me.PanelSuperior.Controls.SetChildIndex(Me.PanelToolBar1, 0)
        Me.PanelSuperior.Controls.SetChildIndex(Me.PanelToolBar2, 0)
        Me.PanelSuperior.Controls.SetChildIndex(Me.MRlAccion, 0)
        Me.PanelSuperior.Controls.SetChildIndex(Me.PictureBox1, 0)
        Me.PanelSuperior.Controls.SetChildIndex(Me.btCargarArchivo, 0)
        Me.PanelSuperior.Controls.SetChildIndex(Me.btCargarDatos, 0)
        Me.PanelSuperior.Controls.SetChildIndex(Me.btSalir, 0)
        '
        'PanelInferior
        '
        Me.PanelInferior.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelInferior.Style.BackColor1.Color = System.Drawing.Color.Transparent
        Me.PanelInferior.Style.BackColor2.Color = System.Drawing.Color.Transparent
        Me.PanelInferior.Style.BackgroundImage = CType(resources.GetObject("PanelInferior.Style.BackgroundImage"), System.Drawing.Image)
        Me.PanelInferior.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelInferior.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.PanelInferior.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelInferior.Style.GradientAngle = 90
        '
        'BubbleBarUsuario
        '
        '
        '
        '
        Me.BubbleBarUsuario.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.BubbleBarUsuario.ButtonBackAreaStyle.BackColor = System.Drawing.Color.Transparent
        Me.BubbleBarUsuario.ButtonBackAreaStyle.BorderBottomWidth = 1
        Me.BubbleBarUsuario.ButtonBackAreaStyle.BorderColor = System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer), CType(CType(245, Byte), Integer))
        Me.BubbleBarUsuario.ButtonBackAreaStyle.BorderLeftWidth = 1
        Me.BubbleBarUsuario.ButtonBackAreaStyle.BorderRightWidth = 1
        Me.BubbleBarUsuario.ButtonBackAreaStyle.BorderTopWidth = 1
        Me.BubbleBarUsuario.ButtonBackAreaStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.BubbleBarUsuario.ButtonBackAreaStyle.PaddingBottom = 3
        Me.BubbleBarUsuario.ButtonBackAreaStyle.PaddingLeft = 3
        Me.BubbleBarUsuario.ButtonBackAreaStyle.PaddingRight = 3
        Me.BubbleBarUsuario.ButtonBackAreaStyle.PaddingTop = 3
        Me.BubbleBarUsuario.MouseOverTabColors.BorderColor = System.Drawing.SystemColors.Highlight
        Me.BubbleBarUsuario.SelectedTabColors.BorderColor = System.Drawing.Color.Black
        '
        'TxtNombreUsu
        '
        Me.TxtNombreUsu.ReadOnly = True
        Me.TxtNombreUsu.Text = "DEFAULT"
        '
        'PanelToolBar1
        '
        Me.PanelToolBar1.Size = New System.Drawing.Size(0, 89)
        '
        'btnSalir
        '
        Me.btnSalir.Size = New System.Drawing.Size(0, 89)
        Me.PanelPrincipal.Controls.SetChildIndex(Me.PanelInferior, 0)
        Me.PanelPrincipal.Controls.SetChildIndex(Me.PanelUsuario, 0)
        Me.PanelPrincipal.Controls.SetChildIndex(Me.PanelSuperior, 0)
        Me.PanelPrincipal.Controls.SetChildIndex(Me.Panel1, 0)
        '
        'PanelNavegacion
        '
        Me.PanelNavegacion.Size = New System.Drawing.Size(700, 48)
        '
        'LblPaginacion
        '
        Me.LblPaginacion.Size = New System.Drawing.Size(472, 48)
        '
        'MRlAccion
        '
        '
        '
        '
        Me.MRlAccion.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.MRlAccion.Location = New System.Drawing.Point(0, 0)
        Me.MRlAccion.Size = New System.Drawing.Size(1205, 89)
        '
        'PanelContent
        '
        Me.PanelContent.Controls.Add(Me.GroupPanel1)
        '
        'MSuperTabControl
        '
        '
        '
        '
        '
        '
        '
        Me.MSuperTabControl.ControlBox.CloseBox.Name = ""
        '
        '
        '
        Me.MSuperTabControl.ControlBox.MenuBox.Name = ""
        Me.MSuperTabControl.ControlBox.Name = ""
        Me.MSuperTabControl.ControlBox.SubItems.AddRange(New DevComponents.DotNetBar.BaseItem() {Me.MSuperTabControl.ControlBox.MenuBox, Me.MSuperTabControl.ControlBox.CloseBox})
        Me.MSuperTabControl.Controls.SetChildIndex(Me.MSuperTabControlPanel1, 0)
        '
        'PanelBuscador
        '
        Me.PanelBuscador.Size = New System.Drawing.Size(1275, 553)
        '
        'btCargarArchivo
        '
        Me.btCargarArchivo.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btCargarArchivo.BackColor = System.Drawing.Color.Transparent
        Me.btCargarArchivo.ColorTable = DevComponents.DotNetBar.eButtonColor.Magenta
        Me.btCargarArchivo.Dock = System.Windows.Forms.DockStyle.Left
        Me.btCargarArchivo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btCargarArchivo.Image = Global.Presentacion.My.Resources.Resources.folder
        Me.btCargarArchivo.ImageFixedSize = New System.Drawing.Size(48, 48)
        Me.btCargarArchivo.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btCargarArchivo.Location = New System.Drawing.Point(0, 0)
        Me.btCargarArchivo.Margin = New System.Windows.Forms.Padding(4)
        Me.btCargarArchivo.Name = "btCargarArchivo"
        Me.btCargarArchivo.Size = New System.Drawing.Size(159, 89)
        Me.btCargarArchivo.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btCargarArchivo.TabIndex = 10
        Me.btCargarArchivo.Text = "CARGAR ARCHIVOS"
        Me.btCargarArchivo.TextColor = System.Drawing.Color.White
        '
        'btCargarDatos
        '
        Me.btCargarDatos.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btCargarDatos.ColorTable = DevComponents.DotNetBar.eButtonColor.Orange
        Me.btCargarDatos.Dock = System.Windows.Forms.DockStyle.Left
        Me.btCargarDatos.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btCargarDatos.Image = Global.Presentacion.My.Resources.Resources.save
        Me.btCargarDatos.ImageFixedSize = New System.Drawing.Size(48, 48)
        Me.btCargarDatos.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btCargarDatos.Location = New System.Drawing.Point(159, 0)
        Me.btCargarDatos.Margin = New System.Windows.Forms.Padding(4)
        Me.btCargarDatos.Name = "btCargarDatos"
        Me.btCargarDatos.Size = New System.Drawing.Size(143, 89)
        Me.btCargarDatos.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btCargarDatos.TabIndex = 11
        Me.btCargarDatos.Text = "GUARDAR DATOS"
        Me.btCargarDatos.TextColor = System.Drawing.Color.White
        '
        'btSalir
        '
        Me.btSalir.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btSalir.ColorTable = DevComponents.DotNetBar.eButtonColor.Orange
        Me.btSalir.Dock = System.Windows.Forms.DockStyle.Left
        Me.btSalir.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btSalir.Image = Global.Presentacion.My.Resources.Resources.atras
        Me.btSalir.ImageFixedSize = New System.Drawing.Size(48, 48)
        Me.btSalir.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
        Me.btSalir.Location = New System.Drawing.Point(302, 0)
        Me.btSalir.Margin = New System.Windows.Forms.Padding(4)
        Me.btSalir.Name = "btSalir"
        Me.btSalir.Size = New System.Drawing.Size(96, 89)
        Me.btSalir.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btSalir.TabIndex = 12
        Me.btSalir.Text = "SALIR"
        Me.btSalir.TextColor = System.Drawing.Color.White
        '
        'GroupPanel1
        '
        Me.GroupPanel1.CanvasColor = System.Drawing.SystemColors.Control
        Me.GroupPanel1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.GroupPanel1.Controls.Add(Me.grMigracion)
        Me.GroupPanel1.DisabledBackColor = System.Drawing.Color.Empty
        Me.GroupPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupPanel1.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupPanel1.Location = New System.Drawing.Point(0, 0)
        Me.GroupPanel1.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupPanel1.Name = "GroupPanel1"
        Me.GroupPanel1.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupPanel1.Size = New System.Drawing.Size(1275, 553)
        '
        '
        '
        Me.GroupPanel1.Style.BackColor = System.Drawing.Color.DodgerBlue
        Me.GroupPanel1.Style.BackColor2 = System.Drawing.Color.DodgerBlue
        Me.GroupPanel1.Style.BackColorGradientAngle = 90
        Me.GroupPanel1.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel1.Style.BorderBottomWidth = 1
        Me.GroupPanel1.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.GroupPanel1.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel1.Style.BorderLeftWidth = 1
        Me.GroupPanel1.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel1.Style.BorderRightWidth = 1
        Me.GroupPanel1.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.GroupPanel1.Style.BorderTopWidth = 1
        Me.GroupPanel1.Style.CornerDiameter = 4
        Me.GroupPanel1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.GroupPanel1.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.GroupPanel1.Style.TextColor = System.Drawing.Color.White
        Me.GroupPanel1.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.GroupPanel1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.GroupPanel1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.GroupPanel1.TabIndex = 21
        Me.GroupPanel1.Text = "DATOS MIGRACION"
        '
        'grMigracion
        '
        Me.grMigracion.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.[False]
        Me.grMigracion.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grMigracion.Font = New System.Drawing.Font("Arial", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grMigracion.HeaderFormatStyle.Font = New System.Drawing.Font("Arial Narrow", 10.2!, System.Drawing.FontStyle.Bold)
        Me.grMigracion.HeaderFormatStyle.FontBold = Janus.Windows.GridEX.TriState.[True]
        Me.grMigracion.Location = New System.Drawing.Point(4, 4)
        Me.grMigracion.Margin = New System.Windows.Forms.Padding(4)
        Me.grMigracion.Name = "grMigracion"
        Me.grMigracion.Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme.Custom
        Me.grMigracion.Office2007CustomColor = System.Drawing.Color.DodgerBlue
        Me.grMigracion.Size = New System.Drawing.Size(1261, 518)
        Me.grMigracion.TabIndex = 0
        Me.grMigracion.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007
        '
        'F0_CargaVentasManuales
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1312, 690)
        Me.Name = "F0_CargaVentasManuales"
        Me.Text = "CARGA DE ARCHIVOS"
        Me.Controls.SetChildIndex(Me.PanelPrincipal, 0)
        Me.PanelSuperior.ResumeLayout(False)
        Me.PanelInferior.ResumeLayout(False)
        CType(Me.BubbleBarUsuario, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelToolBar1.ResumeLayout(False)
        Me.PanelToolBar2.ResumeLayout(False)
        Me.PanelPrincipal.ResumeLayout(False)
        Me.PanelUsuario.ResumeLayout(False)
        Me.PanelUsuario.PerformLayout()
        Me.PanelNavegacion.ResumeLayout(False)
        Me.MPanelUserAct.ResumeLayout(False)
        Me.MPanelUserAct.PerformLayout()
        CType(Me.MEP, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelContent.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.MSuperTabControlPanel1.ResumeLayout(False)
        CType(Me.MSuperTabControl, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MSuperTabControl.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupPanel1.ResumeLayout(False)
        CType(Me.grMigracion, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Protected WithEvents btCargarArchivo As DevComponents.DotNetBar.ButtonX
    Protected WithEvents btCargarDatos As DevComponents.DotNetBar.ButtonX
    Protected WithEvents btSalir As DevComponents.DotNetBar.ButtonX
    Friend WithEvents GroupPanel1 As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents grMigracion As Janus.Windows.GridEX.GridEX
End Class
