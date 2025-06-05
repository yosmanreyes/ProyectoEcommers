using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoEcommers.Models
{
    public class DtoComentariosClientes
    {
        public int idComentario { get; set; }      
        public string? Nombres { get; set; }
        public string? CorreoElectronico { get; set; }
        public string? NumeroTelefono { get; set; }
        public string? Comentarios { get; set; }
        public string? vigente { get; set; }
        public DateTime? fechaCreacion { get; set; }
        public string? maquinaCreacion { get; set; }
        public string fechaCreacionS { get { return fechaCreacion != null ? ((DateTime)fechaCreacion).ToString("dd/MM/yyyy HH:mm") : ""; } }
        public string? NumeroTelefonoConIndicativo { get; set; }
        public string? Atendio { get; set; }
        public string? EnvioCorreo { get; set; }
        public int idComentario2 { get; set; }
        public int idEnvio { get; set; }

    }
}
