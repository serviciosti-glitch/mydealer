using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class SeccionPagosLetrasCambio
    {
        private string numeroLetra; // ReferenceNo

        public string NumeroLetra
        {
            get { return numeroLetra; }
            set { numeroLetra = value; }
        }
        private string codigoBancoAsociado; // BPBankCode

        public string CodigoBancoAsociado
        {
            get { return codigoBancoAsociado; }
            set { codigoBancoAsociado = value; }
        }
        private string cuentaBancariaBancoAsociado; // BPBankAct

        public string CuentaBancariaBancoAsociado
        {
            get { return cuentaBancariaBancoAsociado; }
            set { cuentaBancariaBancoAsociado = value; }
        }
        private string numeroFolio; // FolioNumber

        public string NumeroFolio
        {
            get { return numeroFolio; }
            set { numeroFolio = value; }
        }
        private string prefijoFolio; // FolioPrefixString - Se quema LT

        public string PrefijoFolio
        {
            get { return prefijoFolio; }
            set { prefijoFolio = value; }
        }
        private string fechaVencimientoLetra; // BillOfExchangeDueDate

        public string FechaVencimientoLetra
        {
            get { return fechaVencimientoLetra; }
            set { fechaVencimientoLetra = value; }
        }
    }
}