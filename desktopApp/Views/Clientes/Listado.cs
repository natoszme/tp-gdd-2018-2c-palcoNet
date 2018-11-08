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

namespace PalcoNet.Clientes
{
    public partial class Listado : Form
    {
        public Listado()
        {
            InitializeComponent();
        }

        private void Listado_Load(object sender, EventArgs e)
        {
            actualizarDataGriedView();
        }

        #region HELPER
        private void actualizarDataGriedView()
        {
            using (RagnarEntities db = new RagnarEntities())
            {

                IQueryable<Cliente> clientesFiltrados = db.Cliente.AsQueryable();

                if (!string.IsNullOrWhiteSpace(txtNombre.Text))
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

                if (!string.IsNullOrWhiteSpace(txtDocumento.Text))
                {
                    clientesFiltrados = clientesFiltrados.Where(c => c.numero_documento.ToString().Contains(txtDocumento.Text));
                }

                var clientes = clientesFiltrados.Select(c => new
                {
                    id_usuario = c.id_usuario,
                    nombre = c.nombre,
                    apellido = c.apellido,
                    numero_documento = c.numero_documento,
                    mail = c.mail
                }).OrderBy(c => c.nombre).ThenBy(c => c.apellido);

                DataGridViewUtils.actualizarDataGriedView(dgvClientes, clientes, "id_usuario");
            }
        }
        #endregion

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            MainUtils.abrirFormulario(new Formulario(), actualizarDataGriedView);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int? id = DataGridViewUtils.obtenerIdSeleccionado(dgvClientes);
            MainUtils.abrirFormulario(new Formulario(id), actualizarDataGriedView);
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            actualizarDataGriedView();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtEmail.Text = "";
            txtDocumento.Text = "";

            txtNombre.Focus();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            MainUtils.volverALaHome(this);
        }
    }
}
