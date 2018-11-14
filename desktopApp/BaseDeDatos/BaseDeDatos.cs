using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.BaseDeDatos
{
    class BaseDeDatos
    {
        public static int obtenerIdPorUsuarioPass(String usuario, String password)
        {
            //Debe traer el id de la base de datos que tiene ese usuario, -1 en caso de no existir
            return 1;
        }

        public static bool tieneAlgunRol(int idUsuario)
        {
            //Debe obtener si existe algun rol para ese id
            return true;
        }

        public static List<String> obtenerRolesPorIdEnTexto(int id)
        {
            List<String> roles = new List<string>();
            //TODO debe traer los roles de la db del usuario
            roles.Add("Cliente");
            roles.Add("Administrativo");
            roles.Add("Empresa");
            return roles;
        }

        public static bool existeUsuario(String username)
        {
            //TODO fijarse si existe un usuario con este username, en ese caso devolver true
            return false;
        }

    }
}
