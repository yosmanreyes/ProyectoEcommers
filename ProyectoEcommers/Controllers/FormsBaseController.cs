using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Negocio.Contratos.ConsultasExternas;
using System.Data;
using System.Security.Claims;

namespace ProyectoEcommers.Controllers
{

    [Authorize(Roles = "1,2,3,4")]
    public class FormsBaseController : Controller
    {
        #region Propiedades
        private readonly IBLConsultasExternas _bLConsultasExternas;
        #endregion
        public FormsBaseController(IBLConsultasExternas bLConsultasExternas)
        {
            _bLConsultasExternas = bLConsultasExternas;
        }
        public IActionResult Formularios()
        {
            var Funcionario = User.FindFirstValue("Funcionario");
            var IpMaquina = GetClientIpAddress(HttpContext);

            var roles = ((ClaimsIdentity)User.Identity).Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);

            return View();
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

        public async Task<IActionResult> F_GetDatos(Int64 V_Identificacion)
        {
            var retorno = await _bLConsultasExternas.F_GetCarruselImagenesAsyc();

            if (retorno.Estado)
            {
                return Json(new { success = true, data = retorno.Respuesta });
            }
            else
            {
                return Json(new { success = false, data = retorno.Respuesta });
            }


        }
    }
}

