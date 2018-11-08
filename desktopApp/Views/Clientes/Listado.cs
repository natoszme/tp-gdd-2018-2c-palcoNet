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

namespace PalcoNet.Clientes
{
    public partial class Listado : Form
    {
        public Listado()
        {
            InitializeComponent();
            actualizarDataGriedView();
        }

        private void Listado_Load(object sender, EventArgs e)
        {
            actualizarDataGriedView();
        }

        #region HELPER
        private void actualizarDataGriedView()
        {
            RagnarEntities db = new RagnarEntities();
            var clientes = from c in db.Cliente
                            select new { c.id_usuario, c.nombre, c.apellido, c.numero_documento, c.mail };
            dgvClientes.DataSource = clientes.ToList();
            dgvClientes.Columns["id_usuario"].Visible = false;
        }

        private int? getIdSeleccionado() {
            try {
                return int.Parse(dgvClientes.Rows[dgvClientes.CurrentRow.Index].Cells[0].Value.ToString());
            }
            catch {
                return null;
            }
        }
        #endregion

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Formulario frm = new Formulario();
            // Abro en una ventana nueva, hasta que no cierra no puede usar la anterior
            frm.ShowDialog();

            // Actualizo en cuanto se cierra el formulario
            actualizarDataGriedView();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int? id = getIdSeleccionado();

            if (id != null) {
                Formulario frm = new Formulario(id);
                // Abro en una ventana nueva, hasta que no cierra no puede usar la anterior
                frm.ShowDialog();

                // Actualizo en cuanto se cierra el formulario
                actualizarDataGriedView();
            }            
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Home().Show();
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            using (RagnarEntities db = new RagnarEntities())
            {
                var clientesFiltrados = db.Cliente.AsQueryable();

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
                    //TODO validar parseo ok a decimal
                    clientesFiltrados = clientesFiltrados.Where(c => c.numero_documento.ToString().Contains(txtDocumento.Text));
                }

                var clientes = clientesFiltrados.Select(c => new
                {
                    id_usuario = c.id_usuario,
                    nombre = c.nombre,
                    apellido = c.apellido,
                    numero_documento = c.numero_documento,
                    mail = c.mail
                });

                dgvClientes.DataSource = clientes.ToList();
                dgvClientes.Columns["id_usuario"].Visible = false;
            }
        }
    }
}
