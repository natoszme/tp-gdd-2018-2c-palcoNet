using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalcoNet.Usuarios
{
    public partial class ModificarClaveUsuario : Form
    {
        public ModificarClaveUsuario()
        {
            InitializeComponent();
        }

        private void btnCambiar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Home().Show();
        }
    }
}
