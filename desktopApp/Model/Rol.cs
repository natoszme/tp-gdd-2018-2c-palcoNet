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
    
    public partial class Rol
    {
        public Rol()
        {
            this.Funcionalidad = new HashSet<Funcionalidad>();
            this.Usuario = new HashSet<Usuario>();
        }
    
        public int id_rol { get; set; }
        public string nombre { get; set; }
        public bool habilitado { get; set; }
    
        public virtual ICollection<Funcionalidad> Funcionalidad { get; set; }
        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
