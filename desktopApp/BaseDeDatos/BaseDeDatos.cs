using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PalcoNet.Model;
using PalcoNet.Utils;
using System.Windows.Forms;

namespace PalcoNet.BaseDeDatos
{
    class BaseDeDatos
    {
        private static RagnarEntities dbContext = new RagnarEntities();

        public static Usuario obtenerUsuarioPorCredenciales(String usuario, String password) {
            return dbContext.Usuario
                    .Where(user => user.usuario.Equals(usuario))
                    .Where(user => user.clave.Equals(F_HasheoDeClave(password)))
                    .FirstOrDefault();
        }

        public static bool tieneAlgunRol(Usuario usuario) {
            // TODO: fijarse si tiene algun rol
            return true;
        }

        public static List<string> obtenerRolesHabilitadosDelUsuario(Usuario usuario) {
            // TODO: ver si esta bien resuelto
            return usuario.Usuario_rol.Where(u_r => u_r.habilitado).Select(u_r => u_r.Rol.nombre).ToList();
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

        internal static Empresa empresaPorCuit(String cuit)
        {
            return dbContext.Empresa
                .Where(empresa => empresa.cuit.Equals(cuit))
                .FirstOrDefault();
        }

        internal static Rol obtenerRol(TipoRol nombreRol)
        {
            return dbContext.Rol
                .Where(rol => rol.nombre.Equals(nombreRol.DisplayName))
                .FirstOrDefault();
        }

        public static Usuario insertarYObtenerUsuario(String username, String pass, TipoRol tipoRol)
        {
            Usuario usuario = new Usuario();
            usuario.usuario = username;
            usuario.clave = pass;
            usuario.habilitado = true;

            Rol rol = obtenerRol(tipoRol);

            Usuario_rol usuario_rol = new Usuario_rol()
            {
                Usuario = usuario,
                Rol = rol
            };

            dbContext.Usuario.Add(usuario);
            dbContext.Usuario_rol.Add(usuario_rol);
            Utils.DBUtils.guardar(dbContext);

            return usuario;
        }

        internal static RagnarEntities finalDb(RagnarEntities db)
        {
            if (db == null) return dbContext;
            return db;
        }

        internal static Rol rolPorNombre(String nombreRol)
        {
            return dbContext.Rol
                .Where(rol => rol.nombre.Equals(nombreRol))
                .FirstOrDefault();
        }

        internal static Funcionalidad obtenerFuncionalidadPorDescripcion(RagnarEntities db, String descripcionFuncionalidad)
        {
            return db.Funcionalidad
               .Where(funcionalidad => funcionalidad.descripcion.Equals(descripcionFuncionalidad))
               .FirstOrDefault();
        }

        internal static Grado_publicacion gradoPorDescripcion(String nombreGrado)
        {
            return dbContext.Grado_publicacion
                .Where(grado => grado.descripcion.Equals(nombreGrado))
                .FirstOrDefault();
        }

        #region SQLFunctions
        // Sirve para usar la funcion generada desde SQL
        [System.Data.Entity.DbFunction("RagnarModel.Store", "F_HasheoDeClave")]
        public static string F_HasheoDeClave(string password)
        {
            throw new NotSupportedException("Direct calls are not supported.");
        }
        #endregion       
    
        internal static void modificarClave(Usuario usuario, string pass, Form formContext, Form destino = null)
        {
            modificarClave(dbContext, usuario, pass, formContext, destino);
        }

        internal static void modificarClave(RagnarEntities db, Usuario usuario, string pass, Form formContext, Form destino = null)
        {
            usuario.clave = pass;
            Utils.WindowsFormUtils.guardarYCerrar(db, formContext, destino);
        }
    }
}
