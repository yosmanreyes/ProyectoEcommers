using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProyectoEcommers.Models
{
    public class DtoImagenesProducto
    {
       
        [JsonPropertyName("ImagenId")]
        public int IMAGEN_ID { get; set; }
        [JsonPropertyName("ProductoId")]
        public int? PRODUCTO_ID { get; set; }
        [JsonPropertyName("ContentType")]
        public string? CONTENT_TYPE { get; set; }
        [JsonPropertyName("FileName")]
        public string? FILENAME { get; set; }
        [JsonPropertyName("Foto")]
        public string? FOTO { get; set; }
        [JsonPropertyName("CreadoPor")]
        public int? CREADO_POR { get; set; }
        [JsonPropertyName("FechaCreacion")]
        public DateTime? FECHA_CREACION { get; set; }
        [JsonPropertyName("FechaCreacionS")]
        public string FECHA_CREACIONS { get { return FECHA_CREACION != null ? ((DateTime)FECHA_CREACION).ToString("dd/MM/yyyy HH:mm") : ""; } }
        [JsonPropertyName("MaquinaCreacion")]
        public string? MAQUINA_CREACION { get; set; }
        [JsonPropertyName("ActualizadoPor")]
        public int? ACTUALIZADO_POR { get; set; }
        [JsonPropertyName("FechaActualiza")]
        public DateTime? FECHA_ACTUALIZA { get; set; }
        [JsonPropertyName("MaquinaActualiza")]
        public string? ImagenId { get; set; }
        [JsonPropertyName("Url")]
        public string? URL { get; set; }
        [JsonPropertyName("Vigente")]
        public int? VIGENTE { get; set; }

    }
}
