using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class MBWStock
    {
        string codproducto;

        public string Codproducto
        {
            get { return codproducto; }
            set { codproducto = value; }
        }

        string codbodega;

        public string Codbodega
        {
            get { return codbodega; }
            set { codbodega = value; }
        }

        int stock;

        public int Stock
        {
            get { return stock; }
            set { stock = value; }
        }
    }
}