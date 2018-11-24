using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PalcoNet.Model;
using PalcoNet.Utils;

namespace PalcoNet.Usuarios
{
    public partial class ModificarClaveAdmin : Form
    {
        int id;
        public ModificarClaveAdmin(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void btnCambiar_Click(object sender, EventArgs e)
        {

        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            WindowsFormUtils.volverALaHome(this);
        }
    }
}
