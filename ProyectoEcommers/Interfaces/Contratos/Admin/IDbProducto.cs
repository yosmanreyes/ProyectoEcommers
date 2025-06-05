

namespace ProyectoEcommers.Models
{

    using Comun.Dto;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IDbProducto
    {
        public Task<int> F_InsProducto(DtoProductos producto);
        public Task<List<DtoProductos>> F_GetListaProductos(int marca, int categoria);
        public Task<int> F_InsCompraProducto(DtoMiCompraRealizada producto);
        public Task<int> F_InsCarrito(List<DtoMiCarrito> producto);
        public Task<int> InsProductosMercadoLibre(string preferenceId, int NumeroCompra);
      


        public Task<List<DtoProductos>> F_GetProductosGrilla();
        public Task<List<DtoProductos>> F_GetProductosGrillaDesactivado();
        public Task<List<DtoProductos>> F_GetProductosGrillaTotal();
        public Task<List<DtoImagenesProducto>> F_GetImagenesGrilla(int _idProducto);
        public Task<RespuestaDto<List<DtoProductos>>> F_UpdateProducto(DtoProductos Imagen);
        public Task<RespuestaDto<List<DtoImagenesProducto>>> F_UpdateImagenProducto(DtoImagenesProducto Imagen);
        public Task<List<DtoProductos>> F_GetBuscaIntelEmple(string search);
        public Task<List<DtoMiCompraRealizada>> F_GetProductosMisCompras(string correo);
        public Task<List<DtoMiCompraRealizada>> F_GetProductosMisComprasId(Int32 idCompra);        
        public Task<List<DtoMiCompraRealizada>> F_GetProductosVentas();
        public Task<List<DtoMiCompraRealizada>> F_GetProductosVentasPdf();
        public Task<List<DtoMiCompraRealizada>> F_GetProductosVentasFactura(int id_compra);
        public Task<List<DtoMiCompraRealizada>> F_GetProductosVentasRealizadas();
        public Task<List<DtoMiCompraRealizada>> F_GetProductosTotal();
        public Task<RespuestaDto<List<DtoMiCompraRealizada>>> UpdateProductoVenta(int IdVenta, string Descripcion, string MaquinaModifca, int UsuarioModifica, string _columna);
        public Task<List<DtoComentariosClientes>> F_GetComentarios();
        public Task<List<DtoComentariosClientes>> F_GetConsultaCorreos();
        public Task<List<DtoDominios>> F_GetProductosGrillaMarca();
        public Task<List<DtoDominios>> F_GetProductosGrillaCategoria();
        public Task<RespuestaDto<List<DtoDominios>>> F_UpdateMarca(DtoDominios DtoDominios);
        public Task<RespuestaDto<List<DtoDominios>>> F_UpdateCategoria(DtoDominios DtoDominios);
        public Task<RespuestaDto<List<DtoDominios>>> F_AddMarca(DtoDominios DtoDominios);
        public Task<RespuestaDto<List<DtoDominios>>> F_AddCategoria(DtoDominios DtoDominios);
        public Task<RespuestaDto<List<DtoComentariosClientes>>> F_UpdateComentarios(DtoComentariosClientes Imagen);
        public Task<RespuestaDto<List<DtoComentariosClientes>>> F_UpdateEnvioCorreo(int Imagen);
        public Task<RespuestaDto<List<DtoComentariosClientes>>> F_UpdateEnvioCorreoBandeja(int Imagen);
        public Task<RespuestaDto<List<DtoMiCompraRealizada>>> F_totalVentasPorEnviar();
        public Task<RespuestaDto<List<DtoComentariosClientes>>> F_totalComentarios();
        public Task<List<DtoCiudadEnvia>> F_GetGrillaCiudadEnvia(int numero);
        public Task<RespuestaDto<List<DtoCiudadEnvia>>> UpdateCiudad(DtoCiudadEnvia DtoDominios);
        public Task<RespuestaDto<List<DtoCiudadEnvia>>> F_AddCiudad(DtoCiudadEnvia DtoDominios);
        public Task<RespuestaDto<List<DtoCiudadEnvia>>> F_UpdateCiudad(DtoCiudadEnvia DtoDominios);


        public Task<int> F_InsPreferenceId(Int32 idCompra, string preferenceId, string Idstatus, string Idexternal_reference, string Idmerchant_order_id);

        public Task<int> F_UpdateCompra(DtoMiCompraRealizada dto);

    }
}
