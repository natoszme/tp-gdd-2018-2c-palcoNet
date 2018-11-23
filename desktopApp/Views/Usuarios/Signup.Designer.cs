namespace PalcoNet.Usuarios
{
    partial class Signup
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRegistrarEmpresa = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRepetirClave = new System.Windows.Forms.TextBox();
            this.btnRegistrarCliente = new System.Windows.Forms.Button();
            this.txtClave = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnVolver = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRegistrarEmpresa);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtRepetirClave);
            this.panel1.Controls.Add(this.btnRegistrarCliente);
            this.panel1.Controls.Add(this.txtClave);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtUsuario);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(461, 179);
            this.panel1.TabIndex = 1;
            // 
            // btnRegistrarEmpresa
            // 
            this.btnRegistrarEmpresa.Location = new System.Drawing.Point(240, 136);
            this.btnRegistrarEmpresa.Name = "btnRegistrarEmpresa";
            this.btnRegistrarEmpresa.Size = new System.Drawing.Size(202, 23);
            this.btnRegistrarEmpresa.TabIndex = 5;
            this.btnRegistrarEmpresa.Text = "Registrarme como empresa";
            this.btnRegistrarEmpresa.UseVisualStyleBackColor = true;
            this.btnRegistrarEmpresa.Click += new System.EventHandler(this.btnRegistrarEmpresa_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(238, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Repetir contraseña";
            // 
            // txtRepetirClave
            // 
            this.txtRepetirClave.Location = new System.Drawing.Point(240, 82);
            this.txtRepetirClave.Name = "txtRepetirClave";
            this.txtRepetirClave.Size = new System.Drawing.Size(202, 20);
            this.txtRepetirClave.TabIndex = 3;
            this.txtRepetirClave.UseSystemPasswordChar = true;
            // 
            // btnRegistrarCliente
            // 
            this.btnRegistrarCliente.Location = new System.Drawing.Point(19, 136);
            this.btnRegistrarCliente.Name = "btnRegistrarCliente";
            this.btnRegistrarCliente.Size = new System.Drawing.Size(202, 23);
            this.btnRegistrarCliente.TabIndex = 4;
            this.btnRegistrarCliente.Text = "Registrarme como cliente";
            this.btnRegistrarCliente.UseVisualStyleBackColor = true;
            this.btnRegistrarCliente.Click += new System.EventHandler(this.btnRegistrarCliente_Click);
            // 
            // txtClave
            // 
            this.txtClave.Location = new System.Drawing.Point(19, 82);
            this.txtClave.Name = "txtClave";
            this.txtClave.Size = new System.Drawing.Size(202, 20);
            this.txtClave.TabIndex = 2;
            this.txtClave.UseSystemPasswordChar = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Contraseña";
            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(20, 33);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(201, 20);
            this.txtUsuario.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Usuario";
            // 
            // btnVolver
            // 
            this.btnVolver.Location = new System.Drawing.Point(399, 205);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(75, 23);
            this.btnVolver.TabIndex = 6;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = true;
            this.btnVolver.Click += new System.EventHandler(this.btnVolver_Click);
            // 
            // Signup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 240);
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.panel1);
            this.Name = "Signup";
            this.Text = "Registrar usuario";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRegistrarEmpresa;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRepetirClave;
        private System.Windows.Forms.Button btnRegistrarCliente;
        private System.Windows.Forms.TextBox txtClave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnVolver;
    }
}