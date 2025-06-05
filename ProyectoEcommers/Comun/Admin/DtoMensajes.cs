using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProyectoEcommers.Models
{
    public class DtoMensajes
    {
        [JsonPropertyName("Consecutivo")]
        public long Consecutivo { get; set; }        

        [JsonPropertyName("Mensaje")]
        public string? Mensaje { get; set; }
        [JsonPropertyName("CreadoPor")]
        public int CreadoPor { get; set; }
        [JsonPropertyName("FechaCreacion")]
        public DateTime FechaCreacion { get; set; }
        [JsonPropertyName("MaquinaCreacion")]
        public string? MaquinaCreacion { get; set; }
        [JsonPropertyName("TituloMensaje")]
        public string? TituloMensaje { get; set; }
        [JsonPropertyName("Vigente")]
        public string? Vigente { get; set; }
        [JsonPropertyName("FechaMensaje")]
        public DateTime FechaMensaje { get; set; }
        public List<DtoMensajes> DtoMensajesLis { get; set; }
    }
}
