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
using PalcoNet.Model;

namespace PalcoNet.Roles
{
    public partial class Alta : Form
    {
        Rol rol = new Rol();

        public Alta()
        {
            InitializeComponent();
            cargarComboFuncionalidades();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
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

        private void asignarEntidades(RagnarEntities db)
        {
            rol = new Rol();

            rol.nombre = txtNombre.Text;
            rol.Funcionalidad.Add(BaseDeDatos.BaseDeDatos.obtenerFuncionalidadPorDescripcion(db, cmbBxFuncionalidad.SelectedItem.ToString()));
            rol.habilitado = true;

            db.Rol.Add(rol);
        }

        #region VALIDACIONES
        private bool validarDominio()
        {
            try
            {
                nombreRolNoRepetido();
            }
            catch (ValidationException e)
            {
                WindowsFormUtils.mensajeDeError(e.Message);
                return false;
            }
            return true;
        }

        private bool camposValidos()
        {
            bool camposValidos = true;
            try
            {
                ValidationsUtils.campoObligatorio(txtNombre, "nombre");
                ValidationsUtils.campoAlfabetico(txtNombre, "nombre");
            }
            catch (ValidationException e)
            {
                WindowsFormUtils.mensajeDeError(e.Message);
                camposValidos = false;
            }
            return camposValidos;
        }

        private void nombreRolNoRepetido()
        {
            Rol otroRol = BaseDeDatos.BaseDeDatos.rolPorNombre(txtNombre.Text);
            if (otroRol != null)
            {
                throw new ValidationException("Ya existe otro rol con este nombre");
            }
        }
        #endregion

        private void cargarComboFuncionalidades()
        {
            using(RagnarEntities db = new RagnarEntities())
            {
                cmbBxFuncionalidad.DataSource = (from f in db.Funcionalidad select f.descripcion).ToList(); 
            }
        }

        private void Alta_Load(object sender, EventArgs e)
        {

        }
    }
}
