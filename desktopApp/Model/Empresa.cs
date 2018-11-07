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
    
    public partial class Empresa
    {
        public Empresa()
        {
            this.Compra = new HashSet<Compra>();
            this.Publicacion = new HashSet<Publicacion>();
        }
    
        public long id_usuario { get; set; }
        public string razon_social { get; set; }
        public string cuit { get; set; }
        public string mail { get; set; }
        public string telefono { get; set; }
        public string ciudad { get; set; }
        public string calle { get; set; }
        public decimal portal { get; set; }
        public decimal piso { get; set; }
        public string departamento { get; set; }
        public string localidad { get; set; }
        public string codigo_postal { get; set; }
        public System.DateTime fecha_creacion { get; set; }
    
        public virtual ICollection<Compra> Compra { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<Publicacion> Publicacion { get; set; }
    }
}
