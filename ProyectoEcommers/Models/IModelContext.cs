namespace ProyectoEcommers.Models

{
    using Microsoft.EntityFrameworkCore;
    using System;

    public interface IModelContext : IDisposable
    {
        DbSet<Ctr_usuarios> Ctr_usuarios { get; set; }
        DbSet<ctr_roles> ctr_roles { get; set; }
        DbSet<ctr_roles_user> ctr_roles_user { get; set; }
        DbSet<v_slider> v_slider { get; set; }
        DbSet<ctr_menu_roles> ctr_menu_roles { get; set; }
        DbSet<ctr_menu> ctr_menu { get; set; }
        DbSet<imagenes> imagenes { get; set; }
        DbSet<imagenesizquierda> imagenesizquierda { get; set; }
        DbSet<productos> productos { get; set; }
        DbSet<imagenes_producto> imagenes_producto { get; set; }
        DbSet<ImagenesInferior> ImagenesInferior { get; set; }
        DbSet<imagenesComentarios> imagenesComentarios { get; set; }
        DbSet<ctrl_dominios> ctrl_dominios { get; set; }
        DbSet<comentarios_clientes> comentarios_clientes { get; set; }
        DbSet<mi_carrito> mi_carrito { get; set; }
        DbSet<ctr_paises> ctr_paises { get; set; }
        DbSet<ctr_mensajes> ctr_mensajes { get; set; }
        DbSet<mi_compra_realizada> mi_compra_realizada { get; set; }
        DbSet<envio_correo> envio_correo { get; set; }



    }
}
