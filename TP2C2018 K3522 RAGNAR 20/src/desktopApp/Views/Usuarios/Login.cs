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
        BaseDeDatos.BaseDeDatos db = new BaseDeDatos.BaseDeDatos();
        
        public Login()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            String username = txtUsuario.Text;
            String password = txtClave.Text;

            if (username == "" || password == "") {
                WindowsFormUtils.mensajeDeError("Debe ingresar usuario y contraseña");
                return;
            }

            //si es nuevo, mandar a que toque el otro btn
            if (usuarioPorNombre().es_nuevo != 0)
            {
                txtClave.Text = "";
                WindowsFormUtils.mensajeDeError("Debe completar el proceso de creación de usuario. Utilice el botón para el primer ingreso");
                return;
            }

            if (!usuarioPorNombre().habilitado)
            {
                txtClave.Text = "";
                WindowsFormUtils.mensajeDeError("Usuario inhabilitado. Por favor, contáctese con un administrador");
                return;
            }

            Usuario usuario = obtenerUsuarioDe(username, password);

            if (usuario == null)
            {
                using (RagnarEntities db = new RagnarEntities())
                {
                    db.SP_LoginDeUsuario(txtUsuario.Text, txtClave.Text);
                    txtClave.Text = "";
                    WindowsFormUtils.mensajeDeError("Verifique los datos ingresados y vuelva a intentarlo");
                }
                return;
            }
            else
            {
                loguearYPasarASeleccionarRol(usuario);
            }
        }

        private void loguearYPasarASeleccionarRol(Usuario usuario)
        {
            Global.loguearUsuario(usuario);

            SeleccionarRol formRoles = new SeleccionarRol();
            this.Hide();
        }

        private Usuario obtenerUsuarioDe(String usuario, String password) {
            return BaseDeDatos.BaseDeDatos.obtenerUsuarioPorCredenciales(usuario, password);   
        }

        private void btnRegistrarme_Click(object sender, EventArgs e)
        {
            this.Hide();
            WindowsFormUtils.abrirFormulario(new Usuarios.Signup(), () => { });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String username = txtUsuario.Text;

            if (username == "")
            {
                WindowsFormUtils.mensajeDeError("Debe ingresar el usuario asignado por el administrador");
                return;
            }

            using (RagnarEntities db = new RagnarEntities())
            {
                Usuario usuario = BaseDeDatos.BaseDeDatos.obtenerUsuarioPorNombre(db, txtUsuario.Text);

                if (usuario == null)
                {
                    WindowsFormUtils.mensajeDeError("El usuario ingresado no existe");
                    return;
                }
                
                int esNuevo = usuario.es_nuevo;
                if (esNuevo == 0)
                {
                    WindowsFormUtils.mensajeDeError("El usuario ya ha ingresado previamente en la plataforma. Utilice el botón 'Ingresar'");
                    return;
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
            }
        }

        private void finalizarSeteoDeContrasenia(RagnarEntities db, Usuario usuario)
        {
            if(PersonalizacionPass.personalizacionPassFinalizo)
            {
                usuario.es_nuevo = 0;
            }

            Utils.DBUtils.guardar(db);
        }

        private Usuario usuarioPorNombre()
        {
            return BaseDeDatos.BaseDeDatos.obtenerUsuarioPorNombre(new RagnarEntities(), txtUsuario.Text);
        }
    }
}
