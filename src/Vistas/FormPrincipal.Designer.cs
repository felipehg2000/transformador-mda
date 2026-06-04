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
            this.labelNombreProyecto = new System.Windows.Forms.Label();
            this.textBoxNombreProyecto = new System.Windows.Forms.TextBox();
            this.labelNamespace = new System.Windows.Forms.Label();
            this.labelRutaRaiz = new System.Windows.Forms.Label();
            this.textBoxRutaRaiz = new System.Windows.Forms.TextBox();
            this.textBoxNamespaceProyecto = new System.Windows.Forms.TextBox();
            this.RutaXMI = new System.Windows.Forms.Label();
            this.textBoxRutaXMI = new System.Windows.Forms.TextBox();
            this.buttonGenerarSalida = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.configuraciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.panelPrincipal.Size = new System.Drawing.Size(468, 294);
            this.panelPrincipal.TabIndex = 0;
            // 
            // panelInferior
            // 
            this.panelInferior.BackColor = System.Drawing.SystemColors.Control;
            this.panelInferior.Controls.Add(this.buttonGenerarSalida);
            this.panelInferior.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelInferior.Location = new System.Drawing.Point(0, 255);
            this.panelInferior.Name = "panelInferior";
            this.panelInferior.Size = new System.Drawing.Size(468, 39);
            this.panelInferior.TabIndex = 2;
            // 
            // panelCentral
            // 
            this.panelCentral.BackColor = System.Drawing.SystemColors.Control;
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
            this.panelCentral.Size = new System.Drawing.Size(468, 294);
            this.panelCentral.TabIndex = 1;
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
            // textBoxNombreProyecto
            // 
            this.textBoxNombreProyecto.Location = new System.Drawing.Point(24, 107);
            this.textBoxNombreProyecto.Name = "textBoxNombreProyecto";
            this.textBoxNombreProyecto.Size = new System.Drawing.Size(376, 20);
            this.textBoxNombreProyecto.TabIndex = 1;
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
            // labelRutaRaiz
            // 
            this.labelRutaRaiz.AutoSize = true;
            this.labelRutaRaiz.Location = new System.Drawing.Point(21, 43);
            this.labelRutaRaiz.Name = "labelRutaRaiz";
            this.labelRutaRaiz.Size = new System.Drawing.Size(106, 13);
            this.labelRutaRaiz.TabIndex = 3;
            this.labelRutaRaiz.Text = "Ruta raíz del proyect";
            // 
            // textBoxRutaRaiz
            // 
            this.textBoxRutaRaiz.Location = new System.Drawing.Point(24, 59);
            this.textBoxRutaRaiz.Name = "textBoxRutaRaiz";
            this.textBoxRutaRaiz.Size = new System.Drawing.Size(376, 20);
            this.textBoxRutaRaiz.TabIndex = 4;
            // 
            // textBoxNamespaceProyecto
            // 
            this.textBoxNamespaceProyecto.Location = new System.Drawing.Point(24, 157);
            this.textBoxNamespaceProyecto.Name = "textBoxNamespaceProyecto";
            this.textBoxNamespaceProyecto.Size = new System.Drawing.Size(376, 20);
            this.textBoxNamespaceProyecto.TabIndex = 5;
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
            // textBoxRutaXMI
            // 
            this.textBoxRutaXMI.Location = new System.Drawing.Point(24, 209);
            this.textBoxRutaXMI.Name = "textBoxRutaXMI";
            this.textBoxRutaXMI.Size = new System.Drawing.Size(376, 20);
            this.textBoxRutaXMI.TabIndex = 7;
            // 
            // buttonGenerarSalida
            // 
            this.buttonGenerarSalida.Location = new System.Drawing.Point(329, 4);
            this.buttonGenerarSalida.Name = "buttonGenerarSalida";
            this.buttonGenerarSalida.Size = new System.Drawing.Size(115, 23);
            this.buttonGenerarSalida.TabIndex = 0;
            this.buttonGenerarSalida.Text = "Generar Salida ✔";
            this.buttonGenerarSalida.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configuraciónToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(468, 24);
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
            // 
            // exportarToolStripMenuItem
            // 
            this.exportarToolStripMenuItem.Name = "exportarToolStripMenuItem";
            this.exportarToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exportarToolStripMenuItem.Text = "Exportar";
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 294);
            this.Controls.Add(this.panelPrincipal);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormPrincipal";
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
    }
}

