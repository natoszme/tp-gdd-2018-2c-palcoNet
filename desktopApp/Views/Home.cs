using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalcoNet
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            habilitarBotonesFuncionalidades();
        }

        public void habilitarBotonesFuncionalidades()
        {

        }

        private void btnAbmRol_Click(object sender, EventArgs e)
        {
            new Roles.Listado().Show();
            this.Hide();
        }

        private void btnRegistroUsuario_Click(object sender, EventArgs e)
        {
            new Usuarios.Signup().Show();
            this.Hide();
        }

        private void btnAbmClientes_Click(object sender, EventArgs e)
        {
            new Clientes.Listado().Show();
            this.Hide();
        }

        private void btnAbmEmpresa_Click(object sender, EventArgs e)
        {
            new Empresas.Listado().Show();
            this.Hide();
        }

        private void btnAbmGrado_Click(object sender, EventArgs e)
        {
            new Grados.Listado().Show();
            this.Hide();
        }

        private void btnGenerarPublicacion_Click(object sender, EventArgs e)
        {
            new Publicaciones.Alta().Show();
            this.Hide();
        }

        private void btnEditarPublicacion_Click(object sender, EventArgs e)
        {
            // new Publicaciones.().Show(); Crear el formulario para editar una publicacion
            //this.Hide();
            MessageBox.Show("Todavia no esta implementado");
        }

        private void btnComprar_Click(object sender, EventArgs e)
        {
            // new Publicaciones.().Show(); Crear el formulario para comrar una entrada
            //this.Hide();
            MessageBox.Show("Todavia no esta implementado");
        }

        private void btnHistorial_Click(object sender, EventArgs e)
        {
            //Hay algo raro aca, solo te deja hacerlo desde clientess.Historial
            //Se debe haber cagado algo cuando cambiamos todo de nombre
            //new Clientes.Historial().Show();
           // this.Hide();
            MessageBox.Show("Hay un error con los nombres que no permite que funcione, chequear el codigo del onClick y ver el comentario");
        }

        private void btnCanjePuntos_Click(object sender, EventArgs e)
        {
            new Clientes.CanjearPuntos().Show();
            this.Hide();
        }

        private void btnRendicionComisiones_Click(object sender, EventArgs e)
        {
            // new RendirComisiones.Show(); Crear el formulario para rendir comisiones
            //this.Hide();
            MessageBox.Show("Todavia no esta implementado");
        }

        private void btnModificarContraseña_Click(object sender, EventArgs e)
        {
            if (Global.rolUsuario == Model.TipoRol.Administrativo)
            {
                new Usuarios.ModificarClaveAdmin().Show();
                this.Hide();
            }
            else if (Global.rolUsuario == Model.TipoRol.Cliente)
            {
                new Usuarios.ModificarClaveUsuario().Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("El cambio de contraseña para un usuario con un rol que no sea cliente o administrativo no esta implementado");
            }
        }


    }
}
