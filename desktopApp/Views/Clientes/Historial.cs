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
                IQueryable<Compra> comprasDeCliente = db.Cliente.Find(Global.usuarioLogueado.id_usuario).Compra.AsQueryable();

                var compras = comprasDeCliente.Select(c => new
                {
                    //TODO agregar el total de la compra
                    razon_social = c.Empresa.razon_social,
                    tarjeta_utilizada = c.tarjeta_utilizada,
                    fecha = c.fecha
                }).OrderBy(c => c.fecha);

                DataGridViewUtils.actualizarDataGriedView(dgvCompras, compras, "id_compra");
            }
        }
    }
}
