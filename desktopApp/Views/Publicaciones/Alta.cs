using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalcoNet.Publicaciones
{
    public partial class Alta : Form
    {
        public Alta()
        {
            InitializeComponent();
        }

        private void btnUbicaciones_Click(object sender, EventArgs e)
        {
            new Views.Publicaciones.GenerarUbicaciones().Show();
        }

       
    }
}
