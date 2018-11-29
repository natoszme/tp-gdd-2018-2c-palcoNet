namespace PalcoNet.Clientes
{
    partial class CanjearPuntos
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
            this.dgvPremios = new System.Windows.Forms.DataGridView();
            this.premio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.puntosNecesarios = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.acciones = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnVolver = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPremios)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Puntos disponibles: ";
            // 
            // dgvPremios
            // 
            this.dgvPremios.AllowUserToAddRows = false;
            this.dgvPremios.AllowUserToDeleteRows = false;
            this.dgvPremios.AllowUserToOrderColumns = true;
            this.dgvPremios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPremios.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.premio,
            this.puntosNecesarios,
            this.acciones});
            this.dgvPremios.Location = new System.Drawing.Point(21, 85);
            this.dgvPremios.Name = "dgvPremios";
            this.dgvPremios.ReadOnly = true;
            this.dgvPremios.Size = new System.Drawing.Size(364, 179);
            this.dgvPremios.TabIndex = 3;
            // 
            // premio
            // 
            this.premio.HeaderText = "Premio";
            this.premio.Name = "premio";
            this.premio.ReadOnly = true;
            // 
            // puntosNecesarios
            // 
            this.puntosNecesarios.HeaderText = "Puntos necesarios";
            this.puntosNecesarios.Name = "puntosNecesarios";
            this.puntosNecesarios.ReadOnly = true;
            // 
            // acciones
            // 
            this.acciones.HeaderText = "Acciones";
            this.acciones.Name = "acciones";
            this.acciones.ReadOnly = true;
            // 
            // btnVolver
            // 
            this.btnVolver.Location = new System.Drawing.Point(321, 280);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(75, 23);
            this.btnVolver.TabIndex = 4;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = true;
            this.btnVolver.Click += new System.EventHandler(this.btnVolver_Click);
            // 
            // CanjearPuntos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 315);
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.dgvPremios);
            this.Controls.Add(this.label1);
            this.Name = "CanjearPuntos";
            this.Text = "Canjer de puntos";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPremios)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvPremios;
        private System.Windows.Forms.DataGridViewTextBoxColumn premio;
        private System.Windows.Forms.DataGridViewTextBoxColumn puntosNecesarios;
        private System.Windows.Forms.DataGridViewTextBoxColumn acciones;
        private System.Windows.Forms.Button btnVolver;
    }
}