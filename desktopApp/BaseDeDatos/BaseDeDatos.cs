using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PalcoNet.Model;
using System.Data.Entity.Validation;

namespace PalcoNet.BaseDeDatos
{
    class BaseDeDatos
    {
        private static RagnarEntities dbContext = new RagnarEntities();

        public static void guardar()
        {
            try
            {
                dbContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                            ve.PropertyName,
                            eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName),
                            ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

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

        internal static Cliente clientePorCuil(String cuil)
        {
            return dbContext.Cliente
                .Where(cliente => cliente.cuil.Equals(cuil))
                .FirstOrDefault();
        }

        internal static Rol obtenerRol(TipoRol nombreRol)
        {
            return dbContext.Rol
                .Where(rol => rol.nombre.Equals(nombreRol.DisplayName))
                .FirstOrDefault();
        }

        public static Usuario insertarYObtenerUsuario(String username, String pass, TipoRol rol)
        {
            Usuario usuario = new Usuario();
            usuario.usuario = username;
            usuario.clave = pass;
            usuario.habilitado = true;
            usuario.Rol.Add(obtenerRol(rol));           

            dbContext.Usuario.Add(usuario);

            return usuario;
        }

        internal static void guardarCliente(Cliente cliente)
        {
            dbContext.Cliente.Add(cliente);
        }

        internal static void actualizarCliente(Cliente cliente)
        {
            dbContext.Entry(cliente.Usuario).State = System.Data.Entity.EntityState.Modified;
            dbContext.Entry(cliente).State = System.Data.Entity.EntityState.Modified;
        }
    }
}
