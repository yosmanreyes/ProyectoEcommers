using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoEcommers.Models
{
    public class DtoEnvioCorreo
    {

            public int IdEnvioCorreo { get; set; }
            public int? IdComentario { get; set; }
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
