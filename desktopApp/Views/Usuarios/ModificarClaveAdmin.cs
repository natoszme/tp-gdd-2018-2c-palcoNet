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
            bool camposValidos = true;
            try
            {
                ValidationsUtils.campoObligatorio(txtNuevaClave, "nueva contraseña");
                ValidationsUtils.campoObligatorio(txtRepetirClave, "repetir contraseña");
                ValidationsUtils.clavesCoincidentes(txtNuevaClave, txtRepetirClave);
            }
            catch (ValidationException e)
            {
                WindowsFormUtils.mensajeDeError(e.Message);
                camposValidos = false;
            }
            return camposValidos;
        }
        #endregion

        private void btnCambiar_Click(object sender, EventArgs e)
        {
            using (RagnarEntities db = new RagnarEntities()) {
                Usuario usuario = db.Usuario.Find(id);
                
                if (camposValidos()) {
                    usuario.clave = txtNuevaClave.Text;
                    db.Entry(usuario).State = System.Data.Entity.EntityState.Modified;
                    WindowsFormUtils.guardarYCerrar(db, this, new Home());
                }
            }            
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            WindowsFormUtils.volverALaHome(this);
        }
    }
}
