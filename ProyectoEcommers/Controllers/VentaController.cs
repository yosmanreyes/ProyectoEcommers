namespace ProyectoEcommers.Controllers
{
 
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Negocio.Contratos.ConsultasExternas;
    using System.Data;
    using System.IO;
    using System.Security.Claims;
    using ProyectoEcommers.Models;
    using ProyectoEcommers.Helper;
    using static ProyectoEcommers.Controllers.CuentaController;

    //[Authorize(Roles = "1,2,3,4")]
    public class VentaController : Controller
    {

        private readonly IDbProducto _IDbProducto;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDbAdministracion _dbAdministracion;
        private readonly IBLConsultasExternas _BlConsultasExternas;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AppSettings _appSettings;

        public VentaController(IDbProducto iDbProducto, IHttpContextAccessor httpContextAccessor, IDbAdministracion dbAdministracion, IBLConsultasExternas bLConsultasExternas, IWebHostEnvironment webHostEnvironment, IOptions<AppSettings> options)
        {
            _IDbProducto = iDbProducto;
            _httpContextAccessor = httpContextAccessor;
            _dbAdministracion = dbAdministracion;
            _BlConsultasExternas = bLConsultasExternas;
            _webHostEnvironment = webHostEnvironment;
            _appSettings = options.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Bandeja()
        {
            try
            {
                ViewBag.RutaVisualizador = _appSettings.RutaVisualizador;
                var ImagenesSlider = await _BlConsultasExternas.F_GetCarruselImagenesAsyc();
                var SlidersView = new List<DtoSlider>();
                return View(SlidersView);

            }
            catch (Exception e)
            {
                var SlidersView = new List<DtoSlider>();
                var SliderView = new DtoSlider
                {
                    Consecutivo = 19957,
                    ContentType = "image/jpeg",
                    FileName = "ARTE4_polired.jpg",
                    Ruta = "~/img/Productos/1.jpg"
                };
                SlidersView.Add(SliderView);
                return View(SlidersView);
            }
        }
        [AllowAnonymous]
        public async Task<JsonResult> F_GetProductosVentas()
        {
            var result = await _IDbProducto.F_GetProductosVentas();

            if (result.Count > 0)
            {
                var r = F_totalVentasPorEnviar();
                //var resultado = Json(new { success = true, data = result.OrderByDescending(x => x.Fecha_CreacionS) });
                //resultado.MaxJsonLength = int.MaxValue;
                //return Json(resultado);
                var Msj = "Total ventas por despachar: " + result.Count();
                return Json(new { success = true, data = result.OrderByDescending(x => x.id_compra_realizada), Mensaje = Msj });
            }
            else
            {
                return Json(new { success = false, data = result });
            }
        }
        [AllowAnonymous]
        public async Task<JsonResult> F_GetProductosVentasRealizadas()
        {
            var result = await _IDbProducto.F_GetProductosVentasRealizadas();

            if (result.Count > 0)
            {
                var r = F_totalVentasPorEnviar();
                //var resultado = Json(new { success = true, data = result.OrderByDescending(x => x.Fecha_CreacionS) });
                //resultado.MaxJsonLength = int.MaxValue;
                //return Json(resultado);
                var Msj = "Total ventas despachadas: " + result.Where(x => x.Cancelada != "SI").Count() + ", Total ventas canceladas: " + result.Where(x => x.Cancelada == "SI").Count();
                return Json(new { success = true, data = result.OrderByDescending(x => x.id_compra_realizada), Mensaje = Msj });
            }
            else
            {
                var Msj = "Total ventas despachadas: " + result.Count() + ", Total ventas canceladas: " + result.Where(x => x.Cancelada == "SI").Count();
                return Json(new { success = false, data = result, Mensaje = Msj });
            }
        }

        private string GetClientIpAddress(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrEmpty(ipAddress) && context.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ipAddress = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            }

            return ipAddress ?? "0.0.0.0";
        }

        public async Task<JsonResult> UpdateProductoVenta(int _IdVentaProducto, string _columna, string _descripcion)
        {

            var V_Maquina = GetClientIpAddress(HttpContext);
            var V_Usuario = Convert.ToInt32(User.FindFirstValue("Identificacion"));

            var retorno = await _IDbProducto.UpdateProductoVenta(_IdVentaProducto, _descripcion, V_Maquina, V_Usuario, _columna);
            if (retorno.Estado)
            {
                var r = F_totalVentasPorEnviar();
                CantidadVentaProductos.CantidadVentaProducto = Convert.ToString(retorno.Id);
                //var auditoria = _DbAdministracion.Ins_Auditoria(V_Usuario, "InsUsuarios", "Cambio estado de usuario a: " + dto.Bloqueado, Convert.ToString(dto.Identificacion), V_Maquina);
                return Json(new { success = true, data = retorno.Id, message = retorno.Mensaje });
            }
            else
            {
                return Json(new { success = false, data = retorno.Respuesta, message = retorno.Mensaje });
            }
        }
        [HttpPost]
        public async Task<int> F_totalVentasPorEnviar()
        {
            var retorno = await _IDbProducto.F_totalVentasPorEnviar();
            if (retorno.Estado)
            {
                CantidadVentaProductos.CantidadVentaProducto = Convert.ToString(retorno.Id);
                //var auditoria = _DbAdministracion.Ins_Auditoria(V_Usuario, "InsUsuarios", "Cambio estado de usuario a: " + dto.Bloqueado, Convert.ToString(dto.Identificacion), V_Maquina);
                return 1;
            }
            else
            {
                return 0;
            }
        }
        //Excel con contraseña
        [HttpGet]
        public async Task<FileContentResult> ExcelGeneral()
        {
            try
            {
                var clave = User.FindFirstValue("Clave");
                var retorno = await _IDbProducto.F_GetProductosVentas();
                string[] columns = { "id_compra_realizada", "Nombres", "Apellidos", "Nombre_Empresa", "Direccion", "Opcional_Direccion", "Pais", "Departamento", "Ciudad", "Codigo_Postal", "Celular", "Correo_Electronico", "Comentario", "Fecha_CreacionS", "Vigente", "Cancelada", "Enviado_Satisfactoriamente", "Empresa_Entrega", "Fecha_Envio_productoS", "Fecha_Llegada_productoS", "Estado_producto", "Identifacion_modifica", "Cantidad", "CostoEnvio", "PrecioTotal", "PrecioGeneral" };

                byte[] filecontent = ExcelExportHelperPass.ExportExcel(retorno.ToList(), "Listado General Ventas Por Despachar", Convert.ToString(clave), true, columns);

                return File(filecontent, ExcelExportHelper.ExcelContentType, "ExportToExcelVentasPorDespachar.xlsx");
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Excel con contraseña
        [HttpGet]
        public async Task<FileContentResult> ExcelGeneralventasDespachadas()
        {
            try
            {
                var clave = User.FindFirstValue("Clave");
                var retorno = await _IDbProducto.F_GetProductosVentasRealizadas();
                string[] columns = { "id_compra_realizada", "Nombres", "Apellidos", "Nombre_Empresa", "Direccion", "Opcional_Direccion", "Pais", "Departamento", "Ciudad", "Codigo_Postal", "Celular", "Correo_Electronico", "Comentario", "Fecha_CreacionS", "Vigente", "Cancelada", "Enviado_Satisfactoriamente", "Empresa_Entrega", "Fecha_Envio_productoS", "Fecha_Llegada_productoS", "Estado_producto", "Identifacion_modifica", "Cantidad", "CostoEnvio", "PrecioTotal", "PrecioGeneral" };

                byte[] filecontent = ExcelExportHelperPass.ExportExcel(retorno.ToList(), "Listado General Despachadas y Canceladas", Convert.ToString(clave), true, columns);

                return File(filecontent, ExcelExportHelper.ExcelContentType, "ExportExcelVentasDespachadasCanceladas.xlsx");
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Excel con contraseña
        [HttpGet]
        public async Task<FileContentResult> ExcelTotal()
        {
            try
            {
                var clave = User.FindFirstValue("Clave");
                var retorno = await _IDbProducto.F_GetProductosTotal();
                string[] columns = { "id_compra_realizada", "Nombres", "Apellidos", "Nombre_Empresa", "Direccion", "Opcional_Direccion", "Pais", "Departamento", "Ciudad", "Codigo_Postal", "Celular", "Correo_Electronico", "Comentario", "Fecha_CreacionS", "Vigente", "Cancelada", "Enviado_Satisfactoriamente", "Empresa_Entrega", "Fecha_Envio_productoS", "Fecha_Llegada_productoS", "Estado_producto", "Identifacion_modifica", "Cantidad", "CostoEnvio", "PrecioTotal", "PrecioGeneral" };

                byte[] filecontent = ExcelExportHelperPass.ExportExcel(retorno.ToList(), "Listado General Ventas", Convert.ToString(clave), true, columns);

                return File(filecontent, ExcelExportHelper.ExcelContentType, "ExportExcelTotal.xlsx");
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[HttpGet]
        //public IActionResult DescargarPDF(int _idCompra)
        //{

        //    try
        //    {
        //        // Código que utiliza QuestPDF
        //        var retor = _IDbProducto.F_GetProductosVentasFactura(_idCompra);
        //        var preciototal = 0;
        //        var Comentario = "";
        //        var precioEnvio = 0;
        //        DtoMiCompraRealizada Retorno = new DtoMiCompraRealizada();
        //        List<DtoMiCompraRealizada> RetornoList = new List<DtoMiCompraRealizada>();
        //        foreach (var item in retor.Result)
        //        {
        //            Retorno.Nombres = item.Nombres + " " + item.Apellidos;
        //            Retorno.Direccion = item.Direccion + " " + item.Opcional_Direccion;
        //            Retorno.Celular = item.Celular;
        //            Retorno.Pais = item.Pais;
        //            Retorno.Departamento = item.Departamento;
        //            Retorno.Ciudad = item.Ciudad;
        //            Retorno.Codigo_Postal = item.Codigo_Postal;
        //            Retorno.Correo_Electronico = item.Correo_Electronico;


        //        }
        //        var data = Document.Create(document =>
        //        {
        //            document.Page(page =>
        //            {

        //                page.Margin(30);
        //                page.Header().ShowOnce().Row(row =>
        //                {

        //                    var ruraimagen = Path.Combine(_webHostEnvironment.WebRootPath, "img", "logo.png");
        //                    byte[] imagendata = System.IO.File.ReadAllBytes(ruraimagen);
        //                    //var ruraimagen = Path.Combine(_webHostEnvironment.WebRootPath, "/img/logo.png");
        //                    //byte[] imagendata = System.IO.File.ReadAllBytes(ruraimagen);

        //                    //row.ConstantItem(140).Height(60).Placeholder();
        //                    row.ConstantItem(140).Image(imagendata);


        //                    row.RelativeItem().Column(col =>
        //                    {
        //                        col.Item().AlignCenter().Text("Distribuidora de Productos de Belleza MAJAS").Bold().FontSize(14);
        //                        col.Item().AlignCenter().Text("trasversal 20 a 129 d- Bogota").FontSize(9);
        //                        col.Item().AlignCenter().Text("3204125455/ 5487454").FontSize(9);
        //                        col.Item().AlignCenter().Text("Majas@hotmail.com").FontSize(9);

        //                    });

        //                    row.RelativeItem().Column(col =>
        //                    {
        //                        col.Item().Border(1).BorderColor("#257272")
        //                        .AlignCenter().Text("NIT 21312312312");

        //                        col.Item().Background("#257272").Border(1)
        //                        .BorderColor("#257272").AlignCenter()
        //                        .Text("Factura de venta").FontColor("#fff");

        //                        col.Item().Border(1).BorderColor("#257272").
        //                        AlignCenter().Text("M000" + _idCompra + " - " + _idCompra);


        //                    });
        //                });

        //                page.Content().PaddingVertical(10).Column(col1 =>
        //                {
        //                    col1.Item().Column(col2 =>
        //                    {
        //                        col2.Item().Text("Datos del cliente").Underline().Bold();

        //                        col2.Item().Text(txt =>
        //                        {
        //                            txt.Span("Nombre: ").SemiBold().FontSize(10);
        //                            txt.Span(Retorno.Nombres).FontSize(10);
        //                        });

        //                        col2.Item().Text(txt =>
        //                        {
        //                            txt.Span("CC: ").SemiBold().FontSize(10);
        //                            txt.Span("").FontSize(10);
        //                        });
        //                        col2.Item().Text(txt =>
        //                        {
        //                            txt.Span("Teléfono: ").SemiBold().FontSize(10);
        //                            txt.Span(Retorno.Celular).FontSize(10);
        //                        });


        //                        col2.Item().Text(txt =>
        //                        {
        //                            txt.Span("Direccion: ").SemiBold().FontSize(10);
        //                            txt.Span(Retorno.Direccion).FontSize(10);
        //                        });

        //                        col2.Item().Text(txt =>
        //                        {
        //                            txt.Span("Pais/Departamento/Ciudad/Estado o Provincia: ").SemiBold().FontSize(10);
        //                            txt.Span(Retorno.Pais + "/" + Retorno.Departamento + "/" + Retorno.Ciudad).FontSize(10);
        //                        });
        //                        col2.Item().Text(txt =>
        //                        {
        //                            txt.Span("Código postal: ").SemiBold().FontSize(10);
        //                            txt.Span(Retorno.Codigo_Postal).FontSize(10);
        //                        });
        //                        col2.Item().Text(txt =>
        //                        {
        //                            txt.Span("Correo electrónico: ").SemiBold().FontSize(10);
        //                            txt.Span(Retorno.Correo_Electronico).FontSize(10);
        //                        });
        //                    });

        //                    col1.Item().LineHorizontal(0.5f);

        //                    col1.Item().Table(tabla =>
        //                    {
        //                        tabla.ColumnsDefinition(columns =>
        //                        {
        //                            columns.RelativeColumn(3);
        //                            columns.RelativeColumn();
        //                            columns.RelativeColumn();
        //                            columns.RelativeColumn();
                                 


        //                        });

        //                        tabla.Header(header =>
        //                        {
        //                            header.Cell().Background("#257272")
        //                            .Padding(1).Text("Producto").FontColor("#fff");

        //                            header.Cell().Background("#257272")
        //                           .Padding(1).Text("Cantidad").FontColor("#fff");

        //                            header.Cell().Background("#257272")
        //                           .Padding(2).Text("Precio Unitario").FontColor("#fff");

        //                            //header.Cell().Background("#257272")
        //                            //.Padding(2).Text("Precio Envío").FontColor("#fff");

        //                            header.Cell().Background("#257272")
        //                           .Padding(2).Text("Total").FontColor("#fff");
        //                        });


        //                        foreach (var item in retor.Result)
        //                        {
        //                            Comentario = item.Comentario;
        //                            var sum = 0;
        //                            foreach (var item2 in item.ListaMisCompras)
        //                            {
        //                                var cantidad = item2.Cantidad;
        //                                var precio = item2.precio_Unitario;
        //                                precioEnvio = Convert.ToInt32(item2.Precio_Envio);
        //                                var total = (cantidad * precio) ;
        //                                var Nombre = item2.Descripcion_Producto;     
        //                                sum += Convert.ToInt32(total);   
        //                                preciototal = sum;

        //                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                              .Padding(2).Text(Nombre).FontSize(10);

        //                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                              .Padding(2).Text(cantidad.ToString()).FontSize(10);

        //                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                              .Padding(2).Text($"$ {precio}").FontSize(10);
        //                              //  tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                              //.Padding(2).Text($"$ {precioEnvio}").FontSize(10);

        //                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                              .Padding(2).AlignRight().Text($"$ {total}").FontSize(10);
        //                            }

        //                        }
        //                    });
                           
        //                    col1.Item().AlignRight().Text($"Total Producto: $ {preciototal}").FontSize(12);
        //                    col1.Item().AlignRight().Text($"Total Envío: $ {precioEnvio}").FontSize(12);
        //                    col1.Item().AlignRight().Text($"Total: $ {preciototal + precioEnvio}").FontSize(12);

        //                    if (1 == 1)
        //                        col1.Item().Background(Colors.Grey.Lighten3).Padding(10)
        //                        .Column(column =>
        //                        {
        //                            column.Item().Text("Comentarios").FontSize(14);
        //                            column.Item().Text(Comentario);
        //                            column.Spacing(5);
        //                        });

        //                    col1.Spacing(10);
        //                });


        //                page.Footer()
        //                .AlignRight()
        //                .Text(txt =>
        //                {
        //                    txt.Span("Pagina ").FontSize(10);
        //                    txt.CurrentPageNumber().FontSize(10);
        //                    txt.Span(" de ").FontSize(10);
        //                    txt.TotalPages().FontSize(10);
        //                });
        //            });
        //        }).GeneratePdf();

        //        Stream stream = new MemoryStream(data);
        //        return File(stream, "aplicacion/pdf", "FacturaMajas"+ _idCompra + ".pdf");
        //    }
        //    catch (Exception ex)
        //    {
        //        var nombre = "Se produjo un error al utilizar QuestPDF: " + ex.Message;
        //        // Manejar la excepción de QuestPDF
        //        return File("", "aplicacion/pdf", "FacturaMajas" + _idCompra + ".pdf");

        //    }


        //}
        //[AllowAnonymous]
        //public IActionResult DescargarGeneralPDF()
        //{

        //    try
        //    {
        //        // Código que utiliza QuestPDF
        //        var retor = _IDbProducto.F_GetProductosVentasPdf();
        //        var preciototal = 0;
        //        var Comentario = "";
        //        var can = 0;
        //        var Contador = 0;
        //        var totalEnvio = 0;
        //        var totalFinal = 0;

        //        DtoMiCompraRealizada Retorno = new DtoMiCompraRealizada();
        //        List<DtoMiCompraRealizada> RetornoList = new List<DtoMiCompraRealizada>();
        //        foreach (var item in retor.Result)
        //        {
        //            Retorno.id_compra_realizada = item.id_compra_realizada;
        //            Retorno.FechaInicio = item.FechaInicio;
        //            Retorno.FechaFin = item.FechaFin;

        //        }
        //        var data = Document.Create(document =>
        //        {
        //            document.Page(page =>
        //            {

        //                page.Margin(30);
        //                page.Header().ShowOnce().Row(row =>
        //                {
        //                    var ruraimagen = Path.Combine(_webHostEnvironment.WebRootPath, "img", "logo.png");
        //                    byte[] imagendata = System.IO.File.ReadAllBytes(ruraimagen);

        //                    //row.ConstantItem(140).Height(60).Placeholder();
        //                    row.ConstantItem(140).Image(imagendata);


        //                    row.RelativeItem().Column(col =>
        //                    {
        //                        col.Item().AlignCenter().Text("Distribuidora de Productos de Belleza MAJAS").Bold().FontSize(14);
        //                        col.Item().AlignCenter().Text("trasversal 20 a 129 d- Bogota").FontSize(9);
        //                        col.Item().AlignCenter().Text("3204125455/ 5487454").FontSize(9);
        //                        col.Item().AlignCenter().Text("Majas@hotmail.com").FontSize(9);

        //                    });

        //                    row.RelativeItem().Column(col =>
        //                    {
        //                        col.Item().Border(1).BorderColor("#257272")
        //                        .AlignCenter().Text("NIT 21312312312");

        //                        col.Item().Background("#257272").Border(1)
        //                        .BorderColor("#257272").AlignCenter()
        //                        .Text("Factura de venta").FontColor("#fff");

        //                        col.Item().Border(1).BorderColor("#257272").
        //                        AlignCenter().Text("M000" + Retorno.id_compra_realizada + " - " + Retorno.id_compra_realizada);


        //                    });
        //                });

        //                page.Content().PaddingVertical(10).Column(col1 =>
        //                {
                     

        //                    col1.Item().LineHorizontal(0.5f);

        //                    col1.Item().Table(tabla =>
        //                    {
        //                        tabla.ColumnsDefinition(columns =>
        //                        {
        //                            columns.RelativeColumn(3);
        //                            columns.RelativeColumn();
        //                            columns.RelativeColumn();
        //                            columns.RelativeColumn();
        //                            columns.RelativeColumn();
        //                            columns.RelativeColumn();


        //                        });

        //                        tabla.Header(header =>
        //                        {

        //                            header.Cell().Background("#257272")
        //                            .Padding(1).Text("Producto").FontColor("#fff");

        //                            header.Cell().Background("#257272")
        //                           .Padding(1).Text("Cantidad").FontColor("#fff");

        //                            header.Cell().Background("#257272")
        //                           .Padding(2).Text("Precio Unitario").FontColor("#fff");

        //                            header.Cell().Background("#257272")
        //                            .Padding(2).Text("Precio Envío").FontColor("#fff");

        //                            header.Cell().Background("#257272")
        //                           .Padding(2).Text("Total").FontColor("#fff");
        //                            header.Cell().Background("#257272")
        //                           .Padding(2).Text("Id Pedido").FontColor("#fff");
        //                        });

        //                          var sum = 0;
                           
        //                        foreach (var item in retor.Result)
        //                        {
        //                            Contador++;
        //                            var contador2  = 0;
        //                            Comentario = item.Comentario;
                                  
        //                            foreach (var item2 in item.ListaMisCompras)
        //                            {
                                      
        //                                var cantidad = item2.Cantidad;
        //                                var precio = item2.precio_Unitario;
        //                                var precioEnvio = item2.Precio_Envio;
        //                                var total = (cantidad * precio);
        //                                var Nombre = item2.Descripcion_Producto;
        //                                sum += Convert.ToInt32(total);
        //                                can += Convert.ToInt32(cantidad);   
        //                                preciototal = sum;
                                        

        //                                totalEnvio = Convert.ToInt32(precioEnvio);

        //                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                              .Padding(2).Text(Nombre).FontSize(10);

        //                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                              .Padding(2).Text(cantidad.ToString()).FontSize(10);

        //                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                              .Padding(2).Text($"$ {precio}").FontSize(10);

        //                                if (contador2 != Contador)
        //                                {
        //                                    contador2  = item.id_compra_realizada;
        //                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                                   .Padding(2).Text($"$ {precioEnvio}").FontSize(10);
        //                                }
        //                                else {
        //                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                                   .Padding(2).Text("").FontSize(10);
        //                                }
        //                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                              .Padding(2).AlignRight().Text($"$ {total}").FontSize(10);

        //                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                             .Padding(2).AlignRight().Text(Convert.ToString(item.id_compra_realizada)).FontSize(10);

        //                            }

        //                        }
        //                    });
        //                    totalFinal = totalEnvio * Contador;
        //                    col1.Item().AlignRight().Text($"Total Producto: $ {preciototal}").FontSize(12);
        //                    col1.Item().AlignRight().Text($"Total Envío: $ {totalFinal}").FontSize(12);
        //                    col1.Item().AlignRight().Text($"Total: $ {preciototal + totalFinal}").FontSize(12);

        //                    col1.Item().Background(Colors.Grey.Lighten3).Padding(10)
        //                    .Column(col2 =>
        //                    {
        //                        col2.Item().Text("Total Productos Vendidos: ").Underline().Bold();

        //                        col2.Item().Text(txt =>
        //                        {
        //                            txt.Span("Cantidad: ").SemiBold().FontSize(10);
        //                            txt.Span(Convert.ToString(can)).FontSize(10);
        //                        });
        //                        col2.Item().Text(txt =>
        //                        {
        //                            txt.Span("Precio total venta: ").SemiBold().FontSize(10);
        //                            txt.Span(Convert.ToString($"$ {preciototal}")).FontSize(10);
        //                        });
        //                        col2.Item().Text(txt =>
        //                        {
        //                            txt.Span("Fecha Inicio: ").SemiBold().FontSize(10);
        //                            txt.Span(Convert.ToString(Retorno.FechaInicio)).FontSize(10);
        //                        });
        //                        col2.Item().Text(txt =>
        //                        {
        //                            txt.Span("Fecha Fin: ").SemiBold().FontSize(10);
        //                            txt.Span(Convert.ToString(Retorno.FechaFin)).FontSize(10);
        //                        });

        //                    });
                         

        //                    col1.Spacing(10);
        //                });


        //                page.Footer()
        //                .AlignRight()
        //                .Text(txt =>
        //                {
        //                    txt.Span("Pagina ").FontSize(10);
        //                    txt.CurrentPageNumber().FontSize(10);
        //                    txt.Span(" de ").FontSize(10);
        //                    txt.TotalPages().FontSize(10);
        //                });
        //            });
        //        }).GeneratePdf();

        //        Stream stream = new MemoryStream(data);
        //        return File(stream, "aplicacion/pdf", "FacturaMajasGeneral.pdf");
        //    }
        //    catch (Exception ex)
        //    {
        //        var nombre = "Se produjo un error al utilizar QuestPDF: " + ex.Message;
        //        // Manejar la excepción de QuestPDF
        //        return File("", "aplicacion/pdf", "FacturaMajasGeneral.pdf");

        //    }


        //}
        //[HttpGet]
        //public IActionResult DescargarIndividualPDF()
        //{

        //    try
        //    {
        //        // Código que utiliza QuestPDF
        //        var retor = _IDbProducto.F_GetProductosVentasPdf();
        //        var preciototal = 0;
        //        var precioEnvio = 0;

        //        DtoMiCompraRealizada Retorno = new DtoMiCompraRealizada();
        //        List<DtoMiCompraRealizada> RetornoList = new List<DtoMiCompraRealizada>();
        
        //        var data = Document.Create(document =>
        //        {
        //            document.Page(page =>
        //            {
             
        //                page.Margin(30);
        //                page.Header().ShowOnce().Row(row =>
        //                {
                        
        //                    var ruraimagen = Path.Combine(_webHostEnvironment.WebRootPath, "img", "logo.png");
        //                    byte[] imagendata = System.IO.File.ReadAllBytes(ruraimagen);
                          
        //                    row.ConstantItem(140).Image(imagendata);


        //                    row.RelativeItem().Column(col =>
        //                    {
        //                        col.Item().AlignCenter().Text("Distribuidora de Productos de Belleza MAJAS").Bold().FontSize(14);
        //                        col.Item().AlignCenter().Text("trasversal 20 a 129 d- Bogota").FontSize(9);
        //                        col.Item().AlignCenter().Text("3204125455/ 5487454").FontSize(9);
        //                        col.Item().AlignCenter().Text("Majas@hotmail.com").FontSize(9);

        //                    });

        //                    row.RelativeItem().Column(col =>
        //                    {
        //                        col.Item().Border(1).BorderColor("#257272")
        //                        .AlignCenter().Text("NIT 21312312312");

        //                        col.Item().Background("#257272").Border(1)
        //                        .BorderColor("#257272").AlignCenter()
        //                        .Text("Factura de venta").FontColor("#fff");

        //                        col.Item().Border(1).BorderColor("#257272").
        //                        AlignCenter().Text("MGeneral");


        //                    });
        //                });

             

        //                    page.Content().PaddingVertical(10).Column(col1 =>
        //                     {

        //                     foreach (var item in retor.Result)
        //                     {
        //                         Retorno.Nombres = item.Nombres + " " + item.Apellidos;
        //                         Retorno.Direccion = item.Direccion + " " + item.Opcional_Direccion;
        //                         Retorno.Celular = item.Celular;
        //                         Retorno.id_compra_realizada = item.id_compra_realizada;
        //                         Retorno.Comentario = item.Comentario;
        //                             Retorno.Pais = item.Pais;
        //                             Retorno.Departamento = item.Departamento;
        //                             Retorno.Ciudad = item.Ciudad;
        //                             Retorno.Codigo_Postal = item.Codigo_Postal;
        //                             Retorno.Correo_Electronico = item.Correo_Electronico;

        //                             col1.Item().Column(col2 =>
        //                    {
        //                        col2.Item().Text("Datos del cliente").Underline().Bold();

        //                        col2.Item().Text(txt =>
        //                        {
        //                            txt.Span("Nombre: ").SemiBold().FontSize(10);
        //                            txt.Span(Retorno.Nombres).FontSize(10);
        //                        });

        //                        col2.Item().Text(txt =>
        //                        {
        //                            txt.Span("CC: ").SemiBold().FontSize(10);
        //                            txt.Span("").FontSize(10);
        //                        });
        //                        col2.Item().Text(txt =>
        //                        {
        //                            txt.Span("Teléfono: ").SemiBold().FontSize(10);
        //                            txt.Span(Retorno.Celular).FontSize(10);
        //                        });


        //                        col2.Item().Text(txt =>
        //                        {
        //                            txt.Span("Direccion: ").SemiBold().FontSize(10);
        //                            txt.Span(Retorno.Direccion).FontSize(10);
        //                        });
        //                        col2.Item().Text(txt =>
        //                        {
        //                            txt.Span("Pais/Departamento/Ciudad/Estado o Provincia: ").SemiBold().FontSize(10);
        //                            txt.Span(Retorno.Pais + "/" + Retorno.Departamento + "/" + Retorno.Ciudad).FontSize(10);
        //                        });
        //                        col2.Item().Text(txt =>
        //                        {
        //                            txt.Span("Código postal: ").SemiBold().FontSize(10);
        //                            txt.Span(Retorno.Codigo_Postal).FontSize(10);
        //                        });
        //                        col2.Item().Text(txt =>
        //                        {
        //                            txt.Span("Correo electrónico: ").SemiBold().FontSize(10);
        //                            txt.Span(Retorno.Correo_Electronico).FontSize(10);
        //                        });
        //                    });

        //                    col1.Item().LineHorizontal(0.5f);                  
        //                    col1.Item().Table(tabla =>
        //                    {
        //                        tabla.ColumnsDefinition(columns =>
        //                        {
        //                            columns.RelativeColumn(3);
        //                            columns.RelativeColumn();
        //                            columns.RelativeColumn();
        //                            columns.RelativeColumn();
        //                            //columns.RelativeColumn();

        //                        });

        //                        tabla.Header(header =>
        //                        {
        //                            header.Cell().Background("#257272")
        //                            .Padding(1).Text("Producto").FontColor("#fff");

        //                            header.Cell().Background("#257272")
        //                           .Padding(1).Text("Cantidad").FontColor("#fff");

        //                            header.Cell().Background("#257272")
        //                           .Padding(2).Text("Precio Unitario").FontColor("#fff");

        //                            //header.Cell().Background("#257272")
        //                            //.Padding(2).Text("Precio Envío").FontColor("#fff");

        //                            header.Cell().Background("#257272")
        //                           .Padding(2).Text("Total").FontColor("#fff");
        //                        });

        //                        var sum = 0;
                             
                              

        //                            foreach (var item2 in item.ListaMisCompras)
        //                            {
        //                                var cantidad = item2.Cantidad;
        //                                var precio = item2.precio_Unitario;
        //                                 precioEnvio = Convert.ToInt32(item2.Precio_Envio);
        //                                var total = (cantidad * precio);
        //                                var Nombre = item2.Descripcion_Producto;
        //                                sum += Convert.ToInt32(total);
        //                                preciototal = sum;

        //                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                              .Padding(2).Text(Nombre).FontSize(10);

        //                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                              .Padding(2).Text(cantidad.ToString()).FontSize(10);

        //                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                              .Padding(2).Text($"$ {precio}").FontSize(10);
        //                              //  tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                              //.Padding(2).Text($"$ {precioEnvio}").FontSize(10);

        //                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                              .Padding(2).AlignRight().Text($"$ {total}").FontSize(10);
        //                            }

                               
        //                    });

        //                             col1.Item().AlignRight().Text($"Total Producto: $ {preciototal}").FontSize(12);
        //                             col1.Item().AlignRight().Text($"Total Envío: $ {precioEnvio}").FontSize(12);
        //                             col1.Item().AlignRight().Text($"Total: $ {preciototal + precioEnvio}").FontSize(12);

        //                    if (1 == 1)
        //                        col1.Item().Background(Colors.Grey.Lighten3).Padding(10)
        //                        .Column(column =>
        //                        {
        //                            column.Item().Text("Comentarios").FontSize(14);
        //                            column.Item().Text(Retorno.Comentario);
        //                            column.Spacing(5);
        //                        });

        //                    col1.Spacing(10);
                                 
        //                             col1.Item().Background(Colors.Blue.Lighten3).Padding(4).LineHorizontal((0.1f));
        //                             //col1.Item().LineHorizontal(0.3f);

        //                         }
                               
        //                     });

                      
        //                page.Footer()
        //                .AlignRight()
        //                .Text(txt =>
        //                {
        //                    txt.Span("Pagina ").FontSize(10);
        //                    txt.CurrentPageNumber().FontSize(10);
        //                    txt.Span(" de ").FontSize(10);
        //                    txt.TotalPages().FontSize(10);
        //                });
                       
        //            });
        //        }).GeneratePdf();

        //        Stream stream = new MemoryStream(data);
        //        return File(stream, "aplicacion/pdf", "FacturaMajasGeneral.pdf");
        //    }
        //    catch (Exception ex)
        //    {
        //        var nombre = "Se produjo un error al utilizar QuestPDF: " + ex.Message;
        //        // Manejar la excepción de QuestPDF
        //        return File("", "aplicacion/pdf", "FacturaMajasGeneral.pdf");

        //    }


        //}



        [HttpPost]
        public async Task<IActionResult> UpdateCompraAdmin(DtoMiCompraRealizada dtoCompra)
        {
            // Obtener la IP del cliente y el identificador del usuario
            var V_Maquina = GetClientIpAddress(HttpContext);
            var V_Usuario = Convert.ToInt32(User.FindFirstValue("Identificacion"));

            // Asignar la información adicional (máquina y usuario que hace la actualización)
            dtoCompra.Maquina_Creacion = V_Maquina;
            dtoCompra.Identifacion_modifica = V_Usuario.ToString(); // Suponiendo que es un string

            // Llamar al método de la interfaz para realizar la actualización en la base de datos
            var retorno = await _IDbProducto.F_UpdateCompra(dtoCompra);

            if (retorno >= 1)
            {
                return Json(new { success = true, data = retorno, message = "" });
            }
            else
            {
                return Json(new { success = false, data = retorno, message = "" });
            }
        }


        // Método para actualizar la compra realizada
        //[HttpPost]
        //public async Task<IActionResult> UpdateCompraAdmin([FromBody] DtoMiCompraRealizada dtoCompra)
        //{
        //    // Verificar que el DTO no sea nulo
        //    if (dtoCompra == null)
        //    {
        //        return BadRequest("No se proporcionaron datos válidos.");
        //    }

        //    // Buscar la compra en la base de datos por ID
        //    var compraExistente = _context.mi_compra_realizada
        //        .FirstOrDefault(x => x.id_compra_realizada == dtoCompra.id_compra_realizada);

        //    if (compraExistente == null)
        //    {
        //        return NotFound("La compra no existe.");
        //    }

        //    // Actualizar los campos de la entidad con los datos del DTO
        //    compraExistente.Pais = dtoCompra.Pais;
        //    compraExistente.Nombres = dtoCompra.Nombres;
        //    compraExistente.Apellidos = dtoCompra.Apellidos;
        //    compraExistente.Nombre_Empresa = dtoCompra.Nombre_Empresa;
        //    compraExistente.Direccion = dtoCompra.Direccion;
        //    compraExistente.Opcional_Direccion = dtoCompra.Opcional_Direccion;
        //    compraExistente.Departamento = dtoCompra.Departamento;
        //    compraExistente.Ciudad = dtoCompra.Ciudad;
        //    compraExistente.Codigo_Postal = dtoCompra.Codigo_Postal;
        //    compraExistente.Celular = dtoCompra.Celular;
        //    compraExistente.Correo_Electronico = dtoCompra.Correo_Electronico;
        //    compraExistente.Comentario = dtoCompra.Comentario;
        //    compraExistente.Fecha_Creacion = dtoCompra.Fecha_Creacion;
        //    compraExistente.Fecha_CreacionEs = dtoCompra.Fecha_CreacionEs;
        //    compraExistente.Maquina_Creacion = dtoCompra.Maquina_Creacion;
        //    compraExistente.Vigente = dtoCompra.Vigente;
        //    compraExistente.Cancelada = dtoCompra.Cancelada;
        //    compraExistente.Enviado_Satisfactoriamente = dtoCompra.Enviado_Satisfactoriamente;
        //    compraExistente.Empresa_Entrega = dtoCompra.Empresa_Entrega;
        //    compraExistente.Fecha_Llegada_producto = dtoCompra.Fecha_Llegada_producto;
        //    compraExistente.Fecha_Envio_producto = dtoCompra.Fecha_Envio_producto;
        //    compraExistente.Estado_producto = dtoCompra.Estado_producto;
        //    compraExistente.Identifacion_modifica = dtoCompra.Identifacion_modifica;
        //    compraExistente.Maquina_modifica = dtoCompra.Maquina_modifica;
        //    compraExistente.Producto = dtoCompra.Producto;
        //    compraExistente.Cantidad = dtoCompra.Cantidad;
        //    compraExistente.PrecioTotal = dtoCompra.PrecioTotal;
        //    compraExistente.CelularConIndicativo = dtoCompra.CelularConIndicativo;
        //    compraExistente.EnvioCorreo = dtoCompra.EnvioCorreo;
        //    compraExistente.PrecioGeneral = dtoCompra.PrecioGeneral;
        //    compraExistente.FechaInicio = dtoCompra.FechaInicio;
        //    compraExistente.FechaFin = dtoCompra.FechaFin;
        //    compraExistente.CostoEnvio = dtoCompra.CostoEnvio;
        //    compraExistente.PrecioFinal = dtoCompra.PrecioFinal;
        //    compraExistente.preferenceId = dtoCompra.preferenceId;
        //    compraExistente.status = dtoCompra.status;
        //    compraExistente.external_reference = dtoCompra.external_reference;
        //    compraExistente.merchant_order_id = dtoCompra.merchant_order_id;
        //    compraExistente.preferenceId_Consulta_Inicial = dtoCompra.preferenceId_Consulta_Inicial;

        //    // Si hay una lista de compras, se debe actualizar o insertar la lista también
        //    if (dtoCompra.ListaMisCompras != null && dtoCompra.ListaMisCompras.Any())
        //    {
        //        // El código para actualizar la lista de compras
        //        // Puedes actualizar o insertar los detalles de los productos en la lista de compras (mi_carrito).
        //        // Asegúrate de que la relación esté bien establecida entre mi_compra_realizada y mi_carrito.
        //    }

        //    // Guardar los cambios en la base de datos
        //    _context.mi_compra_realizada.Update(compraExistente);
        //    await _context.SaveChangesAsync();

        //    // Retornar la respuesta
        //    return Ok(new { mensaje = "Compra actualizada exitosamente." });
        //}


    }
}
