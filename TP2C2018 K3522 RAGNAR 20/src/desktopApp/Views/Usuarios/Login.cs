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

            if (usuario == null)
            {
                using (RagnarEntities db = new RagnarEntities())
                {
                    db.SP_LoginDeUsuario(txtUsuario.Text, txtClave.Text);
                    txtClave.Text = "";
                    WindowsFormUtils.mensajeDeError("Verifique los datos ingresados y vuelva a intentarlo");
                }
                return;
            }
            else
            {
                loguearYPasarASeleccionarRol(usuario);
            }

            /*using (RagnarEntities db = new RagnarEntities())
            {
                int respuestaProcedure = db.SP_LoginDeUsuario(txtUsuario.Text, txtClave.Text);

                switch (respuestaProcedure)
                {
                    case 0:
                    case 2:
                        txtClave.Text = "";
                        WindowsFormUtils.mensajeDeError("Verifique los datos ingresados y vuelva a intentarlo");
                        return;
                    
                    case 1:
                        
                        loguearYPasarASeleccionarRol(usuario);
                        return;
                }
            }*/
        }

        private void loguearYPasarASeleccionarRol(Usuario usuario)
        {
            Global.loguearUsuario(usuario);

            SeleccionarRol formRoles = new SeleccionarRol();
            this.Hide();
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
