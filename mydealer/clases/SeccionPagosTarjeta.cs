using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class SeccionPagosTarjeta
    {
        private int tarjetaCredito; // CreditCard

        public int TarjetaCredito
        {
            get { return tarjetaCredito; }
            set { tarjetaCredito = value; }
        }
        private string cuentaTarjetaCredito; // CreditAcct

        public string CuentaTarjetaCredito
        {
            get { return cuentaTarjetaCredito; }
            set { cuentaTarjetaCredito = value; }
        }
        private string numeroDocumento; // CreditCardNumber - folioNum

        public string NumeroDocumento
        {
            get { return numeroDocumento; }
            set { numeroDocumento = value; }
        }
        private string numeroReclamo; // VoucherNum - numero aleatorio

        public string NumeroReclamo
        {
            get { return numeroReclamo; }
            set { numeroReclamo = value; }
        }
        private string numeroFactura; // OwnerIdNum - numeroDocumento

        public string NumeroFactura
        {
            get { return numeroFactura; }
            set { numeroFactura = value; }
        }
        private double sumaCredito; // CreditSum

        public double SumaCredito
        {
            get { return sumaCredito; }
            set { sumaCredito = value; }
        }
    }
}