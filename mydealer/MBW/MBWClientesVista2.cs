using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class MBWClientesVista2
    {
        string codcliente;

        public string Codcliente
        {
            get { return codcliente; }
            set { codcliente = value; }
        }

        string codvendedor;

        public string Codvendedor
        {
            get { return codvendedor; }
            set { codvendedor = value; }
        }

        double limitecredito;

        public double Limitecredito
        {
            get { return limitecredito; }
            set { limitecredito = value; }
        }

        double saldocuenta;

        public double Saldocuenta
        {
            get { return saldocuenta; }
            set { saldocuenta = value; }
        }

        double cupoutilizado;

        public double Cupoutilizado
        {
            get { return cupoutilizado; }
            set { cupoutilizado = value; }
        }

        double saldopendiente;

        public double Saldopendiente
        {
            get { return saldopendiente; }
            set { saldopendiente = value; }
        }

        string documenta;

        public string Documenta
        {
            get { return documenta; }
            set { documenta = value; }
        }

        string diasvencidos;

        public string Diasvencidos
        {
            get { return diasvencidos; }
            set { diasvencidos = value; }
        }

        string codformapago;

        public string Codformapago
        {
            get { return codformapago; }
            set { codformapago = value; }
        }
    }
}