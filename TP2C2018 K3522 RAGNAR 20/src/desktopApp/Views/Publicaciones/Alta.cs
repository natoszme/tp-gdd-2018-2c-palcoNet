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

namespace PalcoNet.Publicaciones
{
    public partial class Alta : Form
    {
        Views.Publicaciones.GenerarUbicaciones formUbicaciones;
        List<DateTime> fechas = new List<DateTime>();
        public Alta()
        {
            InitializeComponent();
            cargarComboRubro();
            cargarComboGrado();
            if (formUbicaciones == null)
                formUbicaciones = new Views.Publicaciones.GenerarUbicaciones(lblCantUbicaciones);
        }

        private void btnUbicaciones_Click(object sender, EventArgs e)
        {
           
            formUbicaciones.Show();
        }

        private void btnGuardarBorrador_Click(object sender, EventArgs e)
        {
            if(camposYDominioValidos())
            {
                MessageBox.Show("Todo valido");
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
            if (camposYDominioFechaValidos())
            {
                DateTime nuevaFecha = DateTime.Parse(dtpFecha.Text);
                nuevaFecha = nuevaFecha.AddHours(int.Parse(txtHora.Text.Substring(0,2)));
                nuevaFecha = nuevaFecha.AddMinutes(int.Parse(txtHora.Text.Substring(3,2)));
                MessageBox.Show("La fecha ingresada = " + nuevaFecha.ToString() + " fue cargada satisfactoriamente");
                fechas.Add(nuevaFecha);
            }
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
            ValidationsUtils.hayError(fechaMayorAUltima, ref errores);
         

            if (errores.Count() > 0)
            {
                WindowsFormUtils.mostrarErrores(errores);
                return false;
            }

            return true;
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
        protected bool camposYDominioValidos()
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

        protected bool validarDominio(ref List<string> errores)
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

    }
}