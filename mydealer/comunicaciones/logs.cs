using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class logs
    {
        private static string ext = ".bts";

        // FX. PARA GRABAR LOGS DE SUCESOS
        public static void grabarLog(string tipo, string datos, string trazabilidad = "")
        {
            try
            {
                string path_logs = AppDomain.CurrentDomain.BaseDirectory + "logs/" +
                DateTime.Now.Year.ToString() + "/" +
                DateTime.Now.Month.ToString() + "/" +
                DateTime.Now.Day.ToString();

                if (!String.IsNullOrEmpty(trazabilidad))
                {
                    path_logs = path_logs + "/" + "TRAZABILIDAD_" + trazabilidad;
                }

                logs.verificarRuta(path_logs);

                StreamWriter log = new StreamWriter(path_logs + "/" + tipo + logs.ext, true);
                String cadena = "";

                cadena += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " >>> " + datos;

                log.WriteLine(cadena);
                log.Close();

            }
            catch (Exception e)
            {
                Console.Out.WriteLine("ERROR LOG: " + e.Message);
            }

        }

        // FX. PARA VERIFICAR SI MI RUTA EXISTE, SINO LA CREA
        public static void verificarRuta(String path)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("ERROR LOG: " + e.Message);
            }
        }
    }
}