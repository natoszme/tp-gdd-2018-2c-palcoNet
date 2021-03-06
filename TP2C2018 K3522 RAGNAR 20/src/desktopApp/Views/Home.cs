﻿using System;
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

namespace PalcoNet
{
    public partial class Home : Form
    {
        private List<String> funcionalidadesTotales = new List<String>();
        private List<Button> botonesTotales = new List<Button>();

        public Home()
        {
            InitializeComponent();
            cargarFuncionalidadesDisponibles();
        }

        private void cargarFuncionalidadesDisponibles()
        {
            //es necesario tenerlo harcodeado aca porque hay funcionalidades de la db que no son exactamente un boton de la home, como por ejemplo el Registro de Usuario
            funcionalidadesTotales.Add("ABM de Rol");
            botonesTotales.Add(btnAbmRol);
            funcionalidadesTotales.Add("ABM de Cliente");
            botonesTotales.Add(btnAbmClientes);
            funcionalidadesTotales.Add("ABM de Empresa de Espectaculos");
            botonesTotales.Add(btnAbmEmpresa);
            funcionalidadesTotales.Add("ABM de Grado de Publicacion");
            botonesTotales.Add(btnAbmGrado);
            funcionalidadesTotales.Add("ABM de Publicacion");
            botonesTotales.Add(btnAbmPublicaciones);
            funcionalidadesTotales.Add("Comprar");
            botonesTotales.Add(btnComprar);
            funcionalidadesTotales.Add("Historial de Cliente");
            botonesTotales.Add(btnHistorial);
            funcionalidadesTotales.Add("Canje y Administracion de Puntos");
            botonesTotales.Add(btnCanjePuntos);
            funcionalidadesTotales.Add("Generar rendicion de comisiones");
            botonesTotales.Add(btnRendicionComisiones);
            funcionalidadesTotales.Add("Listado estadistico");
            botonesTotales.Add(btnListadoEstadistico);
        }

        private void Home_Load(object sender, EventArgs e)
        {
            habilitarBotonesFuncionalidades();
            lblUsername.Text = "Bienvenido, " + obtenerUsernameUsuarioActual();
        }

        private string obtenerUsernameUsuarioActual()
        {
            return Global.obtenerUsuarioLogueado().usuario;
        }

        public void habilitarBotonesFuncionalidades()
        {
            List<Funcionalidad> funcionalidadesDisponibles = obtenerFuncionalidadesDisponibles();
            funcionalidadesDisponibles.ForEach(fDisp => {

                if (funcionalidadesTotales.Contains(fDisp.descripcion))
                {
                    int numeroBotonAMostrar = funcionalidadesTotales.IndexOf(fDisp.descripcion);
                    botonesTotales.ElementAt(numeroBotonAMostrar).Visible = true;
                }
                            
            });
        }

        //un cliente inhabilitado no puede comprar en la plataforma
        private List<Funcionalidad> obtenerFuncionalidadesDisponibles()
        {
            List<Funcionalidad> disponibles = Global.obtenerRolUsuario().Funcionalidad.ToList();

            return disponibles;
        }

        private bool estaInhabilitado()
        {
            return !Global.obtenerUsuarioLogueado().habilitado;
        }

        private void btnAbmRol_Click(object sender, EventArgs e)
        {
            new Roles.Listado().Show();
            this.Hide();
        }

        private void btnRegistroUsuario_Click(object sender, EventArgs e)
        {

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
            new Publicaciones.Listado().Show();
            this.Hide();
        }


        private void btnComprar_Click(object sender, EventArgs e)
        {
            new Views.Publicaciones.ComprarEntrada().Show();
            this.Hide();
        }

        private void btnHistorial_Click(object sender, EventArgs e)
        {
            //Hay algo raro aca, solo te deja hacerlo desde clientess.Historial
            //Se debe haber cagado algo cuando cambiamos todo de nombre
            new Clientes.Historial().Show();
            this.Hide();
           // MessageBox.Show("Hay un error con los nombres que no permite que funcione, chequear el codigo del onClick y ver el comentario");
        }

        private void btnCanjePuntos_Click(object sender, EventArgs e)
        {
            new Clientes.CanjearPuntos().Show();
            this.Hide();
        }

        private void btnRendicionComisiones_Click(object sender, EventArgs e)
        {
            new Views.RendicionComisiones.Listado().Show();
            this.Hide();
        }

        private void btnModificarContraseña_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Usuarios.ModificarClaveUsuario().Show();
        }

        private void btnListadoEstadistico_Click(object sender, EventArgs e)
        {
            WindowsFormUtils.abrirFormulario(new Views.Reportes.Listado(), () => { });
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Global.desloguearUsuario();
            WindowsFormUtils.volverALogin(this);
        }
    }
}
