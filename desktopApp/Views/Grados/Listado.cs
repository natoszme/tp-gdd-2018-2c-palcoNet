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

namespace PalcoNet.Grados
{
    public partial class Listado : Form
    {
        public Listado()
        {
            InitializeComponent();

            actualizarDataGriedView();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Home().Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WindowsFormUtils.abrirFormulario(new Formulario(), actualizarDataGriedView);
        }

        private void actualizarDataGriedView()
        {
            using (RagnarEntities db = new RagnarEntities())
            {
                IQueryable<Grado_publicacion> grados = db.Grado_publicacion.AsQueryable();

                var todosLosGrados = grados.Select(g => new
                {
                    id_grado = g.id_grado,
                    descripcion = g.descripcion,
                    comision = g.comision
                }).OrderBy(g => g.descripcion);

                DataGridViewUtils.actualizarDataGriedView(dgvGrados, todosLosGrados, "id_grado");
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int? id = DataGridViewUtils.obtenerIdSeleccionado(dgvGrados);
            WindowsFormUtils.abrirFormulario(new Formulario(id), actualizarDataGriedView);
        }
    }
}
