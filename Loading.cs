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
    public partial class Loading : Form
    {
        private Timer closeTimer;
        private Inicio inicioForm;

        public Loading()
        {
            InitializeComponent();
            lblFinalizado.Visible = false;
            timer1.Start();

            closeTimer = new Timer();
            closeTimer.Interval = 2000;
            closeTimer.Tick += CloseTimer_Tick;

            inicioForm = new Inicio();
            inicioForm.FormClosed += InicioForm_FormClosed;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Increment(5);
            if (progressBar1.Value == 100)
            {
                lblIniciando.Visible = false;
                lblFinalizado.Visible = true;
                closeTimer.Start();
                timer1.Stop();
            }
            lblPercent.Text = progressBar1.Value + "%";
        }

        private void CloseTimer_Tick(object sender, EventArgs e)
        {
            closeTimer.Stop();
            this.Hide();
            inicioForm.ShowDialog();
        }

        private void InicioForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
    }
}
