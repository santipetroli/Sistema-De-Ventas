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
    public partial class Almacen : Form
    {
        public Almacen()
        {
            InitializeComponent();
        }

        private void btnNuevoProd_Click(object sender, EventArgs e)
        {
            NuevoProducto np = new NuevoProducto();
            this.Hide();
            np.ShowDialog();
            this.Show();
        }

        private void btnExist_Click(object sender, EventArgs e)
        {
            Existencias ex = new Existencias();
            this.Hide();
            ex.ShowDialog();
            this.Show();
        }

        private void btnRubros_Click(object sender, EventArgs e)
        {
            Rubro rubro = new Rubro();
            this.Hide();
            rubro.ShowDialog();
            this.Show();
        }
    }
}
