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
        public Alta()
        {
            InitializeComponent();
            cargarComboRubro();
            cargarComboGrado();
        }

        private void btnUbicaciones_Click(object sender, EventArgs e)
        {
            if(formUbicaciones == null)
                 formUbicaciones = new Views.Publicaciones.GenerarUbicaciones();
            formUbicaciones.Show();
        }

        private void btnGuardarBorrador_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Cantidad de ubicaciones: " + formUbicaciones.ubicaciones.Count);
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
                cmbRubro.DataSource = (from g in db.Grado_publicacion select g.descripcion).ToList();
            }
        }

        private void Alta_Load(object sender, EventArgs e)
        {
            
        }

        private void btnPublicar_Click(object sender, EventArgs e)
        {

        }

        /*
        #region VALIDACIONES
        override protected bool validarDominio()
        {
            try
            {
                
            }
            catch (ValidationException e)
            {
                WindowsFormUtils.mensajeDeError(e.Message);
                return false;
            }
            return true;
        }

        override protected bool camposValidos()
        {
            bool camposValidos = true;
            try
            {
                ValidationsUtils.campoObligatorio(txtNombre, "nombre");
                ValidationsUtils.campoAlfabetico(txtNombre, "nombre");
                ValidationsUtils.campoObligatorio(txtApellido, "apellido");
                ValidationsUtils.campoAlfabetico(txtApellido, "apellido");
                ValidationsUtils.campoObligatorio(txtEmail, "mail");
                ValidationsUtils.emailValido(txtEmail, "mail");
                ValidationsUtils.campoLongitudEntre(txtTelefono, "telefono", 8, 11);
                ValidationsUtils.campoNumericoYPositivo(txtTelefono, "telefono");
                // TODO: validar que la fecha de nacimiento no puede ser posterior a la del archivo de configuracion
                ValidationsUtils.campoObligatorio(dtpFechaNacimiento, "fecha de nacimiento");
                ValidationsUtils.opcionObligatoria(cmbBxTipoDocumento, "tipo de documento");
                ValidationsUtils.campoLongitudFija(txtNroDocumento, "nro. de documento", 8);
                ValidationsUtils.campoNumericoYPositivo(txtNroDocumento, "nro. de documento");
                ValidationsUtils.campoObligatorio(txtCuil, "CUIL");
                ValidationsUtils.cuilOCuitValido(txtCuil, "CUIL");
                ValidationsUtils.campoObligatorio(txtDireccion, "dirección");
                ValidationsUtils.campoObligatorio(txtPortal, "portal");
                ValidationsUtils.campoNumericoYPositivo(txtPortal, "portal");
                ValidationsUtils.campoObligatorio(txtNroPiso, "nro. piso");
                ValidationsUtils.campoNumericoYPositivo(txtNroPiso, "nro. piso");
                ValidationsUtils.campoObligatorio(txtDepto, "departamento");
                ValidationsUtils.campoObligatorio(txtLocalidad, "localidad");
                ValidationsUtils.campoAlfabetico(txtLocalidad, "localidad");
                ValidationsUtils.campoObligatorio(txtCodigoPostal, "codigo postal");
                validarTarjeta();
            }
            catch (ValidationException e)
            {
                WindowsFormUtils.mensajeDeError(e.Message);
                camposValidos = false;
            }
            return camposValidos;
        }

       
        #endregion
        */
       
    }
}
