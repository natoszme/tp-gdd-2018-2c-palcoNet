using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PalcoNet.Utils;
using PalcoNet.Views.Usuarios;

namespace PalcoNet.Usuarios
{
    public partial class Signup : Form
    {
        private String username;
        private String pass;

        public Signup()
        {
            InitializeComponent();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Home().Show();
        }

        private void btnRegistrarCliente_Click(object sender, EventArgs e)
        {
            validarCampos();
            UsuariosUtils.guardarUsuario(username, pass);
            this.Hide();
            new Clientes.Formulario().Show();
        }

        private void btnRegistrarEmpresa_Click(object sender, EventArgs e)
        {
            validarCampos();
            UsuariosUtils.guardarUsuario(username, pass);
            this.Hide();
            new Empresas.Formulario().Show();
        }

        private bool validarCampos()
        {
            try
            {
                Utils.ValidationsUtils.campoAlfabetico(txtUsuario, "usuario");
            }
            catch (ValidationException e)
            {
                WindowsFormUtils.mensajeDeError(e.Message);
                return false;
            }

            if (txtClave.Text != txtRepetirClave.Text)
            {
                MessageBox.Show("Las contraseñas no coinciden", "Error", MessageBoxButtons.OK);
                return false;
            }

            if (BaseDeDatos.BaseDeDatos.existeUsuario(username))
            {
                MessageBox.Show("El usuario ya existe", "Error", MessageBoxButtons.OK);
                return false;
            }

            username = txtUsuario.Text;
            pass = txtClave.Text;

            return true;
        }
    }
}
