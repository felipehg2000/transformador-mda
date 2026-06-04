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
            this.buttonGenerarSalida = new System.Windows.Forms.Button();
            this.panelCentral = new System.Windows.Forms.Panel();
            this.textBoxRutaXMI = new System.Windows.Forms.TextBox();
            this.RutaXMI = new System.Windows.Forms.Label();
            this.textBoxNamespaceProyecto = new System.Windows.Forms.TextBox();
            this.textBoxRutaRaiz = new System.Windows.Forms.TextBox();
            this.labelRutaRaiz = new System.Windows.Forms.Label();
            this.labelNamespace = new System.Windows.Forms.Label();
            this.textBoxNombreProyecto = new System.Windows.Forms.TextBox();
            this.labelNombreProyecto = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.configuraciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonBuscarXMI = new System.Windows.Forms.Button();
            this.buttonDirectorioRaiz = new System.Windows.Forms.Button();
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
            this.panelPrincipal.Size = new System.Drawing.Size(439, 294);
            this.panelPrincipal.TabIndex = 0;
            // 
            // panelInferior
            // 
            this.panelInferior.BackColor = System.Drawing.SystemColors.Control;
            this.panelInferior.Controls.Add(this.buttonGenerarSalida);
            this.panelInferior.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelInferior.Location = new System.Drawing.Point(0, 255);
            this.panelInferior.Name = "panelInferior";
            this.panelInferior.Size = new System.Drawing.Size(439, 39);
            this.panelInferior.TabIndex = 2;
            // 
            // buttonGenerarSalida
            // 
            this.buttonGenerarSalida.Location = new System.Drawing.Point(311, 4);
            this.buttonGenerarSalida.Name = "buttonGenerarSalida";
            this.buttonGenerarSalida.Size = new System.Drawing.Size(115, 23);
            this.buttonGenerarSalida.TabIndex = 0;
            this.buttonGenerarSalida.Text = "Generar Salida ✔";
            this.buttonGenerarSalida.UseVisualStyleBackColor = true;
            this.buttonGenerarSalida.Click += new System.EventHandler(this.buttonGenerarSalida_Click);
            // 
            // panelCentral
            // 
            this.panelCentral.BackColor = System.Drawing.SystemColors.Control;
            this.panelCentral.Controls.Add(this.buttonDirectorioRaiz);
            this.panelCentral.Controls.Add(this.buttonBuscarXMI);
            this.panelCentral.Controls.Add(this.textBoxRutaXMI);
            this.panelCentral.Controls.Add(this.RutaXMI);
            this.panelCentral.Controls.Add(this.textBoxNamespaceProyecto);
            this.panelCentral.Controls.Add(this.textBoxRutaRaiz);
            this.panelCentral.Controls.Add(this.labelRutaRaiz);
            this.panelCentral.Controls.Add(this.labelNamespace);
            this.panelCentral.Controls.Add(this.textBoxNombreProyecto);
            this.panelCentral.Controls.Add(this.labelNombreProyecto);
            this.panelCentral.Controls.Add(this.menuStrip1);
            this.panelCentral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCentral.Location = new System.Drawing.Point(0, 0);
            this.panelCentral.Name = "panelCentral";
            this.panelCentral.Size = new System.Drawing.Size(439, 294);
            this.panelCentral.TabIndex = 1;
            // 
            // textBoxRutaXMI
            // 
            this.textBoxRutaXMI.BackColor = System.Drawing.SystemColors.Info;
            this.textBoxRutaXMI.Enabled = false;
            this.textBoxRutaXMI.Location = new System.Drawing.Point(24, 209);
            this.textBoxRutaXMI.Name = "textBoxRutaXMI";
            this.textBoxRutaXMI.Size = new System.Drawing.Size(376, 20);
            this.textBoxRutaXMI.TabIndex = 7;
            // 
            // RutaXMI
            // 
            this.RutaXMI.AutoSize = true;
            this.RutaXMI.Location = new System.Drawing.Point(21, 193);
            this.RutaXMI.Name = "RutaXMI";
            this.RutaXMI.Size = new System.Drawing.Size(69, 13);
            this.RutaXMI.TabIndex = 6;
            this.RutaXMI.Text = "Ruta del XMI";
            // 
            // textBoxNamespaceProyecto
            // 
            this.textBoxNamespaceProyecto.BackColor = System.Drawing.SystemColors.Info;
            this.textBoxNamespaceProyecto.Enabled = false;
            this.textBoxNamespaceProyecto.Location = new System.Drawing.Point(24, 157);
            this.textBoxNamespaceProyecto.Name = "textBoxNamespaceProyecto";
            this.textBoxNamespaceProyecto.Size = new System.Drawing.Size(376, 20);
            this.textBoxNamespaceProyecto.TabIndex = 5;
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
            // labelNamespace
            // 
            this.labelNamespace.AutoSize = true;
            this.labelNamespace.Location = new System.Drawing.Point(21, 141);
            this.labelNamespace.Name = "labelNamespace";
            this.labelNamespace.Size = new System.Drawing.Size(167, 13);
            this.labelNamespace.TabIndex = 2;
            this.labelNamespace.Text = "Namespace principal del proyecto";
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
            this.importarToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.importarToolStripMenuItem.Text = "Importar";
            this.importarToolStripMenuItem.Click += new System.EventHandler(this.importarToolStripMenuItem_Click);
            // 
            // exportarToolStripMenuItem
            // 
            this.exportarToolStripMenuItem.Name = "exportarToolStripMenuItem";
            this.exportarToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exportarToolStripMenuItem.Text = "Exportar";
            this.exportarToolStripMenuItem.Click += new System.EventHandler(this.exportarToolStripMenuItem_Click);
            // 
            // buttonBuscarXMI
            // 
            this.buttonBuscarXMI.Location = new System.Drawing.Point(406, 209);
            this.buttonBuscarXMI.Name = "buttonBuscarXMI";
            this.buttonBuscarXMI.Size = new System.Drawing.Size(20, 20);
            this.buttonBuscarXMI.TabIndex = 9;
            this.buttonBuscarXMI.Text = "🔎";
            this.buttonBuscarXMI.UseVisualStyleBackColor = true;
            this.buttonBuscarXMI.Click += new System.EventHandler(this.buttonBuscarXMI_Click);
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
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 294);
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
        private System.Windows.Forms.Label labelRutaRaiz;
        private System.Windows.Forms.Label labelNamespace;
        private System.Windows.Forms.TextBox textBoxNombreProyecto;
        private System.Windows.Forms.Label labelNombreProyecto;
        private System.Windows.Forms.TextBox textBoxRutaXMI;
        private System.Windows.Forms.Label RutaXMI;
        private System.Windows.Forms.TextBox textBoxNamespaceProyecto;
        private System.Windows.Forms.TextBox textBoxRutaRaiz;
        private System.Windows.Forms.Button buttonGenerarSalida;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem configuraciónToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportarToolStripMenuItem;
        private System.Windows.Forms.Button buttonDirectorioRaiz;
        private System.Windows.Forms.Button buttonBuscarXMI;
    }
}

