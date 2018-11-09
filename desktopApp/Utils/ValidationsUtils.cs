using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalcoNet.Utils
{
    #region EXCEPTION
    public class ValidationException : Exception {
        public ValidationException() {}
        public ValidationException(string message) : base(message) {}
        public ValidationException(string message, Exception inner) : base(message, inner) {}
    }
    #endregion

    public class ValidationsUtils
    {
        public static void camposNumericos(List<Control> inputs, string nombreInput) {
            try {
                inputs.ForEach(input => campoNumerico(input, nombreInput));
            } catch (Exception) {
                throw new ValidationException("Todos los campos " + nombreInput + " deben ser numericos");
            }
        }

        public static void campoNumerico(Control input, string nombreInput) {
            try {
                Double.Parse(input.Text);
            } catch (Exception) {
                throw new ValidationException("El campo " + nombreInput + " debe ser numerico");
            }
        }

        public static void campoNumericoYPositivo(Control input, string nombreInput) {
            campoNumerico(input, nombreInput);

            int value = int.Parse(input.Text);
            if (value < 0) {
                throw new ValidationException("El campo " + nombreInput + " debe ser mayor a 0");
            }
        }

        public static void campoObligatorio(DateTimePicker dtp, string nombreInput) {
            if (dtp.Value == null)
                throw new ValidationException("El campo " + nombreInput + " es obligatorio");
        }

        public static void campoObligatorio(Control input, string nombreInput) {
            if (string.IsNullOrWhiteSpace(input.Text) || string.IsNullOrEmpty(input.Text))
                throw new ValidationException("El campo " + nombreInput + " es obligatorio");
        }

        public static void opcionObligatoria(ComboBox cmb, string nombreInput) {
            if (cmb.SelectedIndex == -1 || string.IsNullOrWhiteSpace(cmb.SelectedText))
                throw new ValidationException("Debe seleccionar un " + nombreInput);
        }
    }
}
