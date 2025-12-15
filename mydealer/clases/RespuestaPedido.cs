using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class RespuestaPedido
    {
        public bool creado { get; set; }
        public string numeroPedido { get; set; }
        public int numeroPedidoMydealer { get; set; }
        public string error { get; set; }
    }
}