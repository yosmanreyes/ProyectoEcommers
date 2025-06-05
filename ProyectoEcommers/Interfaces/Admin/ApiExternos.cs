
using Comun.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoEcommers.Models
{
    public class ApiExternos: IApiExternos
    {
        private readonly ApiGatewayUrl _apiGatewayUrl;

        public ApiExternos(ApiGatewayUrl apiGatewayUrl)
        {
            _apiGatewayUrl = apiGatewayUrl;
        }

        #region Metodos PIP Plataforma 
      
        public async Task<RespuestaDto<List<DtoSlider>>> F_GetCarruselImagenesAsyc(string token)
        {
            IHttpCliente http = new ClienteHttp();
            var contenido = await http.HttpGet<List<DtoSlider>>($"{_apiGatewayUrl.PipBaseUrl}{_apiGatewayUrl.CarruselImagenes}", token);

            return contenido!;
        }

        public async Task<RespuestaDto<bool>> ObtenerOudSeviciosAsync(DtoCredenciales _credenciales, string token)
        {
            IHttpCliente http = new ClienteHttp();
            var contenido = await http.HttpPost<DtoCredenciales, bool>(_credenciales, $"{_apiGatewayUrl.PipBaseUrl}{_apiGatewayUrl.Oud}", token);
            return contenido;
        }

        public async Task<RespuestaDto<string>> ObtenerTokenPIPAsync(DtoUsuarioPip _usuarioPip)
        {
            IHttpCliente http = new ClienteHttp();
            var contenido = await http.HttpPost<DtoUsuarioPip, string>(_usuarioPip, _apiGatewayUrl.PostPipToken);
            return contenido!;
        }

        #endregion
    }
}
