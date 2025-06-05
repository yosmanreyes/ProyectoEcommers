
namespace ProyectoEcommers.Models

{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class imagenes_producto
    {
        [Key]
        public int IMAGEN_ID { get; set; }

        [ForeignKey("fk_Producto")]
        public int? PRODUCTO_ID { get; set; }
        public string? CONTENT_TYPE { get; set; }
        public string? FILENAME { get; set; }
        public string? FOTO { get; set; }
        public int? CREADO_POR { get; set; }
        public DateTime? FECHA_CREACION { get; set; }
        public string? MAQUINA_CREACION { get; set; }
        public int? ACTUALIZADO_POR { get; set; }
        public DateTime? FECHA_ACTUALIZA { get; set; }
        public string? MAQUINA_ACTUALIZA { get; set; }
        public string? URL { get; set; }
        public int? VIGENTE { get; set; }

        public virtual productos? fk_Producto { get; set; }

    }
}
