using PalcoNet.Model;
using PalcoNet.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalcoNet.Views.Publicaciones
{
    public partial class ComprarEntrada : Form
    {
        public ComprarEntrada()
        {
            InitializeComponent();
        }

        private void ComprarEntrada_Load(object sender, EventArgs e)
        {
            actualizarDataGriedView();
            dgvEspectaculos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            cargarClbCategorias();
        }


        #region HELPER
        private void actualizarDataGriedView()
        {
            using (RagnarEntities db = new RagnarEntities())
            {

                IQueryable<Publicacion> espectaculosTotales = db.Publicacion.AsQueryable();

             
               /* if(clbCategorias.CheckedItems.Count>0){
                    espectaculosTotales = espectaculosTotales.Where(e => clbCategorias.CheckedItems.Contains(e));
                }*/
                DateTime fechaDeHoy = Global.fechaDeHoy();
                espectaculosTotales = espectaculosTotales.Where(e => e.fecha_espectaculo >= fechaDeHoy && fechaDeHoy >= e.fecha_publicacion);

                if (!string.IsNullOrWhiteSpace(txtDescripcion.Text))
                {
                    espectaculosTotales = espectaculosTotales.Where(c => c.descripcion.Contains(txtDescripcion.Text));
                }
                if (cbFiltroFecha.Checked)
                {
                    espectaculosTotales = espectaculosTotales.Where(c => c.fecha_espectaculo >= dtpFechaDesde.Value.Date && c.fecha_espectaculo <=dtpFechaHasta.Value);
                   
                }

                /*
                if (!string.IsNullOrWhiteSpace(txtApellido.Text))
                {
                    clientesFiltrados = clientesFiltrados.Where(c => c.apellido.Contains(txtApellido.Text));
                }

                if (!string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    clientesFiltrados = clientesFiltrados.Where(c => c.mail.Contains(txtEmail.Text));
                }

                if (!string.IsNullOrWhiteSpace(txtTipoDocumento.Text))
                {
                    clientesFiltrados = clientesFiltrados.Where(c => c.tipo_documento.ToString() == txtTipoDocumento.Text);
                }

                if (!string.IsNullOrWhiteSpace(txtDocumento.Text))
                {
                    clientesFiltrados = clientesFiltrados.Where(c => c.numero_documento.ToString() == txtDocumento.Text);
                }*/
                espectaculosTotales = espectaculosTotales.Where(c => c.Estado_publicacion.descripcion.ToString() == "Publicada");
                var espectaculos = espectaculosTotales.Select(c => new
                {
                    id_publicacion = c.id_publicacion,
                    descripcion = c.descripcion,
                    direccion = c.direccion,
                    fecha_espectaculo = c.fecha_espectaculo,
                    fecha_publicacion = c.fecha_publicacion,
                    stock = c.stock,
                    rubro = c.Rubro.descripcion,
                    empresa = c.Empresa.razon_social,
                    visibilidad = c.Grado_publicacion.id_grado
                    
                }).OrderBy(c => c.visibilidad).ThenBy(c => c.fecha_espectaculo);

                DataGridViewUtils.actualizarDataGriedView(dgvEspectaculos, espectaculos, "id_publicacion");
            }
        }
     
        

        #endregion

        bool estaEnListaDeRubrosChecked(Publicacion publicacion)
        {
            foreach (String rubro in clbCategorias.CheckedItems)
            {
                if (publicacion.Rubro.descripcion == rubro)
                    return true;
            }
            return false;
        }

        void cargarClbCategorias()
        {
            clbCategorias.CheckOnClick = true; //Hace que se seleccione con 1 click en vez de 2
            using (RagnarEntities db = new RagnarEntities())
            {
                clbCategorias.Items.AddRange(descripcionesDeRubro(db.Rubro.ToList()).ToArray());
            }
        }
        List<String> descripcionesDeRubro(List<Rubro> rubros)
        {
            List<String> descripciones = new List<String>();
            foreach (Rubro rubro in rubros)
            {
                descripciones.Add(rubro.descripcion);
            }
            return descripciones;
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
          
            actualizarDataGriedView();
        }

        private void btnSeleccionarUbicaciones_Click(object sender, EventArgs e)
        {
            int? id = DataGridViewUtils.obtenerIdSeleccionado(dgvEspectaculos);
            if (id == null)
            {
                MessageBox.Show("Debe seleccionar algun espectaculo");
            }
            else
            {
                new SeleccionarUbicaciones(id).Show();
            }
            
        }

      


    }
}
