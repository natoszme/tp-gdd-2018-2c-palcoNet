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
        public SeleccionarRol(int cantidadDeRoles,int idUsuario)
        {
            InitializeComponent();

            switch (cantidadDeRoles)
            {
                case 1:
                {
                    //Redirije directamente a la ventana de ese rol
                    redirijirA(obtenerRoles(idUsuario)[0]);               
                } break;
                case 0:
                {
                    //Chequear si puede pasar esto y decir si es un error o que pasa
                } break;
                default:
                {
                    //Hay mas de 1, tengo que cargar el comboBox con sus roles
                    List<TipoRol> roles = new List<TipoRol>();
                    roles.Add(TipoRol.Cliente);
                    roles.Add(TipoRol.Administrativo);
                    //Traer los role
                    this.cmbBxRol.DataSource = obtenerRoles(idUsuario);
                } break;
            }
        }

        List<TipoRol> obtenerRoles(int idUsuario)
        {
            List<TipoRol> roles = new List<TipoRol>();
            roles.Add(TipoRol.Cliente);
            roles.Add(TipoRol.Administrativo);
            //traigo los roles de la base de datos
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
            switch (texto)
            {
                case "Administrativo":
                {
                    return TipoRol.Administrativo;
                } break;
                case "Cliente":
                {
                    return TipoRol.Cliente;
                } break;
                case "Empresa":
                {
                    return TipoRol.Empresa;
                } break;
                default:
                {
                    throw new Exception("No existe ese rol");
                } break;
            }
        }
        
    }
}
