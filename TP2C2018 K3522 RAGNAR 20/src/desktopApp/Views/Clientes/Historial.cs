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
        Paginador paginador;
        private bool loadingTime = true;

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
            actualizarDataGriedView();
            volverSiNoTieneCompras();
        }

        private void actualizarDataGriedView() {
            using (RagnarEntities db = new RagnarEntities())
            {
                var historial = db.F_HistorialDeCliente(Global.obtenerUsuarioLogueado().id_usuario).AsQueryable<F_HistorialDeCliente_Result>().OrderByDescending(row => row.FechaDeCompra);

                if (loadingTime) {
                    paginador = new Paginador(10, historial.Count(), lblPaginaActual, new List<Button> { btnPrimera, btnAnterior, btnSiguiente, btnUltima });
                    loadingTime = false;
                } else {
                    paginador.TotalRecords = historial.Count();
                }

                DataGridViewUtils.actualizarDataGriedView(dgvCompras, historial.Skip(paginador.init()).Take(paginador.limit()));
            }
        }

        #region PAGINADO
        private void btnPrimera_Click(object sender, EventArgs e)
        {
            paginador.first();
            actualizarDataGriedView();
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            paginador.prev();
            actualizarDataGriedView();
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            paginador.next();
            actualizarDataGriedView();
        }

        private void btnUltima_Click(object sender, EventArgs e)
        {
            paginador.last();
            actualizarDataGriedView();
        }
        #endregion
    }
}
