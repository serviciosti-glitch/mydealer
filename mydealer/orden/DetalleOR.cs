using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class DetalleOR
    {
        public int idorden { get; set; }
        public int numlinea { get; set; }
        public int srorden { get; set; }
        public string codproducto { get; set; }
        public DateTime fechaorden { get; set; }
        public double cantidad { get; set; }
        public double cantidad_real { get; set; }
        public double precio { get; set; }
        public double descuento { get; set; }
        public double subtotal { get; set; }
        public int orden { get; set; }
        public double descuentoval { get; set; }
        public double impuesto { get; set; }
        public double total { get; set; }
        public DateTime fecharequerida { get; set; }
        public double pieza { get; set; }
        public double dscto_adi { get; set; }
        public string fue_enviado { get; set; }
        public int keyorganizacion { get; set; }
    }
}