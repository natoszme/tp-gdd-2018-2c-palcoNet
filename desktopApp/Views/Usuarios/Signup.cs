﻿using System;
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
using PalcoNet.Utils;

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
            WindowsFormUtils.volverALogin(this);
        }

        private void btnRegistrarCliente_Click(object sender, EventArgs e)
        {
            if (!validarCampos()) return;
            UsuariosUtils.guardarUsuarioYSetearLogueado(username, pass, TipoRol.CLIENTE);
            WindowsFormUtils.abrirFormulario(new Clientes.Formulario(), () => { });
        }

        private void btnRegistrarEmpresa_Click(object sender, EventArgs e)
        {
            if (!validarCampos()) return;
            UsuariosUtils.guardarUsuarioYSetearLogueado(username, pass, TipoRol.EMPRESA);
            WindowsFormUtils.abrirFormulario(new Empresas.Formulario(), () => { });
        }

        private bool validarCampos()
        {
            List<string> errores = new List<string>();

            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtUsuario, "usuario"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.campoAlfabetico(txtUsuario, "usuario"), ref errores);

            ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtClave, "contraseña"), ref errores);

            ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtRepetirClave, "comprobación de contraseña"), ref errores);

            if (errores.Count() > 0)
            {
                WindowsFormUtils.mostrarErrores(errores);
                limpiarPasses();
                return false;
            }

            if (!validarDominio(errores))
            {
                return false;
            }

            username = txtUsuario.Text;
            pass = txtClave.Text;

            return true;
        }

        private bool validarDominio(List<string> errores)
        {
            ValidationsUtils.hayError(() => validarContraseniasCoincidentes(), ref errores);
            ValidationsUtils.hayError(() => validarUsuarioInexistente(), ref errores);

            if (errores.Count() > 0)
            {
                WindowsFormUtils.mostrarErrores(errores);
                limpiarPasses();
                return false;
            }

            return true;
        }

        private void validarContraseniasCoincidentes()
        {
            if (txtClave.Text != txtRepetirClave.Text)
            {
                throw new ValidationException("Las contraseñas no coinciden");
            }
        }

        private void validarUsuarioInexistente()
        {
            if (BaseDeDatos.BaseDeDatos.existeUsuario(txtUsuario.Text))
            {
                throw new ValidationException("El usuario ya existe");
            }
        }

        private void limpiarPasses()
        {
            txtClave.Text = "";
            txtRepetirClave.Text = "";
        }
    }
}
