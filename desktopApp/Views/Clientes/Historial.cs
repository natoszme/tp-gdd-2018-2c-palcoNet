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
    public partial class Historial : Form
    {
        public Historial()
        {
            InitializeComponent();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Home().Show();
        }

        private void Historial_Load(object sender, EventArgs e)
        {
            using (RagnarEntities db = new RagnarEntities())
            {
                var historial = db.F_HistorialDeCliente(Global.usuarioLogueado.id_usuario).AsQueryable<F_HistorialDeCliente_Result>();
                DataGridViewUtils.actualizarDataGriedView(dgvCompras, historial, "id_compra");
            }
        }
    }
}
