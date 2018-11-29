using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Model
{
    public class TipoRol : Headspring.Enumeration<TipoRol, int> {
        public static readonly TipoRol EMPRESA = new TipoRol(0, "Empresa");
        public static readonly TipoRol ADMINISTRATIVO = new TipoRol(1, "Administrativo");
        public static readonly TipoRol CLIENTE = new TipoRol(2, "Cliente");

        private TipoRol(int value, string displayName) : base(value, displayName) {}
    } 
}
