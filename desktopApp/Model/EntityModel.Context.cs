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
    
    public partial class RagnarEntities : DbContext
    {
        public RagnarEntities()
            : base("name=RagnarEntities")
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
    }
}
