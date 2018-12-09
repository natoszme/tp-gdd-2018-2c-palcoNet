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
            actualizarDataGriedView();
            dgvPublicaciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }


        #region HELPER
        public void actualizarDataGriedView()
        {
            using (RagnarEntities db = new RagnarEntities())
            {

                IQueryable<Publicacion> publicacionesQuery = db.Publicacion.AsQueryable();


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

      
    }
}
