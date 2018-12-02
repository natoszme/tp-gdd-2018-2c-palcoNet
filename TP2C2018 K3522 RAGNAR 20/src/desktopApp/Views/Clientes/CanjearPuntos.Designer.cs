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
            this.lblPuntosDisponiblesTexto = new System.Windows.Forms.Label();
            this.dgvPremios = new System.Windows.Forms.DataGridView();
            this.premio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.puntosNecesarios = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.acciones = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnVolver = new System.Windows.Forms.Button();
            this.lblPuntosDisponibles = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPremios)).BeginInit();
            this.SuspendLayout();
            // 
            // lblPuntosDisponiblesTexto
            // 
            this.lblPuntosDisponiblesTexto.AutoSize = true;
            this.lblPuntosDisponiblesTexto.Location = new System.Drawing.Point(28, 34);
            this.lblPuntosDisponiblesTexto.Name = "lblPuntosDisponiblesTexto";
            this.lblPuntosDisponiblesTexto.Size = new System.Drawing.Size(101, 13);
            this.lblPuntosDisponiblesTexto.TabIndex = 0;
            this.lblPuntosDisponiblesTexto.Text = "Puntos disponibles: ";
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
            // lblPuntosDisponibles
            // 
            this.lblPuntosDisponibles.AutoSize = true;
            this.lblPuntosDisponibles.Location = new System.Drawing.Point(126, 34);
            this.lblPuntosDisponibles.Name = "lblPuntosDisponibles";
            this.lblPuntosDisponibles.Size = new System.Drawing.Size(13, 13);
            this.lblPuntosDisponibles.TabIndex = 5;
            this.lblPuntosDisponibles.Text = "0";
            // 
            // CanjearPuntos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 315);
            this.Controls.Add(this.lblPuntosDisponibles);
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.dgvPremios);
            this.Controls.Add(this.lblPuntosDisponiblesTexto);
            this.Name = "CanjearPuntos";
            this.Text = "Canjer de puntos";
            this.Load += new System.EventHandler(this.CanjearPuntos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPremios)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPuntosDisponiblesTexto;
        private System.Windows.Forms.DataGridView dgvPremios;
        private System.Windows.Forms.DataGridViewTextBoxColumn premio;
        private System.Windows.Forms.DataGridViewTextBoxColumn puntosNecesarios;
        private System.Windows.Forms.DataGridViewTextBoxColumn acciones;
        private System.Windows.Forms.Button btnVolver;
        private System.Windows.Forms.Label lblPuntosDisponibles;
    }
}