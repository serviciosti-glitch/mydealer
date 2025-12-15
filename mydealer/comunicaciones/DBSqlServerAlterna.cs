using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class DBSqlServerAlterna
    {
        private static SqlConnection conexion = null;

        public static SqlConnection Conexion
        {
            get { return DBSqlServerAlterna.conexion; }
        }


        private static int error;
        public static int IError
        {
            get { return DBSqlServerAlterna.error; }
            set { DBSqlServerAlterna.error = value; }
        }

        private static string mensaje;
        public static string SError
        {
            get { return DBSqlServerAlterna.mensaje; }
            set { DBSqlServerAlterna.mensaje = value; }
        }

        private static Respuesta respuesta = new Respuesta();

        public static Respuesta Respuesta
        {
            get { return DBSqlServerAlterna.respuesta; }
            set { DBSqlServerAlterna.respuesta = value; }
        }


        private static void conectar()
        {
            try
            {
                // Data Source=ECGU16119;Initial Catalog=MD_DIFERENCIAL;User ID=sa
                // Data Source=ECGU16119;User ID=sa;Password=etech123
                // Data Source=ECGU16119;Initial Catalog=MD_DIFERENCIAL;User ID=sa;Password=etech123
                if (conexion == null)
                {
                    conexion = new SqlConnection();

                    conexion.ConnectionString = DatosEnlace.conexionAlterna;

                    System.Threading.Thread.Sleep(750);
                }

                if (conexion.State == System.Data.ConnectionState.Connecting)
                {
                    while (conexion.State == System.Data.ConnectionState.Connecting)
                        System.Threading.Thread.Sleep(500);
                    logs.grabarLog("ConexionAlterna", "Conexion CONECTANDO");
                }

                if (conexion.State == System.Data.ConnectionState.Closed && conexion.State != System.Data.ConnectionState.Connecting)
                {
                    conexion.Open();
                    logs.grabarLog("ConexionAlterna", "Conexion ABIERTA");
                }

                if (conexion.State == System.Data.ConnectionState.Broken)
                {
                    conexion.Close();
                    conexion.Open();
                    logs.grabarLog("ConexionAlterna", "Conexion ROTA -> CERRADA -> ABIERTA");
                }

                if (conexion.State == System.Data.ConnectionState.Open ||
                    conexion.State == System.Data.ConnectionState.Executing ||
                    conexion.State == System.Data.ConnectionState.Fetching)
                {
                    logs.grabarLog("ConexionAlterna", "Conexion ESTADO: " + conexion.State.ToString());
                }

                respuesta.Exito = true;
            }
            catch (Exception e)
            {
                respuesta.Exito = false;
                respuesta.CodigoError = "Problema al abri la conexion";
                respuesta.CodigoRespuesta = "CON001";
                respuesta.DescripcionError = "Error al conectar a la Base de Datos. " + e.Message;
                Console.WriteLine(e.Message);
                logs.grabarLog("ConexionAlterna", e.Message);
                logs.grabarLog("ConexionAlterna_DEBUG", e.StackTrace);
            }
        }

        public static void ConectaDB()
        {
            //if (conexion != null && conexion.State == System.Data.ConnectionState.Open)
            conectar();
        }

        public static void DesconectaDB()
        {

            try
            {
                //conexion = null;
                //if (conexion.State == System.Data.ConnectionState.Open || conexion.State == System.Data.ConnectionState.Broken)
                respuesta.Exito = true;
            }
            catch (Exception e)
            {
                respuesta.Exito = false;
                respuesta.CodigoError = "CON002";
                respuesta.DescripcionError = "Error al desconectar a la Compañia. " + e.Message;
            }
        }
    }
}