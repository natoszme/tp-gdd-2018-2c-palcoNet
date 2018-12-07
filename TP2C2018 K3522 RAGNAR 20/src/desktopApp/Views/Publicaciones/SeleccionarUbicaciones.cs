﻿using PalcoNet.Model;
using PalcoNet.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalcoNet.Views.Publicaciones
{
    public partial class SeleccionarUbicaciones : Form
    {
        List<int> ubicacionesSeleccionadas = new List<int>();
        int idEspectaculo;

        private String caracteresOcultosTarjeta = "****";
        private int digitosBaseTarjeta = 6;
        private int digitosFinalTarjeta = 4;

        public SeleccionarUbicaciones(int? id)
        {
            InitializeComponent();
            this.idEspectaculo = (int)id;
        }

        private void SeleccionarUbicaciones_Load(object sender, EventArgs e)
        {
            actualizarLabelCantidad();
            actualizarDataGriedView();
            cargarTarjetaCredito();
            dgvUbicaciones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }


        #region HELPER
        private void actualizarDataGriedView()
        {
            using (RagnarEntities db = new RagnarEntities())
            {

                IQueryable<Ubicacion_publicacion> ubicacionesFiltradas = db.Ubicacion_publicacion.AsQueryable().Where(u => u.id_publicacion == idEspectaculo);

                ubicacionesFiltradas = ubicacionesFiltradas.Where(ub => ub.Compra == null);


                var ubicaciones = ubicacionesFiltradas.Select(c => new
                {
                    id_ubicacion = c.id_ubicacion,
                    precio = c.precio,
                    fila = c.fila,
                    asiento = c.asiento,
                    sin_numerar = c.sin_numerar,
                    tipo_asiento = c.Tipo_ubicacion.descripcion
                });

                DataGridViewUtils.actualizarDataGriedView(dgvUbicaciones, ubicaciones, "id_ubicacion");
            }
        }
        #endregion

        void actualizarLabelCantidad()
        {
            lblUbicacionesSeleccionadas.Text = "Cantidad de ubicaciones seleccionadas = " + ubicacionesSeleccionadas.Count;
        }

        #region TARJETACREDITO
        void cargarTarjetaCredito()
        {
            Cliente clienteActual = BaseDeDatos.BaseDeDatos.clientePorId(Global.obtenerUsuarioLogueado().id_usuario);
            if (clienteActual != null)
            {
                txtTarjeta.Text = tarjetaConAsteriscos(clienteActual.tarjeta_credito);
            }
            else
            {
                 MessageBox.Show("No puede comprar porque no esta registrado como cliente");
            }
                 
           
        }

        
        private string recortarTarjetaDeCredito(string tarjeta)
        {
            if (tarjeta != "")
            {
                return tarjeta.Substring(0, digitosBaseTarjeta) + tarjeta.Substring(tarjeta.Length - digitosFinalTarjeta, digitosFinalTarjeta);
            }
            return null;
        }

        private string tarjetaConAsteriscos(string tarjeta)
        {
            if (tarjeta != null)
            {
                return tarjeta.Substring(0, digitosBaseTarjeta) + caracteresOcultosTarjeta + tarjeta.Substring(tarjeta.Length - digitosFinalTarjeta, digitosFinalTarjeta);
            }

            return "";
        }

        private bool validarTarjeta()
        {
            bool valido = true;
            List<string> errores = new List<string>();
            if(!ValidationsUtils.hayError(() =>ValidationsUtils.campoObligatorio(txtTarjeta, "tarjeta de credito"),ref errores))
            {
                ValidationsUtils.hayError(() => ValidationsUtils.campoLongitudEntre(txtTarjeta, "tarjeta de credito", 15, 16), ref errores);
                ValidationsUtils.hayError(() => ValidationsUtils.campoEnteroYPositivo(txtTarjeta, "tarjeta de credito"), ref errores);
           }
           
           
            if (errores.Count() > 0)
            {
                WindowsFormUtils.mostrarErrores(errores);
                valido = false;
            }
            else
            {
                valido = true;
            }

            return valido;
        }
        #endregion

        private void btnSeleccionar_Click(object sender, EventArgs e)
        {
          
            int? id = DataGridViewUtils.obtenerIdSeleccionado(dgvUbicaciones);
            if (id == null)
            {
                MessageBox.Show("Debe seleccionar una ubicacion");
            }
            else
            {
                if (ubicacionesSeleccionadas.Contains((int)id))
                {
                    MessageBox.Show("Esa ubicacion ya fue seleccionada");
                }
                else
                {
                    ubicacionesSeleccionadas.Add((int)id);
                    actualizarLabelCantidad();
                }

            }

            
           
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        private void btnComprar_Click(object sender, EventArgs e)
        {
            if (validarTarjeta())
            {
                if (importeTotal() > 0)
                {
                    using (RagnarEntities db = new RagnarEntities())
                    {
                        asignarEntidades(db);
                        WindowsFormUtils.mensajeDeExito("Muchas gracias por realizar la compra. Que disfrute el espectáculo!");
                        WindowsFormUtils.guardarYCerrar(db, this);
                    }
                }
                else
                {
                    MessageBox.Show("El importe total no puede ser menor a 0");
                }
               
            }
        }

        decimal importeTotal()
        {
            decimal importe = 0;
            RagnarEntities db = new RagnarEntities();
            foreach (int idUbicacion in ubicacionesSeleccionadas)
            {
                
                IQueryable<Ubicacion_publicacion> ubicacionActual = db.Ubicacion_publicacion.AsQueryable().Where(ub => ub.id_ubicacion == idUbicacion);
                importe += ubicacionActual.FirstOrDefault().precio;
            }
            return importe;
        }

        protected void asignarEntidades(RagnarEntities db)
        {
           
            
            Compra compra = new Compra();
            compra.Cliente = BaseDeDatos.BaseDeDatos.clientePorId(db,Global.obtenerUsuarioLogueado().id_usuario);
            compra.fecha = Global.fechaDeHoy();
            compra.tarjeta_utilizada = recortarTarjetaDeCredito(txtTarjeta.Text);
            
            
            Ubicacion_publicacion ubicacionActual;
            foreach (int idUbicacion in ubicacionesSeleccionadas)
            {
                //IQueryable<Ubicacion_publicacion> ubicacionActual = db.Ubicacion_publicacion.AsQueryable().Where(ub => ub.id_ubicacion == idUbicacion);
                ubicacionActual  = new Ubicacion_publicacion();
                 ubicacionActual= db.Ubicacion_publicacion.Find(idUbicacion);
                compra.Ubicacion_publicacion.Add(ubicacionActual);
                
               // ubicacionActual.Compra = compra;
               // ubicacionActual.FirstOrDefault().Compra = compra;
            }
            db.Compra.Add(compra);
            asignarPuntos(db, compra.Cliente);

        }
        #region PUNTOS

        private void asignarPuntos(RagnarEntities db, Cliente cliente)
        {
            Puntos_cliente puntos = new Puntos_cliente();
            puntos.Cliente = cliente;
            puntos.puntos = 50;
            puntos.vencimiento = Global.fechaDeHoy().AddYears(1);
            db.Puntos_cliente.Add(puntos);
        }

        #endregion 
    }
}
