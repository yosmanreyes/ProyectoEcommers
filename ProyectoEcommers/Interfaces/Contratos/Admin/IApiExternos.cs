
using Comun.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoEcommers.Models
{
    public interface IApiExternos
    {
        public Task<RespuestaDto<List<DtoSlider>>> F_GetCarruselImagenesAsyc(string token);
        public Task<RespuestaDto<bool>> ObtenerOudSeviciosAsync(DtoCredenciales _credenciales, string token);
        public Task<RespuestaDto<string>> ObtenerTokenPIPAsync(DtoUsuarioPip _usuarioPip);

    }
}
