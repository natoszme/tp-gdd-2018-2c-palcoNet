using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Windows.Forms;
using System.Globalization;

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
                inputs.ForEach(input => campoNumericoEntero(input, nombreInput));
            } catch (Exception) {
                throw new ValidationException("Todos los campos " + nombreInput + " deben ser numericos");
            }
        }

        public static void campoNumericoEntero(Control input, string nombreInput) {
            if (!input.Text.All(chr => char.IsDigit(chr)))
                throw new ValidationException("El campo " + nombreInput + " debe ser numerico");
        }

        public static decimal decimalDeInput(Control input)
        {
            String aParsear = input.Text.Replace('.', ',');
            return (decimal) Convert.ToDouble(aParsear);
        }

        public static decimal campoNumerico(Control input, string nombreInput)
        {
            decimal respuesta;
            try
            {
                respuesta = decimalDeInput(input);
            }
            catch (Exception)
            {
                throw new ValidationException("El campo " + nombreInput + " debe ser numerico");
            }
            return respuesta;
        }

        public static void emailValido(TextBox txtBox, string nombreInput) {
            try {
                new MailAddress(txtBox.Text.Trim());
            } catch (Exception) {
                throw new ValidationException("El email ingresado en el campo " + nombreInput + " es invalido");
            }
        }

        public static void campoPositivo(Control input, string nombreInput)
        {
            double value = Double.Parse(input.Text);
            if (value < 0)
            {
                throw new ValidationException("El campo " + nombreInput + " debe ser positivo");
            }
        }

        public static void campoEnteroYPositivo(Control input, string nombreInput) {
            campoNumericoEntero(input, nombreInput);

            campoPositivo(input, nombreInput);
            
        }

        public static void campoFloatYPositivo(Control input, string nombreInput)
        {
            decimal inputFloat = campoNumerico(input, nombreInput);

            campoPositivo(input, nombreInput);
        }

        public static void campoLongitudEntre(Control input, string nombreInput, int longitudMinima, int longitudMaxima)
        {
            if (!(input.Text.Length >= longitudMinima) || !(input.Text.Length <= longitudMaxima))
                throw new ValidationException("El campo " + nombreInput + " debe tener entre " + longitudMinima + " y " + longitudMaxima + " caracteres");
        }

        public static void campoLongitudFija(Control input, string nombreInput, int longitud)
        {
            if (!(input.Text.Length == longitud))
                throw new ValidationException("El campo " + nombreInput + " debe tener " + longitud + " caracteres");
        }

        public static void valorEntre(Control input, string nombreInput, int minimo, int maximo)
        {
            double valor = double.Parse(input.Text);
            if (valor < minimo || valor > maximo)
                throw new ValidationException("El campo " + nombreInput + " debe ser un numero entre " + minimo + " y " + maximo);
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
            if (!input.Text.All(chr => char.IsLetter(chr) || char.IsWhiteSpace(chr)))
                throw new ValidationException("El campo " + nombreInput + " sólo acepta letras");
        }

        public static void opcionObligatoria(ComboBox cmb, string nombreInput) {
            if (cmb.SelectedIndex == -1)
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

        public static void cuilOCuitValido(TextBox txtCuilOCuit, string nombreInput) {
            string cuil = txtCuilOCuit.Text;

            campoEnteroYPositivo(txtCuilOCuit, nombreInput);
            campoLongitudFija(txtCuilOCuit, nombreInput, 11);
            
            int calculado = CalcularDigitoCuil(cuil);
            int digito = int.Parse(cuil.Substring(10));
            if (calculado != digito) {
                throw new ValidationException("El " + nombreInput + " ingresado es invalido");
            }
        }

        public static void clavesCoincidentes(TextBox txtNuevaClave, TextBox txtRepetirClave) {
            if (txtNuevaClave.Text != txtRepetirClave.Text) {
                throw new ValidationException("La nueva clave ingresada, no coincide con la repetición");
            }
        }

        public static void fechaMenorAHoy(DateTimePicker datePicker, string nombreCampoFecha)
        {
            if (datePicker.Value.Date >= Global.fechaDeHoy()){
                throw new ValidationException("La fecha ingresada en " + nombreCampoFecha + " debe ser previa al dia de la fecha (" + Global.fechaDeHoy().ToShortDateString() + ")");
            }
        }

        public static void fechaMayorOIgualAHoy(DateTimePicker datePicker,TextBox txt, string nombreCampoFecha)
        {
            DateTime fechaIngresada = datePicker.Value.Date.AddHours(int.Parse(txt.Text.Substring(0, 2))).AddMinutes(int.Parse(txt.Text.Substring(3, 2)));
            if (fechaIngresada < Global.fechaDeHoy())
            {
                throw new ValidationException("La fecha ingresada en " + nombreCampoFecha + " no debe ser menor al dia de la fecha (" + Global.fechaDeHoy().ToShortDateString() + ")");
            }
            
        }

        public static void formatoHorarioValido(TextBox txtHorario, string nombreInput)
        {
            string horayMinuto = txtHorario.Text;

            
            campoLongitudFija(txtHorario, nombreInput, 5);
            if (horayMinuto.ElementAt(2) != ':')
            {
                throw new ValidationException("La " + nombreInput + " ingresada es invalida");
            }
            else
            {
                int hora;
                int minuto;
                try
                {
                    hora = int.Parse(horayMinuto.Substring(0, 2));
                    minuto= int.Parse(horayMinuto.Substring(3, 2));
                }
                catch (Exception)
                {
                     throw new ValidationException("La " + nombreInput + " ingresada es invalida");
                }
                if(hora<0 || hora>23 || minuto<0 || minuto>59)
                    throw new ValidationException("La " + nombreInput + " ingresada es invalida");
            }
        }


        public static bool hayError(Action validacion, ref List<string> errores)
        {
            try
            {
                validacion();
            }
            catch (ValidationException e)
            {
                errores.Add(e.Message);
                return true;
            }

            return false;
        }
    }
}
