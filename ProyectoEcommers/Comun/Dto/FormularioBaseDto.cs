using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Comun.Dto
{
    public class FormularioBaseDto
    {
        [JsonPropertyName("Consecutivo")]
        public decimal Consecutivo { get; set; }
        [JsonPropertyName("Empleado")]
        public string Empleado { get; set; }
        [JsonPropertyName("Identificacion")]
        public string Identificacion { get; set; }
        [JsonPropertyName("Direccion")]
        public string Direccion { get; set; }
        [JsonPropertyName("Telefono")]
        public string Telefono { get; set; }
    }
}
