namespace ProyectoEcommers.Controllers
{

    using Comun.Dto;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Negocio.Contratos.ConsultasExternas;
    using System.Data;
    using System.Security.Claims;
    using ProyectoEcommers.Models;
    using static ProyectoEcommers.Controllers.CuentaController;
    using ProyectoEcommers.Helper;

    //[Route("d16ba302-7035-479f-a39c-a187f0afc471")]
    //[Authorize]

    [Authorize(Roles = "1,2,3,4")]
    public class ProductoController : Controller
    {   
        private readonly IDbProducto _IDbProducto;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDbAdministracion _dbAdministracion;
        private readonly IBLConsultasExternas _BlConsultasExternas;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductoController(IDbProducto iDbProducto, IHttpContextAccessor httpContextAccessor, IDbAdministracion dbAdministracion, IBLConsultasExternas bLConsultasExternas, IWebHostEnvironment webHostEnvironment)
        {
            _IDbProducto = iDbProducto;
            _httpContextAccessor = httpContextAccessor;
            _dbAdministracion = dbAdministracion;
            _BlConsultasExternas = bLConsultasExternas;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public async Task<IActionResult> GestionProducto()
        {

            try
            {
                var ImagenesSlider = await _BlConsultasExternas.F_GetCarruselImagenesAsyc();
                var SlidersView = new List<DtoSlider>();

                foreach (var item in ImagenesSlider.Respuesta)
                {
                    string ruta = ConsultarRuta(Convert.ToInt32(item.Consecutivo));
                    string ruta1 = "";

                    if (ruta == null || ruta == "")
                    {
                        ruta1 = F_GetImagenes(Convert.ToInt32(item.Consecutivo), item.ContentType, item.Foto);
                    }
                    else
                    {
                        ruta1 = ruta;
                    }
                    var SliderView = new DtoSlider
                    {
                        Consecutivo = item.Consecutivo,
                        ContentType = item.ContentType,
                        FileName = item.FileName,
                        Ruta = ruta1
                    };

                    SlidersView.Add(SliderView);
                }
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

        private string GetClientIpAddress(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrEmpty(ipAddress) && context.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ipAddress = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            }

            return ipAddress ?? "0.0.0.0";
        }

        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Ins_Producto(IFormFile Imagen1, IFormFile Imagen2, IFormFile Imagen3, IFormFile Imagen4, DtoProductos producto)
        {
            int contador = 1;
            string tipo = "";

            if (Imagen1 != null)
            {


                if (Imagen1.Length < 1)
                    return Json(new { success = false, data = new List<Int16?>(), message = "Verifique el archivo que esta subiendo" });
            }

            List<DtoImagenesProducto> ListaImg = new List<DtoImagenesProducto>();
            List<IFormFile> listaImagen = new List<IFormFile>();
            listaImagen.Add(Imagen1);
            if (Imagen2 != null)
                listaImagen.Add(Imagen2);
            if (Imagen3 != null)
                listaImagen.Add(Imagen3);
            if (Imagen4 != null)
                listaImagen.Add(Imagen4);

            foreach (var item in listaImagen)
            {
                if (item != null)
                {
                    DtoImagenesProducto imagen = new DtoImagenesProducto();

                    using (var ms = new MemoryStream())
                    {
                        item.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        string ImagenBase64 = Convert.ToBase64String(fileBytes);
                        var nameTime = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();//DateTime a formato UNIX de milisegundos

                        if (item.ContentType == "image/png")
                        {
                            tipo = ".png";
                        }
                        else if (item.ContentType == "image/jpeg")
                        {
                            tipo = ".jpg";
                        }

                        imagen.FILENAME = nameTime.ToString() + contador.ToString() + tipo;
                        imagen.URL = $"{"\\img\\Productos\\"}" + imagen.FILENAME;
                        imagen.FOTO = ImagenBase64;
                        imagen.CREADO_POR = Convert.ToInt32(User.FindFirstValue("Identificacion"));
                        imagen.MAQUINA_CREACION = GetClientIpAddress(HttpContext);
                        imagen.CONTENT_TYPE = item.ContentType;
                        ListaImg.Add(imagen);
                        contador++;
                    }
                }
            }

            producto.ListaImagenes = ListaImg;

            var result = await _IDbProducto.F_InsProducto(producto);

            if (result > 0)
                return Json(new { success = true, data = result, message = "Se realiza el registro exitosamente" });
            else
                return Json(new { success = false, data = result, message = "No fue posible guardar la fotografia" });
        }

        [AllowAnonymous]
        public async Task<JsonResult> F_GetProductos(int marca, int categoria)
        {
            var result = await _IDbProducto.F_GetListaProductos(marca, categoria);

            if (result.Count > 0)
            {
                return Json(new { success = true, data = result.OrderByDescending(x=> x.PRODUCTO_ID) });
                //resultado.MaxJsonLength = int.MaxValue;
              
            }
            else
            {
                return Json(new { success = false, data = result });
            }
        }
        //[AllowAnonymous]
        //public async Task<JsonResult> F_GetProductos(int marca, int categoria)

        //{
        //    var result = await _IDbProducto.F_GetListaProductos(marca, categoria);

        //    if (result.Count > 0)
        //    {

        //        return Json(new { success = true, data = result });
        //    }
        //    else
        //    {
        //        return Json(new { success = false, data = result });
        //    }
        //}
        [AllowAnonymous]
        public async Task<JsonResult> F_GetProductosGrilla()
        {
            var result = await _IDbProducto.F_GetProductosGrilla();

            if (result.Count > 0)
            {
                return Json(new { success = true, data = result.OrderByDescending(x => x.PRODUCTO_ID) });
            }
            else
            {
                return Json(new { success = false, data = result });
            }
        }
        [AllowAnonymous]
        public async Task<JsonResult> F_GetProductosGrillaDesactivado()
        {
            var result = await _IDbProducto.F_GetProductosGrillaDesactivado();

            if (result.Count > 0)
            {
                return Json(new { success = true, data = result });
            }
            else
            {
                return Json(new { success = false, data = result });
            }
        }
        [AllowAnonymous]
        public async Task<JsonResult> F_GetImagenGrilla(int IdProducto)
        {
            var result = await _IDbProducto.F_GetImagenesGrilla(IdProducto);

            if (result.Count > 0)
            {
                return Json(new { success = true, data = result });
            }
            else
            {
                return Json(new { success = false, data = result });
            }
        }
        [HttpPost]
        public string ConsultarRuta(int Consecutivo)
        {
            // verificar si la imagen existe en una carpeta
            string[] formatos = new[] { ".tiff", ".ief", ".gif", ".jpg", ".png" };
            string ruta = "";
            string extensionArchivo = "";
            bool existe = false;
            string Resultado = "";
            foreach (string formato in formatos)
            {

                string webRootPath = _webHostEnvironment.WebRootPath;
                string contentRootPath = _webHostEnvironment.ContentRootPath;

                ruta = Path.Combine(webRootPath, "img/Productos/") + Consecutivo.ToString() + formato;
                //or path = Path.Combine(contentRootPath , "wwwroot" ,"CSS" );

                if (System.IO.File.Exists(ruta))
                {
                    existe = true;
                    extensionArchivo = formato;
                    break;
                }
            }
            // si existe devolverla
            if (existe)
            {
                return  Consecutivo.ToString() + extensionArchivo;
            }
            else
            {
                return Resultado;
            }
        }
        [HttpPost]
        public string F_GetImagenes(int Consecutivo, string ContentType, string Foto)
        {
            bool existe = false;
            string ruta = "";
            string extensionArchivo = "";
            string Resultado = "";

            // validar que haya un resultado
            if (Foto != null & Foto != "")
            {
                byte[] _Foto = Convert.FromBase64String(Foto);
                // obtener la extension
                if (ContentType.Equals("image/tiff"))
                    extensionArchivo = ".tiff";
                else if (ContentType.Equals("image/ief"))
                    extensionArchivo = ".ief";
                else if (ContentType.Equals("image/gif"))
                    extensionArchivo = ".gif";
                else if (ContentType.Equals("image/jpg") | ContentType.Equals("image/jpeg"))
                    extensionArchivo = ".jpg";
                else if (ContentType.Equals("image/png"))
                    extensionArchivo = ".png";

                // guardar la imagen en la carpeta
                string webRootPath = _webHostEnvironment.WebRootPath;
                string contentRootPath = _webHostEnvironment.ContentRootPath;

                ruta = Path.Combine(webRootPath, "img/Productos/") + Consecutivo.ToString() + extensionArchivo;
                System.IO.File.WriteAllBytes(ruta, _Foto);
                existe = true;
            }
            else
            {
                existe = false;
            }

            // si existe devolverla
            if (existe)
            {
                return "~/img/Productos/" + Consecutivo.ToString() + extensionArchivo;
            }
            else
            {
                return Resultado;
            }

        }
        //Insertar al carrito
        [HttpPost]
        public async Task<JsonResult> UpdateProducto(DtoProductos dto)
        {

            var V_Maquina = GetClientIpAddress(HttpContext);
            var V_Usuario = Convert.ToInt32(User.FindFirstValue("Identificacion"));


            dto.MAQUINA_CREACION = V_Maquina;
            dto.ACTUALIZADO_POR = V_Usuario;

            var retorno = await _IDbProducto.F_UpdateProducto(dto);
            if (retorno.Estado)
            {
                //var auditoria = _DbAdministracion.Ins_Auditoria(V_Usuario, "InsUsuarios", "Cambio estado de usuario a: " + dto.Bloqueado, Convert.ToString(dto.Identificacion), V_Maquina);
                return Json(new { success = true, data = retorno.Respuesta, message = retorno.Mensaje });
            }
            else
            {
                return Json(new { success = false, data = retorno.Respuesta, message = retorno.Mensaje });
            }
        }
        [HttpPost]
        public async Task<JsonResult> UpdateImagen(DtoImagenesProducto dto)
        {

            var V_Maquina = GetClientIpAddress(HttpContext);
            var V_Usuario = Convert.ToInt32(User.FindFirstValue("Identificacion"));


            dto.MAQUINA_CREACION = V_Maquina;
            dto.ACTUALIZADO_POR = V_Usuario;
            dto.VIGENTE = 0;

            var retorno = await _IDbProducto.F_UpdateImagenProducto(dto);
            if (retorno.Estado)
            {
                //var auditoria = _DbAdministracion.Ins_Auditoria(V_Usuario, "InsUsuarios", "Cambio estado de usuario a: " + dto.Bloqueado, Convert.ToString(dto.Identificacion), V_Maquina);
                return Json(new { success = true, data = retorno.Respuesta, message = retorno.Mensaje = "Se realiza el registro exitosamente" });
            }
            else
            {
                return Json(new { success = false, data = retorno.Respuesta, message = retorno.Mensaje = "No es posible Guardar" });
            }
        }
        [AllowAnonymous]
        public async Task<JsonResult> F_GetEmpleadosInte(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                //return new Json { Data = null };
                return Json(new { success = false, data = 0 });
            }
            else
            {
                List<DtoProductos> Lista = new List<DtoProductos>();
                Lista = await _IDbProducto.F_GetBuscaIntelEmple(search);
                //Lista = (await new _IDbProducto.F_GetBuscaIntelEmple(search)).ToList();
                return Json(new { success = false, data = Lista });
                //return new Json { Data = Lista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [AllowAnonymous]
        public async Task<JsonResult> F_GetProductosMisCompras(string correo)
        {
            var result = await _IDbProducto.F_GetProductosMisCompras(correo);

            if (result.Count > 0)
            {
                return Json(new { success = true, data = result.OrderByDescending(x => x.id_compra_realizada) });
            }
            else
            {
                return Json(new { success = false, data = result });
            }
        }
        //[AllowAnonymous]
        //public async Task<JsonResult> F_GetProductosMisComprasId(Int32 idCompra, string preferenceId, string Idstatus, string Idexternal_reference, string Idmerchant_order_id)
        //{
        //    var resultado = 0;
        //    if (preferenceId != null || preferenceId != "") {
        //        if (idCompra > 0) { 
        //          resultado = await _IDbProducto.F_InsPreferenceId(idCompra, preferenceId, Idstatus, Idexternal_reference, Idmerchant_order_id);
        //        }
        //        if (resultado > 0)
        //        {

        //        }
        //    }
        //    var result = await _IDbProducto.F_GetProductosMisComprasId(idCompra);

        //    if (result.Count > 0)
        //    {
        //        return Json(new { success = true, data = result.OrderByDescending(x => x.id_compra_realizada) });
        //    }
        //    else
        //    {
        //        return Json(new { success = false, data = result });
        //    }
        //}
        [AllowAnonymous]
        public async Task<JsonResult> F_GetProductosGrillaMarca()
        {
            var result = await _IDbProducto.F_GetProductosGrillaMarca();

            if (result.Count > 0)
            {
                return Json(new { success = true, data = result });
            }
            else
            {
                return Json(new { success = false, data = result });
            }
        }
        [AllowAnonymous]
        public async Task<JsonResult> F_GetProductosGrillaCategoria()
        {
            var result = await _IDbProducto.F_GetProductosGrillaCategoria();

            if (result.Count > 0)
            {
                return Json(new { success = true, data = result });
            }
            else
            {
                return Json(new { success = false, data = result });
            }
        }
        //Marca categoria
        [HttpGet]
        public IActionResult MarcaCategoria()
        {        
                return View();         
        }
        [HttpPost]
        public async Task<JsonResult> UpdateMarca(DtoDominios dto)
        {

            var V_Maquina = GetClientIpAddress(HttpContext);
            var V_Usuario = Convert.ToInt32(User.FindFirstValue("Identificacion"));


            //dto.MAQUINA_CREACION = V_Maquina;
            //dto.ACTUALIZADO_POR = V_Usuario;

            var retorno = await _IDbProducto.F_UpdateMarca(dto);
            if (retorno.Estado)
            {
                //var auditoria = _DbAdministracion.Ins_Auditoria(V_Usuario, "InsUsuarios", "Cambio estado de usuario a: " + dto.Bloqueado, Convert.ToString(dto.Identificacion), V_Maquina);
                return Json(new { success = true, data = retorno.Respuesta, message = retorno.Mensaje });
            }
            else
            {
                return Json(new { success = false, data = retorno.Respuesta, message = retorno.Mensaje });
            }
        }
        [HttpPost]
        public async Task<JsonResult> UpdateCategoria(DtoDominios dto)
        {

            var V_Maquina = GetClientIpAddress(HttpContext);
            var V_Usuario = Convert.ToInt32(User.FindFirstValue("Identificacion"));


            //dto.MAQUINA_CREACION = V_Maquina;
            //dto.ACTUALIZADO_POR = V_Usuario;

            var retorno = await _IDbProducto.F_UpdateCategoria(dto);
            if (retorno.Estado)
            {
                //var auditoria = _DbAdministracion.Ins_Auditoria(V_Usuario, "InsUsuarios", "Cambio estado de usuario a: " + dto.Bloqueado, Convert.ToString(dto.Identificacion), V_Maquina);
                return Json(new { success = true, data = retorno.Respuesta, message = retorno.Mensaje });
            }
            else
            {
                return Json(new { success = false, data = retorno.Respuesta, message = retorno.Mensaje });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddMarca(DtoDominios dto)
        {
            //RespuestaDto retorno = new RespuestaDto();

            var retorno = new RespuestaDto<List<DtoDominios>>();
            if (dto.IdDominio > 0)
            {
                retorno = await _IDbProducto.F_UpdateMarca(dto);
             

            }
            else {
                retorno = await _IDbProducto.F_AddMarca(dto);
            }
            if (retorno.Estado)
            {
                //var auditoria = _DbAdministracion.Ins_Auditoria(V_Usuario, "InsUsuarios", "Cambio estado de usuario a: " + dto.Bloqueado, Convert.ToString(dto.Identificacion), V_Maquina);
                return Json(new { success = true, data = retorno.Respuesta, message = retorno.Mensaje });
            }
            else
            {
                return Json(new { success = false, data = retorno.Respuesta, message = retorno.Mensaje });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddCategoria(DtoDominios dto)
        {

            var retorno = new RespuestaDto<List<DtoDominios>>();
            if (dto.IdDominio > 0)
            {
                retorno = await _IDbProducto.F_UpdateCategoria(dto);
            }
            else
            {
                retorno = await _IDbProducto.F_AddCategoria(dto);
            }
            if (retorno.Estado)
            {
                //var auditoria = _DbAdministracion.Ins_Auditoria(V_Usuario, "InsUsuarios", "Cambio estado de usuario a: " + dto.Bloqueado, Convert.ToString(dto.Identificacion), V_Maquina);
                return Json(new { success = true, data = retorno.Respuesta, message = retorno.Mensaje });
            }
            else
            {
                return Json(new { success = false, data = retorno.Respuesta, message = retorno.Mensaje });
            }
        }
        //Excel con contraseña
        [HttpGet]
        public async Task<FileContentResult> ExcelProductoTotal()
        {
            try
            {
                var clave = User.FindFirstValue("Clave");
                var retorno = await _IDbProducto.F_GetProductosGrillaTotal();
                string[] columns = { "PRODUCTO_ID", "MARCA_ID", "CATEGORIA_ID", "NOMBRE", "DESCRIPCION", "CANTIDAD", "CANTIDAD_TOTAL", "PRECIO", "VIGENTE", "FECHA_CREACIONS", "MAQUINA_CREACION", "ACTUALIZADO_POR", "Marca", "Categoria" };

                byte[] filecontent = ExcelExportHelperPass.ExportExcel(retorno.ToList(), "Listado General Productos Totales", Convert.ToString(clave), true, columns);

                return File(filecontent, ExcelExportHelper.ExcelContentType, "ExportExcelProductosTotal.xlsx");
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        //Excel con contraseña
        public async Task<FileContentResult> ExcelProductoVigentes()
        {
            try
            {
                var clave = User.FindFirstValue("Clave");
                var retorno = await _IDbProducto.F_GetProductosGrilla();
                string[] columns = { "PRODUCTO_ID", "MARCA_ID", "CATEGORIA_ID", "NOMBRE", "DESCRIPCION", "CANTIDAD", "CANTIDAD_TOTAL", "PRECIO", "VIGENTE", "FECHA_CREACIONS", "MAQUINA_CREACION", "ACTUALIZADO_POR", "Marca", "Categoria" };

                byte[] filecontent = ExcelExportHelperPass.ExportExcel(retorno.ToList(), "Listado General Productos Vigentes", Convert.ToString(clave), true, columns);

                return File(filecontent, ExcelExportHelper.ExcelContentType, "ExportExcelProductosVigentes.xlsx");
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        //Excel con contraseña
        public async Task<FileContentResult> ExcelProductoDesactivados()
        {
            try
            {
                var clave = User.FindFirstValue("Clave");
                var retorno = await _IDbProducto.F_GetProductosGrillaDesactivado();
                string[] columns = { "PRODUCTO_ID", "MARCA_ID", "CATEGORIA_ID", "NOMBRE", "DESCRIPCION", "CANTIDAD", "CANTIDAD_TOTAL", "PRECIO", "VIGENTE", "FECHA_CREACIONS", "MAQUINA_CREACION", "ACTUALIZADO_POR", "Marca", "Categoria" };

                byte[] filecontent = ExcelExportHelperPass.ExportExcel(retorno.ToList(), "Listado General Productos Desactivados", Convert.ToString(clave), true, columns);

                return File(filecontent, ExcelExportHelper.ExcelContentType, "ExportExcelProductosDesactivados.xlsx");
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Ciudad Envia
        [HttpGet]
        public  IActionResult CiudadEnvia()
        {
            return View();

        }
        [HttpPost]
        public async Task<JsonResult> UpdateCiudad(DtoCiudadEnvia dto)
        {

            var V_Maquina = GetClientIpAddress(HttpContext);
            var V_Usuario = Convert.ToInt32(User.FindFirstValue("Identificacion"));


            //dto.MAQUINA_CREACION = V_Maquina;
            //dto.ACTUALIZADO_POR = V_Usuario;

            var retorno = await _IDbProducto.UpdateCiudad(dto);
            if (retorno.Estado)
            {
                //var auditoria = _DbAdministracion.Ins_Auditoria(V_Usuario, "InsUsuarios", "Cambio estado de usuario a: " + dto.Bloqueado, Convert.ToString(dto.Identificacion), V_Maquina);
                return Json(new { success = true, data = retorno.Respuesta, message = retorno.Mensaje });
            }
            else
            {
                return Json(new { success = false, data = retorno.Respuesta, message = retorno.Mensaje });
            }
        }
        [HttpPost]
        public async Task<JsonResult> AddCiudad(DtoCiudadEnvia dto)
        {
            //RespuestaDto retorno = new RespuestaDto();

            var retorno = new RespuestaDto<List<DtoCiudadEnvia>>();
            if (dto.CODIGO > 0)
            {
                
                retorno = await _IDbProducto.F_UpdateCiudad(dto);


            }
            else
            {
                retorno = await _IDbProducto.F_AddCiudad(dto);
            }
            if (retorno.Estado)
            {
                //var auditoria = _DbAdministracion.Ins_Auditoria(V_Usuario, "InsUsuarios", "Cambio estado de usuario a: " + dto.Bloqueado, Convert.ToString(dto.Identificacion), V_Maquina);
                return Json(new { success = true, data = retorno.Respuesta, message = retorno.Mensaje });
            }
            else
            {
                return Json(new { success = false, data = retorno.Respuesta, message = retorno.Mensaje });
            }
        }
        [HttpPost]
        public async Task<JsonResult> F_GetGrillaCiudadEnvia(int numero)
        {
            var result = await _IDbProducto.F_GetGrillaCiudadEnvia(numero);

            if (result.Count > 0)
            {
                return Json(new { success = true, data = result });
            }
            else
            {
                return Json(new { success = false, data = result });
            }
        }

        [HttpGet]
        public IActionResult GestionCiudad()
        {
            return View();

        }

    }
}
