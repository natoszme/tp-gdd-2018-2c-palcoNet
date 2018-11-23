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
        public static Usuario usuarioAAsignar(RagnarEntities db, String username, Usuario usuario, TipoRol rol) {

            if (!Global.hayUsuarioGenerado())
                return generarUsuarioRandom(db, username, usuario, rol);

            return Global.obtenerYLimpiarUsuarioGenerado();
        }

        public static Usuario generarUsuarioRandom(RagnarEntities db, String username, Usuario usuario, TipoRol rol) {
            return BaseDeDatos.BaseDeDatos.insertarYObtenerUsuario(db, username, generarPassword(usuario), rol);
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

        public static void guardarUsuario(String username, String pass, TipoRol rol)
        {
            Global.usuarioGenerado = BaseDeDatos.BaseDeDatos.insertarYObtenerUsuario(null, username, pass, rol);
        }
    }
}
