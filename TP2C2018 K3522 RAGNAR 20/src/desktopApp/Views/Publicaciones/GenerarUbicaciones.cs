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

    public partial class GenerarUbicaciones : Form
    {

        public List<Ubicacion_publicacion> ubicaciones;
        Label lblCantUbicaciones;
        Publicacion publicacionActual;
        RagnarEntities db;
        public GenerarUbicaciones(Label lblUbicaciones, Publicacion publicacion, RagnarEntities contexto)
        {
           /* Ubicacion nuevaUbicacion = new Ubicacion(1, 1, 10, new Tipo_ubicacion(), true);
            ubicaciones.Add(nuevaUbicacion);
            dgvUbicaciones.DataSource = ubicaciones;*/
            InitializeComponent();
            cargarComboTipo();
            ubicaciones = new List<Ubicacion_publicacion>();
            lblCantUbicaciones = lblUbicaciones;
            publicacionActual = publicacion;
            db = contexto;
            
        }

    
        private void cargarComboTipo()
        {
            using (RagnarEntities db2 = new RagnarEntities())
            {
                cboTipo.DataSource = (from g in db2.Tipo_ubicacion select g.descripcion).ToList();
            }
        }

        private List<String> obtenerTextoDeTipoUbicacion( List<Tipo_ubicacion> tipos)
        {
            List<String> aux = new List<String>();
            
            foreach (Tipo_ubicacion element in tipos)
            {
                aux.Add(element.descripcion);
            }
            return aux;
        }

        private void btnAgregar_Click(object sender, EventArgs e) {
            string filaIngresada = txtFila.Text;
            string asientoIngresado = txtAsiento.Text;
            if (ubicacionValida()) {
                MessageBox.Show("Ubicacion creada con exito");
                dgvUbicaciones.DataSource = ubicaciones;
            }
        }

        private bool ubicacionValida() {
          
            List<Control> inputs = new List<Control>();
            inputs.Add(txtFila);
            inputs.Add(txtAsiento);
            inputs.Add(txtPrecio);

            try {
                ValidationsUtils.campoObligatorio(txtAsiento, "asiento");
                ValidationsUtils.campoObligatorio(txtFila, "fila");
                ValidationsUtils.campoObligatorio(txtPrecio, "precio");
                ValidationsUtils.camposNumericos(inputs, "fila, asiento y precio");
                ValidationsUtils.opcionObligatoria(cboTipo, "tipo de ubicacion");
            } catch (ValidationException e) {
                WindowsFormUtils.mensajeDeError(e.Message);
                return false;
            }


            //Ubicacion_publicacion nuevaUbicacion = new Ubicacion_publicacion(int.Parse(txtFila.Text), int.Parse(txtAsiento.Text), int.Parse(txtPrecio.Text), new Tipo_ubicacion(), cbxNumerada.Checked);
            Ubicacion_publicacion nuevaUbicacion = new Ubicacion_publicacion();
            nuevaUbicacion.precio = int.Parse(txtPrecio.Text);
            nuevaUbicacion.Tipo_ubicacion = BaseDeDatos.BaseDeDatos.tipoUbicacionPorDescripcion(db,WindowsFormUtils.textoSeleccionadoDe(cboTipo));
            nuevaUbicacion.sin_numerar = cbxNumerada.Checked;
            nuevaUbicacion.Publicacion = publicacionActual;
            nuevaUbicacion.fila = txtFila.Text;
            nuevaUbicacion.asiento = int.Parse(txtAsiento.Text);
           

               

            if (ubicaciones.Any(ubicacion => esMismaUbicacion(ubicacion, nuevaUbicacion))) {
                WindowsFormUtils.mensajeDeError("Esa ubicacion ya esta ingresada");
            } else {
                ubicaciones.Add(nuevaUbicacion);
                lblCantUbicaciones.Text = "Ubicaciones cargadas = " + ubicaciones.Count;
                return true;
            }

       

            return false ;
        }


        private bool esMismaUbicacion(Ubicacion_publicacion existente, Ubicacion_publicacion nuevaUbicacion)
        {
            return (existente.fila == nuevaUbicacion.fila && existente.asiento == nuevaUbicacion.asiento);
        }

        private void GenerarUbicaciones_Load(object sender, EventArgs e) {
            dgvUbicaciones.DataSource = ubicaciones;
        }

        private void btnFinalizar_Click(object sender, EventArgs e)
        {

            this.Hide();
        }
    }
}
