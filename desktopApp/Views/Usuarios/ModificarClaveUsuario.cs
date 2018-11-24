using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PalcoNet.Model;
using PalcoNet.Utils;

namespace PalcoNet.Usuarios
{
    public partial class ModificarClaveUsuario : Form
    {
        public ModificarClaveUsuario()
        {
            InitializeComponent();
        }

        #region VALIDACIONES
        private bool camposValidos() {
            bool camposValidos = true;
            try {
                ValidationsUtils.campoObligatorio(txtClaveActual, "contraseña actual");
                ValidationsUtils.campoObligatorio(txtNuevaClave, "nueva contraseña");
                ValidationsUtils.campoObligatorio(txtRepetirClave, "repetir contraseña");
                ValidationsUtils.clavesCoincidentes(txtNuevaClave, txtRepetirClave);
            } catch (ValidationException e) {
                WindowsFormUtils.mensajeDeError(e.Message);
                camposValidos = false;
            }
            return camposValidos;
        }

        private bool validarDominio() {
            try {
                claveActualExistente();
            } catch (ValidationException e) {
                WindowsFormUtils.mensajeDeError(e.Message);
                return false;
            }
            return true;
        }

        private void claveActualExistente() {
            Usuario usuarioDB = BaseDeDatos.BaseDeDatos.obtenerUsuarioPorCredenciales(Global.usuarioLogueado.usuario, txtClaveActual.Text);
            if (usuarioDB == null || usuarioDB.id_usuario != Global.usuarioLogueado.id_usuario) {
                throw new ValidationException("La clave actual ingresada es incorrecta");
            }
        }
        #endregion

        private void btnVolver_Click(object sender, EventArgs e)
        {
            WindowsFormUtils.volverALaHome(this);
        }

        private void btnCambiar_Click(object sender, EventArgs e)
        {
            if (camposValidos())
            {
                if (!validarDominio()) return;

                using (RagnarEntities db = new RagnarEntities())
                {
                    Usuario usuario = db.Usuario.Find(Global.usuarioLogueado.id_usuario);
                    usuario.clave = txtNuevaClave.Text;
                    db.Entry(usuario).State = System.Data.Entity.EntityState.Modified;
                    WindowsFormUtils.guardarYCerrar(db, this, new Home());
                }
            }
        }
    }
}
