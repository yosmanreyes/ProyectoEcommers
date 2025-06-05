using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoEcommers.Models
{
    public class DtoMiCompraRealizada
    {
        public int id_compra_realizada { get; set; }
        public string? Pais { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Nombre_Empresa { get; set; }
        public string? Direccion { get; set; }
        public string? DireccionS { get; set; }
        public string? Opcional_Direccion { get; set; }
        public string? Departamento { get; set; }
        public string? Ciudad { get; set; }
        public string? Codigo_Postal { get; set; }
        public string? Celular { get; set; }
        public string? Correo_Electronico { get; set; }
        public string? Comentario { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string? Fecha_CreacionEs { get; set; }
        public string Fecha_CreacionS { get { return Fecha_Creacion != null ? ((DateTime)Fecha_Creacion).ToString("dd/MM/yyyy hh:mm tt") : ""; } }
        public string? Maquina_Creacion { get; set; }
        public string? Vigente { get; set; }
        public string? Cancelada { get; set; }
        public string? Enviado_Satisfactoriamente { get; set; }
        public string? Empresa_Entrega { get; set; }
        public DateTime? Fecha_Llegada_producto { get; set; }
        public string Fecha_Llegada_productoS { get { return Fecha_Llegada_producto != null ? ((DateTime)Fecha_Llegada_producto).ToString("dd/MM/yyyy hh:mm tt") : ""; } }
        public DateTime? Fecha_Envio_producto { get; set; }
        public string Fecha_Envio_productoS { get { return Fecha_Envio_producto != null ? ((DateTime)Fecha_Envio_producto).ToString("dd/MM/yyyy hh:mm tt") : ""; } }
        //public string Fecha_Envio_productoS { get;  set; }
        public string? Estado_producto { get; set; }
        public string? Identifacion_modifica { get; set; }
        public string? Maquina_modifica { get; set; }
        public string? Producto { get; set; }      
        public string? Cantidad { get; set; }
        public string? PrecioTotal { get; set; }
        public string? CelularConIndicativo { get; set; }
        public List<DtoMiCarrito>? ListaMisCompras { get; set; }
        public string? EnvioCorreo { get; set; }
        public Int64? PrecioGeneral { get; set; }
        public string? FechaInicio { get; set; }
        public string? FechaFin { get; set; }

        public decimal? CostoEnvio { get; set; }

        public decimal? PrecioFinal { get; set; }

        public string? preferenceId { get; set; }

        public string? status { get; set; }

        public string? external_reference { get; set; }

        public string? merchant_order_id { get; set; }

        public string? preferenceId_Consulta_Inicial { get; set; }


        public string? EntregaMercanciaLocal { get; set; }
        public string? PagoNequi { get; set; }
        public string? PagoDaviplata { get; set; }
        public string? OtroMedioPago { get; set; }
        public string? PagoEfectivo { get; set; }

        //public string FechaCreacionStr { get { return FechaCreacion.ToShortDateString(); } }
        //public string FechaCreacionStrTime { get { return FechaCreacion.ToString("dd/MM/yyyy HH:mm"); } }
        //public DateTime? FechaModifica { get; set; }
        //public string FechaModificaStr { get { return FechaModifica != null ? ((DateTime)FechaModifica).ToShortDateString() : ""; } }
        //public string FechaModificaStrTime { get { return FechaModifica != null ? ((DateTime)FechaModifica).ToString("dd/MM/yyyy HH:mm") : ""; } }

    }
}
