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
       
        public GenerarUbicaciones(Label lblUbicaciones, Publicacion publicacion)
        {
           /* Ubicacion nuevaUbicacion = new Ubicacion(1, 1, 10, new Tipo_ubicacion(), true);
            ubicaciones.Add(nuevaUbicacion);
            dgvUbicaciones.DataSource = ubicaciones;*/
            InitializeComponent();
            cargarComboTipo();
            ubicaciones = new List<Ubicacion_publicacion>();
            lblCantUbicaciones = lblUbicaciones;
            publicacionActual = publicacion;
         
            
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
                if (!esUbicacionRepetida())
                {  
                    
                    using(RagnarEntities db = new RagnarEntities())
                    {
                        asignarEntidades(db);
                        dgvUbicaciones.DataSource = ubicaciones;
                        MessageBox.Show("Ubicacion creada con exito");
                        lblCantUbicaciones.Text = "Ubicaciones cargadas = " + ubicaciones.Count; //Actualiza el lbl del formulario de alta
                    }
                    
                }
            }
        }

        private bool esUbicacionRepetida()
        {
            Ubicacion_publicacion nuevaUbicacion = new Ubicacion_publicacion();
            nuevaUbicacion.precio = int.Parse(txtPrecio.Text);
            nuevaUbicacion.Tipo_ubicacion = BaseDeDatos.BaseDeDatos.tipoUbicacionPorDescripcion(WindowsFormUtils.textoSeleccionadoDe(cboTipo));
            nuevaUbicacion.sin_numerar = cbxNumerada.Checked;
            nuevaUbicacion.Publicacion = publicacionActual;
            nuevaUbicacion.fila = txtFila.Text;
            nuevaUbicacion.asiento = int.Parse(txtAsiento.Text);
            if (ubicaciones.Any(ubicacion => esMismaUbicacion(ubicacion, nuevaUbicacion)))
            {
                WindowsFormUtils.mensajeDeError("Esa ubicacion ya esta ingresada");
                 return true;
            }
            return false;
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
            return true;
            
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

            using (RagnarEntities db = new RagnarEntities())
            {
                WindowsFormUtils.guardarYCerrar(db, this);
            }

           /* asignarEntidades(db);
            formListado.actualizarDataGriedView();
            WindowsFormUtils.guardarYCerrar(db, this);
            this.Hide();*/
        }

        void asignarEntidades(RagnarEntities db)
        {
            Ubicacion_publicacion nuevaUbicacion = new Ubicacion_publicacion();
            nuevaUbicacion.precio = int.Parse(txtPrecio.Text);
            nuevaUbicacion.Tipo_ubicacion = BaseDeDatos.BaseDeDatos.tipoUbicacionPorDescripcion(db,WindowsFormUtils.textoSeleccionadoDe(cboTipo));
            nuevaUbicacion.sin_numerar = cbxNumerada.Checked;
            nuevaUbicacion.Publicacion = publicacionActual;
            nuevaUbicacion.fila = txtFila.Text;
            nuevaUbicacion.asiento = int.Parse(txtAsiento.Text);
            db.Ubicacion_publicacion.Add(nuevaUbicacion);
            ubicaciones.Add(nuevaUbicacion);
        }
    }
}
