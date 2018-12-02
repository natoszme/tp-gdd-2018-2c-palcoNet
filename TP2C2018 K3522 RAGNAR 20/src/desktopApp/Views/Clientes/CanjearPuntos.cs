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
    public partial class CanjearPuntos : Form
    {
        public CanjearPuntos()
        {
            InitializeComponent();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Home().Show();
        }

        private void CanjearPuntos_Load(object sender, EventArgs e)
        {
            actualizarPuntosDisponibles();
            actualizarDataGriedView();

            if (esCliente())
            {
                btnCanjearPremio.Enabled = true;
            }
        }

        private void actualizarDataGriedView()
        {
            RagnarEntities db = new RagnarEntities();

            IQueryable<Premio> roles = db.Premio.AsQueryable();
            var premiosOrdenados = roles.Select(premio => new
            {
                id_premio = premio.id_premio,
                descripcion = premio.descripcion,
                puntos_necesarios = premio.puntos_necesarios
            }).OrderBy(premio => premio.descripcion);

            DataGridViewUtils.actualizarDataGriedView(dgvPremios, premiosOrdenados, "id_premio");
        }

        private void btnCanjearPremio_Click(object sender, EventArgs e)
        {
            int? id = DataGridViewUtils.obtenerIdSeleccionado(dgvPremios);

            using (RagnarEntities db = new RagnarEntities())
            {
                BaseDeDatos.BaseDeDatos.clienteCompraPremio(db, Global.obtenerUsuarioLogueado().Cliente, (int)id);
            }
            actualizarPuntosDisponibles();
        }

        private bool esCliente()
        {
            return Global.obtenerRolUsuario().nombre.Equals("Cliente");
        }

        private void actualizarPuntosDisponibles()
        {
            lblPuntosDisponibles.Text = BaseDeDatos.BaseDeDatos.obtenerPuntosNoVencidosDe(Global.obtenerUsuarioLogueado()).ToString();
        }
    }
}
