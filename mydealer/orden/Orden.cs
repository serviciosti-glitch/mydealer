using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class Orden
    {
        public static RespuestaOR registrarOrden(CabeceraOR cabecera, DetalleOR[] detalles, AutorizacionOR[] autorizaciones)
        {
            RespuestaOR respuesta = new RespuestaOR();
            respuesta.Estado = 0;
            respuesta.Mensaje = "";

            // Establecer conexion

            DBSqlServerAlterna.ConectaDB();

            if (!DBSqlServerAlterna.Respuesta.Exito)
            {
                respuesta.Estado = 0;
                respuesta.Mensaje = DBSqlServerAlterna.Respuesta.DescripcionError;

                logs.grabarLog("registrarOrden", DBSqlServerAlterna.Respuesta.DescripcionError);

                return respuesta;
            }

            try
            {
                // Validar si el pedido existe

                int total = 0;

                SqlCommand com = new SqlCommand(" SELECT COUNT(*) AS total FROM " +
                    " md_ordencab " +
                    " WHERE srorden = " + cabecera.srorden +
                    " AND keyorganizacion = " + cabecera.keyorganizacion + " ", DBSqlServerAlterna.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                if (record.HasRows)
                {
                    if (record.Read())
                    {
                        total = int.Parse(record.GetValue(0).ToString());
                    }
                }

                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();

                if (total > 0)
                {
                    SqlCommand eliminar_ordencab = new SqlCommand(" DELETE FROM md_ordencab " +
                    " WHERE srorden = " + cabecera.srorden +
                    " AND keyorganizacion = " + cabecera.keyorganizacion, DBSqlServerAlterna.Conexion);
                    eliminar_ordencab.ExecuteNonQuery();

                    SqlCommand eliminar_ordendet = new SqlCommand(" DELETE FROM md_ordendet " +
                    " WHERE srorden = " + cabecera.srorden +
                    " AND keyorganizacion = " + cabecera.keyorganizacion, DBSqlServerAlterna.Conexion);
                    eliminar_ordendet.ExecuteNonQuery();

                    SqlCommand eliminar_ordenaut = new SqlCommand(" DELETE FROM md_ordenautorizacion " +
                    " WHERE srorden = " + cabecera.srorden +
                    " AND keyorganizacion = " + cabecera.keyorganizacion, DBSqlServerAlterna.Conexion);
                    eliminar_ordenaut.ExecuteNonQuery();

                    logs.grabarLog("registrarOrden", "El pedido ( " + cabecera.srorden + " ), ha sido eliminado");
                }

                // Registrar la cabecera

                SqlCommand insertar_orden = new SqlCommand(" INSERT INTO md_ordencab ( srorden,codcliente,fecha,total,codformapago,paisenvio,ciudadenvio,direccionenvio,email,codvendedor,numordenbaan,observaciones,referencia2,subtotal,descuento,impuesto,estado,moneda,codzona,coddivision,fechaentrega,codbodega,numfac,numguia,costo_region,costo_area,costo_departamento,destino,cancelado_sap,numintentos,error_erp,comen_auto,archivo,keyorganizacion,fecha_sincronizacion, loginusuario, referencia1, syp_online, origen ) " +
                " VALUES ( @srorden,@codcliente,@fecha,@total,@codformapago,@paisenvio,@ciudadenvio,@direccionenvio,@email,@codvendedor,@numordenbaan,@observaciones,@referencia2,@subtotal,@descuento,@impuesto,@estado,@moneda,@codzona,@coddivision,@fechaentrega,@codbodega,@numfac,@numguia,@costo_region,@costo_area,@costo_departamento,@destino,@cancelado_sap,@numintentos,@error_erp,@comen_auto,@archivo,@keyorganizacion, GETDATE(), @loginusuario, @referencia1, @syp_online, @origen ) " +
                " ; SELECT CAST(scope_identity() AS int) ", DBSqlServerAlterna.Conexion);

                insertar_orden.Parameters.AddWithValue("@srorden", cabecera.srorden);
                insertar_orden.Parameters.AddWithValue("@codcliente", cabecera.codcliente);
                insertar_orden.Parameters.AddWithValue("@fecha", cabecera.fecha);
                insertar_orden.Parameters.AddWithValue("@total", cabecera.total);
                insertar_orden.Parameters.AddWithValue("@codformapago", cabecera.codformapago);
                insertar_orden.Parameters.AddWithValue("@paisenvio", cabecera.paisenvio);
                insertar_orden.Parameters.AddWithValue("@ciudadenvio", cabecera.ciudadenvio);
                insertar_orden.Parameters.AddWithValue("@direccionenvio", cabecera.direccionenvio);
                insertar_orden.Parameters.AddWithValue("@email", cabecera.email);
                insertar_orden.Parameters.AddWithValue("@codvendedor", cabecera.codvendedor);
                insertar_orden.Parameters.AddWithValue("@numordenbaan", cabecera.numordenbaan);
                insertar_orden.Parameters.AddWithValue("@observaciones", cabecera.observaciones);
                insertar_orden.Parameters.AddWithValue("@referencia2", cabecera.referencia2);
                insertar_orden.Parameters.AddWithValue("@subtotal", cabecera.subtotal);
                insertar_orden.Parameters.AddWithValue("@descuento", cabecera.descuento);
                insertar_orden.Parameters.AddWithValue("@impuesto", cabecera.impuesto);
                insertar_orden.Parameters.AddWithValue("@estado", cabecera.estado);
                insertar_orden.Parameters.AddWithValue("@moneda", cabecera.moneda);
                insertar_orden.Parameters.AddWithValue("@codzona", cabecera.codzona);
                insertar_orden.Parameters.AddWithValue("@coddivision", cabecera.coddivision);
                insertar_orden.Parameters.AddWithValue("@fechaentrega", cabecera.fechaentrega);
                insertar_orden.Parameters.AddWithValue("@codbodega", cabecera.codbodega);
                insertar_orden.Parameters.AddWithValue("@numfac", cabecera.numfac);
                insertar_orden.Parameters.AddWithValue("@numguia", cabecera.numguia);
                insertar_orden.Parameters.AddWithValue("@costo_region", cabecera.costo_region);
                insertar_orden.Parameters.AddWithValue("@costo_area", cabecera.costo_area);
                insertar_orden.Parameters.AddWithValue("@costo_departamento", cabecera.costo_departamento);
                insertar_orden.Parameters.AddWithValue("@destino", cabecera.destino);
                insertar_orden.Parameters.AddWithValue("@cancelado_sap", cabecera.cancelado_sap);
                insertar_orden.Parameters.AddWithValue("@numintentos", cabecera.numintentos);
                insertar_orden.Parameters.AddWithValue("@error_erp", cabecera.error_erp);
                insertar_orden.Parameters.AddWithValue("@comen_auto", cabecera.comen_auto);
                insertar_orden.Parameters.AddWithValue("@archivo", cabecera.archivo);
                insertar_orden.Parameters.AddWithValue("@keyorganizacion", cabecera.keyorganizacion);
                insertar_orden.Parameters.AddWithValue("@loginusuario", cabecera.loginusuario);
                insertar_orden.Parameters.AddWithValue("@referencia1", cabecera.referencia1);
                insertar_orden.Parameters.AddWithValue("@syp_online", cabecera.syp_online);
                insertar_orden.Parameters.AddWithValue("@origen", cabecera.origen);

                int idorden = (int)insertar_orden.ExecuteScalar();

                // Registrar el detalle

                foreach (DetalleOR detalle in detalles)
                {
                    SqlCommand insertar_detalle = new SqlCommand(" INSERT INTO md_ordendet ( idorden ,numlinea ,srorden ,codproducto ,fechaorden ,cantidad ,cantidad_real ,precio ,descuento ,subtotal ,orden ,descuentoval ,impuesto ,total ,fecharequerida ,pieza ,dscto_adi ,fue_enviado ,keyorganizacion ) " +
                    " VALUES ( @idorden,@numlinea,@srorden,@codproducto,@fechaorden,@cantidad,@cantidad_real,@precio,@descuento,@subtotal,@orden,@descuentoval,@impuesto,@total,@fecharequerida,@pieza,@dscto_adi,@fue_enviado,@keyorganizacion ) ", DBSqlServerAlterna.Conexion);

                    insertar_detalle.Parameters.AddWithValue("@idorden", idorden);
                    insertar_detalle.Parameters.AddWithValue("@numlinea", detalle.numlinea);
                    insertar_detalle.Parameters.AddWithValue("@srorden", detalle.srorden);
                    insertar_detalle.Parameters.AddWithValue("@codproducto", detalle.codproducto);
                    insertar_detalle.Parameters.AddWithValue("@fechaorden", detalle.fechaorden);
                    insertar_detalle.Parameters.AddWithValue("@cantidad", detalle.cantidad);
                    insertar_detalle.Parameters.AddWithValue("@cantidad_real", detalle.cantidad_real);
                    insertar_detalle.Parameters.AddWithValue("@precio", detalle.precio);
                    insertar_detalle.Parameters.AddWithValue("@descuento", detalle.descuento);
                    insertar_detalle.Parameters.AddWithValue("@subtotal", detalle.subtotal);
                    insertar_detalle.Parameters.AddWithValue("@orden", detalle.orden);
                    insertar_detalle.Parameters.AddWithValue("@descuentoval", detalle.descuentoval);
                    insertar_detalle.Parameters.AddWithValue("@impuesto", detalle.impuesto);
                    insertar_detalle.Parameters.AddWithValue("@total", detalle.total);
                    insertar_detalle.Parameters.AddWithValue("@fecharequerida", detalle.fecharequerida);
                    insertar_detalle.Parameters.AddWithValue("@pieza", detalle.pieza);
                    insertar_detalle.Parameters.AddWithValue("@dscto_adi", detalle.dscto_adi);
                    insertar_detalle.Parameters.AddWithValue("@fue_enviado", detalle.fue_enviado);
                    insertar_detalle.Parameters.AddWithValue("@keyorganizacion", detalle.keyorganizacion);

                    insertar_detalle.ExecuteNonQuery();
                }

                // Registrar la autorizacion

                foreach (AutorizacionOR autorizacion in autorizaciones)
                {
                    SqlCommand insertar_autorizacion = new SqlCommand(" INSERT INTO md_ordenautorizacion ( idorden,id,srorden,tipo,descripcion,detalle,autorizado,fecha_sys,[user],fecha_autorizacion,usuario_autoriza,notificado,keyorganizacion ) " +
                    " VALUES ( @idorden,@id,@srorden,@tipo,@descripcion,@detalle,@autorizado,@fecha_sys,@user,@fecha_autorizacion,@usuario_autoriza,@notificado,@keyorganizacion ) ", DBSqlServerAlterna.Conexion);

                    insertar_autorizacion.Parameters.AddWithValue("@idorden", idorden);
                    insertar_autorizacion.Parameters.AddWithValue("@id", autorizacion.id);
                    insertar_autorizacion.Parameters.AddWithValue("@srorden", autorizacion.srorden);
                    insertar_autorizacion.Parameters.AddWithValue("@tipo", autorizacion.tipo);
                    insertar_autorizacion.Parameters.AddWithValue("@descripcion", autorizacion.descripcion);
                    insertar_autorizacion.Parameters.AddWithValue("@detalle", autorizacion.detalle);
                    insertar_autorizacion.Parameters.AddWithValue("@autorizado", autorizacion.autorizado);
                    insertar_autorizacion.Parameters.AddWithValue("@fecha_sys", autorizacion.fecha_sys);
                    insertar_autorizacion.Parameters.AddWithValue("@user", autorizacion.user);
                    insertar_autorizacion.Parameters.AddWithValue("@fecha_autorizacion", autorizacion.fecha_autorizacion);
                    insertar_autorizacion.Parameters.AddWithValue("@usuario_autoriza", autorizacion.usuario_autoriza);
                    insertar_autorizacion.Parameters.AddWithValue("@notificado", autorizacion.notificado);
                    insertar_autorizacion.Parameters.AddWithValue("@keyorganizacion", autorizacion.keyorganizacion);

                    insertar_autorizacion.ExecuteNonQuery();
                }

                respuesta.Estado = 1;
                respuesta.Mensaje = "El pedido ( " + cabecera.srorden + " ), ha sido registrado";

                logs.grabarLog("registrarOrden", "El pedido ( " + cabecera.srorden + " ), ha sido registrado");

            }
            catch (Exception e)
            {
                respuesta.Estado = 0;
                respuesta.Mensaje = e.Message;

                logs.grabarLog("registrarOrden", e.Message);
                logs.grabarLog("registrarOrden_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServerAlterna.DesconectaDB();
            }

            return respuesta;
        }
    }
}