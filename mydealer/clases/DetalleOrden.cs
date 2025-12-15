using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class DetalleOrden
    {
        public string CodigoProducto { get; set; }
        public double CantidadProducto { get; set; }
        public double PrecioProducto { get; set; }
        public double Dscto_adicional { get; set; }
        public string Wsc { get; set; }
        public double PorcentajeDescuento { get; set; }
        public string Taxcode { get; set; }
        public double PorcIva { get; set; }
    }
}