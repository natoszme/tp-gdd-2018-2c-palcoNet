using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using PalcoNet.Model;
using PalcoNet.Views.Usuarios;

namespace PalcoNet.Utils
{
    class WindowsFormUtils
    {
        public static void abrirFormulario(Form frm, Action callback) {
            // TODO: se podria mejorar pasando la clase por parametro, y creando una instancia aca
            // Se podria fijar que en caso de estar seleccionada una row de una dgv, abrir para editar
            frm.ShowDialog();
            callback();
        }

        public static void guardarYCerrar(RagnarEntities db, Form context, Form destino = null) {
            DBUtils.guardar(db);
            cerrar(context, destino);
        }

        public static void guardarYCerrar(Form context, Form destino = null) {
            DBUtils.guardar();
            cerrar(context, destino);
        }

        private static void cerrar(Form context, Form destino = null) {
            context.Close();
            if (destino != null) destino.Show();
        }

        public static void mensajeDeError(string error) {
            MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void mostrarErrores(List<string> errores) {
            errores = errores.Select(error => "- " + error).ToList();
            string erroresAMostrar = String.Join("\n", errores);
            mensajeDeError(erroresAMostrar);
        }

        public static void volverALaHome(Control context) {
            context.Hide();
            new Home().Show();
        }

        public static void volverALogin(Control context)
        {
            context.Hide();
            new Usuarios.Login().Show();
        }

        public static int numeroSeleccionadoDe(ComboBox combo)
        {
            ComboBoxItem selectedItem = (ComboBoxItem) combo.SelectedItem;
            return selectedItem.value;
        }

        public static String textoSeleccionadoDe(ComboBox combo)
        {
            return combo.GetItemText(combo.SelectedItem);
        }
    }
}
