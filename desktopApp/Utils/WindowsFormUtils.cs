using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using PalcoNet.Model;

namespace PalcoNet.Utils
{
    class WindowsFormUtils
    {
        public static void abrirFormulario(Form frm, Action callback) {
            // TODO: se podria mejorar pasando la clase por parametro, y creando una instancia aca
            // Se podria fijar que en caso de estar seleccionada una row de una dgv, abrir para editar
            frm.ShowDialog();
            callback();
        }

        public static void guardarYCerrar(RagnarEntities db, Form context, Form destino = null) {
            DBUtils.guardar(db);
            context.Close();
            if (destino != null) destino.Show();
        }

        public static void mensajeDeError(string error) {
            MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void volverALaHome(Control context) {
            context.Hide();
            new Home().Show();
        }

        public static String seleccionadoDe(ComboBox combo)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)combo.SelectedItem;
            return selectedItem.text;
        }
    }
}
