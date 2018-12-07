using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Windows.Forms;

namespace PalcoNet.Utils
{
    class Paginador
    {
        /*
         * Se instancia en el loading del listado que lo vaya a actualizar con la cantidad de registros obtenidos en total
         * Se crean los eventos (onlick) de los 4 botones y se los llama dentro del form
         * Se pasa tambien el lbl para cambiar el numero de la pagina actual
         * Es importante usar el Skip y Take en la query donde se obtienen los registros, utilizando los metodos INIT y LIMIT
         * */

        private int pageSize { get; set; }
        private int totalRecords { get; set; }

        private int currentPage;
        private int totalPages { get; set;  }

        private Label lblCurrentPage;

        public Paginador(int pageSize, int totalRecords, Label lblCurrentPage) {
            this.pageSize = pageSize;
            this.totalRecords = totalRecords;
            this.currentPage = 1;

            this.totalPages = (int) Math.Ceiling((double) totalRecords / (double) pageSize);

            this.lblCurrentPage = lblCurrentPage;
        }

        public int CurrentPage
        {
            get { return currentPage; }
            set { 
                currentPage = value; 
                lblCurrentPage.Text = currentPage.ToString();  
            }
        }

        public void restart() {
            this.first();
        }

        public void first() {
            CurrentPage = 1;
        }

        public void prev() {
            CurrentPage = Math.Max(1, currentPage - 1);
        }

        public void next() {
            CurrentPage = Math.Min(totalPages, currentPage + 1);
        }

        public void last() {
            CurrentPage = totalPages;
        }

        public int init() {
            return (currentPage - 1) * pageSize;
        }

        public int limit() {
            return pageSize;
        }
    }
}
