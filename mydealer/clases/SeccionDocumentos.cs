using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class SeccionDocumentos
    {
        private string tipoDocumento; // tipodocumento - InvoiceType

        public string TipoDocumento
        {
            get { return tipoDocumento; }
            set { tipoDocumento = value; }
        }
        private int numeroDocumento; // documento - DocEntry

        public int NumeroDocumento
        {
            get { return numeroDocumento; }
            set { numeroDocumento = value; }
        }
        private int numeroCuota; // DocLine

        public int NumeroCuota
        {
            get { return numeroCuota; }
            set { numeroCuota = value; }
        }
        private int numeroCuotaSAP; // cuota_operacion

        public int NumeroCuotaSAP
        {
            get { return numeroCuotaSAP; }
            set { numeroCuotaSAP = value; }
        }
        private double valor; // valor - SumApplied y PaidSum

        public double Valor
        {
            get { return valor; }
            set { valor = value; }
        }
        private string numeroRetencion; // numeroRetencion U_no_reten solo en caso de pago con retencion

        public string NumeroRetencion
        {
            get { return numeroRetencion; }
            set { numeroRetencion = value; }
        }
        private double montoRetencion; // montoRetencion U_mto_reten solo en caso de retencion

        public double MontoRetencion
        {
            get { return montoRetencion; }
            set { montoRetencion = value; }
        }
    }
}