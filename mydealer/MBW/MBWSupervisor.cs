using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class MBWSupervisor
    {
        string codsupervisor;

        public string Codsupervisor
        {
            get { return codsupervisor; }
            set { codsupervisor = value; }
        }

        string nombre;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        string email;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }
    }
}