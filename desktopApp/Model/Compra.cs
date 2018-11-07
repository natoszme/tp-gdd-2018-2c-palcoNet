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
    
    public partial class Compra
    {
        public Compra()
        {
            this.Item_factura = new HashSet<Item_factura>();
            this.Ubicacion_publicacion = new HashSet<Ubicacion_publicacion>();
        }
    
        public long id_compra { get; set; }
        public long id_cliente { get; set; }
        public long id_empresa { get; set; }
        public System.DateTime fecha { get; set; }
        public string tarjeta_utilizada { get; set; }
    
        public virtual Cliente Cliente { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual ICollection<Item_factura> Item_factura { get; set; }
        public virtual ICollection<Ubicacion_publicacion> Ubicacion_publicacion { get; set; }
    }
}