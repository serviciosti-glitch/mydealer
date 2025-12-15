using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mydealer
{
    public class CabeceraOrden
    {
        private int numeroOrdenWeb; // numero_pedido

        public int NumeroOrdenWeb
        {
            get { return numeroOrdenWeb; }
            set { numeroOrdenWeb = value; }
        }

        private int numeroOrden;

        public int NumeroOrden
        {
            get { return numeroOrden; }
            set { numeroOrden = value; }
        }

        private string codigoCliente; // codigo_cliente

        public string CodigoCliente
        {
            get { return codigoCliente; }
            set { codigoCliente = value; }
        }
        private string codigoCobrador; // Agent_Code

        public string CodigoCobrador
        {
            get { return codigoCobrador; }
            set { codigoCobrador = value; }
        }
        private string codigoDireccionEnvio; // codigo_direccion_envio

        public string CodigoDireccionEnvio
        {
            get { return codigoDireccionEnvio; }
            set { codigoDireccionEnvio = value; }
        }
        private string fechaGeneracionPedido; // fecha_registro_pedido

        public string FechaGeneracionPedido
        {
            get { return fechaGeneracionPedido; }
            set { fechaGeneracionPedido = value; }
        }
        private string fechaEntregaPedido;   //fecha de despacho del pedido

        public string FechaEntregaPedido
        {
            get { return fechaEntregaPedido; }
            set { fechaEntregaPedido = value; }
        }
        private int codigoVendedor; // codigo_vendedor

        public int CodigoVendedor
        {
            get { return codigoVendedor; }
            set { codigoVendedor = value; }
        }
        private string observaciones; // observaciones

        public string Observaciones
        {
            get { return observaciones; }
            set { observaciones = value; }
        }
        private string numeroOrdenPedidoFisico; // numero_fisico+

        public string NumeroOrdenPedidoFisico
        {
            get { return numeroOrdenPedidoFisico; }
            set { numeroOrdenPedidoFisico = value; }
        }
        private string aprobadoventa;    //Indicador de aprobacion por venta

        public string Aprobadoventa
        {
            get { return aprobadoventa; }
            set { aprobadoventa = value; }
        }
        private string codigoTransportista; // codigo_transportista

        public string CodigoTransportista
        {
            get { return codigoTransportista; }
            set { codigoTransportista = value; }
        }
        private string pedidoaprobado;   //indicador de aprobacion general de pedido

        public string Pedidoaprobado
        {
            get { return pedidoaprobado; }
            set { pedidoaprobado = value; }
        }
        private int codigoFormaPago; // codigo_formapago

        public int CodigoFormaPago
        {
            get { return codigoFormaPago; }
            set { codigoFormaPago = value; }
        }
        private string codigoServicioCliente; // codigo_servicio_cliente

        public string CodigoServicioCliente
        {
            get { return codigoServicioCliente; }
            set { codigoServicioCliente = value; }
        }
        private string codmotivonoaprobacion;    //Codigo del motivo por el cual el pedido no es aprobado

        public string Codmotivonoaprobacion
        {
            get { return codmotivonoaprobacion; }
            set { codmotivonoaprobacion = value; }
        }
        private string detallemotivoaprobacion; // Detalle del motivo de aprobacion

        public string Detallemotivoaprobacion
        {
            get { return detallemotivoaprobacion; }
            set { detallemotivoaprobacion = value; }
        }
        private double totalPedido; // DocTotal

        public double TotalPedido
        {
            get { return totalPedido; }
            set { totalPedido = value; }
        }
        private string descprioridad;    //Decripcion de la prioridad del pedido

        public string Descprioridad
        {
            get { return descprioridad; }
            set { descprioridad = value; }
        }
        // campos adicionales por flete - solo aplica en Ecuador
        private double totalGasto;

        public double TotalGasto
        {
            get { return totalGasto; }
            set { totalGasto = value; }
        }

        // Campos usados cuando cabecera de orden se usa en reclamos - jvillavi
        private string numeroReclamo;

        public string NumeroReclamo
        {
            get { return numeroReclamo; }
            set { numeroReclamo = value; }
        }
        private string codigoMotivoReclamo;

        public string CodigoMotivoReclamo
        {
            get { return codigoMotivoReclamo; }
            set { codigoMotivoReclamo = value; }
        }

        string ruta_logistica;

        public string Ruta_logistica
        {
            get { return ruta_logistica; }
            set { ruta_logistica = value; }
        }

        int ruta_secuencia;

        public int Ruta_secuencia
        {
            get { return ruta_secuencia; }
            set { ruta_secuencia = value; }
        }

        string pedir_autorizacion;

        public string Pedir_autorizacion
        {
            get { return pedir_autorizacion; }
            set { pedir_autorizacion = value; }
        }

        string tipo_pedido;

        public string Tipo_pedido
        {
            get { return tipo_pedido; }
            set { tipo_pedido = value; }
        }



        double subotal;
        public double Subotal
        {
            get { return subotal; }
            set { subotal = value; }
        }

        double valor_descto;
        public double Valor_descto
        {
            get { return valor_descto; }
            set { valor_descto = value; }
        }

        int series;
        public int Series
        {
            get { return series; }
            set { series = value; }
        }

        //  PEDIDO REALIZADO EN LINEA
        string SYP_ONLINE;
        public string U_SYP_ONLINE
        {
            get { return SYP_ONLINE; }
            set { SYP_ONLINE = value; }
        }

        //  NUMERO DE ORDEN COMPRA
        string FE_ordenCompra;
        public string U_FE_ordenCompra
        {
            get { return FE_ordenCompra; }
            set { FE_ordenCompra = value; }
        }

        //  CODIGO ALMACEN
        string FE_codigoAlmacen;
        public string U_FE_codigoAlmacen
        {
            get { return FE_codigoAlmacen; }
            set { FE_codigoAlmacen = value; }
        }

        string Costo_region;
        public string costo_region
        {
            get { return Costo_region; }
            set { Costo_region = value; }
        }

        string Costo_area;
        public string costo_area
        {
            get { return Costo_area; }
            set { Costo_area = value; }
        }

        string Costo_departamento;
        public string costo_departamento
        {
            get { return Costo_departamento; }
            set { Costo_departamento = value; }
        }

        string u_persona_consig;
        public string U_persona_consig
        {
            get { return u_persona_consig; }
            set { u_persona_consig = value; }
        }

        string u_horario_recoge;
        public string U_horario_recoge
        {
            get { return u_horario_recoge; }
            set { u_horario_recoge = value; }
        }

        string u_Alm_Dist_Entrega;
        public string U_Alm_Dist_Entrega
        {
            get { return u_Alm_Dist_Entrega; }
            set { u_Alm_Dist_Entrega = value; }
        }

        string u_tipo_pedido;
        public string U_tipo_pedido
        {
            get { return u_tipo_pedido; }
            set { u_tipo_pedido = value; }
        }

        string u_Entrega;
        public string U_Entrega
        {
            get { return u_Entrega; }
            set { u_Entrega = value; }
        }

        string u_Agencia;
        public string U_Agencia
        {
            get { return u_Agencia; }
            set { u_Agencia = value; }
        }

        string u_Direccion;
        public string U_Direccion
        {
            get { return u_Direccion; }
            set { u_Direccion = value; }
        }
    }
}