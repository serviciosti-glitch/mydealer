using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class MBWFormasPago
    {

        string codformapago;

        public string Codformapago
        {
            get { return codformapago; }
            set { codformapago = value; }
        }

        string descripcion;

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        int dias;

        public int Dias
        {
            get { return dias; }
            set { dias = value; }
        }

    }
}