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

namespace PalcoNet.Publicaciones
{
    public partial class Alta : UsuarioFormulario
    {
        Publicacion publicacion = new Publicacion();
        Views.Publicaciones.GenerarUbicaciones formUbicaciones;
        List<DateTime> fechas = new List<DateTime>();

        public Alta(int? id = null) : base(id)
        {
            InitializeComponent();
            cargarComboRubro();
            cargarComboGrado();
            if (formUbicaciones == null)
                formUbicaciones = new Views.Publicaciones.GenerarUbicaciones(lblCantUbicaciones,publicacion);

            if (editando())
            {
                cargarDatos();
                lblCantUbicaciones.Text = "Ubicaciones cargadas = " + publicacion.Ubicacion_publicacion.Count;
                btnAgregarFecha.Text = "Modificar fecha";
            }

        }

        private void btnUbicaciones_Click(object sender, EventArgs e)
        {
           
            formUbicaciones.Show();
        }

        private void btnGuardarBorrador_Click(object sender, EventArgs e)
        {
            using (RagnarEntities db = new RagnarEntities())
            {
                if (camposYDominioValidos())
                {
                    MessageBox.Show("Todo valido");


                    asignarEntidades(db);

                    WindowsFormUtils.guardarYCerrar(db, this);
                }
                else
                {
                    return;
                }
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

        private void Alta_Load(object sender, EventArgs e)
        {
            
        }

        private void btnPublicar_Click(object sender, EventArgs e)
        {

        }

        private void btnAgregarFecha_Click(object sender, EventArgs e)
        {
            if (editando())
            {
                if (camposYDominioFechaValidos())
                {
                    DateTime nuevaFecha = DateTime.Parse(dtpFecha.Text);
                    nuevaFecha = nuevaFecha.AddHours(int.Parse(txtHora.Text.Substring(0, 2)));
                    nuevaFecha = nuevaFecha.AddMinutes(int.Parse(txtHora.Text.Substring(3, 2)));
                    MessageBox.Show("La fecha ingresada = " + nuevaFecha.ToString() + " fue cargada satisfactoriamente");
                    fechas.RemoveAt(0);
                    fechas.Add(nuevaFecha);
                    return;
                }
              
            }
            if (camposYDominioFechaValidos())
            {
                DateTime nuevaFecha = DateTime.Parse(dtpFecha.Text);
                nuevaFecha = nuevaFecha.AddHours(int.Parse(txtHora.Text.Substring(0,2)));
                nuevaFecha = nuevaFecha.AddMinutes(int.Parse(txtHora.Text.Substring(3,2)));
                MessageBox.Show("La fecha ingresada = " + nuevaFecha.ToString() + " fue cargada satisfactoriamente");
                fechas.Add(nuevaFecha);
            }
        }


        protected override void asignarEntidades(RagnarEntities db)
        {
            if (id == null)
            {
               // publicacion = new Publicacion();
            }
            else
            {
                publicacion = db.Publicacion.Find(id);
            }
            foreach(DateTime fecha in fechas){
                publicacion.descripcion = txtDescripcion.Text;
                publicacion.direccion = txtDireccion.Text;
                publicacion.Estado_publicacion = BaseDeDatos.BaseDeDatos.estadoDePublicacionPorNombre("Borrador");
                publicacion.fecha_espectaculo = fecha;
                publicacion.Grado_publicacion = BaseDeDatos.BaseDeDatos.gradoPorDescripcion(WindowsFormUtils.textoSeleccionadoDe(cmbGradoPublicacion));
                publicacion.Rubro = BaseDeDatos.BaseDeDatos.rubroPorDescripcion(WindowsFormUtils.textoSeleccionadoDe(cmbRubro));
                publicacion.stock = int.Parse(txtStock.Text);
                publicacion.Empresa = Global.obtenerUsuarioLogueado().Empresa;
                publicacion.fecha_publicacion = Global.fechaDeHoy();

                if (!editando())
                {
                    db.Publicacion.Add(publicacion);
                }
            }
       
        }

        override protected void mostrarPanelAdmin()
        {
          
        }
        protected override TextBox textBoxCui()
        {
            return new TextBox();
        }
        #region VALIDACIONESFECHA
        protected bool camposYDominioFechaValidos()
        {
            bool valido = true;
            List<string> errores = new List<string>();

            if (! ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(dtpFecha, "fecha publicacion"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.fechaMayorOIgualAHoy(dtpFecha,txtHora, "fecha publicacion"), ref errores);

           
            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtHora, "hora"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.formatoHorarioValido(txtHora, "hora"), ref errores);

            

            if (errores.Count() > 0)
            {
                WindowsFormUtils.mostrarErrores(errores);
                valido = false;
            }
            else
            {
                valido = validarDominioFecha(ref errores);
            }

            return valido;
        }

        protected bool validarDominioFecha(ref List<string> errores)
        {
            if (editando())
            {
                ValidationsUtils.hayError(noExistefechaDeMismaPublicacion, ref errores);
            }
            else
            {
                ValidationsUtils.hayError(fechaMayorAUltima, ref errores);
            }
            
         

            if (errores.Count() > 0)
            {
                WindowsFormUtils.mostrarErrores(errores);
                return false;
            }

            return true;
        }

        private void noExistefechaDeMismaPublicacion(){
            if(BaseDeDatos.BaseDeDatos.existePublicacionEnMismaFecha(publicacion))
                throw new ValidationException("No se puede elegir una fecha de un espectaculo que sea realizado a la misma hora en el mismo lugar");
          
        }

        private void fechaMayorAUltima()
        {
            if(fechas.Count!=0)
            {
                if(DateTime.Parse(dtpFecha.Text).AddHours(int.Parse(txtHora.Text.Substring(0, 2))).AddMinutes(int.Parse(txtHora.Text.Substring(3, 2)))<=fechas.ElementAt(fechas.Count-1))
                    throw new ValidationException("La fecha cargada debe ser superior a la ultima fecha de un espectaculo");
            }
        }

        #endregion

        #region VALIDACIONES
        protected override bool camposYDominioValidos()
        {
            bool valido = true;
            List<string> errores = new List<string>();

            ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtDireccion, "direccion"), ref errores);
            

            ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtDescripcion, "descripcion"), ref errores);

            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtStock, "stock"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.campoEnteroYPositivo(txtStock, "stock"), ref errores);

            ValidationsUtils.hayError(() => ValidationsUtils.opcionObligatoria(cmbRubro, "rubro"), ref errores);

            ValidationsUtils.hayError(() => ValidationsUtils.opcionObligatoria(cmbGradoPublicacion, "grado de publicacion"), ref errores);
       
            if (errores.Count() > 0)
            {
                WindowsFormUtils.mostrarErrores(errores);
                valido = false;
            }
            else
            {
                valido = validarDominio(ref errores);
            }

            return valido;
        }

        protected override bool validarDominio(ref List<string> errores)
        {
            if(!ValidationsUtils.hayError(hayUbicacionesCargadas, ref errores))
              ValidationsUtils.hayError(stockMenorAUbicaciones, ref errores);
            ValidationsUtils.hayError(algunaFechaIngresada, ref errores);

            if (errores.Count() > 0)
            {
                WindowsFormUtils.mostrarErrores(errores);
                return false;
            }

            return true;
        }

        private void hayUbicacionesCargadas()
        {
            
            if (formUbicaciones.ubicaciones==null || formUbicaciones.ubicaciones.Count <= 0)
            {
                throw new ValidationException("Debe haber al menos una ubicacion cargada");
            }

        }

        private void stockMenorAUbicaciones()
        {
        
            if (int.Parse(txtStock.Text)>formUbicaciones.ubicaciones.Count)
            {
                throw new ValidationException("El stock no puede ser mayor a las ubicaciones disponibles");
            }
            
        }

        private void algunaFechaIngresada()
        {
            if (fechas.Count<1)
            {
                throw new ValidationException("Debe ingresar al menos 1 fecha");
            }
            
        }
        #endregion


        #region CARGADATOS
        override protected void cargarDatos()
        {
            using (RagnarEntities db = new RagnarEntities())
            {
                try
                {
                    publicacion = db.Publicacion.Find(id);
                    txtStock.Text = publicacion.stock.ToString();
                    txtHora.Text = publicacion.fecha_espectaculo.Hour + ":" + publicacion.fecha_espectaculo.Minute;
                    txtDireccion.Text = publicacion.direccion;
                    txtDescripcion.Text = publicacion.descripcion;
                    dtpFecha.Value = (System.DateTime) publicacion.fecha_publicacion;
                    fechas.Add((System.DateTime)publicacion.fecha_publicacion);
                    cmbGradoPublicacion.Text = publicacion.Grado_publicacion.descripcion;
                    cmbRubro.Text = publicacion.Rubro.descripcion;
                    formUbicaciones.ubicaciones = publicacion.Ubicacion_publicacion.ToList();

                  
                }
                catch (Exception)
                {
                    WindowsFormUtils.mensajeDeError("Error al intentar cargar a la publicacion");
                }
            }
        }
        #endregion

    }
}