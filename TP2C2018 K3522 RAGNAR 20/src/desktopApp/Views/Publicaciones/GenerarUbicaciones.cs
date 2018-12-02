﻿using System;
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
using PalcoNet.Publicaciones;

namespace PalcoNet.Views.Publicaciones
{

    public partial class GenerarUbicaciones : Form
    {

        public List<Ubicacion> ubicaciones;
        Label lblCantUbicaciones;
        public GenerarUbicaciones(Label lblUbicaciones)
        {
           /* Ubicacion nuevaUbicacion = new Ubicacion(1, 1, 10, new Tipo_ubicacion(), true);
            ubicaciones.Add(nuevaUbicacion);
            dgvUbicaciones.DataSource = ubicaciones;*/
            InitializeComponent();
            cargarComboTipo();
            ubicaciones = new List<Ubicacion>();
            lblCantUbicaciones = lblUbicaciones;
            
        }

    
        private void cargarComboTipo()
        {
            using (RagnarEntities db = new RagnarEntities())
            {
                cboTipo.DataSource = (from g in db.Tipo_ubicacion select g.descripcion).ToList();
            }
        }

        private List<String> obtenerTextoDeTipoUbicacion( List<Tipo_ubicacion> tipos)
        {
            List<String> aux = new List<String>();
            
            foreach (Tipo_ubicacion element in tipos)
            {
                aux.Add(element.descripcion);
            }
            return aux;
        }

        private void btnAgregar_Click(object sender, EventArgs e) {
            string filaIngresada = txtFila.Text;
            string asientoIngresado = txtAsiento.Text;
            if (ubicacionValida()) {
                MessageBox.Show("Ubicacion creada con exito");
                dgvUbicaciones.DataSource = ubicaciones;
            }
        }

        private bool ubicacionValida() {
          
            List<Control> inputs = new List<Control>();
            inputs.Add(txtFila);
            inputs.Add(txtAsiento);
            inputs.Add(txtPrecio);

            try {
                ValidationsUtils.camposNumericos(inputs, "fila, asiento y precio");
                ValidationsUtils.opcionObligatoria(cboTipo, "tipo de ubicacion");
            } catch (ValidationException e) {
                WindowsFormUtils.mensajeDeError(e.Message);
                return false;
            }
            
            Ubicacion nuevaUbicacion = new Ubicacion(int.Parse(txtFila.Text), int.Parse(txtAsiento.Text), int.Parse(txtPrecio.Text), new Tipo_ubicacion(), cbxNumerada.Checked);
            if (ubicaciones.Any(ubicacion => esMismaUbicacion(ubicacion, nuevaUbicacion))) {
                WindowsFormUtils.mensajeDeError("Esa ubicacion ya esta ingresada");
            } else {
                ubicaciones.Add(nuevaUbicacion);
                lblCantUbicaciones.Text = "Ubicaciones cargadas = " + ubicaciones.Count;
                return true;
            }

            /*
             * TODO: usar las funciones del utils
             * if (new ValidationsUtils.campoVacio(txtFila))
            {
                MessageBox.Show("Campo vacio");
            }
            else
            {
                MessageBox.Show("No vacio");
            }

            if (new ValidationsUtils.esNumerico(txtFila))
            {
                MessageBox.Show("Es numerico");
            }
            else
            {
                MessageBox.Show("No es numerico");
            }*/


            return false ;
        }


        private bool esMismaUbicacion(Ubicacion existente, Ubicacion nuevaUbicacion) {
            return (existente.fila == nuevaUbicacion.fila && existente.asiento == nuevaUbicacion.asiento);
        }

        private void GenerarUbicaciones_Load(object sender, EventArgs e) {
            dgvUbicaciones.DataSource = ubicaciones;
        }

        private void btnFinalizar_Click(object sender, EventArgs e)
        {

            this.Hide();
        }
    }
}