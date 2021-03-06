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

namespace PalcoNet.Views.RendicionComisiones
{
    public partial class Listado : Form
    {
        public Listado()
        {
            InitializeComponent();
            dgvItemsFacturas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            actualizarDataGridView(new RagnarEntities());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (camposValidos())
            {
                using (RagnarEntities db = new RagnarEntities())
                {
                    db.SP_RendicionDeComisiones(int.Parse(txtComprasARendir.Text), Global.fechaDeHoy());
                    actualizarDataGridView(db);
                    WindowsFormUtils.mensajeDeExito("Se han rendido las comisiones");
                }
            }
        }

        private bool camposValidos()
        {
            bool camposValidos = true;
            try
            {
                ValidationsUtils.campoObligatorio(txtComprasARendir, "compras a rendir");
                ValidationsUtils.campoEnteroYPositivo(txtComprasARendir, "compras a rendir");
                if (int.Parse(txtComprasARendir.Text) == 0)
                {
                    throw new ValidationException("La cantidad de compras a rendir debe ser mayor a 0");
                }
            }
            catch (ValidationException e)
            {
                WindowsFormUtils.mensajeDeError(e.Message);
                camposValidos = false;
            }
            return camposValidos;
        }

        private void actualizarDataGridView(RagnarEntities db)
        {
            IQueryable<Item_factura> itemsFacturas = db.Item_factura.AsQueryable();

            var facturasFiltradas =
                itemsFacturas
                .Select(itemFactura => new
                {
                    id_item_factura = itemFactura.id_item,
                    numeroDeFactura = itemFactura.Factura.numero,
                    fechaFactura = itemFactura.Factura.fecha,
                    descripcion = itemFactura.descripcion,
                    precioUbicacion = itemFactura.Ubicacion_publicacion.precio,                    
                    comision = itemFactura.Ubicacion_publicacion.precio * itemFactura.Ubicacion_publicacion.Publicacion.Grado_publicacion.comision
                })
                .OrderByDescending(itemFactura => itemFactura.numeroDeFactura)
                .ThenBy(itemFactura => itemFactura.fechaFactura);

            DataGridViewUtils.actualizarDataGriedView(dgvItemsFacturas, facturasFiltradas, "id_item_factura");          
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            WindowsFormUtils.volverALaHome(this);
        }

        private void Listado_Load(object sender, EventArgs e)
        {

        }
    }
}
