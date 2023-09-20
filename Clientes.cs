﻿using System;
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
    public partial class Clientes : Form
    {
        private Consultas consultas;
        public Clientes()
        {
            consultas = new Consultas();
            InitializeComponent();
            LabelOFF();
            LlenarGrid();
            AjustarGrid();
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            NuevoTextBox();
            LabelON();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNombre.TextLength != 0 && txtDni.TextLength != 0 && txtDomicilio.TextLength != 0 && txtTelefono.TextLength != 0)
                {
                    bool exist = consultas.ExisteCliente(double.Parse(txtDni.Text));

                    if (exist)
                    {
                        DialogResult result = MessageBox.Show("El cliente ya existe. ¿Desea actualizar los datos del cliente?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            consultas.ActualizarCliente(txtNombre.Text, double.Parse(txtDni.Text), txtFantasia.Text, txtDomicilio.Text, double.Parse(txtTelefono.Text), txtEmail.Text);
                            MessageBox.Show("Datos del cliente actualizados correctamente.");
                        }
                        else
                        {
                            MessageBox.Show("No se realizaron cambios en los datos.");
                        }
                    }
                    else
                    {
                        consultas.NuevoCliente(txtNombre.Text, double.Parse(txtDni.Text), txtFantasia.Text, txtDomicilio.Text, double.Parse(txtTelefono.Text), txtEmail.Text);
                        MessageBox.Show("Cliente ingresado correctamente.");
                    }

                    DeshabilitarTextBox();
                    LabelOFF();
                    LlenarGrid();
                }
                else
                {
                    MessageBox.Show("Los campos resaltados con * deben ser completados.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            HabilitarTextBox();
            LabelON();
        }

        public void HabilitarTextBox()
        {
            txtNombre.Enabled = true;
            txtFantasia.Enabled = true;
            txtDomicilio.Enabled = true;
            txtDni.Enabled = true;
            txtTelefono.Enabled = true;
            txtEmail.Enabled = true;
            txtNombre.Focus();
        }

        public void DeshabilitarTextBox()
        {
            txtNombre.Enabled = false;
            txtFantasia.Enabled = false;
            txtDomicilio.Enabled = false;
            txtDni.Enabled = false;
            txtTelefono.Enabled = false;
            txtEmail.Enabled = false;
        }

        public void NuevoTextBox()
        {
            HabilitarTextBox();
            txtNombre.Text = "";
            txtFantasia.Text = "";
            txtDomicilio.Text = "";
            txtDni.Text = "";
            txtTelefono.Text = "";
            txtEmail.Text = "";
        }

        public void LlenarGrid()
        {
            consultas.LlenarGridClientes(dataGridView1);
        }

        public void AjustarGrid()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns[0].HeaderText = "CODIGO";
            dataGridView1.Columns[1].HeaderText = "NOMBRE DEL CLIENTE";

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        public void LabelON()
        {
            label8.Visible = true;
            label9.Visible = true;
            label10.Visible = true;
            label11.Visible = true;
        }

        public void LabelOFF()
        {
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                int id = Convert.ToInt32(row.Cells[0].Value);

                consultas.ObtenerCliente(id);

                txtNombre.Text = consultas.NombreCliente;
                txtDni.Text = consultas.DniCliente;
                txtFantasia.Text = consultas.FantasiaCliente;
                txtDomicilio.Text = consultas.DomicilioCliente;
                txtTelefono.Text = consultas.TelefonoCliente;
                txtEmail.Text = consultas.EmailCliente;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                int id = Convert.ToInt32(row.Cells[0].Value);

                consultas.ObtenerCliente(id);

                txtNombre.Text = consultas.NombreCliente;
                txtDni.Text = consultas.DniCliente;
                txtFantasia.Text = consultas.FantasiaCliente;
                txtDomicilio.Text = consultas.DomicilioCliente;
                txtTelefono.Text = consultas.TelefonoCliente;
                txtEmail.Text = consultas.EmailCliente;
            }
        }
    }
}
