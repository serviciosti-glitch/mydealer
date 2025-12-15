using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Xml;

namespace mydealer
{
    public static class Pedidos
    {
        public static RespuestaExistenciaPedido existePedido(int numPedido)
        {
            RespuestaExistenciaPedido response = new RespuestaExistenciaPedido();
            response.ExistePedido = false;
            response.NumeroPedidoSAP = "0";
            response.NumeroPedidoMyDealer = "0";
            response.DetalleError = "";

            string nombre_log = "PEDIDO_" + numPedido;

            logs.grabarLog(nombre_log, "INICIO PROCESO EXISTE PEDIDO", "PEDIDO");

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                response.ExistePedido = false;
                response.DetalleError = DBSqlServer.Respuesta.DescripcionError;
                response.NumeroPedidoSAP = "0";
                response.NumeroPedidoMyDealer = "0";
                return response;
            }

            // ABRIMOS LA CONEXION A LA BASE DE DATOS MD_DIFERENCIAL
            //if ( DBSqlServer.Conexion.State == ConnectionState.Closed )
            //    DBSqlServer.Conexion.Open();

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            try
            {
                SqlCommand com = new SqlCommand(" select numorden, numordenweb, DocNum, tipo " +
                    " from md_pedidos" + DatosEnlace.sufijo + " where numordenweb=" + numPedido, DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                if (record.HasRows)
                {
                    if (record.Read())
                    {
                        response.ExistePedido = true;
                        response.NumeroPedidoSAP = record.GetValue(2).ToString();
                        response.NumeroPedidoMyDealer = record.GetValue(0).ToString();
                        response.Tipo = record.GetValue(3).ToString();
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                response.ExistePedido = false;
                response.DetalleError = e.Message;
                response.NumeroPedidoSAP = "0";
                response.NumeroPedidoMyDealer = "0";
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            logs.grabarLog(nombre_log, "FIN PROCESO EXISTE PEDIDO", "PEDIDO");

            return response;
        }


        /**
         * Ingresar una orden en SAP BO
         * @param cabecera La cabecera de la orden
         * @param detalles Los detalles de la orden
         * @return El XML con la representacion de la respuesta
         */
        public static Respuesta ingresarOrden(CabeceraOrden cabecera, DetalleOrden[] detalles, string nombre_log, string nombre_archivo, string extension_archivo, string base_archivo)
        {

            Respuesta respuesta = new Respuesta();
            respuesta.Exito = false;
            respuesta.CodigoError = "0";
            respuesta.CodigoRespuesta = "";
            respuesta.DescripcionError = "";
            respuesta.EntradaRAW = "";

            int error = 0;
            string mensaje = "";


            logs.grabarLog(nombre_log, "INICIO CONEXION SAP", "PEDIDO");

            DataBase.ConectaDB();

            if (!DataBase.Respuesta.Exito)
            {
                respuesta.CodigoError = DataBase.Respuesta.CodigoError;
                respuesta.CodigoRespuesta = DataBase.Respuesta.CodigoRespuesta;
                respuesta.DescripcionError = DataBase.Respuesta.DescripcionError;
                return respuesta;
            }

            logs.grabarLog(nombre_log, "FIN CONEXION SAP", "PEDIDO");

            try
            {

                SAPbobsCOM.Documents oDoc;

                string destino = "REAL";
                if (cabecera.Pedir_autorizacion.Equals("S"))
                {
                    // si se necesita aprobacion, se grabara como draft.
                    oDoc = (SAPbobsCOM.Documents)DataBase.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts); //oDrafts
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oOrders;
                    destino = "DRAFT";
                }
                else
                {
                    // si el pedido del cliente no necesita ser aprobado se va a la tabla de pedidos
                    oDoc = (SAPbobsCOM.Documents)DataBase.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                }

                //  Consulto al grupo al que pertenece el cliente

                DBSqlServer.ConectaDB();

                SqlCommand com = new SqlCommand(" SELECT TOP 1 b.groupname " +
                " FROM OCRD AS a " +
                " INNER JOIN OCRG AS b ON (a.GroupCode=b.GroupCode) " +
                " WHERE a.cardcode = '" + cabecera.CodigoCliente + "'", DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                string codigoGrupo = null;

                if (record.HasRows)
                {
                    if (record.Read())
                    {
                        codigoGrupo = record.GetValue(0).ToString();
                    }
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();

                DBSqlServer.DesconectaDB();

                //********************************************************************

                oDoc.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Items;
                oDoc.DocDate = DateTime.Parse(cabecera.FechaGeneracionPedido);
                oDoc.DocDueDate = DateTime.Parse(cabecera.FechaEntregaPedido);
                oDoc.CardCode = cabecera.CodigoCliente;
                oDoc.Comments = cabecera.Observaciones;
                oDoc.PaymentGroupCode = cabecera.CodigoFormaPago;

                oDoc.SalesPersonCode = cabecera.CodigoVendedor;
                oDoc.ShipToCode = cabecera.CodigoDireccionEnvio;
                oDoc.Series = cabecera.Series;
                oDoc.DocCurrency = DatosEnlace.moneda;
                oDoc.UserFields.Fields.Item("U_SYP_U_PED_NUM").Value = cabecera.NumeroOrdenWeb;

                if (DatosEnlace.empresa == "ECU")
                {
                    oDoc.UserFields.Fields.Item("U_SYP_ONLINE").Value = cabecera.U_SYP_ONLINE;
                    oDoc.UserFields.Fields.Item("U_FE_ordenCompra").Value = cabecera.U_FE_ordenCompra;

                    if (!String.IsNullOrEmpty(codigoGrupo))
                    {
                        oDoc.UserFields.Fields.Item("U_SYP_TipoCliente").Value = codigoGrupo;
                    }
                    if (!String.IsNullOrEmpty(cabecera.U_FE_codigoAlmacen))
                    {
                        oDoc.UserFields.Fields.Item("U_FE_codigoAlmacen").Value = cabecera.U_FE_codigoAlmacen;
                    }
                }

                if (DatosEnlace.empresa == "LLP")
                {
                    //oDoc.UserFields.Fields.Item("U_ALM_TipoEntrega").Value = "RECOJO EN AGENCIA";
                    //oDoc.UserFields.Fields.Item("U_LLL_OrdCompraCl").Value = "000";

                    oDoc.NumAtCard = (!String.IsNullOrEmpty(cabecera.U_FE_ordenCompra) ? cabecera.U_FE_ordenCompra : cabecera.NumeroOrdenWeb.ToString());


                    oDoc.UserFields.Fields.Item("U_horario_recoge").Value = cabecera.U_horario_recoge;
                    oDoc.UserFields.Fields.Item("U_LLP_PFinal").Value = cabecera.CodigoDireccionEnvio;
                    oDoc.UserFields.Fields.Item("U_LLP_Fec_OCCliente").Value = cabecera.FechaGeneracionPedido;
                    oDoc.UserFields.Fields.Item("U_fecha_entrega").Value = cabecera.FechaEntregaPedido;

                    if (String.IsNullOrEmpty(DatosEnlace.sufijo))
                    {
                        oDoc.UserFields.Fields.Item("U_persona_consig").Value = cabecera.U_persona_consig;
                        //oDoc.UserFields.Fields.Item("U_Division").Value = "VTFER-MD";
                        //oDoc.UserFields.Fields.Item("U_FIN_NORMA_REPARTO").Value = "VTFER-MD";
                        //oDoc.UserFields.Fields.Item("U_Alm_Dist_Entrega").Value = cabecera.U_Alm_Dist_Entrega;
                        oDoc.UserFields.Fields.Item("U_Entrega").Value = cabecera.U_Entrega;
                        oDoc.UserFields.Fields.Item("U_Agencia").Value = cabecera.U_Agencia;
                        oDoc.UserFields.Fields.Item("U_Direccion").Value = cabecera.U_Direccion;
                    }
                    else
                    {
                        // Añadir el log solicitado por Jerson
                        // Cambiar -> select U_CC_VERT_N3 from OHEM where salesPrson = código vendedor
                        string U_Division = getU_Division(cabecera.CodigoVendedor);

                        oDoc.UserFields.Fields.Item("U_Division").Value = U_Division;
                        oDoc.UserFields.Fields.Item("U_FIN_NORMA_REPARTO").Value = U_Division;
                    }

                    oDoc.UserFields.Fields.Item("U_LLP_Penalidad").Value = "No";
                    oDoc.UserFields.Fields.Item("U_CC_PROY").Value = "NO";
                    oDoc.UserFields.Fields.Item("U_tipo_pedido").Value = cabecera.U_tipo_pedido;
                    oDoc.UserFields.Fields.Item("U_LLL_USER").Value = "MYDEALER";

                    int OwnerCode = empleadoPropietario(cabecera.CodigoVendedor);

                    if (OwnerCode != 0)
                    {
                        oDoc.DocumentsOwner = OwnerCode;
                    }
                }


                if (DatosEnlace.aprobacion.Equals("true"))
                {
                    if (cabecera.Pedidoaprobado.Equals("Y")) oDoc.Confirmed = SAPbobsCOM.BoYesNoEnum.tYES;
                    else oDoc.Confirmed = SAPbobsCOM.BoYesNoEnum.tNO;
                }

                //oDoc.AgentCode = cabecera.CodigoCobrador;
                //oDoc.TransportationCode = cabecera.CodigoTransportista;
                //oDoc.DocTotal = cabecera.TotalPedido;
                //oDoc.VatSum = cabecera.TotalGasto;

                // DETALLE LINEAS
                int linea = 0;
                foreach (DetalleOrden detalle in detalles)
                {
                    if (linea > 0) oDoc.Lines.Add();
                    oDoc.Lines.SetCurrentLine(linea);
                    oDoc.Lines.ItemCode = detalle.CodigoProducto;
                    oDoc.Lines.Quantity = detalle.CantidadProducto;
                    oDoc.Lines.UnitPrice = detalle.PrecioProducto;

                    //oDoc.Lines.DiscountPercent = detalle.PorcentajeDescuento;
                    double dsct_total = detalle.Dscto_adicional; // +detalle.PorcentajeDescuento;
                    oDoc.Lines.DiscountPercent = dsct_total;

                    if (String.IsNullOrEmpty(DatosEnlace.sufijo))
                    {
                        oDoc.Lines.WarehouseCode = detalle.Wsc;
                    }
                    else
                    {
                        oDoc.Lines.WarehouseCode = "01";
                    }

                    if (DatosEnlace.empresa == "ECU")
                    {
                        oDoc.Lines.UserFields.Fields.Item("U_SYP_DSCTORI").Value = detalle.PorcentajeDescuento;
                    }

                    if (DatosEnlace.empresa == "LLP")
                    {
                        oDoc.Lines.UserFields.Fields.Item("U_SYP_TEntrega").Value = "001";
                        oDoc.Lines.UserFields.Fields.Item("U_lll_fecprog").Value = cabecera.FechaEntregaPedido;
                        oDoc.Lines.TaxCode = detalle.Taxcode;
                        oDoc.Lines.SalesPersonCode = cabecera.CodigoVendedor;
                    }

                    if (DatosEnlace.empresa == "ECU" || DatosEnlace.empresa == "DKS")
                    {
                        if (detalle.PorcIva > 0)
                        {
                            oDoc.Lines.DeferredTax = SAPbobsCOM.BoYesNoEnum.tYES;
                        }
                    }

                    oDoc.Lines.VatGroup = detalle.Taxcode;
                    //oDoc.Lines.LineTotal = detalle.TotalLinea;
                    //oDoc.Lines.Weight1 = detalle.Peso_total;
                                        
                    oDoc.Lines.COGSCostingCode = cabecera.costo_region;
                    oDoc.Lines.CostingCode = cabecera.costo_region;
                    oDoc.Lines.CostingCode2 = cabecera.costo_area;
                    oDoc.Lines.CostingCode3 = cabecera.costo_departamento;
                    
                    linea++;
                }

                /*if (DatosEnlace.paisSistema.Equals("EC")) {
                    oDoc.Expenses.ExpenseCode = 1;
                    oDoc.Expenses.LineTotal = cabecera.TotalGasto; // Estandarizador.obtenerNumeroConDecimales()
                    oDoc.Expenses.VatGroup = "IVA";
                }*/

                logs.grabarLog(nombre_log, "INICIO CREACION PEDIDO SAP", "PEDIDO");

                error = oDoc.Add();

                logs.grabarLog(nombre_log, "FIN CREACION PEDIDO SAP", "PEDIDO");

                if (error != 0)
                {
                    DataBase.Company.GetLastError(out error, out mensaje);
                    throw new Exception("Error en " + destino + " (orden " + cabecera.NumeroOrdenWeb + ") # " + error + ", desc: " + mensaje);
                }
                else
                {
                    string codigoSAP = DataBase.Company.GetNewObjectKey();
                    RespuestaExistenciaPedido res = existePedido(cabecera.NumeroOrdenWeb);

                    if (!String.IsNullOrEmpty(nombre_archivo))
                    {
                        logs.grabarLog(nombre_log, "INICIO ADJUNTAR ARCHIVO SAP", "PEDIDO");

                        guardarAdjuntoPedido(nombre_archivo, extension_archivo, base_archivo, cabecera.NumeroOrdenWeb, int.Parse(codigoSAP), cabecera.Pedir_autorizacion, nombre_log);

                        logs.grabarLog(nombre_log, "FIN ADJUNTAR ARCHIVO SAP", "PEDIDO");
                    }

                    respuesta.Exito = true;
                    respuesta.CodigoRespuesta = res.NumeroPedidoSAP;

                    logs.grabarLog(nombre_log, "Pedido en " + destino + " creado (orden " + cabecera.NumeroOrdenWeb + ") # " + codigoSAP, "PEDIDO");
                }

                oDoc = null;
            }
            catch (Exception e)
            {
                respuesta.CodigoError = error.ToString();
                respuesta.CodigoRespuesta = "PED001";
                respuesta.DescripcionError = e.Message;

                logs.grabarLog(nombre_log, e.Message, "PEDIDO");
                logs.grabarLog(nombre_log, e.StackTrace, "PEDIDO");
            }
            finally
            {
                DataBase.DesconectaDB();
            }

            return respuesta;
        }

        public static void guardarAdjuntoPedido(string nombre_archivo, string extension_archivo, string base_archivo, int orden_web, int codigo_sap, string pedir_autorizacion, string nombre_log)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "archivos";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string ruta_archivo = path + "/" + nombre_archivo + "." + extension_archivo;
            Byte[] bytes = Convert.FromBase64String(base_archivo);
            File.WriteAllBytes(ruta_archivo, bytes);

            SAPbobsCOM.Documents oNewDoc;
            SAPbobsCOM.Attachments2 oAtt = (SAPbobsCOM.Attachments2)DataBase.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2);

            if (pedir_autorizacion.Equals("S"))
            {
                // si se necesita aprobacion, se grabara como draft.
                oNewDoc = (SAPbobsCOM.Documents)DataBase.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);
            }
            else
            {
                oNewDoc = (SAPbobsCOM.Documents)DataBase.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
            }

            oNewDoc.GetByKey(codigo_sap);

            int file_error;
            string file_mensaje;

            // En caso que el documento tenga adjuntos, le puedo añadir mas
            if (oAtt.GetByKey(oNewDoc.AttachmentEntry))
            {
                // Agregar archivos
                oAtt.Lines.Add();
                oAtt.Lines.FileName = nombre_archivo;
                oAtt.Lines.FileExtension = extension_archivo;
                oAtt.Lines.SourcePath = path;
                oAtt.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES;

                if (oAtt.Update() != 0)
                {
                    DataBase.Company.GetLastError(out file_error, out file_mensaje);
                    logs.grabarLog(nombre_log, "NO ACTUALIZO ORDEN # " + orden_web + " ( NroError " + file_error + " ) : " + file_mensaje, "PEDIDO");
                }

            }
            else
            {

                oAtt.Lines.Add();
                oAtt.Lines.FileName = nombre_archivo;
                oAtt.Lines.FileExtension = extension_archivo;
                oAtt.Lines.SourcePath = path;
                oAtt.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES;
                int iAttEntry = -1;
                if (oAtt.Add() == 0)
                {
                    iAttEntry = int.Parse(DataBase.Company.GetNewObjectKey());
                    //Assign the attachment to the GR object (GR is my SAPbobsCOM.Documents object)
                    oNewDoc.AttachmentEntry = iAttEntry;
                    oNewDoc.Update();
                }
                else
                {
                    DataBase.Company.GetLastError(out file_error, out file_mensaje);
                    logs.grabarLog(nombre_log, "NO AGREGO ORDEN # " + orden_web + " ( NroError " + file_error + " ) : " + file_mensaje, "PEDIDO");
                }
            }

            // Eliminar el archivo

            File.Delete(ruta_archivo);

        }

        public static List<DetalleXml> StringToList(string XML)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(XML);

            XmlNodeList items = xml.GetElementsByTagName("items");
            XmlNodeList item = ((XmlElement)items[0]).GetElementsByTagName("item");
            List<DetalleXml> list = new List<DetalleXml>();

            foreach (XmlElement nodo in item)
            {
                DetalleXml detalle = new DetalleXml();
                detalle.CantidadProducto = XmlConvert.ToDouble(nodo.GetElementsByTagName("CantidadProducto")[0].InnerText);
                detalle.CodigoDocumentoRelacionado = nodo.GetElementsByTagName("CodigoDocumentoRelacionado")[0].InnerText;
                detalle.CodigoProducto = nodo.GetElementsByTagName("CodigoProducto")[0].InnerText;
                detalle.NumeroLineaDetalle = XmlConvert.ToInt32(nodo.GetElementsByTagName("NumeroLineaDetalle")[0].InnerText);
                detalle.PorcentajeDescuento = XmlConvert.ToDouble(nodo.GetElementsByTagName("PorcentajeDescuento")[0].InnerText);
                detalle.PrecioProducto = XmlConvert.ToDouble(nodo.GetElementsByTagName("PrecioProducto")[0].InnerText);
                detalle.TotalLinea = XmlConvert.ToDouble(nodo.GetElementsByTagName("TotalLinea")[0].InnerText);
                detalle.Wsc = nodo.GetElementsByTagName("Wsc")[0].InnerText;
                detalle.Detalle_descuento = nodo.GetElementsByTagName("Detalle_descuento")[0].InnerText;
                detalle.Detalle_promocion = nodo.GetElementsByTagName("Detalle_promocion")[0].InnerText;
                detalle.Promocion_estado = nodo.GetElementsByTagName("Promocion_estado")[0].InnerText;
                detalle.Piezas = XmlConvert.ToInt32(nodo.GetElementsByTagName("Piezas")[0].InnerText);
                detalle.Peso = XmlConvert.ToDouble(nodo.GetElementsByTagName("Peso")[0].InnerText);
                detalle.Peso_total = XmlConvert.ToDouble(nodo.GetElementsByTagName("Peso_total")[0].InnerText);
                detalle.Taxcode = nodo.GetElementsByTagName("TaxCode")[0].InnerText;
                try
                {
                    detalle.PorcIva = XmlConvert.ToDouble(nodo.GetElementsByTagName("PorcIva")[0].InnerText);
                    detalle.Impuesto = XmlConvert.ToDouble(nodo.GetElementsByTagName("Impuesto")[0].InnerText);
                    detalle.Dscto_adicional = XmlConvert.ToDouble(nodo.GetElementsByTagName("DsctoAdic")[0].InnerText);
                }
                catch (Exception e)
                {
                    detalle.PorcIva = 0;
                    logs.grabarLog("PEDIDO", e.Message);
                    logs.grabarLog("PEDIDO_DEBUG", e.StackTrace);
                }

                list.Add(detalle);
            }


            return list;
        }

        public static Respuesta obtenerCabeceraOrden(int numorden, int numordenweb)
        {
            Respuesta response = new Respuesta();

            RespuestaExistenciaPedido ped = existePedido(numordenweb);

            DataBase.ConectaDB();
            if (!DataBase.Respuesta.Exito)
            {
                response.Exito = false;
                response.CodigoError = DataBase.Respuesta.CodigoError;
                response.CodigoRespuesta = DataBase.Respuesta.CodigoRespuesta;
                response.DescripcionError = DataBase.Respuesta.DescripcionError;
                return response;
            }

            SAPbobsCOM.Documents oDoc = (SAPbobsCOM.Documents)DataBase.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);

            bool retornar = false;
            try
            {
                if (!ped.ExistePedido)
                {
                    throw new Exception("");
                }

                if (ped.Tipo.Equals("O"))
                {
                    oDoc = (SAPbobsCOM.Documents)DataBase.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);

                    if (oDoc.GetByKey(int.Parse(ped.NumeroPedidoSAP)) != true)
                        throw new Exception("");
                }
                else
                {
                    oDoc = (SAPbobsCOM.Documents)DataBase.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts); //oDrafts
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oOrders;

                    if (oDoc.GetByKey(int.Parse(ped.NumeroPedidoSAP)) != true)
                        throw new Exception("");
                }
                retornar = false;
            }
            catch (Exception e)
            {
                response.Exito = false;
                response.CodigoError = "CAB002";
                response.CodigoRespuesta = "CAB002";
                response.DescripcionError = "PEDIDO NO ENCONTRADO.";
                // response;
                retornar = true;
                logs.grabarLog("PEDIDO", e.Message);
                logs.grabarLog("PEDIDO_DEBUG", e.StackTrace);
            }

            if (retornar)
            {
                DataBase.DesconectaDB();
                return response;
            }

            try
            {
                CabeceraOrden cab = new CabeceraOrden();
                cab.Aprobadoventa = "";
                cab.CodigoCliente = oDoc.CardCode;
                cab.CodigoCobrador = oDoc.AgentCode;
                cab.CodigoDireccionEnvio = oDoc.ShipToCode;
                cab.CodigoFormaPago = oDoc.PaymentGroupCode;
                cab.CodigoMotivoReclamo = "";
                cab.CodigoServicioCliente = "";
                cab.CodigoTransportista = "";
                cab.CodigoVendedor = oDoc.SalesPersonCode;
                cab.Codmotivonoaprobacion = "";
                cab.Descprioridad = "";
                cab.Detallemotivoaprobacion = "";
                cab.FechaEntregaPedido = oDoc.DocDueDate.ToString();
                cab.FechaGeneracionPedido = oDoc.DocDate.ToString();
                cab.NumeroOrdenPedidoFisico = "";
                cab.NumeroOrden = oDoc.DocEntry;
                cab.NumeroOrdenWeb = (int)oDoc.UserFields.Fields.Item("U_ita_ped_num").Value;
                cab.NumeroReclamo = "";
                cab.Observaciones = oDoc.Comments;
                cab.Pedidoaprobado = (oDoc.Confirmed.Equals(SAPbobsCOM.BoYesNoEnum.tYES) ? "Y" : "N");
                cab.Pedir_autorizacion = oDoc.UserFields.Fields.Item("U_ita_ped_aut").Value.ToString();
                cab.Ruta_logistica = oDoc.UserFields.Fields.Item("U_ita_rut_log").Value.ToString();
                cab.Ruta_secuencia = (int)oDoc.UserFields.Fields.Item("U_ita_rut_sec").Value;
                cab.Subotal = 0;
                cab.Tipo_pedido = oDoc.UserFields.Fields.Item("U_ita_ped_tipo").Value.ToString();
                cab.TotalGasto = oDoc.VatSum;
                cab.TotalPedido = oDoc.DocTotal;
                cab.Valor_descto = 0;

                response.Exito = true;
                response.CodigoError = "";
                response.DescripcionError = "";
                response.CodigoRespuesta = "OK";
                // response.EntradaRAW = cab;

                XmlSerializer xmlSerializer = new XmlSerializer(cab.GetType());
                StringWriter textWriter = new StringWriter();

                xmlSerializer.Serialize(textWriter, cab);
                response.EntradaRAW = textWriter.ToString();
                oDoc = null;
                cab = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.Exito = false;
                response.DescripcionError = e.Message;
                response.CodigoError = "CAB001";
                response.CodigoRespuesta = "CAB001";
                response.EntradaRAW = "";
            }
            finally
            {
                DataBase.DesconectaDB();
            }

            return response;
        }

        public static Respuesta obtenerDetalleOrden(int numorden, int numordenweb)
        {
            Respuesta response = new Respuesta();

            RespuestaExistenciaPedido ped = existePedido(numordenweb);


            DataBase.ConectaDB();
            if (!DataBase.Respuesta.Exito)
            {
                response.Exito = false;
                response.CodigoError = DataBase.Respuesta.CodigoError;
                response.CodigoRespuesta = DataBase.Respuesta.CodigoRespuesta;
                response.DescripcionError = DataBase.Respuesta.DescripcionError;
                return response;
            }

            SAPbobsCOM.Documents oDoc = (SAPbobsCOM.Documents)DataBase.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);


            bool retornar = false;
            try
            {
                if (!ped.ExistePedido)
                {
                    throw new Exception("");
                }

                if (ped.Tipo.Equals("O"))
                {
                    oDoc = (SAPbobsCOM.Documents)DataBase.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);

                    if (oDoc.GetByKey(int.Parse(ped.NumeroPedidoSAP)) != true)
                        throw new Exception("");
                }
                else
                {
                    oDoc = (SAPbobsCOM.Documents)DataBase.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts); //oDrafts
                    oDoc.DocObjectCode = SAPbobsCOM.BoObjectTypes.oOrders;

                    if (oDoc.GetByKey(int.Parse(ped.NumeroPedidoSAP)) != true)
                        throw new Exception("");
                }
                retornar = false;
            }
            catch (Exception e)
            {
                response.Exito = false;
                response.CodigoError = "CAB002";
                response.CodigoRespuesta = "CAB002";
                response.DescripcionError = "PEDIDO NO ENCONTRADO.";
                retornar = true;
                logs.grabarLog("PEDIDO", e.Message);
                logs.grabarLog("PEDIDO_DEBUG", e.StackTrace);
            }

            if (retornar)
            {
                DataBase.DesconectaDB();
                return response;
            }

            try
            {
                List<DetalleXml> det = new List<DetalleXml>();

                int numItems = oDoc.Lines.Count;


                DetalleXml detalle;

                for (int i = 0; i < numItems; i++)
                {
                    detalle = new DetalleXml();
                    oDoc.Lines.SetCurrentLine(i);

                    detalle.CodigoProducto = oDoc.Lines.ItemCode;
                    detalle.CantidadProducto = oDoc.Lines.Quantity;
                    detalle.CodigoDocumentoRelacionado = "";
                    detalle.Codunidadmedida = "";
                    detalle.Detalle_descuento = oDoc.Lines.UserFields.Fields.Item("U_ita_des_num").Value.ToString();
                    detalle.Detalle_promocion = oDoc.Lines.UserFields.Fields.Item("U_ita_pro_num").Value.ToString();
                    detalle.NumeroLineaDetalle = i;
                    detalle.Peso = (double)oDoc.Lines.UserFields.Fields.Item("U_ita_pes_std").Value;
                    detalle.Peso_total = (double)oDoc.Lines.Weight1;
                    detalle.Piezas = (int)oDoc.Lines.UserFields.Fields.Item("U_ita_ped_pza").Value;
                    detalle.PorcentajeDescuento = oDoc.Lines.DiscountPercent;
                    if (oDoc.Lines.DeferredTax == SAPbobsCOM.BoYesNoEnum.tYES)
                        detalle.PorcIva = 12;
                    else
                        detalle.PorcIva = 0;
                    detalle.PrecioProducto = oDoc.Lines.UnitPrice;
                    detalle.Promocion_estado = oDoc.Lines.UserFields.Fields.Item("U_ita_ped_est").Value.ToString();
                    detalle.Subtotal = detalle.PrecioProducto * detalle.CantidadProducto;
                    detalle.TotalLinea = oDoc.Lines.LineTotal;
                    detalle.Valor_dscto = Math.Round(detalle.Subtotal * detalle.PorcentajeDescuento / 100, 2);
                    detalle.Wsc = oDoc.Lines.WarehouseCode;

                    det.Add(detalle);
                }
                /*
                 vatsum es el valor del iva por cada linea
    [15:00:30] Silvia Vivar: linetotal es el total de cada línea ya con descuento e iva
    [15:00:51] Silvia Vivar: podria calcularlo de la siguiente manera
    [15:06:13] Silvia Vivar: el campo pricebefdi es el precio del item antes del descuento aplicado
    [15:06:39] Silvia Vivar: el campo price, ya contiene el precio menos el descuento
    [15:07:23] Silvia Vivar: por lo tanto quantity *  price es el subtotal por línea
                 */

                response.Exito = true;
                response.CodigoError = "";
                response.DescripcionError = "";
                response.CodigoRespuesta = "OK";
                // response.EntradaRAW = cab;

                XmlSerializer xmlSerializer = new XmlSerializer(det.GetType());
                StringWriter textWriter = new StringWriter();

                xmlSerializer.Serialize(textWriter, det);
                response.EntradaRAW = textWriter.ToString();
                oDoc = null;
                det = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                response.Exito = false;
                response.DescripcionError = e.Message;
                response.CodigoError = "CAB001";
                response.CodigoRespuesta = "CAB001";
                response.EntradaRAW = "";
            }
            finally
            {
                DataBase.DesconectaDB();
            }

            return response;
        }

        public static string getU_Division(int slpcode)
        {
            string respuesta = "";

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return "";
            }

            try
            {
                // select U_CC_VERT_N3 from OHEM where salesPrson
                SqlCommand com = new SqlCommand(" SELECT TOP 1 U_CC_VERT_N3 " +
                    " FROM OHEM WHERE salesPrson = " + slpcode, DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                if (record.HasRows)
                {
                    if (record.Read())
                    {
                        respuesta = record.GetValue(0).ToString();
                    }
                }

                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                respuesta = "";
                logs.grabarLog("getU_Division", e.Message);
                logs.grabarLog("getU_Division_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return respuesta;
        }

        public static int empleadoPropietario(int slpcode)
        {
            int respuesta = 0;

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return respuesta;
            }

            try
            {
                SqlCommand com = new SqlCommand(" SELECT TOP 1 empid " +
                    " FROM md_propietarios" + DatosEnlace.sufijo + " WHERE slpcode = " + slpcode, DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                if (record.HasRows)
                {
                    if (record.Read())
                    {
                        respuesta = int.Parse(record.GetValue(0).ToString());
                    }
                }

                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                respuesta = 0;
                logs.grabarLog("empleadoPropietario", e.Message);
                logs.grabarLog("empleadoPropietario_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return respuesta;
        }
    }
}