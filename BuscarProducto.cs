using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrabajoPracticoFinal
{
    public partial class BuscarProducto : Form
    {
        private Consultas consultas;
        int globalAction = 0;

        // 0 es venta
        // 1 es compra
        public BuscarProducto(int action)
        {
            InitializeComponent();
            globalAction = action;
            consultas = new Consultas();
            if (globalAction == 0)
            {
                consultas.LlenarGridProductosConRecargo(dataGridView1);
            }
            if (globalAction == 1)
            {
                consultas.LlenarGridProductos(dataGridView1);
            }
            AjustarGrid();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (globalAction == 0)
            {
                consultas.EscribirProductosConRecargo(dataGridView1, txtBuscar.Text);
            }
            if (globalAction == 1)
            {
                consultas.EscribirProductos(dataGridView1, txtBuscar.Text);
            }
        }
        public void AjustarGrid()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns[0].HeaderText = "CODIGO";
            dataGridView1.Columns[1].HeaderText = "PRODUCTO";
            dataGridView1.Columns[2].HeaderText = "PRECIO";

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        public int cliente = -1;

        private void txtBuscar_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    dataGridView1.Focus();
                    if (dataGridView1.Rows.Count > 1)
                    {
                        DataGridViewCell primercelda = dataGridView1.Rows[1].Cells[0];
                        dataGridView1.CurrentCell = primercelda;
                    }
                }
                if (e.KeyCode == Keys.Enter)
                {
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        DataGridViewRow row = dataGridView1.SelectedRows[0];
                        cliente = Convert.ToInt32(row.Cells[0].Value);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        DataGridViewRow row = dataGridView1.SelectedRows[0];
                        cliente = Convert.ToInt32(row.Cells[0].Value);
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BuscarProducto_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (globalAction == 0)
            {
                Venta formularioVenta = Application.OpenForms.OfType<Venta>().FirstOrDefault();

                if (formularioVenta != null)
                {
                    formularioVenta.dataGridViewProductos.Focus();
                }
            }
            if (globalAction == 1)
            {
                Compra formularioCompra = Application.OpenForms.OfType<Compra>().FirstOrDefault();

                if (formularioCompra != null)
                {
                    formularioCompra.dataGridViewProductos.Focus();
                }
            }
        }
    }
}
