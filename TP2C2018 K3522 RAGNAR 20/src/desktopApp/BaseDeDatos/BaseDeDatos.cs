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
        private static RagnarEntities dbContext()
        {
            return new RagnarEntities();
        }

        public static Usuario obtenerUsuarioPorCredenciales(RagnarEntities db, String usuario, String password) {
            return db.Usuario
                    .Where(user => user.usuario.Equals(usuario))
                    .Where(user => user.clave.Equals(F_HasheoDeClave(password)))
                    .FirstOrDefault();
        }

        public static List<string> obtenerRolesHabilitadosDelUsuario(Usuario usuario) {
            return usuario.Usuario_rol.Where(u_r => u_r.habilitado).Select(u_r => u_r.Rol.nombre).ToList();
        }

        public static bool existeUsuario(String username) {
            return dbContext().Usuario.Any(user => user.usuario.Equals(username));
        }

        public static Cliente clientePorDocumento(String tipoDoc, String numeroDoc)
        {
            decimal nroDoc = decimal.Parse(numeroDoc);
            return dbContext().Cliente
                .Where(cliente => cliente.tipo_documento.Equals(tipoDoc) && cliente.numero_documento.Equals(nroDoc))
                .FirstOrDefault();
        }

        internal static Cliente clientePorCuil(String cuil)
        {
            return dbContext().Cliente
                .Where(cliente => cliente.cuil.Equals(cuil))
                .FirstOrDefault();
        }

        internal static List<Ubicacion_publicacion> ubicacionesPorIdPublicacion(Publicacion publicacion)
        {
            return dbContext().Ubicacion_publicacion
                .Where(ubicacion => ubicacion.id_publicacion.Equals(publicacion.id_publicacion)).ToList();
        }

        internal static Empresa empresaPorCuit(String cuit)
        {
            return dbContext().Empresa
                .Where(empresa => empresa.cuit.Equals(cuit))
                .FirstOrDefault();
        }

        internal static Empresa empresaPorId(int id)
        {
            return dbContext().Empresa
                .Where(empresa => empresa.id_usuario.Equals(id))
                .FirstOrDefault();
        }

        internal static Rol obtenerRol(RagnarEntities db, TipoRol nombreRol)
        {
            return db.Rol
                .Where(rol => rol.nombre.Equals(nombreRol.DisplayName))
                .FirstOrDefault();
        }

        public static Usuario insertarYObtenerUsuario(String username, String pass, TipoRol tipoRol, int esNuevo)
        {
            Usuario usuario = new Usuario();
            usuario.usuario = username;
            usuario.clave = pass;
            usuario.habilitado = true;
            usuario.es_nuevo = (byte) esNuevo;

            RagnarEntities db = dbContext();

            Rol rol = obtenerRol(db, tipoRol);

            Usuario_rol usuario_rol = new Usuario_rol()
            {
                Usuario = usuario,
                Rol = rol,
                habilitado = true
            };            

            db.Usuario.Add(usuario);
            db.Usuario_rol.Add(usuario_rol);
            Utils.DBUtils.guardar(db);

            return usuario;
        }

        internal static RagnarEntities finalDb(RagnarEntities db)
        {
            if (db == null) return dbContext();
            return db;
        }

        internal static Rol rolPorNombre(String nombreRol)
        {
            return dbContext().Rol
                .Where(rol => rol.nombre.Equals(nombreRol))
                .FirstOrDefault();
        }

        internal static Tipo_ubicacion tipoUbicacionPorDescripcion(String descripcion)
        {
            return dbContext().Tipo_ubicacion
                .Where(tipo => tipo.descripcion.Equals(descripcion))
                .FirstOrDefault();
        }

        internal static Publicacion publicacionPorId(int id)
        {
            return dbContext().Publicacion
                .Where(publicacion => publicacion.id_publicacion.Equals(id))
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
            return dbContext().Grado_publicacion
                .Where(grado => grado.descripcion.Equals(nombreGrado))
                .FirstOrDefault();
        }

        internal static Rubro rubroPorDescripcion(String descripcionRubro)
        {
            return dbContext().Rubro
                .Where(rubro => rubro.descripcion.Equals(descripcionRubro))
                .FirstOrDefault();
        }

        internal static Estado_publicacion estadoDePublicacionPorNombre(String nombreEstado)
        {
            return dbContext().Estado_publicacion
                .Where(estado => estado.descripcion.Equals(nombreEstado))
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
            modificarClave(dbContext(), usuario, pass, formContext, destino);
        }

        internal static void modificarClave(RagnarEntities db, Usuario usuario, string pass, Form formContext, Form destino = null)
        {
            db.Usuario.Find(usuario.id_usuario).clave = pass;
            Utils.WindowsFormUtils.guardarYCerrar(db, formContext, destino);
        }

        internal static List<Funcionalidad> obtenerFuncionalidades()
        {
            return dbContext().Funcionalidad.ToList();
        }

        public static Rol obtenerRol(Rol rol)
        {
            return obtenerRol(dbContext(), rol);
        }

        public static Rol obtenerRol(RagnarEntities db, Rol rol)
        {
            return db.Rol.Find(rol.id_rol);
        }

        public static Usuario obtenerUsuario(Usuario usuarioLogueado)
        {
            return obtenerUsuario(dbContext(), usuarioLogueado);
        }

        internal static Usuario obtenerUsuario(RagnarEntities db, Usuario usuarioLogueado)
        {
            return db.Usuario.Find(usuarioLogueado.id_usuario);
        }

        public static Estado_publicacion obtenerEstadoDePublicacionPorId(int id)
        {
            return obtenerEstadoDePublicacionPorId(dbContext(), id);
        }

        internal static Estado_publicacion obtenerEstadoDePublicacionPorId(RagnarEntities db, int id)
        {
            return db.Publicacion.Where(publicacion => publicacion.id_publicacion.Equals(id))
                .FirstOrDefault().Estado_publicacion;
        }

        public static void eliminarUsuario(RagnarEntities db, Usuario usuario)
        {
            var rolesUsuario = db.Usuario_rol.Where(u_r => u_r.id_usuario == usuario.id_usuario).ToList();
            rolesUsuario.ForEach(rol => db.Usuario_rol.Remove(rol));
            db.Usuario.Remove(db.Usuario.Find(usuario.id_usuario));
        }

        public static bool existePublicacionEnMismaFecha(Publicacion publicacion)
        {
            return existePublicacionEnMismaFecha(dbContext(), publicacion);
        }

        public static bool existePublicacionEnMismaFecha(RagnarEntities db, Publicacion publicacion)
        {
            List<Publicacion> publicacionIgual = db.Publicacion.Where(u_r => u_r.direccion == publicacion.direccion && u_r.fecha_espectaculo == publicacion.fecha_espectaculo).ToList();
            return publicacionIgual.Count > 0;
        }

        public static Usuario obtenerUsuarioPorNombre(RagnarEntities db, String nombreUsuario)
        {
            return db.Usuario.Where(usuario => usuario.usuario.Equals(nombreUsuario))
                .FirstOrDefault();
        }

        public static int obtenerPuntosNoVencidosDe(Cliente cliente)
        {
            return cliente.Puntos_cliente
                .Where(puntaje => puntaje.vencimiento >= Global.fechaDeHoy())
                .Select(puntaje => puntaje.puntos).ToList().Sum();
        }

        public static void clienteCompraPremio(RagnarEntities db, Cliente cliente, Premio premio)
        {
            //TODO compra el premio. santi esta haciendo trigger para que al hacer insert sobre Canje_premio, se resten los puntos
            //cliente.Premio.Add(premio);
        }

        internal static Empresa obtenerEmpresaPorId(RagnarEntities db, int idEmpresa)
        {
            return db.Empresa.Find(idEmpresa);
        }
    }
}
