
namespace ProyectoEcommers.Models

{

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class productos
    {
        [Key]
        public int PRODUCTO_ID { get; set; }

        [ForeignKey("FK_MARCA")]
        public int? MARCA_ID { get; set; }
        public string? NOMBRE { get; set; }
        public string? DESCRIPCION { get; set; }

        [ForeignKey("FK_CATEGORIA")]
        public int? CATEGORIA_ID { get; set; }
        public int? CANTIDAD { get; set; }
        public DateTime? FECHA_CREACION { get; set; }
        public string? MAQUINA_CREACION { get; set; }
        public int? ACTUALIZADO_POR { get; set; }
        public decimal? PRECIO { get; set; }
        public int? CANTIDAD_TOTAL { get; set; }
        public int? VIGENTE { get; set; }

        public virtual ctrl_dominios FK_MARCA { get; set; }
        public virtual ctrl_dominios FK_CATEGORIA { get; set; }
    }
}
