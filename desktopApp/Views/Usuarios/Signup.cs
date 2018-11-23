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
using PalcoNet.Model;

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
            if (!validarCampos()) return;
            UsuariosUtils.guardarUsuario(username, pass, TipoRol.CLIENTE);
            this.Hide();
            new Clientes.Formulario(null, new Home()).Show();
        }

        private void btnRegistrarEmpresa_Click(object sender, EventArgs e)
        {
            if (!validarCampos()) return;
            UsuariosUtils.guardarUsuario(username, pass, TipoRol.EMPRESA);
            this.Hide();
            new Empresas.Formulario().Show();
        }

        private bool validarCampos()
        {
            try
            {
                Utils.ValidationsUtils.campoObligatorio(txtUsuario, "usuario");
                Utils.ValidationsUtils.campoAlfabetico(txtUsuario, "usuario");
                Utils.ValidationsUtils.campoObligatorio(txtClave, "contraseña");
                Utils.ValidationsUtils.campoObligatorio(txtRepetirClave, "comprobación de contraseña");
            }
            catch (ValidationException e)
            {
                WindowsFormUtils.mensajeDeError(e.Message);
                return false;
            }

            if (txtClave.Text != txtRepetirClave.Text)
            {
                WindowsFormUtils.mensajeDeError("Las contraseñas no coinciden");
                return false;
            }

            if (BaseDeDatos.BaseDeDatos.existeUsuario(txtUsuario.Text))
            {
                WindowsFormUtils.mensajeDeError("El usuario ya existe");
                return false;
            }

            username = txtUsuario.Text;
            pass = txtClave.Text;

            return true;
        }
    }
}
