using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PalcoNet.Utils;

namespace PalcoNet.Empresas
{
    public partial class Formulario : Form
    {
        public Formulario()
        {
            InitializeComponent();

            //TODO validar si quien esta logueado es admin. Si lo es hay que mostrar el panel pnlDatosUsuario
            //solo en el caso de que sea edicion!!
        }

        private void btnCambiarPass_Click(object sender, EventArgs e)
        {
            new Usuarios.ModificarClaveAdmin().ShowDialog();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

        }

        private bool validarDominio()
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

        private void cuilNoRepetido()
        {
            /*Cliente otroCliente = BaseDeDatos.BaseDeDatos.clientePorCuil(txtCuil.Text);
            if (otroCliente != null)
            {
                if ((editando() && id != otroCliente.id_usuario) || !editando())
                {
                    throw new ValidationException("Ya existe otro cliente con este cuil");
                }
            }*/
        }

        #region VALIDACIONES
        private bool camposValidos()
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
                ValidationsUtils.campoObligatorio(txtCuit, "cuit");
                ValidationsUtils.cuilValido(txtCuit);
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
    }
}
