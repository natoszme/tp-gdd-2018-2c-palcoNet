using PalcoNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet
{
    public static class Global
    {
        private static Usuario usuarioLogueado {get; set;}
        private static Rol rolUsuario { get; set; }
        
        public static Usuario usuarioGenerado { get; set; }

        public static void loguearUsuario(Usuario usuario) {
            usuarioLogueado = usuario;
        }

        public static Usuario obtenerYLimpiarUsuarioGenerado() {
            Usuario usrGenerado = usuarioGenerado;
            limpiarUsuarioGenerado();
            return usrGenerado;
        }

        public static void limpiarUsuarioGenerado() {
            usuarioGenerado = null;
        }

        public static bool hayUsuarioGenerado() {
            return usuarioGenerado != null;
        }

        public static DateTime fechaDeHoy()
        {
            return Utils.ConfigReader.getInstance().Fecha;
        }

        internal static void desloguearUsuario()
        {
            usuarioLogueado = null;
        }

        public static Rol obtenerRolUsuario(RagnarEntities db)
        {
            return db.Rol.Find(rolUsuario.id_rol);
        }

        public static Rol obtenerRolUsuario()
        {
            return BaseDeDatos.BaseDeDatos.obtenerRol(rolUsuario);
        }

        internal static void setearRol(Rol rol)
        {
            rolUsuario = rol;
        }

        public static Usuario obtenerUsuarioLogueado()
        {
            return BaseDeDatos.BaseDeDatos.obtenerUsuario(usuarioLogueado);
        }

        //TODO reutilizar la logica para no repetir esto tambien en BaseDeDatos
        public static Usuario obtenerUsuarioLogueado(RagnarEntities db)
        {
            return db.Usuario.Find(usuarioLogueado);
        }
    }
}
