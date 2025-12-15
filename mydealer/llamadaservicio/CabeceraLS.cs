using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class CabeceraLS
    {
        public string CustomerCode { get; set; }
        public string Subject { get; set; }
        public string itemCode { get; set; }
        public string Description { get; set; }
        public int Series { get; set; }
        public string IdDevolucion { get; set; }
        public int ProblemType { get; set; }
        public string Telephone { get; set; }
        public string Notes { get; set; }
        public string DocEntry { get; set; }
        public int DocType { get; set; }
        public string U_BK_NROT { get; set; }
        public string U_BK_TIPOSERV { get; set; }
        public string U_area_division { get; set; }
        public string U_MSS_ProfitCode { get; set; }
        public string U_MSS_OcrCode2 { get; set; }
        public string U_TIPO_RECURSO { get; set; }
        public string U_localidad { get; set; }
        public string U_vertical { get; set; }
        public string U_centro_costo { get; set; }
        public int U_num_soldev_det { get; set; }
    }
}