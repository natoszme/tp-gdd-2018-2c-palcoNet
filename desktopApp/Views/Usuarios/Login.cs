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
            String usuario = txtUsuario.Text;
            String password = txtClave.Text;
            int idUsuario = 0;

            if (usuario == "" || password == "")
            {
                MessageBox.Show("Debe ingresar usuario y contraseña", "Datos invalidos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            idUsuario = obtenerIdDe(usuario, password);

            if (idUsuario != -1)
            {

                if (!BaseDeDatos.BaseDeDatos.tieneAlgunRol(idUsuario))
                {
                    MessageBox.Show("No tiene ningún rol habilitado. Por favor, contáctese con el administrador", "No posee rol", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Global.loguearUsuario(idUsuario);
                
                SeleccionarRol formRoles = new SeleccionarRol();
                this.Hide();
                formRoles.Show();
            }
            else
            {
                MessageBox.Show("Verifique los datos ingresados y vuelva a intentarlo", "Usuario no identificado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int obtenerIdDe(String usuario, String password)
        {
            return BaseDeDatos.BaseDeDatos.obtenerIdPorUsuarioPass(usuario, password);   
        }
    }
}
