using System.Text.Json.Serialization;
using System;
using System.Web;
namespace ProyectoEcommers.Models
{
    public class DtoProductos
    {
       
        [JsonPropertyName("ProductoId")]
        public int PRODUCTO_ID { get; set; }
        [JsonPropertyName("MarcaId")]
        public int? MARCA_ID { get; set; }
        [JsonPropertyName("Nombre")]
        public string? NOMBRE { get; set; }
        [JsonPropertyName("Descripcion")]
        public string? DESCRIPCION { get; set; }
        [JsonPropertyName("CategoriaId")]
        public int? CATEGORIA_ID { get; set; }
        [JsonPropertyName("Cantidad")]
        public int? CANTIDAD { get; set; }
        [JsonPropertyName("FechaCreacion")]
        public DateTime? FECHA_CREACION { get; set; }
        public string FECHA_CREACIONS { get { return FECHA_CREACION != null ? ((DateTime)FECHA_CREACION).ToString("dd/MM/yyyy HH:mm") : ""; } }
        [JsonPropertyName("MaquinaCreacion")]
        public string? MAQUINA_CREACION { get; set; }
        [JsonPropertyName("ActualizadoPor")]
        public int? ACTUALIZADO_POR { get; set; }
        [JsonPropertyName("Precio")]
        public decimal? PRECIO { get; set; }
        [JsonPropertyName("CantidadTotal")]
        public int? CANTIDAD_TOTAL { get; set; }
        [JsonPropertyName("Vigente")]
        public int? VIGENTE { get; set; }

        public string? Marca { get; set; }

        public string? Categoria { get; set; }
        [JsonPropertyName("CantidadVenta")]
        public int? CANTIDAD_VENTA { get; set; }

        public List<DtoImagenesProducto>? ListaImagenes { get; set; }

    }
}
