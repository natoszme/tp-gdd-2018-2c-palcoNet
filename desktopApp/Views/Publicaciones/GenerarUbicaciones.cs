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

namespace PalcoNet.Views.Publicaciones
{
    public partial class GenerarUbicaciones : Form
    {

        List<Ubicacion> ubicaciones = new List<Ubicacion>();

        public GenerarUbicaciones()
        {
            InitializeComponent();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string filaIngresada = txtFila.Text;
            string asientoIngresado = txtAsiento.Text;
            if (ubicacionValida())
            {
                MessageBox.Show("Ubicacion creada con exito");
                
                dgvUbicaciones.Update();
                dgvUbicaciones.Refresh();
                
                
            }
        }

        bool ubicacionValida()
        {
          
            List<Control> inputs = new List<Control>();
            inputs.Add(txtFila);
            inputs.Add(txtAsiento);
            inputs.Add(txtPrecio);
            if (new Funciones_generales.Validaciones().todosNumericos(inputs))
            {
                if (new Funciones_generales.Validaciones().campoVacio(cboTipo))
                {
                    MessageBox.Show("Debe seleccionar un tipo de ubicacion");
                }
                else
                {
                    Ubicacion nuevaUbicacion = new Ubicacion(int.Parse(txtFila.Text), int.Parse(txtAsiento.Text), int.Parse(txtPrecio.Text), new Tipo_ubicacion(), cbxNumerada.Checked);
                    if (ubicaciones.Any(ubicacion => esMismaUbicacion(ubicacion, nuevaUbicacion)))
                    {
                        MessageBox.Show("Esa ubicacion ya esta ingresada");
                    }
                    else
                    {
                        ubicaciones.Add(nuevaUbicacion);
                        return true;
                    }
                   
                }
            }
            else
            {
                MessageBox.Show("Debe ingresar campos numericos");
            }
            /*if (new Funciones_generales.Validaciones().campoVacio(txtFila))
            {
                MessageBox.Show("Campo vacio");
            }
            else
            {
                MessageBox.Show("No vacio");
            }

            if (new Funciones_generales.Validaciones().esNumerico(txtFila))
            {
                MessageBox.Show("Es numerico");
            }
            else
            {
                MessageBox.Show("No es numerico");
            }*/


            return false ;
        }


        bool esMismaUbicacion(Ubicacion existente, Ubicacion nuevaUbicacion)
        {
            return (existente.fila == nuevaUbicacion.fila && existente.asiento == nuevaUbicacion.asiento);
        }

        private void GenerarUbicaciones_Load(object sender, EventArgs e)
        {
            dgvUbicaciones.DataSource = ubicaciones;
        }

     

        
        

     

    }
}
