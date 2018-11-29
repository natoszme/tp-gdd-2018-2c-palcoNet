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

            dgvCompras.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void volverSiNoTieneCompras()
        {
            if (dgvCompras.Rows.Count == 0)
            {
                WindowsFormUtils.mensajeDeError("Aún no tiene compras en el sistema");
                this.Close();
                new Home().Show(); 
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            WindowsFormUtils.volverALaHome(this);
        }

        private void Historial_Load(object sender, EventArgs e)
        {
            using (RagnarEntities db = new RagnarEntities())
            {
                var historial = db.F_HistorialDeCliente(Global.obtenerUsuarioLogueado().id_usuario).AsQueryable<F_HistorialDeCliente_Result>();
                DataGridViewUtils.actualizarDataGriedView(dgvCompras, historial);
            }

            volverSiNoTieneCompras();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
