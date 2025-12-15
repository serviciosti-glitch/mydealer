using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class AutorizacionOR
    {
        public int idorden { get; set; }
        public int id { get; set; }
        public int srorden { get; set; }
        public string tipo { get; set; }
        public string descripcion { get; set; }
        public string detalle { get; set; }
        public string autorizado { get; set; }
        public DateTime fecha_sys { get; set; }
        public string user { get; set; }
        public DateTime fecha_autorizacion { get; set; }
        public string usuario_autoriza { get; set; }
        public string notificado { get; set; }
        public int keyorganizacion { get; set; }
    }
}