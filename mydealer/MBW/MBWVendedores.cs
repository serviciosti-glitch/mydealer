using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class MBWVendedores
    {
        string codvendedor;

        public string Codvendedor
        {
            get { return codvendedor; }
            set { codvendedor = value; }
        }

        string descripcion;

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        string codbodegadef;

        public string Codbodegadef
        {
            get { return codbodegadef; }
            set { codbodegadef = value; }
        }

        string codsupervisor;

        public string Codsupervisor
        {
            get { return codsupervisor; }
            set { codsupervisor = value; }
        }

        string codsucursal;

        public string Codsucursal
        {
            get { return codsucursal; }
            set { codsucursal = value; }
        }

        string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
    }
}