using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class DetallePedido
    {
        public int linea { get; set; }
        public string codproducto { get; set; }
        public string nombreProducto { get; set; }
        public string codbodega { get; set; }
        public string observacion { get; set; }
        public double precioUnitario { get; set; }
        public double precioConDescuento { get; set; }
        public int porcentajeDescuento { get; set; }
        public double descuentoUnitario { get; set; }
        public double cantidad { get; set; }
        public double igv { get; set; }
        public double totalIgv { get; set; }
        public double subtotal { get; set; }
        public double total { get; set; }
    }
}