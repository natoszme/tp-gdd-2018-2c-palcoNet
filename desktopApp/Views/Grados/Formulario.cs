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

namespace PalcoNet.Grados
{
    public partial class Formulario : Form
    {
        Grado_publicacion grado = new Grado_publicacion();
        int? id;

        public Formulario(int? id = null)
        {
            InitializeComponent();
            this.id = id;

            if (editando())
            {
                cargarDatos();
            }
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
            if (id == null)
            {
                grado = new Grado_publicacion();
            }
            else
            {
                grado = db.Grado_publicacion.Find(id);
            }

            grado.descripcion = txtDescripcion.Text;
            grado.comision = Decimal.Parse(txtComision.Text);

            if (!editando())
            {
                db.Grado_publicacion.Add(grado);
            }
        }

        #region VALIDACIONES
        private bool validarDominio()
        {
            try
            {
                descripcionNoRepetida();
            }
            catch (ValidationException e)
            {
                WindowsFormUtils.mensajeDeError(e.Message);
                return false;
            }
            return true;
        }

        private bool camposValidos() {
            List<string> errores = new List<string>();      

            if(!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtDescripcion, "descripción"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.campoAlfabetico(txtDescripcion, "descripción"), ref errores);

            if(!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtComision, "comision"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.campoNumericoYPositivo(txtComision, "comision"), ref errores);

            if (errores.Count() > 0)
            {
                WindowsFormUtils.mostrarErrores(errores);
                return false;
            }

            return true;
        }

        private void descripcionNoRepetida()
        {
            Grado_publicacion otroGrado = BaseDeDatos.BaseDeDatos.gradoPorDescripcion(txtDescripcion.Text);
            if (otroGrado != null)
            {
                if ((editando() && id != otroGrado.id_grado) || !editando())
                {
                    throw new ValidationException("Ya existe otro grado de publicación con este nombre");
                }
            }
        }
        #endregion

        #region CARGADATOS
        private void cargarDatos()
        {
            using (RagnarEntities db = new RagnarEntities())
            {
                try
                {
                    grado = db.Grado_publicacion.Find(id);
                    txtDescripcion.Text = grado.descripcion;
                    txtComision.Text = grado.comision.ToString();
                }
                catch (Exception)
                {
                    WindowsFormUtils.mensajeDeError("Error al intentar cargar al cliente");
                }
            }
        }
        #endregion

        private Boolean editando()
        {
            return id != null;
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Formulario_Load(object sender, EventArgs e)
        {

        }
    }
}
