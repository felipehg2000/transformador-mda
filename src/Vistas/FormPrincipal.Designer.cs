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
            this.panelPrincipal = new System.Windows.Forms.Panel();
            this.panelInferior = new System.Windows.Forms.Panel();
            this.panelCentral = new System.Windows.Forms.Panel();
            this.buttonDirectorioRaiz = new System.Windows.Forms.Button();
            this.buttonBuscarFicheroDeEntrada = new System.Windows.Forms.Button();
            this.textBoxRutaFicheroDeEntrada = new System.Windows.Forms.TextBox();
            this.RutaFicheroEntrada = new System.Windows.Forms.Label();
            this.textBoxRutaRaiz = new System.Windows.Forms.TextBox();
            this.labelRutaRaiz = new System.Windows.Forms.Label();
            this.textBoxNombreProyecto = new System.Windows.Forms.TextBox();
            this.labelNombreProyecto = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.configuraciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonContinuar = new System.Windows.Forms.Button();
            this.panelPrincipal.SuspendLayout();
            this.panelInferior.SuspendLayout();
            this.panelCentral.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelPrincipal
            // 
            this.panelPrincipal.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panelPrincipal.Controls.Add(this.panelInferior);
            this.panelPrincipal.Controls.Add(this.panelCentral);
            this.panelPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPrincipal.Location = new System.Drawing.Point(0, 0);
            this.panelPrincipal.Name = "panelPrincipal";
            this.panelPrincipal.Size = new System.Drawing.Size(439, 226);
            this.panelPrincipal.TabIndex = 0;
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
            this.panelCentral.Controls.Add(this.menuStrip1);
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
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configuraciónToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(439, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // configuraciónToolStripMenuItem
            // 
            this.configuraciónToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importarToolStripMenuItem,
            this.exportarToolStripMenuItem});
            this.configuraciónToolStripMenuItem.Name = "configuraciónToolStripMenuItem";
            this.configuraciónToolStripMenuItem.Size = new System.Drawing.Size(95, 20);
            this.configuraciónToolStripMenuItem.Text = "Configuración";
            // 
            // importarToolStripMenuItem
            // 
            this.importarToolStripMenuItem.Name = "importarToolStripMenuItem";
            this.importarToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.importarToolStripMenuItem.Text = "Importar";
            this.importarToolStripMenuItem.Click += new System.EventHandler(this.importarToolStripMenuItem_Click);
            // 
            // exportarToolStripMenuItem
            // 
            this.exportarToolStripMenuItem.Name = "exportarToolStripMenuItem";
            this.exportarToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.exportarToolStripMenuItem.Text = "Exportar";
            this.exportarToolStripMenuItem.Click += new System.EventHandler(this.exportarToolStripMenuItem_Click);
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
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 226);
            this.Controls.Add(this.panelPrincipal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generador MDA";
            this.panelPrincipal.ResumeLayout(false);
            this.panelInferior.ResumeLayout(false);
            this.panelCentral.ResumeLayout(false);
            this.panelCentral.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelPrincipal;
        private System.Windows.Forms.Panel panelInferior;
        private System.Windows.Forms.Panel panelCentral;
        private System.Windows.Forms.TextBox textBoxNombreProyecto;
        private System.Windows.Forms.Label labelNombreProyecto;
        private System.Windows.Forms.TextBox textBoxRutaFicheroDeEntrada;
        private System.Windows.Forms.Label RutaFicheroEntrada;
        private System.Windows.Forms.TextBox textBoxRutaRaiz;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem configuraciónToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportarToolStripMenuItem;
        private System.Windows.Forms.Button buttonDirectorioRaiz;
        private System.Windows.Forms.Button buttonBuscarFicheroDeEntrada;
        private System.Windows.Forms.Label labelRutaRaiz;
        private System.Windows.Forms.Button buttonContinuar;
    }
}

