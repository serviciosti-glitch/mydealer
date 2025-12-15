using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class SolicitudTraslado
    {
        public static RespuestaST crearSolicitudTraslado(CabeceraST cabecera, DetalleST[] detalles, string nombre_archivo, string extension_archivo, string base_archivo)
        {
            RespuestaST respuesta = new RespuestaST();
            respuesta.Estado = 0;
            respuesta.Mensaje = "";
            respuesta.NumeroDocumento = "";

            DataBase.ConectaDB();

            if (!DataBase.Respuesta.Exito)
            {
                respuesta.Mensaje = "Error al conectar a la empresa";
                respuesta.NumeroDocumento = "";
            }

            logs.grabarLog("SolicitudTraslado", "Procesando solicitud: " + cabecera.IdDevolucion);

            SAPbobsCOM.StockTransfer oDoc;

            try
            {
                oDoc = (SAPbobsCOM.StockTransfer)DataBase.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryTransferRequest);

                oDoc.DocDate = cabecera.DocDate;
                oDoc.CardCode = cabecera.CardCode;
                oDoc.FromWarehouse = cabecera.FromWarehouse;
                oDoc.ToWarehouse = cabecera.ToWarehouse;
                oDoc.Series = cabecera.Series;
                oDoc.SalesPersonCode = cabecera.SalesPersonCode;
                oDoc.Comments = cabecera.Comments;
                oDoc.Address = cabecera.Address;

                if (DatosEnlace.empresa == "LLP")
                {
                    oDoc.UserFields.Fields.Item("U_num_soldev").Value = int.Parse(cabecera.IdDevolucion);
                    oDoc.UserFields.Fields.Item("U_fecha_caducidad").Value = cabecera.U_fecha_caducidad;
                    oDoc.UserFields.Fields.Item("U_Alm_Dist_Entrega").Value = cabecera.U_Alm_Dist_Entrega;
                    oDoc.UserFields.Fields.Item("U_BPP_MDSD").Value = cabecera.U_BPP_MDSD;
                    oDoc.UserFields.Fields.Item("U_SYP_TipoOrden").Value = cabecera.U_SYP_TipoOrden;
                    oDoc.UserFields.Fields.Item("U_SYP_Descripcion").Value = cabecera.U_SYP_Descripcion;
                    oDoc.UserFields.Fields.Item("U_tipo_pedido").Value = cabecera.U_tipo_pedido;
                    oDoc.UserFields.Fields.Item("U_Agencia").Value = cabecera.U_Agencia;
                    oDoc.UserFields.Fields.Item("U_Direccion").Value = cabecera.U_Direccion;
                    oDoc.UserFields.Fields.Item("U_LLP_PFinal").Value = cabecera.U_LLP_PFinal;
                    oDoc.UserFields.Fields.Item("U_DIR_PTO_ORIGEN").Value = cabecera.U_DIR_PTO_ORIGEN;
                    oDoc.UserFields.Fields.Item("U_tipo_soldev").Value = cabecera.U_tipo_soldev;
                }

                int linea = 0;
                foreach (DetalleST detalle in detalles)
                {
                    if (linea > 0) oDoc.Lines.Add();
                    oDoc.Lines.SetCurrentLine(linea);

                    oDoc.Lines.ItemCode = detalle.ItemCode;
                    oDoc.Lines.ItemDescription = detalle.ItemDescription;
                    oDoc.Lines.Quantity = detalle.Quantity;

                    linea++;
                }

                int error_documento = oDoc.Add();

                if (error_documento != 0)
                {
                    int error = 0;
                    string mensaje = "";

                    DataBase.Company.GetLastError(out error, out mensaje);

                    logs.grabarLog("SolicitudTraslado", error + " : " + mensaje);

                    respuesta.Mensaje = error + " : " + mensaje;
                }
                else
                {
                    string NumeroDocumento = DataBase.Company.GetNewObjectKey();

                    if (!String.IsNullOrEmpty(nombre_archivo) && DatosEnlace.empresa == "LLP")
                    {
                        guardarEnCarpetaCompartidaST(nombre_archivo, extension_archivo, base_archivo, int.Parse(NumeroDocumento));
                    }

                    logs.grabarLog("SolicitudTraslado", "Exito: " + NumeroDocumento);

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

                logs.grabarLog("SolicitudTraslado", e.Message);
                logs.grabarLog("SolicitudTraslado_DEBUG", e.StackTrace);
            }

            oDoc = null;

            return respuesta;
        }

        public static void guardarEnCarpetaCompartidaST(string nombre_archivo, string extension_archivo, string base_archivo, int codigo_sap)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "archivos";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string ruta_archivo = path + "/" + nombre_archivo + "." + extension_archivo;
            Byte[] bytes = Convert.FromBase64String(base_archivo);
            File.WriteAllBytes(ruta_archivo, bytes);

            //string archFuente = @"\\servidor\carpetaCompartida\archivo.extension";
            string nomArch = Path.GetFileName(ruta_archivo);
            string destino = @"\\GVS044\share\SharedFolderSU1\" + DatosEnlace.nombreBaseDatos + @"\Attachments\MYDEALER_ST";
            File.Copy(ruta_archivo, Path.Combine(destino, nomArch), true);

            SAPbobsCOM.StockTransfer oNewDoc = (SAPbobsCOM.StockTransfer)DataBase.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryTransferRequest);

            oNewDoc.GetByKey(codigo_sap);

            if (DatosEnlace.empresa == "LLP")
            {
                oNewDoc.UserFields.Fields.Item("U_anexo_soldev").Value = Path.Combine(destino, nomArch);
            }

            int file_error;
            string file_mensaje;

            int error_documento = oNewDoc.Update();

            if (error_documento != 0)
            {
                DataBase.Company.GetLastError(out file_error, out file_mensaje);
                logs.grabarLog("ST_ARCHIVO", "NO AGREGO ST # " + codigo_sap + " ( NroError " + file_error + " ) : " + file_mensaje);
            }

            // Eliminar el archivo

            File.Delete(ruta_archivo);

        }

        /*
        public static void guardarAdjuntoST( string nombre_archivo, string extension_archivo, string base_archivo, int codigo_sap)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "archivos";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string ruta_archivo = path + "/" + nombre_archivo + "." + extension_archivo;
            Byte[] bytes = Convert.FromBase64String(base_archivo);
            File.WriteAllBytes(ruta_archivo, bytes);

            SAPbobsCOM.StockTransfer oNewDoc;

            oNewDoc = (SAPbobsCOM.StockTransfer)DataBase.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryTransferRequest);

            SAPbobsCOM.Attachments2 oAtt = (SAPbobsCOM.Attachments2)DataBase.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2);
            
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
                    logs.grabarLog("ST_ARCHIVO", "NO ACTUALIZO ST # " + codigo_sap + " ( NroError " + file_error + " ) : " + file_mensaje);
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
                    logs.grabarLog("ST_ARCHIVO", "NO AGREGO ST # " + codigo_sap + " ( NroError " + file_error + " ) : " + file_mensaje);
                }
            }

            // Eliminar el archivo

            File.Delete(ruta_archivo);

        }
        */
    }
}