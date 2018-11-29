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

namespace PalcoNet.Roles
{
    public partial class Listado : Form
    {
        public Listado()
        {
            InitializeComponent();

            dgvRoles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void Listado_Load(object sender, EventArgs e)
        {
            actualizarDataGriedView();
        }

        #region HELPER
        private void actualizarDataGriedView()
        {
            RagnarEntities db = new RagnarEntities();

            IQueryable<Rol> roles = db.Rol.AsQueryable();
            var rolesOrdenados = roles.Select(rol => new{
                id_rol = rol.id_rol,
                nombre = rol.nombre
            }).OrderBy(rol => rol.nombre);

            DataGridViewUtils.actualizarDataGriedView(dgvRoles, rolesOrdenados, "id_rol");
        }
        #endregion

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            WindowsFormUtils.abrirFormulario(new Formulario(), actualizarDataGriedView);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int? id = DataGridViewUtils.obtenerIdSeleccionado(dgvRoles);
            int idABuscar = id ?? default(int);
            WindowsFormUtils.abrirFormulario(new Formulario(idABuscar), actualizarDataGriedView);
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            WindowsFormUtils.volverALaHome(this);
        }
    }
}
