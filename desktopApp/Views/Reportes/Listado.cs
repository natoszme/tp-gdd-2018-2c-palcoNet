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

namespace PalcoNet.Views.Reportes
{
    public partial class Listado : Form
    {
        public Listado()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtAnio.Text = "";
            txtTrimestre.Text = "";

            txtAnio.Focus();
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            using (RagnarEntities db = new RagnarEntities()) {
                if (camposValidos()) {
                    var reporte = Reportero.getReporte(db, int.Parse(txtAnio.Text), int.Parse(txtTrimestre.Text), cmbReporte.SelectedIndex);
                    DataGridViewUtils.actualizarDataGriedView(dgvReporte, reporte);
                }
            }

        }
        #region VALIDACIONES
        private bool camposValidos()
        {
            bool valido = true;
            List<string> errores = new List<string>();

            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtAnio, "año"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.campoNumericoYPositivo(txtAnio, "año"), ref errores);

            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtTrimestre, "trimestre"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.campoNumericoYPositivo(txtTrimestre, "trimestre"), ref errores);

            ValidationsUtils.hayError(() => ValidationsUtils.opcionObligatoria(cmbReporte, "tipo de reporte"), ref errores);

            if (errores.Count() > 0)
            {
                WindowsFormUtils.mostrarErrores(errores);
                valido = false;
            }

            return valido;
        }
        #endregion
    }
}
