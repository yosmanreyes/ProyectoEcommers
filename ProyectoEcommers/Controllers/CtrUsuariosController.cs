

namespace ProyectoEcommers.Controllers
{


    using Comun.Enumeraciones;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Negocio.Contratos.ConsultasExternas;
    using ProyectoEcommers.Models;
    using System.Security.Claims;
    using static ProyectoEcommers.Controllers.CuentaController;


    [Authorize(Roles = "1,2,3,4")]
    public class CtrUsuariosController : Controller
    {

        private readonly IDbAdministracion _dbAdministracion;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IBLConsultasExternas _BlConsultasExternas;

        public CtrUsuariosController(IHttpContextAccessor httpContextAccessor, IDbAdministracion dbAdministracion, IWebHostEnvironment webHostEnvironment, IBLConsultasExternas bLConsultasExternas)
        {

            _httpContextAccessor = httpContextAccessor;
            _dbAdministracion = dbAdministracion;
            _webHostEnvironment = webHostEnvironment;
            _BlConsultasExternas = bLConsultasExternas;
        }
        [HttpGet]
        public IActionResult Usuarios()
        {

            return View();
        }
        [HttpPost]
        public async Task<JsonResult> ConsultaDominios()
        {
            try
            {
                var Roles = await _dbAdministracion.GetRoles();
                var Cliente = await _BlConsultasExternas.F_GetDominios(42);
                if (Roles.Codigo == EstadoOperacion.Bueno)
                {
                    // var auditoria = IDbSlider.Ins_Auditoria(V_Usuario, "P_GetUsuarios", "Consulta Usuarios del Sistema: ", "", V_Maquina);
                    return Json(new { success = true, data = Roles.Respuesta, data1 = Cliente.Respuesta, message = "" });
                }
                else
                {
                    return Json(new { success = false, data = Roles.Respuesta, data1 = Cliente.Respuesta, message = "" });
                }

            }
            catch (Exception e)
            {
                return Json(new { success = false, data = 0, data1 = 0, message = "" });
            }

        }
        [HttpPost]
        public async Task<JsonResult> F_GetTarjetaEmpleado(Int32 _Identifcacion)
        {
            try
            {
                var Roles = await _dbAdministracion.GetUsuario(_Identifcacion);
                if (Roles.Codigo == EstadoOperacion.Bueno)
                {
                    // var auditoria = IDbSlider.Ins_Auditoria(V_Usuario, "P_GetUsuarios", "Consulta Usuarios del Sistema: ", "", V_Maquina);
                    return Json(new { success = true, data = Roles.Respuesta, message = "" });
                }
                else
                {
                    return Json(new { success = false, data = Roles.Respuesta, message = "" });
                }

            }
            catch (Exception e)
            {
                return Json(new { success = false, data = 0, data1 = 0, message = "" });
            }


        }
        [HttpPost]
        public async Task<JsonResult> F_GetEmpleadoGrilla(Int32 _Identifcacion)
        {
            try
            {
                var Roles = await _dbAdministracion.GetUsuarioGrilla(_Identifcacion);
                if (Roles.Codigo == EstadoOperacion.Bueno)
                {
                    // var auditoria = IDbSlider.Ins_Auditoria(V_Usuario, "P_GetUsuarios", "Consulta Usuarios del Sistema: ", "", V_Maquina);
                    return Json(new { success = true, data = Roles.Respuesta, message = "" });
                }
                else
                {
                    return Json(new { success = false, data = Roles.Respuesta, message = "" });
                }

            }
            catch (Exception e)
            {
                return Json(new { success = false, data = 0, data1 = 0, message = "" });
            }

        }
        [HttpPost]
        public async Task<IActionResult> InsDesactivarUsuario(DtoUserRoles dto)
        {

            var V_Maquina = GetClientIpAddress(HttpContext);
            var V_Usuario = Convert.ToInt64(User.FindFirstValue("Identificacion"));


            dto.Maquina = V_Maquina;
            dto.Usuario = V_Usuario;

            var retorno = await _dbAdministracion.InsDesactivarUsuario(dto);
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
        public async Task<IActionResult> InsUsuario(DtoUsuario dto)
        {

            var V_Maquina = GetClientIpAddress(HttpContext);
            var V_Usuario = Convert.ToString(User.FindFirstValue("Identificacion"));


            dto.Maquina = V_Maquina;
            dto.Usuario = V_Usuario;

            var retorno = await _dbAdministracion.InsUsuario(dto);
            if (retorno > 0)
            {
                //var auditoria = _DbAdministracion.Ins_Auditoria(V_Usuario, "InsUsuarios", "Cambio estado de usuario a: " + dto.Bloqueado, Convert.ToString(dto.Identificacion), V_Maquina);
                return Json(new { success = true, data = retorno, message = "Se realizo el registro exitosamente" });
            }
            else if (retorno == -1)
            {
                return Json(new { success = true, data = retorno, message = "Usted ya cuenta con un rol registrado, por favor activar o desactivar si es necesario, Gracias" });
            }
            else if (retorno == -2)
            {
                return Json(new { success = true, data = retorno, message = "Se actualizó el rol exitosamente" });
            }
            else
            {
                return Json(new { success = false, data = retorno, message = "No es posible Guardar" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdUsuario(DtoUsuario dto)
        {

            var V_Maquina = GetClientIpAddress(HttpContext);
            var V_Usuario = Convert.ToString(User.FindFirstValue("Identificacion"));


            dto.Maquina = V_Maquina;
            dto.Usuario = V_Usuario;

            var retorno = await _dbAdministracion.UpdUsuario(dto);
            if (retorno > 0)
            {
                //var auditoria = _DbAdministracion.Ins_Auditoria(V_Usuario, "InsUsuarios", "Cambio estado de usuario a: " + dto.Bloqueado, Convert.ToString(dto.Identificacion), V_Maquina);
                return Json(new { success = true, data = retorno, message = "Se realizo el registro exitosamente" });
            }
            else if (retorno == -1)
            {
                return Json(new { success = true, data = retorno, message = "Usted ya cuenta con un rol registrado, por favor activar o desactivar si es necesario, Gracias" });
            }
            else if (retorno == -2)
            {
                return Json(new { success = true, data = retorno, message = "Se actualizó el rol exitosamente" });
            }
            else
            {
                return Json(new { success = false, data = retorno, message = "No es posible Guardar" });
            }
        }

    }
}
