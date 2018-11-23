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

namespace PalcoNet.Clientes
{
    public partial class Formulario : UsuarioFormulario
    {
        Cliente cliente = new Cliente();
        private String caracteresOcultosTarjeta = "****";
        private int digitosBaseTarjeta = 6;
        private int digitosFinalTarjeta = 4;

        public Formulario(int? id = null) : base(id)
        {
            InitializeComponent();

            cargarComboTipoDocumento();          

            // TODO: sacar cuando funque, lo dejo para no completar todo a mano todo el tiempo
            if (!editando())
            {
                txtNombre.Text = "Kevin";
                txtApellido.Text = "Szuchet";
                txtEmail.Text = "kevinszuchet@gmail.com";
                txtTelefono.Text = "1140495754";
                txtNroDocumento.Text = "40539748";
                txtCuil.Text = "20405397480";
                txtDireccion.Text = "JRV";
                txtPortal.Text = "225";
                txtNroPiso.Text = "1";
                txtDepto.Text = "A";
                txtLocalidad.Text = "CABA";
                txtCodigoPostal.Text = "1414";
                txtTarjeta.Text = "1234567891011121";
            }

            if (editando())
            {
                cargarDatos();
            }

            if (hayQueMostrarPanelAdmin()) mostrarPanelAdmin();
        }

        override protected void mostrarPanelAdmin()
        {
            pnlDatosUsuario.Visible = true;
        }

        protected override void asignarEntidades(RagnarEntities db)
        {
            if (id == null)
            {
                cliente = new Cliente();
            }

            cliente.nombre = txtNombre.Text;
            cliente.apellido = txtApellido.Text;
            cliente.mail = txtEmail.Text;
            cliente.telefono = txtTelefono.Text;
            cliente.fecha_nacimiento = dtpFechaNacimiento.Value;
            // Justo en este caso se guarda el string como tipo de documento
            // TODO: revisar si esta bien o es mejor manejarlo numericamente
            cliente.tipo_documento = Utils.WindowsFormUtils.seleccionadoDe(cmbBxTipoDocumento);
            cliente.numero_documento = Decimal.Parse(txtNroDocumento.Text);
            cliente.cuil = txtCuil.Text;
            cliente.calle = txtDireccion.Text;
            cliente.portal = Decimal.Parse(txtPortal.Text);
            cliente.piso = Decimal.Parse(txtNroPiso.Text);
            cliente.departamento = txtDepto.Text;
            cliente.localidad = txtLocalidad.Text;
            cliente.codigo_postal = txtCodigoPostal.Text;
            cliente.tarjeta_credito = recortarTarjetaDeCredito(txtTarjeta.Text);

            if (SessionUtils.esAdmin() && editando())
            {
                cliente.Usuario.habilitado = chkBxHabilitado.Checked;
            }

            if (!editando())
            {
                cliente.Usuario = UsuariosUtils.usuarioAAsignar(db, UsuariosUtils.generarUsername(cliente), cliente, Model.TipoRol.CLIENTE);
                db.Cliente.Add(cliente);
            }
            else
            {
                //actualizamos tambien el usuario porque podria haber cambiado el checkbox de habilitado
                db.Entry(cliente.Usuario).State = System.Data.Entity.EntityState.Modified;
                db.Entry(cliente).State = System.Data.Entity.EntityState.Modified;
            }
        }

        private void documentoNoRepetido()
        {
            String tipoDoc = Utils.WindowsFormUtils.seleccionadoDe(cmbBxTipoDocumento);
            Cliente otroCliente = BaseDeDatos.BaseDeDatos.clientePorDocumento(tipoDoc, txtNroDocumento.Text);
            if (otroCliente != null)
            {
                if ((editando() && id != otroCliente.id_usuario) || !editando())
                {
                    throw new ValidationException("Ya existe otro cliente con este tipo y numero de documento");
                }
            }                
        }

        #region VALIDACIONES
        override protected bool validarDominio()
        {
            try
            {
                documentoNoRepetido();
                cuilNoRepetido();
            }
            catch (ValidationException e)
            {
                WindowsFormUtils.mensajeDeError(e.Message);
                return false;
            }
            return true;
        }

        override protected bool camposValidos() {
            bool camposValidos = true;
            try {
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
            } catch(ValidationException e) {
                WindowsFormUtils.mensajeDeError(e.Message);
                camposValidos = false;
            }
            return camposValidos;
        }
        #endregion

        #region CARGADATOS
        override protected void cargarDatos() {
                using (RagnarEntities db = new RagnarEntities()) {
                    try {
                        cliente = db.Cliente.Find(id);
                        txtNombre.Text = cliente.nombre;
                        txtApellido.Text = cliente.apellido;
                        txtEmail.Text = cliente.mail;
                        txtTelefono.Text = cliente.telefono;
                        dtpFechaNacimiento.Value = cliente.fecha_nacimiento;
                        cmbBxTipoDocumento.Text = cliente.tipo_documento;
                        txtNroDocumento.Text = cliente.numero_documento.ToString();
                        txtCuil.Text = cliente.cuil;
                        txtDireccion.Text = cliente.calle;
                        txtPortal.Text = cliente.portal.ToString();
                        txtNroPiso.Text = cliente.piso.ToString();
                        txtDepto.Text = cliente.departamento;
                        txtLocalidad.Text = cliente.localidad;
                        txtCodigoPostal.Text = cliente.codigo_postal;
                        txtTarjeta.Text = tarjetaConAsteriscos(cliente.tarjeta_credito);
                        chkBxHabilitado.Checked = cliente.Usuario.habilitado;
                    } catch (Exception) {
                        WindowsFormUtils.mensajeDeError("Error al intentar cargar al cliente");
                    }
                }
            }
        #endregion

        private string recortarTarjetaDeCredito(string tarjeta)
        {
            if (tarjeta != "")
            {
                return tarjeta.Substring(0, digitosBaseTarjeta) + tarjeta.Substring(tarjeta.Length - digitosFinalTarjeta, digitosFinalTarjeta);
            }
            return null;
        }

        private string tarjetaConAsteriscos(string tarjeta)
        {
            if (tarjeta != null)
            {
                return tarjeta.Substring(0, digitosBaseTarjeta) + caracteresOcultosTarjeta + tarjeta.Substring(tarjeta.Length - digitosFinalTarjeta, digitosFinalTarjeta);
            }

            return "";
        }

        private void Formulario_Load(object sender, EventArgs e) {
        }

        private void cargarComboTipoDocumento()
        {
            cmbBxTipoDocumento.DataSource = TipoDocumento.GetAll().Select(
                tipoDoc => new ComboBoxItem(tipoDoc.Value, tipoDoc.DisplayName)
            ).ToList();
            cmbBxTipoDocumento.ValueMember = "value";
            cmbBxTipoDocumento.DisplayMember = "text";
        }

        private void validarTarjeta()
        {
            if (editando())
            {
                //TODO mejorar esto. no se puede editar toda la tarjeta, ni tampoco se valida que los caracteres sean todos numericos!
                ValidationsUtils.campoLongitudFija(txtTarjeta, "tarjeta de credito", digitosBaseTarjeta + digitosFinalTarjeta + caracteresOcultosTarjeta.Length);
            }
            else
            {
                ValidationsUtils.campoLongitudEntre(txtTarjeta, "tarjeta de credito", 15, 16);
                ValidationsUtils.campoNumericoYPositivo(txtTarjeta, "tarjeta de credito");
            }
        }

        protected override TextBox textBoxCuil()
        {
            return txtCuil;
        }
    }
}
