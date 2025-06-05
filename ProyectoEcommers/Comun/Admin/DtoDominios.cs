

namespace ProyectoEcommers.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class DtoDominios
    {
        public Int32 IdDominio { get; set; }
        public string? Descripcion { get; set; }
        public Int32? PadreId { get; set; }
        public Int32? Vigente { get; set; }     
        public string? Abreviatura { get; set; }
        public string? Observacion { get; set; }
        public List<SelectListItem> Items { get; set; }
        public decimal? CostoEnvio { get; set; }
        public List<SelectListItem> Disponibilidades { get; set; }
    }
}
