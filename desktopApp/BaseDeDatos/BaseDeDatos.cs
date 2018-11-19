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
                    .Where(user => user.usuario.Equals(usuario))
                    .Where(user => user.clave.Equals(password))
                    .FirstOrDefault();
        }

        public static bool tieneAlgunRol(Usuario usuario) {
            // TODO: fijarse si tiene algun rol
            return true;
        }

        //TODO esto esta harcodeado, no deberia
        public static List<String> obtenerRolesDelUsuario(Usuario usuario) {
            List<String> roles = new List<string>();
            roles.Add("Cliente");
            roles.Add("Administrativo");
            roles.Add("Empresa");
            return roles;
        }

        public static bool existeUsuario(String username) {
            return dbContext.Usuario.Any(user => user.usuario.Equals(username));
        }

        public static Cliente clientePorDocumento(String tipoDoc, String numeroDoc)
        {
            decimal nroDoc = decimal.Parse(numeroDoc);
            return dbContext.Cliente
                .Where(cliente => cliente.tipo_documento.Equals(tipoDoc) && cliente.numero_documento.Equals(nroDoc))
                .FirstOrDefault();
        }
    }
}
