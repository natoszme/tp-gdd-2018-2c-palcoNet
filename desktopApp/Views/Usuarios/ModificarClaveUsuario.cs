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
        private bool camposYDominioValidos()
        {
            bool valido = true;
            List<string> errores = new List<string>();

            ValidationsUtils.validarError(() => ValidationsUtils.campoObligatorio(txtClaveActual, "contraseña actual"), ref errores);
            ValidationsUtils.validarError(() => ValidationsUtils.campoObligatorio(txtNuevaClave, "nueva contraseña"), ref errores);
            ValidationsUtils.validarError(() => ValidationsUtils.campoObligatorio(txtRepetirClave, "repetir contraseña"), ref errores);
            ValidationsUtils.validarError(() => ValidationsUtils.clavesCoincidentes(txtNuevaClave, txtRepetirClave), ref errores);

            ValidationsUtils.validarError(claveActualExistente, ref errores);
            
            if (errores.Count() > 0) {
                WindowsFormUtils.mostrarErrores(errores);
                valido = false;
            }

            return valido;
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
            if (camposYDominioValidos())
            {
                BaseDeDatos.BaseDeDatos.modificarClave(Global.usuarioLogueado, txtNuevaClave.Text, this);
            }
        }
    }
}
