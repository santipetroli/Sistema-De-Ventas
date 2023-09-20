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
    public partial class Rubro : Form
    {
        private Consultas consultas;
        public Rubro()
        {
            InitializeComponent();
            consultas = new Consultas();
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            if (txtNombre.TextLength != 0 && txtRecargo.TextLength != 0)
            {
                try
                {
                    consultas.NuevoRubro(txtNombre.Text, double.Parse(txtRecargo.Text));
                    Limpiar();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Debe completar todos los campos.");
            }
            
        }

        public void Limpiar()
        {
            txtNombre.Text = "";
            txtRecargo.Text = "";
        }
    }
}

