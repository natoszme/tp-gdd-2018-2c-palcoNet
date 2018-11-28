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

namespace PalcoNet.Empresas
{
    public partial class Formulario : UsuarioFormulario
    {
        Empresa empresa = new Empresa();

        public Formulario(int? id = null) : base(id)
        {
            InitializeComponent();

            if (editando())
            {
                cargarDatos();
            }

            if (hayQueMostrarPanelAdmin()) mostrarPanelAdmin();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {

        }

        protected override void asignarEntidades(RagnarEntities db)
        {
            if (id == null)
            {
                empresa = new Empresa();
            }
            else
            {
                empresa = db.Empresa.Find(id);
            }
            empresa.razon_social = txtRazonSocial.Text;
            empresa.mail = txtEmail.Text;
            empresa.telefono = txtTelefono.Text;
            empresa.cuit = txtCuit.Text;
            empresa.calle = txtDireccion.Text;
            empresa.portal = Decimal.Parse(txtPortal.Text);
            empresa.piso = Decimal.Parse(txtNroPiso.Text);
            empresa.departamento = txtDepto.Text;
            empresa.ciudad = txtCuidad.Text;
            empresa.localidad = txtLocalidad.Text;
            empresa.codigo_postal = txtCodigoPostal.Text;

            if (esAdminEditando())
            {
                empresa.Usuario.habilitado = chkBxHabilitado.Checked;
            }

            if (!editando())
            {
                empresa.Usuario = UsuariosUtils.usuarioAAsignar(db, UsuariosUtils.generarUsername(empresa), empresa, Model.TipoRol.EMPRESA);
                db.Empresa.Add(empresa);
            }
        }

        #region VALIDACIONES
        override protected bool camposYDominioValidos()
        {
            bool valido = true;
            List<string> errores = new List<string>();

            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtRazonSocial, "razon social"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.campoAlfabetico(txtRazonSocial, "razon social"), ref errores);

            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtEmail, "mail"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.emailValido(txtEmail, "mail"), ref errores);

            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoLongitudEntre(txtTelefono, "telefono", 8, 11), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.campoNumericoYPositivo(txtTelefono, "telefono"), ref errores);

            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtCuit, "CUIT"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.cuilOCuitValido(txtCuit, "CUIT"), ref errores);

            ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtDireccion, "dirección"), ref errores);
            
            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtPortal, "portal"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.campoNumericoYPositivo(txtPortal, "portal"), ref errores);

            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtNroPiso, "nro. piso"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.campoNumericoYPositivo(txtNroPiso, "nro. piso"), ref errores);

            ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtDepto, "departamento"), ref errores);
            
            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtCuidad, "ciudad"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.campoAlfabetico(txtCuidad, "ciudad"), ref errores);

            if (!ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtLocalidad, "localidad"), ref errores))
                ValidationsUtils.hayError(() => ValidationsUtils.campoAlfabetico(txtLocalidad, "localidad"), ref errores);

            ValidationsUtils.hayError(() => ValidationsUtils.campoObligatorio(txtCodigoPostal, "codigo postal"), ref errores);
            
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

        override protected bool validarDominio(ref List<string> errores)
        {
            ValidationsUtils.hayError(cuitNoRepetido, ref errores);
            
            if (errores.Count() > 0)
            {
                WindowsFormUtils.mostrarErrores(errores);
                return false;
            }
            return true;
        }

        protected void cuitNoRepetido()
        {
            Empresa otraEmpresa = BaseDeDatos.BaseDeDatos.empresaPorCuit(txtCuit.Text);
            if (otraEmpresa != null)
            {
                if ((editando() && id != otraEmpresa.id_usuario) || !editando())
                {
                    throw new ValidationException("Ya existe otra empresa con este cuit");
                }
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
                    empresa = db.Empresa.Find(id);
                    txtRazonSocial.Text = empresa.razon_social;
                    txtEmail.Text = empresa.mail;
                    txtCuit.Text = empresa.cuit;
                    txtTelefono.Text = empresa.telefono;
                    txtDireccion.Text = empresa.calle;
                    txtPortal.Text = empresa.portal.ToString();
                    txtNroPiso.Text = empresa.piso.ToString();
                    txtDepto.Text = empresa.departamento;
                    txtCuidad.Text = empresa.ciudad;
                    txtLocalidad.Text = empresa.localidad;
                    txtCodigoPostal.Text = empresa.codigo_postal;
                    chkBxHabilitado.Checked = empresa.Usuario.habilitado;
                }
                catch (Exception)
                {
                    WindowsFormUtils.mensajeDeError("Error al intentar cargar a la empresa de espectáculos");
                }
            }
        }
        #endregion

        override protected void mostrarPanelAdmin()
        {
            pnlDatosUsuario.Visible = true;
        }

        override protected TextBox textBoxCui() {
            return txtCuit;
        }

        private void btnVolver_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
