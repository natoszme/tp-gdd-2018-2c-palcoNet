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
        Paginador paginador;
        private bool loadingTime = true;

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
             
                if(clbCategorias.CheckedItems.Count > 0){
                    espectaculosTotales = espectaculosTotales.ToList().Where(e => clbCategorias.CheckedItems.Contains(e.Rubro.descripcion)).AsQueryable();
                }

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

                
                espectaculosTotales = espectaculosTotales.Where(c => c.Estado_publicacion.descripcion.ToString() == "Publicada");
                espectaculosTotales = espectaculosTotales.Where(c => c.stock >= 0).OrderByDescending(c => c.Grado_publicacion.comision).ThenBy(c => c.fecha_espectaculo);
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
                    grado = c.Grado_publicacion.descripcion
                });

                if (loadingTime) {
                    paginador = new Paginador(10, espectaculos.Count(), lblPaginaActual, new List<Button> { btnPrimera, btnAnterior, btnSiguiente, btnUltima });
                    loadingTime = false;
                } else {
                    paginador.TotalRecords = espectaculos.Count();
                }

                DataGridViewUtils.actualizarDataGriedView(dgvEspectaculos, espectaculos.Skip(paginador.init()).Take(paginador.limit()), "id_publicacion");
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
            paginador.restart();
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
                WindowsFormUtils.abrirFormulario(new SeleccionarUbicaciones(id), actualizarDataGriedView);
            }
            
        }

      


        private void btnLimpiar_Click(object sender, EventArgs e) {

            txtDescripcion.Text = "";
            cbFiltroFecha.Checked = false;
            
            // Hay un metodo nativo "clearSelected" que se supone que lo hace, pero no funca
            foreach (int i in clbCategorias.CheckedIndices)
                clbCategorias.SetItemCheckState(i, CheckState.Unchecked);

            paginador.restart();
            actualizarDataGriedView();
        }

        #region PAGINADO
        private void btnPrimera_Click_1(object sender, EventArgs e)
        {
            paginador.first();
            actualizarDataGriedView();
        }

        private void btnAnterior_Click_1(object sender, EventArgs e)
        {
            paginador.prev();
            actualizarDataGriedView();
        }

        private void btnSiguiente_Click_1(object sender, EventArgs e)
        {
            paginador.next();
            actualizarDataGriedView();
        }

        private void btnUltima_Click_1(object sender, EventArgs e)
        {
            paginador.last();
            actualizarDataGriedView();
        }
        #endregion

        private void btnVolver_Click(object sender, EventArgs e)
        {
            WindowsFormUtils.volverALaHome(this);
        }

        
    }
}
