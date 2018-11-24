﻿using System;
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
            Utils.DBUtils.guardar(dbContext);

            return usuario;
        }

        internal static RagnarEntities finalDb(RagnarEntities db)
        {
            if (db == null) return dbContext;
            return db;
        }

        internal static void guardarCliente(RagnarEntities db)
        {
            Utils.DBUtils.guardar(db);
        }

        internal static Rol rolPorNombre(String nombreRol)
        {
            return dbContext.Rol
                .Where(rol => rol.nombre.Equals(nombreRol))
                .FirstOrDefault();
        }

        /*internal static object obtenerFuncionalidades()
        {
            return dbContext.Funcionalidad.All();
            var clientes = clientesFiltrados.Select(c => new
            {
                id_usuario = c.id_usuario,
                nombre = c.nombre,
                apellido = c.apellido,
                tipo_documento = c.tipo_documento,
                numero_documento = c.numero_documento,
                mail = c.mail
            }).OrderBy(c => c.nombre).ThenBy(c => c.apellido);
        }*/
    }
}
