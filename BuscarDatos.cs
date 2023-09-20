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
    public partial class BuscarDatos : Form
    {
        private Consultas consultas;

        int globalAction = 0;
        // 0 es cliente
        // 1 es proveedor
        public BuscarDatos(int action)
        {
            InitializeComponent();
            consultas = new Consultas();

            globalAction = action;

            if (globalAction == 0)
            {
                Text = "CLIENTES";
                consultas.LlenarGridClientes(dataGridView1);
                AjustarGridClientes();
            }
            if (globalAction == 1)
            {
                Text = "PROVEEDORES";
                consultas.LlenarGridProv(dataGridView1);
                AjustarGridProv();
            }
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            if (globalAction == 0)
            {
                consultas.EscribirClientes(dataGridView1, txtBuscar.Text);
            }
            if (globalAction == 1)
            {
                consultas.EscribirProveedores(dataGridView1, txtBuscar.Text);
            }
        }

        public void AjustarGridClientes()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns[0].HeaderText = "CODIGO";
            dataGridView1.Columns[1].HeaderText = "NOMBRE DEL CLIENTE";

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        public void AjustarGridProv()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns[0].HeaderText = "CODIGO";
            dataGridView1.Columns[1].HeaderText = "NOMBRE DEL PROVEEDOR";

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;           
        }

        public int cliente = -1;

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
    }
}
