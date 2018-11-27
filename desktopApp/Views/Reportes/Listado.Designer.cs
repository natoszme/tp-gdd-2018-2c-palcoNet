namespace PalcoNet.Views.Reportes
{
    partial class Listado
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.lblTrimestreOMes = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAnio = new System.Windows.Forms.TextBox();
            this.txtTrimestre = new System.Windows.Forms.TextBox();
            this.cmbReporte = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.dgvReporte = new System.Windows.Forms.DataGridView();
            this.btnMostrar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbGrado = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReporte)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Año";
            // 
            // lblTrimestreOMes
            // 
            this.lblTrimestreOMes.AutoSize = true;
            this.lblTrimestreOMes.Location = new System.Drawing.Point(165, 24);
            this.lblTrimestreOMes.Name = "lblTrimestreOMes";
            this.lblTrimestreOMes.Size = new System.Drawing.Size(50, 13);
            this.lblTrimestreOMes.TabIndex = 1;
            this.lblTrimestreOMes.Text = "Trimestre";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(443, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Reporte";
            // 
            // txtAnio
            // 
            this.txtAnio.Location = new System.Drawing.Point(46, 40);
            this.txtAnio.Name = "txtAnio";
            this.txtAnio.Size = new System.Drawing.Size(100, 20);
            this.txtAnio.TabIndex = 1;
            // 
            // txtTrimestre
            // 
            this.txtTrimestre.Location = new System.Drawing.Point(171, 40);
            this.txtTrimestre.Name = "txtTrimestre";
            this.txtTrimestre.Size = new System.Drawing.Size(100, 20);
            this.txtTrimestre.TabIndex = 2;
            // 
            // cmbReporte
            // 
            this.cmbReporte.FormattingEnabled = true;
            this.cmbReporte.Items.AddRange(new object[] {
            "Empresas con mas no vendidas",
            "Clientes con mas puntos vencidos",
            "Clientes con mas compras"});
            this.cmbReporte.Location = new System.Drawing.Point(446, 40);
            this.cmbReporte.Name = "cmbReporte";
            this.cmbReporte.Size = new System.Drawing.Size(121, 21);
            this.cmbReporte.TabIndex = 4;
            this.cmbReporte.SelectedIndexChanged += new System.EventHandler(this.cmbReporte_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(678, 317);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Volver";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dgvReporte
            // 
            this.dgvReporte.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReporte.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReporte.Location = new System.Drawing.Point(0, 0);
            this.dgvReporte.Name = "dgvReporte";
            this.dgvReporte.Size = new System.Drawing.Size(707, 221);
            this.dgvReporte.TabIndex = 7;
            // 
            // btnMostrar
            // 
            this.btnMostrar.Enabled = false;
            this.btnMostrar.Location = new System.Drawing.Point(588, 40);
            this.btnMostrar.Name = "btnMostrar";
            this.btnMostrar.Size = new System.Drawing.Size(75, 23);
            this.btnMostrar.TabIndex = 5;
            this.btnMostrar.Text = "Mostrar";
            this.btnMostrar.UseVisualStyleBackColor = true;
            this.btnMostrar.Click += new System.EventHandler(this.btnMostrar_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Location = new System.Drawing.Point(678, 40);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(75, 23);
            this.btnLimpiar.TabIndex = 6;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvReporte);
            this.panel1.Location = new System.Drawing.Point(46, 79);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(707, 221);
            this.panel1.TabIndex = 10;
            // 
            // cmbGrado
            // 
            this.cmbGrado.Enabled = false;
            this.cmbGrado.FormattingEnabled = true;
            this.cmbGrado.Items.AddRange(new object[] {
            "Empresas con mas no vendidas",
            "Clientes con mas puntos vencidos",
            "Clientes con mas compras"});
            this.cmbGrado.Location = new System.Drawing.Point(293, 40);
            this.cmbGrado.Name = "cmbGrado";
            this.cmbGrado.Size = new System.Drawing.Size(121, 21);
            this.cmbGrado.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(290, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Grado";
            // 
            // Listado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 352);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbGrado);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.btnMostrar);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cmbReporte);
            this.Controls.Add(this.txtTrimestre);
            this.Controls.Add(this.txtAnio);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblTrimestreOMes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Name = "Listado";
            this.Text = "Listado";
            this.Load += new System.EventHandler(this.Listado_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReporte)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTrimestreOMes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAnio;
        private System.Windows.Forms.TextBox txtTrimestre;
        private System.Windows.Forms.ComboBox cmbReporte;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dgvReporte;
        private System.Windows.Forms.Button btnMostrar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cmbGrado;
        private System.Windows.Forms.Label label4;
    }
}