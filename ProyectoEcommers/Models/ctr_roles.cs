

namespace ProyectoEcommers.Models

{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class ctr_roles
    {
        [Key]
        public int ID_ROL { get; set; }
        public string? DESCRIPCION { get; set; }
        public DateTime? FECHA_CREACION { get; set; }
        public int? VIGENTE { get; set; }
    }
}
