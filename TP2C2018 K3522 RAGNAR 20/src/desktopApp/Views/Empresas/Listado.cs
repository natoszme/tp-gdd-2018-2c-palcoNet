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

namespace PalcoNet.Empresas
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
            dgvEmpresas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            WindowsFormUtils.abrirFormulario(new Formulario(), actualizarDataGriedView);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int? id = DataGridViewUtils.obtenerIdSeleccionado(dgvEmpresas);
            WindowsFormUtils.abrirFormulario(new Formulario(id), actualizarDataGriedView);
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Home().Show();
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            actualizarDataGriedView();
        }

        #region HELPER
        private void actualizarDataGriedView()
        {
            using (RagnarEntities db = new RagnarEntities())
            {

                IQueryable<Empresa> empresasFiltradas = db.Empresa.AsQueryable();

                if (!string.IsNullOrWhiteSpace(txtRazonSocial.Text))
                {
                    empresasFiltradas = empresasFiltradas.Where(c => c.razon_social.Contains(txtRazonSocial.Text));
                }

                if (!string.IsNullOrWhiteSpace(txtCuit.Text))
                {
                    empresasFiltradas = empresasFiltradas.Where(c => c.cuit.ToString() == txtCuit.Text);
                }

                if (!string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    empresasFiltradas = empresasFiltradas.Where(c => c.mail.Contains(txtEmail.Text));
                }

                var empresas = empresasFiltradas.Select(c => new
                {
                    id_usuario = c.id_usuario,
                    razonSocial = c.razon_social,
                    cuit = c.cuit,
                    mail = c.mail
                }).OrderBy(c => c.razonSocial);

                DataGridViewUtils.actualizarDataGriedView(dgvEmpresas, empresas, "id_usuario");
            }
        }
        #endregion

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtRazonSocial.Text = "";
            txtCuit.Text = "";
            txtEmail.Text = "";

            txtRazonSocial.Focus();
            actualizarDataGriedView();
        }
    }
}
