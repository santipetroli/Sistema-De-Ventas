using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrabajoPracticoFinal
{
    public static class Conexiones
    {
        private static readonly string cadenaConexion = "Data Source=DESKTOP-B4F2S1B\\SQLEXPRESS;Initial Catalog=TrabajoFinal;User ID=sa;Password=santi";

        public static SqlConnection ObtenerConexion()
        {
            SqlConnection conexion = new SqlConnection(cadenaConexion);
            conexion.Open();

            return conexion;
        }
    }

    public class Consultas
    {
        public void NuevoRubro(string nombre, double recargo)
        {
            string BuscarRubro = "SELECT COUNT(*) FROM Rubro WHERE Nombre = @nombre";
            string InsertarRubro = "INSERT INTO Rubro (Nombre, Recargo) VALUES (@nombre, @recargo)";
            string ActualizarRubro = "UPDATE Rubro SET Recargo = @recargo WHERE Nombre = @nombre";

            using (SqlConnection conexion = Conexiones.ObtenerConexion())
            {
                using (SqlCommand comandoBuscarRubro = new SqlCommand(BuscarRubro, conexion))
                {
                    comandoBuscarRubro.Parameters.AddWithValue("@nombre", nombre);

                    int rubroExistente = (int)comandoBuscarRubro.ExecuteScalar();

                    if (rubroExistente > 0)
                    {
                        DialogResult resultado = MessageBox.Show("El rubro ya existe. ¿Desea actualizar el recargo?", "Rubro Existente", MessageBoxButtons.YesNo);

                        if (resultado == DialogResult.Yes)
                        {
                            using (SqlCommand comandoActualizarRubro = new SqlCommand(ActualizarRubro, conexion))
                            {
                                comandoActualizarRubro.Parameters.AddWithValue("@nombre", nombre);
                                comandoActualizarRubro.Parameters.AddWithValue("@recargo", recargo);

                                comandoActualizarRubro.ExecuteNonQuery();
                                MessageBox.Show("El recargo del rubro se actualizó correctamente.");
                            }
                        }
                    }
                    else
                    {
                        using (SqlCommand comandoInsertarRubro = new SqlCommand(InsertarRubro, conexion))
                        {
                            comandoInsertarRubro.Parameters.AddWithValue("@nombre", nombre);
                            comandoInsertarRubro.Parameters.AddWithValue("@recargo", recargo);

                            comandoInsertarRubro.ExecuteNonQuery();
                            MessageBox.Show("Se ingresó correctamente el nuevo rubro.");
                        }
                    }
                }
            }
        }

        public void CrearProducto(ComboBox comboBox, int codigo, string nombre, string desc, double precio)
        {
            try
            {
                string rubro = comboBox.SelectedItem.ToString();

                if (!string.IsNullOrEmpty(rubro))
                {
                    string BuscarProducto = "SELECT COUNT(*) FROM Producto WHERE id = @codigo";
                    string InsertarProducto = @"INSERT INTO Producto (id, Nombre, Descripcion, Precio, idRubro) VALUES (@id, @nombre, @desc, @precio, @rubro)";
                    string ActualizarProducto = @"UPDATE Producto SET Nombre = @nombre, Descripcion = @desc, Precio = @precio, idRubro = @rubro WHERE id = @codigo";
                    string InsertarExistencias = @"INSERT INTO Existencias (Cantidad, idProducto) VALUES (0, @idproducto)";

                    using (SqlConnection conexion = Conexiones.ObtenerConexion())
                    {
                        using (SqlCommand comandoBuscarProducto = new SqlCommand(BuscarProducto, conexion))
                        {
                            comandoBuscarProducto.Parameters.AddWithValue("@codigo", codigo);

                            int productoExistente = (int)comandoBuscarProducto.ExecuteScalar();

                            if (productoExistente > 0)
                            {
                                DialogResult resultado = MessageBox.Show("El producto ya existe. ¿Desea actualizar los datos?", "Producto Existente", MessageBoxButtons.YesNo);

                                if (resultado == DialogResult.Yes)
                                {
                                    using (SqlCommand comandoActualizarProducto = new SqlCommand(ActualizarProducto, conexion))
                                    {
                                        comandoActualizarProducto.Parameters.AddWithValue("@codigo", codigo);
                                        comandoActualizarProducto.Parameters.AddWithValue("@nombre", nombre);
                                        comandoActualizarProducto.Parameters.AddWithValue("@desc", desc);
                                        comandoActualizarProducto.Parameters.AddWithValue("@precio", precio);
                                        comandoActualizarProducto.Parameters.AddWithValue("@rubro", ObtenerIdRubro(rubro));

                                        comandoActualizarProducto.ExecuteNonQuery();
                                        MessageBox.Show("Se actualizaron los datos del producto correctamente.");
                                    }
                                }
                            }
                            else
                            {
                                using (SqlCommand comandoInsertarProducto = new SqlCommand(InsertarProducto, conexion))
                                {
                                    comandoInsertarProducto.Parameters.AddWithValue("@id", codigo);
                                    comandoInsertarProducto.Parameters.AddWithValue("@nombre", nombre);
                                    comandoInsertarProducto.Parameters.AddWithValue("@desc", desc);
                                    comandoInsertarProducto.Parameters.AddWithValue("@precio", precio);
                                    comandoInsertarProducto.Parameters.AddWithValue("@rubro", ObtenerIdRubro(rubro));

                                    comandoInsertarProducto.ExecuteNonQuery();
                                    MessageBox.Show("Se ingresó correctamente el nuevo producto.");

                                    using (SqlCommand comandoInsertarExistencias = new SqlCommand(InsertarExistencias, conexion))
                                    {
                                        comandoInsertarExistencias.Parameters.AddWithValue("@idproducto", codigo);
                                        comandoInsertarExistencias.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No seleccionó un rubro.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int ObtenerIdRubro(string nombreRubro)
        {
            string query = "SELECT id FROM Rubro WHERE Nombre = @nombreRubro";
            int rubroId;

            using (SqlConnection conexion = Conexiones.ObtenerConexion())
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@nombreRubro", nombreRubro);
                    rubroId = (int)comando.ExecuteScalar();
                }
            }

            return rubroId;
        }

        public void NuevoCliente(string nombre, double dni, string fantasia, string domicilio, double telefono, string email)
        {
            string consulta = @"INSERT INTO Clientes (Nombre, Dni, Fantasia, Domicilio, Telefono, Email) VALUES (@nombre, @dni, @fantasia, @domicilio, @telefono, @email)";

            try
            {
                using (SqlConnection conexion = Conexiones.ObtenerConexion())
                {
                    using (SqlCommand comando = new SqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@nombre", nombre);
                        comando.Parameters.AddWithValue("@dni", dni);
                        comando.Parameters.AddWithValue("@fantasia", fantasia);
                        comando.Parameters.AddWithValue("@domicilio", domicilio);
                        comando.Parameters.AddWithValue("@telefono", telefono);
                        comando.Parameters.AddWithValue("@email", email);

                        comando.ExecuteNonQuery();
                        MessageBox.Show("Se ingresó correctamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public bool ExisteCliente(double dni)
        {
            string consulta = "SELECT COUNT(*) FROM Clientes WHERE Dni = @dni";
            try
            {
                using (SqlConnection conexion = Conexiones.ObtenerConexion())
                {
                    using (SqlCommand comando = new SqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@dni", dni);
                        int count = (int)comando.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        public void ActualizarCliente(string nombre, double dni, string fantasia, string domicilio, double telefono, string email)
        {
            string consulta = @"UPDATE Clientes SET Nombre = @nombre, Fantasia = @fantasia, Domicilio = @domicilio, Telefono = @telefono, Email = @email WHERE Dni = @dni";
            try
            {
                using (SqlConnection conexion = Conexiones.ObtenerConexion())
                {
                    using (SqlCommand comando = new SqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@nombre", nombre);
                        comando.Parameters.AddWithValue("@dni", dni);
                        comando.Parameters.AddWithValue("@fantasia", fantasia);
                        comando.Parameters.AddWithValue("@domicilio", domicilio);
                        comando.Parameters.AddWithValue("@telefono", telefono);
                        comando.Parameters.AddWithValue("@email", email);

                        comando.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void NuevoProveedor(string nombre, double dni, string domicilio, double telefono, string email)
        {
            string consulta = @"INSERT INTO Proveedores (Nombre, Dni, Domicilio, Telefono, Email) VALUES (@nombre, @dni, @domicilio, @telefono, @email)";

            try
            {
                using (SqlConnection conexion = Conexiones.ObtenerConexion())
                {
                    using (SqlCommand comando = new SqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@nombre", nombre);
                        comando.Parameters.AddWithValue("@dni", dni);
                        comando.Parameters.AddWithValue("@domicilio", domicilio);
                        comando.Parameters.AddWithValue("@telefono", telefono);
                        comando.Parameters.AddWithValue("@email", email);

                        comando.ExecuteNonQuery();
                        MessageBox.Show("Se ingresó correctamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public bool ExisteProveedor(double dni)
        {
            string consulta = "SELECT COUNT(*) FROM Proveedores WHERE Dni = @dni";
            try
            {
                using (SqlConnection conexion = Conexiones.ObtenerConexion())
                {
                    using (SqlCommand comando = new SqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@dni", dni);
                        int count = (int)comando.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
        }

        public void ActualizarProveedor(double dni, string nombre, string domicilio, double telefono, string email)
        {
            string consulta = @"UPDATE Proveedores SET Nombre = @nombre, Domicilio = @domicilio, Telefono = @telefono, Email = @email WHERE Dni = @dni";
            try
            {
                using (SqlConnection conexion = Conexiones.ObtenerConexion())
                {
                    using (SqlCommand comando = new SqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@nombre", nombre);
                        comando.Parameters.AddWithValue("@dni", dni);
                        comando.Parameters.AddWithValue("@domicilio", domicilio);
                        comando.Parameters.AddWithValue("@telefono", telefono);
                        comando.Parameters.AddWithValue("@email", email);

                        comando.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void LlenarGridClientes(DataGridView dataGridView)
        {
            string query = "SELECT UPPER(id), UPPER(Nombre) FROM Clientes";
            SqlConnection conexion = Conexiones.ObtenerConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(comando))
                    {
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);
                        dataGridView.DataSource = tabla;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void LlenarGridProv(DataGridView dataGridView)
        {
            string query = "SELECT UPPER(id), UPPER(Nombre) FROM Proveedores";
            SqlConnection conexion = Conexiones.ObtenerConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(comando))
                    {
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);
                        dataGridView.DataSource = tabla;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void EscribirClientes(DataGridView dataGridView, string filtro)
        {
            string query = "SELECT UPPER(id), UPPER(Nombre) FROM Clientes WHERE Nombre LIKE @filtro";
            SqlConnection conexion = Conexiones.ObtenerConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@filtro", "%" + filtro + "%");
                    using (SqlDataAdapter adapter = new SqlDataAdapter(comando))
                    {
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);
                        dataGridView.DataSource = tabla;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void EscribirProveedores(DataGridView dataGridView, string filtro)
        {
            string query = "SELECT UPPER(id), UPPER(Nombre) FROM Proveedores WHERE Nombre LIKE @filtro";
            SqlConnection conexion = Conexiones.ObtenerConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@filtro", "%" + filtro + "%");
                    using (SqlDataAdapter adapter = new SqlDataAdapter(comando))
                    {
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);
                        dataGridView.DataSource = tabla;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void LlenarGridProductosConRecargo(DataGridView dataGridView)
        {
            string query = "SELECT UPPER(id), UPPER(Nombre), Precio * (1 +(SELECT Recargo / 100 from Rubro WHERE id = Producto.idRubro)) from Producto";
            SqlConnection conexion = Conexiones.ObtenerConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(comando))
                    {
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);
                        dataGridView.DataSource = tabla;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void EscribirProductosConRecargo(DataGridView dataGridView, string filtro)
        {
            string query = "SELECT UPPER(id), UPPER(Nombre), Precio * (1 +(SELECT Recargo / 100 from Rubro WHERE id = Producto.idRubro)) from Producto WHERE Nombre LIKE @filtro";
            SqlConnection conexion = Conexiones.ObtenerConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@filtro", "%" + filtro + "%");
                    using (SqlDataAdapter adapter = new SqlDataAdapter(comando))
                    {
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);
                        dataGridView.DataSource = tabla;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void LlenarGridProductos(DataGridView dataGridView)
        {
            string query = "SELECT UPPER(id), UPPER(Nombre), Precio from Producto";
            SqlConnection conexion = Conexiones.ObtenerConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(comando))
                    {
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);
                        dataGridView.DataSource = tabla;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void EscribirProductos(DataGridView dataGridView, string filtro)
        {
            string query = "SELECT UPPER(id), UPPER(Nombre), Precio from Producto WHERE Nombre LIKE @filtro";
            SqlConnection conexion = Conexiones.ObtenerConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@filtro", "%" + filtro + "%");
                    using (SqlDataAdapter adapter = new SqlDataAdapter(comando))
                    {
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);
                        dataGridView.DataSource = tabla;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public string NombreCliente { get; private set; }
        public string FantasiaCliente { get; private set; }
        public string DomicilioCliente { get; private set; }
        public string DniCliente { get; private set; }
        public string TelefonoCliente { get; private set; }
        public string EmailCliente { get; private set; }

        public void ObtenerCliente(int codigo)
        {
            using (SqlConnection connection = Conexiones.ObtenerConexion())
            {
                string query = "SELECT * FROM Clientes WHERE id = @codigoCliente";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@codigoCliente", codigo);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            NombreCliente = reader["nombre"].ToString();
                            FantasiaCliente = reader["fantasia"].ToString();
                            DomicilioCliente = reader["domicilio"].ToString();
                            DniCliente = reader["dni"].ToString();
                            TelefonoCliente = reader["telefono"].ToString();
                            EmailCliente = reader["email"].ToString();
                        }
                    }
                }
            }
        }

        public string NombreProveedor { get; private set; }
        public string DomicilioProveedor { get; private set; }
        public string DniProveedor { get; private set; }
        public string TelefonoProveedor { get; private set; }
        public string EmailProveedor { get; private set; }

        public void ObtenerProveedor(int codigo)
        {
            using (SqlConnection connection = Conexiones.ObtenerConexion())
            {
                string query = "SELECT * FROM Proveedores WHERE id = @codigoproveedor";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@codigoproveedor", codigo);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            NombreProveedor = reader["nombre"].ToString();
                            DniProveedor = reader["dni"].ToString();
                            DomicilioProveedor = reader["domicilio"].ToString();
                            TelefonoProveedor = reader["telefono"].ToString();
                            EmailCliente = reader["email"].ToString();
                        }
                    }
                }
            }
        }

        public string Factura { get; private set; }

        public string NroFactura()
        {
            string consulta = "SELECT MAX(idVenta) + 1 FROM Ventas";
            try
            {
                using (SqlConnection conexion = Conexiones.ObtenerConexion())
                {
                    using (SqlCommand comando = new SqlCommand(consulta, conexion))
                    {
                        object valor = comando.ExecuteScalar();

                        if (valor != null && valor != DBNull.Value)
                        {
                            Factura = valor.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return Factura;
        }

        public string NroFacturaCompra()
        {
            string consulta = "SELECT MAX(idCompra) + 1 FROM Compras";
            try
            {
                using (SqlConnection conexion = Conexiones.ObtenerConexion())
                {
                    using (SqlCommand comando = new SqlCommand(consulta, conexion))
                    {
                        object valor = comando.ExecuteScalar();

                        if (valor != null && valor != DBNull.Value)
                        {
                            Factura = valor.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return Factura;
        }

        public string NombreProducto { get; private set; }
        public string PrecioProducto { get; private set; }

        public void ObtenerProductoConRecargo(int codigo)
        {
            using (SqlConnection connection = Conexiones.ObtenerConexion())
            {
                string query = "SELECT UPPER(id), UPPER(Nombre) as 'Nombre', Precio * (1 +(SELECT Recargo / 100 from Rubro WHERE id = Producto.idRubro)) as 'Precio' from Producto WHERE id = @codigoProducto";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@codigoProducto", codigo);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            NombreProducto = reader["nombre"].ToString();
                            PrecioProducto = reader["precio"].ToString();
                        }
                    }
                }
            }
        }

        public void ObtenerProducto(int codigo)
        {
            using (SqlConnection connection = Conexiones.ObtenerConexion())
            {
                string query = "SELECT UPPER(id), UPPER(Nombre) as 'Nombre', Precio from Producto WHERE id = @codigoProducto";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@codigoProducto", codigo);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            NombreProducto = reader["nombre"].ToString();
                            PrecioProducto = reader["precio"].ToString();
                        }
                    }
                }
            }
        }

        public void InsertarVentayDetalle(DateTime fecha, double total, int idcliente, int idventa, DataGridView dataGridView)
        {
            string query = "INSERT INTO Ventas (fecha_venta, total_venta, id_cliente) VALUES (@fechaventa, @total, @idcliente);";

            using (SqlConnection conexion = Conexiones.ObtenerConexion())
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@fechaventa", fecha);
                    comando.Parameters.AddWithValue("@total", total);
                    comando.Parameters.AddWithValue("@idcliente", idcliente);

                    comando.ExecuteNonQuery();
                }
            }

            string insertarDetalle = "INSERT INTO DetalleVentas (idVenta, idProducto, CantidadProducto, PrecioUnitario) VALUES (@idventa, @idproducto, @cantidad, @precio);";

            using (SqlConnection conexion = Conexiones.ObtenerConexion())
            {
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    int idproducto = Convert.ToInt32(row.Cells[0].Value);
                    double cantidad = Convert.ToDouble(row.Cells[2].Value);
                    string precioString = row.Cells[3].Value.ToString();

                    precioString = precioString.Replace("$", "");

                    double precio = Convert.ToDouble(precioString, CultureInfo.InvariantCulture);

                    using (SqlCommand comando = new SqlCommand(insertarDetalle, conexion))
                    {
                        comando.Parameters.AddWithValue("@idventa", idventa);
                        comando.Parameters.AddWithValue("@idproducto", idproducto);
                        comando.Parameters.AddWithValue("@cantidad", cantidad);
                        comando.Parameters.AddWithValue("@precio", precio);

                        comando.ExecuteNonQuery();
                    }
                }
            }
        }

        public void InsertarComprayDetalle(DateTime fecha, double total, int idproveedor, int idcompra, DataGridView dataGridView)
        {
            string query = "INSERT INTO Compras (fecha_compra, total_compra, id_proveedor) VALUES (@fechacompra, @total, @idproveedor);";

            using (SqlConnection conexion = Conexiones.ObtenerConexion())
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@fechacompra", fecha);
                    comando.Parameters.AddWithValue("@total", total);
                    comando.Parameters.AddWithValue("@idproveedor", idproveedor);

                    comando.ExecuteNonQuery();
                }
            }

            string insertarDetalle = "INSERT INTO DetalleCompras (idCompra, idProducto, CantidadProducto, PrecioUnitario) VALUES (@idcompra, @idproducto, @cantidad, @precio);";

            using (SqlConnection conexion = Conexiones.ObtenerConexion())
            {
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    int idproducto = Convert.ToInt32(row.Cells[0].Value);
                    double cantidad = Convert.ToDouble(row.Cells[2].Value);
                    string precioString = row.Cells[3].Value.ToString();

                    precioString = precioString.Replace("$", "");

                    double precio = Convert.ToDouble(precioString, CultureInfo.InvariantCulture);

                    using (SqlCommand comando = new SqlCommand(insertarDetalle, conexion))
                    {
                        comando.Parameters.AddWithValue("@idcompra", idcompra);
                        comando.Parameters.AddWithValue("@idproducto", idproducto);
                        comando.Parameters.AddWithValue("@cantidad", cantidad);
                        comando.Parameters.AddWithValue("@precio", precio);

                        comando.ExecuteNonQuery();
                    }
                }
            }
        }

        public void ObtenerRubros(ComboBox comboBox)
        {
            string query = "SELECT Nombre FROM Rubro;";
            using (SqlConnection conexion = Conexiones.ObtenerConexion())
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        // Agregar los nombres de los rubros al ComboBox
                        while (reader.Read())
                        {
                            string nombre = reader.GetString(0);
                            comboBox.Items.Add(nombre);
                        }
                    }
                }
            }
        }

        public int ObtenerExistencias(int idProducto)
        {
            string query = "SELECT SUM(Cantidad) FROM Existencias WHERE idProducto = @idProducto";
            int existencias = 0;

            using (SqlConnection conexion = Conexiones.ObtenerConexion())
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@idProducto", idProducto);
                    object resultado = comando.ExecuteScalar();
                    if (resultado != null && resultado != DBNull.Value)
                    {
                        existencias = Convert.ToInt32(resultado);
                    }
                }
            }

            return existencias;
        }

        public void LlenarExistencias(DataGridView dataGridView)
        {
            string query = "SELECT p.id, UPPER(p.Nombre), SUM(e.Cantidad) FROM Producto p INNER JOIN Existencias e ON e.idProducto = p.id GROUP BY P.ID, P.Nombre";
            SqlConnection conexion = Conexiones.ObtenerConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(comando))
                    {
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);
                        dataGridView.DataSource = tabla;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void EscribirExistencias(DataGridView dataGridView, string filtro)
        {
            string query = "SELECT p.id, UPPER(p.Nombre), SUM(e.Cantidad) FROM Producto p INNER JOIN Existencias e ON e.idProducto = p.id WHERE p.Nombre LIKE @filtro GROUP BY P.ID, P.Nombre";
            SqlConnection conexion = Conexiones.ObtenerConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    comando.Parameters.AddWithValue("@filtro", "%" + filtro + "%");
                    using (SqlDataAdapter adapter = new SqlDataAdapter(comando))
                    {
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);
                        dataGridView.DataSource = tabla;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void IngresarCantidad(double exist, int idproducto)
        {
            string query = "INSERT INTO Existencias (Cantidad, idProducto) values (@cantidad, @idproducto)";
            try
            {
                using (SqlConnection conexion = Conexiones.ObtenerConexion())
                {
                    using (SqlCommand comando = new SqlCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@cantidad", exist);
                        comando.Parameters.AddWithValue("@idproducto", idproducto);

                        comando.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void EgresarCantidad(double exist, int idproducto)
        {
            string query = "INSERT INTO Existencias (Cantidad, idProducto) values (-@cantidad, @idproducto)";
            try
            {
                using (SqlConnection conexion = Conexiones.ObtenerConexion())
                {
                    using (SqlCommand comando = new SqlCommand(query, conexion))
                    {
                        comando.Parameters.AddWithValue("@cantidad", exist);
                        comando.Parameters.AddWithValue("@idproducto", idproducto);

                        comando.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ListarVentas(DataGridView dataGridView)
        {
            string query = "SELECT c.Nombre AS 'NOMBRE', v.idventa AS 'N° FACTURA', v.total_venta AS 'MONTO', v.fecha_venta AS 'FECHA' from Ventas v INNER JOIN Clientes c on v.id_cliente = c.id;";
            SqlConnection conexion = Conexiones.ObtenerConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(comando))
                    {
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);
                        dataGridView.DataSource = tabla;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ListarCompras(DataGridView dataGridView)
        {
            string query = "SELECT p.Nombre AS 'NOMBRE', c.idcompra AS 'N° FACTURA', c.total_compra AS 'MONTO', c.fecha_compra AS 'FECHA' from Compras c INNER JOIN PRoveedores p on c.id_proveedor = p.id;";
            SqlConnection conexion = Conexiones.ObtenerConexion();
            try
            {
                using (SqlCommand comando = new SqlCommand(query, conexion))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(comando))
                    {
                        DataTable tabla = new DataTable();
                        adapter.Fill(tabla);
                        dataGridView.DataSource = tabla;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public class FacturaVenta
        {
            public string CodigoCliente { get; set; }
            public string NombreCliente { get; set; }
            public string DomicilioCliente { get; set; }
            public string FantasiaCliente { get; set; }
            public string DniCliente { get; set; }
            public string FechaVenta { get; set; }
            public string IdProducto { get; set; }
            public string NombreProducto { get; set; }
            public string Cantidad { get; set; }
            public string PrecioProducto { get; set; }
            public string TotalVenta { get; set; }
        }

        public class FacturaCompra
        {
            public string CodigoProveedor { get; set; }
            public string NombreProveedor { get; set; }
            public string DomicilioProveedor { get; set; }
            public string DniProveedor { get; set; }
            public string FechaCompra { get; set; }
            public string IdProducto { get; set; }
            public string NombreProducto { get; set; }
            public string Cantidad { get; set; }
            public string PrecioProducto { get; set; }
            public string TotalCompra { get; set; }
        }

        public List<FacturaVenta> ObtenerFacturaVenta(int factura)
        {
            List<FacturaVenta> facturas = new List<FacturaVenta>();

            using (SqlConnection connection = Conexiones.ObtenerConexion())
            {
                string query = "SELECT c.id as id, c.Nombre as nombre, c.Domicilio as domicilio, c.Fantasia as fantasia, c.Dni as dni, v.fecha_venta as fecha_venta, v.total_venta as total_venta, dv.idProducto as idproducto, p.Nombre as producto, dv.CantidadProducto as cantidad, dv.PrecioUnitario as precio FROM clientes c INNER JOIN ventas v ON c.id = v.id_cliente INNER JOIN DetalleVentas dv ON dv.idVenta = v.idVenta INNER JOIN producto p ON p.id = dv.idProducto WHERE v.idVenta = @factura";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@factura", factura);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FacturaVenta facturaVenta = new FacturaVenta();
                            facturaVenta.CodigoCliente = reader["id"].ToString();
                            facturaVenta.NombreCliente = reader["nombre"].ToString();
                            facturaVenta.DomicilioCliente = reader["domicilio"].ToString();
                            facturaVenta.FantasiaCliente = reader["fantasia"].ToString();
                            facturaVenta.DniCliente = reader["dni"].ToString();
                            facturaVenta.FechaVenta = reader["fecha_venta"].ToString();
                            facturaVenta.IdProducto = reader["idproducto"].ToString();
                            facturaVenta.NombreProducto = reader["producto"].ToString();
                            facturaVenta.Cantidad = reader["cantidad"].ToString();
                            facturaVenta.PrecioProducto = reader["precio"].ToString();
                            facturaVenta.TotalVenta = reader["total_venta"].ToString();

                            facturas.Add(facturaVenta);
                        }
                    }
                }
            }

            return facturas;
        }

        public List<FacturaCompra> ObtenerFacturaCompra(int factura)
        {
            List<FacturaCompra> facturas = new List<FacturaCompra>();

            using (SqlConnection connection = Conexiones.ObtenerConexion())
            {
                string query = "SELECT p.id as id, p.Nombre as nombre, p.Domicilio as domicilio, p.Dni as dni, c.fecha_compra as fecha_compra, c.total_compra as total_compra, dc.idProducto as idproducto, prod.Nombre as producto, dc.CantidadProducto as cantidad, dc.PrecioUnitario as precio from Proveedores P INNER JOIN Compras c on p.id = c.id_proveedor INNER JOIN DetalleCompras dc on dc.idCompra = c.idCompra INNER JOIN Producto prod on prod.id = dc.idProducto where c.idCompra = @factura";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@factura", factura);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FacturaCompra facturaCompra = new FacturaCompra();
                            facturaCompra.CodigoProveedor = reader["id"].ToString();
                            facturaCompra.NombreProveedor = reader["nombre"].ToString();
                            facturaCompra.DomicilioProveedor = reader["domicilio"].ToString();
                            facturaCompra.DniProveedor = reader["dni"].ToString();
                            facturaCompra.FechaCompra = reader["fecha_compra"].ToString();
                            facturaCompra.IdProducto = reader["idproducto"].ToString();
                            facturaCompra.NombreProducto = reader["producto"].ToString();
                            facturaCompra.Cantidad = reader["cantidad"].ToString();
                            facturaCompra.PrecioProducto = reader["precio"].ToString();
                            facturaCompra.TotalCompra = reader["total_compra"].ToString();

                            facturas.Add(facturaCompra);
                        }
                    }
                }
            }

            return facturas;
        }

    }
}

