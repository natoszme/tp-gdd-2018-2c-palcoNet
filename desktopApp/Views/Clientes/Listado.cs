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
            using (GD2C2018Entities db = new GD2C2018Entities()) {
                var clientes = from c in db.Cliente
                               select new { c.id_usuario, c.nombre, c.apellido, c.numero_documento, c.mail };
                dgvClientes.DataSource = clientes.ToList();
                dgvClientes.Columns["id_usuario"].Visible = false;
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
    }
}
