using PalcoNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet
{
    public static class Global
    {
        public static int idUsuario;
        public static TipoRol rolUsuario;

        public static void loguearUsuario(int id)
        {
            idUsuario = id;
        }
        public static void setearRolDeSesion(TipoRol rol)
        {
            rolUsuario = rol;
        }
    }
}
