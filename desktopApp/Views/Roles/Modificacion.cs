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

namespace PalcoNet.Roles
{
    public partial class Modificacion : Form
    {
        int id;
        Rol rol = new Rol();

        public Modificacion(int id)
        {
            this.id = id;
            InitializeComponent();

            cargarDatos();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Modificacion_Load(object sender, EventArgs e)
        {

        }

        #region CARGADATOS
        private void cargarDatos()
        {
            using (RagnarEntities db = new RagnarEntities())
            {
                try
                {
                    rol = db.Rol.Find(id);
                    txtNombre.Text = rol.nombre;
                    chkBxHabilitado.Checked = rol.habilitado;
                    //checkearFuncionalidadesAsignadas();
                    
                }
                catch (Exception)
                {
                    WindowsFormUtils.mensajeDeError("Error al intentar cargar el rol");
                }
            }
        }
        #endregion

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

                    rol = db.Rol.SingleOrDefault(r => r.id_rol == id);
                    rol.nombre = txtNombre.Text;
                    rol.habilitado = chkBxHabilitado.Checked;

                    WindowsFormUtils.guardarYCerrar(db, this);
                }
            }
        }

        /*private RagnarEntities asignarEntidades(RagnarEntities db)
        {
            rol = new Rol();

            rol.nombre = txtNombre.Text;
            //rol.Funcionalidad.Add(BaseDeDatos.BaseDeDatos.obtenerFuncionalidadPorDescripcion(db, cmbBxFuncionalidad.SelectedItem.ToString()));
            rol.habilitado = chkBxHabilitado.Checked;

            /*actualizarAsignacionFuncionalidades(db);
            db.Entry(rol).State = System.Data.Entity.EntityState.Modified;*/

            /*return db;
        }

        private void actualizarAsignacionFuncionalidades(RagnarEntities db)
        {
            foreach (var rolFuncionalidad in rol.Funcionalidad)
            {
                db.Entry(rolFuncionalidad).State = System.Data.Entity.EntityState.Modified;
            }
        }*/

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
            if (otroRol != null && otroRol.id_rol != rol.id_rol)
            {
                throw new ValidationException("Ya existe otro rol con este nombre");
            }
        }
        #endregion
    }
}
