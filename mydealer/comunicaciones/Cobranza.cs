using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace mydealer
{
    public class Cobranza
    {
        public static RespuestaExistenciaPedido existeCobranza(string numCobranza)
        {
            RespuestaExistenciaPedido response = new RespuestaExistenciaPedido();
            response.ExistePedido = false;
            response.NumeroPedidoSAP = "0";
            response.NumeroPedidoMyDealer = "0";
            response.DetalleError = "";

            DataBase.ConectaDB();
            if (!DataBase.Respuesta.Exito)
            {
                response.ExistePedido = true;
                response.DetalleError = DataBase.Respuesta.DescripcionError;
                response.NumeroPedidoSAP = "0";
                response.NumeroPedidoMyDealer = "0";
                logs.grabarLog("DATA", DataBase.Respuesta.CodigoError + " :: " + DataBase.Respuesta.DescripcionError);
                return response;
            }

            // CONSULTAR A LA VISTA POR MEDIO DEL NUMERO DE PEDIDO DE MYDEALER.
            try
            {
                SAPbobsCOM.Recordset record;
                record = (SAPbobsCOM.Recordset)DataBase.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                record.DoQuery("select DocEntry, U_ita_cob_num, DocNum from ORCT where U_ita_cob_num='" + numCobranza + "' ");
                int y = record.RecordCount;
                if (y > 0)
                {
                    record.MoveFirst();
                    response.ExistePedido = true;
                    response.NumeroPedidoSAP = record.Fields.Item("DocEntry").Value.ToString();
                    response.NumeroPedidoMyDealer = record.Fields.Item("U_ita_cob_num").Value.ToString();
                }

            }
            catch (Exception e)
            {
                response.ExistePedido = true;
                response.DetalleError = e.Message;
                response.NumeroPedidoSAP = "0";
                response.NumeroPedidoMyDealer = "0";
                logs.grabarLog("V_COBRANZA", e.Message);
                logs.grabarLog("V_COBRANZA_DEBUG", e.StackTrace);
            }

            return response;
        }

        /**
         * Permite ingresar una cobranza en SBO
         * @param cabeceraPagos la cabecera comun de todos los pagos generados
         * @param documentos La lista de documentos a ingresar
         * @param pagos La lista de pagos a ingresar
         * @return El XML con la representacion de la respuesta
         */
        public static List<String> ingresarCobranza(SeccionPagosEfectivo cabeceraPagos, List<SeccionDocumentos> documentos, List<Pago> pagos)
        {
            List<String> response = new List<String>();

            DataBase.ConectaDB();
            if (!DataBase.Respuesta.Exito)
            {
                response.Add("Error");
                response.Add(DataBase.Respuesta.CodigoError);
                response.Add(DataBase.Respuesta.CodigoRespuesta);
                response.Add(DataBase.Respuesta.DescripcionError);
                logs.grabarLog("DATA", DataBase.Respuesta.CodigoError + " :: " + DataBase.Respuesta.DescripcionError);
                return response;
            }

            int error = 0; string mensaje = "";

            SAPbobsCOM.Payments oDoc = (SAPbobsCOM.Payments)DataBase.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments);

            try
            {
                oDoc.DocDate = DateTime.Parse(cabeceraPagos.FechaDocumento);
                oDoc.CardCode = cabeceraPagos.CodigoCliente;
                oDoc.Reference1 = cabeceraPagos.Observaciones1;
                oDoc.Reference2 = cabeceraPagos.Observaciones2;
                oDoc.DueDate = DateTime.Parse(cabeceraPagos.FechaVencimiento);
                oDoc.CounterReference = cabeceraPagos.NumeroFisico;
                oDoc.Remarks = cabeceraPagos.NumeroMBW;

                //oDoc.UserFields.Fields.Item("U_ita_cob_num").Value = cabeceraPagos.NumeroMBW;
                //oDoc.UserFields.Fields.Item("U_ita_cod_cob").Value = cabeceraPagos.CodigoCobrador;
                oDoc.Series = Int32.Parse(cabeceraPagos.Serie);


                if (documentos != null && documentos.Count() > 0)
                {
                    //oDoc.Invoices.
                    int linea = 0;
                    foreach (SeccionDocumentos documento in documentos)
                    {
                        if (linea > 0) oDoc.Invoices.Add();
                        oDoc.Invoices.SetCurrentLine(linea);
                        oDoc.Invoices.DocEntry = documento.NumeroDocumento;
                        oDoc.Invoices.SumApplied = documento.Valor;
                        oDoc.Invoices.DocLine = documento.NumeroCuota;
                        oDoc.Invoices.InstallmentId = documento.NumeroCuotaSAP;

                        switch (documento.TipoDocumento)
                        {
                            case "FAC":
                                oDoc.Invoices.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_Invoice; // 13
                                break;

                            case "NTD":
                                oDoc.Invoices.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_CredItnote; // 14
                                break;

                            case "CHP":
                                oDoc.Invoices.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_Receipt; // 24
                                break;
                        }
                        linea++;
                    }

                } // End Inovoices :: Documents

                bool fueDeclaradaCuentaCheque = false;
                int linea_tar = 0; // Payments_CreditCards
                int linea_che = 0; // Payments_Checks

                foreach (Pago pago in pagos)
                {
                    switch (pago.TipoPago)
                    {
                        case "TAR":
                            if (linea_tar > 0) oDoc.CreditCards.Add();
                            oDoc.CreditCards.SetCurrentLine(linea_tar);
                            oDoc.CreditCards.CreditCard = pago.PagosConTarjeta.TarjetaCredito;
                            oDoc.CreditCards.CreditAcct = pago.PagosConTarjeta.CuentaTarjetaCredito;
                            oDoc.CreditCards.CreditCardNumber = pago.PagosConTarjeta.NumeroDocumento;
                            oDoc.CreditCards.VoucherNum = pago.PagosConTarjeta.NumeroReclamo;
                            oDoc.CreditCards.OwnerIdNum = pago.PagosConTarjeta.NumeroFactura;
                            oDoc.CreditCards.NumOfPayments = 1;
                            oDoc.CreditCards.CreditSum = pago.PagosConTarjeta.SumaCredito;
                            linea_tar++;
                            break;

                        case "EFE":
                            oDoc.CashAccount = cabeceraPagos.CuentaCaja;
                            oDoc.CashSum = cabeceraPagos.MontoEfectivo;
                            break;

                        case "DEP":
                            oDoc.TransferAccount = cabeceraPagos.CuentaTransferencia;
                            oDoc.TransferSum = cabeceraPagos.SumaTransferencia;
                            oDoc.TransferDate = DateTime.Parse(cabeceraPagos.FechaTransferencia);
                            oDoc.TransferReference = cabeceraPagos.ReferenciaTransferencia;
                            break;

                        case "CHE":
                            if (linea_che > 0) oDoc.Checks.Add();
                            oDoc.Checks.SetCurrentLine(linea_che);
                            oDoc.Checks.DueDate = DateTime.Parse(pago.PagosConCheques.FechaVencimiento);
                            oDoc.Checks.CheckNumber = pago.PagosConCheques.NumeroCheques;
                            oDoc.Checks.BankCode = pago.PagosConCheques.CodigoBanco;
                            oDoc.Checks.CheckAccount = pago.PagosConCheques.CuentaCheque;
                            oDoc.Checks.AccounttNum = pago.PagosConCheques.NumeroCuentaBancaria;
                            oDoc.Checks.CheckSum = pago.PagosConCheques.SumaValorCheques;
                            oDoc.Checks.CountryCode = pago.PagosConCheques.CodigoPais;
                            if (!fueDeclaradaCuentaCheque)
                            {
                                fueDeclaradaCuentaCheque = true;
                                oDoc.CheckAccount = pago.PagosConCheques.CuentaCheque;
                            }
                            linea_che++;
                            break;

                        case "RET":
                            //int contadorRetencion = 0;
                            int codigoTarjeta = 0;
                            string cuentaContable = "";
                            oDoc.Series = DatosEnlace.serieRetencion;

                            foreach (Retencion retencion in pago.PagosConCheques.RetencionesAsociadas)
                            {
                                /*
                            try  {
                                OdbcConnection con = Comunicaciones.obtenerConexion();
                                con.Open();
                                string sqlQuery = "select r.cod_tarjeta,r.cuenta_cont from md_retenciones_facturas r ";
                                sqlQuery += "where r.cliente='" + cabeceraPagos.codigoCliente + "' and r.numdocumento='" + retencion.numeroDocumento + "' ";
                                sqlQuery += "and r.codigo_retencion='" + retencion.codigoRetencion + "'";
                                OdbcCommand com = con.CreateCommand();
                                com.CommandText = sqlQuery;
                                OdbcDataReader rst = com.ExecuteReader();
                                int indiceCodigoTarjeta = rst.GetOrdinal("cod_tarjeta");
                                int indiceCuentaContableTarjeta = rst.GetOrdinal("cuenta_cont");
                                while (rst.Read())  {
                                    codigoTarjeta = rst.GetValue(indiceCodigoTarjeta).ToString();
                                    cuentaContable = rst.GetValue(indiceCuentaContableTarjeta).ToString();
                                }
                                rst.Close();
                                rst.Dispose();
                                Comunicaciones.terminarConexion(con);
                            }
                            catch (Exception e)  {
                                codigoTarjeta = 0;
                                cuentaContable = "";
                            }*/
                                if (linea_tar > 0) oDoc.CreditCards.Add();
                                oDoc.CreditCards.SetCurrentLine(linea_tar);
                                //oDoc.CreditCards.LineNum = contadorRetencion;
                                oDoc.CreditCards.CreditCard = codigoTarjeta;
                                oDoc.CreditCards.CreditAcct = cuentaContable;
                                oDoc.CreditCards.CreditCardNumber = retencion.NumeroRetencion;
                                oDoc.CreditCards.CardValidUntil = DateTime.Parse(retencion.FechaRetencion);
                                oDoc.CreditCards.VoucherNum = retencion.DocEntry;
                                oDoc.CreditCards.OwnerIdNum = pago.PagosConCheques.NumeroAutorizacion;
                                oDoc.CreditCards.FirstPaymentSum = retencion.ValorRetencion;
                                oDoc.CreditCards.FirstPaymentDue = DateTime.Today; //va la fecha en q se genera el pago de la retención en SAP
                                oDoc.CreditCards.OwnerPhone = "001-001";
                                oDoc.CreditCards.PaymentMethodCode = 1; //siempre va 1
                                oDoc.CreditCards.NumOfPayments = 1; //siempre va 1
                                oDoc.CreditCards.NumOfCreditPayments = 1; //siempre va 1
                            }
                            break;

                        case "LET":
                            oDoc.BillOfExchange.ReferenceNo = pago.PagosConLetras.NumeroLetra;
                            oDoc.BillOfExchange.BPBankCode = pago.PagosConLetras.CodigoBancoAsociado;
                            oDoc.BillOfExchange.BPBankAct = pago.PagosConLetras.CuentaBancariaBancoAsociado;
                            oDoc.BillOfExchange.PaymentMethodCode = "Letras";
                            oDoc.BillOfExchange.BillOfExchangeDueDate = DateTime.Parse(pago.PagosConLetras.FechaVencimientoLetra);
                            oDoc.BillOfExchangeAmount = cabeceraPagos.MontoLetra;
                            break;
                    }
                }

                error = oDoc.Add();
                if (error != 0)
                {
                    DataBase.Company.GetLastError(out error, out mensaje);
                    throw new Exception(mensaje + " :: " + cabeceraPagos.NumeroMBW);
                }
                else
                {
                    response.Add("Success");
                    response.Add(DataBase.Company.GetNewObjectKey());
                    response.Add("");
                }
                oDoc = null;

            }
            catch (Exception e)
            {
                logs.grabarLog("COBRANZA", e.Message);
                logs.grabarLog("COBRANZA_DEBUG", e.StackTrace);
                response.Clear();
                response.Add("Error");
                response.Add(error.ToString());
                response.Add("COB001");
                response.Add(e.Message);
            }
            finally
            {
                DataBase.DesconectaDB();
            }


            return response;
        }


        public static List<SeccionDocumentos> StringToListSeccionDocumentos(string XML)
        {
            List<SeccionDocumentos> list = new List<SeccionDocumentos>();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(XML);

            XmlNodeList items = xml.GetElementsByTagName("ListSeccionDocumentos");
            XmlNodeList item = ((XmlElement)items[0]).GetElementsByTagName("SeccionDocumentos");

            foreach (XmlElement nodo in item)
            {
                SeccionDocumentos documento = new SeccionDocumentos();
                documento.MontoRetencion = XmlConvert.ToDouble(nodo.GetElementsByTagName("MontoRetencion")[0].InnerText);
                documento.NumeroCuota = XmlConvert.ToInt32(nodo.GetElementsByTagName("NumeroCuota")[0].InnerText);
                documento.NumeroCuotaSAP = XmlConvert.ToInt32(nodo.GetElementsByTagName("NumeroCuotaSAP")[0].InnerText);
                documento.NumeroDocumento = XmlConvert.ToInt32(nodo.GetElementsByTagName("NumeroDocumento")[0].InnerText);
                documento.NumeroRetencion = nodo.GetElementsByTagName("NumeroRetencion")[0].InnerText;
                documento.TipoDocumento = nodo.GetElementsByTagName("TipoDocumento")[0].InnerText;
                documento.Valor = XmlConvert.ToDouble(nodo.GetElementsByTagName("Valor")[0].InnerText);

                list.Add(documento);
            }

            return list;
        }

        public static List<Pago> StringToListPago(string XML)
        {
            List<Pago> list = new List<Pago>();
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(XML);

            XmlNodeList items = xml.GetElementsByTagName("ListPago");
            XmlNodeList item = ((XmlElement)items[0]).GetElementsByTagName("Pago");

            foreach (XmlElement nodo in item)
            {
                Pago pago = new Pago();
                pago.TipoPago = nodo.GetElementsByTagName("TipoPago")[0].InnerText;
                pago.PagosConTarjeta = null;
                pago.PagosConLetras = null;
                pago.PagosConCheques = null;
                switch (pago.TipoPago)
                {
                    case "CHE":
                        pago.PagosConCheques = new SeccionPagosCheques();
                        pago.PagosConCheques.CodigoBanco = nodo.GetElementsByTagName("CodigoBanco")[0].InnerText;
                        pago.PagosConCheques.CodigoPais = nodo.GetElementsByTagName("CodigoPais")[0].InnerText;
                        pago.PagosConCheques.CuentaCheque = nodo.GetElementsByTagName("CuentaCheque")[0].InnerText;
                        pago.PagosConCheques.FechaVencimiento = nodo.GetElementsByTagName("FechaVencimiento")[0].InnerText;
                        pago.PagosConCheques.NumeroAutorizacion = nodo.GetElementsByTagName("NumeroAutorizacion")[0].InnerText;
                        pago.PagosConCheques.NumeroCheques = XmlConvert.ToInt32(nodo.GetElementsByTagName("NumeroCheques")[0].InnerText);
                        pago.PagosConCheques.NumeroCuentaBancaria = nodo.GetElementsByTagName("NumeroCuentaBancaria")[0].InnerText;
                        pago.PagosConCheques.RetencionesAsociadas = null;
                        pago.PagosConCheques.SumaValorCheques = XmlConvert.ToDouble(nodo.GetElementsByTagName("SumaValorCheques")[0].InnerText);
                        break;
                }

                /*pago.MontoRetencion = XmlConvert.ToDouble(nodo.GetElementsByTagName("MontoRetencion")[0].InnerText);
                pago.NumeroCuota = XmlConvert.ToInt32(nodo.GetElementsByTagName("NumeroCuota")[0].InnerText);
                pago.NumeroCuotaSAP = XmlConvert.ToInt32(nodo.GetElementsByTagName("NumeroCuotaSAP")[0].InnerText);
                pago.NumeroDocumento = XmlConvert.ToInt32(nodo.GetElementsByTagName("NumeroDocumento")[0].InnerText);
                pago.NumeroRetencion = nodo.GetElementsByTagName("NumeroRetencion")[0].InnerText;
                pago.TipoDocumento = nodo.GetElementsByTagName("TipoDocumento")[0].InnerText;
                pago.Valor = XmlConvert.ToDouble(nodo.GetElementsByTagName("Valor")[0].InnerText);*/

                list.Add(pago);
            }

            return list;
        }
    }
}