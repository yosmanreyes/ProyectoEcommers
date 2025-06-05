using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Negocio.Contratos.ConsultasExternas;
using ProyectoEcommers.Models;

namespace ProyectoEcommers.Controllers;

public class HomeController : Controller
{
    //private readonly ILogger<HomeController> _logger;

    //public HomeController(ILogger<HomeController> logger)
    //{
    //    _logger = logger;
    //}

    public IActionResult Index()
    {
        return View();
    }

    //public IActionResult Privacy()
    //{
    //    return View();
    //}

    //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //public IActionResult Error()
    //{
    //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    //}
    //public IActionResult Commers()
    //{
    //    return View();
    //}

    private readonly IBLConsultasExternas _BLConsultasExternas;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public HomeController(IBLConsultasExternas bLConsultasExternas, IWebHostEnvironment webHostEnvironment)
    {
        _BLConsultasExternas = bLConsultasExternas;
        _webHostEnvironment = webHostEnvironment;
    }
    public async Task<IActionResult> Commers()
    {
        try
        {
            var ImagenesSlider = await _BLConsultasExternas.F_GetCarruselImagenesAsyc();

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
    }
    public IActionResult Perfil()
    {
        return View();
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
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

}
