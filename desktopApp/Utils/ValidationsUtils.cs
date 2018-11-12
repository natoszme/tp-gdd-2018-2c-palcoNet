using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
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
            if (!input.Text.All(chr => char.IsDigit(chr)))
                throw new ValidationException("El campo " + nombreInput + " debe ser numerico");
        }

        public static void emailValido(TextBox txtBox, string nombreInput) {
            try {
                new MailAddress(txtBox.Text.Trim());
            } catch (Exception) {
                throw new ValidationException("El email ingresado en el campo " + nombreInput + " es invalido");
            }
        }

        public static void campoNumericoYPositivo(Control input, string nombreInput) {
            campoNumerico(input, nombreInput);

            double value = Double.Parse(input.Text);
            if (value < 0) {
                throw new ValidationException("El campo " + nombreInput + " debe ser positivo");
            }
        }

        public static void campoLongitudMaxima(Control input, string nombreInput, int longitudMinima, int longitudMaxima)
        {
            if (!(input.Text.Length >= longitudMinima) || !(input.Text.Length <= longitudMaxima))
                throw new ValidationException("El campo " + nombreInput + " debe tener entre " + longitudMinima + " y " + longitudMaxima + " caracteres");
        }

        public static void campoLongitudFija(Control input, string nombreInput, int longitud)
        {
            if (!(input.Text.Length == longitud))
                throw new ValidationException("El campo " + nombreInput + " debe tener " + longitud + " caracteres");
        }

        public static void campoObligatorio(DateTimePicker dtp, string nombreInput) {
            if (dtp.Value == null)
                throw new ValidationException("El campo " + nombreInput + " es obligatorio");
        }

        public static void campoObligatorio(Control input, string nombreInput) {
            if (string.IsNullOrWhiteSpace(input.Text) || string.IsNullOrEmpty(input.Text))
                throw new ValidationException("El campo " + nombreInput + " es obligatorio");
        }

        public static void campoAlfabetico(Control input, string nombreInput)
        {
            if (!input.Text.All(chr => char.IsLetter(chr)))
                throw new ValidationException("El campo " + nombreInput + " sólo acepta letras");
        }

        public static void opcionObligatoria(ComboBox cmb, string nombreInput) {
            if (cmb.SelectedIndex == -1 || string.IsNullOrWhiteSpace(cmb.SelectedText))
                throw new ValidationException("Debe seleccionar un " + nombreInput);
        }

        public static int CalcularDigitoCuil(string cuil) {
            int[] multiplicadores = new[] { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
            char[] nums = cuil.ToCharArray();

            // Mutiplico cada multiplicador por el digito de la posicion correspondiente y sumo todo
            int total = multiplicadores.Select((mult, i) => int.Parse(nums[i].ToString()) * mult).Sum();

            var resto = total % 11;
            return resto == 0 ? 0 : resto == 1 ? 9 : 11 - resto;
        }

        public static void cuilValido(TextBox txtCuil) {
            string cuil = txtCuil.Text;

            campoNumericoYPositivo(txtCuil, "CUIL");
            campoLongitudFija(txtCuil, "CUIL", 11);
            
            int calculado = CalcularDigitoCuil(cuil);
            int digito = int.Parse(cuil.Substring(10));
            if (calculado != digito) {
                throw new ValidationException("El CUIL ingresado es invalido");
            }
        }
    }
}
