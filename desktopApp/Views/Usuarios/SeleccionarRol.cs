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

namespace PalcoNet.Usuarios
{
    public partial class SeleccionarRol : Form
    {
        public SeleccionarRol()
        {
            InitializeComponent();

            this.cmbBxRol.DataSource = BaseDeDatos.BaseDeDatos.obtenerRolesDelUsuario(Global.usuarioLogueado);
        }

        public bool tieneAlgunRol(Usuario usuario)
        {
            return BaseDeDatos.BaseDeDatos.tieneAlgunRol(usuario);
        }

        private void redirijirA(Rol rol) {
            Global.rolUsuario = rol;
            this.Hide();
            new Home().Show();
        }
        
        private void btnIngresar_Click(object sender, EventArgs e)
        {
            redirijirA(convertirStringARol(cmbBxRol.Text));
        }

        private Rol convertirStringARol(String rolSeleccionado)
        {
            return BaseDeDatos.BaseDeDatos.rolPorNombre(rolSeleccionado);
        }

        private void SeleccionarRol_Load(object sender, EventArgs e)
        {

        }

        private void cmbBxRol_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
    }
}
