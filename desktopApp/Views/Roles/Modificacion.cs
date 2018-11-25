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
using System.Data.Entity;

namespace PalcoNet.Roles
{
    public partial class Modificacion : Form
    {
        int id;
        Rol rol = new Rol();
        List<String> funcionalidades = new List<String>();

        public Modificacion(int id)
        {
            this.id = id;
            InitializeComponent();

            cargarCheckboxesFuncionalidads();

            cargarDatos();
        }

        private void cargarCheckboxesFuncionalidads()
        {
            foreach (object checkboxF in chkLstBxFuncionalidades.Items)
            {
                funcionalidades.Add(checkboxF.ToString());
            }
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
                    checkearFuncionalidadesAsignadas();
                    
                }
                catch (Exception)
                {
                    WindowsFormUtils.mensajeDeError("Error al intentar cargar el rol");
                }
            }
        }

        private void checkearFuncionalidadesAsignadas()
        {
            using (RagnarEntities db = new RagnarEntities())
            {
                List<Funcionalidad> funcionalidadesAsignadas = db.Rol.Where(r => r.id_rol == id).Select(r => r.Funcionalidad).FirstOrDefault().ToList();
                funcionalidadesAsignadas.ForEach(f => { tildarCheck(f); });
            }
        }

        private void tildarCheck(Funcionalidad f)
        {
            chkLstBxFuncionalidades.SetItemChecked(funcionalidades.IndexOf(f.descripcion), true);

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

                    //volvemos a hacer el find porque para que se actualice bien el rol, necestiamos este contexto
                    rol = db.Rol.Find(id);
                    rol.nombre = txtNombre.Text;
                    rol.habilitado = chkBxHabilitado.Checked;

                    funcionalidades.ForEach(f => {
                        if (funcionalidadesSeleccionadas().Contains(f))
                        {
                            rol.Funcionalidad.Add(BaseDeDatos.BaseDeDatos.obtenerFuncionalidadPorDescripcion(db, f));
                        }
                        else
                        {
                            rol.Funcionalidad.Remove(BaseDeDatos.BaseDeDatos.obtenerFuncionalidadPorDescripcion(db, f));
                        }
                    });

                    WindowsFormUtils.guardarYCerrar(db, this);
                }
            }
        }

        private List<String> funcionalidadesSeleccionadas()
        {
            List<String> funcs = new List<String>();
            foreach (object item in chkLstBxFuncionalidades.CheckedItems)
            {
                funcs.Add(item.ToString());
            }
            return funcs;
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
            if (otroRol != null && otroRol.id_rol != rol.id_rol)
            {
                throw new ValidationException("Ya existe otro rol con este nombre");
            }
        }
        #endregion
    }
}
