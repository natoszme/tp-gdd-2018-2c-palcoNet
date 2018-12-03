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
        Cliente cliente = Global.obtenerUsuarioLogueado().Cliente;

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
                Premio premio = db.Premio.Find(id);
                if(!leAlcanzanLosPuntos(premio))
                {
                    WindowsFormUtils.mensajeDeError("Puntos insuficientes");
                    return;
                }
            
                BaseDeDatos.BaseDeDatos.clienteCompraPremio(db, cliente, premio);
                WindowsFormUtils.mensajeDeExito("Felicidades, ha adquirido " + premio.descripcion);

                DBUtils.guardar(db);
            }
            actualizarPuntosDisponibles();
        }

        private bool leAlcanzanLosPuntos(Premio premio)
        {
            return premio.puntos_necesarios <= puntosCliente();
        }

        private int puntosCliente()
        {
            return BaseDeDatos.BaseDeDatos.obtenerPuntosNoVencidosDe(cliente);
        }

        private void actualizarPuntosDisponibles()
        {
            lblPuntosDisponibles.Text = puntosCliente().ToString();
        }
    }
}
