using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class MBWVentas
    {
        int anio;
        public int Anio
        {
            get { return anio; }
            set { anio = value; }
        }

        int mes;
        public int Mes
        {
            get { return mes; }
            set { mes = value; }
        }

        string codproducto;
        public string Codproducto
        {
            get { return codproducto; }
            set { codproducto = value; }
        }

        int numClientes;
        public int NumClientes
        {
            get { return numClientes; }
            set { numClientes = value; }
        }

        double vtaDolares;
        public double VtaDolares
        {
            get { return vtaDolares; }
            set { vtaDolares = value; }
        }

        int vendedor;
        public int Vendedor
        {
            get { return vendedor; }
            set { vendedor = value; }
        }

        int numItems;
        public int NumItems
        {
            get { return numItems; }
            set { numItems = value; }
        }
    }
}