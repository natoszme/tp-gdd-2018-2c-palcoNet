using PalcoNet.Model;
using PalcoNet.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalcoNet.Views.Publicaciones
{
    public partial class SeleccionarUbicaciones : Form
    {
        int idEspectaculo;
        public SeleccionarUbicaciones(int? id)
        {
            InitializeComponent();
            this.idEspectaculo = (int)id;
        }

        private void SeleccionarUbicaciones_Load(object sender, EventArgs e)
        {
            actualizarDataGriedView();
            dgvUbicaciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }


        #region HELPER
        private void actualizarDataGriedView()
        {
            using (RagnarEntities db = new RagnarEntities())
            {

                IQueryable<Ubicacion_publicacion> ubicacionesFiltradas = db.Ubicacion_publicacion.AsQueryable().Where(u => u.id_publicacion == idEspectaculo);

                ubicacionesFiltradas = ubicacionesFiltradas.Where(ub => ub.Compra == null);


                var ubicaciones = ubicacionesFiltradas.Select(c => new
                {
                    id_ubicacion = c.id_ubicacion,
                    precio = c.precio,
                    fila = c.fila,
                    asiento = c.asiento,
                    sin_numerar = c.sin_numerar,
                    tipo_asiento = c.Tipo_ubicacion.descripcion
                });

                DataGridViewUtils.actualizarDataGriedView(dgvUbicaciones, ubicaciones, "id_ubicacion");
            }
        }
        #endregion
    }
}
