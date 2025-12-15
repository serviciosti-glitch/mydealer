using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class MBWDescuentos
    {
        string tipocliente;

        public string Tipocliente
        {
            get { return tipocliente; }
            set { tipocliente = value; }
        }
        string tipoproducto;

        public string Tipoproducto
        {
            get { return tipoproducto; }
            set { tipoproducto = value; }
        }
        string codcliente;

        public string Codcliente
        {
            get { return codcliente; }
            set { codcliente = value; }
        }
        string codproducto;

        public string Codproducto
        {
            get { return codproducto; }
            set { codproducto = value; }
        }
        double descuento;

        public double Descuento
        {
            get { return descuento; }
            set { descuento = value; }
        }
        string cantidad;

        public string Cantidad
        {
            get { return cantidad; }
            set { cantidad = value; }
        }
        string tipodescuento;

        public string Tipodescuento
        {
            get { return tipodescuento; }
            set { tipodescuento = value; }
        }
        string prioridad;

        public string Prioridad
        {
            get { return prioridad; }
            set { prioridad = value; }
        }
        string fechainicial;

        public string Fechainicial
        {
            get { return fechainicial; }
            set { fechainicial = value; }
        }
        string fechafinal;

        public string Fechafinal
        {
            get { return fechafinal; }
            set { fechafinal = value; }
        }
    }
}