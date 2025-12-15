using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class MBWListaPrecioDet
    {
        string codlistaprecio;

        public string Codlistaprecio
        {
            get { return codlistaprecio; }
            set { codlistaprecio = value; }
        }

        string codproducto;

        public string Codproducto
        {
            get { return codproducto; }
            set { codproducto = value; }
        }

        double precio;

        public double Precio
        {
            get { return precio; }
            set { precio = value; }
        }
    }
}