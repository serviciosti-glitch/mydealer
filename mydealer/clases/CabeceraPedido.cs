using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class CabeceraPedido
    {
        public int numeroPedidoMydealer { get; set; } // numero_pedido
        public DateTime FechaEmison { get; set; }
        public string codcondicionpago { get; set; }
        public string codvendedor { get; set; }
        public string codcliente { get; set; }
        public string nombreCliente { get; set; }
        public string coddireccion { get; set; }
        public string ordenCompra { get; set; }
        public string direccion { get; set; }
        public DateTime FechaEntrega { get; set; }
        public string observacion { get; set; }
        public double subtotal { get; set; }
        public double totaligv { get; set; }
        public double total { get; set; }
        public string moneda { get; set; }

    }
}