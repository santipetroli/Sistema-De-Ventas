using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static TrabajoPracticoFinal.Consultas;

namespace TrabajoPracticoFinal
{
    public partial class Compra : Form
    {
        private Consultas consultas;
        public Compra()
        {
            InitializeComponent();
            consultas = new Consultas();
            AjustarGrid();

            txtFecha.Text = DateTime.Now.ToShortDateString();
            txtFactura.Text = consultas.NroFacturaCompra();
        }

        private void dataGridViewProductos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2)
            {
                btnBuscar.PerformClick();
            }
            if (txtCodigo.TextLength != 0)
            {
                if (e.KeyCode == Keys.F12)
                {
                    BuscarProducto bp = new BuscarProducto(1);
                    bp.ShowDialog();
                    if (bp.cliente != -1)
                    {
                        int codigo = bp.cliente;
                        consultas.ObtenerProducto(codigo);

                        int rowIndex = dataGridViewProductos.Rows.Add();

                        DataGridViewRow row = dataGridViewProductos.Rows[rowIndex];
                        row.Cells[0].Value = codigo.ToString();
                        row.Cells[1].Value = consultas.NombreProducto;
                        row.Cells[3].Value = "$" + consultas.PrecioProducto;

                        if (dataGridViewProductos.Rows.Count > 0)
                        {
                            int column = 2;
                            int fila = 0;

                            if (column >= 0 && column < dataGridViewProductos.Columns.Count &&
                                fila >= 0 && fila < dataGridViewProductos.Rows.Count)
                            {
                                dataGridViewProductos.CurrentCell = dataGridViewProductos.Rows[rowIndex].Cells[column];

                                dataGridViewProductos.BeginEdit(true);
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Debe seleccionar un cliente primero.");
            }
        }

        private void btnComprar_Click(object sender, EventArgs e)
        {
            if (txtTotal.TextLength != 0)
            {
                try
                {
                    consultas.InsertarComprayDetalle(DateTime.Now, double.Parse(txtTotal.Text.Replace("$", "")), int.Parse(txtCodigo.Text), int.Parse(txtFactura.Text), dataGridViewProductos);
                    for (int i = 0; i < dataGridViewProductos.Rows.Count; i++)
                    {
                        DataGridViewRow row = dataGridViewProductos.Rows[i];

                        int idproducto = Convert.ToInt32(row.Cells[0].Value);
                        int exist = Convert.ToInt32(row.Cells[2].Value);

                        consultas.IngresarCantidad(exist, idproducto);
                    }
                    Deshabilitar();
                    MessageBox.Show("COMPRA REALIZADA CON EXITO.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Tiene que vender algún producto para guardar.");
            }
        }

        public void AjustarGrid()
        {
            dataGridViewProductos.Columns.Add("IDProducto", "ID Producto");
            dataGridViewProductos.Columns.Add("NombreProducto", "Nombre Producto");
            dataGridViewProductos.Columns.Add("Cantidad", "Cantidad");
            dataGridViewProductos.Columns.Add("Precio", "Precio");
            dataGridViewProductos.Columns.Add("Total", "Total");

            dataGridViewProductos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridViewProductos.Columns[0].HeaderText = "CODIGO";
            dataGridViewProductos.Columns[1].HeaderText = "PRODUCTO";
            dataGridViewProductos.Columns[2].HeaderText = "CANTIDAD";
            dataGridViewProductos.Columns[3].HeaderText = "PRECIO";
            dataGridViewProductos.Columns[4].HeaderText = "TOTAL";

            dataGridViewProductos.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewProductos.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewProductos.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewProductos.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridViewProductos.Columns[0].ReadOnly = true;
            dataGridViewProductos.Columns[1].ReadOnly = true;
            dataGridViewProductos.Columns[3].ReadOnly = true;
            dataGridViewProductos.Columns[4].ReadOnly = true;
            dataGridViewProductos.Columns[2].ReadOnly = false;



            dataGridViewProductos.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewProductos.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewProductos.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewProductos.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewProductos.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BuscarDatos bd = new BuscarDatos(1);
            bd.ShowDialog();
            dataGridViewProductos.Focus();
            if (bd.cliente != -1)
            {
                int codigo = bd.cliente;
                consultas.ObtenerProveedor(codigo);

                txtCodigo.Text = codigo.ToString();
                txtNombre.Text = consultas.NombreProveedor;
                txtDni.Text = consultas.DniProveedor;
                txtDomicilio.Text = consultas.DomicilioProveedor;
                txtTelefono.Text = consultas.TelefonoProveedor;
            }
        }

        private void txtCobro_TextChanged(object sender, EventArgs e)
        {
            if (float.TryParse(txtTotal.Text.Replace("$", ""), out float total) && float.TryParse(txtCobro.Text.Replace("$", ""), out float cobro))
            {
                float vuelto = cobro - total;
                txtVuelto.Text = "$" + vuelto.ToString();
            }
        }

        private void txtCobro_Enter(object sender, EventArgs e)
        {
            txtCobro.Text = string.Empty;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridViewProductos_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex >= 0 && rowIndex < dataGridViewProductos.RowCount)
            {
                DataGridViewRow row = dataGridViewProductos.Rows[rowIndex];

                int cantidad = Convert.ToInt32(row.Cells[2].Value);
                object precio = row.Cells[3].Value;
                string precioString = string.Empty;

                if (precio != null)
                {
                    precioString = precio.ToString();
                    precioString = precioString.Replace("$", "");
                }

                float final;
                if (float.TryParse(precioString, out final))
                {
                    float total = cantidad * final;
                    row.Cells[4].Value = total;
                }
            }

            int suma = 0;

            foreach (DataGridViewRow row in dataGridViewProductos.Rows)
            {
                if (row.Cells[4].Value != null)
                {
                    int valorCelda = Convert.ToInt32(row.Cells[4].Value);
                    suma += valorCelda;
                }
            }
            txtTotal.Text = "$" + suma.ToString();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        public void Limpiar()
        {
            txtNombre.Text = string.Empty;
            txtCodigo.Text = string.Empty;
            txtDni.Text = string.Empty;
            txtDomicilio.Text = string.Empty;
            txtFactura.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtFecha.Text = string.Empty;
            txtTotal.Text = string.Empty;
            txtVuelto.Text = string.Empty;
            txtCobro.Text = string.Empty;
            txtFecha.Text = DateTime.Now.ToShortDateString();
            txtFactura.Text = consultas.NroFactura();
            dataGridViewProductos.Rows.Clear();
            Habilitar();
        }

        public void Deshabilitar()
        {
            txtNombre.Enabled = false;
            txtCodigo.Enabled = false;
            txtDni.Enabled = false;
            txtDomicilio.Enabled = false;
            txtTelefono.Enabled = false;
            txtFactura.Enabled = false;
            txtFecha.Enabled = false;
            txtTotal.Enabled = false;
            txtVuelto.Enabled = false;
            txtCobro.Enabled = false;
            dataGridViewProductos.Enabled = false;
            btnBuscar.Enabled = false;
            btnComprar.Enabled = false;
        }

        public void Habilitar()
        {
            txtNombre.Enabled = true;
            txtCodigo.Enabled = true;
            txtDni.Enabled = true;
            txtDomicilio.Enabled = true;
            txtTelefono.Enabled = true;
            txtFactura.Enabled = true;
            txtFecha.Enabled = true;
            txtTotal.Enabled = true;
            txtVuelto.Enabled = true;
            txtCobro.Enabled = true;
            dataGridViewProductos.Enabled = true;
            btnBuscar.Enabled = true;
            btnComprar.Enabled = true;
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            Listados li = new Listados(1);
            li.ShowDialog();
            if (li.factura != -1)
            {
                Limpiar();
                Deshabilitar();
                int factura = li.factura;
                List<FacturaCompra> facturas = consultas.ObtenerFacturaCompra(factura);

                if (facturas.Count > 0)
                {
                    txtNombre.Text = facturas[0].NombreProveedor;
                    txtDomicilio.Text = facturas[0].DomicilioProveedor;
                    txtDni.Text = facturas[0].DniProveedor;
                    txtFecha.Text = facturas[0].FechaCompra;
                    txtCodigo.Text = facturas[0].CodigoProveedor;
                    txtFactura.Text = factura.ToString();

                    dataGridViewProductos.Rows.Clear();

                    foreach (FacturaCompra facturaCompra in facturas)
                    {
                        int rowIndex = dataGridViewProductos.Rows.Add();
                        DataGridViewRow row = dataGridViewProductos.Rows[rowIndex];
                        row.Cells[0].Value = facturaCompra.IdProducto;
                        row.Cells[1].Value = facturaCompra.NombreProducto;
                        row.Cells[2].Value = facturaCompra.Cantidad;
                        row.Cells[3].Value = "$" + facturaCompra.PrecioProducto;
                        row.Cells[4].Value = "$" + facturaCompra.TotalCompra;
                    }
                }
            }
        }
    }
}
