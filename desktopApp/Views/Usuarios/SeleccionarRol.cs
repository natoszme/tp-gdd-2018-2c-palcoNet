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

            List<TipoRol> roles = obtenerRolesDeId(Global.usuarioLogueado);
            this.cmbBxRol.DataSource = roles;
        }

        public bool tieneAlgunRol(Usuario usuario)
        {
            return BaseDeDatos.BaseDeDatos.tieneAlgunRol(usuario);
        }

        List<TipoRol> obtenerRolesDeId(Usuario usuario)
        {
            List<TipoRol> roles = new List<TipoRol>();
            return obtenerRolesPorIdEnTexto(usuario).Select(rol => convertirStringARol(rol)).ToList();
        }

        private List<String> obtenerRolesPorIdEnTexto(Usuario usuario)
        {
            return BaseDeDatos.BaseDeDatos.obtenerRolesDelUsuario(usuario);     
        }

        private void redirijirA(TipoRol rol) {
            Global.rolUsuario = rol;
            this.Hide();
            new Home().Show();
        }
        
        private void btnIngresar_Click(object sender, EventArgs e)
        {
            redirijirA(convertirStringARol(cmbBxRol.Text));
        }

        TipoRol convertirStringARol(String texto)
        {
            TipoRol tipo;

            switch (texto)
            {
                case "Administrativo":
                {
                    tipo = TipoRol.Administrativo;
                } break;
                case "Cliente":
                {
                    tipo = TipoRol.Cliente;
                } break;
                case "Empresa":
                {
                    tipo = TipoRol.Empresa;
                } break;
                default:
                {
                    throw new Exception("No existe ese rol");
                };
            }

            return tipo;
        }

        private void SeleccionarRol_Load(object sender, EventArgs e)
        {

        }

        private void cmbBxRol_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
    }
}
