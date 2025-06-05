

namespace ProyectoEcommers.Models

{
    using System.ComponentModel.DataAnnotations;
    public class v_slider
    {
        [Key]
        public string CONSECUTIVO { get; set; }
        public string IMAGENES_CONSECUTIVO { get; set; }
        public string URL { get; set; }
        public string FILENAME { get; set; }
        public int ORDEN { get; set; }
        public string? URL_LINK { get; set; } = null!;
    }
}
