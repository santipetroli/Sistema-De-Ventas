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
    public partial class NuevoProducto : Form
    {
        private Consultas consultas;
        public NuevoProducto()
        {
            InitializeComponent();
            consultas = new Consultas();
            consultas.ObtenerRubros(cbRubro);
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            try
            {
                consultas.CrearProducto(cbRubro, int.Parse(txtCodigo.Text), txtNombre.Text, txtDesc.Text, double.Parse(txtPrecio.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
