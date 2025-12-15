using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class MBWDireccionCliente
    {
        string coddireccion;

        public string Coddireccion
        {
            get { return coddireccion; }
            set { coddireccion = value; }
        }

        string cliente;

        public string Cliente
        {
            get { return cliente; }
            set { cliente = value; }
        }

        string codcliente;

        public string Codcliente
        {
            get { return codcliente; }
            set { codcliente = value; }
        }

        string direccion;

        public string Direccion
        {
            get { return direccion; }
            set { direccion = value; }
        }
        string ciudad;

        public string Ciudad
        {
            get { return ciudad; }
            set { ciudad = value; }
        }

        string pais;

        public string Pais
        {
            get { return pais; }
            set { pais = value; }
        }

        string codAddress;

        public string CodAddress
        {
            get { return codAddress; }
            set { codAddress = value; }
        }

        int orden;

        public int Orden
        {
            get { return orden; }
            set { orden = value; }
        }

        string ciudadProvincia;

        public string CiudadProvincia
        {
            get { return ciudadProvincia; }
            set { ciudadProvincia = value; }
        }

        string provincia;

        public string Provincia
        {
            get { return provincia; }
            set { provincia = value; }
        }

        string telefono;

        public string Telefono
        {
            get { return telefono; }
            set { telefono = value; }
        }

    }
}