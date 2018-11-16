using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Utils
{
    class ComboBoxItem {
        public int value { get; set; }
        public string text { get; set; }
        
        public ComboBoxItem(int value, string text) {
            this.value = value;
            this.text = text;
        }
    }
}
