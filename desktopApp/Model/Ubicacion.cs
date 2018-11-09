using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Model
{
    class Ubicacion
    {
        public int fila{ get; set; }
        public int asiento { get; set; }
        public double precio { get; set; }
        public Tipo_ubicacion tipo = new Tipo_ubicacion();
        public bool esNumerada { get; set; }

        public Ubicacion(int fila, int asiento, double precio, Tipo_ubicacion tipo, bool esNumerada)
        {
            this.fila = fila;
            this.asiento = asiento;
            this.precio = precio;
            this.tipo = tipo;
            this.esNumerada = esNumerada;
        }

        

        
    }
}
