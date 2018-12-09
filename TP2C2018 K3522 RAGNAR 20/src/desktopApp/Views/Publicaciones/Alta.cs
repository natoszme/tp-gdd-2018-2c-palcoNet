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
using PalcoNet.Views.Usuarios;
using PalcoNet.Views.Publicaciones;

namespace PalcoNet.Publicaciones
{
    public partial class Alta : Form
    {
        Publicacion publicacion = new Publicacion();       
        List<DateTime> fechas = new List<DateTime>();

        protected int? id;

        public Alta(int? id = null) 
        {
            InitializeComponent();
            UbicacionesGlobal.ubicaciones.Clear();
            this.id = id;
            cargarComboRubro();
            cargarComboGrado();
            dtpFecha.Value = Global.fechaDeHoy();
            if (editando())
            {           
                cargarDatos();
            
                lblCantUbicaciones.Text = "Ubicaciones cargadas = " + BaseDeDatos.BaseDeDatos.cantidadUbicacionesDePublicacion((int)id);

                btnAgregarFecha.Text = "Modificar fecha";
            }
        }


        public bool editando() {
            return id != null;
        }


        private void btnUbicaciones_Click(object sender, EventArgs e)
        {
          
            new Views.Publicaciones.GenerarUbicaciones(lblCantUbicaciones, publicacion,editando()).Show();
        }

        private void btnGuardarBorrador_Click(object sender, EventArgs e) {            
            if (camposYDominioValidos()) {
                WindowsFormUtils.mensajeDeExito("Publicación guardada como borrador");
                RagnarEntities db = UbicacionesGlobal.contextoGlobal;
                guardarPublicacion(db, BaseDeDatos.BaseDeDatos.estadoDePublicacionPorNombre(db, "Borrador"));
            }
        }

        private void cargarComboRubro()
        {
            using (RagnarEntities db = new RagnarEntities())
            {
                cmbRubro.DataSource = (from f in db.Rubro select f.descripcion).ToList();
            }
        }

        private void cargarComboGrado()
        {
            using (RagnarEntities db = new RagnarEntities())
            {
                cmbGradoPublicacion.DataSource = (from g in db.Grado_publicacion select g.descripcion).ToList();
            }
        }

        

        private void btnPublicar_Click(object sender, EventArgs e) {
            if (camposYDominioValidos()) {
                WindowsFormUtils.mensajeDeExito("Publicación publicada");
                RagnarEntities db = UbicacionesGlobal.contextoGlobal;
                guardarPublicacion(db, BaseDeDatos.BaseDeDatos.estadoDePublicacionPorNombre(db, "Publicada"));
            }
        }

        private void btnAgregarFecha_Click(object sender, EventArgs e)
        {
            if (camposYDominioFechaValidos()) {
                DateTime nuevaFecha = dtpFecha.Value;

                if (editando())
                    fechas.RemoveAt(0);

                fechas.Add(nuevaFecha);
                WindowsFormUtils.mensajeDeExito("La fecha " + nuevaFecha.ToString() + " fue cargada satisfactoriamente");
            }
        }

        protected void guardarPublicacion(RagnarEntities db, Estado_publicacion estado) {
            bool estaEditando = false;
            if (id != null)
            {
                publicacion = db.Publicacion.Find(id); 
                estaEditando = true;
            }
                
           
            foreach(DateTime fecha in fechas) {
                
                if (id == null)
                 publicacion = new Publicacion();
                 

                publicacion.descripcion = txtDescripcion.Text;
                publicacion.direccion = txtDireccion.Text;
                publicacion.Estado_publicacion = estado;
                publicacion.fecha_espectaculo = fecha;
                publicacion.Grado_publicacion = BaseDeDatos.BaseDeDatos.gradoPorDescripcion(db, WindowsFormUtils.textoSeleccionadoDe(cmbGradoPublicacion));
                publicacion.Rubro = BaseDeDatos.BaseDeDatos.rubroPorDescripcion(db, WindowsFormUtils.textoSeleccionadoDe(cmbRubro));
                publicacion.Empresa = Global.obtenerUsuarioLogueado(db).Empresa;
                publicacion.fecha_publicacion = Global.fechaDeHoy();
                publicacion.fecha_vencimiento = fecha;
               

                if (!estaEditando)
                {
                    publicacion.stock = 0;
                    foreach (Ubicacion_publicacion ubicacion in UbicacionesGlobal.ubicaciones)
                    {
                        Ubicacion_publicacion ubicacionNueva = new Ubicacion_publicacion();
                        ubicacionNueva.asiento = ubicacion.asiento;
                        ubicacionNueva.fila = ubicacion.fila;
                        ubicacionNueva.precio = ubicacion.precio;
                        ubicacionNueva.sin_numerar = ubicacion.sin_numerar;
                        ubicacionNueva.Tipo_ubicacion = ubicacion.Tipo_ubicacion;
                        ubicacionNueva.Publicacion = publicacion;
                        ubicacionNueva.habilitado = ubicacion.habilitado;
                        db.Ubicacion_publicacion.Add(ubicacionNueva);
                        publicacion.Ubicacion_publicacion.Add(ubicacionNueva);
                    }
                    db.Publicacion.Add(publicacion);
                
                }
                    
            }
            
            WindowsFormUtils.guardarYCerrar(db, this);
        }

        #region VALIDACIONESFECHA
        protected bool camposYDominioFechaValidos()
        {
            bool valido = true;
            List<string> errores = new List<string>();

            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(dtpFecha, "fecha publicacion"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.fechaMayorOIgualAHoy(dtpFecha, "fecha publicacion"), ref errores);

            if (errores.Count() > 0) {
                WindowsFormUtils.mostrarErrores(errores);
                valido = false;
            } else {
                valido = validarDominioFecha(ref errores);
            }

            return valido;
        }

        protected bool validarDominioFecha(ref List<string> errores) {
            if (!editando())
                ValidationsUtils.hayError(fechaMayorAUltima, ref errores);

            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtDireccion, "direccion"), ref errores))
                ValidationsUtils.hayError(noExistefechaDeMismaPublicacion, ref errores);         

            if (errores.Count() > 0) {
                WindowsFormUtils.mostrarErrores(errores);
                return false;
            }

            return true;
        }

        private void noExistefechaDeMismaPublicacion() {

            Publicacion otraPublicacion = BaseDeDatos.BaseDeDatos.publicacionEnMismaFecha(txtDescripcion.Text, dtpFecha.Value);

            if (otraPublicacion != null)
            {
                if (editando())
                {
                    if (otraPublicacion.id_publicacion != id)
                    {
                        excepcionEspectaculoDuplicado();
                    }
                }
                else
                {
                    excepcionEspectaculoDuplicado();
                }
            }
                
        }

        private void excepcionEspectaculoDuplicado()
        {
            throw new ValidationException("No se puede elegir una fecha de un espectaculo que sea realizado a la misma hora en el mismo lugar");
        }

        private void fechaMayorAUltima() {
            if (fechas.Count != 0) {
                if (dtpFecha.Value <= fechas.ElementAt(fechas.Count - 1))
                    throw new ValidationException("La fecha cargada debe ser superior a la ultima fecha de un espectaculo");
            }
        }
        #endregion

        #region VALIDACIONES
        protected bool camposYDominioValidos() {
            bool valido = true;
            List<string> errores = new List<string>();

            ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtDescripcion, "descripcion"), ref errores);

            ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtDireccion, "direccion"), ref errores);

            ValidationsUtils.hayError(() => ValidationsUtils.opcionObligatoria(cmbRubro, "rubro"), ref errores);

            ValidationsUtils.hayError(() => ValidationsUtils.opcionObligatoria(cmbGradoPublicacion, "grado de publicacion"), ref errores);
       
            if (errores.Count() > 0) {
                WindowsFormUtils.mostrarErrores(errores);
                valido = false;
            } else {
                valido = validarDominio(ref errores);
            }

            return valido;
        }

        protected bool validarDominio(ref List<string> errores) {
            ValidationsUtils.hayError(hayUbicacionesCargadas, ref errores);             
            ValidationsUtils.hayError(algunaFechaIngresada, ref errores);

            if (errores.Count() > 0) {
                WindowsFormUtils.mostrarErrores(errores);
                return false;
            }

            return true;
        }

        private void hayUbicacionesCargadas() {
            if (!editando() && UbicacionesGlobal.ubicaciones.Count <= 0)
                throw new ValidationException("Debe haber al menos una ubicacion cargada");
        }

        private void algunaFechaIngresada() {
            if (fechas.Count < 1)
                throw new ValidationException("Debe ingresar al menos 1 fecha");            
        }
        #endregion

        #region CARGADATOS
        protected void cargarDatos() {            
            try {
                publicacion = UbicacionesGlobal.contextoGlobal.Publicacion.Find(id);
                txtDireccion.Text = publicacion.direccion;
                txtDescripcion.Text = publicacion.descripcion;
                dtpFecha.Value = (System.DateTime) publicacion.fecha_espectaculo;
                fechas.Add((System.DateTime) publicacion.fecha_espectaculo);
                cmbGradoPublicacion.Text = publicacion.Grado_publicacion.descripcion;
                cmbRubro.Text = publicacion.Rubro.descripcion;

                UbicacionesGlobal.ubicaciones = BaseDeDatos.BaseDeDatos.ubicacionesDePublicacion(UbicacionesGlobal.contextoGlobal, (int) id);
            } catch (Exception) {
                WindowsFormUtils.mensajeDeError("Error al intentar cargar a la publicacion");
            }
        }
        #endregion

        private void btnFinalizar_Click(object sender, EventArgs e)
        {
            if (camposYDominioValidos())
            {
                WindowsFormUtils.mensajeDeExito("Publicación finalizada");
                RagnarEntities db = UbicacionesGlobal.contextoGlobal;
                guardarPublicacion(db, BaseDeDatos.BaseDeDatos.estadoDePublicacionPorNombre(db, "Finalizada"));
            }
        }
    }
}