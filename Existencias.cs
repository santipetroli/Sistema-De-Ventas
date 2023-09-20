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
    public partial class Existencias : Form
    {
        private Consultas consultas;
        public Existencias()
        {
            InitializeComponent();
            consultas = new Consultas();
            consultas.LlenarExistencias(dataGridView1);
            AjustarGrid();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            consultas.EscribirExistencias(dataGridView1, txtBuscar.Text);
        }

        public void AjustarGrid()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns[0].HeaderText = "CODIGO";
            dataGridView1.Columns[1].HeaderText = "PRODUCTO";
            dataGridView1.Columns[2].HeaderText = "CANTIDAD";

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }
    }
}
