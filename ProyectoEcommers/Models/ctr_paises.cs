

namespace ProyectoEcommers.Models

{
    using System.ComponentModel.DataAnnotations;
    public class ctr_paises
    {
        [Key]
        public int CODIGO { get; set; }
        public string? DESCRIPCION { get; set; }
        public string? ABREVIATURA { get; set; }
        public string? TIPO { get; set; }
        public string? ZONA { get; set; }
        public int? LUGE_CODIGO { get; set; }
        public string? VIGENTE { get; set; }
        public string? CREADO_POR { get; set; }
        public string? MAQUINA_CREACION { get; set; }
        public int? FECHA_CREACION { get; set; }
        public string? INDICATIVO { get; set; }
        public decimal? COSTO_ENVIO { get; set; }
    }
}
