using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocio.Contratos.ConsultasExternas;
using ProyectoEcommers.Models;
using System.Security.Claims;

namespace ProyectoEcommers.Controllers
{
    [Authorize(Roles = "1,2,3,4")]
    public class SliderController : Controller
    {

        private readonly IDbSlider _IDbSlider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDbAdministracion _dbAdministracion;
        private readonly IBLConsultasExternas _BlConsultasExternas;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SliderController(IDbSlider iDbSlider, IHttpContextAccessor httpContextAccessor, IDbAdministracion dbAdministracion, IBLConsultasExternas bLConsultasExternas, IWebHostEnvironment webHostEnvironment)
        {
            _IDbSlider = iDbSlider;
            _httpContextAccessor = httpContextAccessor;
            _dbAdministracion = dbAdministracion;
            _BlConsultasExternas = bLConsultasExternas;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public async Task<IActionResult> Slider()
        {
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
                        Ruta = "~/img/Carrusel/19957.jpg"
                    };
                    SlidersView.Add(SliderView);
                    return View(SlidersView);
                }



                //Immplementar con microservicio


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

                ruta = Path.Combine(webRootPath, "img/Carrusel/") + Consecutivo.ToString() + formato;
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
                return "~/img/Carrusel/" + Consecutivo.ToString() + extensionArchivo;
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

                ruta = Path.Combine(webRootPath, "img/Carrusel/") + Consecutivo.ToString() + extensionArchivo;
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
                return "~/img/Carrusel/" + Consecutivo.ToString() + extensionArchivo;
            }
            else
            {
                return Resultado;
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Ins_ImagenSlider(IFormFile Archivo, long Identificacion)
        {

            var insFotografia = new DtoSlider();
            string ImagenBase64;

            if (Archivo.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    Archivo.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    ImagenBase64 = Convert.ToBase64String(fileBytes);
                }
            }
            else
            {
                return Json(new { success = false, data = new List<Int16?>(), message = "Verifique el archivo que esta subiendo" });

            }

            //string Extension = "";
            //if (Archivo.ContentType == "application/pdf")
            //{
            //    Extension = ".pdf";
            //}
            //else if (Archivo.ContentType == "image/jpeg")
            //{
            //    Extension = ".jpg";
            //}
            //else
            //{
            //    Extension = ".mp4";
            //}

            insFotografia.FileName = Archivo.FileName;
            string Ruta = $"{"\\img\\carrusel\\"}" + Archivo.FileName;
            insFotografia.FotoBase64 = ImagenBase64;
            insFotografia.Identificacion = Identificacion;
            insFotografia.Usuario = Convert.ToInt64(User.FindFirstValue("Identificacion"));
            insFotografia.Maquina = GetClientIpAddress(HttpContext);
            insFotografia.ContentType = Archivo.ContentType;
            insFotografia.Ruta = Ruta;

            var Resultado = await _IDbSlider.F_GetImagen(insFotografia);
            if (Resultado != 0)
            {
                //var Auditoria = await _DbAdministracion.Ins_Auditoria(Convert.ToInt64(User.FindFirstValue("Identificacion")), "InsFotoEmpleado", "Inserta Fotografia Empleado", Convert.ToString(insFotografia.Identificacion), HttpContext.Session.GetString("IpMaquina"));
                return Json(new { success = true, data = Resultado, message = "Se realiza el registro exitosamente" });
            }
            else
            {
                return Json(new { success = false, data = Resultado, message = "No fue posible guardar la fotografia" });
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

        [HttpPost]
        public async Task<JsonResult> P_GetImagenes()
        {

            var V_Maquina = GetClientIpAddress(HttpContext);
            var V_Usuario = Convert.ToInt64(User.FindFirstValue("Identificacion"));

            var retorno = await _IDbSlider.F_GetSlide();
            if (retorno.Estado)
            {
                // var auditoria = IDbSlider.Ins_Auditoria(V_Usuario, "P_GetUsuarios", "Consulta Usuarios del Sistema: ", "", V_Maquina);
                return Json(new { success = true, data = retorno.Respuesta, message = retorno.Mensaje });
            }
            else
            {
                return Json(new { success = false, data = 0, message = retorno.Mensaje });
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsUsuarios(DtoSlider dto)
        {

            var V_Maquina = GetClientIpAddress(HttpContext);
            var V_Usuario = Convert.ToInt64(User.FindFirstValue("Identificacion"));


            dto.Maquina = V_Maquina;
            dto.Usuario = V_Usuario;

            var retorno = await _IDbSlider.F_UpdateSlide(dto);
            if (retorno.Estado)
            {

                //Permite Eliminar un archivo
                string[] formatos = new[] { ".tiff", ".ief", ".gif", ".jpg", ".png" };
                string webRootPath = _webHostEnvironment.WebRootPath;
                foreach (string formato in formatos)
                {
                    var rutaArchivo = Path.Combine(webRootPath, "img/Carrusel/") + dto.Consecutivo.ToString() + formato;
                    if (System.IO.File.Exists(rutaArchivo))
                    {
                        System.IO.File.Delete(rutaArchivo);
                    }
                }
                //var auditoria = _DbAdministracion.Ins_Auditoria(V_Usuario, "InsUsuarios", "Cambio estado de usuario a: " + dto.Bloqueado, Convert.ToString(dto.Identificacion), V_Maquina);
                return Json(new { success = true, data = retorno.Respuesta, message = retorno.Mensaje });
            }
            else
            {
                return Json(new { success = false, data = retorno.Respuesta, message = retorno.Mensaje });
            }
        }
        //Creacion de slider Izquierdo de la pagina principal

        public async Task<IActionResult> SliderIzquierdo()
        {
            {
                try
                {
                    var ImagenesSlider = await _BlConsultasExternas.F_GetCarruselImagenesIzquierdoPonalAsyc();

                    var SlidersView = new List<DtoSliderizquierdo>();
                    if (ImagenesSlider.Respuesta.Count > 0)
                    {


                        foreach (var item in ImagenesSlider.Respuesta)
                        {
                            string ruta = ConsultarRutaIzquierdo(Convert.ToInt32(item.Consecutivo));
                            string ruta1 = "";

                            if (ruta == null || ruta == "")
                            {
                                ruta1 = F_GetImagenesIzquierdo(Convert.ToInt32(item.Consecutivo), item.ContentType, item.Foto);
                            }
                            else
                            {
                                ruta1 = ruta;
                            }
                            var SliderView = new DtoSliderizquierdo
                            {
                                Consecutivo = item.Consecutivo,
                                ContentType = item.ContentType,
                                FileName = item.FileName,
                                Ruta = ruta1
                            };
                            SlidersView.Add(SliderView);
                        }
                    }
                    else
                    {
                        var SliderView = new DtoSliderizquierdo
                        {
                            Consecutivo = 0,
                            ContentType = "",
                            FileName = "",
                            Ruta = ""
                        };
                        SlidersView.Add(SliderView);

                    }
                    return View(SlidersView);

                }
                catch (Exception e)
                {
                    var SlidersView = new List<DtoSliderizquierdo>();
                    var SliderView = new DtoSliderizquierdo
                    {
                        Consecutivo = 19957,
                        ContentType = "image/jpeg",
                        FileName = "ARTE4_polired.jpg",
                        Ruta = "~/img/SliderIzquierdo/19957.jpg"
                    };
                    SlidersView.Add(SliderView);
                    return View(SlidersView);
                }



                //Immplementar con microservicio


            }
        }

        public string ConsultarRutaIzquierdo(int Consecutivo)
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

                ruta = Path.Combine(webRootPath, "img/SliderIzquierdo/") + Consecutivo.ToString() + formato;
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
                return Consecutivo.ToString() + extensionArchivo;
            }
            else
            {
                return Resultado;
            }
        }

        public string F_GetImagenesIzquierdo(int Consecutivo, string ContentType, string Foto)
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

                ruta = Path.Combine(webRootPath, "img/SliderIzquierdo/") + Consecutivo.ToString() + extensionArchivo;
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
                return "/img/SliderIzquierdo/" + Consecutivo.ToString() + extensionArchivo;
            }
            else
            {
                return Resultado;
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Ins_ImagenSliderIzquierdo(IFormFile Archivo, long Identificacion)
        {

            var insFotografia = new DtoSliderizquierdo();
            string ImagenBase64;

            if (Archivo.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    Archivo.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    ImagenBase64 = Convert.ToBase64String(fileBytes);
                }
            }
            else
            {
                return Json(new { success = false, data = new List<Int16?>(), message = "Verifique el archivo que esta subiendo" });

            }

            //string Extension = "";
            //if (Archivo.ContentType == "application/pdf")
            //{
            //    Extension = ".pdf";
            //}
            //else if (Archivo.ContentType == "image/jpeg")
            //{
            //    Extension = ".jpg";
            //}
            //else
            //{
            //    Extension = ".mp4";
            //}

            insFotografia.FileName = Archivo.FileName;
            string Ruta = $"{"\\img\\SliderIzquierdo\\"}" + Archivo.FileName;
            insFotografia.FotoBase64 = ImagenBase64;
            insFotografia.Identificacion = Identificacion;
            insFotografia.Usuario = Convert.ToInt64(User.FindFirstValue("Identificacion"));
            insFotografia.Maquina = GetClientIpAddress(HttpContext);
            insFotografia.ContentType = Archivo.ContentType;
            insFotografia.Ruta = Ruta;

            var Resultado = await _IDbSlider.F_GetImagenIzquierdo(insFotografia);
            if (Resultado != 0)
            {
                //var Auditoria = await _DbAdministracion.Ins_Auditoria(Convert.ToInt64(User.FindFirstValue("Identificacion")), "InsFotoEmpleado", "Inserta Fotografia Empleado", Convert.ToString(insFotografia.Identificacion), HttpContext.Session.GetString("IpMaquina"));
                return Json(new { success = true, data = Resultado, message = "Se realiza el registro exitosamente" });
            }
            else
            {
                return Json(new { success = false, data = Resultado, message = "No fue posible guardar la fotografia" });
            }

        }
        [HttpPost]
        public async Task<JsonResult> P_GetImagenesIzquierdo()
        {

            var V_Maquina = GetClientIpAddress(HttpContext);
            var V_Usuario = Convert.ToInt64(User.FindFirstValue("Identificacion"));

            var retorno = await _IDbSlider.F_GetSlideIzquierdo();
            if (retorno.Estado)
            {
                // var auditoria = IDbSlider.Ins_Auditoria(V_Usuario, "P_GetUsuarios", "Consulta Usuarios del Sistema: ", "", V_Maquina);
                return Json(new { success = true, data = retorno.Respuesta, message = retorno.Mensaje });
            }
            else
            {
                return Json(new { success = false, data = retorno.Respuesta, message = retorno.Mensaje });
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsUsuariosIzquierdo(DtoSliderizquierdo dto)
        {

            var V_Maquina = GetClientIpAddress(HttpContext);
            var V_Usuario = Convert.ToInt64(User.FindFirstValue("Identificacion"));


            dto.Maquina = V_Maquina;
            dto.Usuario = V_Usuario;

            var retorno = await _IDbSlider.F_UpdateSlideIzquierdo(dto);
            if (retorno.Estado)
            {
                //Permite Eliminar un archivo
                string[] formatos = new[] { ".tiff", ".ief", ".gif", ".jpg", ".png" };
                string webRootPath = _webHostEnvironment.WebRootPath;
                foreach (string formato in formatos)
                {
                    var rutaArchivo = Path.Combine(webRootPath, "img/SliderIzquierdo/") + dto.Consecutivo.ToString() + formato;
                    if (System.IO.File.Exists(rutaArchivo))
                    {
                        System.IO.File.Delete(rutaArchivo);
                    }
                }
                //var auditoria = _DbAdministracion.Ins_Auditoria(V_Usuario, "InsUsuarios", "Cambio estado de usuario a: " + dto.Bloqueado, Convert.ToString(dto.Identificacion), V_Maquina);
                return Json(new { success = true, data = retorno.Respuesta, message = retorno.Mensaje });
            }
            else
            {
                return Json(new { success = false, data = retorno.Respuesta, message = retorno.Mensaje });
            }
        }
        [HttpPost]
        public async Task<JsonResult> ConsultaImagenIzquierda()
        {
            try
            {
                var ImagenesSlider = await _BlConsultasExternas.F_GetCarruselImagenesIzquierdoPonalAsyc();

                var SlidersView = new List<DtoSliderizquierdo>();

                foreach (var item in ImagenesSlider.Respuesta)
                {
                    string ruta = ConsultarRutaIzquierdo(Convert.ToInt32(item.Consecutivo));
                    string ruta1 = "";

                    if (ruta == null || ruta == "")
                    {
                        ruta1 = F_GetImagenesIzquierdo(Convert.ToInt32(item.Consecutivo), item.ContentType, item.Foto);
                    }
                    else
                    {
                        ruta1 = ruta;
                    }
                    var SliderView = new DtoSliderizquierdo
                    {
                        Consecutivo = item.Consecutivo,
                        ContentType = item.ContentType,
                        FileName = item.FileName,
                        Ruta = ruta1
                    };
                    SlidersView.Add(SliderView);
                }
                if (SlidersView.Count > 0)
                {
                    // var auditoria = IDbSlider.Ins_Auditoria(V_Usuario, "P_GetUsuarios", "Consulta Usuarios del Sistema: ", "", V_Maquina);
                    return Json(new { success = true, data = SlidersView, message = "" });
                }
                else
                {
                    return Json(new { success = false, data = SlidersView, message = "" });
                }

            }
            catch (Exception e)
            {
                var SlidersView = new List<DtoSliderizquierdo>();
                var SliderView = new DtoSliderizquierdo
                {
                    Consecutivo = 19957,
                    ContentType = "image/jpeg",
                    FileName = "ARTE4_polired.jpg",
                    Ruta = "/img/SliderIzquierdo/1.jpg"
                };
                SlidersView.Add(SliderView);
                return Json(new { success = true, data = SlidersView, message = "" });
            }



            //Immplementar con microservicio
        }
        //Creacion de slider inferior de la pagina principal
        [HttpGet]
        public async Task<IActionResult> SliderInferior()
        {
            {
                try
                {
                    var ImagenesSlider = await _BlConsultasExternas.F_GetCarruselImagenesInferiorPonalAsyc();

                    var SlidersView = new List<DtoSliderInferior>();
                    if (SlidersView.Count > 0)
                    {


                        foreach (var item in ImagenesSlider.Respuesta)
                        {
                            string ruta = ConsultarRutaInferior(Convert.ToInt32(item.Consecutivo));
                            string ruta1 = "";

                            if (ruta == null || ruta == "")
                            {
                                ruta1 = F_GetImagenesInferior(Convert.ToInt32(item.Consecutivo), item.ContentType, item.Foto);
                            }
                            else
                            {
                                ruta1 = ruta;
                            }
                            var SliderView = new DtoSliderInferior
                            {
                                Consecutivo = item.Consecutivo,
                                ContentType = item.ContentType,
                                FileName = item.FileName,
                                Ruta = ruta1
                            };
                            SlidersView.Add(SliderView);
                        }
                    }
                    else
                    {
                        var SliderView = new DtoSliderInferior
                        {
                            Consecutivo = 0,
                            ContentType = "",
                            FileName = "",
                            Ruta = ""
                        };
                        SlidersView.Add(SliderView);

                    }
                    return View(SlidersView);

                }
                catch (Exception e)
                {
                    var SlidersView = new List<DtoSliderInferior>();
                    var SliderView = new DtoSliderInferior
                    {
                        Consecutivo = 19957,
                        ContentType = "image/jpeg",
                        FileName = "ARTE4_polired.jpg",
                        Ruta = "~/img/SliderInferior/1.jpg"
                    };
                    SlidersView.Add(SliderView);
                    return View(SlidersView);
                }



                //Immplementar con microservicio


            }
        }
        [HttpPost]
        public string ConsultarRutaInferior(int Consecutivo)
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

                ruta = Path.Combine(webRootPath, "img/SliderInferior/") + Consecutivo.ToString() + formato;
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
                return Consecutivo.ToString() + extensionArchivo;
            }
            else
            {
                return Resultado;
            }
        }
        [HttpPost]
        public string F_GetImagenesInferior(int Consecutivo, string ContentType, string Foto)
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

                ruta = Path.Combine(webRootPath, "img/SliderInferior/") + Consecutivo.ToString() + extensionArchivo;
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
                return "/img/SliderInferior/" + Consecutivo.ToString() + extensionArchivo;
            }
            else
            {
                return Resultado;
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Ins_ImagenSliderInferior(IFormFile Archivo, long Identificacion)
        {

            var insFotografia = new DtoSliderInferior();
            string ImagenBase64;

            if (Archivo.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    Archivo.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    ImagenBase64 = Convert.ToBase64String(fileBytes);
                }
            }
            else
            {
                return Json(new { success = false, data = new List<Int16?>(), message = "Verifique el archivo que esta subiendo" });

            }

            //string Extension = "";
            //if (Archivo.ContentType == "application/pdf")
            //{
            //    Extension = ".pdf";
            //}
            //else if (Archivo.ContentType == "image/jpeg")
            //{
            //    Extension = ".jpg";
            //}
            //else
            //{
            //    Extension = ".mp4";
            //}

            insFotografia.FileName = Archivo.FileName;
            string Ruta = $"{"\\img\\SliderInferior\\"}" + Archivo.FileName;
            insFotografia.FotoBase64 = ImagenBase64;
            insFotografia.Identificacion = Identificacion;
            insFotografia.Usuario = Convert.ToInt64(User.FindFirstValue("Identificacion"));
            insFotografia.Maquina = GetClientIpAddress(HttpContext);
            insFotografia.ContentType = Archivo.ContentType;
            insFotografia.Ruta = Ruta;

            var Resultado = await _IDbSlider.F_GetImagenInferior(insFotografia);
            if (Resultado != 0)
            {
                //var Auditoria = await _DbAdministracion.Ins_Auditoria(Convert.ToInt64(User.FindFirstValue("Identificacion")), "InsFotoEmpleado", "Inserta Fotografia Empleado", Convert.ToString(insFotografia.Identificacion), HttpContext.Session.GetString("IpMaquina"));
                return Json(new { success = true, data = Resultado, message = "Se realiza el registro exitosamente" });
            }
            else
            {
                return Json(new { success = false, data = Resultado, message = "No fue posible guardar la fotografia" });
            }

        }
        [HttpPost]
        public async Task<JsonResult> P_GetImagenesInferior()
        {

            var V_Maquina = GetClientIpAddress(HttpContext);
            var V_Usuario = Convert.ToInt64(User.FindFirstValue("Identificacion"));

            var retorno = await _IDbSlider.F_GetSlideInferior();
            if (retorno.Estado)
            {
                // var auditoria = IDbSlider.Ins_Auditoria(V_Usuario, "P_GetUsuarios", "Consulta Usuarios del Sistema: ", "", V_Maquina);
                return Json(new { success = true, data = retorno.Respuesta, message = retorno.Mensaje });
            }
            else
            {
                return Json(new { success = false, data = retorno.Respuesta, message = retorno.Mensaje });
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsUsuariosInferior(DtoSliderInferior dto)
        {

            var V_Maquina = GetClientIpAddress(HttpContext);
            var V_Usuario = Convert.ToInt64(User.FindFirstValue("Identificacion"));


            dto.Maquina = V_Maquina;
            dto.Usuario = V_Usuario;

            var retorno = await _IDbSlider.F_UpdateSlideInferior(dto);
            if (retorno.Estado)
            {
                //Permite Eliminar un archivo
                string[] formatos = new[] { ".tiff", ".ief", ".gif", ".jpg", ".png" };
                string webRootPath = _webHostEnvironment.WebRootPath;
                foreach (string formato in formatos)
                {
                    var rutaArchivo = Path.Combine(webRootPath, "img/SliderInferior/") + dto.Consecutivo.ToString() + formato;
                    if (System.IO.File.Exists(rutaArchivo))
                    {
                        System.IO.File.Delete(rutaArchivo);
                    }
                }
                //var auditoria = _DbAdministracion.Ins_Auditoria(V_Usuario, "InsUsuarios", "Cambio estado de usuario a: " + dto.Bloqueado, Convert.ToString(dto.Identificacion), V_Maquina);
                return Json(new { success = true, data = retorno.Respuesta, message = retorno.Mensaje });
            }
            else
            {
                return Json(new { success = false, data = retorno.Respuesta, message = retorno.Mensaje });
            }
        }
        [HttpPost]
        public async Task<JsonResult> ConsultaImagenInferior()
        {
            try
            {
                var ImagenesSlider = await _BlConsultasExternas.F_GetCarruselImagenesInferiorPonalAsyc();

                var SlidersView = new List<DtoSliderInferior>();

                foreach (var item in ImagenesSlider.Respuesta)
                {
                    string ruta = ConsultarRutaInferior(Convert.ToInt32(item.Consecutivo));
                    string ruta1 = "";

                    if (ruta == null || ruta == "")
                    {
                        ruta1 = F_GetImagenesInferior(Convert.ToInt32(item.Consecutivo), item.ContentType, item.Foto);
                    }
                    else
                    {
                        ruta1 = ruta;
                    }
                    var SliderView = new DtoSliderInferior
                    {
                        Consecutivo = item.Consecutivo,
                        ContentType = item.ContentType,
                        FileName = item.FileName,
                        Ruta = ruta1
                    };
                    SlidersView.Add(SliderView);
                }
                if (SlidersView.Count > 0)
                {
                    // var auditoria = IDbSlider.Ins_Auditoria(V_Usuario, "P_GetUsuarios", "Consulta Usuarios del Sistema: ", "", V_Maquina);
                    return Json(new { success = true, data = SlidersView, message = "" });
                }
                else
                {
                    return Json(new { success = false, data = SlidersView, message = "" });
                }

            }
            catch (Exception e)
            {
                var SlidersView = new List<DtoSliderInferior>();
                var SliderView = new DtoSliderInferior
                {
                    Consecutivo = 19957,
                    ContentType = "image/jpeg",
                    FileName = "ARTE4_polired.jpg",
                    Ruta = "/img/SliderInferior/1.jpg"
                };
                SlidersView.Add(SliderView);
                return Json(new { success = true, data = SlidersView, message = "" });
            }



            //Immplementar con microservicio
        }
        //Creacion de slider Comentarios de la pagina principal
        [HttpGet]
        public async Task<IActionResult> SliderComentarios()
        {
            {
                try
                {
                    var ImagenesSlider = await _BlConsultasExternas.F_GetCarruselImagenesComentariosPonalAsyc();

                    var SlidersView = new List<DtoSliderComentarios>();
                    if (SlidersView.Count > 0)
                    {


                        foreach (var item in ImagenesSlider.Respuesta)
                        {
                            string ruta = ConsultarRutaComentarios(Convert.ToInt32(item.Consecutivo));
                            string ruta1 = "";

                            if (ruta == null || ruta == "")
                            {
                                ruta1 = F_GetImagenesComentarios(Convert.ToInt32(item.Consecutivo), item.ContentType, item.Foto);
                            }
                            else
                            {
                                ruta1 = ruta;
                            }
                            var SliderView = new DtoSliderComentarios
                            {
                                Consecutivo = item.Consecutivo,
                                ContentType = item.ContentType,
                                FileName = item.FileName,
                                Ruta = ruta1
                            };
                            SlidersView.Add(SliderView);
                        }
                    }
                    else
                    {
                        var SliderView = new DtoSliderComentarios
                        {
                            Consecutivo = 0,
                            ContentType = "",
                            FileName = "",
                            Ruta = ""
                        };
                        SlidersView.Add(SliderView);

                    }
                    return View(SlidersView);

                }
                catch (Exception e)
                {
                    var SlidersView = new List<DtoSliderComentarios>();
                    var SliderView = new DtoSliderComentarios
                    {
                        Consecutivo = 19957,
                        ContentType = "image/jpeg",
                        FileName = "ARTE4_polired.jpg",
                        Ruta = "~/img/SliderComentarios/1.jpg"
                    };
                    SlidersView.Add(SliderView);
                    return View(SlidersView);
                }



                //Immplementar con microservicio


            }
        }
        [HttpGet]
        public string ConsultarRutaComentarios(int Consecutivo)
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

                ruta = Path.Combine(webRootPath, "img/SliderComentarios/") + Consecutivo.ToString() + formato;
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
                //return "/img/SliderComentarios/" + Consecutivo.ToString() + extensionArchivo;
                return Consecutivo.ToString() + extensionArchivo;
            }
            else
            {
                return Resultado;
            }
        }
        [HttpGet]
        public string F_GetImagenesComentarios(int Consecutivo, string ContentType, string Foto)
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

                ruta = Path.Combine(webRootPath, "img/SliderInferior/") + Consecutivo.ToString() + extensionArchivo;
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
                return "/img/SliderComentarios/" + Consecutivo.ToString() + extensionArchivo;
            }
            else
            {
                return Resultado;
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Ins_ImagenSliderComentarios(IFormFile Archivo, string FechaComentario, string TituloComentario, string Comentario)
        {

            var insFotografia = new DtoSliderComentarios();
            string ImagenBase64;

            if (Archivo.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    Archivo.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    ImagenBase64 = Convert.ToBase64String(fileBytes);
                }
            }
            else
            {
                return Json(new { success = false, data = new List<Int16?>(), message = "Verifique el archivo que esta subiendo" });

            }

            //string Extension = "";
            //if (Archivo.ContentType == "application/pdf")
            //{
            //    Extension = ".pdf";
            //}
            //else if (Archivo.ContentType == "image/jpeg")
            //{
            //    Extension = ".jpg";
            //}
            //else
            //{
            //    Extension = ".mp4";
            //}

            insFotografia.FileName = Archivo.FileName;
            string Ruta = $"{"\\img\\SliderComentarios\\"}" + Archivo.FileName;
            insFotografia.FotoBase64 = ImagenBase64;
            insFotografia.Identificacion = Convert.ToInt64(User.FindFirstValue("Identificacion"));
            insFotografia.Usuario = Convert.ToInt64(User.FindFirstValue("Identificacion"));
            insFotografia.Maquina = GetClientIpAddress(HttpContext);
            insFotografia.ContentType = Archivo.ContentType;
            insFotografia.Ruta = Ruta;
            insFotografia.FechaComentario = FechaComentario;
            insFotografia.TituloComentario = TituloComentario;
            insFotografia.Comentario = Comentario;

            var Resultado = await _IDbSlider.F_GetImagenComentarios(insFotografia);
            if (Resultado != 0)
            {
                //var Auditoria = await _DbAdministracion.Ins_Auditoria(Convert.ToInt64(User.FindFirstValue("Identificacion")), "InsFotoEmpleado", "Inserta Fotografia Empleado", Convert.ToString(insFotografia.Identificacion), HttpContext.Session.GetString("IpMaquina"));
                return Json(new { success = true, data = Resultado, message = "Se realiza el registro exitosamente" });
            }
            else
            {
                return Json(new { success = false, data = Resultado, message = "No fue posible guardar la fotografia" });
            }

        }
        [HttpPost]
        public async Task<JsonResult> P_GetImagenesComentarios()
        {

            var V_Maquina = GetClientIpAddress(HttpContext);
            var V_Usuario = Convert.ToInt64(User.FindFirstValue("Identificacion"));

            var retorno = await _IDbSlider.F_GetSlideComentarios();
            if (retorno.Estado)
            {
                // var auditoria = IDbSlider.Ins_Auditoria(V_Usuario, "P_GetUsuarios", "Consulta Usuarios del Sistema: ", "", V_Maquina);
                return Json(new { success = true, data = retorno.Respuesta, message = retorno.Mensaje });
            }
            else
            {
                return Json(new { success = false, data = retorno.Respuesta, message = retorno.Mensaje });
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsUsuariosComentarios(DtoSliderComentarios dto)
        {

            var V_Maquina = GetClientIpAddress(HttpContext);
            var V_Usuario = Convert.ToInt64(User.FindFirstValue("Identificacion"));


            dto.Maquina = V_Maquina;
            dto.Usuario = V_Usuario;

            var retorno = await _IDbSlider.F_UpdateSlideComentarios(dto);
            if (retorno.Estado)
            {
                //Permite Eliminar un archivo
                string[] formatos = new[] { ".tiff", ".ief", ".gif", ".jpg", ".png" };
                string webRootPath = _webHostEnvironment.WebRootPath;
                foreach (string formato in formatos)
                {
                    var rutaArchivo = Path.Combine(webRootPath, "img/SliderComentarios/") + dto.Consecutivo.ToString() + formato;
                    if (System.IO.File.Exists(rutaArchivo))
                    {
                        System.IO.File.Delete(rutaArchivo);
                    }
                }
                //var auditoria = _DbAdministracion.Ins_Auditoria(V_Usuario, "InsUsuarios", "Cambio estado de usuario a: " + dto.Bloqueado, Convert.ToString(dto.Identificacion), V_Maquina);
                return Json(new { success = true, data = retorno.Respuesta, message = retorno.Mensaje });
            }
            else
            {
                return Json(new { success = false, data = retorno.Respuesta, message = retorno.Mensaje });
            }
        }
        [HttpPost]
        public async Task<JsonResult> ConsultaImagenComentarios()
        {
            try
            {
                var ImagenesSlider = await _BlConsultasExternas.F_GetCarruselImagenesComentariosPonalAsyc();

                var SlidersView = new List<DtoSliderComentarios>();

                foreach (var item in ImagenesSlider.Respuesta)
                {
                    string ruta = ConsultarRutaComentarios(Convert.ToInt32(item.Consecutivo));
                    string ruta1 = "";

                    if (ruta == null || ruta == "")
                    {
                        ruta1 = F_GetImagenesComentarios(Convert.ToInt32(item.Consecutivo), item.ContentType, item.Foto);
                    }
                    else
                    {
                        ruta1 = ruta;
                    }
                    var fecha = Convert.ToDateTime(item.FechaComentario);

                    // Utiliza el método ToString con un formato personalizado
                    string fechaFormateada = fecha.ToString("dd-MMM-yyyy");
                    var SliderView = new DtoSliderComentarios
                    {
                        Consecutivo = item.Consecutivo,
                        ContentType = item.ContentType,
                        FileName = item.FileName,
                        Ruta = ruta1,
                        FechaComentario = fechaFormateada,
                        TituloComentario = item.TituloComentario,
                        Comentario = item.Comentario
                    };
                    SlidersView.Add(SliderView);
                }
                if (SlidersView.Count > 0)
                {
                    // var auditoria = IDbSlider.Ins_Auditoria(V_Usuario, "P_GetUsuarios", "Consulta Usuarios del Sistema: ", "", V_Maquina);
                    return Json(new { success = true, data = SlidersView, message = "" });
                }
                else
                {
                    return Json(new { success = false, data = SlidersView, message = "" });
                }

            }
            catch (Exception e)
            {
                var SlidersView = new List<DtoSliderComentarios>();
                var SliderView = new DtoSliderComentarios
                {
                    Consecutivo = 19957,
                    ContentType = "image/jpeg",
                    FileName = "ARTE4_polired.jpg",
                    Ruta = "/img/SliderComentarios/1.jpg"
                };
                SlidersView.Add(SliderView);
                return Json(new { success = true, data = SlidersView, message = "" });
            }



            //Immplementar con microservicio
        }
        //Mensaje
        [HttpGet]
        public IActionResult Mensajes()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> Ins_Mensajes(string TituloMensaje, string Mensaje, string Vigente)
        {

            var insMensaje = new DtoMensajes();
            insMensaje.CreadoPor = Convert.ToInt32(User.FindFirstValue("Identificacion"));
            insMensaje.MaquinaCreacion = GetClientIpAddress(HttpContext);
            insMensaje.TituloMensaje = TituloMensaje;
            insMensaje.Mensaje = Mensaje;
            insMensaje.Vigente = Vigente;

            var Resultado = await _IDbSlider.Ins_Mensaje(insMensaje);
            if (Resultado != 0)
            {
                return Json(new { success = true, data = Resultado, message = "Se realiza el registro exitosamente" });
            }
            else
            {
                return Json(new { success = false, data = Resultado, message = "No fue posible guardar la fotografia" });
            }

        }
        [HttpPost]
        public async Task<JsonResult> P_GetMensajeGrilla()
        {

            var V_Maquina = GetClientIpAddress(HttpContext);
            var V_Usuario = Convert.ToInt64(User.FindFirstValue("Identificacion"));

            var retorno = await _IDbSlider.F_GetMensajeGrilla();
            if (retorno.Estado)
            {
                // var auditoria = IDbSlider.Ins_Auditoria(V_Usuario, "P_GetUsuarios", "Consulta Usuarios del Sistema: ", "", V_Maquina);
                return Json(new { success = true, data = retorno.Respuesta, message = retorno.Mensaje });
            }
            else
            {
                return Json(new { success = false, data = retorno.Respuesta, message = retorno.Mensaje });
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsIdMensaje(DtoMensajes dto)
        {

            var V_Maquina = GetClientIpAddress(HttpContext);
            var V_Usuario = Convert.ToInt32(User.FindFirstValue("Identificacion"));


            dto.MaquinaCreacion = V_Maquina;
            dto.CreadoPor = V_Usuario;

            var retorno = await _IDbSlider.InsIdMensaje(dto);
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

    }
}
