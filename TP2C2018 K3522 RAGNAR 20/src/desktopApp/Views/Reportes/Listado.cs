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
            dgvReporte.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtAnio.Text = "";
            txtTrimestre.Text = "";
            cmbGrado.ResetText();
            cmbGrado.SelectedIndex = -1;
            cmbReporte.ResetText();

            txtAnio.Focus();

            dgvReporte.DataSource = null;
            btnMostrar.Enabled = false;
            lblTrimestreOMes.Text = "Trimestre";
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            using (RagnarEntities db = new RagnarEntities()) {
                if (camposValidos()) {
                    int id_grado = 0;
                    
                    if (cmbGrado.Enabled)
                        id_grado = WindowsFormUtils.numeroSeleccionadoDe(cmbGrado);

                    var reporte = Reportero.getReporte(db, int.Parse(txtAnio.Text), int.Parse(txtTrimestre.Text), cmbReporte.SelectedIndex, id_grado);

                    DataGridViewUtils.actualizarDataGriedView(dgvReporte, reporte);
                    
                    if (reporte.Count() == 0)
                    {
                        WindowsFormUtils.mensajeDeError("No hubo resultados para los filtros establecidos");
                    }
                }
            }

        }

        #region VALIDACIONES
        private bool camposValidos()
        {
            bool valido = true;
            List<string> errores = new List<string>();

            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtAnio, "año"), ref errores))
            {
                ValidationsUtils.hayError(() => ValidationsUtils.campoEnteroYPositivo(txtAnio, "año"), ref errores);
                ValidationsUtils.hayError(() => anioConFormatoCorrecto(txtAnio), ref errores);
            }

            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtTrimestre, textErrorLableFiltro()), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.campoEnteroYPositivo(txtTrimestre, textErrorLableFiltro()), ref errores);

            if (!ValidationsUtils.hayError(() => ValidationsUtils.opcionObligatoria(cmbReporte, "tipo de reporte"), ref errores)) {
                if (cmbReporte.SelectedIndex == 0) {
                    ValidationsUtils.hayError(() => ValidationsUtils.opcionObligatoria(cmbGrado, "grado"), ref errores);
                }
            }

            if (errores.Count() > 0)
            {
                WindowsFormUtils.mostrarErrores(errores);
                valido = false;
            } else {
                valido = validarDominio(ref errores);
            }

            return valido;
        }

        private bool validarDominio(ref List<string> errores)
        {
            int maximo = (cmbReporte.SelectedIndex == 0 ? 12 : 4);
            ValidationsUtils.hayError(() => ValidationsUtils.valorEntre(txtTrimestre, textErrorLableFiltro(), 1, maximo), ref errores);

            if (errores.Count() > 0)
            {
                WindowsFormUtils.mostrarErrores(errores);
                return false;
            }

            return true;
        }
        #endregion

        private void cmbReporte_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnMostrar.Enabled = cmbReporte.SelectedIndex >= 0;
            // Solo habilito el grado para el reporte de empresas
            cmbGrado.Enabled = (cmbReporte.SelectedIndex == 0);
            lblTrimestreOMes.Text = textLabelFiltro();
        }

        private string textLabelFiltro() {
            // Si es el reporte de empresas, se filtra por mes, no por trimestre
            return (cmbReporte.SelectedIndex == 0 ? "Mes" : "Trimestre");
        }

        private string textErrorLableFiltro() {
            return textLabelFiltro().ToLower();
        }

        private void Listado_Load(object sender, EventArgs e)
        {
            cargarGrados();
        }

        private void cargarGrados() {
            using (RagnarEntities db = new RagnarEntities()) {
                cmbGrado.DataSource = db.Grado_publicacion.ToList().Select(
                    grado => new ComboBoxItem(grado.id_grado, grado.descripcion)
                ).ToList();

                cmbGrado.ValueMember = "value";
                cmbGrado.DisplayMember = "text";
            }
        }

        private static void anioConFormatoCorrecto(TextBox txtAnio)
        {
            if (int.Parse(txtAnio.Text) < 1000)
            {
                throw new ValidationException("El año ingresado debe tener el formato AAAA");
            }
        }
    }
}
