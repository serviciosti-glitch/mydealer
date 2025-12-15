using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class CabeceraST
    {
        public DateTime DocDate { get; set; }
        public string CardCode { get; set; }
        public string IdDevolucion { get; set; }
        public string FromWarehouse { get; set; }
        public string ToWarehouse { get; set; }
        public int Series { get; set; }
        public int SalesPersonCode { get; set; }
        public string Comments { get; set; }
        public DateTime U_fecha_caducidad { get; set; }
        public string U_Alm_Dist_Entrega { get; set; }
        public string U_BPP_MDSD { get; set; }
        public string U_SYP_TipoOrden { get; set; }
        public string U_SYP_Descripcion { get; set; }
        public string U_tipo_pedido { get; set; }
        public string U_Agencia { get; set; }
        public string U_Direccion { get; set; }
        public string U_LLP_PFinal { get; set; }
        public string U_DIR_PTO_ORIGEN { get; set; }
        public string Address { get; set; }
        public string U_tipo_soldev { get; set; }
    }
}