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
    public partial class Listados : Form
    {
        private Consultas consultas;

        int globalAction = 0;
        // 0 es ventas
        // 1 es compras
        public Listados(int action)
        {
            InitializeComponent();
            consultas = new Consultas();
            globalAction = action;

            if (globalAction == 0)
            {
                Text = "Listado de ventas";
                consultas.ListarVentas(dataGridView1);
                
            }
            if (globalAction == 1)
            {
                Text = "Listado de compras";
                consultas.ListarCompras(dataGridView1);

            }
            AjustarGrid();
        }

        public int factura = -1;

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (globalAction == 0)
            {
                try
                {
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        DataGridViewRow row = dataGridView1.SelectedRows[0];
                        factura = Convert.ToInt32(row.Cells[1].Value);
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            if (globalAction == 1)
            {
                try
                {
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        DataGridViewRow row = dataGridView1.SelectedRows[0];
                        factura = Convert.ToInt32(row.Cells[1].Value);
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void AjustarGrid()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns[0].HeaderText = "NOMBRE";
            dataGridView1.Columns[1].HeaderText = "N° FACTURA";
            dataGridView1.Columns[2].HeaderText = "MONTO";
            dataGridView1.Columns[3].HeaderText = "FECHA";

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }
    }
}
