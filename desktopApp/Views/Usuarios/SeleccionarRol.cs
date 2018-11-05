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
        BaseDeDatos.BaseDeDatos db = new BaseDeDatos.BaseDeDatos();
        public SeleccionarRol()
        {
            InitializeComponent();

            List<TipoRol> roles = obtenerRolesDeId(Global.idUsuario);
          //  roles.Add(TipoRol.Cliente);
            //roles.Add(TipoRol.Administrativo);

            this.cmbBxRol.DataSource = roles;
        }

        public bool tieneAlgunRol(int idUsuario)
        {
            return db.tieneAlgunRol(idUsuario);
        }

        List<TipoRol> obtenerRolesDeId(int idUsuario)
        {
            List<TipoRol> roles = new List<TipoRol>();
            return obtenerRolesPorIdEnTexto(idUsuario).Select(rol => convertirStringARol(rol)).ToList();
        }

        private List<String> obtenerRolesPorIdEnTexto(int idUsuario)
        {
            return db.obtenerRolesPorIdEnTexto(idUsuario);     
        }

        private void redirijirA(TipoRol rol)
        {
            Global.setearRolDeSesion(rol);
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
        
    }
}
