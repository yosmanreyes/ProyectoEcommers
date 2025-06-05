using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoEcommers.Models
{
    public class DtoMiCarrito
    {
        public int id_carrito { get; set; }
        public int? id_compra_realizada { get; set; }
        public int? id_producto { get; set; }
        public string? Descripcion_Producto { get; set; }
        public int? Cantidad { get; set; }
        public int? precio_Unitario { get; set; }
        public int? Precio_Total { get; set; }
        public int? Precio_Envio { get; set; }
        public int? Descuento { get; set; }
        public string? Cancelado { get; set; }
        public string? Producto { get; set; }
        public string? Marca { get; set; }
        public string? Categoria { get; set; }


        public List<DtoImagenesProducto>? ListaImagenesC { get; set; }

    }
}
