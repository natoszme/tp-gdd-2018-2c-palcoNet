using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PalcoNet.BaseDeDatos;
using PalcoNet.Usuarios;
using PalcoNet.Model;
using PalcoNet.Utils;

namespace PalcoNet.Usuarios
{
    public partial class Login : Form 
    {      
        public Login()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            String username = txtUsuario.Text;
            String password = txtClave.Text;

            if (username == "" || password == "")
            {
                WindowsFormUtils.mensajeDeError("Debe ingresar usuario y contraseña");
                return;
            }

            if (!estaHabilitado())
            {
                txtClave.Text = "";
                WindowsFormUtils.mensajeDeError("Usuario inhabilitado. Por favor, contáctese con un administrador");
                return;
            }            

            using (RagnarEntities db = new RagnarEntities())
            {
                Usuario usuario = BaseDeDatos.BaseDeDatos.obtenerUsuarioPorCredenciales(db, username, password);

                if (usuario == null)
                {
                    db.SP_LoginDeUsuario(txtUsuario.Text, txtClave.Text);
                    txtClave.Text = "";
                    WindowsFormUtils.mensajeDeError("Verifique los datos ingresados y vuelva a intentarlo");
                    return;
                }
                else
                {
                    int esNuevo = usuario.es_nuevo;
                    if (esNuevo == 0)
                    {
                        loguearYPasarASeleccionarRol(usuario);
                        if (usuario.Login_fallido != null)
                        {
                            usuario.Login_fallido.nro_intento = 0;
                        }                        
                    }
                    else
                    {
                        if (esNuevo == 2)
                        {
                            WindowsFormUtils.mensajeDeError("La contraseña no fue cambiada en el primer intento. Por favor, contáctese con un administrador");
                            return;
                        }

                        //si es nuevo (es decir, es_nuevo = 1)
                        usuario.es_nuevo = 2;
                        PersonalizacionPass.seEstaPersonalizandoPass = true;
                        WindowsFormUtils.abrirFormulario(new ModificarClaveAdmin((int)usuario.id_usuario), () => finalizarSeteoDeContrasenia(db, usuario));
                        PersonalizacionPass.seEstaPersonalizandoPass = false;
                    }
                    Utils.DBUtils.guardar(db);
                }
            }
        }

        private bool estaHabilitado()
        {
            Usuario usuario = usuarioPorNombre();
            return (usuario != null && usuario.habilitado) || usuario == null;
        }

        private void loguearYPasarASeleccionarRol(Usuario usuario)
        {
            Global.loguearUsuario(usuario);

            SeleccionarRol formRoles = new SeleccionarRol();
            this.Hide();
        }

        private void btnRegistrarme_Click(object sender, EventArgs e)
        {
            this.Hide();
            WindowsFormUtils.abrirFormulario(new Usuarios.Signup(), () => { });
        }

        private void finalizarSeteoDeContrasenia(RagnarEntities db, Usuario usuario)
        {
            if(PersonalizacionPass.personalizacionPassFinalizo)
            {
                usuario.es_nuevo = 0;
            }
        }

        private Usuario usuarioPorNombre()
        {
            return BaseDeDatos.BaseDeDatos.obtenerUsuarioPorNombre(new RagnarEntities(), txtUsuario.Text);
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
