using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Xml;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class LlamadaServicio
    {
        public static RespuestaLS crearLlamadaServicio(CabeceraLS cabecera)
        {
            RespuestaLS respuesta = new RespuestaLS();
            respuesta.Estado = 0;
            respuesta.Mensaje = "";
            respuesta.NumeroDocumento = "";

            DataBase.ConectaDB();

            if (!DataBase.Respuesta.Exito)
            {
                respuesta.Mensaje = "Error al conectar a la empresa";
                respuesta.NumeroDocumento = "";
            }

            logs.grabarLog("LlamadaServicio", "Procesando llamada servicio");

            //SAPbobsCOM.ServiceCalls oDoc;

            try
            {
                // Propiedad para no guardar el xml en un archivo en el server
                DataBase.Company.XMLAsString = true;

                StringWriter sObjectXML = null;
                XmlTextWriter writer = default(XmlTextWriter);
                sObjectXML = new System.IO.StringWriter();
                writer = new XmlTextWriter(sObjectXML);
                writer.Formatting = System.Xml.Formatting.Indented;
                writer.Indentation = 4;
                writer.WriteStartDocument();
                writer.WriteStartElement("BOM", "");
                writer.WriteStartElement("BO", "");

                /*** ADMIN ***/
                writer.WriteStartElement("AdmInfo", "");
                writer.WriteStartElement("Object", "");
                writer.WriteString("191");
                writer.WriteEndElement();
                writer.WriteEndElement();

                /*** OSCL ***/
                writer.WriteStartElement("OSCL", "");
                writer.WriteStartElement("row", "");

                writer.WriteStartElement("customer", "");
                writer.WriteString(cabecera.CustomerCode);
                writer.WriteEndElement();

                writer.WriteStartElement("subject", "");
                writer.WriteString(cabecera.Subject);
                writer.WriteEndElement();

                writer.WriteStartElement("itemCode", "");
                writer.WriteString(cabecera.itemCode);
                writer.WriteEndElement();

                writer.WriteStartElement("descrption", "");
                writer.WriteString(cabecera.Description);
                writer.WriteEndElement();

                writer.WriteStartElement("Series", "");
                writer.WriteString(cabecera.Series.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("problemTyp", "");
                writer.WriteString(cabecera.ProblemType.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("Telephone", "");
                writer.WriteString(cabecera.Telephone);
                writer.WriteEndElement();

                DireccionLS dir_envio = obtenerDireccion(cabecera.CustomerCode, "Envio");
                DireccionLS dir_facturacion = obtenerDireccion(cabecera.CustomerCode, "Facturacion");

                if (dir_envio.Estado == 1)
                {
                    writer.WriteStartElement("BPShipCode", "");
                    writer.WriteString(dir_envio.Address);
                    writer.WriteEndElement();

                    writer.WriteStartElement("BPShipAddr", "");
                    writer.WriteString(dir_envio.Direccion);
                    writer.WriteEndElement();
                }

                if (dir_facturacion.Estado == 1)
                {
                    writer.WriteStartElement("BPBillCode", "");
                    writer.WriteString(dir_facturacion.Address);
                    writer.WriteEndElement();

                    writer.WriteStartElement("BPBillAddr", "");
                    writer.WriteString(dir_facturacion.Direccion);
                    writer.WriteEndElement();
                }

                if (DatosEnlace.empresa == "LLP")
                {
                    /*
                    oDoc.UserFields.Fields.Item("U_num_soldev").Value = int.Parse(cabecera.IdDevolucion);
                    oDoc.UserFields.Fields.Item("U_BK_NROT").Value = cabecera.U_BK_NROT;
                    oDoc.UserFields.Fields.Item("U_BK_TIPOSERV").Value = cabecera.U_BK_TIPOSERV;
                    oDoc.UserFields.Fields.Item("U_area_division").Value = cabecera.U_area_division;
                    oDoc.UserFields.Fields.Item("U_MSS_ProfitCode").Value = cabecera.U_MSS_ProfitCode;
                    oDoc.UserFields.Fields.Item("U_MSS_OcrCode2").Value = cabecera.U_MSS_OcrCode2;
                    oDoc.UserFields.Fields.Item("U_TIPO_RECURSO").Value = cabecera.U_TIPO_RECURSO;
                    oDoc.UserFields.Fields.Item("U_localidad").Value = cabecera.U_localidad;
                    oDoc.UserFields.Fields.Item("U_vertical").Value = cabecera.U_vertical;
                    oDoc.UserFields.Fields.Item("U_centro_costo").Value = cabecera.U_centro_costo;
                    */

                    writer.WriteStartElement("U_num_soldev", "");
                    writer.WriteString(cabecera.IdDevolucion);
                    writer.WriteEndElement();

                    writer.WriteStartElement("U_BK_NROT", "");
                    writer.WriteString(cabecera.U_BK_NROT);
                    writer.WriteEndElement();

                    writer.WriteStartElement("U_BK_TIPOSERV", "");
                    writer.WriteString(cabecera.U_BK_TIPOSERV);
                    writer.WriteEndElement();

                    writer.WriteStartElement("U_area_division", "");
                    writer.WriteString(cabecera.U_area_division);
                    writer.WriteEndElement();

                    writer.WriteStartElement("U_MSS_ProfitCode", "");
                    writer.WriteString(cabecera.U_MSS_ProfitCode);
                    writer.WriteEndElement();

                    writer.WriteStartElement("U_MSS_OcrCode2", "");
                    writer.WriteString(cabecera.U_MSS_OcrCode2);
                    writer.WriteEndElement();

                    writer.WriteStartElement("U_TIPO_RECURSO", "");
                    writer.WriteString(cabecera.U_TIPO_RECURSO);
                    writer.WriteEndElement();

                    writer.WriteStartElement("U_localidad", "");
                    writer.WriteString(cabecera.U_localidad);
                    writer.WriteEndElement();

                    writer.WriteStartElement("U_vertical", "");
                    writer.WriteString(cabecera.U_vertical);
                    writer.WriteEndElement();

                    writer.WriteStartElement("U_centro_costo", "");
                    writer.WriteString(cabecera.U_centro_costo);
                    writer.WriteEndElement();

                    writer.WriteStartElement("U_num_soldev_det", "");
                    writer.WriteString(cabecera.U_num_soldev_det.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndElement();

                SAPbobsCOM.ServiceCalls oObject = (SAPbobsCOM.ServiceCalls)DataBase.Company.GetBusinessObjectFromXML(sObjectXML.ToString(), 0);

                if (oObject.Add() != 0)
                {
                    int error = 0;
                    string mensaje = "";

                    DataBase.Company.GetLastError(out error, out mensaje);

                    logs.grabarLog("LlamadaServicio", error + " : " + mensaje);

                    respuesta.Mensaje = error + " : " + mensaje;
                }
                else
                {
                    string NumeroDocumento = DataBase.Company.GetNewObjectKey();

                    // Crear la actividad

                    SAPbobsCOM.Contacts oContacts = (SAPbobsCOM.Contacts)DataBase.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oContacts);

                    oContacts.Notes = cabecera.Notes;
                    oContacts.CardCode = cabecera.CustomerCode;

                    oContacts.DocEntry = cabecera.DocEntry;
                    oContacts.DocType = cabecera.DocType;

                    oContacts.Add();

                    string NumeroActividad = DataBase.Company.GetNewObjectKey();

                    SAPbobsCOM.ServiceCalls oServiceCalls = (SAPbobsCOM.ServiceCalls)DataBase.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oServiceCalls);

                    if (oServiceCalls.GetByKey(int.Parse(NumeroDocumento)))
                    {
                        // Se incrementa en caso de que existan mas
                        //oServiceCalls.Activities.Add();

                        oServiceCalls.Activities.ActivityCode = Convert.ToInt32(NumeroActividad);

                        if (oServiceCalls.Update() != 0)
                        {
                            int error_act = 0;
                            string mensaje_act = "";

                            DataBase.Company.GetLastError(out error_act, out mensaje_act);

                            logs.grabarLog("Actividad", error_act + " : " + mensaje_act);
                        }
                        else
                        {
                            logs.grabarLog("Actividad", "Exito: " + NumeroActividad);
                        }
                    }


                    // Codigo para agregar historial
                    /*
                    SAPbobsCOM.ServiceCalls oService = (SAPbobsCOM.ServiceCalls)DataBase.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oServiceCalls);

                    //Load your service call
                    if (oService.GetByKey(int.Parse(NumeroDocumento)))
                    {
                        //Check if it is necessary open a new line for your expense
                        if (oService.Expenses.DocEntry != 0)
                            oService.Expenses.Add();


                        //Provide the docEntry of your document and the type of your document
                        oService.Expenses.DocEntry = documento.DocEntry;
                        oService.Expenses.DocumentType = SAPbobsCOM.BoSvcEpxDocTypes.edt_StockTransfer;

                        //oService.Activities.ActivityCode

                        oService.Update();

                        //if (oService.Update() != 0)
                            //MessageBox.Show(oCompany.GetLastErrorDescription());
                    }
                    */
                    // Fin

                    logs.grabarLog("LlamadaServicio", "Exito: " + NumeroDocumento);

                    respuesta.Estado = 1;
                    respuesta.Mensaje = "Exito";
                    respuesta.NumeroDocumento = NumeroDocumento;
                }

            }
            catch (Exception e)
            {
                respuesta.Estado = 0;
                respuesta.Mensaje = e.Message;
                respuesta.NumeroDocumento = "";

                logs.grabarLog("LlamadaServicio", e.Message);
                logs.grabarLog("LlamadaServicio_DEBUG", e.StackTrace);
            }

            //oDoc = null;

            return respuesta;
        }

        public static DireccionLS obtenerDireccion(string CardCode, string type)
        {
            DireccionLS respuesta = new DireccionLS();
            respuesta.Estado = 0;
            respuesta.Address = "";
            respuesta.Direccion = "";

            DBSqlServer.ConectaDB();
            if (!DBSqlServer.Respuesta.Exito)
            {
                return respuesta;
            }

            try
            {
                string sql_text = "SELECT TOP 1 Address, SUBSTRING((Street+'-'+Block+'-'+City+'-'+County),1,254) AS Direccion FROM CRD1 WHERE CardCode = '" + CardCode + "' AND AdresType = 'S' AND Address = ( SELECT TOP 1 ShipToDef FROM OCRD WHERE CardCode = '" + CardCode + "' ) ";

                if (type == "Facturacion")
                {
                    sql_text = "SELECT TOP 1 Address, SUBSTRING((Street+'-'+Block+'-'+City+'-'+County),1,254) AS Direccion FROM CRD1 WHERE CardCode = '" + CardCode + "' AND AdresType = 'B' AND Address = ( SELECT TOP 1 BillToDef FROM OCRD WHERE CardCode = '" + CardCode + "' ) ";
                }

                SqlCommand com = new SqlCommand(sql_text, DBSqlServer.Conexion);
                com.CommandType = CommandType.Text;

                SqlDataReader record = com.ExecuteReader();

                if (record.HasRows)
                {
                    if (record.Read())
                    {
                        respuesta.Estado = 1;
                        respuesta.Address = record.GetValue(0).ToString();
                        respuesta.Direccion = record.GetValue(1).ToString();
                    }
                }

                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                logs.grabarLog("obtenerDireccion", e.Message);
                logs.grabarLog("obtenerDireccion_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServer.DesconectaDB();
            }

            return respuesta;
        }

        public static RespuestaLS crearLlamadaServicioOLD(CabeceraLS cabecera)
        {
            RespuestaLS respuesta = new RespuestaLS();
            respuesta.Estado = 0;
            respuesta.Mensaje = "";
            respuesta.NumeroDocumento = "";

            DataBase.ConectaDB();

            if (!DataBase.Respuesta.Exito)
            {
                respuesta.Mensaje = "Error al conectar a la empresa";
                respuesta.NumeroDocumento = "";
            }

            logs.grabarLog("LlamadaServicio", "Procesando llamada servicio");

            SAPbobsCOM.ServiceCalls oDoc;

            try
            {
                oDoc = (SAPbobsCOM.ServiceCalls)DataBase.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oServiceCalls);

                oDoc.CustomerCode = cabecera.CustomerCode;
                oDoc.Subject = cabecera.Subject;
                oDoc.Description = cabecera.Description;
                oDoc.Series = cabecera.Series;
                oDoc.ProblemType = cabecera.ProblemType;

                //oDoc.ContactCode = 4405;

                //oDoc.Tele = cabecera.Telephone;
                //oDoc.UserFields.Fields.Item("Telephone").Value = cabecera.Telephone;

                if (DatosEnlace.empresa == "LLP")
                {
                    oDoc.UserFields.Fields.Item("U_num_soldev").Value = int.Parse(cabecera.IdDevolucion);
                    oDoc.UserFields.Fields.Item("U_BK_NROT").Value = cabecera.U_BK_NROT;
                    oDoc.UserFields.Fields.Item("U_BK_TIPOSERV").Value = cabecera.U_BK_TIPOSERV;
                    oDoc.UserFields.Fields.Item("U_area_division").Value = cabecera.U_area_division;
                    oDoc.UserFields.Fields.Item("U_MSS_ProfitCode").Value = cabecera.U_MSS_ProfitCode;
                    oDoc.UserFields.Fields.Item("U_MSS_OcrCode2").Value = cabecera.U_MSS_OcrCode2;
                    oDoc.UserFields.Fields.Item("U_TIPO_RECURSO").Value = cabecera.U_TIPO_RECURSO;
                    oDoc.UserFields.Fields.Item("U_localidad").Value = cabecera.U_localidad;
                    oDoc.UserFields.Fields.Item("U_vertical").Value = cabecera.U_vertical;
                    oDoc.UserFields.Fields.Item("U_centro_costo").Value = cabecera.U_centro_costo;
                }

                int error_documento = oDoc.Add();

                if (error_documento != 0)
                {
                    int error = 0;
                    string mensaje = "";

                    DataBase.Company.GetLastError(out error, out mensaje);

                    logs.grabarLog("LlamadaServicio", error + " : " + mensaje);

                    respuesta.Mensaje = error + " : " + mensaje;
                }
                else
                {
                    string NumeroDocumento = DataBase.Company.GetNewObjectKey();

                    // Crear la actividad

                    SAPbobsCOM.Contacts oContacts = (SAPbobsCOM.Contacts)DataBase.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oContacts);

                    oContacts.Notes = cabecera.Notes;
                    oContacts.CardCode = cabecera.CustomerCode;

                    oContacts.DocEntry = cabecera.DocEntry;
                    oContacts.DocType = cabecera.DocType;

                    oContacts.Add();

                    string NumeroActividad = DataBase.Company.GetNewObjectKey();

                    SAPbobsCOM.ServiceCalls oServiceCalls = (SAPbobsCOM.ServiceCalls)DataBase.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oServiceCalls);

                    if (oServiceCalls.GetByKey(int.Parse(NumeroDocumento)))
                    {
                        // Se incrementa en caso de que existan mas
                        //oServiceCalls.Activities.Add();

                        oServiceCalls.Activities.ActivityCode = Convert.ToInt32(NumeroActividad);

                        if (oServiceCalls.Update() != 0)
                        {
                            int error_act = 0;
                            string mensaje_act = "";

                            DataBase.Company.GetLastError(out error_act, out mensaje_act);

                            logs.grabarLog("Actividad", error_act + " : " + mensaje_act);
                        }
                        else
                        {
                            logs.grabarLog("Actividad", "Exito: " + NumeroActividad);
                        }
                    }


                    // Codigo para agregar historial
                    /*
                    SAPbobsCOM.ServiceCalls oService = (SAPbobsCOM.ServiceCalls)DataBase.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oServiceCalls);

                    //Load your service call
                    if (oService.GetByKey(int.Parse(NumeroDocumento)))
                    {
                        //Check if it is necessary open a new line for your expense
                        if (oService.Expenses.DocEntry != 0)
                            oService.Expenses.Add();


                        //Provide the docEntry of your document and the type of your document
                        oService.Expenses.DocEntry = documento.DocEntry;
                        oService.Expenses.DocumentType = SAPbobsCOM.BoSvcEpxDocTypes.edt_StockTransfer;

                        //oService.Activities.ActivityCode

                        oService.Update();

                        //if (oService.Update() != 0)
                            //MessageBox.Show(oCompany.GetLastErrorDescription());
                    }
                    */
                    // Fin

                    logs.grabarLog("LlamadaServicio", "Exito: " + NumeroDocumento);

                    respuesta.Estado = 1;
                    respuesta.Mensaje = "Exito";
                    respuesta.NumeroDocumento = NumeroDocumento;
                }

            }
            catch (Exception e)
            {
                respuesta.Estado = 0;
                respuesta.Mensaje = e.Message;
                respuesta.NumeroDocumento = "";

                logs.grabarLog("LlamadaServicio", e.Message);
                logs.grabarLog("LlamadaServicio_DEBUG", e.StackTrace);
            }

            oDoc = null;

            return respuesta;
        }
    }
}