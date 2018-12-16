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


namespace PalcoNet.Publicaciones
{
    public partial class Listado : Form
    {
        
        BaseDeDatos.BaseDeDatos db = new BaseDeDatos.BaseDeDatos();
        public Listado()
        {
            InitializeComponent();
        }

        private void Listado_Load(object sender, EventArgs e)
        {
            cargarEstados();
            actualizarDataGriedView();
            dgvPublicaciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }


        #region HELPER
        public void actualizarDataGriedView()
        {
            using (RagnarEntities db = new RagnarEntities())
            {

                IQueryable<Publicacion> publicacionesQuery = db.Publicacion.AsQueryable();

                // Filtro por rango de fecha de publicación
                if (cbFiltroFecha.Checked) {
                    publicacionesQuery = publicacionesQuery.Where(
                        p => p.fecha_espectaculo >= dtpFechaDesde.Value.Date && p.fecha_espectaculo <= dtpFechaHasta.Value
                    );
                }

                int id_estado = WindowsFormUtils.numeroSeleccionadoDe(cmbEstado);

                if (id_estado > 0) {
                    publicacionesQuery = publicacionesQuery.Where(
                        p => p.id_estado == id_estado
                    );
                }

                var publicaciones = publicacionesQuery.Select(c => new
                {
                    id_publicacion = c.id_publicacion,
                    descripcion = c.descripcion,
                    direccion = c.direccion,
                    empresa = c.Empresa.cuit,
                    estado_publicacion = c.Estado_publicacion.descripcion,
                    fecha_espectaculo = c.fecha_espectaculo,
                    grado = c.Grado_publicacion.descripcion,
                    rubro = c.Rubro.descripcion,
                    stock = c.stock
                    
                }).OrderByDescending(c => c.fecha_espectaculo);

                DataGridViewUtils.actualizarDataGriedView(dgvPublicaciones, publicaciones, "id_publicacion");
            }
        }
        #endregion


        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Home().Show();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            WindowsFormUtils.abrirFormulario(new Alta(null), actualizarDataGriedView);
           
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int? id = DataGridViewUtils.obtenerIdSeleccionado(dgvPublicaciones);
            if(id!=null)
            {
                if (BaseDeDatos.BaseDeDatos.obtenerEstadoDePublicacionPorId((int)id).descripcion != "Borrador")
                {
                    WindowsFormUtils.mensajeDeError("Sólo puede editar publicaciones en estado 'Borrador'");
                    return;
                }
            }
                
            WindowsFormUtils.abrirFormulario(new Alta(id), actualizarDataGriedView);
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            actualizarDataGriedView();
        }

        private void btnLimpiar_Click(object sender, EventArgs e) {
            cbFiltroFecha.Checked = false;
            cmbEstado.ResetText();
            cmbEstado.SelectedIndex = -1;

            actualizarDataGriedView();
        }

        private void cargarEstados() {
            using (RagnarEntities db = new RagnarEntities())
            {
                cmbEstado.DataSource = db.Estado_publicacion.ToList().Select(
                    estado => new ComboBoxItem(estado.id_estado, estado.descripcion)
                ).ToList();

                cmbEstado.ValueMember = "value";
                cmbEstado.DisplayMember = "text";

                setFiltrosDefault(db);
            }
        }

        private void setFiltrosDefault(RagnarEntities db) {
            // Es feo, pero asi usa por defecto el estado borrador
            Estado_publicacion estado = db.Estado_publicacion.Find(1);
            cmbEstado.SelectedIndex = estado.id_estado;

            DateTime fechaConfig = ConfigReader.getInstance().Fecha;

            dtpFechaDesde.Value = fechaConfig;
            dtpFechaHasta.Value = fechaConfig.AddMonths(1);
        }
    }
}
