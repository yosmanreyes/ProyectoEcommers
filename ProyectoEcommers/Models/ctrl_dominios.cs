
namespace ProyectoEcommers.Models

{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ctrl_dominios
    {
        [Key]
        public Int32 ID_DOMINIO { get; set; }
        public string? DESCRIPCION { get; set; }
        public Int32? PADRE_ID { get; set; }
        public Int32? VIGENTE { get; set; }
        public string? ABREVIATURA { get; set; }
        public string? OBSERVACION { get; set; }
    }
}
