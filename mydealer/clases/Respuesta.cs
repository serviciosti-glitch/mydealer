using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class Respuesta
    {
        private bool exito = false; // exito o fracaso de la operacion

        public bool Exito
        {
            get { return exito; }
            set { exito = value; }
        }
        private string codigoError; // el codigo del error (solo si exito=false)

        public string CodigoError
        {
            get { return codigoError; }
            set { codigoError = value; }
        }
        private string descripcionError; // la descripcion del error (solo si exito=false)

        public string DescripcionError
        {
            get { return descripcionError; }
            set { descripcionError = value; }
        }
        private string codigoRespuesta; // la respuesta (puede variar entre codigos, descripciones, etc)

        public string CodigoRespuesta
        {
            get { return codigoRespuesta; }
            set { codigoRespuesta = value; }
        }

        private int maxRegistros;

        public int MaxRegistros
        {
            get { return maxRegistros; }
            set { maxRegistros = value; }
        }

        private String registros; // xml

        public String Registros
        {
            get { return registros; }
            set { registros = value; }
        }

        private Object entradaRAW; // xml de entrada

        public Object EntradaRAW
        {
            get { return entradaRAW; }
            set { entradaRAW = value; }
        }
    }
}