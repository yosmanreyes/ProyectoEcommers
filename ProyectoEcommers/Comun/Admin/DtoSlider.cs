using System.Text.Json.Serialization;
using System;
using System.Web;
namespace ProyectoEcommers.Models
{
    public class DtoSlider
    {
       
        [JsonPropertyName("Consecutivo")]
        public long Consecutivo { get; set; }
        [JsonPropertyName("ContentType")]
        public string? ContentType { get; set; }
        [JsonPropertyName("FileName")]
        public string? FileName { get; set; }
        [JsonPropertyName("Foto")]
        public string? Foto { get; set; }
        [JsonPropertyName("Ruta")]
        public string? Ruta { get; set; }
        [JsonPropertyName("ImagesConsecutivo")]
        public string? ImagesConsecutivo { get; set; }
        [JsonPropertyName("Url")]
        public string? Url { get; set; }
        [JsonPropertyName("UrlLink")]
        public string? UrlLink { get; set; }
        [JsonPropertyName("FotoBase64")]
        public string FotoBase64 { get; set; }
        [JsonPropertyName("Identificacion")]
        public Int64 Identificacion { get; set; }
        [JsonPropertyName("Usuario")]
        public Int64 Usuario { get; set; }
        [JsonPropertyName("RutaFoto")]
        public string? RutaFoto { get; set; }
        [JsonPropertyName("Maquina")]
        public string? Maquina { get; set; }
        [JsonPropertyName("Vigente")]
        public int Vigente { get; set; }
        public List<DtoSlider> DtoSliderLis { get; set; }

    }
}
