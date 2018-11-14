using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Views.Usuarios
{
    public static class UsuariosUtils
    {
        public static long idAAsignar(String nombre, String apellido)
        {
            long idAAsignar = 0;
            if (Global.hayIdGenerado())
            {
                idAAsignar = Global.idUsuarioGenerado;
                Global.limpiarIdGenerado();
            }
            else
            {
                idAAsignar = generarUsuarioConPassRandom(nombre, apellido);
            }
            return idAAsignar;
        }

        public static long generarUsuarioConPassRandom(String nombre, String apellido)
        {
            return insertarUsuarioYObtenerId(nombre + apellido, "default");
        }

        public static void guardarUsuario(String username, String pass)
        {
            long idGenerado = idGenerado = insertarUsuarioYObtenerId(username, pass);
            Global.idUsuarioGenerado = idGenerado;
        }

        private static long insertarUsuarioYObtenerId(String username, String pass)
        {
            //TODO terminar
            //BaseDeDatos.insertarUsuario(username, pass);
            return 1;
        }
    }
}
