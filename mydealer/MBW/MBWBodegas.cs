using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class MBWBodegas
    {
        string codbodega;

        public string Codbodega
        {
            get { return codbodega; }
            set { codbodega = value; }
        }
        string descripcion;

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }
        string seriefacturacion;

        public string Seriefacturacion
        {
            get { return seriefacturacion; }
            set { seriefacturacion = value; }
        }
    }
}