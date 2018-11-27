using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PalcoNet.Model;

namespace PalcoNet.Views.Reportes
{
    class Reportero
    {
        /* REPORTES
         * 0: Empresas con mas no vendidas
         * 1: Clientes con mas puntos vencidos
         * 2: Clientes con mas compras
         */

        public static IQueryable<Object> getReporte(RagnarEntities dbContext, int anio, int trimestre, int tipo) {
            int mes = getMes(trimestre, tipo);
            DateTime fecha = new DateTime(anio, mes, DateTime.DaysInMonth(anio, mes));
            switch (tipo) {
                case 0:
                    // dbContext.F_EmpresasConMasLocalidadesNoVencidas(grado??, trimestre, anio);
                    return null;

                case 1:
                    return dbContext.F_ClientesConMasPuntosVencidos(fecha).AsQueryable<F_ClientesConMasPuntosVencidos_Result>();

                case 2:
                    return dbContext.F_ClientesConMasCompras(fecha).AsQueryable<F_ClientesConMasCompras_Result>();
                
                default:
                    return null;
            }
        }

        private static int getMes(int trimestreOmes, int tipo) {
            switch (tipo) {
                case 0:
                    return trimestreOmes;

                case 1:
                case 2:
                default:
                    return (trimestreOmes * 3) - 2;
            }
        }
    }
}
