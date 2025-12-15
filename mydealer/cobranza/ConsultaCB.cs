using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using SAPbobsCOM;

namespace mydealer
{
    public class ConsultaCB
    {
        public static RespuestaCB obtenerCobros(DateTime FECHAINICIO, DateTime FECHAFIN, string CODVENDEDOR) {
            RespuestaCB respuesta = new RespuestaCB();
            respuesta.Estado = false;
            respuesta.DescripcionError = "";
            respuesta.ListaRespuesta = "";

            DBSqlServerAux.ConectaDB();
            if (!DBSqlServerAux.Respuesta.Exito)
            {
                respuesta.Estado = false;
            }

            try
            {
                // Consulto la deuda total del ente                 
                SqlCommand com = new SqlCommand("SYP_REPORTE_COBROS_POR_SECTORISTA_INC_SECTORISTA_MYDEALER", DBSqlServerAux.Conexion);

                com.CommandTimeout = 900;

                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.Add("@FECHAINI_PERCUR", SqlDbType.DateTime).Value = FECHAINICIO;
                com.Parameters.Add("@FECHAFIN_PERCUR", SqlDbType.DateTime).Value = FECHAFIN;
                //com.Parameters.AddWithValue("@CODVENDEDOR", CODVENDEDOR);

                SqlDataReader record = com.ExecuteReader();

                if (record.HasRows)
                {

                    List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();

                    Dictionary<string, string> row;

                    while (record.Read())
                    {
                        row = new Dictionary<string, string>();

                        bool registro_valido = false;

                        for (int f = 0; f < record.FieldCount; f++)
                        {
                            row.Add(record.GetName(f), record.GetValue(f).ToString());

                            if ( record.GetName(f) == "Cod_vendedor" ) {
                                if ( CODVENDEDOR == record.GetValue(f).ToString() ) {
                                    registro_valido = true;
                                }
                            }

                        }

                        if (registro_valido)
                        {
                            rows.Add(row);
                        }
                    }
                    respuesta.Estado = true;
                    var serializer = new JavaScriptSerializer();
                    serializer.MaxJsonLength = Int32.MaxValue;
                    respuesta.ListaRespuesta = serializer.Serialize(rows);
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                respuesta.DescripcionError = e.Message;
                logs.grabarLog("obtenerCobros", e.Message);
                logs.grabarLog("obtenerCobros_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServerAux.DesconectaDB();
            }

            return respuesta;
        }
        public static RespuestaCB obtenerCobranzaLLP(DateTime FECHAINICIO, DateTime FECHAFIN, string CARDCODE)
        {
            RespuestaCB respuesta = new RespuestaCB();
            respuesta.Estado = false;
            respuesta.DescripcionError = "";
            respuesta.ListaRespuesta = "";

            DBSqlServerAux.ConectaDB();
            if (!DBSqlServerAux.Respuesta.Exito)
            {
                respuesta.Estado = false;
            }

            try
            {
                // Consulto la deuda total del ente                 
                SqlCommand com = new SqlCommand("SYP_REPORTE_AUXILIAR_CUENTAS_POR_COBRAR_MYDEALER", DBSqlServerAux.Conexion);

                com.CommandTimeout = 180;

                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.Add("@FECINI", SqlDbType.DateTime).Value = FECHAINICIO;
                com.Parameters.Add("@TAXDATE", SqlDbType.DateTime).Value = FECHAFIN;
                com.Parameters.AddWithValue("@CARDCODE", CARDCODE);

                SqlDataReader record = com.ExecuteReader();

                if (record.HasRows)
                {

                    List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();

                    Dictionary<string, string> row;

                    while (record.Read())
                    {
                        row = new Dictionary<string, string>();

                        for (int f = 0; f < record.FieldCount; f++)
                        {
                            row.Add(record.GetName(f), record.GetValue(f).ToString());
                        }

                        rows.Add(row);
                    }
                    respuesta.Estado = true;
                    var serializer = new JavaScriptSerializer();
                    serializer.MaxJsonLength = Int32.MaxValue;
                    respuesta.ListaRespuesta = serializer.Serialize(rows);
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                respuesta.DescripcionError = e.Message;
                logs.grabarLog("obtenerCobranzaLLP", e.Message);
                logs.grabarLog("obtenerCobranzaLLP_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServerAux.DesconectaDB();
            }

            return respuesta;
        }

        public static RespuestaCB obtenerCobranza(DateTime FECHAFIN, string CARDCODE)
        {
            RespuestaCB respuesta = new RespuestaCB();
            respuesta.Estado = false;
            respuesta.DescripcionError = "";
            respuesta.ListaRespuesta = "";

            DBSqlServerAux.ConectaDB();
            if (!DBSqlServerAux.Respuesta.Exito)
            {
                respuesta.Estado = false;
            }

            try
            {
                // Consulto la deuda total del ente                 
                SqlCommand com = new SqlCommand("SYP_REPORTE_AUXILIAR_CUENTAS_POR_COBRAR_MYDEALER", DBSqlServerAux.Conexion);

                com.CommandTimeout = 180;

                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.Add("@TAXDATE", SqlDbType.DateTime).Value = FECHAFIN;
                com.Parameters.AddWithValue("@CARDCODE", CARDCODE);

                SqlDataReader record = com.ExecuteReader();

                if (record.HasRows)
                {

                    List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();

                    Dictionary<string, string> row;

                    while (record.Read())
                    {
                        row = new Dictionary<string, string>();

                        for (int f = 0; f < record.FieldCount; f++)
                        {
                            row.Add(record.GetName(f), record.GetValue(f).ToString());
                        }

                        rows.Add(row);
                    }
                    respuesta.Estado = true;
                    var serializer = new JavaScriptSerializer();
                    serializer.MaxJsonLength = Int32.MaxValue;
                    respuesta.ListaRespuesta = serializer.Serialize(rows);
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                respuesta.DescripcionError = e.Message;
                logs.grabarLog("obtenerCobranza", e.Message);
                logs.grabarLog("obtenerCobranza_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServerAux.DesconectaDB();
            }

            return respuesta;
        }

        public static RespuestaCB obtenerCobranzaCompleta()
        {
            RespuestaCB respuesta = new RespuestaCB();
            respuesta.Estado = false;
            respuesta.DescripcionError = "";
            respuesta.ListaRespuesta = "";

            DBSqlServerAux.ConectaDB();
            if (!DBSqlServerAux.Respuesta.Exito)
            {
                respuesta.Estado = false;
            }

            try
            {
                // Consulto la deuda total del ente                 
                SqlCommand com = new SqlCommand("SYP_REPORTE_AUXILIAR_CUENTAS_POR_COBRAR", DBSqlServerAux.Conexion);

                com.CommandTimeout = 900;

                com.CommandType = CommandType.StoredProcedure;

                com.Parameters.Add("@TAXDATE", SqlDbType.DateTime).Value = DateTime.Now;

                SqlDataReader record = com.ExecuteReader();

                if (record.HasRows)
                {

                    List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();

                    Dictionary<string, string> row;

                    while (record.Read())
                    {
                        row = new Dictionary<string, string>();

                        for (int f = 0; f < record.FieldCount; f++)
                        {
                            row.Add(record.GetName(f), record.GetValue(f).ToString());
                        }

                        rows.Add(row);
                    }

                    respuesta.Estado = true;
                    var serializer = new JavaScriptSerializer();
                    serializer.MaxJsonLength = Int32.MaxValue;
                    respuesta.ListaRespuesta = serializer.Serialize(rows);
                }
                record.Close();
                record.Dispose();
                record = null;
                com.Dispose();
            }
            catch (Exception e)
            {
                respuesta.DescripcionError = e.Message;
                logs.grabarLog("obtenerCobranzaCompleta", e.Message);
                logs.grabarLog("obtenerCobranzaCompleta_DEBUG", e.StackTrace);
            }
            finally
            {
                DBSqlServerAux.DesconectaDB();
            }

            return respuesta;
        }
    }
}