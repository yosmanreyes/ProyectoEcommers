

namespace ProyectoEcommers.Controllers
{

    using Comun.Enumeraciones;
    using MailKit.Net.Smtp;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using MimeKit;

    using Negocio.Contratos.ConsultasExternas;
    using ProyectoEcommers.Helper;
    using ProyectoEcommers.Models;
    using System.Data;
    using System.Security.Claims;
    using OfficeOpenXml;

    using static ProyectoEcommers.Controllers.CuentaController;
 

    [Authorize(Roles = "1,2,3,4")]
    public class AdminComentariosController : Controller
    {
        private readonly IDbProducto _IDbProducto;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDbAdministracion _dbAdministracion;
        private readonly IBLConsultasExternas _BlConsultasExternas;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IDbComentarios _IDbComentarios;

        public AdminComentariosController(IDbProducto iDbProducto, IHttpContextAccessor httpContextAccessor, IDbAdministracion dbAdministracion, IBLConsultasExternas bLConsultasExternas, IWebHostEnvironment webHostEnvironment, IDbComentarios dbComentarios)
        {
            _IDbProducto = iDbProducto;
            _httpContextAccessor = httpContextAccessor;
            _dbAdministracion = dbAdministracion;
            _BlConsultasExternas = bLConsultasExternas;
            _webHostEnvironment = webHostEnvironment;
            _IDbComentarios = dbComentarios;
        }
        [HttpGet]
        public IActionResult AdminComentarios()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AdminEnvioCorreos()
        {
            return View();
        }
        [AllowAnonymous]
        public async Task<JsonResult> F_GetComentarios()
        {
            var result = await _IDbProducto.F_GetComentarios();

            if (result.Count > 0)
            {
              var retorno =  F_totalComentarios();
                var retorno2 = F_totalVentasPorEnviar();
                return Json(new { success = true, data = result.OrderByDescending(x => x.fechaCreacion) });
            }
            else
            {
                return Json(new { success = false, data = result });
            }
        }
        [AllowAnonymous]
        public async Task<JsonResult> F_GetConsultaCorreos()
        {
            var result = await _IDbProducto.F_GetConsultaCorreos();

            if (result.Count > 0)
            {
                return Json(new { success = true, data = result.OrderByDescending(x => x.fechaCreacion) });
            }
            else
            {
                return Json(new { success = false, data = result });
            }
        }
        [HttpPost]
        public async Task<JsonResult> ConsultaDominiosComentarios()
        {
            try
            {
                var Roles = await _dbAdministracion.GetCorreosComentarios();
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
        public async Task<JsonResult> ConsultaDominiosBandejaVentas()
        {
            try
            {
                var Roles = await _dbAdministracion.GetCorreosBandejaVentas();
                if (Roles.Codigo == EstadoOperacion.Bueno)
                {
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
        public async Task<JsonResult> ConsultaDominiosCorreosGeneral()
        {
            try
            {
                var Roles = await _dbAdministracion.GetCorreos();
                if (Roles.Codigo == EstadoOperacion.Bueno)
                {
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
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> EnvioCorreoPersonal(Int32 _IdComentario, string _Correo, string _Asumnto, string _Mensaje)
        {
            try
            {
                var insComentarios = new DtoEnvioCorreo();
                insComentarios.IdComentario = _IdComentario;
                insComentarios.CorreoEnviar = _Correo;
                insComentarios.Mensaje = _Mensaje;
                insComentarios.Asunto = _Asumnto;
                insComentarios.MaquinaCreacion = GetClientIpAddress(HttpContext);
                insComentarios.fechaCreacion = DateTime.Now;
                var Resultado = await _IDbComentarios.Ins_ComentarioEnvioCorreo(insComentarios);
                if (Resultado > 0)
                {
                   
                    var retorno = await _IDbProducto.F_UpdateEnvioCorreo(_IdComentario);
               
                    var re = EnviarCorreoElectronico(_Correo, _Asumnto, _Mensaje);
                    return Json(new { success = true, data = 1, mensaje = "Se realizo el envío de correo satisfactoriamente" });
                }
                else
                {
                    return Json(new { success = false, data = 1, mensaje = "No es posible enviar intente mas tarde" });
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { success = true, data = 1, mensaje = "" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> EnvioCorreoPersonalUnion(Int32 _IdComentario, string _Correo, string _Asumnto, string _Mensaje, Int32 _idComentario2, Int32 _idEnvio)
        {
            try
            {
                var insComentarios = new DtoEnvioCorreo();
                insComentarios.IdComentario = _IdComentario;
                insComentarios.CorreoEnviar = _Correo;
                insComentarios.Mensaje = _Mensaje;
                insComentarios.Asunto = _Asumnto;
                insComentarios.MaquinaCreacion = GetClientIpAddress(HttpContext);
                insComentarios.fechaCreacion = DateTime.Now;
                var Resultado = await _IDbComentarios.Ins_ComentarioEnvioCorreo(insComentarios);
                if (Resultado > 0)
                {
                    if (_idEnvio == 1)
                    {
                        var retorno = await _IDbProducto.F_UpdateEnvioCorreoBandeja(_idComentario2);
                       

                    }
                    else {
                        var retorno = await _IDbProducto.F_UpdateEnvioCorreo(_idComentario2);
                    }
                
                    var re = EnviarCorreoElectronico(_Correo, _Asumnto, _Mensaje);
                    return Json(new { success = true, data = 1, mensaje = "Se realizo el envío de correo satisfactoriamente" });
                }
                else
                {
                    return Json(new { success = false, data = 1, mensaje = "No es posible enviar intente mas tarde" });
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { success = true, data = 1, mensaje = "" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> EnvioCorreoPersonalBandeja(Int32 _IdComentario, string _Correo, string _Asumnto, string _Mensaje)
        {
            try
            {
                var insComentarios = new DtoEnvioCorreo();
                insComentarios.IdComentario = _IdComentario;
                insComentarios.CorreoEnviar = _Correo;
                insComentarios.Mensaje = _Mensaje;
                insComentarios.Asunto = _Asumnto;
                insComentarios.MaquinaCreacion = GetClientIpAddress(HttpContext);
                insComentarios.fechaCreacion = DateTime.Now;
                var Resultado = await _IDbComentarios.Ins_ComentarioEnvioCorreo(insComentarios);
                if (Resultado > 0)
                {
                    var retorno = await _IDbProducto.F_UpdateEnvioCorreoBandeja(_IdComentario);
                    var re = EnviarCorreoElectronico(_Correo, _Asumnto, _Mensaje);
                    return Json(new { success = true, data = 1, mensaje = "Se realizo el envío de correo satisfactoriamente" });
                }
                else
                {
                    return Json(new { success = false, data = 1, mensaje = "No es posible enviar intente mas tarde" });
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { success = true, data = 1, mensaje = "" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> EnvioCorreoMasivo(List<DtoEnvioCorreo> obj)
        {
            try
            {
                var Resultado = 0;
                foreach (var item in obj)
                {
                    var insComentarios = new DtoEnvioCorreo();
                    insComentarios.IdComentario = 0;
                    insComentarios.CorreoEnviar = item.CorreoEnviar;
                    insComentarios.Mensaje = item.Mensaje;
                    insComentarios.Asunto = item.Asunto;
                    insComentarios.MaquinaCreacion = GetClientIpAddress(HttpContext);
                    insComentarios.fechaCreacion = DateTime.Now;

                    Resultado = await _IDbComentarios.Ins_ComentarioEnvioCorreo(insComentarios);
                    if (Resultado > 0)
                    {                     
                        var re = EnviarCorreoElectronico(item.CorreoEnviar, item.Asunto, item.Mensaje);
                    }
                    else
                    {
                        return Json(new { success = false, data = 1, mensaje = "No es posible enviar el correo electrónico intente mas tarde..." });
                    }
                }
                if (Resultado > 0)
                {
                    return Json(new { success = true, data = 1, mensaje = "Se realizo el envío de correo satisfactoriamente..." });
                }
                else
                {
                    return Json(new { success = false, data = 1, mensaje = "No es posible enviar el correo electrónico intente mas tarde..." });

                }
            }
            catch (Exception ex)
            {
            }
            return Json(new { success = true, data = 1, mensaje = "" });

        }
        public int EnviarCorreoElectronico(string _email, string _asunto, string _mensaje)
        {
            string remitente = "grupolasantaclub@gmail.com";
            string destinatario = _email;
            string asunto = _asunto;
            string cuerpo = _mensaje;

            var mensaje = new MimeMessage();
            mensaje.From.Add(MailboxAddress.Parse(remitente));
            mensaje.To.Add(MailboxAddress.Parse(destinatario));
            mensaje.Subject = asunto;

            mensaje.Body = new TextPart("plain")
            {
                Text = cuerpo
            };

            using (var clienteSmtp = new SmtpClient())
            {
                clienteSmtp.Connect("smtp.gmail.com", 587, false);
                clienteSmtp.Authenticate(remitente, "labv fkpe ijli rayx");

                clienteSmtp.Send(mensaje);
                clienteSmtp.Disconnect(true);
            }
            return 1;
        }
        #region Reportes Excel
        [HttpGet]
        public async Task<FileContentResult> ExcelGeneral()
        {
            try
            {
                var clave = User.FindFirstValue("Clave");
                var retorno = await _IDbProducto.F_GetComentarios();
                string[] columns = { "idComentario", "Nombres", "CorreoElectronico", "NumeroTelefono", "Comentarios", "vigente", "fechaCreacion", "maquinaCreacion" };

                byte[] filecontent = ExcelExportHelperPass.ExportExcel(retorno.ToList(), "Listado General Comentarios", Convert.ToString(clave), true, columns);

                return File(filecontent, ExcelExportHelper.ExcelContentType, "ExportToExcelTotalComentarios.xlsx");
            }
            catch (Exception)
            {
                throw;
            }
        }
        //Excel con contraseña
        [HttpGet]
        public async Task<FileContentResult> ExcelGeneralCorreos()
        {
            try
            {
                var clave = User.FindFirstValue("Clave");
                var retorno = await _IDbProducto.F_GetConsultaCorreos();
                string[] columns = { "idComentario", "Nombres", "NumeroTelefono", "CorreoElectronico" };

                byte[] filecontent = ExcelExportHelperPass.ExportExcel(retorno.ToList(), "Listado General Correos", Convert.ToString(clave), true, columns);

                return File(filecontent, ExcelExportHelper.ExcelContentType, "ExportExcelCorreos.xlsx");
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
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
        public async Task<JsonResult> UpdateComentarios(DtoComentariosClientes dto)
        {
            var V_Maquina = GetClientIpAddress(HttpContext);
            var V_Usuario = Convert.ToInt32(User.FindFirstValue("Identificacion"));
            var retorno = await _IDbProducto.F_UpdateComentarios(dto);
            if (retorno.Estado)            {
              
                CantidadMensaje.CantidadMensajes = Convert.ToString(retorno.Id);
                //var auditoria = _DbAdministracion.Ins_Auditoria(V_Usuario, "InsUsuarios", "Cambio estado de usuario a: " + dto.Bloqueado, Convert.ToString(dto.Identificacion), V_Maquina);
                return Json(new { success = true, data = retorno.Respuesta, message = retorno.Mensaje = "Se realiza el registro exitosamente" });
            }
            else
            {
                return Json(new { success = false, data = retorno.Respuesta, message = retorno.Mensaje = "No es posible Guardar" });
            }
        }
        [HttpPost]
        public async Task<int> F_totalComentarios()
        {
            var retorno = await _IDbProducto.F_totalComentarios();
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
        //Excel sin contraseña
        //public async Task<FileContentResult> ExcelGeneral()
        //{

        //    var retorno = await _IDbProducto.F_GetComentarios();
        //    string[] columns = { "idComentario", "Nombres", "CorreoElectronico", "NumeroTelefono", "Comentarios", "vigente", "fechaCreacion", "maquinaCreacion" };
        //    byte[] filecontent;

        //    try
        //    {
        //        if (retorno.Count > 0)
        //        {
        //            filecontent = ExcelExportHelper.ExportExcel(retorno, "Reporte General Comentarios", true, columns);
        //            return File(filecontent, ExcelExportHelper.ExcelContentType, "ReporteComentarios.xlsx");
        //        }

        //        filecontent = ExcelExportHelper.ExportExcel(new List<DtoEnvioCorreo>(), "Reporte sin información", true, columns);
        //        return File(filecontent, ExcelExportHelper.ExcelContentType, "ReporteComentarios.xlsx");

        //    }
        //    catch (Exception e)
        //    {
        //        filecontent = ExcelExportHelper.ExportExcel(new List<DtoEnvioCorreo>(), "Reporte sin información", true, columns);
        //        return File(filecontent, ExcelExportHelper.ExcelContentType, "ReporteComentarios.xlsx");
        //    }
        //}

        //Excel con contraseña

    }
}
