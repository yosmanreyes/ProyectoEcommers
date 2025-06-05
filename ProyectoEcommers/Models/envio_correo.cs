
namespace ProyectoEcommers.Models

{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class envio_correo
        {
            [Key]
            public int idEnvioCorreo { get; set; }
            public int? idComentario { get; set; }
            public string? CorreoEnviar { get; set; }
            public string? Asunto { get; set; }
            public string? Mensaje { get; set; }
            public DateTime? fechaCreacion { get; set; }
            public string? MaquinaCreacion { get; set; }
            public string? CreadoPor { get; set; }
            public string? Vigente { get; set; }
            public string? EnvioMasivo { get; set; }
        }
}
