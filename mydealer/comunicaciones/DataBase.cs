using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class DataBase
    {
        private static SAPbobsCOM.Company company;
        public static SAPbobsCOM.Company Company
        {
            get { return DataBase.company; }
            set { DataBase.company = value; }
        }

        private static int error;
        public static int IError
        {
            get { return DataBase.error; }
            set { DataBase.error = value; }
        }

        private static string mensaje;
        public static string SError
        {
            get { return DataBase.mensaje; }
            set { DataBase.mensaje = value; }
        }

        private static Respuesta respuesta = new Respuesta();

        public static Respuesta Respuesta
        {
            get { return DataBase.respuesta; }
            set { DataBase.respuesta = value; }
        }

        private static void conectar()
        {
            try
            {
                company = new SAPbobsCOM.Company();
                company.Server = DatosEnlace.ipBaseDatos; // "SAPSRVBBDD";
                company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2019;
                company.CompanyDB = DatosEnlace.nombreBaseDatos; // "DB_CAPACITACION";
                company.UserName = DatosEnlace.usuarioSAP; // "mydealer";
                company.Password = DatosEnlace.passwordSAP; // "myd321";
                company.DbUserName = DatosEnlace.usuarioBaseDatos; // "mydealer";
                company.DbPassword = DatosEnlace.passwordBaseDatos; // "myd321";
                company.UseTrusted = false;
                company.LicenseServer = DatosEnlace.ipServidorLicencia;
                error = company.Connect();
                if (error != 0)
                {
                    company.GetLastError(out error, out mensaje);
                    throw new Exception(mensaje);
                }
                respuesta.Exito = true;
            }
            catch (Exception e)
            {
                respuesta.Exito = false;
                respuesta.CodigoError = error.ToString();
                respuesta.CodigoRespuesta = "COM001";
                respuesta.DescripcionError = mensaje + " :: " + e.Message; //  "Error al conectar a la Compañia";
                Console.WriteLine(e.Message);
                logs.grabarLog("DATA_SAP", e.Message);
                logs.grabarLog("DATA_SAP_DEBUG", e.StackTrace);
            }
        }

        public static void ConectaDB()
        {
            if (company == null || !company.Connected)
                conectar();
        }

        public static void DesconectaDB()
        {

            try
            {
                if (company.Connected)
                {
                    company.Disconnect();
                }
                respuesta.Exito = true;
            }
            catch (Exception e)
            {
                respuesta.Exito = false;
                respuesta.CodigoError = "COM002";
                respuesta.DescripcionError = "Error al desconectar a la Compañia";
                logs.grabarLog("PEDIDO", e.Message);
                logs.grabarLog("PEDIDO_DEBUG", e.StackTrace);
            }
        }
    }
}