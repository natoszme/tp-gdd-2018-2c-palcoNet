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

            if (SessionUtils.esAdmin() && editando())
            {
                empresa.Usuario.habilitado = chkBxHabilitado.Checked;
            }

            if (!editando())
            {
                empresa.Usuario = UsuariosUtils.usuarioAAsignar(db, UsuariosUtils.generarUsername(empresa), empresa, Model.TipoRol.EMPRESA);
                db.Empresa.Add(empresa);
            }
            else
            {
                //actualizamos tambien el usuario porque podria haber cambiado el checkbox de habilitado
                db.Entry(empresa.Usuario).State = System.Data.Entity.EntityState.Modified;
                db.Entry(empresa).State = System.Data.Entity.EntityState.Modified;
            }
        }

        #region VALIDACIONES
        override protected bool validarDominio()
        {
            try
            {
                cuilNoRepetido();
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
                ValidationsUtils.campoObligatorio(txtRazonSocial, "razon social");
                ValidationsUtils.campoAlfabetico(txtRazonSocial, "razon social");
                ValidationsUtils.campoObligatorio(txtEmail, "mail");
                ValidationsUtils.emailValido(txtEmail, "mail");
                ValidationsUtils.campoLongitudEntre(txtTelefono, "telefono", 8, 11);
                ValidationsUtils.campoNumericoYPositivo(txtTelefono, "telefono");
                ValidationsUtils.campoObligatorio(txtCuit, "CUIT");
                ValidationsUtils.cuilOCuitValido(txtCuit, "CUIT");
                ValidationsUtils.campoObligatorio(txtDireccion, "dirección");
                ValidationsUtils.campoObligatorio(txtPortal, "portal");
                ValidationsUtils.campoNumericoYPositivo(txtPortal, "portal");
                ValidationsUtils.campoObligatorio(txtNroPiso, "nro. piso");
                ValidationsUtils.campoNumericoYPositivo(txtNroPiso, "nro. piso");
                ValidationsUtils.campoObligatorio(txtDepto, "departamento");
                ValidationsUtils.campoObligatorio(txtCuidad, "ciudad");
                ValidationsUtils.campoAlfabetico(txtCuidad, "ciudad");
                ValidationsUtils.campoObligatorio(txtLocalidad, "localidad");
                ValidationsUtils.campoAlfabetico(txtLocalidad, "localidad");
                ValidationsUtils.campoObligatorio(txtCodigoPostal, "codigo postal");
            }
            catch (ValidationException e)
            {
                WindowsFormUtils.mensajeDeError(e.Message);
                camposValidos = false;
            }
            return camposValidos;
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

        protected override TextBox textBoxCuil()
        {
            return txtCuit;
        }
    }
}
