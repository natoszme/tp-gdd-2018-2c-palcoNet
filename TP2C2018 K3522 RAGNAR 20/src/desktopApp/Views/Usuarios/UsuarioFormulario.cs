using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PalcoNet.Utils;
using PalcoNet.Model;
using PalcoNet.Usuarios;

namespace PalcoNet.Views.Usuarios
{
    [TypeDescriptionProvider(typeof(DescriptionProvider<UsuarioFormulario, Form>))]
    public abstract partial class UsuarioFormulario : Form
    {
        protected int? id;

        public UsuarioFormulario(int? id = null)
        {
            InitializeComponent();
            this.ControlBox = false;

            this.id = id;
        }

        private void UsuarioFormulario_Load(object sender, EventArgs e)
        {

        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            using (RagnarEntities db = new RagnarEntities())
            {
                if (Global.hayUsuarioGenerado() && !editando())
                {
                    UsuariosUtils.deshacerCreacionDeUsuarioRegistrado(db);
                    WindowsFormUtils.guardarYCerrar(db, this);
                }
                else
                {
                    this.Hide();
                }
                
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            using (RagnarEntities db = new RagnarEntities())
            {
                if (camposYDominioValidos())
                {
                    asignarEntidades(db);
                    WindowsFormUtils.guardarYCerrar(db, this);
                }
            }
        }

        protected void btnCambiarPass_Click(object sender, EventArgs e)
        {
            int idUsuarioAEditarPass = id ?? default(int);
            new ModificarClaveAdmin(idUsuarioAEditarPass).ShowDialog();
        }

        public bool editando()
        {
            return id != null;
        }

        protected bool hayQueMostrarPanelAdmin()
        {
            return esAdminEditando();
        }

        protected bool esAdminEditando()
        {
            //ya que el unico que puede editar, es el admin
            return editando();
        }

        protected abstract void asignarEntidades(RagnarEntities db);
        protected abstract bool camposYDominioValidos();
        protected abstract bool validarDominio(ref List<string> errores);
        protected abstract void mostrarPanelAdmin();
        protected abstract void cargarDatos();
        protected abstract TextBox textBoxCui();
    }
}
