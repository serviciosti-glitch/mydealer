using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class RespuestaCB
    {
        string listaRespuesta;

        public string ListaRespuesta
        {
            get { return listaRespuesta; }
            set { listaRespuesta = value; }
        }
        //******************************************
        bool estado;
        public bool Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        string descripcionError;
        public string DescripcionError
        {
            get { return descripcionError; }
            set { descripcionError = value; }
        }
    }
}