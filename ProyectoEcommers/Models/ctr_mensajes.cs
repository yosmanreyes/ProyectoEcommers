
namespace ProyectoEcommers.Models

{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class ctr_mensajes
    {
        [Key]
        public int CONSECUTIVO { get; set; }
        public string? MENSAJE { get; set; }
        public int? CREADO_POR { get; set; }
        public DateTime? FECHA_CREACION { get; set; }
        public string? MAQUINA_CREACION { get; set; }
        public int? ACTUALIZADO_POR { get; set; }
        public DateTime? FECHA_ACTUALIZA { get; set; }
        public string? MAQUINA_ACTUALIZA { get; set; }
        public string? VIGENTE { get; set; }
        public DateTime? FECHA_MENSAJE { get; set; }
        public string? TITULOMENSAJES { get; set; }
    }
}
