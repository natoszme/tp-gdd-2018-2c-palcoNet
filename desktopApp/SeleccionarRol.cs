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
    public partial class SeleccionarRol : Form
    {
        public SeleccionarRol()
        {
            InitializeComponent();

            List<TipoRol> roles = obtenerRoles();
            roles.Add(TipoRol.Cliente);
            roles.Add(TipoRol.Administrativo);

            switch (roles.Count)
            {
                case 1:
                {
                    redirijirA(roles[0]);
                } break;

                default:
                {
                    this.cmbBxRol.DataSource = obtenerRoles();
                } break;
            }
        }

        public bool tieneAlgunRol(int idUsuario)
        {
            //TODO ir a db
            return true;
        }

        List<TipoRol> obtenerRoles()
        {
            List<TipoRol> roles = new List<TipoRol>();
            return obtenerRolesEnTexto().Select(rol => convertirStringARol(rol)).ToList();
        }

        private List<String> obtenerRolesEnTexto()
        {
            //TODO ir a db (buscar los roles activos)
            List<String> roles = new List<String>();
            roles.Add("Administrativo");
            return roles;
        }

        void redirijirA(TipoRol rol)
        {
            switch(rol){
                case TipoRol.Administrativo:
                {
                    MessageBox.Show("Bienvenido administrativo");
                } break;
                case TipoRol.Cliente:
                {
                    MessageBox.Show("Bienvenido cliente");
                } break;
                case TipoRol.Empresa:
                {
                    MessageBox.Show("Bienvenido empresa");
                } break;
                default:
                {
                    //Error
                } break;
            }
        
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
                } break;
            }

            return tipo;
        }

        private void SeleccionarRol_Load(object sender, EventArgs e)
        {

        }
        
    }
}
