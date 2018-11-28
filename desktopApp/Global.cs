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
        public static Usuario usuarioLogueado {get; set;}
        public static Rol rolUsuario { get; set; }
        
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
    }
}
