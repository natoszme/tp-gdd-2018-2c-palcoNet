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
                generarUsuarioRandom(username, usuario, rol);

            return db.Usuario.Find(Global.obtenerYLimpiarUsuarioGenerado().id_usuario);
        }

        public static void generarUsuarioRandom(String username, Usuario usuario, TipoRol rol) {
            guardarUsuario(username, username, rol);
        }

        #region Generar credenciales
        public static String generarUsername(Cliente cliente) {
            return cliente.tipo_documento + cliente.numero_documento;
        }

        public static String generarUsername(Empresa empresa) {
            return empresa.cuit.ToString();
        }
        #endregion

        public static void guardarUsuario(String username, String pass, TipoRol rol)
        {
            Global.usuarioGenerado = BaseDeDatos.BaseDeDatos.insertarYObtenerUsuario(username, pass, rol);            
        }

        public static void guardarUsuarioYSetearLogueado(String username, String pass, TipoRol rol)
        {
            guardarUsuario(username, pass, rol);
            Global.loguearUsuario(Global.usuarioGenerado);
            Global.setearRol(BaseDeDatos.BaseDeDatos.obtenerRol(new RagnarEntities(), rol));
        }
    }
}
