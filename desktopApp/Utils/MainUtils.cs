using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;

namespace PalcoNet.Utils
{
    class MainUtils
    {
        public static void abrirFormulario(Form frm, Action callback) {
            frm.ShowDialog();
            callback();
        }

        public static void volverALaHome(Control actualContext) {
            actualContext.Hide();
            new Home().Show();
        }
    }
}
