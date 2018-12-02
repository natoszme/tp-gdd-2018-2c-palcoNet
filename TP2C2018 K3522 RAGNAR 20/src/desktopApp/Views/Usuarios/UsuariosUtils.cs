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
            //este usuario es nuevo, haciendo referencia a que van a tener que cambiar la pass
            guardarUsuario(username, username, rol, 1);
        }

        #region Generar credenciales
        public static String generarUsername(Cliente cliente) {
            return cliente.tipo_documento + cliente.numero_documento;
        }

        public static String generarUsername(Empresa empresa) {
            return empresa.cuit.ToString();
        }
        #endregion

        public static void guardarUsuario(String username, String pass, TipoRol rol, int esNuevo)
        {
            Global.usuarioGenerado = BaseDeDatos.BaseDeDatos.insertarYObtenerUsuario(username, pass, rol, esNuevo);            
        }

        public static void guardarUsuarioYSetearLogueado(String username, String pass, TipoRol rol)
        {
            //este usuario no es nuevo: es el propio usuario y no el admin el que lo genera
            guardarUsuario(username, pass, rol, 0);
            Global.loguearUsuario(Global.usuarioGenerado);
            Global.setearRol(BaseDeDatos.BaseDeDatos.obtenerRol(new RagnarEntities(), rol));
        }

        //lo usamos para eliminar al usuario recientemente ingresado, en el caso de que vuelva para atras mientras cargaba los datos de cliente/empresa
        public static void deshacerCreacionDeUsuarioRegistrado(RagnarEntities db)
        {
            BaseDeDatos.BaseDeDatos.eliminarUsuario(db, Global.usuarioGenerado);
            Global.desloguearUsuario();
            Global.limpiarUsuarioGenerado();
        }
    }
}
