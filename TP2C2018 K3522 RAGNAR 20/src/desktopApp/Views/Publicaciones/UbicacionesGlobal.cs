using PalcoNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Views.Publicaciones
{
    public static class UbicacionesGlobal {
        public static RagnarEntities contextoGlobal = new RagnarEntities();
        public static List<Ubicacion_publicacion> ubicaciones = new List<Ubicacion_publicacion>();
    }
}
