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

namespace PalcoNet.Clientes
{
    public partial class Formulario : Form
    {
        public Formulario()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // TODO: faltan las validaciones
            using (RagnarEntities db = new RagnarEntities())
            {
                var cliente = new Cliente();
                cliente.nombre = txtNombre.Text;
                cliente.apellido = txtApellido.Text;
                cliente.mail = txtEmail.Text;
                cliente.telefono = txtTelefono.Text;
                cliente.fecha_nacimiento = dtpFechaNacimiento.Value;
                cliente.tipo_documento = cmbBxTipoDocumento.SelectedValue != null ? cmbBxTipoDocumento.SelectedValue.ToString() : null;
                cliente.numero_documento = Decimal.Parse(txtNroDocumento.Text);
                cliente.cuil = txtCuil.Text;
                // TODO: falta la direccion
                cliente.portal = Decimal.Parse(txtPortal.Text);
                cliente.piso = Decimal.Parse(txtNroPiso.Text);
                cliente.departamento = "";
                cliente.localidad = txtLocalidad.Text;
                cliente.codigo_postal = txtCodigoPostal.Text;
                // TODO: Recortar tarjeta
                cliente.tarjeta_credito = txtTarjeta.Text;

                db.Cliente.Add(cliente);
                db.SaveChanges();
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            // Cierro el dialog al apretar el btn volver
            this.ParentForm.DialogResult = DialogResult.Cancel;
        }
    }
}
