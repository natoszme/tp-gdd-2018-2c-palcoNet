using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PalcoNet.BaseDeDatos;
using PalcoNet.Usuarios;
using PalcoNet.Model;
using PalcoNet.Utils;

namespace PalcoNet.Usuarios
{
    public partial class Login : Form 
    {
        BaseDeDatos.BaseDeDatos db = new BaseDeDatos.BaseDeDatos();
        
        public Login()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            String username = txtUsuario.Text;
            String password = txtClave.Text;

            if (username == "" || password == "") {
                MessageBox.Show("Debe ingresar usuario y contraseña", "Datos invalidos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Usuario usuario = obtenerUsuarioDe(username, password);

            if (usuario != null) {

                if (!BaseDeDatos.BaseDeDatos.tieneAlgunRol(usuario)) {
                    MessageBox.Show("No tiene ningún rol habilitado. Por favor, contáctese con el administrador", "No posee rol", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Global.loguearUsuario(usuario);
                
                SeleccionarRol formRoles = new SeleccionarRol();
                this.Hide();
                formRoles.Show();
            }
            else
            {
                txtClave.Text = "";
                MessageBox.Show("Verifique los datos ingresados y vuelva a intentarlo", "Usuario no identificado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Usuario obtenerUsuarioDe(String usuario, String password) {
            return BaseDeDatos.BaseDeDatos.obtenerUsuarioPorCredenciales(usuario, password);   
        }

        private void btnRegistrarme_Click(object sender, EventArgs e)
        {
            this.Hide();
            WindowsFormUtils.abrirFormulario(new Usuarios.Signup(), () => { });
        }
    }
}
