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
    public partial class ComprarEntrada : Form
    {
        public ComprarEntrada()
        {
            InitializeComponent();
        }

        private void ComprarEntrada_Load(object sender, EventArgs e)
        {
            actualizarDataGriedView();
            dgvEspectaculos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }


        #region HELPER
        private void actualizarDataGriedView()
        {
            using (RagnarEntities db = new RagnarEntities())
            {

                IQueryable<Publicacion> espectaculosTotales = db.Publicacion.AsQueryable();

                /*if (!string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    clientesFiltrados = clientesFiltrados.Where(c => c.nombre.Contains(txtNombre.Text));
                }

                if (!string.IsNullOrWhiteSpace(txtApellido.Text))
                {
                    clientesFiltrados = clientesFiltrados.Where(c => c.apellido.Contains(txtApellido.Text));
                }

                if (!string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    clientesFiltrados = clientesFiltrados.Where(c => c.mail.Contains(txtEmail.Text));
                }

                if (!string.IsNullOrWhiteSpace(txtTipoDocumento.Text))
                {
                    clientesFiltrados = clientesFiltrados.Where(c => c.tipo_documento.ToString() == txtTipoDocumento.Text);
                }

                if (!string.IsNullOrWhiteSpace(txtDocumento.Text))
                {
                    clientesFiltrados = clientesFiltrados.Where(c => c.numero_documento.ToString() == txtDocumento.Text);
                }*/

                var espectaculos = espectaculosTotales.Select(c => new
                {
                    id_publicacion = c.id_publicacion,
                    descripcion = c.descripcion,
                    direccion = c.direccion,
                    fecha_espectaculo = c.fecha_espectaculo,
                    stock = c.stock,
                    rubro = c.Rubro
                })/*.OrderBy(c => c.nombre).ThenBy(c => c.apellido)*/;

                DataGridViewUtils.actualizarDataGriedView(dgvEspectaculos, espectaculos, "id_publicacion");
            }
        }
        #endregion
    }
}
