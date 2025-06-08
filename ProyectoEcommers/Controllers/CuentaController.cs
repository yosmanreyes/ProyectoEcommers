
namespace ProyectoEcommers.Controllers


{

    using Comun.Enumeraciones;
    using MailKit.Net.Smtp;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using MimeKit;
    using MySqlX.XDevAPI;
    using Negocio.Contratos.ConsultasExternas;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using ProyectoEcommers.Models;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Mail;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;






    //[Authorize]
    //[Route("344cc462-f756-41f4-aa00-cd4d36730e09")]
    public class CuentaController : Controller
    {
        private readonly IDbAdministracion _IDbAdministracion;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDbAdministracion _dbAdministracion;
        private readonly IBLConsultasExternas _BlConsultasExternas;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDbComentarios _IDbComentarios;
        private readonly IDbProducto _IDbProducto;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;

        private static readonly HttpClient client = new HttpClient();

  
        bool cotiene = false;
        public CuentaController(IDbAdministracion iDbAdministracion, IHttpContextAccessor httpContextAccessor, IDbAdministracion dbAdministracion, IBLConsultasExternas bLConsultasExternas, IWebHostEnvironment webHostEnvironment, IDbComentarios iDbComentarios, IDbProducto iDbProducto, IConfiguration configuration, HttpClient client)
        {
            _IDbAdministracion = iDbAdministracion;
            _httpContextAccessor = httpContextAccessor;
            _dbAdministracion = dbAdministracion;
            _BlConsultasExternas = bLConsultasExternas;
            _webHostEnvironment = webHostEnvironment;
            _IDbComentarios = iDbComentarios;
            _IDbProducto = iDbProducto;
            _configuration = configuration;
            _client = client;

        }

        public class OrderData
        {
            public int Quantity { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
        }
        public class PreferenceResponse
        {
            public string Id { get; set; }
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            return View();
        }
        //[ValidateAntiForgeryToken]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(DtoCredenciales loginUsuario, string? returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Action(nameof(HomeController.Index), "Home");
            if (!ModelState.IsValid)
                return View(loginUsuario);
            var Ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            HttpContext.Session.SetString("IpMaquina", Ip);

            var Usuario = await _IDbAdministracion.F_GetValidaUser(loginUsuario, Ip);
            if (Usuario.DtoUserRoles.Count != 0)
            {
                if (Usuario.Bloqueado == 1)
                {
                    ModelState.AddModelError("", "Su cuenta de usuario está DESHABILITADA, contacte al Administrador");
                    return View();
                }

                cotiene = Usuario != null ? Usuario.DtoUserRoles.Any(x => x.IdRol == Convert.ToInt32(Usuario.DtoUserRoles[0].IdRol)) : false;

                if (cotiene)
                {
                    //Generamos el Menú Administrador
                    var Menu = await _IDbAdministracion.F_GetMenu(Usuario.DtoUserRoles, Usuario.Identificacion);
                    var MenuGeneral = await _IDbAdministracion.F_GetMenuGeneral();

                    HttpContext.Session.SetObject("ListaMenu", Menu.OrderBy(x => x.DESCRIPCION));
                    HttpContext.Session.SetObject("ListaMenuGeneral", MenuGeneral);

                    CantidadVentaProductos.CantidadVentaProducto = Convert.ToString(Usuario.CantidadVentaProductos);
                    CantidadMensaje.CantidadMensajes = Convert.ToString(Usuario.CantidadMensajes);

                    //generamos los claims
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, Usuario.Usuario),
                        new Claim("Funcionario", Usuario.Funcionario),
                        new Claim("Identificacion", Convert.ToString(Usuario.Identificacion)),
                        new Claim("IdUsuario", Convert.ToString(Usuario.IdUsuario)),
                        new Claim("Apellidos", Convert.ToString(Usuario.Apellidos)),
                        new Claim("Correo", Convert.ToString(Usuario.Correo)),
                        new Claim("Celular", Convert.ToString(Usuario.Celular)),
                        new Claim("Usuario", Convert.ToString(Usuario.Usuario)),
                        new Claim("Clave", Convert.ToString(loginUsuario.ClaveEmpresarial))
                        //new Claim("CantidadVentaProductos", Convert.ToString(Usuario.CantidadVentaProductos)),
                        //new Claim("CantidadMensajes", Convert.ToString(Usuario.CantidadMensajes))
                    };
                    foreach (var rol in Usuario.DtoUserRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, Convert.ToString(rol.IdRol)));
                        claims.Add(new Claim(ClaimTypes.Actor, Convert.ToString(rol.Descripcion)));
                    }



                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                }

                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
            {
                ModelState.AddModelError("", "No se encontro el usuario");
                return View();
            }
        }
        [HttpGet]
        public async Task<IActionResult> CerrarSesion()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(CuentaController.Index), "Cuenta");
            //return RedirectToAction(nameof(InicioSesion));
        }
        [AllowAnonymous]
        public IActionResult Index(string? returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;


            return View();
        }
        public async Task<JsonResult> ConsultaDominios()
        {
            try
            {
                var Marcas = await _BlConsultasExternas.F_GetDominios(2);
                var Categorias = await _BlConsultasExternas.F_GetDominios(32);
                if (Marcas.Codigo == EstadoOperacion.Bueno)
                {
                    return Json(new { success = true, data = Marcas.Respuesta, data1 = Categorias.Respuesta, message = "" });
                }
                else
                {
                    return Json(new { success = false, data = Marcas.Respuesta, data1 = Categorias.Respuesta, message = "" });
                }

            }
            catch (Exception e)
            {
                var SlidersView = new List<DtoSlider>();
                var SliderView = new DtoSlider
                {
                    Consecutivo = 19957,
                    ContentType = "image/jpeg",
                    FileName = "ARTE4_polired.jpg",
                    Ruta = "/img/Carrusel/1.jpg"
                };
                SlidersView.Add(SliderView);
                return Json(new { success = true, data = SlidersView, message = "" });
            }

        }
        //Consulta slider principal
        [HttpPost]
        public async Task<JsonResult> Tiendas()
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
                var SlidersView = new List<DtoSlider>();
                var SliderView = new DtoSlider
                {
                    Consecutivo = 19957,
                    ContentType = "image/jpeg",
                    FileName = "ARTE4_polired.jpg",
                    Ruta = "/img/Carrusel/1.jpg"
                };
                SlidersView.Add(SliderView);
                return Json(new { success = true, data = SlidersView, message = "" });
            }



            //Immplementar con microservicio
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
                return Consecutivo.ToString() + extensionArchivo;
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
                return "/img/Carrusel/" + Consecutivo.ToString() + extensionArchivo;
            }
            else
            {
                return Resultado;
            }

        }
        //Consultas slider izquierdo
        [HttpPost]
        public async Task<JsonResult> Sliderizquierdo()
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
                    Ruta = "/img/SliderIzquierdo/2.jpg"
                };
                SlidersView.Add(SliderView);
                return Json(new { success = true, data = SlidersView, message = "" });
            }



            //Immplementar con microservicio
        }
        [HttpPost]
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
        [HttpPost]
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
                return "/img/Carrusel/" + Consecutivo.ToString() + extensionArchivo;
            }
            else
            {
                return Resultado;
            }

        }
        //Consultas slider Inferior
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
        //Consultas slider Comentarios
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
                        FechaComentario = Convert.ToString(fechaFormateada),
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
        [HttpPost]
        public string ConsultarRutaComentarios(int Consecutivo)
        {
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
                if (System.IO.File.Exists(ruta))
                {
                    existe = true;
                    extensionArchivo = formato;
                    break;
                }
            }
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

                ruta = Path.Combine(webRootPath, "img/SliderComentarios/") + Consecutivo.ToString() + extensionArchivo;
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
        [HttpGet]
        public IActionResult Carrito()
        {
            return View();
        }
        [HttpGet]
        public IActionResult VerificarCompra()
        {
            //var Categorias = await _BlConsultasExternas.F_GetPaises(1);
            //ViewBag.IdPais = new SelectList(Categorias.Respuesta, "IdDominio", "Descripcion");
            //ViewBag.IdPais = new List<SelectListItem>{new SelectListItem { Value = "0", Text = "Seleccione" },};
            //new SelectListItem { Value = "2", Text = "Íems solo para estudiantes en período de formación" },

            //Response.Headers.Add("Content-Type", "text/html; charset=utf-8");

            return View();
        }
        [HttpPost]
        public async Task<JsonResult> F_GetPaises()
        {

            try
            {
                var Marcas = await _BlConsultasExternas.F_GetPaises(1);

                if (Marcas.Codigo == EstadoOperacion.Bueno)
                {
                    return Json(new { success = true, data = Marcas.Respuesta, message = "" });
                }
                else
                {
                    return Json(new { success = false, data = Marcas.Respuesta, message = "" });
                }

            }


            catch (Exception e)
            {
                var SlidersView = new List<DtoSlider>();
                var SliderView = new DtoSlider
                {
                    Consecutivo = 19957,
                    ContentType = "image/jpeg",
                    FileName = "ARTE4_polired.jpg",
                    Ruta = "/img/Carrusel/1.jpg"
                };
                SlidersView.Add(SliderView);
                return Json(new { success = true, data = SlidersView, message = "" });
            }
        }
        [HttpPost]
        public async Task<JsonResult> F_Departamento(int IdDto)
        {
            try
            {
                var Marcas = await _BlConsultasExternas.F_GetPaisesDepartamento(IdDto);

                if (Marcas.Codigo == EstadoOperacion.Bueno)
                {
                    // var auditoria = IDbSlider.Ins_Auditoria(V_Usuario, "P_GetUsuarios", "Consulta Usuarios del Sistema: ", "", V_Maquina);
                    return Json(new { success = true, data = Marcas.Respuesta, message = "" });
                }
                else
                {
                    return Json(new { success = false, data = Marcas.Respuesta, message = "" });
                }

            }

            catch (Exception e)
            {
                var SlidersView = new List<DtoSlider>();
                var SliderView = new DtoSlider
                {
                    Consecutivo = 19957,
                    ContentType = "image/jpeg",
                    FileName = "ARTE4_polired.jpg",
                    Ruta = "/img/Carrusel/1.jpg"
                };
                SlidersView.Add(SliderView);
                return Json(new { success = true, data = SlidersView, message = "" });
            }
            //Immplementar con microservicio
        }
        [HttpPost]
        public async Task<JsonResult> F_CostoDeEnvio(int IdDto)
        {
            try
            {
                var Marcas = await _BlConsultasExternas.F_CostoDeEnvio(IdDto);

                if (Marcas.Codigo == EstadoOperacion.Bueno)
                {
                    // var auditoria = IDbSlider.Ins_Auditoria(V_Usuario, "P_GetUsuarios", "Consulta Usuarios del Sistema: ", "", V_Maquina);
                    return Json(new { success = true, data = Marcas.Respuesta, message = "" });
                }
                else
                {
                    return Json(new { success = false, data = Marcas.Respuesta, message = "" });
                }

            }


            catch (Exception e)
            {
                var SlidersView = new List<DtoSlider>();
                var SliderView = new DtoSlider
                {
                    Consecutivo = 19957,
                    ContentType = "image/jpeg",
                    FileName = "ARTE4_polired.jpg",
                    Ruta = "/img/Carrusel/1.jpg"
                };
                SlidersView.Add(SliderView);
                return Json(new { success = true, data = SlidersView, message = "" });
            }



            //Immplementar con microservicio
        }
        [HttpPost]
        public async Task<JsonResult> F_CostoDeEnvioDepartamento(int IdDto)
        {
            try
            {
                var Marcas = await _BlConsultasExternas.F_CostoDeEnvioDepartamento(IdDto);

                if (Marcas.Codigo == EstadoOperacion.Bueno)
                {
                    // var auditoria = IDbSlider.Ins_Auditoria(V_Usuario, "P_GetUsuarios", "Consulta Usuarios del Sistema: ", "", V_Maquina);
                    return Json(new { success = true, data = Marcas.Respuesta, message = "" });
                }
                else
                {
                    return Json(new { success = false, data = Marcas.Respuesta, message = "" });
                }

            }


            catch (Exception e)
            {
                var SlidersView = new List<DtoSlider>();
                var SliderView = new DtoSlider
                {
                    Consecutivo = 19957,
                    ContentType = "image/jpeg",
                    FileName = "ARTE4_polired.jpg",
                    Ruta = "/img/Carrusel/1.jpg"
                };
                SlidersView.Add(SliderView);
                return Json(new { success = true, data = SlidersView, message = "" });
            }



            //Immplementar con microservicio
        }
        [HttpPost]
        public async Task<JsonResult> F_CostoDeEnviopais(int IdDto)
        {
            try
            {
                var Marcas = await _BlConsultasExternas.F_CostoDeEnviopais(IdDto);

                if (Marcas.Codigo == EstadoOperacion.Bueno)
                {
                    // var auditoria = IDbSlider.Ins_Auditoria(V_Usuario, "P_GetUsuarios", "Consulta Usuarios del Sistema: ", "", V_Maquina);
                    return Json(new { success = true, data = Marcas.Respuesta, message = "" });
                }
                else
                {
                    return Json(new { success = false, data = Marcas.Respuesta, message = "" });
                }

            }


            catch (Exception e)
            {
                var SlidersView = new List<DtoSlider>();
                var SliderView = new DtoSlider
                {
                    Consecutivo = 19957,
                    ContentType = "image/jpeg",
                    FileName = "ARTE4_polired.jpg",
                    Ruta = "/img/Carrusel/1.jpg"
                };
                SlidersView.Add(SliderView);
                return Json(new { success = true, data = SlidersView, message = "" });
            }



            //Immplementar con microservicio
        }
        [HttpGet]
        public IActionResult Contacto()
        {
            //    string codigo = new Random().Next(1000, 9999).ToString();
            //    string telefono = "573124604321";                

            //    //string objJson = JsonConvert.SerializeObject(product);
            //    string token = "EAAK7vPclLwcBO3VoxXtuZBkDtSbdUrNFIF7QKoNKOHQqajRijngHORBKny9ZBdvhX26LZCfSfAmqaSPT29gK3rxw8q8c6GVEjgnDHW8QuMYZCqZC2HptlwYnUQEfpC6rke9UEkk1eWepI2zzvnFDmtjxzRMo6RqiLBbJLR0c6RPfv2KzX4zy42tkoYkvooYTYmkxdRA7nqH8qb9XuwwoUvIA4y0GM6nAZD";
            //    string idTelefono = "247590451770025";
            //    HttpClient client = new HttpClient();
            //    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://graph.facebook.com/v15.0/" + idTelefono + "/messages");
            //    request.Headers.Add("Authorization", "Bearer " + token);
            //    //request.Content = new StringContent(objJson);
            //    //request.Content = new StringContent("{ \"messaging_product\": \"whatsapp\", \"to\": \"" + telefono + "\", \"type\": \"template\", \"template\": { \"name\": \"validacion_codigo\", \"language\": { \"code\": \"es\" } } }");
            //    request.Content = new StringContent("{\"messaging_product\": \"whatsapp\",\"to\": \"" + telefono + "\",\"type\": \"template\",\"template\": {\"name\": \"validacion_codigo\",\"language\": {\"code\": \"es\"},\"components\": [{\"type\": \"body\",\"parameters\": [{\"type\": \"text\",\"text\": \""+ codigo + "\"},{\"type\": \"text\",\"text\": \""+ codigo + "\"}]}]}}");
            //    request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //    HttpResponseMessage response = await client.SendAsync(request);
            //string responseBody = await response.Content.ReadAsStringAsync();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Contacto22()
        {
            string codigo = new Random().Next(1000, 9999).ToString();
            string telefono = "573124604321";

            //string objJson = JsonConvert.SerializeObject(product);
            string token = "EAAK7vPclLwcBO3VoxXtuZBkDtSbdUrNFIF7QKoNKOHQqajRijngHORBKny9ZBdvhX26LZCfSfAmqaSPT29gK3rxw8q8c6GVEjgnDHW8QuMYZCqZC2HptlwYnUQEfpC6rke9UEkk1eWepI2zzvnFDmtjxzRMo6RqiLBbJLR0c6RPfv2KzX4zy42tkoYkvooYTYmkxdRA7nqH8qb9XuwwoUvIA4y0GM6nAZD";
            string idTelefono = "247590451770025";
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://graph.facebook.com/v15.0/" + idTelefono + "/messages");
            request.Headers.Add("Authorization", "Bearer " + token);
            //request.Content = new StringContent(objJson);
            //request.Content = new StringContent("{ \"messaging_product\": \"whatsapp\", \"to\": \"" + telefono + "\", \"type\": \"template\", \"template\": { \"name\": \"validacion_codigo\", \"language\": { \"code\": \"es\" } } }");
            request.Content = new StringContent("{\"messaging_product\": \"whatsapp\",\"to\": \"" + telefono + "\",\"type\": \"template\",\"template\": {\"name\": \"validacion_codigo\",\"language\": {\"code\": \"es\"},\"components\": [{\"type\": \"body\",\"parameters\": [{\"type\": \"text\",\"text\": \"" + codigo + "\"},{\"type\": \"text\",\"text\": \"" + codigo + "\"}]}]}}");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await client.SendAsync(request);
            string responseBody = await response.Content.ReadAsStringAsync();

            return View();
        }
        [HttpPost]
        public async Task<JsonResult> Contactos(string Nombres, string CorreoElectronico, string NumeroTelefono, string Comentarios, string NumeroTelefonoConIndicativo)
        {
            try
            {
                var insComentarios = new DtoComentariosClientes();
                insComentarios.Nombres = Nombres;
                insComentarios.CorreoElectronico = CorreoElectronico;
                insComentarios.NumeroTelefono = NumeroTelefono;
                insComentarios.Comentarios = Comentarios;
                insComentarios.maquinaCreacion = GetClientIpAddress(HttpContext);
                insComentarios.fechaCreacion = DateTime.Now;
                insComentarios.NumeroTelefonoConIndicativo = NumeroTelefonoConIndicativo;
                insComentarios.EnvioCorreo = "NO";
                insComentarios.Atendio = "NO";

                var Resultado = await _IDbComentarios.Ins_Comentarios(insComentarios);
                if (Resultado > 0)
                {
                    //var re = EnviarCorreoElectronico(CorreoElectronico);
                    return Json(new { success = true, data = 1, mensaje = "" });
                }
                else
                {
                    return Json(new { success = false, data = 1, mensaje = "" });

                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { success = true, data = 1, mensaje = ViewBag.Mensaje });
        }
        [HttpPost]
        public IActionResult SaveComentario()
        {
            // Recupera el mensaje de éxito o error desde TempData
            ViewBag.Mensaje = TempData["Mensajes"] as string;
            ViewBag.Error = TempData["Error"] as string;

            return View();
        }
        [HttpGet]
        public IActionResult SobreNosotros()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> GetSuscribete(string _email)
        {

            try
            {
                if (_email == null || _email == "")
                {
                    return Json(new { success = true, data = 1, message = "" });
                }

                var insComentarios = new DtoComentariosClientes();
                insComentarios.Nombres = "Sin Nombre";
                insComentarios.CorreoElectronico = _email;
                insComentarios.NumeroTelefono = "Sin número de Telefono";
                insComentarios.Comentarios = "Sin Comentarios";
                insComentarios.maquinaCreacion = GetClientIpAddress(HttpContext);
                insComentarios.fechaCreacion = DateTime.Now;
                insComentarios.EnvioCorreo = "NO";
                insComentarios.Atendio = "NO";

                var Resultado = await _IDbComentarios.Ins_Comentarios(insComentarios);


                if (Resultado > 0)
                {
                    var re = EnviarCorreoElectronico(_email);
                    return Json(new { success = true, data = Resultado, message = "" });
                }
                else
                {
                    //TempData["Error"] = $"Error al guardar";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al guardar: {ex.Message}";

            }
            return Json(new { success = true, data = 0, message = "" });
        }
        [HttpPost]
        public int EnviarCorreoElectronico(string _email)
        {
            string remitente = "distrimajas@gmail.com";
            string destinatario = _email;
            string asunto = "Gracias por Suscríbete al Boletín";
            string cuerpo = "Maria Bonita agradece la suscripción, próximamente estarás recibiendo nuestros últimos productos y descuentos";

            var mensaje = new MimeMessage();
            mensaje.From.Add(MailboxAddress.Parse(remitente));
            mensaje.To.Add(MailboxAddress.Parse(destinatario));
            mensaje.Subject = asunto;

            mensaje.Body = new TextPart("plain")
            {
                Text = cuerpo
            };

            using (var clienteSmtp = new MailKit.Net.Smtp.SmtpClient())
            {
                clienteSmtp.Connect("smtp.gmail.com", 587, false);
                clienteSmtp.Authenticate(remitente, "labv fkpe ijli rayx");

                clienteSmtp.Send(mensaje);
                clienteSmtp.Disconnect(true);
            }
            return 1;

            //Console.WriteLine("Correo electrónico enviado correctamente.");
        }
        [AllowAnonymous]
        public async Task<JsonResult> F_GetMensaje()
        {
            var retorno = await _IDbComentarios.F_GetMensaje();
            if (retorno.Estado == true)
            {

                return Json(new { success = true, data = retorno.Respuesta, mensaje = "" });

            }
            else
            {
                return Json(new { success = false, data = 1, mensaje = "" });
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

        [AllowAnonymous]
        //[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> InsCompraProducto(DtoMiCompraRealizada dto)
        {
            try
            {



                //var V_Maquina = GetClientIpAddress(HttpContext);
                var V_Maquina = GetClientIpAddress(HttpContext);
                var V_Usuario = Convert.ToInt64(User.FindFirstValue("Identificacion"));


                var resultado = await _IDbProducto.F_InsCompraProducto(dto);
                if (resultado > 0)
                {
                    //EnviarCorreoElectronicoCompra(dto.Correo_Electronico, dto.Direccion, dto.Opcional_Direccion, dto.Nombres, dto.Apellidos);
                    return Json(new { success = true, data = resultado, message = "" });
                }
                else
                {
                    return Json(new { success = false, data = 0, message = 0 });
                }

            }
            catch (Exception ex)
            {

                throw;
            }



        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> InsProductos(List<DtoMiCarrito> Validar)
        {

            var V_Maquina = GetClientIpAddress(HttpContext);
            var V_Usuario = Convert.ToInt64(User.FindFirstValue("Identificacion"));


            var resultado = await _IDbProducto.F_InsCarrito(Validar);
            if (resultado > 0)
            {
                return Json(new { success = true, data = resultado, message = "" });
            }
            else
            {
                return Json(new { success = false, data = 0, message = 0 });
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> InsProductosMercadoLibre(string preferenceId, int NumeroCompra)
        {

            var V_Maquina = GetClientIpAddress(HttpContext);
            var V_Usuario = Convert.ToInt64(User.FindFirstValue("Identificacion"));


            var resultado = await _IDbProducto.InsProductosMercadoLibre(preferenceId, NumeroCompra);
            if (resultado > 0)
            {
                return Json(new { success = true, data = resultado, message = "" });
            }
            else
            {
                return Json(new { success = false, data = 0, message = 0 });
            }


        }




        [HttpGet]
        public IActionResult MiCompra()
        {
            return View();
        }
        [HttpGet]
        public IActionResult CompraRealizada()
        {
            var url = HttpContext.Request.Path + HttpContext.Request.QueryString;

            // Verificar si la cadena de consulta está presente y no está vacía
            if (!string.IsNullOrEmpty(HttpContext.Request.QueryString.Value))
            {
                // Parsear la cadena de consulta para obtener los parámetros
                var queryString = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(HttpContext.Request.QueryString.Value);

                // Obtener el valor de los parámetros
                if (queryString.TryGetValue("payment_id", out var paymentId))
                {
                    ViewData["payment_id"] = paymentId.ToString();
                }
                else
                {
                    ViewData["payment_id"] = "";
                }

                if (queryString.TryGetValue("status", out var status))
                {
                    ViewData["status"] = status.ToString();
                }
                else
                {
                    ViewData["status"] = "";
                }

                if (queryString.TryGetValue("external_reference", out var externalReference))
                {
                    ViewData["external_reference"] = externalReference.ToString();
                }
                else
                {
                    ViewData["external_reference"] = "";
                }

                if (queryString.TryGetValue("merchant_order_id", out var merchantOrderId))
                {
                    ViewData["merchant_order_id"] = merchantOrderId.ToString();
                }
                else
                {
                    ViewData["merchant_order_id"] = "";
                }
            }
            else
            {
                // Manejar el caso donde no hay cadena de consulta
                ViewData["payment_id"] = "";
                ViewData["status"] = "";
                ViewData["external_reference"] = "";
                ViewData["merchant_order_id"] = "";
            }

            return View();

            //var url = HttpContext.Request.Path + HttpContext.Request.QueryString;

            //// Verificar si la cadena de consulta está presente y no está vacía
            //if (!string.IsNullOrEmpty(HttpContext.Request.QueryString.Value))
            //{
            //    // Parsear la cadena de consulta para obtener los parámetros
            //    var queryString = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(HttpContext.Request.QueryString.Value);

            //    // Obtener el valor del parámetro preference_id

            //    //payment_id:  ID(identificador) del pago de Mercado Pago.
            //    //    status: Status del pago.Por ejemplo: approved para un pago aprobado o pending para un pago pendiente.
            //    //    external_reference: Referencia que puedes sincronizar con tu sistema de pagos.
            //    //    merchant_order_id: ID(identificador) de la orden de pago generada en Mercado Pago.

            //    if (queryString.TryGetValue("payment_id", out var preferenceId))
            //    {
            //        // Aquí puedes hacer lo que necesites con el preferenceId


            //        ViewData["payment_id"] = preferenceId.ToString();
            //    }
            //    else
            //    {
            //        // Manejar el caso donde el parámetro preference_id no está presente
            //        ViewData["payment_id"] = "";
            //    }
            //}
            //else
            //{
            //    // Manejar el caso donde no hay cadena de consulta
            //    ViewData["payment_id"] = "";
            //}

            //return View();
        }

        private string ObtenerPreferenceIdDeUrl(string url)
        {
            var uri = new Uri(url);
            var query = HttpUtility.ParseQueryString(uri.Query);
            return query.Get("preference_id");
        }
        public static class CantidadVentaProductos
        {
            public static string CantidadVentaProducto { get; set; }
        }
        public static class CantidadMensaje
        {
            public static string CantidadMensajes { get; set; }
        }
        //MercadoPago de Pago
        public async Task<JsonResult> CreatePreference()
        {
            try
            {
                // Leer los datos del cuerpo de la solicitud
                using (var reader = new System.IO.StreamReader(Request.Body))
                {
                    var requestBody = await reader.ReadToEndAsync();
                    dynamic requestData = JObject.Parse(requestBody);

                    // Verificar si los datos 'items' y 'payer' están presentes
                    if (requestData.items != null && requestData.payer != null)
                    {
                        var itemsData = requestData.items;
                        var payerData = requestData.payer;

                        // Verificar si 'items' es un array
                        if (itemsData is JArray)
                        {
                            var items = new List<object>();

                            // Iterar sobre cada elemento del array "items"
                            foreach (var item in itemsData)
                            {
                                int quantity = item.quantity;
                                string description = item.description;
                                decimal price = item.price;

                                // Agregar cada item a la lista de items
                                items.Add(new
                                {
                                    title = description,
                                    quantity = quantity,
                                    unit_price = price,
                                });
                            }

                            // Crear la preferencia con los items y los datos del comprador
                            var preference = new
                            {
                                items = items,
                                payer = new
                                {
                                    name = payerData.name,
                                    email = payerData.email,
                                    address = new
                                    {
                                        // Verifica si las propiedades están disponibles
                                        city = payerData.address.city,
                                        country = payerData.address.country // Si está presente
                                    },
                                    phone = new
                                    {
                                        area_code = payerData.phone.area_code, // Si está presente
                                        number = payerData.phone.number
                                    }
                                },
                                back_urls = new
                                {
                                    success = "https://distribuidoramajas.com/Cuenta/CompraRealizada",
                                    failure = "https://distribuidoramajas.com/Cuenta/VerificarCompra",
                                    pending = "https://distribuidoramajas.com/Cuenta/VerificarCompra"
                                },
                                auto_return = "approved"
                            };

                            // Hacer la solicitud HTTP a la API de Mercado Pago
                            using (var client = new HttpClient())
                            {
                                // Agregar el token de autenticación en las cabeceras
                                var accessToken = _configuration.GetSection("AppSettings").GetValue<string>("AccessToken");
                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                                // Enviar la solicitud POST a la API de Mercado Pago para crear la preferencia
                                var response = await client.PostAsJsonAsync("https://api.mercadopago.com/checkout/preferences", preference);

                                // Verificar si la respuesta fue exitosa
                                if (response.IsSuccessStatusCode)
                                {
                                    // Usamos ReadFromJsonAsync<T> para leer y deserializar directamente el JSON.
                                    var responseBody = await response.Content.ReadFromJsonAsync<PreferenceResponse>();
                                    return Json(new { id = responseBody.Id }); // Retorna el preferenceId
                                }
                                else
                                {
                                    // Si hay error en la solicitud, leer el contenido del error
                                    var errorContent = await response.Content.ReadAsStringAsync();
                                    return Json(new { error = "Error al crear la preferencia: " + errorContent });
                                }
                            }
                        }
                        else
                        {
                            return Json(new { error = "'items' debe ser un array." });
                        }
                    }
                    else
                    {
                        return Json(new { error = "'items' o 'payer' no están presentes en los datos recibidos." });
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                // Manejo específico de errores HTTP
                return Json(new { error = "Error en la solicitud HTTP: " + ex.Message });
            }
            catch (Exception ex)
            {
                // Manejo genérico de otros errores
                return Json(new { error = "Error: " + ex.Message });
            }
        }




        public class PaymentResponse
        {
            public string Status { get; set; } // Puede ser "approved", "rejected", etc.
            public string StatusDetail { get; set; } // Detalle sobre el estado del pago
            public string Id { get; set; } // ID de la transacción
            public string PayerEmail { get; set; } // Correo del pagador, por ejemplo
            public DateTime DateCreated { get; set; } // Fecha de creación del pago
        }


        [AllowAnonymous]
        public async Task<JsonResult> F_GetProductosMisComprasId(Int32 idCompra, string preferenceId, string Idstatus, string Idexternal_reference, string Idmerchant_order_id)
        {
            var resultado = 0;
            if (preferenceId != null || preferenceId != "")
            {
                if (idCompra > 0)
                {
                    resultado = await _IDbProducto.F_InsPreferenceId(idCompra, preferenceId, Idstatus, Idexternal_reference, Idmerchant_order_id);
                }
                if (resultado > 0)
                {

                }
            }
            var result = await _IDbProducto.F_GetProductosMisComprasId(idCompra);

            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    //EnviarCorreoElectronicoCompra(item.Correo_Electronico, item.Direccion, item.Opcional_Direccion, item.Nombres, item.Apellidos, item.Cantidad, Convert.ToDecimal(item.CostoEnvio), item.PrecioTotal, Convert.ToDecimal(item.PrecioFinal), item.preferenceId, item.merchant_order_id, item.status, item.ListaMisCompras);

                }

                return Json(new { success = true, data = result.OrderByDescending(x => x.id_compra_realizada) });
            }
            else
            {
                return Json(new { success = false, data = result });
            }
        }


        [AllowAnonymous]
        public async Task<JsonResult> F_GetEnviarCorreoId(Int32 idCompra)
        {



            var result = await _IDbProducto.F_GetProductosMisComprasId(idCompra);

            if (result.Count > 0)
            {
                foreach (var item in result)
                {
                    EnviarCorreoElectronicoCompra(item.Correo_Electronico, item.Direccion, item.Opcional_Direccion, item.Nombres, item.Apellidos, item.Cantidad, Convert.ToDecimal(item.CostoEnvio), item.PrecioTotal, Convert.ToDecimal(item.PrecioFinal), item.preferenceId, item.merchant_order_id, item.status, item.ListaMisCompras);

                }

                return Json(new { success = true, data = result.OrderByDescending(x => x.id_compra_realizada) });
            }
            else
            {
                return Json(new { success = false, data = result });
            }
        }
        [HttpPost]
        public int EnviarCorreoElectronicoCompra(string _email, string _direccion, string _direccioOpcional, string _nombres, string _apellidos, string Cantidad, decimal CostoEnvio, string PrecioTotal, decimal PrecioFinal, string preferenceId, string merchant_order_id, string status, List<DtoMiCarrito> listaProductos)
        {
            string remitente = "distrimajas@gmail.com";
            string destinatario = _email;
            string asunto = "Gracias por realizar su compra";

            // Construye el cuerpo del correo electrónico
            string cuerpo = $"Señor(a) {_nombres} {_apellidos},\r\n\r\nMaria Bonita agradece por su compra.\r\n\r\nAquí está la lista de productos adquiridos:\r\n\r\n";

            foreach (var producto in listaProductos)
            {
                cuerpo += $"- {producto.Producto} - {producto.Descripcion_Producto} Precio Unitario: {producto.precio_Unitario}\r\n";
            }

            cuerpo += "\r\nDetalles de la compra:\r\n";
            cuerpo += $"Cantidad: {Cantidad}\r\n";
            cuerpo += $"Costo de Envío: {CostoEnvio}\r\n";
            cuerpo += $"Precio Total: {PrecioTotal}\r\n";
            cuerpo += $"Precio Final: {PrecioFinal}\r\n";
            cuerpo += $"Preference ID Mercado Pago: {preferenceId}\r\n";
            cuerpo += $"Order Mercado Pago ID: {merchant_order_id}\r\n";
            cuerpo += $"Estado Producto: {status}\r\n\r\n";

            cuerpo += $"Dirección de envío: {_direccion}\r\n";
            cuerpo += $"Dirección opcional: {_direccioOpcional}\r\n\r\n";

            cuerpo += $"Puede seguir el estado de su pedido Ingresando a la página web en la siguiente URL: https://distribuidoramajas.com/Cuenta/MiCompra ingresa su correo electrónico le clic en el boton buscar y listo, o si desea en mi carrito de compras le das clic el botón mis compras realizadas ingresas su correo electrónico ...\r\n\r\n";
            cuerpo += $"Para consultas adicionales, comuníquese con nuestros asesores al número telefónico 3132219524.\r\n\r\n";
            cuerpo += $"Gracias por preferirnos.\r\n\r\nAtentamente,\r\nMAJAS\r\nDistribuidora de Productos de Belleza .";

            var mensaje = new MimeMessage();
            mensaje.From.Add(MailboxAddress.Parse(remitente));
            mensaje.To.Add(MailboxAddress.Parse(destinatario));
            mensaje.Subject = asunto;

            mensaje.Body = new TextPart("plain")
            {
                Text = cuerpo
            };

            using (var clienteSmtp = new MailKit.Net.Smtp.SmtpClient())
            {
                clienteSmtp.Connect("smtp.gmail.com", 587, false);
                clienteSmtp.Authenticate(remitente, "labv fkpe ijli rayx");

                clienteSmtp.Send(mensaje);
                clienteSmtp.Disconnect(true);
            }
            return 1;
        }



        //[HttpPost]
        //public int EnviarCorreoElectronicoCompra(string _email, string _direccion, string _direccioOpcional, string _nombres, string _apellidos, string Cantidad, decimal CostoEnvio, string PrecioTotal, decimal PrecioFinal, string preferenceId, string merchant_order_id, string status)
        //{
        //    string remitente = "distrimajas@gmail.com";
        //    string destinatario = _email;
        //    string asunto = "Gracias por realizar su compra";
        //    string cuerpo = "señor(a)\r\n" + _nombres + " " + _apellidos + "\r\n\r\nMaria Bonita agradece por su compra, se envía el paso a paso con el fin de que usted puede realizar seguimiento a su pedido en la siguiente URL https://DistribuidoraMajas.com/Cuenta/MiCompra, anexa su correo electrónico y listo, o si desea Ingrese a la página web y en mi carrito de compras le das clic el botón  mis compras realizadas ingresas su correo electrónico y listo, de igual manera se puede comunicar de forma directa con nuestros asesores al número telefónico 3132219524, gracias por preferirnos.\r\n\r\nDirección de Envío:  " + _direccion + " " + _direccioOpcional + " -9584\r\n\r\n\r\nAtentamente,\r\n\r\n\r\n\r\nMAJAS\r\nDistribuidora de Productos de Belleza .";


        //    // Código existente para construir el cuerpo del correo electrónico
        //    //string cuerpo = "señor(a)\r\n" + _nombres + " " + _apellidos + "\r\n\r\nMaria Bonita agradece por su compra. Aquí está la lista de productos:\r\n\r\n";


        //    var mensaje = new MimeMessage();
        //    mensaje.From.Add(MailboxAddress.Parse(remitente));
        //    mensaje.To.Add(MailboxAddress.Parse(destinatario));
        //    mensaje.Subject = asunto;

        //    mensaje.Body = new TextPart("plain")
        //    {
        //        Text = cuerpo
        //    };

        //    using (var clienteSmtp = new SmtpClient())
        //    {
        //        clienteSmtp.Connect("smtp.gmail.com", 587, false);
        //        clienteSmtp.Authenticate(remitente, "labv fkpe ijli rayx");

        //        clienteSmtp.Send(mensaje);
        //        clienteSmtp.Disconnect(true);
        //    }
        //    return 1;

        //}


        [HttpGet]
        public IActionResult GetPublicKey()
        {
            // Obtener la clave pública de MercadoPago desde la configuración

            var publicKeys = _configuration.GetSection("AppSettings").GetValue<string>("MercadoPagoPublicKey");

            var publicKey = _configuration.GetValue<string>("AppSettings:MercadoPagoPublicKey");

            if (string.IsNullOrEmpty(publicKey))
            {
                return BadRequest("La clave pública no está configurada.");
            }

            // Devolver la clave pública al frontend
            return Ok(new { publicKey });
        }

        public IActionResult CartaMariaBonita(string? returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            return View();
        }









    }
}

