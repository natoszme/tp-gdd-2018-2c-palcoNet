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
            this.btnVolver = new System.Windows.Forms.Button();
            this.lblPuntosDisponibles = new System.Windows.Forms.Label();
            this.btnCanjearPremio = new System.Windows.Forms.Button();
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
            this.dgvPremios.Location = new System.Drawing.Point(21, 85);
            this.dgvPremios.Name = "dgvPremios";
            this.dgvPremios.ReadOnly = true;
            this.dgvPremios.Size = new System.Drawing.Size(364, 179);
            this.dgvPremios.TabIndex = 3;
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
            // btnCanjearPremio
            // 
            this.btnCanjearPremio.Location = new System.Drawing.Point(290, 29);
            this.btnCanjearPremio.Name = "btnCanjearPremio";
            this.btnCanjearPremio.Size = new System.Drawing.Size(95, 23);
            this.btnCanjearPremio.TabIndex = 6;
            this.btnCanjearPremio.Text = "Canjear puntos";
            this.btnCanjearPremio.UseVisualStyleBackColor = true;
            this.btnCanjearPremio.Click += new System.EventHandler(this.btnCanjearPremio_Click);
            // 
            // CanjearPuntos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 315);
            this.Controls.Add(this.btnCanjearPremio);
            this.Controls.Add(this.lblPuntosDisponibles);
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.dgvPremios);
            this.Controls.Add(this.lblPuntosDisponiblesTexto);
            this.Name = "CanjearPuntos";
            this.Text = "Canjer puntos";
            this.Load += new System.EventHandler(this.CanjearPuntos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPremios)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPuntosDisponiblesTexto;
        private System.Windows.Forms.DataGridView dgvPremios;
        private System.Windows.Forms.Button btnVolver;
        private System.Windows.Forms.Label lblPuntosDisponibles;
        private System.Windows.Forms.Button btnCanjearPremio;
    }
}