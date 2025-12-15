using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;

namespace mydealer
{
    /// <summary>
    /// Summary description for WSIntegracion
    /// </summary>
    [WebService(Namespace = "http://michael7x.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WSIntegracion : System.Web.Services.WebService
    {
        [WebMethod(Description = "Permite ingresar una orden MyDealer en el ERP")]
        public RespuestaPedido crearPedidoFesepsa(CabeceraPedido cabecera, DetallePedido[] detalles)
        {
            RespuestaPedido respuesta = General.existePedidoFesepsa(cabecera.numeroPedidoMydealer);

            if (respuesta.creado)
            {
                return respuesta;
            }
            else
            {
                return General.crearPedidoFesepsa(cabecera, detalles);
            }
        }

        [WebMethod(Description = "Permite verificar si el pedido existe")]
        public RespuestaPedido existePedidoFesepsa(int numeroPedidoMydealer)
        {
            return General.existePedidoFesepsa(numeroPedidoMydealer);
        }        

        [WebMethod(Description = "Permite crear una direccion")]
        public Respuesta crearDireccion(String ndoc, String direcent, String ubigeo)
        {
            return General.crearDireccion(ndoc, direcent, ubigeo);
        }

        [WebMethod(Description = "Permite crear un cliente")]
        public Respuesta crearCliente(String usuario, String tipodoc, String ndoc, String rsocial, String appat, String apmat, String nombres, String ubigeo, String correo, String tipclt, String direccion, String telef1, String telef2)
        {
            return General.crearCliente(usuario, tipodoc, ndoc, rsocial, appat, apmat, nombres, ubigeo, correo, tipclt, direccion, telef1, telef2);
        }

        [WebMethod(Description = "Permite obtener las cantidades en la tabla que pasemos a consultar")]
        public Respuesta obtenerCantidadRegistros(string tabla, int numdias)
        {
            return General.obtenerCantidadRegistros(tabla, numdias);
        }

        [WebMethod(Description = "Permite obtener los registros de la tabla que pasemos a consultar")]
        public Respuesta obtenerRegistros(string tabla, int numCampos, string inicio, string limit, int numdias)
        {
            return General.obtenerRegistros(tabla, numCampos, inicio, limit, numdias);
        }

        //public Respuesta ingresarOrden(CabeceraOrden cabecera, DetalleOrden[] detalles, string nombre_archivo, string extension_archivo, string base_archivo)
        //{
        //    Respuesta respuesta = new Respuesta();

        //    string nombre_log = "PEDIDO_" + cabecera.NumeroOrdenWeb;

        //    logs.grabarLog(nombre_log, "INICIO PROCESO CREAR PEDIDO", "PEDIDO");

        //    RespuestaExistenciaPedido existe = Pedidos.existePedido(cabecera.NumeroOrdenWeb);
        //    if (existe.ExistePedido)
        //    {
        //        respuesta.CodigoError = "1";
        //        respuesta.CodigoRespuesta = existe.NumeroPedidoSAP;
        //        respuesta.DescripcionError = "Pedido existe" + " # " + existe.NumeroPedidoSAP;
        //        respuesta.Exito = false;
        //        logs.grabarLog(nombre_log, "Pedido ya Existe (Orden " + existe.NumeroPedidoMyDealer + ") # " + existe.NumeroPedidoSAP, "PEDIDO");
        //    }
        //    else
        //    {

        //        logs.grabarLog(nombre_log, " ** Cabecera: ", "PEDIDO");

        //        foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(cabecera))
        //        {
        //            string name = descriptor.Name;
        //            object value = descriptor.GetValue(cabecera);
        //            logs.grabarLog(nombre_log, name + " => " + value, "PEDIDO");
        //        }

        //        if (!String.IsNullOrEmpty(nombre_archivo))
        //        {
        //            logs.grabarLog(nombre_log, " ** Archivo: " + nombre_archivo, "PEDIDO");
        //        }

        //        logs.grabarLog(nombre_log, " ** Detalle Inicio: ", "PEDIDO");

        //        foreach (DetalleOrden detalle in detalles)
        //        {
        //            logs.grabarLog(nombre_log, "CodigoProducto => " + detalle.CodigoProducto, "PEDIDO");
        //            logs.grabarLog(nombre_log, "CantidadProducto => " + detalle.CantidadProducto, "PEDIDO");
        //            logs.grabarLog(nombre_log, "PrecioProducto => " + detalle.PrecioProducto, "PEDIDO");
        //            logs.grabarLog(nombre_log, "Dscto_adicional => " + detalle.Dscto_adicional, "PEDIDO");
        //            logs.grabarLog(nombre_log, "Wsc => " + detalle.Wsc, "PEDIDO");
        //            logs.grabarLog(nombre_log, "PorcentajeDescuento => " + detalle.PorcentajeDescuento, "PEDIDO");
        //            logs.grabarLog(nombre_log, "Taxcode => " + detalle.Taxcode, "PEDIDO");
        //            logs.grabarLog(nombre_log, "PorcIva => " + detalle.PorcIva, "PEDIDO");
        //        }

        //        logs.grabarLog(nombre_log, " ** Detalle Fin: ", "PEDIDO");

        //        respuesta = Pedidos.ingresarOrden(cabecera, detalles, nombre_log, nombre_archivo, extension_archivo, base_archivo);
        //    }

        //    logs.grabarLog(nombre_log, "FIN PROCESO CREAR PEDIDO", "PEDIDO");

        //    return respuesta;
        //}

    }
}
