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
    
    public partial class Tipo_ubicacion
    {
        public Tipo_ubicacion()
        {
            this.Ubicacion_publicacion = new HashSet<Ubicacion_publicacion>();
        }
    
        public int id_tipo_ubicacion { get; set; }
        public Nullable<decimal> codigo { get; set; }
        public string descripcion { get; set; }
    
        public virtual ICollection<Ubicacion_publicacion> Ubicacion_publicacion { get; set; }
    }
}
