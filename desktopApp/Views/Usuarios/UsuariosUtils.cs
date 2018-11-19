using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PalcoNet.Model;

namespace PalcoNet.Views.Usuarios
{
    public static class UsuariosUtils
    {
        public static RagnarEntities dbContext { get; set; }

        public static Usuario usuarioAAsignar(String username, Usuario usuario) {

            if (!Global.hayUsuarioGenerado())
                return generarUsuarioRandom(username, usuario);

            return Global.obtenerYLimpiarUsuarioGenerado();
        }

        public static Usuario generarUsuarioRandom(String username, Usuario usuario) {
            return insertarYObtenerUsuario(username, generarPassword(usuario), usuario.Rol.First());
        }

        #region Generar credenciales
        public static String generarUsername(Cliente cliente) {
            return cliente.tipo_documento + cliente.numero_documento;
        }

        public static String generarUsername(Empresa empresa) {
            return empresa.cuit.ToString();
        }

        private static String generarPassword(Usuario usuario) {
            return usuario.id_usuario.ToString() + DateTime.Now.ToString();
        }
        #endregion

        public static void guardarUsuario(String username, String pass, Rol rol) {
            Global.usuarioGenerado = insertarYObtenerUsuario(username, pass, rol);
        }

        private static Usuario insertarYObtenerUsuario(String username, String pass, Rol rol) {
            Usuario usuario = new Usuario();
            usuario.usuario = username;
            usuario.clave = pass;
            usuario.habilitado = true;
            usuario.Rol.Add(rol);
            
            if (dbContext == null) {
                dbContext = new RagnarEntities();
            }
            dbContext.Usuario.Add(usuario);

            return usuario;
        }
    }
}
