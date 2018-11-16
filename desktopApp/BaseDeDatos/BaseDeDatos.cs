using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PalcoNet.Model;

namespace PalcoNet.BaseDeDatos
{
    class BaseDeDatos
    {
        private static RagnarEntities dbContext = new RagnarEntities();

        public static Usuario obtenerUsuarioPorCredenciales(String usuario, String password) {
            return dbContext.Usuario
                    .Where(user => user.usuario == usuario)
                    .Where(user => user.clave == password)
                    .FirstOrDefault();
        }

        public static bool tieneAlgunRol(Usuario usuario) {
            // TODO: fijarse si tiene algun rol
            return true;
        }

        public static List<String> obtenerRolesDelUsuario(Usuario usuario) {
            List<String> roles = new List<string>();
            // TODO: esto se deberia poder resolver con Usuario.Roles, no es necesario llamar a BaseDeDatos
            roles.Add("Cliente");
            roles.Add("Administrativo");
            roles.Add("Empresa");
            return roles;
        }

        public static bool existeUsuario(String username) {
            return dbContext.Usuario.Count(user => user.usuario == username) > 0;
        }
    }
}
