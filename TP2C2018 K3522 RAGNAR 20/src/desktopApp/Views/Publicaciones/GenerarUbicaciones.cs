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
using PalcoNet.Publicaciones;

namespace PalcoNet.Views.Publicaciones
{

    public partial class GenerarUbicaciones : Form {
        /* 
         * TODO: revisar todo esto que estaba comentado
         * public List<Ubicacion_publicacion> ubicaciones;
         */

        Label lblCantUbicaciones;
        Publicacion publicacionActual;
        Ubicacion_publicacion nuevaUbicacion;
       
        public GenerarUbicaciones(Label lblUbicaciones, Publicacion publicacion) {
            // TODO: revisar todo esto que estaba comentado

            /* 
             * Ubicacion nuevaUbicacion = new Ubicacion(1, 1, 10, new Tipo_ubicacion(), true);
             * ubicaciones.Add(nuevaUbicacion);
             * dgvUbicaciones.DataSource = ubicaciones;
             */

            InitializeComponent();
            cargarComboTipo();
            // ubicaciones = new List<Ubicacion_publicacion>();
            lblCantUbicaciones = lblUbicaciones;
            publicacionActual = publicacion;
        }
    
        private void cargarComboTipo() {
            using (RagnarEntities db = new RagnarEntities()) {
                cboTipo.DataSource = (from g in db.Tipo_ubicacion select g.descripcion).ToList();
            }
        }

        private List<String> obtenerTextoDeTipoUbicacion(List<Tipo_ubicacion> tipos) {
            return (List<String>) tipos.Select(tipo => tipo.descripcion).ToList();
        }

        private void btnAgregar_Click(object sender, EventArgs e) {
            string filaIngresada = txtFila.Text;
            string asientoIngresado = txtAsiento.Text;

            if (ubicacionValida() && !esUbicacionRepetida()) {
                // TODO: revisar lo comentado (si no va saquemoslo)
                    
                // using(RagnarEntities db = UbicacionesGlobal.contextoGlobal) {
                    guardarUbicaciones(UbicacionesGlobal.contextoGlobal);
                    dgvUbicaciones.DataSource = UbicacionesGlobal.ubicaciones;
                    MessageBox.Show("Ubicacion creada con exito");
                    lblCantUbicaciones.Text = "Ubicaciones cargadas = " + UbicacionesGlobal.ubicaciones.Count; //Actualiza el lbl del formulario de alta
                // }
            }
        }

        private bool esUbicacionRepetida() {
            Ubicacion_publicacion nuevaUbicacion = new Ubicacion_publicacion();
            nuevaUbicacion.precio = int.Parse(txtPrecio.Text);
            nuevaUbicacion.Tipo_ubicacion = BaseDeDatos.BaseDeDatos.tipoUbicacionPorDescripcion(WindowsFormUtils.textoSeleccionadoDe(cboTipo));
            nuevaUbicacion.sin_numerar = cbxNumerada.Checked;
            nuevaUbicacion.Publicacion = publicacionActual;
            nuevaUbicacion.fila = txtFila.Text;
            nuevaUbicacion.asiento = int.Parse(txtAsiento.Text);

            if (UbicacionesGlobal.ubicaciones.Any(ubicacion => esMismaUbicacion(ubicacion, nuevaUbicacion))) {
                WindowsFormUtils.mensajeDeError("Esa ubicacion ya esta ingresada");
                return true;
            }

            return false;
        }

        private bool ubicacionValida() {

            bool valido = true;
            List<string> errores = new List<string>();

            List<Control> inputs = new List<Control>();
            inputs.Add(txtFila);
            inputs.Add(txtAsiento);
            inputs.Add(txtPrecio);

            ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtAsiento, "asiento"), ref errores);
            ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtFila, "fila"), ref errores);
            ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtPrecio, "precio"), ref errores);

            if (errores.Count() <= 0)
                ValidationsUtils.hayError(() => ValidationsUtils.camposNumericos(inputs, "fila, asiento y precio"), ref errores);
            
            ValidationsUtils.hayError(() => ValidationsUtils.opcionObligatoria(cboTipo, "tipo de ubicacion"), ref errores);

            if (errores.Count() > 0) {
                WindowsFormUtils.mostrarErrores(errores);
                valido = false;
            }

            return valido;
        }


        private bool esMismaUbicacion(Ubicacion_publicacion existente, Ubicacion_publicacion nuevaUbicacion) {
            return (existente.fila == nuevaUbicacion.fila && existente.asiento == nuevaUbicacion.asiento);
        }

        private void GenerarUbicaciones_Load(object sender, EventArgs e) {
            dgvUbicaciones.DataSource = UbicacionesGlobal.ubicaciones;
        }

        private void btnFinalizar_Click(object sender, EventArgs e) {
            // TODO: revisar lo comentado (si no va saquemoslo)

            /* 
             * using (RagnarEntities db = UbicacionesGlobal.contextoGlobal) {
             *      WindowsFormUtils.guardarYCerrar(db, this);
             * }
             */

            this.Close();

           /*
            * guardarUbicaciones(db);
            * formListado.actualizarDataGriedView();
            * WindowsFormUtils.guardarYCerrar(db, this);
            * this.Hide();
            */
        }

        private void guardarUbicaciones(RagnarEntities db) {
            nuevaUbicacion = new Ubicacion_publicacion();
            nuevaUbicacion.precio = int.Parse(txtPrecio.Text);
            nuevaUbicacion.Tipo_ubicacion = BaseDeDatos.BaseDeDatos.tipoUbicacionPorDescripcion(db,WindowsFormUtils.textoSeleccionadoDe(cboTipo));
            nuevaUbicacion.sin_numerar = cbxNumerada.Checked;
            nuevaUbicacion.Publicacion = publicacionActual;
            nuevaUbicacion.fila = txtFila.Text;
            nuevaUbicacion.asiento = int.Parse(txtAsiento.Text);

            UbicacionesGlobal.ubicaciones.Add(nuevaUbicacion);
            
            /* TODO: revisar lo comentado
             * db.Ubicacion_publicacion.Add(nuevaUbicacion);
             * ubicaciones.Add(nuevaUbicacion);
             */
        }
    }
}
