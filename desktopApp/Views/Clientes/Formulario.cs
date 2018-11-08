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
        int? id;
        Cliente cliente = new Cliente();

        public Formulario(int? id = null)
        {
            this.id = id;
            InitializeComponent();
            
            if (id != null) {
                cargarDatos();
            }

            //TODO validar si quien esta logueado es admin. Si lo es hay que cambiar el boton de cambio de pass, y mostrar el panel de habilitado
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // TODO: faltan las validaciones
            using (RagnarEntities db = new RagnarEntities())
            {
                if (id != null) {
                    cliente = new Cliente();
                }

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
                cliente.departamento = txtDepto.Text;
                cliente.localidad = txtLocalidad.Text;
                cliente.codigo_postal = txtCodigoPostal.Text;
                // TODO: Recortar tarjeta
                cliente.tarjeta_credito = txtTarjeta.Text;

                if (id == null) {
                    db.Cliente.Add(cliente);
                } else {
                    db.Entry(cliente).State = System.Data.Entity.EntityState.Modified;
                }

                db.SaveChanges();

                this.Close();
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

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
                    // TODO: falta la direccion
                    txtPortal.Text = cliente.portal.ToString();
                    txtNroPiso.Text = cliente.piso.ToString();
                    txtDepto.Text = cliente.departamento;
                    txtLocalidad.Text = cliente.localidad;
                    txtCodigoPostal.Text = cliente.codigo_postal;
                    // TODO: Recortar tarjeta
                    txtTarjeta.Text = cliente.tarjeta_credito;
                }
            }
        #endregion

        private void btnCambiarPass_Click(object sender, EventArgs e)
        {
            new Usuarios.ModificarClaveUsuario().Show();
        }

    }
}
