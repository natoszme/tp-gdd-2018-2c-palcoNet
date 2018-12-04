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
    public partial class ModificarClaveAdmin : Form
    {
        int id;
        public ModificarClaveAdmin(int id) {
            InitializeComponent();
            this.id = id;
        }

        #region VALIDACIONES
        private bool camposValidos()
        {
            bool valido = true;
            List<string> errores = new List<string>();

            ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtNuevaClave, "nueva contraseña"), ref errores);
            ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtRepetirClave, "repetir contraseña"), ref errores);
            ValidationsUtils.hayError(() => ValidationsUtils.clavesCoincidentes(txtNuevaClave, txtRepetirClave), ref errores);

            if (errores.Count() > 0)
            {
                WindowsFormUtils.mostrarErrores(errores);
                valido = false;
            }

            return valido;
        }
        #endregion

        private void btnCambiar_Click(object sender, EventArgs e)
        {
            if (PersonalizacionPass.seEstaPersonalizandoPass)
            {
                PersonalizacionPass.personalizacionPassFinalizo = true;
            }

            using (RagnarEntities db = new RagnarEntities()) {
                if (camposValidos()) {
                    BaseDeDatos.BaseDeDatos.modificarClave(db, db.Usuario.Find(id), txtNuevaClave.Text, this);
                }
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            if (PersonalizacionPass.seEstaPersonalizandoPass)
            {
                PersonalizacionPass.personalizacionPassFinalizo = false;
                DialogResult respuestaNoDiagonalidad = MessageBox.Show("La contraseña que le fue proporcionada tiene validez para un único acceso. Si sale, deberá solicitar al administrador que le proporcione una nueva. ¿Desea salir?", "Advertencia", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (respuestaNoDiagonalidad == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }

            this.Hide();
        }
    }
}
