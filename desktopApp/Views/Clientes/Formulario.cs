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
            else
            {
                cliente = db.Cliente.Find(id);
            }

            cliente.nombre = txtNombre.Text;
            cliente.apellido = txtApellido.Text;
            cliente.mail = txtEmail.Text;
            cliente.telefono = txtTelefono.Text;
            cliente.fecha_nacimiento = dtpFechaNacimiento.Value;
            // Justo en este caso se guarda el string como tipo de documento
            cliente.tipo_documento = cmbBxTipoDocumento.GetItemText(cmbBxTipoDocumento.SelectedItem);
            cliente.numero_documento = Decimal.Parse(txtNroDocumento.Text);
            cliente.cuil = txtCuil.Text;
            cliente.calle = txtDireccion.Text;
            cliente.portal = Decimal.Parse(txtPortal.Text);
            cliente.piso = Decimal.Parse(txtNroPiso.Text);
            cliente.departamento = txtDepto.Text;
            cliente.localidad = txtLocalidad.Text;
            cliente.codigo_postal = txtCodigoPostal.Text;
            cliente.tarjeta_credito = recortarTarjetaDeCredito(txtTarjeta.Text);

            if (esAdminEditando())
            {
                cliente.Usuario.habilitado = chkBxHabilitado.Checked;
            }

            if (!editando())
            {
                cliente.Usuario = UsuariosUtils.usuarioAAsignar(db, UsuariosUtils.generarUsername(cliente), cliente, Model.TipoRol.CLIENTE);
                db.Cliente.Add(cliente);
            }
        }

        #region VALIDACIONES
        override protected bool camposYDominioValidos() {
            bool valido = true;
            List<string> errores = new List<string>();

            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtNombre, "nombre"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.campoAlfabetico(txtNombre, "nombre"), ref errores);

            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtApellido, "apellido"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.campoAlfabetico(txtApellido, "apellido"), ref errores);

            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtEmail, "mail"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.emailValido(txtEmail, "mail"), ref errores);

            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoLongitudEntre(txtTelefono, "telefono", 8, 11), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.campoNumericoYPositivo(txtTelefono, "telefono"), ref errores);

            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(dtpFechaNacimiento, "fecha de nacimiento"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.fechaMenorAHoy(dtpFechaNacimiento, "fecha de nacimiento"), ref errores);

            ValidationsUtils.hayError(() => ValidationsUtils.opcionObligatoria(cmbBxTipoDocumento, "tipo de documento"), ref errores);
            
            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoLongitudFija(txtNroDocumento, "nro. de documento", 8), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.campoNumericoYPositivo(txtNroDocumento, "nro. de documento"), ref errores);

            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtCuil, "CUIL"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.cuilOCuitValido(txtCuil, "CUIL"), ref errores);

            ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtDireccion, "dirección"), ref errores);
            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtPortal, "portal"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.campoNumericoYPositivo(txtPortal, "portal"), ref errores);

            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtNroPiso, "nro. piso"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.campoNumericoYPositivo(txtNroPiso, "nro. piso"), ref errores);

            ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtDepto, "departamento"), ref errores);
            
            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtLocalidad, "localidad"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.campoAlfabetico(txtLocalidad, "localidad"), ref errores);

            ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtCodigoPostal, "codigo postal"), ref errores);

            if (errores.Count() > 0)
            {
                WindowsFormUtils.mostrarErrores(errores);
                valido = false;
            } else {
                valido = validarDominio(ref errores);
            }

            return valido;
        }

        override protected bool validarDominio(ref List<string> errores)
        {
            ValidationsUtils.hayError(documentoNoRepetido, ref errores);
            ValidationsUtils.hayError(cuilNoRepetido, ref errores);

            if (errores.Count() > 0)
            {
                WindowsFormUtils.mostrarErrores(errores);
                return false;
            }

            return true;
        }

        private void documentoNoRepetido()
        {
            String tipoDoc = cmbBxTipoDocumento.SelectedText;
            Cliente otroCliente = BaseDeDatos.BaseDeDatos.clientePorDocumento(tipoDoc, txtNroDocumento.Text);
            if (otroCliente != null)
            {
                if ((editando() && id != otroCliente.id_usuario) || !editando())
                {
                    throw new ValidationException("Ya existe otro cliente con este tipo y numero de documento");
                }
            }
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

        #region TARJETACREDITO
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
        #endregion

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

        protected override TextBox textBoxCui()
        {
            return txtCuil;
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
