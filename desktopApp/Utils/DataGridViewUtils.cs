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
                return int.Parse(dgv.Rows[dgv.CurrentRow.Index].Cells[0].Value.ToString());
            }
            catch
            {
                return null;
            }
        }

        public static void actualizarDataGriedView(DataGridView dgv, IQueryable<Object> query, string idOculto = null)
        {
            dgv.DataSource = query.ToList();
            if (dgv.Rows.Count > 0 && !string.IsNullOrWhiteSpace(idOculto) && dgv.Columns[idOculto] != null) {
                dgv.Columns[idOculto].Visible = false;
            }
        }
    }
}
