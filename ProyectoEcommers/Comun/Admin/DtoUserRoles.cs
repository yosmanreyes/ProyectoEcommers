using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProyectoEcommers.Models
{
    public class DtoUserRoles
    {

        [JsonPropertyName("IdRol")]
        public int? IdRol { get; set; }
        [JsonPropertyName("IdUserRol")]
        public Int32? IdUserRol { get; set; }
        [JsonPropertyName("IdUsuario")]
        public Int32? IdUsuario { get; set; }
        [JsonPropertyName("Descripcion")]
        public string? Descripcion { get; set; } = string.Empty;
        [JsonPropertyName("FechaCreacion")]
        public string? FechaCreacion { get; set; } = string.Empty;
        [JsonPropertyName("FuncionarioCreacion")]
        public string? FuncionarioCreacion { get; set; } = string.Empty;
        [JsonPropertyName("Vigente")]
        public string? Vigente { get; set; } = string.Empty;
        [JsonPropertyName("Rol")]
        public string? Rol { get; set; } = string.Empty;
        [JsonPropertyName("FechaFin")]
        public string? FechaFin { get; set; } = string.Empty;
        [JsonPropertyName("Justificacion")]
        public string? Justificacion { get; set; } = string.Empty;

        [JsonPropertyName("Maquina")]
        public string? Maquina { get; set; } = string.Empty;
        [JsonPropertyName("Usuario")]
        public Int64? Usuario { get; set; }
    }
}
