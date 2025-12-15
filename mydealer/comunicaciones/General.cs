using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections;
using System.Xml.Linq;
using Microsoft.Win32;
using System.Web.UI.WebControls;
using SAPbobsCOM;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Data.SqlTypes;
using System.ComponentModel;

namespace mydealer
{
    public class General
    {
        //md_productos

        public static List<MBWProductos> obtenerProductos(string filtro, int numdias)
        {
            List<MBWProductos> response = new List<MBWProductos>();

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            try
            {
                string where = obtenerWhereGeneral(numdias, filtro, "pro");

                SqlCommand com = new SqlCommand(" select codproducto, nombre, codtipoproducto, codgrupoproducto, pagaimpuesto, iUoMEntry, unidadmedida, isnull(costo,0), isnull(stock,0), isnull(precio,0), isnull(peso,0), vendible, estado,  " +
                    " isnull(UMV,0), isnull(ventaminima,0), codigoalterno, costoultcompra from md_productos" + DatosEnlace.sufijo + " " +
                    where, DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                MBWProductos producto;
                if (record.HasRows)
                {
                    while (record.Read())
                    {
                        producto = new MBWProductos();
                        producto.Codproducto = record.GetValue(0).ToString();
                        producto.Nombre = record.GetValue(1).ToString();
                        producto.Codtipoproducto = record.GetValue(2).ToString();
                        producto.Codgrupoproducto = record.GetValue(3).ToString();
                        producto.Pagaimpuesto = record.GetValue(4).ToString();
                        producto.IUoMEntry = record.GetValue(5).ToString();
                        producto.Unidadmedida = record.GetValue(6).ToString();
                        producto.Costo = Double.Parse(record.GetValue(7).ToString());
                        producto.Stock = Int32.Parse(record.GetValue(8).ToString());
                        producto.Precio = Double.Parse(record.GetValue(9).ToString());
                        producto.Peso = Double.Parse(record.GetValue(10).ToString());
                        producto.Vendible = record.GetValue(11).ToString();
                        producto.Estado = record.GetValue(12).ToString();
                        producto.UMV = Int32.Parse(record.GetValue(13).ToString());
                        producto.Ventaminima = Int32.Parse(record.GetValue(14).ToString());
                        producto.Codigoalterno = record.GetValue(15).ToString();
                        producto.Costoultcompra = record.GetValue(16).ToString();

                        response.Add(producto);
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.Clear();
                logs.grabarLog("PRODUCTO", e.Message);
                logs.grabarLog("PRODUCTO_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }

        public static List<MBWStock> obtenerProductosStock()
        {
            List<MBWStock> response = new List<MBWStock>();

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            try
            {
                SqlCommand com = new SqlCommand(" select codproducto, codbodega, stock from md_stock " + DatosEnlace.sufijo , DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                MBWStock stock;
                if (record.HasRows)
                {
                    while (record.Read())
                    {
                        stock = new MBWStock();
                        stock.Codproducto = record.GetValue(0).ToString();
                        stock.Codbodega = record.GetValue(1).ToString();
                        stock.Stock = Int32.Parse(record.GetValue(2).ToString());

                        response.Add(stock);
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.Clear();
                logs.grabarLog("PRODUCTO_STOCK", e.Message);
                logs.grabarLog("PRODUCTO_STOCK_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }

        public static List<MBWGrupoProducto> obtenerGruposProducto(string filtro)
        {
            List<MBWGrupoProducto> response = new List<MBWGrupoProducto>();

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            try
            {
                SqlCommand com = new SqlCommand(" select codgrupoproducto, descripcion from md_grupoproducto " + DatosEnlace.sufijo + " " + (!filtro.Equals("") ? " where codtipoproducto like '%" + filtro +
                    "%' or descripcion like '%" + filtro + "%'" : ""), DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                MBWGrupoProducto cliente;
                if (record.HasRows)
                {
                    while (record.Read())
                    {
                        cliente = new MBWGrupoProducto();
                        cliente.Codgrupoproducto = record.GetValue(0).ToString();
                        cliente.Descripcion = record.GetValue(1).ToString();

                        response.Add(cliente);
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.Clear();
                logs.grabarLog("GRUPO_PRODUCTO", e.Message);
                logs.grabarLog("GRUPO_PRODUCTO_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }


        public static List<MBWTipoProducto> obtenerTiposProducto(string filtro)
        {
            List<MBWTipoProducto> response = new List<MBWTipoProducto>();

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            try
            {
                SqlCommand com = new SqlCommand(" select codtipoproducto, descripcion, codgrupoproducto from md_tipoproducto" + DatosEnlace.sufijo + " " + (!filtro.Equals("") ? " where codtipoproducto like '%" + filtro +
                    "%' or descripcion like '%" + filtro + "%'" : ""), DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                MBWTipoProducto cliente;
                if (record.HasRows)
                {
                    while (record.Read())
                    {
                        cliente = new MBWTipoProducto();
                        cliente.Codtipoproducto = record.GetValue(0).ToString();
                        cliente.Descripcion = record.GetValue(1).ToString();
                        cliente.Codgrupoproducto = record.GetValue(2).ToString();

                        response.Add(cliente);
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.Clear();
                logs.grabarLog("TIPO_PRODUCTO", e.Message);
                logs.grabarLog("TIPO_PRODUCTO_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }

        private static string obtenerWhereGeneral(int numdias, string filtro, string tabla)
        {
            string where = " where 1 = 1";
            if (numdias > 0)
                where += " AND ((CreateDate > DATEADD(day, - " + numdias + ", GETDATE())) " +
                    " OR  (UpdateDate > DATEADD(day, - " + numdias + ", GETDATE()))) ";

            if (filtro != "" && tabla == "cli")
                where += " AND ( codcliente like '%" + filtro + "%' or descripcion like '%" + filtro + "%' ) ";

            if (filtro != "" && tabla == "dircli")
                where += " AND ( codaddress like '%" + filtro + "%' or codcliente like '%" + filtro + "%' or direccion like '%" + filtro + "%' ) ";

            if (filtro != "" && tabla == "pro")
                where += " AND ( codproducto like '%" + filtro + "%' or nombre like '%" + filtro + "%' ) ";

            return where;
        }

        public static List<MBWClientes> obtenerClientes(string filtro, int numdias)
        {
            List<MBWClientes> response = new List<MBWClientes>();

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            try
            {
                string where = obtenerWhereGeneral(numdias, filtro, "cli");

                SqlCommand com = new SqlCommand(" select codcliente, descripcion, isnull(codzona, ''), isnull(codtipocliente,0), Address, isnull(Phone1,''), Country, " +
                    " City, cedularuc, codlista, estado, isnull(codvendedor,''), isnull(correo,''), isnull(codcondicionpago,''), isnull(codformapago,''), isnull(coddireccionenvio,''), isnull(limitecredito,0), isnull(saldopendiente,0), isnull(saldovencido,0) " +
                    " from md_clientes" + DatosEnlace.sufijo + " " + where, DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                MBWClientes cliente;
                if (record.HasRows)
                {
                    while (record.Read())
                    {
                        cliente = new MBWClientes();
                        cliente.Codcliente = record.GetValue(0).ToString();
                        cliente.Descripcion = record.GetValue(1).ToString();
                        cliente.Codzona = record.GetValue(2).ToString();
                        cliente.CodTipoCliente = record.GetValue(3).ToString();
                        cliente.Address = record.GetValue(4).ToString();
                        cliente.Phone1 = record.GetValue(5).ToString();
                        cliente.Country = record.GetValue(6).ToString();
                        cliente.City = record.GetValue(7).ToString();
                        cliente.CedulaRuc = record.GetValue(8).ToString();
                        cliente.Codlista = record.GetValue(9).ToString();
                        cliente.Estado = record.GetValue(10).ToString();
                        cliente.Codvendedor = record.GetValue(11).ToString();
                        cliente.Correo = record.GetValue(12).ToString();
                        cliente.Codcondicionpago = record.GetValue(13).ToString();
                        cliente.Codformapago = record.GetValue(14).ToString();
                        cliente.Coddireccionenvio = record.GetValue(15).ToString();
                        cliente.Limitecredito = double.Parse(record.GetValue(16).ToString());
                        cliente.Saldopendiente = double.Parse(record.GetValue(17).ToString());
                        cliente.Saldovencido = double.Parse(record.GetValue(18).ToString());

                        response.Add(cliente);
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.Clear();
                logs.grabarLog("CLIENTES", e.Message);
                logs.grabarLog("CLIENTES_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }

        public static List<MBWClientesVista2> obtenerClientesVista2(string filtro, int numdias)
        {
            List<MBWClientesVista2> response = new List<MBWClientesVista2>();

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            try
            {
                string where = obtenerWhereGeneral(numdias, filtro, "cli");

                SqlCommand com = new SqlCommand(" SELECT codcliente, codvendedor,  isnull(limitecredito,0),  isnull(saldocuenta,0),  isnull(cupoutilizado,0),  documenta, diasvencidos, codformapago, isnull(saldopendiente,0)  FROM gc_cliente2" + DatosEnlace.sufijo + " " + where, DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                MBWClientesVista2 cliente;
                if (record.HasRows)
                {
                    while (record.Read())
                    {
                        cliente = new MBWClientesVista2();
                        cliente.Codcliente = record.GetValue(0).ToString();
                        cliente.Codvendedor = record.GetValue(1).ToString();
                        cliente.Limitecredito = double.Parse(record.GetValue(2).ToString());
                        cliente.Saldocuenta = double.Parse(record.GetValue(3).ToString());
                        cliente.Cupoutilizado = double.Parse(record.GetValue(4).ToString());
                        cliente.Documenta = record.GetValue(5).ToString();
                        cliente.Diasvencidos = record.GetValue(6).ToString();
                        cliente.Codformapago = record.GetValue(7).ToString();
                        cliente.Saldopendiente = double.Parse(record.GetValue(8).ToString());
                        response.Add(cliente);
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.Clear();
                logs.grabarLog("CLIENTES_VISTA2", e.Message);
                logs.grabarLog("CLIENTES_VISTA2_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }

        public static List<List<string>> obtenerConsulta(string tabla, string campos, int desde, int hasta, int numdias)
        {
            List<List<string>> response = new List<List<string>>();

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            string where = "";

            if (numdias != 0)
            {
                where = " fecha_update > DATEADD(day, -" + numdias + ", GETDATE())";
            }
            else
            {
                where = " row BETWEEN " + desde + " AND " + hasta;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            try
            {
                SqlCommand com = new SqlCommand(" SELECT " + campos +
                    " FROM " + tabla + DatosEnlace.sufijo + " WHERE " + where, DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                com.CommandTimeout = 90;
                SqlDataReader record = com.ExecuteReader();

                DataTable schemaTable = record.GetSchemaTable();
                List<string> listado;

                if (record.HasRows)
                {
                    while (record.Read())
                    {
                        listado = new List<string>();
                        // To iterate over it.
                        for (int f = 0; f < schemaTable.Rows.Count; f++)
                        {
                            listado.Add(record.GetValue(f).ToString());
                        }
                        response.Add(listado);
                    }
                }

                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.Clear();
                logs.grabarLog("CONSULTA", e.Message);
                logs.grabarLog("CONSULTA_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }

        public static string obtenerConsultaCount(string tabla)
        {
            string response = "0";

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            try
            {
                SqlCommand com = new SqlCommand(" SELECT COUNT(*) FROM " + tabla + DatosEnlace.sufijo, DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();
                DataTable schemaTable = record.GetSchemaTable();

                if (record.HasRows)
                {
                    while (record.Read())
                    {
                        response = record.GetValue(0).ToString();
                    }
                }

                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response = "0";
                logs.grabarLog("CONSULTA_COUNT", e.Message);
                logs.grabarLog("CONSULTA_COUNT_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }


        public static List<MBWSupervisor> obtenerSupervisores(string filtro)
        {
            List<MBWSupervisor> response = new List<MBWSupervisor>();

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            try
            {
                SqlCommand com = new SqlCommand(" select codsupervisor, nombre, email from md_supervisores" + DatosEnlace.sufijo + " " + (!filtro.Equals("") ? " where codsupervisor like '%" + filtro +
                    "%' or nombre like '%" + filtro + "%'" : ""), DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                if (record.HasRows)
                {
                    MBWSupervisor registro;
                    while (record.Read())
                    {
                        registro = new MBWSupervisor();
                        registro.Codsupervisor = record.GetValue(0).ToString();
                        registro.Nombre = record.GetValue(1).ToString();
                        registro.Email = record.GetValue(2).ToString();

                        response.Add(registro);
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.Clear();
                logs.grabarLog("SUPERVISORES", e.Message);
                logs.grabarLog("SUPERVISORES_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }


        public static List<MBWVendedores> obtenerVendedores(string filtro)
        {
            List<MBWVendedores> response = new List<MBWVendedores>();

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            try
            {
                SqlCommand com = new SqlCommand(" select codvendedor, descripcion, codbodegadef, codsupervisor, codsucursal, email from md_vendedores" + DatosEnlace.sufijo + " " + (!filtro.Equals("") ? " where codvendedor like '%" + filtro +
                    "%' or descripcion like '%" + filtro + "%'" : ""), DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                if (record.HasRows)
                {
                    MBWVendedores vendedor;
                    while (record.Read())
                    {
                        vendedor = new MBWVendedores();
                        vendedor.Codvendedor = record.GetValue(0).ToString();
                        vendedor.Descripcion = record.GetValue(1).ToString();
                        vendedor.Codbodegadef = record.GetValue(2).ToString();
                        vendedor.Codsupervisor = record.GetValue(3).ToString();
                        vendedor.Codsucursal = record.GetValue(4).ToString();
                        vendedor.Email = record.GetValue(5).ToString();

                        response.Add(vendedor);
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.Clear();
                logs.grabarLog("VENDEDORES", e.Message);
                logs.grabarLog("VENDEDORES_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }


        public static List<MBWFormasPago> obtenerFormasPago(string filtro)
        {
            List<MBWFormasPago> response = new List<MBWFormasPago>();

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            try
            {
                SqlCommand com = new SqlCommand(" select codformapago, descripcion, isnull(dias,0) from md_formapago" + DatosEnlace.sufijo + " " + (!filtro.Equals("") ? " where codformapago like '%" + filtro +
                    "%' or descripcion like '%" + filtro + "%'" : ""), DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                MBWFormasPago formapago;

                if (record.HasRows)
                {
                    while (record.Read())
                    {
                        formapago = new MBWFormasPago();
                        formapago.Codformapago = record.GetValue(0).ToString();
                        formapago.Descripcion = record.GetValue(1).ToString();
                        formapago.Dias = Int32.Parse(record.GetValue(2).ToString());

                        response.Add(formapago);
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.Clear();
                logs.grabarLog("FORMA_PAGO", e.Message);
                logs.grabarLog("FORMA_PAGO_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }


        public static List<MBWSubcanal> obtenerSubcanales(string filtro)
        {
            List<MBWSubcanal> response = new List<MBWSubcanal>();

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            // select DocNum, NumAtCard from ORDR where NumAtCard=1
            try
            {
                SqlCommand com = new SqlCommand(" select codsubcanal, descripcion from md_subcanales" + DatosEnlace.sufijo + " " + (!filtro.Equals("") ? " where codsubcanal like '%" + filtro +
                    "%' or descripcion like '%" + filtro + "%'" : ""), DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                MBWSubcanal subcanal;
                if (record.HasRows)
                {
                    while (record.Read())
                    {
                        subcanal = new MBWSubcanal();
                        subcanal.Codsubcanal = Int32.Parse(record.GetValue(0).ToString());
                        subcanal.Descripcion = record.GetValue(1).ToString();
                        response.Add(subcanal);
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.Clear();
                logs.grabarLog("SUBCANAL", e.Message);
                logs.grabarLog("SUBCANAL_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }


        public static List<MBWZonas> obtenerZonas(string filtro)
        {
            List<MBWZonas> response = new List<MBWZonas>();

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            // select DocNum, NumAtCard from ORDR where NumAtCard=1
            try
            {
                SqlCommand com = new SqlCommand(" select codzona, nombre, codvendedor from md_zonas" + DatosEnlace.sufijo + " " + (!filtro.Equals("") ? " where codzona like '%" + filtro +
                    "%' or descripcion like '%" + filtro + "%'" : ""), DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                MBWZonas zona;
                if (record.HasRows)
                {
                    while (record.Read())
                    {
                        zona = new MBWZonas();
                        zona.Codzona = record.GetValue(0).ToString();
                        zona.Descripcion = record.GetValue(1).ToString();
                        zona.Codvendedor = record.GetValue(2).ToString();

                        response.Add(zona);
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.Clear();
                logs.grabarLog("ZONAS", e.Message);
                logs.grabarLog("ZONAS_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }

        public static List<MBWClienteTipo> obtenerClienteTipo()
        {
            List<MBWClienteTipo> response = new List<MBWClienteTipo>();

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            // select DocNum, NumAtCard from ORDR where NumAtCard=1
            try
            {
                SqlCommand com = new SqlCommand(" select codtipocliente, descripcion from md_tipocliente " + DatosEnlace.sufijo + " ", DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                MBWClienteTipo registro;
                if (record.HasRows)
                {
                    while (record.Read())
                    {
                        registro = new MBWClienteTipo();
                        registro.Codtipocliente = record.GetValue(0).ToString();
                        registro.Descripcion = record.GetValue(1).ToString();

                        response.Add(registro);
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.Clear();
                logs.grabarLog("CLIENTETIPO", e.Message);
                logs.grabarLog("CLIENTETIPO_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }

        public static List<MBWListaPrecio> obtenerListaPrecios()
        {
            List<MBWListaPrecio> response = new List<MBWListaPrecio>();

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            // select DocNum, NumAtCard from ORDR where NumAtCard=1
            try
            {
                SqlCommand com = new SqlCommand(" select codlistaprecio, nombre from md_listaprecios " + DatosEnlace.sufijo + " ", DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                MBWListaPrecio listaprecio;
                if (record.HasRows)
                {
                    while (record.Read())
                    {
                        listaprecio = new MBWListaPrecio();
                        listaprecio.Codlistaprecio = record.GetValue(0).ToString();
                        listaprecio.Nombre = record.GetValue(1).ToString();

                        response.Add(listaprecio);
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.Clear();
                logs.grabarLog("LISTAPRECIOS", e.Message);
                logs.grabarLog("LISTAPRECIOS_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }

        public static List<MBWListaPrecioDet> obtenerListaPreciosDet(string filtro)
        {
            List<MBWListaPrecioDet> response = new List<MBWListaPrecioDet>();

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            // select DocNum, NumAtCard from ORDR where NumAtCard=1
            try
            {
                SqlCommand com = new SqlCommand(" select codlistaprecio, codproducto, isnull(precio,0) from md_listapreciosdet " + DatosEnlace.sufijo + " " + (!filtro.Equals("") ? " where codlistaprecio like '%" + filtro : ""), DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                MBWListaPrecioDet listaprecio;
                if (record.HasRows)
                {
                    while (record.Read())
                    {
                        listaprecio = new MBWListaPrecioDet();
                        listaprecio.Codlistaprecio = record.GetValue(0).ToString();
                        listaprecio.Codproducto = record.GetValue(1).ToString();
                        listaprecio.Precio = Double.Parse(record.GetValue(2).ToString());


                        response.Add(listaprecio);
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.Clear();
                logs.grabarLog("LISTAPRECIOSDET", e.Message);
                logs.grabarLog("LISTAPRECIOSDET_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }

        public static List<MBWSucursales> obtenerSucursales(string filtro)
        {
            List<MBWSucursales> response = new List<MBWSucursales>();

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            // select DocNum, NumAtCard from ORDR where NumAtCard=1
            try
            {
                SqlCommand com = new SqlCommand(" select codsucursal, nombre from md_sucursal " + DatosEnlace.sufijo + " " + (!filtro.Equals("") ? " where codsucursal like '%" + filtro +
                    "%' or nombre like '%" + filtro + "%'" : ""), DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                MBWSucursales sucursal;
                if (record.HasRows)
                {
                    while (record.Read())
                    {
                        sucursal = new MBWSucursales();
                        sucursal.Codsucursal = record.GetValue(0).ToString();
                        sucursal.Nombre = record.GetValue(1).ToString();


                        response.Add(sucursal);
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.Clear();
                logs.grabarLog("SUCURSALES", e.Message);
                logs.grabarLog("SUCURSALES_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }


        public static List<MBWBodegas> obtenerBodegas(string filtro)
        {
            List<MBWBodegas> response = new List<MBWBodegas>();

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            // select DocNum, NumAtCard from ORDR where NumAtCard=1
            try
            {
                SqlCommand com = new SqlCommand(" select codbodega, descripcion, series from md_bodegas" + DatosEnlace.sufijo + " " + (!filtro.Equals("") ? " where codbodega like '%" + filtro +
                    "%' or descripcion like '%" + filtro + "%'" : ""), DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                MBWBodegas bodega;
                if (record.HasRows)
                {
                    while (record.Read())
                    {
                        bodega = new MBWBodegas();
                        bodega.Codbodega = record.GetValue(0).ToString();
                        bodega.Descripcion = record.GetValue(1).ToString();
                        bodega.Seriefacturacion = record.GetValue(2).ToString();


                        response.Add(bodega);
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.Clear();
                logs.grabarLog("BODEGAS", e.Message);
                logs.grabarLog("BODEGAS_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }


        public static List<MBWDireccionCliente> obtenerDireccionClientes(string filtro, int numdias)
        {
            List<MBWDireccionCliente> response = new List<MBWDireccionCliente>();

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            try
            {
                string where = obtenerWhereGeneral(numdias, filtro, "dircli");

                SqlCommand com = new SqlCommand(" select coddireccion, cliente, codcliente, direccion, ciudad, pais, codaddress, ISNULL(orden,999), ciudad_provincia, provincia, telefono from md_direccioncliente" + DatosEnlace.sufijo + " " + where, DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                MBWDireccionCliente direccion;
                if (record.HasRows)
                {
                    while (record.Read())
                    {
                        direccion = new MBWDireccionCliente();
                        direccion.Coddireccion = record.GetValue(0).ToString();
                        direccion.Cliente = record.GetValue(1).ToString();
                        direccion.Codcliente = record.GetValue(2).ToString();
                        direccion.Direccion = record.GetValue(3).ToString();
                        direccion.Ciudad = record.GetValue(4).ToString();
                        direccion.Pais = record.GetValue(5).ToString();
                        direccion.CodAddress = record.GetValue(6).ToString();
                        direccion.Orden = Int32.Parse(record.GetValue(7).ToString());
                        direccion.CiudadProvincia = record.GetValue(8).ToString();
                        direccion.Provincia = record.GetValue(9).ToString();
                        direccion.Telefono = record.GetValue(10).ToString();

                        response.Add(direccion);
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.Clear();
                logs.grabarLog("DIRECCION_CLIENTE", e.Message);
                logs.grabarLog("DIRECCION_CLIENTE_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }


        public static List<MBWBanco> obtenerBancos()
        {
            List<MBWBanco> response = new List<MBWBanco>();

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            try
            {
                SqlCommand com = new SqlCommand(" SELECT codbanco, descripcion FROM md_bancos" + DatosEnlace.sufijo + " ", DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                MBWBanco banco;
                if (record.HasRows)
                {
                    while (record.Read())
                    {
                        banco = new MBWBanco();
                        banco.Codbanco = record.GetValue(0).ToString();
                        banco.Descripcion = record.GetValue(1).ToString();
                        response.Add(banco);
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.Clear();
                logs.grabarLog("BANCOS", e.Message);
                logs.grabarLog("BANCOS_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }


        public static List<CuentasBancarias> obtenerCuentasBancariasPorBanco(string codbanco)
        {
            List<CuentasBancarias> response = new List<CuentasBancarias>();

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            try
            {
                string where_codbanco = "";
                if (!codbanco.Equals("**ALL**") && !codbanco.Equals(""))
                {
                    where_codbanco = " WHERE codbanco = '" + codbanco + "' ";
                }
                SqlCommand com = new SqlCommand(" SELECT codbanco, descripcion, numcuenta, tipo, GLAccount FROM md_bancospropios" + DatosEnlace.sufijo + " " +
                    where_codbanco, DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                CuentasBancarias cuenta;
                if (record.HasRows)
                {
                    while (record.Read())
                    {
                        cuenta = new CuentasBancarias();
                        cuenta.Codbanco = record.GetValue(0).ToString();
                        cuenta.Descripcion = record.GetValue(1).ToString();
                        cuenta.Numcuenta = record.GetValue(2).ToString();
                        cuenta.Tipo = record.GetValue(3).ToString();
                        cuenta.GLAccount1 = record.GetValue(4).ToString();

                        response.Add(cuenta);
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.Clear();
                logs.grabarLog("CTASBANCARIAS", e.Message);
                logs.grabarLog("CTASBANCARIAS_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }


        // md_ctas_caja_chq
        public static ctas_caja_chq obtenerCtasCajaCheque()
        {
            ctas_caja_chq response = new ctas_caja_chq();

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            try
            {
                SqlCommand com = new SqlCommand(" SELECT cta_cheque, cta_caja FROM md_ctas_caja_chq" + DatosEnlace.sufijo + " ", DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                if (record.HasRows)
                {
                    record.Read();

                    response.Cta_cheque = record.GetValue(0).ToString();
                    response.Cta_caja = record.GetValue(1).ToString();
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response = new ctas_caja_chq();
                logs.grabarLog("CTACAJACHEQUE", e.Message);
                logs.grabarLog("CTACAJACHEQUE_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }

        // md_ctas_caja_chq
        public static List<MBWVentas> obtenerVentas()
        {
            List<MBWVentas> response = new List<MBWVentas>();

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            try
            {
                SqlCommand com = new SqlCommand(" SELECT anio, mes, Vendedor, VentaDolares, NumeroClientes, numero_items FROM md_ventas" + DatosEnlace.sufijo + " ", DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                MBWVentas venta;
                if (record.HasRows)
                {
                    while (record.Read())
                    {
                        venta = new MBWVentas();
                        venta.Anio = int.Parse(record.GetValue(0).ToString());
                        venta.Mes = int.Parse(record.GetValue(1).ToString());
                        venta.Vendedor = int.Parse(record.GetValue(2).ToString());
                        venta.VtaDolares = double.Parse(record.GetValue(3).ToString());
                        venta.NumClientes = int.Parse(record.GetValue(4).ToString());
                        venta.NumItems = int.Parse(record.GetValue(5).ToString());
                        venta.Codproducto = "";

                        response.Add(venta);
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();

            }
            catch (Exception e)
            {
                response.Clear();
                logs.grabarLog("VENTAS", e.Message);
                logs.grabarLog("VENTAS_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }



            return response;
        }


        public static List<MBWClienteFormapago> obtenerClientesFormaspago(int numdias)
        {
            List<MBWClienteFormapago> response = new List<MBWClienteFormapago>();

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            try
            {
                string where = obtenerWhereGeneral(numdias, "", "");

                SqlCommand com = new SqlCommand(" SELECT codcliente, codformapago FROM md_clientesformaspago" + DatosEnlace.sufijo + " " + where, DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                MBWClienteFormapago cuenta;
                if (record.HasRows)
                {
                    while (record.Read())
                    {
                        cuenta = new MBWClienteFormapago();
                        cuenta.Codcliente = record.GetValue(0).ToString();
                        cuenta.Codformapago = int.Parse(record.GetValue(1).ToString());

                        response.Add(cuenta);
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();

            }
            catch (Exception e)
            {
                response.Clear();
                logs.grabarLog("CLIENTEFORMAPAGO", e.Message);
                logs.grabarLog("CLIENTEFORMAPAGO_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }

        public static List<MBWPrecios> obtenerPrecios(string filtro)
        {
            List<MBWPrecios> response = new List<MBWPrecios>();

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            // select DocNum, NumAtCard from ORDR where NumAtCard=1
            try
            {
                SqlCommand com = new SqlCommand(" select codproducto, codlista, isnull(precio,0) from md_precios" + DatosEnlace.sufijo + " " +
                                (!filtro.Equals("") ? " where codproducto like '%" + filtro + "%' " : ""), DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                MBWPrecios precio;
                if (record.HasRows)
                {
                    while (record.Read())
                    {
                        precio = new MBWPrecios();
                        precio.CodProducto = record.GetValue(0).ToString();
                        precio.CodTipoCliente = record.GetValue(1).ToString();
                        precio.Precio = Double.Parse(record.GetValue(2).ToString());

                        response.Add(precio);
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.Clear();
                logs.grabarLog("PRECIOS", e.Message);
                logs.grabarLog("PRECIOS_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }


        public static Respuesta obtenerCantidadRegistros(String tabla, int numdias)
        {
            Respuesta response = new Respuesta();
            response.Exito = false;
            response.MaxRegistros = 0;

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();
            try
            {
                string where = obtenerWhereGeneral(numdias, "", "");

                SqlCommand com = new SqlCommand(" select count(*) as m1 from " + tabla + DatosEnlace.sufijo + where, DBSqlServer.Conexion);
                com.CommandTimeout = 900;
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                if (record.HasRows)
                {
                    if (record.Read())
                    {
                        response.Exito = true;
                        response.MaxRegistros = Int32.Parse(record.GetValue(0).ToString());
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.Exito = false;
                response.EntradaRAW = "0";
                logs.grabarLog("CANT_REGISTROS", e.Message);
                logs.grabarLog("CANT_REGISTROS", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }



        public static Respuesta crearDireccion(String ndoc, String direcent, String ubigeo)
        {

            Respuesta response = new Respuesta();
            response.Exito = false;
            response.DescripcionError = "";
            response.CodigoRespuesta = "";

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            try
            {
                // Validar que la direccion no exista

                int total = 0;

                SqlCommand com = new SqlCommand(" select count(*) as m1 from DirecEnt " +
                    "where Cd_Clt = (SELECT TOP 1 Cd_Clt FROM Cliente2 WHERE NDoc=@ndoc) " +
                    "AND Direc = @direcent " +
                    "AND Cd_UDt = @ubigeo ", DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;
                com.CommandTimeout = 900;
                com.Parameters.AddWithValue("@ndoc", ndoc);
                com.Parameters.AddWithValue("@direcent", direcent);
                com.Parameters.AddWithValue("@ubigeo", ubigeo);

                SqlDataReader record = com.ExecuteReader();

                if (record.HasRows)
                {
                    if (record.Read())
                    {
                        total = Int32.Parse(record.GetValue(0).ToString());
                    }
                }
                record.Close();
                record.Dispose();

                if (total == 0)
                {

                    SqlCommand com2 = new SqlCommand(" SELECT TOP 1 Nombre FROM UDist WHERE Cd_UDt = @UBIGEO ", DBSqlServer.Conexion);
                    com2.CommandType = CommandType.Text;
                    com2.CommandTimeout = 900;
                    com2.Parameters.AddWithValue("@UBIGEO", ubigeo);

                    String nombre_ubigeo = "";

                    SqlDataReader record2 = com2.ExecuteReader();

                    if (record2.HasRows)
                    {
                        if (record2.Read())
                        {
                            nombre_ubigeo = record2.GetValue(0).ToString();
                        }
                    }
                    record2.Close();
                    record2.Dispose();

                    //----------------------------------

                    SqlCommand cmd = new SqlCommand("FESEPSA_MNG_CLIENTE", DBSqlServer.Conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900;
                    cmd.Parameters.AddWithValue("@OPC", "4");
                    cmd.Parameters.AddWithValue("@NDOC", ndoc);
                    cmd.Parameters.AddWithValue("@DIREC", direcent);
                    cmd.Parameters.AddWithValue("@UBIGEO", nombre_ubigeo);

                    String res = "";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {

                        while (dr.Read())
                        {
                            res = dr["Respuesta"].ToString();
                        }

                    }

                    if (res == "ERROR")
                    {
                        response.DescripcionError = "Error al crear la direccion";
                    }
                    else
                    {
                        response.Exito = true;

                        SqlCommand com3 = new SqlCommand(" SELECT TOP 1 coddireccion FROM md_direccioncliente  WHERE codcliente = (SELECT TOP 1 Cd_Clt FROM Cliente2 WHERE NDoc=@ndoc) AND  cod_ubigeo = @ubigeo ", DBSqlServer.Conexion);
                        com3.CommandType = CommandType.Text;
                        com3.CommandTimeout = 900;
                        com3.Parameters.AddWithValue("@ndoc", ndoc);
                        com3.Parameters.AddWithValue("@ubigeo", ubigeo);

                        SqlDataReader record3 = com3.ExecuteReader();

                        if (record3.HasRows)
                        {
                            if (record3.Read())
                            {
                                response.CodigoRespuesta = record3.GetValue(0).ToString();
                            }
                        }
                        record3.Close();
                        record3.Dispose();
                    }
                }
                else {
                    response.DescripcionError = "La dirección existe en el sistema";
                }

            }
            catch (Exception e)
            {
                response.DescripcionError = e.Message;
                logs.grabarLog("CREAR_DIRECCION", e.Message);
                logs.grabarLog("CREAR_DIRECCION", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;

        }

        public static RespuestaPedido existePedidoFesepsa(int numeroPedidoMydealer)
        {
            RespuestaPedido response = new RespuestaPedido();
            response.creado = false;
            response.numeroPedido = "0";
            response.numeroPedidoMydealer = numeroPedidoMydealer;
            response.error = "";

            string nombre_log = "PEDIDO_" + numeroPedidoMydealer;

            logs.grabarLog(nombre_log, "INICIO PROCESO EXISTE PEDIDO", "PEDIDO");

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                response.error = DBSqlServer.Respuesta.DescripcionError;
                return response;
            }

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.

            try
            {
                SqlCommand com = new SqlCommand(" select numpedidoerp, srorden " +
                    " from md_tracking" + DatosEnlace.sufijo + " where srorden=" + numeroPedidoMydealer, DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                if (record.HasRows)
                {
                    if (record.Read())
                    {
                        response.creado = true;
                        response.numeroPedido = record.GetValue(0).ToString();
                        response.numeroPedidoMydealer = Int32.Parse(record.GetValue(1).ToString());
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.creado = false;
                response.error = e.Message;
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            logs.grabarLog(nombre_log, "FIN PROCESO EXISTE PEDIDO", "PEDIDO");

            return response;
        }

        public static string obtenerUltimoNroOp() {

            string nroop = "";

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return "";
            }

            SqlCommand com = new SqlCommand("SELECT dbo.Nro_OP(@RucE)", DBSqlServer.Conexion);
            com.CommandType = CommandType.Text;
            com.CommandTimeout = 900;
            com.Parameters.AddWithValue("@RucE", "20100004080");

            SqlDataReader record = com.ExecuteReader();

            if (record.HasRows)
            {
                if (record.Read())
                {
                    nroop = record.GetValue(0).ToString();
                }
            }
            record.Close();
            record.Dispose();

            return nroop;

        }

        public static string obtenerCd_FPC( string codcondicionpago )
        {

            string cd_fpc = "";

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return "";
            }

            SqlCommand com = new SqlCommand("select top 1 cd_fpc from FormaPC where ca01 = @ca01", DBSqlServer.Conexion);
            com.CommandType = CommandType.Text;
            com.CommandTimeout = 900;
            com.Parameters.AddWithValue("@ca01", codcondicionpago);

            SqlDataReader record = com.ExecuteReader();

            if (record.HasRows)
            {
                if (record.Read())
                {
                    cd_fpc = record.GetValue(0).ToString();
                }
            }
            record.Close();
            record.Dispose();

            return cd_fpc;

        }

        public static string obtenerCamMda(string fecha)
        {

            string CamMda = "";

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return "";
            }

            SqlCommand com = new SqlCommand("select top 1 tcvta from tipcam where fectc = @fecha", DBSqlServer.Conexion);
            com.CommandType = CommandType.Text;
            com.CommandTimeout = 900;
            com.Parameters.AddWithValue("@fecha", fecha);

            SqlDataReader record = com.ExecuteReader();

            if (record.HasRows)
            {
                if (record.Read())
                {
                    CamMda = record.GetValue(0).ToString();
                }
            }
            record.Close();
            record.Dispose();

            return CamMda;

        }

        public static string obtenerUsuCrea(string codvendedor)
        {

            string UsuCrea = "";

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return "";
            }

            SqlCommand com = new SqlCommand("select top 1 CA02 from vendedor2 where Cd_Vdr = @codvendedor", DBSqlServer.Conexion);
            com.CommandType = CommandType.Text;
            com.CommandTimeout = 900;
            com.Parameters.AddWithValue("@codvendedor", codvendedor);

            SqlDataReader record = com.ExecuteReader();

            if (record.HasRows)
            {
                if (record.Read())
                {
                    UsuCrea = record.GetValue(0).ToString();
                }
            }
            record.Close();
            record.Dispose();

            return UsuCrea;

        }

        public static string obtenerCd_CC(string codvendedor)
        {

            string Cd_CC = "";

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return "";
            }

            SqlCommand com = new SqlCommand("select top 1 CA03 from vendedor2 where Cd_Vdr = @codvendedor", DBSqlServer.Conexion);
            com.CommandType = CommandType.Text;
            com.CommandTimeout = 900;
            com.Parameters.AddWithValue("@codvendedor", codvendedor);

            SqlDataReader record = com.ExecuteReader();

            if (record.HasRows)
            {
                if (record.Read())
                {
                    Cd_CC = record.GetValue(0).ToString();
                }
            }
            record.Close();
            record.Dispose();

            return Cd_CC;

        }

        public static int obtenerDiasCondicionPago(string codcondicionpago)
        {

            int dias = 0;

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return 0;
            }

            SqlCommand com = new SqlCommand("select top 1 dias from MD_CONDICIONPAGODIAS where codcondicionpago = @codcondicionpago", DBSqlServer.Conexion);
            com.CommandType = CommandType.Text;
            com.CommandTimeout = 900;
            com.Parameters.AddWithValue("@codcondicionpago", codcondicionpago);

            SqlDataReader record = com.ExecuteReader();

            if (record.HasRows)
            {
                if (record.Read())
                {
                    dias = Int32.Parse(record.GetValue(0).ToString());
                }
            }
            record.Close();
            record.Dispose();

            return dias;

        }

        public static int obtenerId_prec(string codproducto)
        {

            int id_prec = 0;

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return 0;
            }

            SqlCommand com = new SqlCommand("select top 1 id_prec from precio where cd_prod = @codproducto", DBSqlServer.Conexion);
            com.CommandType = CommandType.Text;
            com.CommandTimeout = 900;
            com.Parameters.AddWithValue("@codproducto", codproducto);

            SqlDataReader record = com.ExecuteReader();

            if (record.HasRows)
            {
                if (record.Read())
                {
                    id_prec = Int32.Parse(record.GetValue(0).ToString());
                }
            }
            record.Close();
            record.Dispose();

            return id_prec;

        }

        public static string obtenerCliente(string codcliente)
        {

            string cliente = "";

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return "";
            }

            SqlCommand com = new SqlCommand("select top 1 rsocial from cliente2 where cd_clt = @codcliente", DBSqlServer.Conexion);
            com.CommandType = CommandType.Text;
            com.CommandTimeout = 900;
            com.Parameters.AddWithValue("@codcliente", codcliente);

            SqlDataReader record = com.ExecuteReader();

            if (record.HasRows)
            {
                if (record.Read())
                {
                    cliente = record.GetValue(0).ToString();
                }
            }
            record.Close();
            record.Dispose();

            return cliente;

        }

        public static string obtenerProducto(string codproducto)
        {

            string producto = "";

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return "";
            }

            SqlCommand com = new SqlCommand("select top 1 nombre1 from producto2 where cd_prod = @codproducto", DBSqlServer.Conexion);
            com.CommandType = CommandType.Text;
            com.CommandTimeout = 900;
            com.Parameters.AddWithValue("@codproducto", codproducto);

            SqlDataReader record = com.ExecuteReader();

            if (record.HasRows)
            {
                if (record.Read())
                {
                    producto = record.GetValue(0).ToString();
                }
            }
            record.Close();
            record.Dispose();

            return producto;

        }

        public static RespuestaPedido crearPedidoFesepsa(CabeceraPedido cabecera, DetallePedido[] detalles)
        {

            RespuestaPedido response = new RespuestaPedido();
            response.creado = false;
            response.numeroPedido = "0";
            response.numeroPedidoMydealer = cabecera.numeroPedidoMydealer;

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                response.error = DBSqlServer.Respuesta.DescripcionError;
                return response;
            }

            string nombre_log = "PEDIDO_" + cabecera.numeroPedidoMydealer;

            logs.grabarLog(nombre_log, "INICIO PROCESO CREAR PEDIDO", "PEDIDO");


            logs.grabarLog(nombre_log, " ** Cabecera: ", "PEDIDO");

            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(cabecera))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(cabecera);
                logs.grabarLog(nombre_log, name + " => " + value, "PEDIDO");
            }

            logs.grabarLog(nombre_log, " ** Detalle Inicio: ", "PEDIDO");

            bool tiene_detale = false;

            foreach (DetallePedido detalle in detalles)
            {
                tiene_detale = true;

                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(detalle))
                {
                    string name = descriptor.Name;
                    object value = descriptor.GetValue(detalle);
                    logs.grabarLog(nombre_log, name + " => " + value, "PEDIDO");
                }
            }

            logs.grabarLog(nombre_log, " ** Detalle Fin: ", "PEDIDO");

            try
            {
                if(!tiene_detale) throw new ApplicationException("El pedido no tiene detalle de items");

                string Cd_FPC = obtenerCd_FPC(cabecera.codcondicionpago);

                if(string.IsNullOrEmpty(Cd_FPC)) throw new ApplicationException("El cliente no tiene condicion de pago.");

                string CamMda = obtenerCamMda(cabecera.FechaEmison.ToString(DatosEnlace.fecha_moneda));

                if (string.IsNullOrEmpty(CamMda)) throw new ApplicationException("La fecha "+ cabecera.FechaEmison.ToString(DatosEnlace.fecha_moneda) + ", no tiene cambio de moneda.");

                string UsuCrea = obtenerUsuCrea(cabecera.codvendedor);

                if (string.IsNullOrEmpty(UsuCrea)) throw new ApplicationException("Error al obtener el usuario creación.");

                //string Cd_CC = obtenerCd_CC(cabecera.codvendedor);
                string Cd_CC = DatosEnlace.cd_cc;
                string Cd_SC = DatosEnlace.cd_sc;
                string Cd_SS = DatosEnlace.cd_ss;

                if (string.IsNullOrEmpty(Cd_CC)) throw new ApplicationException("Error al obtener el centro de costo.");

                int dias_pago = obtenerDiasCondicionPago(cabecera.codcondicionpago);

                DateTime FecVencimiento = cabecera.FechaEmison.AddDays(dias_pago);

                string[] P_ITEM_DIRECCION_ENTREGA = cabecera.coddireccion.Split('-');

                // Insertar cabecera de pedido

                SqlCommand cmd = new SqlCommand(DatosEnlace.sp_pedido_cab, DBSqlServer.Conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 900;
                cmd.Parameters.AddWithValue("@RucE", "20100004080");
                cmd.Parameters.AddWithValue("@NroOP", obtenerUltimoNroOp());
                cmd.Parameters.AddWithValue("@FecE", cabecera.FechaEmison); // Fecha de emisión de mydealer
                cmd.Parameters.AddWithValue("@Cd_FPC", Cd_FPC); // codcondicionpago - MD_CONDICIONPAGO
                cmd.Parameters.AddWithValue("@Cd_Vdr", cabecera.codvendedor); // codvendedor - MD_VENDEDORES
                cmd.Parameters.AddWithValue("@Cd_Area", "VENTAS");
                cmd.Parameters.AddWithValue("@Cd_Cte", DBNull.Value);
                cmd.Parameters.AddWithValue("@Cd_Clt", cabecera.codcliente); // codcliente - MD_CLIENTES
                cmd.Parameters.AddWithValue("@DirecEnt", cabecera.direccion); // direccion - MD_DIRECCIONCLIENTE
                cmd.Parameters.AddWithValue("@FecEnt", cabecera.FechaEntrega); // fecha de entrega
                cmd.Parameters.AddWithValue("@Obs", cabecera.observacion); // texto
                cmd.Parameters.AddWithValue("@Valor", cabecera.subtotal); // valor total sin IGV - base imponible
                cmd.Parameters.AddWithValue("@TotDsctoP", DBNull.Value);
                cmd.Parameters.AddWithValue("@TotDsctoI", DBNull.Value);
                cmd.Parameters.AddWithValue("@ValorNeto", cabecera.subtotal); // iguaol que la columna valor
                cmd.Parameters.AddWithValue("@INF", DBNull.Value);
                cmd.Parameters.AddWithValue("@DsctoFnzInf_P", DBNull.Value);
                cmd.Parameters.AddWithValue("@DsctoFnzInf_I", DBNull.Value);
                cmd.Parameters.AddWithValue("@INF_Neto", DBNull.Value);
                cmd.Parameters.AddWithValue("@BIM", cabecera.subtotal); // iguaol que la columna valor
                cmd.Parameters.AddWithValue("@DsctoFnzAf_P", DBNull.Value);
                cmd.Parameters.AddWithValue("@DsctoFnzAf_I", DBNull.Value);
                cmd.Parameters.AddWithValue("@BIM_Neto", cabecera.subtotal); // iguaol que la columna valor
                cmd.Parameters.AddWithValue("@IGV", cabecera.totaligv); // Total IGV
                cmd.Parameters.AddWithValue("@Total", cabecera.total); //  Total
                cmd.Parameters.AddWithValue("@Cd_Mda", cabecera.moneda); // MyDealer envia ( 01 SOLES - 02 USD )
                cmd.Parameters.AddWithValue("@CamMda", CamMda);
                cmd.Parameters.AddWithValue("@UsuCrea", UsuCrea);
                cmd.Parameters.AddWithValue("@Id_EstOP", "01");
                cmd.Parameters.AddWithValue("@Cd_Cot", DBNull.Value);
                cmd.Parameters.AddWithValue("@CA01", cabecera.ordenCompra); // ORDEN DE COMPRA DEL CLIENTE, puede ser null

                cmd.Parameters.AddWithValue("@CA02", DBNull.Value);
                cmd.Parameters.AddWithValue("@CA03", DBNull.Value);
                cmd.Parameters.AddWithValue("@CA04", DBNull.Value);
                cmd.Parameters.AddWithValue("@CA05", DBNull.Value);
                cmd.Parameters.AddWithValue("@CA06", DBNull.Value);
                cmd.Parameters.AddWithValue("@CA07", DBNull.Value);
                cmd.Parameters.AddWithValue("@CA08", DBNull.Value);
                cmd.Parameters.AddWithValue("@CA09", DBNull.Value);
                cmd.Parameters.AddWithValue("@CA10", DBNull.Value);
                cmd.Parameters.AddWithValue("@CA11", DBNull.Value);
                cmd.Parameters.AddWithValue("@CA12", DBNull.Value);
                cmd.Parameters.AddWithValue("@CA13", DBNull.Value);
                cmd.Parameters.AddWithValue("@CA14", DBNull.Value);
                cmd.Parameters.AddWithValue("@CA15", DBNull.Value);
                cmd.Parameters.AddWithValue("@CA30", cabecera.numeroPedidoMydealer);
                cmd.Parameters.AddWithValue("@TipAut", 1);
                cmd.Parameters.AddWithValue("@Cd_CC", Cd_CC); 
                cmd.Parameters.AddWithValue("@Cd_SC", Cd_SC);
                cmd.Parameters.AddWithValue("@Cd_SS", Cd_SS);
                cmd.Parameters.AddWithValue("@Percep", DBNull.Value);
                cmd.Parameters.AddWithValue("@FecVencimiento", FecVencimiento); // Fecha de emision + ( dias ( condicion pago ) , vista MD_CONDICIONPAGODIAS )
                cmd.Parameters.AddWithValue("@IB_Percep", 0);
                cmd.Parameters.AddWithValue("@IB_EsImport", 0);
                cmd.Parameters.AddWithValue("@Cliente", cabecera.nombreCliente);
                cmd.Parameters.AddWithValue("@P_ITEM_DIRECENT", DBNull.Value);
                cmd.Parameters.AddWithValue("@P_ISCTOTAL", DBNull.Value);
                cmd.Parameters.AddWithValue("@P_FECHA_CADUCIDAD", FecVencimiento.AddDays(1)); // Fecha de vencimiento mas un día
                cmd.Parameters.AddWithValue("@P_IB_SIN_CADUCAR", 0);
                cmd.Parameters.AddWithValue("@P_PORC_MARGEN", DBNull.Value);
                cmd.Parameters.AddWithValue("@P_AUTORIZACION_LINEA_CREDITO", DBNull.Value);
                cmd.Parameters.AddWithValue("@P_IGV_TASA", 18.00);
                cmd.Parameters.AddWithValue("@P_ITEM_DIRECCION_ENTREGA", P_ITEM_DIRECCION_ENTREGA[1]); // coddireccion - md_direccioncliente (el valor despues del guion)
                cmd.Parameters.AddWithValue("@P_FECHA_FACTURACION", cabecera.FechaEmison); // fecha de emisión
                cmd.Parameters.AddWithValue("@P_SUB_TOTAL", cabecera.subtotal); // iguaol que la columna valor

                SqlParameter Cd_OP_output = new SqlParameter("@Cd_OP", SqlDbType.Char, 10) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(Cd_OP_output);
                SqlParameter msj_output = new SqlParameter("@msj", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(msj_output);

                logs.grabarLog(nombre_log, "INICIO PROCESO EXEC SP CABECERA", "PEDIDO");

                cmd.ExecuteNonQuery();

                logs.grabarLog(nombre_log, "FIN PROCESO EXEC SP CABECERA", "PEDIDO");

                String msj = msj_output.Value.ToString();

                if (!string.IsNullOrEmpty(msj)) throw new ApplicationException(msj);

                String Cd_OP = Cd_OP_output.Value.ToString();

                // Insertar detalle

                foreach (DetallePedido detalle in detalles)
                {

                    logs.grabarLog(nombre_log, "CodigoProducto => " + detalle.codproducto, "PEDIDO");

                    double porcentaje_igv = (detalle.igv / 100);

                    //string nombreProducto = obtenerProducto(detalle.codproducto);

                    SqlCommand cmd2 = new SqlCommand(DatosEnlace.sp_pedido_det, DBSqlServer.Conexion);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.CommandTimeout = 900;
                    cmd2.Parameters.AddWithValue("@RucE", "20100004080");
                    cmd2.Parameters.AddWithValue("@Cd_OP", Cd_OP);
                    cmd2.Parameters.AddWithValue("@Item", detalle.linea); // numero de linea
                    cmd2.Parameters.AddWithValue("@Cd_Prod", detalle.codproducto); // codproducto - md_productos
                    cmd2.Parameters.AddWithValue("@Cd_Srv", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@Descrip", detalle.nombreProducto); // nombre - md_productos
                    cmd2.Parameters.AddWithValue("@ID_UMP", "1");
                    cmd2.Parameters.AddWithValue("@PU", detalle.precioUnitario); // precio unitario sin descuento y sin impuesto
                    cmd2.Parameters.AddWithValue("@Cant", detalle.cantidad); // cantidad
                    cmd2.Parameters.AddWithValue("@Valor", detalle.subtotal); // precio con descuento sin impuesto * cantidad
                    cmd2.Parameters.AddWithValue("@DsctoP", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@DsctoI", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@BIM", detalle.subtotal); // lo mismo que el valor
                    cmd2.Parameters.AddWithValue("@IGV", detalle.totalIgv); // valor * 0.18
                    cmd2.Parameters.AddWithValue("@Total", detalle.total); // total suma del BIM con IGV
                    cmd2.Parameters.AddWithValue("@PENDEnt", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@Cd_Alm", detalle.codbodega); // codbodega - md_bodegas
                    cmd2.Parameters.AddWithValue("@Obs", detalle.observacion); // observacion
                    cmd2.Parameters.AddWithValue("@FecMdf", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@UsuMdf", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@CA01", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@CA02", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@CA03", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@CA04", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@CA05", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@CA06", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@CA07", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@CA08", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@CA09", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@CA10", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@Cd_CC", Cd_CC); // Cabecera
                    cmd2.Parameters.AddWithValue("@Cd_SC", Cd_SC); // Cabecera
                    cmd2.Parameters.AddWithValue("@Cd_SS", Cd_SS); // Cabecera
                    cmd2.Parameters.AddWithValue("@ValVtaUnit", detalle.precioConDescuento); // precio unitario - ( todos los descuento )
                    cmd2.Parameters.AddWithValue("@TotalVtaSD", detalle.precioUnitario); // @PU
                    cmd2.Parameters.AddWithValue("@PrecioUnitSD", (detalle.precioUnitario * detalle.cantidad)); // @PU * Cantidad
                    cmd2.Parameters.AddWithValue("@PercepP", 0);
                    cmd2.Parameters.AddWithValue("@PercepI", 0);
                    cmd2.Parameters.AddWithValue("@TotalNeto", detalle.total); // @Total
                    cmd2.Parameters.AddWithValue("@ValVtaUnitDsctoP", detalle.porcentajeDescuento); // Primer descuento ( 100 - 10 % = 10 )
                    cmd2.Parameters.AddWithValue("@ValVtaUnitDsctoI", detalle.descuentoUnitario); // Precio unitario con su descuento
                    cmd2.Parameters.AddWithValue("@ValorVentaConDscto", detalle.precioConDescuento); // @ValVtaUnit
                    cmd2.Parameters.AddWithValue("@HabDeshab_Percep", 0); // Indica si se ha forzado o no, la percepción, en la Orden de Pedido
                    cmd2.Parameters.AddWithValue("@Id_LP", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@ItemLP", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@Id_Prec", obtenerId_prec(detalle.codproducto)); // select id_prec from precio where cd_prod
                    cmd2.Parameters.AddWithValue("@_CantAumentBon", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@_DepENDe", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@ValVta_Referencial", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@OtrosImpAfectos", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@OtrosImpInafectos", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@IB_ConSol", 0); // Validar
                    cmd2.Parameters.AddWithValue("@CA11", DBNull.Value); // *
                    cmd2.Parameters.AddWithValue("@UsuAutMontos", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@Cantidad_UM_Secundario", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@P_ISC", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@P_ISC_UNITARIO", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@P_CODIGO_ISC", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@P_DESCUENTO_UNITARIO_P_2", 0);
                    cmd2.Parameters.AddWithValue("@P_DESCUENTO_UNITARIO_I_2", 0);
                    cmd2.Parameters.AddWithValue("@P_DESCUENTO_UNITARIO_P_3", 0);
                    cmd2.Parameters.AddWithValue("@P_DESCUENTO_UNITARIO_I_3", 0);
                    cmd2.Parameters.AddWithValue("@P_IGVUNI", (detalle.precioConDescuento * porcentaje_igv)); // @ValVtaUnit * 0.18
                    cmd2.Parameters.AddWithValue("@P_IB_INC_IGV", (detalle.igv > 0 ? 1 : 0 )); // 0 - 1 , si tiene igv o no
                    cmd2.Parameters.AddWithValue("@P_COSTO_MARGEN", 0);
                    cmd2.Parameters.AddWithValue("@P_FACTOR_UM", 1);
                    cmd2.Parameters.AddWithValue("@P_IC_CL", "M");
                    cmd2.Parameters.AddWithValue("@P_CANT_FACTOR", detalle.cantidad); // @Cant
                    cmd2.Parameters.AddWithValue("@P_STOCK", 0);
                    cmd2.Parameters.AddWithValue("@P_PRECIO_UNITARIO_IGV", (detalle.precioConDescuento * porcentaje_igv) + detalle.precioConDescuento); // ( @valvtaunit * 0.18 ) + @valvtaunit 
                    cmd2.Parameters.AddWithValue("@P_DESCUENTO_UNIT", 0);
                    cmd2.Parameters.AddWithValue("@P_ISC_UNIT", 0);
                    cmd2.Parameters.AddWithValue("@P_IB_REDONDEO_ISC", 0);
                    cmd2.Parameters.AddWithValue("@P_IB_BONIFICACION", 0);
                    cmd2.Parameters.AddWithValue("@P_COD_BONIFICACION", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@P_CANT_USO_BONIFICACION", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@P_COD_IAV", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@P_COD_TIPO_IAV", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@P_IB_TRANSFERENCIA_GRATUITA", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@P_CODIGO_LOTE", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@P_NUMERO_LOTE", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@P_ID_SERIAL", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@P_SERIAL", DBNull.Value);
                    cmd2.Parameters.AddWithValue("@P_RECARGO_CONSUMO_I", 0);
                    cmd2.Parameters.AddWithValue("@P_RECARGO_CONSUMO_P", 0);

                    SqlParameter msj_output2 = new SqlParameter("@msj", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };
                    cmd2.Parameters.Add(msj_output2);

                    cmd2.ExecuteNonQuery();
                }

                response.creado = true;
                response.numeroPedido = Cd_OP;

            }
            catch (Exception e)
            {
                response.creado = false;
                response.error = e.Message;
                logs.grabarLog(nombre_log, e.Message, "PEDIDO");
                logs.grabarLog(nombre_log, e.StackTrace, "PEDIDO");
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            logs.grabarLog(nombre_log, "FIN PROCESO CREAR PEDIDO", "PEDIDO");

            return response;

        }

        public static Respuesta crearCliente(String usuario, String tipodoc, String ndoc, String rsocial, String appat, String apmat, String nombres, String ubigeo, String correo, String tipclt, String direccion, String telef1, String telef2)
        {

            Respuesta response = new Respuesta();
            response.Exito = false;
            response.DescripcionError = "";

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            try
            {

                // Obtener el vendedor

                String vendedor = "";

                SqlCommand com1 = new SqlCommand(" SELECT CA02 FROM vendedor2 " +
                    " WHERE Cd_Vdr = @Cd_Vdr ", DBSqlServer.Conexion);
                com1.CommandType = CommandType.Text;
                com1.CommandTimeout = 900;
                com1.Parameters.AddWithValue("@Cd_Vdr", usuario);

                SqlDataReader record1 = com1.ExecuteReader();

                if (record1.HasRows)
                {
                    if (record1.Read())
                    {
                        vendedor = record1.GetValue(0).ToString();
                    }
                }
                record1.Close();
                record1.Dispose();

                // Validar si el cliente existe

                int total = 0;

                SqlCommand com = new SqlCommand(" select count(*) as m1 from cliente2 " +
                    "where NDoc = @ndoc " , DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;
                com.CommandTimeout = 900;
                com.Parameters.AddWithValue("@ndoc", ndoc);

                SqlDataReader record = com.ExecuteReader();

                if (record.HasRows)
                {
                    if (record.Read())
                    {
                        total = Int32.Parse(record.GetValue(0).ToString());
                    }
                }
                record.Close();
                record.Dispose();

                if (total == 0 && vendedor != "")
                {

                    SqlCommand cmd = new SqlCommand("FESEPSA_MNG_CLIENTE2", DBSqlServer.Conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 900;
                    cmd.Parameters.AddWithValue("@OPC", "2");
                    cmd.Parameters.AddWithValue("@USUARIO", vendedor);
                    cmd.Parameters.AddWithValue("@DOC", tipodoc);
                    cmd.Parameters.AddWithValue("@NDOC", ndoc);
                    cmd.Parameters.AddWithValue("@RSOCIAL", rsocial);
                    cmd.Parameters.AddWithValue("@APPAT", appat);
                    cmd.Parameters.AddWithValue("@APMAT", apmat);
                    cmd.Parameters.AddWithValue("@NOMBRES", nombres);
                    cmd.Parameters.AddWithValue("@UBIGEO", ubigeo);
                    cmd.Parameters.AddWithValue("@CORREO", correo);
                    cmd.Parameters.AddWithValue("@TIPCLT", tipclt);
                    cmd.Parameters.AddWithValue("@DIREC", direccion);
                    cmd.Parameters.AddWithValue("@TELEF1", telef1);
                    cmd.Parameters.AddWithValue("@TELEF2", telef2);

                    String res = "";

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {

                        while (dr.Read())
                        {
                            res = dr["Respuesta"].ToString();
                        }

                    }

                    if (res == "TRUE")
                    {
                        response.Exito = true;
                    }
                    else
                    {
                        response.DescripcionError = "Error al crear el cliente";
                    }
                }
                else
                {
                    if (total > 0)
                    {
                        response.DescripcionError = "El cliente existe en el sistema";
                    }
                    else {
                        response.DescripcionError = "El usuario no existe en el sistema";
                    }
                }
            }
            catch (Exception e)
            {
                response.DescripcionError = "Error cliente";
                logs.grabarLog("CREAR_CLIENTE", e.Message);
                logs.grabarLog("CREAR_CLIENTE", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;

        }

        public static Respuesta obtenerRegistros(string tabla, int numCampos, string inicio, string limit, int numdias)
        {
            Respuesta response = new Respuesta();
            response.Exito = false;
            response.MaxRegistros = 0;

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            if ( inicio == "0" ) {
                inicio = "1";
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            try
            {
                string where = obtenerWhereGeneral(numdias, "", "");
                if (numdias == 0)
                    where += " AND indice between " + inicio + " and " + ( ( Int32.Parse(inicio) + Int32.Parse(limit) ) - 1 );

                SqlCommand com = new SqlCommand(" SELECT  * FROM " + tabla + DatosEnlace.sufijo + where, DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;
                com.CommandTimeout = 900;

                SqlDataReader record = com.ExecuteReader();
                if (record.HasRows)
                {
                    response.Exito = true;

                    //response.EntradaRAW = "";
                    //while (record.Read())
                    //{
                    //    for (int i = 0; i < numCampos; i++)
                    //        response.EntradaRAW += record.GetValue(i) + "|";
                    //    response.EntradaRAW += "=|";
                    //}

                    String rows = "";

                    while (record.Read())
                    {
                        rows += "<Registro>";

                        for (int f = 0; f < record.FieldCount; f++)
                        {
                            rows += "<"+ record.GetName(f) + ">";
                            rows += Estandarizador.estandarizarCadena(record.GetValue(f).ToString());
                            rows += "</" + record.GetName(f) + ">";
                        }

                        rows += "</Registro>";
                    }

                    response.Registros = rows;

                    Respuesta resp_cantidad = obtenerCantidadRegistros(tabla, numdias);

                    //response.MaxRegistros = Int32.Parse(resp_cantidad.EntradaRAW.ToString());
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.Exito = false;
                logs.grabarLog(tabla.ToUpper(), e.Message);
                logs.grabarLog(tabla.ToUpper() + "_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }

        public static List<Mantenimiento> obtenerMantenimientos()
        {
            List<Mantenimiento> response = new List<Mantenimiento>();

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            try
            {
                SqlCommand com = new SqlCommand(" SELECT * FROM md_mantenimiento" + DatosEnlace.sufijo + " ", DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                Mantenimiento mantenimiento;
                if (record.HasRows)
                {
                    while (record.Read())
                    {
                        mantenimiento = new Mantenimiento();
                        mantenimiento.NRO_ORDEN = record.GetValue(0).ToString();
                        mantenimiento.ID = record.GetValue(1).ToString();
                        mantenimiento.SERIE = record.GetValue(2).ToString();
                        mantenimiento.MODELO = record.GetValue(3).ToString();
                        mantenimiento.HORAS = record.GetValue(4).ToString();
                        mantenimiento.CANTIDAD = record.GetValue(5).ToString();
                        mantenimiento.ARTICULO = record.GetValue(6).ToString();
                        mantenimiento.DESCRIPCION = record.GetValue(7).ToString();

                        response.Add(mantenimiento);
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.Clear();
                logs.grabarLog("Mantenimiento", e.Message);
                logs.grabarLog("Mantenimiento_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return response;
        }

        public static ConsultaGeneralJsonResp WsConsultaGeneralJson(string campos, string vista, string condicion, string clausulas)
        {
            ConsultaGeneralJsonResp respuesta = new ConsultaGeneralJsonResp();
            respuesta.Estado = false;
            respuesta.DescripcionError = "";
            respuesta.ListaRespuesta = "";

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                respuesta.Estado = false;
            }

            string where = "";

            if (!String.IsNullOrEmpty(condicion))
            {
                where = " WHERE " + condicion;
            }

            try
            {
                // Consulto la deuda total del ente                 
                SqlCommand com = new SqlCommand("SELECT " + campos + " FROM " + vista + " " + where + " " + clausulas, DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();
                if (record.HasRows)
                {
                    List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
                    Dictionary<string, string> row;
                    while (record.Read())
                    {
                        row = new Dictionary<string, string>();
                        for (int f = 0; f < record.FieldCount; f++)
                        {
                            row.Add(record.GetName(f), record.GetValue(f).ToString());
                        }
                        rows.Add(row);
                    }
                    respuesta.Estado = true;
                    var serializer = new JavaScriptSerializer();
                    serializer.MaxJsonLength = Int32.MaxValue;
                    respuesta.ListaRespuesta = serializer.Serialize(rows);
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                respuesta.DescripcionError = e.Message;
                logs.grabarLog("CONSULTAGENERALJSON", e.Message);
                logs.grabarLog("CONSULTAGENERALJSON_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return respuesta;
        }
    }
}