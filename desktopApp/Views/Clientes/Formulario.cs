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
    public partial class Formulario : Form
    {
        int? id;
        Cliente cliente = new Cliente();
        private String caracteresOcultosTarjeta = "****";
        private Dictionary<string, string> noObligatoriosEdicion = new Dictionary<string, string>();
        private int digitosBaseTarjeta = 6;
        private int digitosFinalTarjeta = 4;
        private Form destino;

        public Formulario(int? id = null, Form destino = null)
        {
            this.id = id;
            this.destino = destino;
            InitializeComponent();

            cargarComboTipoDocumento();
            
            if (editando()) {
                cargarDatos();
            }

            // TODO: sacar cuando funque, lo dejo para no completar todo a mano todo el tiempo
            /*txtNombre.Text = "Kevin";
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
            txtTarjeta.Text = "1234567891011121";*/

            if (SessionUtils.esAdmin()) {
                if(editando())
                {
                    pnlDatosUsuario.Visible = true;
                }
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

                    if (!validarDominio())
                    {
                        return;
                    }

                    if (id == null) {
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

                    if (SessionUtils.esAdmin() && editando()) {
                        cliente.habilitado = chkBxHabilitado.Checked;
                    }

                    if (!editando()) {
                        UsuariosUtils.dbContext = db;
                        //TODO testear esto
                        cliente.Rol.Add(BaseDeDatos.BaseDeDatos.obtenerRol(Model.TipoRol.CLIENTE.DisplayName));
                        cliente.Usuario = UsuariosUtils.usuarioAAsignar(UsuariosUtils.generarUsername(cliente), cliente);
                        db.Cliente.Add(cliente);
                    } else {
                        db.Entry(cliente).State = System.Data.Entity.EntityState.Modified;
                    }

                    WindowsFormUtils.guardarYCerrar(db, this, destino);
                }
            }
        }

        private bool validarDominio()
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

        private void cuilNoRepetido()
        {
            if (!validable(txtCuil, "cuil")) return;

            Cliente otroCliente = BaseDeDatos.BaseDeDatos.clientePorCuil(txtCuil.Text);
            if (otroCliente != null)
            {
                if ((editando() && id != otroCliente.id_usuario) || !editando())
                {
                    throw new ValidationException("Ya existe otro cliente con este cuil");
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
                if (validable(txtTelefono, "telefono"))
                {
                    ValidationsUtils.campoLongitudEntre(txtTelefono, "telefono", 8, 11);
                    ValidationsUtils.campoNumericoYPositivo(txtTelefono, "telefono");
                }
                // TODO: validar que la fecha de nacimiento no puede ser posterior a la del archivo de configuracion
                ValidationsUtils.campoObligatorio(dtpFechaNacimiento, "fecha de nacimiento");
                ValidationsUtils.opcionObligatoria(cmbBxTipoDocumento, "tipo de documento");
                ValidationsUtils.campoLongitudFija(txtNroDocumento, "nro. de documento", 8);
                ValidationsUtils.campoNumericoYPositivo(txtNroDocumento, "nro. de documento");
                if (validable(txtCuil, "cuil"))
                {
                    ValidationsUtils.campoObligatorio(txtCuil, "cuil");
                    ValidationsUtils.cuilValido(txtCuil);
                }
                ValidationsUtils.campoObligatorio(txtDireccion, "dirección");
                ValidationsUtils.campoObligatorio(txtPortal, "portal");
                ValidationsUtils.campoNumericoYPositivo(txtPortal, "portal");
                ValidationsUtils.campoObligatorio(txtNroPiso, "nro. piso");
                ValidationsUtils.campoNumericoYPositivo(txtNroPiso, "nro. piso");
                ValidationsUtils.campoObligatorio(txtDepto, "departamento");
                if (validable(txtLocalidad, "localidad"))
                {
                    ValidationsUtils.campoObligatorio(txtLocalidad, "localidad");
                    ValidationsUtils.campoAlfabetico(txtLocalidad, "localidad");
                }
                ValidationsUtils.campoObligatorio(txtCodigoPostal, "codigo postal");
                if (validable(txtTarjeta, "tarjetaCredito"))
                {
                    validarTarjeta();
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

        private void btnCambiarPass_Click(object sender, EventArgs e)
        {
            new Usuarios.ModificarClaveAdmin().ShowDialog();
        }

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
            if (editando())
            {
                cargarDatosNoValidablesEnEdicion();
            }
        }

        private void cargarComboTipoDocumento()
        {
            cmbBxTipoDocumento.DataSource = TipoDocumento.GetAll().Select(
                tipoDoc => new ComboBoxItem(tipoDoc.Value, tipoDoc.DisplayName)
            ).ToList();
            cmbBxTipoDocumento.ValueMember = "value";
            cmbBxTipoDocumento.DisplayMember = "text";
        }

        private void cargarDatosNoValidablesEnEdicion()
        {
            noObligatoriosEdicion.Add("cuil", txtCuil.Text);
            noObligatoriosEdicion.Add("telefono", txtTelefono.Text);
            noObligatoriosEdicion.Add("localidad", txtLocalidad.Text);
            noObligatoriosEdicion.Add("tarjetaCredito", txtTarjeta.Text);
        }

        private bool validable(Control input, String nombreCampo) {
            return !editando() || (editando() && noObligatoriosEdicion[nombreCampo] != "");
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
    }
}
