using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.BaseDeDatos
{
    class BaseDeDatos
    {
        public int obtenerIdPorUsuarioPass(String usuario, String password)
        {
            //Debe traer el id de la base de datos que tiene ese usuario, -1 en caso de no existir
            return 1;
        }

        public bool tieneAlgunRol(int idUsuario)
        {
            //Debe obtener si existe algun rol para ese id
            return true;
        }

        public List<String> obtenerRolesPorIdEnTexto(int id)
        {
            List<String> roles = new List<string>();
            //TODO debe traer los roles de la db del usuario
            roles.Add("Cliente");
            roles.Add("Administrativo");
            roles.Add("Empresa");
            return roles;
        }

    }
}
