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

namespace PalcoNet.Roles
{
    public partial class Modificacion : Form
    {
        int? id;
        Rol cliente = new Rol();

        public Modificacion(int? id = null)
        {
            this.id = id;
            InitializeComponent();

            if (id != null)
            {
                // cargarDatos();
            }
        }
    }
}
