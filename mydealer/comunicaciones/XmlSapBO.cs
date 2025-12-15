using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class XmlSapBO
    {
        public static int xmlLlamadaServicio(int IdLlamada)
        {
            DataBase.ConectaDB();

            if (!DataBase.Respuesta.Exito)
            {
                return 0;
            }

            SAPbobsCOM.ServiceCalls oServiceCalls = (SAPbobsCOM.ServiceCalls)DataBase.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oServiceCalls);

            if (oServiceCalls.GetByKey(IdLlamada))
            {
                // Deja el archivo en la carpeta BIN
                //string filename = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", "") + "\\file.xml";
                string filename = AppDomain.CurrentDomain.BaseDirectory + "\\file_bo.xml";
                oServiceCalls.SaveXML(ref filename);
            }

            return 1;
        }
    }
}