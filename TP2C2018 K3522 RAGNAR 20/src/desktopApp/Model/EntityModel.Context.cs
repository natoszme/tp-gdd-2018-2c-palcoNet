﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PalcoNet.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;

    public partial class RagnarEntities : DbContext
    {
        public RagnarEntities()
            : base(Utils.ConfigReader.getInstance().connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Compra> Compra { get; set; }
        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Estado_publicacion> Estado_publicacion { get; set; }
        public DbSet<Factura> Factura { get; set; }
        public DbSet<Funcionalidad> Funcionalidad { get; set; }
        public DbSet<Grado_publicacion> Grado_publicacion { get; set; }
        public DbSet<Item_factura> Item_factura { get; set; }
        public DbSet<Login_fallido> Login_fallido { get; set; }
        public DbSet<Premio> Premio { get; set; }
        public DbSet<Publicacion> Publicacion { get; set; }
        public DbSet<Puntos_cliente> Puntos_cliente { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Rubro> Rubro { get; set; }
        public DbSet<Tipo_ubicacion> Tipo_ubicacion { get; set; }
        public DbSet<Ubicacion_publicacion> Ubicacion_publicacion { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Usuario_rol> Usuario_rol { get; set; }

        [DbFunction("RagnarEntities", "F_ClientesConMasCompras")]
        public virtual IQueryable<F_ClientesConMasCompras_Result> F_ClientesConMasCompras(Nullable<System.DateTime> fecha)
        {
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("Fecha", fecha) :
                new ObjectParameter("Fecha", typeof(System.DateTime));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<F_ClientesConMasCompras_Result>("[RagnarEntities].[F_ClientesConMasCompras](@Fecha)", fechaParameter);
        }

        [DbFunction("RagnarEntities", "F_ClientesConMasPuntosVencidos")]
        public virtual IQueryable<F_ClientesConMasPuntosVencidos_Result> F_ClientesConMasPuntosVencidos(Nullable<System.DateTime> fecha)
        {
            var fechaParameter = fecha.HasValue ?
                new ObjectParameter("Fecha", fecha) :
                new ObjectParameter("Fecha", typeof(System.DateTime));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<F_ClientesConMasPuntosVencidos_Result>("[RagnarEntities].[F_ClientesConMasPuntosVencidos](@Fecha)", fechaParameter);
        }

        [DbFunction("RagnarEntities", "F_HistorialDeCliente")]
        public virtual IQueryable<F_HistorialDeCliente_Result> F_HistorialDeCliente(Nullable<long> cliente)
        {
            var clienteParameter = cliente.HasValue ?
                new ObjectParameter("Cliente", cliente) :
                new ObjectParameter("Cliente", typeof(long));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<F_HistorialDeCliente_Result>("[RagnarEntities].[F_HistorialDeCliente](@Cliente)", clienteParameter);
        }

        [DbFunction("RagnarEntities", "F_EmpresasConMasLocalidadesNoVencidas")]
        public virtual IQueryable<F_EmpresasConMasLocalidadesNoVencidas_Result> F_EmpresasConMasLocalidadesNoVencidas(Nullable<int> id_grado, string mes, string anio)
        {
            var id_gradoParameter = id_grado.HasValue ?
                new ObjectParameter("Id_grado", id_grado) :
                new ObjectParameter("Id_grado", typeof(int));

            var mesParameter = mes != null ?
                new ObjectParameter("Mes", mes) :
                new ObjectParameter("Mes", typeof(string));

            var anioParameter = anio != null ?
                new ObjectParameter("Anio", anio) :
                new ObjectParameter("Anio", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.CreateQuery<F_EmpresasConMasLocalidadesNoVencidas_Result>("[RagnarEntities].[F_EmpresasConMasLocalidadesNoVencidas](@Id_grado, @Mes, @Anio)", id_gradoParameter, mesParameter, anioParameter);
        }

        public virtual int SP_LoginDeUsuario(string usuario, string clave)
        {
            var usuarioParameter = usuario != null ?
                new ObjectParameter("Usuario", usuario) :
                new ObjectParameter("Usuario", typeof(string));

            var claveParameter = clave != null ?
                new ObjectParameter("Clave", clave) :
                new ObjectParameter("Clave", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_LoginDeUsuario", usuarioParameter, claveParameter);
        }

        public virtual int SP_RendicionDeComisiones(Nullable<int> cantidadAFacturar, Nullable<System.DateTime> fechaDelSistema)
        {
            var cantidadAFacturarParameter = cantidadAFacturar.HasValue ?
                new ObjectParameter("CantidadAFacturar", cantidadAFacturar) :
                new ObjectParameter("CantidadAFacturar", typeof(int));

            var fechaDelSistemaParameter = fechaDelSistema.HasValue ?
                new ObjectParameter("FechaDelSistema", fechaDelSistema) :
                new ObjectParameter("FechaDelSistema", typeof(System.DateTime));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_RendicionDeComisiones", cantidadAFacturarParameter, fechaDelSistemaParameter);
        }
    }
}