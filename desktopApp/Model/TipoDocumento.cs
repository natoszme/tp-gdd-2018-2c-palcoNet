using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Model
{
    class TipoDocumento : Headspring.Enumeration<TipoDocumento, int> {
        public static readonly TipoDocumento DNI = new TipoDocumento(1, "DNI");
        public static readonly TipoDocumento LC = new TipoDocumento(2, "LC");
        public static readonly TipoDocumento LE = new TipoDocumento(3, "LE");

        private TipoDocumento(int value, string displayName) : base(value, displayName) {}
    }
}
