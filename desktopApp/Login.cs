using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalcoNet
{
    public partial class Login : Form
    {

        int idUsuarioAutenticado;

        public Login()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            String usuarioIngresado = txtUsuario.Text;
            String passwordIngresada = txtClave.Text;

            if (validarUsuarioYSetearloSiExiste(usuarioIngresado, passwordIngresada))
            {

                SeleccionarRol formRoles = new SeleccionarRol(cantidadRolesDeUsuario(idUsuarioAutenticado),idUsuarioAutenticado);
               // this.Close(); Ver como cerrar sin que se cierre la app
                this.Hide();
                formRoles.Show();
    
            }
        }

        int cantidadRolesDeUsuario(int id)
        {
            int cantidadRoles = 1;
            //Consultar contra la base
            return cantidadRoles;
        }

        bool validarUsuarioYSetearloSiExiste(String usuario, String password)
        {
            idUsuarioAutenticado = obtenerIdUsuarioPor(usuario, password);
            return true;
        }

        int obtenerIdUsuarioPor(String usuario, String password)
        {
            int id = 1;
            //TODO consultar contra la base y traer ese ID , si no existe pone -1
            return id;
        }
    }
}
