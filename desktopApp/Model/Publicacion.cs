//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Publicacion
    {
        public Publicacion()
        {
            this.Ubicacion_publicacion = new HashSet<Ubicacion_publicacion>();
        }
    
        public long id_publicacion { get; set; }
        public Nullable<decimal> codigo_publicacion { get; set; }
        public string descripcion { get; set; }
        public Nullable<int> stock { get; set; }
        public Nullable<System.DateTime> fecha_publicacion { get; set; }
        public int id_rubro { get; set; }
        public string direccion { get; set; }
        public Nullable<int> id_grado { get; set; }
        public Nullable<long> id_empresa { get; set; }
        public System.DateTime fecha_vencimiento { get; set; }
        public System.DateTime fecha_espectaculo { get; set; }
        public int id_estado { get; set; }
    
        public virtual Empresa Empresa { get; set; }
        public virtual Estado_publicacion Estado_publicacion { get; set; }
        public virtual Grado_publicacion Grado_publicacion { get; set; }
        public virtual Rubro Rubro { get; set; }
        public virtual ICollection<Ubicacion_publicacion> Ubicacion_publicacion { get; set; }
    }
}