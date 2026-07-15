namespace transformador_mda.src.Vistas
{
    partial class FormPrincipal
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ImageList imageList;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrincipal));
            this.panelAbrirXMI = new System.Windows.Forms.Panel();
            this.panelInferior = new System.Windows.Forms.Panel();
            this.buttonContinuar = new System.Windows.Forms.Button();
            this.panelCentral = new System.Windows.Forms.Panel();
            this.buttonDirectorioRaiz = new System.Windows.Forms.Button();
            this.buttonBuscarFicheroDeEntrada = new System.Windows.Forms.Button();
            this.textBoxRutaFicheroDeEntrada = new System.Windows.Forms.TextBox();
            this.RutaFicheroEntrada = new System.Windows.Forms.Label();
            this.textBoxRutaRaiz = new System.Windows.Forms.TextBox();
            this.labelRutaRaiz = new System.Windows.Forms.Label();
            this.textBoxNombreProyecto = new System.Windows.Forms.TextBox();
            this.labelNombreProyecto = new System.Windows.Forms.Label();
            this.treeViewEstructura = new System.Windows.Forms.TreeView();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.configuraciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importarDiagramaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importarXMIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelPrincipal = new System.Windows.Forms.Panel();
            this.panelIzquierdo = new System.Windows.Forms.Panel();
            this.treeView = new System.Windows.Forms.TreeView();
            this.textBoxBuscarComponente = new System.Windows.Forms.TextBox();
            imageList = new System.Windows.Forms.ImageList(this.components);
            this.panelAbrirXMI.SuspendLayout();
            this.panelInferior.SuspendLayout();
            this.panelCentral.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.panelPrincipal.SuspendLayout();
            this.panelIzquierdo.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList
            // 
            imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            imageList.TransparentColor = System.Drawing.Color.Transparent;
            imageList.Images.SetKeyName(0, "carpeta");
            imageList.Images.SetKeyName(1, "clase");
            imageList.Images.SetKeyName(2, "enumeracion");
            imageList.Images.SetKeyName(3, "interfaz");
            // 
            // panelAbrirXMI
            // 
            this.panelAbrirXMI.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panelAbrirXMI.Controls.Add(this.panelInferior);
            this.panelAbrirXMI.Controls.Add(this.panelCentral);
            this.panelAbrirXMI.Location = new System.Drawing.Point(170, 145);
            this.panelAbrirXMI.Name = "panelAbrirXMI";
            this.panelAbrirXMI.Size = new System.Drawing.Size(439, 226);
            this.panelAbrirXMI.TabIndex = 0;
            // 
            // panelInferior
            // 
            this.panelInferior.BackColor = System.Drawing.SystemColors.Control;
            this.panelInferior.Controls.Add(this.buttonContinuar);
            this.panelInferior.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelInferior.Location = new System.Drawing.Point(0, 187);
            this.panelInferior.Name = "panelInferior";
            this.panelInferior.Size = new System.Drawing.Size(439, 39);
            this.panelInferior.TabIndex = 2;
            // 
            // buttonContinuar
            // 
            this.buttonContinuar.Enabled = false;
            this.buttonContinuar.Location = new System.Drawing.Point(351, 13);
            this.buttonContinuar.Name = "buttonContinuar";
            this.buttonContinuar.Size = new System.Drawing.Size(75, 23);
            this.buttonContinuar.TabIndex = 0;
            this.buttonContinuar.Text = "Continuar ->";
            this.buttonContinuar.UseVisualStyleBackColor = true;
            this.buttonContinuar.Click += new System.EventHandler(this.buttonContinuar_Click);
            // 
            // panelCentral
            // 
            this.panelCentral.BackColor = System.Drawing.SystemColors.Control;
            this.panelCentral.Controls.Add(this.buttonDirectorioRaiz);
            this.panelCentral.Controls.Add(this.buttonBuscarFicheroDeEntrada);
            this.panelCentral.Controls.Add(this.textBoxRutaFicheroDeEntrada);
            this.panelCentral.Controls.Add(this.RutaFicheroEntrada);
            this.panelCentral.Controls.Add(this.textBoxRutaRaiz);
            this.panelCentral.Controls.Add(this.labelRutaRaiz);
            this.panelCentral.Controls.Add(this.textBoxNombreProyecto);
            this.panelCentral.Controls.Add(this.labelNombreProyecto);
            this.panelCentral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCentral.Location = new System.Drawing.Point(0, 0);
            this.panelCentral.Name = "panelCentral";
            this.panelCentral.Size = new System.Drawing.Size(439, 226);
            this.panelCentral.TabIndex = 1;
            // 
            // buttonDirectorioRaiz
            // 
            this.buttonDirectorioRaiz.Location = new System.Drawing.Point(406, 59);
            this.buttonDirectorioRaiz.Name = "buttonDirectorioRaiz";
            this.buttonDirectorioRaiz.Size = new System.Drawing.Size(20, 20);
            this.buttonDirectorioRaiz.TabIndex = 10;
            this.buttonDirectorioRaiz.Text = "🔎";
            this.buttonDirectorioRaiz.UseVisualStyleBackColor = true;
            this.buttonDirectorioRaiz.Click += new System.EventHandler(this.buttonDirectorioRaiz_Click);
            // 
            // buttonBuscarFicheroDeEntrada
            // 
            this.buttonBuscarFicheroDeEntrada.Location = new System.Drawing.Point(406, 159);
            this.buttonBuscarFicheroDeEntrada.Name = "buttonBuscarFicheroDeEntrada";
            this.buttonBuscarFicheroDeEntrada.Size = new System.Drawing.Size(20, 20);
            this.buttonBuscarFicheroDeEntrada.TabIndex = 9;
            this.buttonBuscarFicheroDeEntrada.Text = "🔎";
            this.buttonBuscarFicheroDeEntrada.UseVisualStyleBackColor = true;
            this.buttonBuscarFicheroDeEntrada.Click += new System.EventHandler(this.buttonBuscarFicheroDeEntrada_Click);
            // 
            // textBoxRutaFicheroDeEntrada
            // 
            this.textBoxRutaFicheroDeEntrada.BackColor = System.Drawing.SystemColors.Info;
            this.textBoxRutaFicheroDeEntrada.Enabled = false;
            this.textBoxRutaFicheroDeEntrada.Location = new System.Drawing.Point(24, 159);
            this.textBoxRutaFicheroDeEntrada.Name = "textBoxRutaFicheroDeEntrada";
            this.textBoxRutaFicheroDeEntrada.Size = new System.Drawing.Size(376, 20);
            this.textBoxRutaFicheroDeEntrada.TabIndex = 7;
            // 
            // RutaFicheroEntrada
            // 
            this.RutaFicheroEntrada.AutoSize = true;
            this.RutaFicheroEntrada.Location = new System.Drawing.Point(21, 143);
            this.RutaFicheroEntrada.Name = "RutaFicheroEntrada";
            this.RutaFicheroEntrada.Size = new System.Drawing.Size(136, 13);
            this.RutaFicheroEntrada.TabIndex = 6;
            this.RutaFicheroEntrada.Text = "Ruta del fichero de entrada";
            // 
            // textBoxRutaRaiz
            // 
            this.textBoxRutaRaiz.BackColor = System.Drawing.SystemColors.Info;
            this.textBoxRutaRaiz.Enabled = false;
            this.textBoxRutaRaiz.Location = new System.Drawing.Point(24, 59);
            this.textBoxRutaRaiz.Name = "textBoxRutaRaiz";
            this.textBoxRutaRaiz.Size = new System.Drawing.Size(376, 20);
            this.textBoxRutaRaiz.TabIndex = 4;
            // 
            // labelRutaRaiz
            // 
            this.labelRutaRaiz.AutoSize = true;
            this.labelRutaRaiz.Location = new System.Drawing.Point(21, 43);
            this.labelRutaRaiz.Name = "labelRutaRaiz";
            this.labelRutaRaiz.Size = new System.Drawing.Size(106, 13);
            this.labelRutaRaiz.TabIndex = 3;
            this.labelRutaRaiz.Text = "Ruta raíz del proyect";
            // 
            // textBoxNombreProyecto
            // 
            this.textBoxNombreProyecto.Location = new System.Drawing.Point(24, 107);
            this.textBoxNombreProyecto.Name = "textBoxNombreProyecto";
            this.textBoxNombreProyecto.Size = new System.Drawing.Size(376, 20);
            this.textBoxNombreProyecto.TabIndex = 1;
            this.textBoxNombreProyecto.TextChanged += new System.EventHandler(this.textBoxNombreProyecto_TextChanged);
            // 
            // labelNombreProyecto
            // 
            this.labelNombreProyecto.AutoSize = true;
            this.labelNombreProyecto.Location = new System.Drawing.Point(21, 91);
            this.labelNombreProyecto.Name = "labelNombreProyecto";
            this.labelNombreProyecto.Size = new System.Drawing.Size(105, 13);
            this.labelNombreProyecto.TabIndex = 0;
            this.labelNombreProyecto.Text = "Nombre del proyecto";
            // 
            // treeViewEstructura
            // 
            this.treeViewEstructura.LineColor = System.Drawing.Color.Empty;
            this.treeViewEstructura.Location = new System.Drawing.Point(0, 0);
            this.treeViewEstructura.Name = "treeViewEstructura";
            this.treeViewEstructura.Size = new System.Drawing.Size(121, 97);
            this.treeViewEstructura.TabIndex = 0;
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configuraciónToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(784, 24);
            this.menuStrip.TabIndex = 8;
            this.menuStrip.Text = "menuStrip1";
            // 
            // configuraciónToolStripMenuItem
            // 
            this.configuraciónToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importarToolStripMenuItem,
            this.exportarToolStripMenuItem,
            this.importarDiagramaToolStripMenuItem});
            this.configuraciónToolStripMenuItem.Name = "configuraciónToolStripMenuItem";
            this.configuraciónToolStripMenuItem.Size = new System.Drawing.Size(95, 20);
            this.configuraciónToolStripMenuItem.Text = "Configuración";
            // 
            // importarToolStripMenuItem
            // 
            this.importarToolStripMenuItem.Name = "importarToolStripMenuItem";
            this.importarToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.importarToolStripMenuItem.Text = "Importar";
            this.importarToolStripMenuItem.Click += new System.EventHandler(this.importarToolStripMenuItem_Click);
            // 
            // exportarToolStripMenuItem
            // 
            this.exportarToolStripMenuItem.Name = "exportarToolStripMenuItem";
            this.exportarToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.exportarToolStripMenuItem.Text = "Exportar";
            this.exportarToolStripMenuItem.Click += new System.EventHandler(this.exportarToolStripMenuItem_Click);
            // 
            // importarDiagramaToolStripMenuItem
            // 
            this.importarDiagramaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importarXMIToolStripMenuItem});
            this.importarDiagramaToolStripMenuItem.Name = "importarDiagramaToolStripMenuItem";
            this.importarDiagramaToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.importarDiagramaToolStripMenuItem.Text = "Importar diagrama";
            // 
            // importarXMIToolStripMenuItem
            // 
            this.importarXMIToolStripMenuItem.Name = "importarXMIToolStripMenuItem";
            this.importarXMIToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.importarXMIToolStripMenuItem.Text = "Importar XMI";
            this.importarXMIToolStripMenuItem.Click += new System.EventHandler(this.importarXMIToolStripMenuItem_Click);
            // 
            // panelPrincipal
            // 
            this.panelPrincipal.BackColor = System.Drawing.Color.White;
            this.panelPrincipal.Controls.Add(this.panelIzquierdo);
            this.panelPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPrincipal.Location = new System.Drawing.Point(0, 24);
            this.panelPrincipal.Name = "panelPrincipal";
            this.panelPrincipal.Size = new System.Drawing.Size(784, 437);
            this.panelPrincipal.TabIndex = 9;
            // 
            // panelIzquierdo
            // 
            this.panelIzquierdo.BackColor = System.Drawing.SystemColors.Control;
            this.panelIzquierdo.Controls.Add(this.treeView);
            this.panelIzquierdo.Controls.Add(this.textBoxBuscarComponente);
            this.panelIzquierdo.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelIzquierdo.Location = new System.Drawing.Point(0, 0);
            this.panelIzquierdo.Name = "panelIzquierdo";
            this.panelIzquierdo.Padding = new System.Windows.Forms.Padding(5);
            this.panelIzquierdo.Size = new System.Drawing.Size(358, 437);
            this.panelIzquierdo.TabIndex = 0;
            this.panelIzquierdo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelIzquierdo_MouseDown);
            this.panelIzquierdo.MouseLeave += new System.EventHandler(this.PanelIzquierdo_MouseLeave);
            this.panelIzquierdo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PanelIzquierdo_MouseMove);
            this.panelIzquierdo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanelIzquierdo_MouseUp);
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = imageList;
            this.treeView.Location = new System.Drawing.Point(5, 25);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(348, 407);
            this.treeView.TabIndex = 1;
            // 
            // textBoxBuscarComponente
            // 
            this.textBoxBuscarComponente.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxBuscarComponente.Location = new System.Drawing.Point(5, 5);
            this.textBoxBuscarComponente.Name = "textBoxBuscarComponente";
            this.textBoxBuscarComponente.Size = new System.Drawing.Size(348, 20);
            this.textBoxBuscarComponente.TabIndex = 0;
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.panelPrincipal);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.panelAbrirXMI);
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "FormPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generador MDA";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panelAbrirXMI.ResumeLayout(false);
            this.panelInferior.ResumeLayout(false);
            this.panelCentral.ResumeLayout(false);
            this.panelCentral.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.panelPrincipal.ResumeLayout(false);
            this.panelIzquierdo.ResumeLayout(false);
            this.panelIzquierdo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelAbrirXMI;
        private System.Windows.Forms.Panel panelInferior;
        private System.Windows.Forms.Panel panelCentral;
        private System.Windows.Forms.TextBox textBoxNombreProyecto;
        private System.Windows.Forms.Label labelNombreProyecto;
        private System.Windows.Forms.TextBox textBoxRutaFicheroDeEntrada;
        private System.Windows.Forms.Label RutaFicheroEntrada;
        private System.Windows.Forms.TextBox textBoxRutaRaiz;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem configuraciónToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportarToolStripMenuItem;
        private System.Windows.Forms.Button buttonDirectorioRaiz;
        private System.Windows.Forms.Button buttonBuscarFicheroDeEntrada;
        private System.Windows.Forms.Label labelRutaRaiz;
        private System.Windows.Forms.Button buttonContinuar;
        private System.Windows.Forms.Panel panelPrincipal;
        private System.Windows.Forms.TreeView treeViewEstructura;
        private System.Windows.Forms.ToolStripMenuItem importarDiagramaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importarXMIToolStripMenuItem;
        private System.Windows.Forms.Panel panelIzquierdo;
        private System.Windows.Forms.TextBox textBoxBuscarComponente;
        private System.Windows.Forms.TreeView treeView;
    }
}

