

namespace ProyectoEcommers.Models

{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class mi_compra_realizada
    {
        [Key]
        public int id_compra_realizada { get; set; }
        public string? Pais { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Nombre_Empresa { get; set; }
        public string? Direccion { get; set; }
        public string? Opcional_Direccion { get; set; }
        public string? Departamento { get; set; }
        public string? Ciudad { get; set; }
        public string? Codigo_Postal { get; set; }
        public string? Celular { get; set; }
        public string? Correo_Electronico { get; set; }
        public string? Comentario { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
        public string? Maquina_Creacion { get; set; }
        public string? Vigente { get; set; }
        public string? Cancelada { get; set; }
        public string? Enviado_Satisfactoriamente { get; set; }
        public string? Empresa_Entrega { get; set; }
        public DateTime? Fecha_Llegada_producto { get; set; }
        public DateTime? Fecha_Envio_producto { get; set; }
        public string? Estado_producto { get; set; } = null;    
        public string? Identifacion_modifica { get; set; }
        public string? Maquina_modifica { get; set; }
        public string? CelularConIndicativo { get; set; }
        public string? EnvioCorreo { get; set; }

        public decimal? CostoEnvio { get; set; }

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


    }
}
