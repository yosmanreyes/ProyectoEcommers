namespace ProyectoEcommers.Models

{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class mi_carrito
    {
        [Key]
        public int id_carrito { get; set; }
        [ForeignKey("FK_mi_compra_realizada")]
        public int? id_compra_realizada { get; set; }
        public int? id_producto { get; set; }
        public string? Descripcion_Producto { get; set; }
        public int? Cantidad { get; set; }
        public int? precio_Unitario { get; set; }
        public int? Precio_Total { get; set; }   
        public int? Precio_Envio { get; set; }  
        public int? Descuento { get; set; }
        public string? Cancelado { get; set; }     
        public virtual mi_compra_realizada FK_mi_compra_realizada { get; set; }
    }
}
