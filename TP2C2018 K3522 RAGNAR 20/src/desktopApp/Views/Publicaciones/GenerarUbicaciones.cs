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
       

        Label lblCantUbicaciones;
        Publicacion publicacionActual;
        Ubicacion_publicacion nuevaUbicacion;
        int? idUbicacion;
        bool editandoPublicacion;
        public GenerarUbicaciones(Label lblUbicaciones, Publicacion publicacion,bool editando) {
           

            InitializeComponent();
            cargarComboTipo();
          
            lblCantUbicaciones = lblUbicaciones;
            publicacionActual = publicacion;
            this.editandoPublicacion = editando;
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
            if (editandoPublicacion)
            {
                if (idUbicacion == null)
                {
                    WindowsFormUtils.mensajeDeError("Debe seleccionar una ubicacion");
                    return;
                }
                
            }
            string filaIngresada = txtFila.Text;
            string asientoIngresado = txtAsiento.Text;

            if (ubicacionValida())
            {
                if(!esUbicacionRepetida()){

                    guardarUbicaciones(UbicacionesGlobal.contextoGlobal);
                    actualizarDataGriedView();

                    if (editandoPublicacion)
                    {
                        WindowsFormUtils.mensajeDeExito("Ubicacion editada con exito");    
                    }
                    else
                    {
                        WindowsFormUtils.mensajeDeExito("Ubicacion creada con exito"); 
                    }
                
                    lblCantUbicaciones.Text = "Ubicaciones cargadas = " + UbicacionesGlobal.ubicaciones.Count; //Actualiza el lbl del formulario de alta               

                }
            }
            idUbicacion = null;
            
        }

        private bool esUbicacionRepetida() {
            Ubicacion_publicacion nuevaUbicacion = new Ubicacion_publicacion();
            nuevaUbicacion.precio = int.Parse(txtPrecio.Text);
            nuevaUbicacion.Tipo_ubicacion = BaseDeDatos.BaseDeDatos.tipoUbicacionPorDescripcion(WindowsFormUtils.textoSeleccionadoDe(cboTipo));
            nuevaUbicacion.sin_numerar = cbxNumerada.Checked;
            nuevaUbicacion.Publicacion = publicacionActual;
            nuevaUbicacion.fila = txtFila.Text;
            nuevaUbicacion.asiento = int.Parse(txtAsiento.Text);
            if (editandoPublicacion)
            {
                if (BaseDeDatos.BaseDeDatos.cantidadUbicacionesDePublicacionConFilaAsientoQueNoSean(publicacionActual.id_publicacion, txtFila.Text, int.Parse(txtAsiento.Text),(int)idUbicacion) >= 1)
                {
                    WindowsFormUtils.mensajeDeError("Esa ubicacion ya esta ingresada");
                    return true;
                }
            }
            else if (UbicacionesGlobal.ubicaciones.Any(ubicacion => esMismaUbicacion(ubicacion, nuevaUbicacion))) {
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

            if(!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtAsiento, "asiento"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.campoNumericoEntero(txtAsiento, "asiento"), ref errores);

            if(!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtFila, "fila"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.campoNumericoEntero(txtFila, "fila"), ref errores);

            if(!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtPrecio, "precio"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.campoNumericoEntero(txtPrecio, "precio"), ref errores);
            
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
            if (editandoPublicacion)
            {
                btnSeleccionarUbicacion.Visible = true;
                btnAgregar.Text = "Editar";
                btnEliminar.Visible = true;
            }
              
            actualizarDataGriedView();
          
        }


       


     

        #region HELPER
        private void actualizarDataGriedView()
        {
            if (editandoPublicacion)
            {
                dgvUbicaciones.Visible = true;
                using (RagnarEntities db = new RagnarEntities())
                {

                    IQueryable<Ubicacion_publicacion> ubicacionesBase = db.Ubicacion_publicacion.AsQueryable().Where(ub=>ub.id_publicacion == publicacionActual.id_publicacion &&ub.habilitado!=null && ub.habilitado==true);



                    var ubicaciones = ubicacionesBase.Select(c => new
                    {
                        id_ubicacion = c.id_ubicacion,
                        fila = c.fila,
                        asiento = c.asiento,
                        precio = c.precio,
                        sin_numerar = c.sin_numerar,
                        tipo_ubicacion = c.Tipo_ubicacion.descripcion
                    }).OrderBy(c => c.fila).ThenBy(c => c.asiento);

                    DataGridViewUtils.actualizarDataGriedView(dgvUbicaciones, ubicaciones, "id_ubicacion");
                }
            }
           
        }
        #endregion



     

      

        private void btnFinalizar_Click(object sender, EventArgs e) {

            this.Close();

        }

        private void guardarUbicaciones(RagnarEntities db) {
            if (editandoPublicacion)
            {
                nuevaUbicacion = db.Ubicacion_publicacion.Find(idUbicacion);
                nuevaUbicacion.precio = int.Parse(txtPrecio.Text);
                nuevaUbicacion.Tipo_ubicacion = BaseDeDatos.BaseDeDatos.tipoUbicacionPorDescripcion(db, WindowsFormUtils.textoSeleccionadoDe(cboTipo));
                nuevaUbicacion.sin_numerar = cbxNumerada.Checked;
                nuevaUbicacion.Publicacion = publicacionActual;
                nuevaUbicacion.fila = txtFila.Text;
                nuevaUbicacion.asiento = int.Parse(txtAsiento.Text);
                nuevaUbicacion.habilitado = true;
                DBUtils.guardar(db);
            }
            else
            {
                nuevaUbicacion = new Ubicacion_publicacion();
                nuevaUbicacion.precio = int.Parse(txtPrecio.Text);
                nuevaUbicacion.Tipo_ubicacion = BaseDeDatos.BaseDeDatos.tipoUbicacionPorDescripcion(db, WindowsFormUtils.textoSeleccionadoDe(cboTipo));
                nuevaUbicacion.sin_numerar = cbxNumerada.Checked;
                nuevaUbicacion.Publicacion = publicacionActual;
                nuevaUbicacion.fila = txtFila.Text;
                nuevaUbicacion.asiento = int.Parse(txtAsiento.Text);
                nuevaUbicacion.habilitado = true;
                UbicacionesGlobal.ubicaciones.Add(nuevaUbicacion);
            }
            
           

            

        }

       

        private void btnSeleccionarUbicacion_Click(object sender, EventArgs e)
        {
            idUbicacion = DataGridViewUtils.obtenerIdSeleccionado(dgvUbicaciones);
            cargarDatosUbicacion();
        }

        void cargarDatosUbicacion()
        {
            using (RagnarEntities db = new RagnarEntities())
            {
                try
                {
                    nuevaUbicacion = db.Ubicacion_publicacion.Find(idUbicacion);
                    txtAsiento.Text = nuevaUbicacion.asiento.ToString();
                    txtFila.Text = nuevaUbicacion.fila.ToString();
                    txtPrecio.Text = nuevaUbicacion.precio.ToString();
                    cbxNumerada.Checked = nuevaUbicacion.sin_numerar;
                    cboTipo.Text = nuevaUbicacion.Tipo_ubicacion.descripcion;
                }
                catch (Exception)
                {
                    WindowsFormUtils.mensajeDeError("Error al intentar cargar a la ubicacion");
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int? idSeleccionado;
            idSeleccionado = DataGridViewUtils.obtenerIdSeleccionado(dgvUbicaciones);
            if (idSeleccionado != null && idUbicacion != null)
            {
                if (new RagnarEntities().Ubicacion_publicacion.Find(idSeleccionado).habilitado != null && new RagnarEntities().Ubicacion_publicacion.Find(idUbicacion).habilitado == true)
                {
                    if (BaseDeDatos.BaseDeDatos.cantidadUbicacionesDePublicacion((int)publicacionActual.id_publicacion) > 1)
                    {
                        DialogResult eliminacion = MessageBox.Show(null, "¿Desea eliminar la ubicacion?", "Eliminar ubicacion", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (eliminacion == DialogResult.OK)
                        {
                            eliminarUbicacion((int)idSeleccionado);
                            lblCantUbicaciones.Text = "Ubicaciones cargadas = " + BaseDeDatos.BaseDeDatos.cantidadUbicacionesDePublicacion((int)publicacionActual.id_publicacion);

                        }
                    }
                    else
                    {
                        WindowsFormUtils.mensajeDeError("No se puede dejar una publicacion sin ubicaciones");
                    }
                    
                }
                else
                {
                    WindowsFormUtils.mensajeDeError("No existe la ubicacion que desea borrar");
                }
                
            }
            else
            {
                WindowsFormUtils.mensajeDeError("Debe seleccionar una ubicacion");
            }
           
        }

        private void eliminarUbicacion(int idUbicacionEliminar)
        {
            UbicacionesGlobal.ubicaciones.RemoveAll(ub => ub.id_ubicacion == idUbicacionEliminar);
            
            UbicacionesGlobal.contextoGlobal.Ubicacion_publicacion.Find(idUbicacion).habilitado = false;
            publicacionActual.stock = publicacionActual.stock - 1;
            DBUtils.guardar(UbicacionesGlobal.contextoGlobal);
            actualizarDataGriedView();

        }

    }
}
