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
            using (RagnarEntities db = new RagnarEntities()) {
                
                if (camposValidos()) {
                    BaseDeDatos.BaseDeDatos.modificarClave(db.Usuario.Find(id), txtNuevaClave.Text, this, db);
                }
            }            
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            WindowsFormUtils.volverALaHome(this);
        }
    }
}
