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
using PalcoNet.Model;
using PalcoNet.Usuarios;

namespace PalcoNet.Views.Usuarios
{
    [TypeDescriptionProvider(typeof(DescriptionProvider<UsuarioFormulario, Form>))]
    public abstract partial class UsuarioFormulario : Form
    {
        protected int? id;

        public UsuarioFormulario(int? id = null)
        {
            InitializeComponent();
            this.ControlBox = false;

            this.id = id;
        }

        private void UsuarioFormulario_Load(object sender, EventArgs e)
        {

        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            using (RagnarEntities db = new RagnarEntities())
            {
                if (camposValidos())
                {

                    if (!validarDominio())
                    {
                        return;
                    }

                    asignarEntidades(db);

                    WindowsFormUtils.guardarYCerrar(db, this);
                }
            }
        }

        protected void btnCambiarPass_Click(object sender, EventArgs e)
        {
            int idUsuarioAEditarPass = id ?? default(int);
            new ModificarClaveAdmin(idUsuarioAEditarPass).ShowDialog();
        }

        public bool editando()
        {
            return id != null;
        }

        protected bool hayQueMostrarPanelAdmin()
        {
            return SessionUtils.esAdmin() && editando();
        }

        protected abstract void asignarEntidades(RagnarEntities db);
        protected abstract bool camposValidos();
        protected abstract bool validarDominio();
        protected abstract void mostrarPanelAdmin();
        protected abstract void cargarDatos();
    }
}
