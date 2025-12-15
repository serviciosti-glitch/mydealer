using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class MBWClienteTipo
    {
        string codtipocliente;

        public string Codtipocliente
        {
            get { return codtipocliente; }
            set { codtipocliente = value; }
        }

        string descripcion;

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }
    }
}