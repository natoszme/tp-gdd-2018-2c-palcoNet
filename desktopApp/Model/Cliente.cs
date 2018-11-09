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
    
    public partial class Cliente : Usuario
    {
        public Cliente()
        {
            this.Compra = new HashSet<Compra>();
            this.Puntos_cliente = new HashSet<Puntos_cliente>();
        }
    
        public long id_usuario { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string tipo_documento { get; set; }
        public decimal numero_documento { get; set; }
        public string cuil { get; set; }
        public string mail { get; set; }
        public string telefono { get; set; }
        public string calle { get; set; }
        public decimal portal { get; set; }
        public decimal piso { get; set; }
        public string departamento { get; set; }
        public string localidad { get; set; }
        public string codigo_postal { get; set; }
        public System.DateTime fecha_nacimiento { get; set; }
        public Nullable<System.DateTime> fecha_creacion { get; set; }
        public string tarjeta_credito { get; set; }
    
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<Compra> Compra { get; set; }
        public virtual ICollection<Puntos_cliente> Puntos_cliente { get; set; }
    }
}
