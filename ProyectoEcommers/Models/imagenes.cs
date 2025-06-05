

namespace ProyectoEcommers.Models

{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class imagenes
    {
        [Key]
        public int CONSECUTIVO { get; set; }
        public string? CONTENT_TYPE { get; set; }
        public string? FILENAME { get; set; }
        public byte[]? FOTO { get; set; }
        public int? CREADO_POR { get; set; }
        public DateTime? FECHA_CREACION { get; set; }
        public string? MAQUINA_CREACION { get; set; }
        public int? ACTUALIZADO_POR { get; set; }
        public DateTime? FECHA_ACTUALIZA { get; set; }
        public string? MAQUINA_ACTUALIZA { get; set; }
        public string? URL { get; set; }
        public int? VIGENTE { get; set; }
    }
}
