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
    
    public partial class Usuario
    {
        public Usuario()
        {
            this.Usuario_rol = new HashSet<Usuario_rol>();
        }
    
        public long id_usuario { get; set; }
        public string usuario { get; set; }
        public string clave { get; set; }
        public bool habilitado { get; set; }
        public byte es_nuevo { get; set; }
    
        public virtual Cliente Cliente { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual Login_fallido Login_fallido { get; set; }
        public virtual ICollection<Usuario_rol> Usuario_rol { get; set; }
    }
}
