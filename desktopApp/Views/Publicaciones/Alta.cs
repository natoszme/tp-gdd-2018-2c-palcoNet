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

namespace PalcoNet.Publicaciones
{
    public partial class Alta : Form
    {
        Views.Publicaciones.GenerarUbicaciones formUbicaciones;
        public Alta()
        {
            InitializeComponent();
        }

        private void btnUbicaciones_Click(object sender, EventArgs e)
        {
            if(formUbicaciones == null)
                 formUbicaciones = new Views.Publicaciones.GenerarUbicaciones();
            formUbicaciones.Show();
        }

        private void btnGuardarBorrador_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Cantidad de ubicaciones: " + formUbicaciones.ubicaciones.Count);
        }

       
    }
}
