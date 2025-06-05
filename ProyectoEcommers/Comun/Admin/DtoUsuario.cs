using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProyectoEcommers.Models
{
    public class DtoUsuario
    {
        [JsonPropertyName("IdUsuario")]
        public Int32 IdUsuario { get; set; }
        [JsonPropertyName("IdCargo")]
        public Int32 IdCargo { get; set; }
        [JsonPropertyName("EmplConsecutivo")]
        public Int32 EmplConsecutivo { get; set; }
        [JsonPropertyName("EmplUndeConsecutivo")]
        public Int32 EmplUndeConsecutivo { get; set; }
        [JsonPropertyName("EmplUndeFuerza")]
        public Int32 EmplUndeFuerza { get; set; }
        [JsonPropertyName("Identificacion")]
        public Int64 Identificacion { get; set; }
        [JsonPropertyName("Usuario")]
        public string Usuario { get; set; }
        [JsonPropertyName("GradAlfabetico")]
        public string GradAlfabetico { get; set; }
        [JsonPropertyName("NombreGrado")]
        public string NombreGrado { get; set; }
        [JsonPropertyName("Funcionario")]
        public string Funcionario { get; set; }
        [JsonPropertyName("ApellidosNombres")]
        public string ApellidosNombres { get; set; }
        [JsonPropertyName("Cargo")]
        public string Cargo { get; set; }
        [JsonPropertyName("Dependencia")]
        public string Dependencia { get; set; }
        [JsonPropertyName("Fisica")]
        public string Fisica { get; set; }
        [JsonPropertyName("IdUndeLaborando")]
        public Int32 IdUndeLaborando { get; set; }
        [JsonPropertyName("Correo")]
        public string Correo { get; set; }
        [JsonPropertyName("Bloqueado")]
        public int Bloqueado { get; set; }
        [JsonPropertyName("Celular")]
        public Int64 Celular { get; set; }
        [JsonPropertyName("FechaCreacion")]
        public string? FechaCreacion { get; set; }
        [JsonPropertyName("DtoUserRoles")]
        public List<DtoUserRoles> DtoUserRoles { get; set; }
        [JsonPropertyName("Resultado")]
        public Int32? Resultado { get; set; }

        [JsonPropertyName("Apellidos")]
        public string? Apellidos { get; set; }

        [JsonPropertyName("Nombres")]
        public string? Nombres { get; set; }


        [JsonPropertyName("DireccionResidencia")]
        public string? DireccionResidencia { get; set; }


        [JsonPropertyName("Cliente")]
        public string? Cliente { get; set; }

        [JsonPropertyName("Vigente")]
        public string? Vigente { get; set; }

        [JsonPropertyName("Clave")]
        public string? Clave { get; set; }

        [JsonPropertyName("FechaNacimiento")]
        public string? FechaNacimiento { get; set; }

        [JsonPropertyName("FechaVigencia")]
        public string? FechaVigencia { get; set; }

        [JsonPropertyName("Maquina")]
        public string? Maquina { get; set; }

        [JsonPropertyName("IdRol")]
        public string? IdRol { get; set; }

        [JsonPropertyName("CantidadVentaProductos")]
        public Int32? CantidadVentaProductos { get; set; }

        [JsonPropertyName("CantidadMensajes")]
        public Int32? CantidadMensajes { get; set; }


    }
}
