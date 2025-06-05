

namespace ProyectoEcommers.Models

{

    using System;
    using System.ComponentModel.DataAnnotations;
    public class ctr_menu
    {
        [Key]
        public int ID_MENU { get; set; }
        public string DESCRIPCION { get; set; }
        public int PADREID { get; set; }
        public int? POSICION { get; set; }
        public string? AREA { get; set; }
        public string? CONTROLADOR { get; set; }
        public string? VISTA { get; set; }
        public string? ICONO { get; set; }
        public DateTime? FECHA_CREACION { get; set; }
        public int? USUARIO_CREACION { get; set; }
        public string? MAQUINA_CREACION { get; set; }
        public DateTime? FECHA_MODIFICA { get; set; }
        public int? USUARIO_MODIFICA { get; set; }
        public string? MAQUINA_MODIFICA { get; set; }
        public string? TIPO { get; set; }
        public int VIGENTE { get; set; }
        public string? DETALLE { get; set; }
    }
}
