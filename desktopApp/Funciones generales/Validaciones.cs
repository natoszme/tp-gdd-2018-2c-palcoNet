using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalcoNet.Funciones_generales
{
    public class Validaciones
    {

        public bool todosNumericos(List<Control> inputs)
        {
            if (inputs.All(esNumerico))
            {
                return true;
            }
            return false;
        }

        public bool esNumerico(Control input)
        {
            try
            {
                int.Parse(input.Text);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool campoVacio(Control input)
        {
            if (input.Text == "")
                return true;
            return false;
        }

        public bool tieneNulos(Control input)
        {
            if (input.Text == null)
                return true;
            return false;
   
        }
    }
}
