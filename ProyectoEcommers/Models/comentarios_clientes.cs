

namespace ProyectoEcommers.Models

{

    using System;
    using System.ComponentModel.DataAnnotations;
    public class comentarios_clientes
    {

        [Key]
        public int idComentario { get; set; }
        public string? Nombres { get; set; }
        public string? CorreoElectronico { get; set; }
        public string? NumeroTelefono { get; set; }
        public string? Comentarios { get; set; }
        public string? vigente { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string? maquinaCreacion { get; set; }

        public string? NumeroTelefonoConIndicativo { get; set; }
        public string? Atendio { get; set; }
        public string? EnvioCorreo { get; set; }


    }
}
