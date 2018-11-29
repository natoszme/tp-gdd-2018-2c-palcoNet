using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalcoNet.Utils
{
    class DataGridViewUtils
    {

        public static int? obtenerIdSeleccionado(DataGridView dgv) {
            try
            {
                if (!haySeleccionado(dgv)) return obtenerIdDe(dgv, 0);
                return obtenerIdDe(dgv, dgv.CurrentRow.Index);
            }
            catch
            {
                return null;
            }
        }

        public static int obtenerIdDe(DataGridView dgv, int fila)
        {
            return int.Parse(dgv.Rows[fila].Cells[0].Value.ToString());
        }

        public static Boolean haySeleccionado(DataGridView dgv)
        {
            return dgv.CurrentRow != null;
        }

        public static void actualizarDataGriedView(DataGridView dgv, IQueryable<Object> query, string idOculto = null)
        {
            if (query != null)
                dgv.DataSource = query.ToList();

            if (dgv.Rows.Count > 0 && !string.IsNullOrWhiteSpace(idOculto) && dgv.Columns[idOculto] != null) {
                dgv.Columns[idOculto].Visible = false;
            }
        }
    }
}
