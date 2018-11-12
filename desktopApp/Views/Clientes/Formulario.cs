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

namespace PalcoNet.Clientes
{
    public partial class Formulario : Form
    {
        int? id;
        Cliente cliente = new Cliente();
        private String caracteresOcultosTarjeta = "****";

        public Formulario(int? id = null)
        {
            this.id = id;
            InitializeComponent();
            
            if (editando()) {
                cargarDatos();
            }

            if (SessionUtils.esAdmin()) {
                pnlDatosUsuario.Visible = true;
            }
        }

        private bool editando()
        {
            return id != null;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            using (RagnarEntities db = new RagnarEntities())
            {
                if (camposValidos()) {
                    if (id != null) {
                        cliente = new Cliente();
                    }

                    cliente.nombre = txtNombre.Text;
                    cliente.apellido = txtApellido.Text;
                    cliente.mail = txtEmail.Text;
                    cliente.telefono = txtTelefono.Text;
                    cliente.fecha_nacimiento = dtpFechaNacimiento.Value;
                    cliente.tipo_documento = cmbBxTipoDocumento.SelectedValue.ToString();
                    cliente.numero_documento = Decimal.Parse(txtNroDocumento.Text);
                    cliente.cuil = txtCuil.Text;
                    cliente.portal = Decimal.Parse(txtPortal.Text);
                    cliente.piso = Decimal.Parse(txtNroPiso.Text);
                    cliente.departamento = txtDepto.Text;
                    cliente.localidad = txtLocalidad.Text;
                    cliente.codigo_postal = txtCodigoPostal.Text;
                    cliente.tarjeta_credito = recortarTarjetaDeCredito(txtTarjeta.Text);

                    // TODO: chequear si esta el checkbox
                    cliente.habilitado = chkBxHabilitado.Checked;

                    if (!editando()) {
                        db.Cliente.Add(cliente);
                    } else {
                        db.Entry(cliente).State = System.Data.Entity.EntityState.Modified;
                    }

                    WindowsFormUtils.guardarYCerrar(db, this);
                }
            }
        }

        #region VALIDACIONES
        private bool camposValidos() {
            bool camposValidos = true;
            try {
                ValidationsUtils.campoObligatorio(txtNombre, "nombre");
                ValidationsUtils.campoAlfabetico(txtNombre, "nombre");
                ValidationsUtils.campoObligatorio(txtApellido, "apellido");
                ValidationsUtils.campoAlfabetico(txtApellido, "apellido");
                ValidationsUtils.campoObligatorio(txtEmail, "mail");
                ValidationsUtils.emailValido(txtEmail, "mail");
                if (validable(txtTelefono))
                {
                    ValidationsUtils.campoLongitudMaxima(txtTelefono, "telefono", 8, 11);
                    ValidationsUtils.campoNumericoYPositivo(txtTelefono, "telefono");
                }
                // TODO: validar que la fecha de nacimiento no puede ser posterior a la del archivo de configuracion
                ValidationsUtils.campoObligatorio(dtpFechaNacimiento, "fecha de nacimiento");
                ValidationsUtils.campoObligatorio(cmbBxTipoDocumento, "tipo de documento");
                ValidationsUtils.campoLongitudFija(txtNroDocumento, "nro. de documento", 8);
                ValidationsUtils.campoNumericoYPositivo(txtNroDocumento, "nro. de documento");
                if (validable(txtCuil)) ValidationsUtils.cuilValido(txtCuil);
                ValidationsUtils.campoObligatorio(txtDireccion, "dirección");
                ValidationsUtils.campoObligatorio(txtPortal, "portal");
                ValidationsUtils.campoNumericoYPositivo(txtPortal, "portal");
                ValidationsUtils.campoObligatorio(txtNroPiso, "nro. piso");
                ValidationsUtils.campoNumericoYPositivo(txtNroPiso, "nro. piso");
                ValidationsUtils.campoObligatorio(txtDepto, "departamento");
                if (validable(txtLocalidad))
                {
                    ValidationsUtils.campoObligatorio(txtLocalidad, "localidad");
                    ValidationsUtils.campoAlfabetico(txtLocalidad, "localidad");
                }
                //TODO validar codigo postal que sea valido (al menos para argentina)
                ValidationsUtils.campoObligatorio(txtCodigoPostal, "codigo postal");
                if (validable(txtTarjeta))
                {
                    ValidationsUtils.campoLongitudMaxima(txtTarjeta, "tarjeta de credito", 15, 16);
                    ValidationsUtils.campoNumericoYPositivo(txtTarjeta, "tarjeta de credito");
                }
            } catch(ValidationException e) {
                WindowsFormUtils.mensajeDeError(e.Message);
                camposValidos = false;
            }
            return camposValidos;
        }
        #endregion

        #region HELPER
        private void cargarDatos() {
                using (RagnarEntities db = new RagnarEntities()) {
                    cliente = db.Cliente.Find(id);

                    txtNombre.Text = cliente.nombre;
                    txtApellido.Text = cliente.apellido;
                    txtEmail.Text = cliente.mail;
                    txtTelefono.Text = cliente.telefono;
                    dtpFechaNacimiento.Value = cliente.fecha_nacimiento;
                    cmbBxTipoDocumento.SelectedValue = cliente.tipo_documento;
                    txtNroDocumento.Text = cliente.numero_documento.ToString();
                    txtCuil.Text = cliente.cuil;
                    txtDireccion.Text = cliente.calle;
                    txtPortal.Text = cliente.portal.ToString();
                    txtNroPiso.Text = cliente.piso.ToString();
                    txtDepto.Text = cliente.departamento;
                    txtLocalidad.Text = cliente.localidad;
                    txtCodigoPostal.Text = cliente.codigo_postal;
                    txtTarjeta.Text = tarjetaConAsteriscos(cliente.tarjeta_credito);
                }
            }
        #endregion

        private void btnCambiarPass_Click(object sender, EventArgs e)
        {
            new Usuarios.ModificarClaveAdmin().ShowDialog();
        }

        private string recortarTarjetaDeCredito(string tarjeta) {
            return tarjeta.Substring(0, 6) + tarjeta.Substring(tarjeta.Length - 4, 4);
        }

        private string tarjetaConAsteriscos(string tarjeta)
        {
            if (tarjeta != null)
            {
                return tarjeta.Substring(0, 6) + caracteresOcultosTarjeta + tarjeta.Substring(tarjeta.Length - 4, 4);
            }

            return "";
        }

        private void Formulario_Load(object sender, EventArgs e)
        {
            /* TODO: cargar todos los tipos en el combo de tipos de documento
             * TipoDocumento.GetAll().Select(
                tipoDoc => 
            );*/
        }

        private bool validable(Control input)
        {
            return !editando() || (editando() && input.Text != "");
        }
    }
}
