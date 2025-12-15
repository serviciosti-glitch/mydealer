using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class DatosEnlace
    {
        //public static string ipBaseDatos = "192.168.2.3"; // ip de la base de datos de SBO
        public static string ipBaseDatos = ConfigurationManager.AppSettings["ipBaseDatos"]; // ip de la base de datos de SBO
        public static string nombreBaseDatos = ConfigurationManager.AppSettings["nombreBaseDatos"]; // nombre de la base de datos SBO-COMMON
        //public static string nombreBaseDatos = "IMPROV_2010";
        public static string usuarioBaseDatos = ConfigurationManager.AppSettings["usuarioBaseDatos"]; // usuario de la base de datos de SAP
        public static string passwordBaseDatos = ConfigurationManager.AppSettings["passwordBaseDatos"]; // clave de la base de datos de SAP
        public static string ipServidorLicencia = ConfigurationManager.AppSettings["ipServidorLicencia"]; // ip del servidor de licencias (ojo.- incluir puerto)
        //public static string usuarioSAP = "manager"; // usuario SAP (usuario registrado en SBO)
        //public static string passwordSAP = "SAPIMP"; // clave del usuario SAP (registrado en SBO)
        public static string usuarioSAP = ConfigurationManager.AppSettings["usuarioSAP"]; // usuario SAP (usuario registrado en SBO)
        public static string passwordSAP = ConfigurationManager.AppSettings["passwordSAP"]; // clave del usuario SAP (registrado en SBO)
        public static string lenguajeConector = ConfigurationManager.AppSettings["lenguajeConector"]; // lenguaje de la base de datos
        public static string tipoBaseDatos = ConfigurationManager.AppSettings["tipoBaseDatos"]; // dialecto de la base de datos
        public static string paisSistema = ConfigurationManager.AppSettings["paisSistema"]; // pais del sistema, define comportamientos especificos
        public static string aprobacion = ConfigurationManager.AppSettings["aprobacion"];   // indicador de si debe enviarse o no los parametros de aprobacion del pedido a SAP
        public static int serieRetencion = int.Parse(ConfigurationManager.AppSettings["serieRetencion"]); // serie usada en el caso retenciones (cobranzas)
        public static string empresa = ConfigurationManager.AppSettings["empresa"].ToUpper();

        public static string moneda = ConfigurationManager.AppSettings["moneda"].ToUpper();

        public static string sufijo = (String.IsNullOrEmpty(ConfigurationManager.AppSettings["sufijo"]) ? "" : ConfigurationManager.AppSettings["sufijo"].ToLower());

        public static string conexionAlterna = ConfigurationManager.AppSettings["conexionAlterna"];
        public static string sp_pedido_cab = ConfigurationManager.AppSettings["sp_pedido_cab"];
        public static string sp_pedido_det = ConfigurationManager.AppSettings["sp_pedido_det"];
        public static string fecha_moneda = ConfigurationManager.AppSettings["fecha_moneda"];
        public static string cd_cc = ConfigurationManager.AppSettings["cd_cc"];
        public static string cd_sc = ConfigurationManager.AppSettings["cd_sc"];
        public static string cd_ss = ConfigurationManager.AppSettings["cd_ss"];
    }
}