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

namespace PalcoNet.Usuarios
{
    public partial class SeleccionarRol : Form
    {
        public SeleccionarRol()
        {
            InitializeComponent();

            cargarRolesHabilitados();
        }

        private void cargarRolesHabilitados()
        {
            List<string> rolesHabilitadosDeUsuario = BaseDeDatos.BaseDeDatos.obtenerRolesHabilitadosDelUsuario(Global.usuarioLogueado);

            if(!tieneAlgunRol(rolesHabilitadosDeUsuario))
            {
                WindowsFormUtils.mensajeDeError("No tiene ningún rol habilitado. Por favor contáctese con un administrador");
                WindowsFormUtils.volverALogin(this);
                this.Close();
                return;
            }

            this.cmbBxRol.DataSource = rolesHabilitadosDeUsuario;
            this.Show();
        }

        private bool tieneAlgunRol(List<string> roles)
        {
            return roles.Any();
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
