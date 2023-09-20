using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrabajoPracticoFinal
{
    public partial class Inicio : Form
    {
        public Inicio()
        {
            InitializeComponent();
        }

        private void btnVenta_Click(object sender, EventArgs e)
        {
            Venta venta = new Venta();
            this.Hide();
            venta.ShowDialog();
            this.Show();
        }

        private void btnCompra_Click(object sender, EventArgs e)
        {
            Compra compra = new Compra();
            this.Hide();
            compra.ShowDialog();
            this.Show();
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            Clientes clientes = new Clientes();
            this.Hide();
            clientes.ShowDialog();
            this.Show();
        }

        private void btnProveedores_Click(object sender, EventArgs e)
        {
            Proveedores proveedores = new Proveedores();
            this.Hide();
            proveedores.ShowDialog();
            this.Show();
        }

        private void btnStock_Click(object sender, EventArgs e)
        {
            Almacen stock = new Almacen();
            this.Hide();
            stock.ShowDialog();
            this.Show();
        }
    }
}
