using Comun.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoEcommers.Models
{
    public interface IHttpCliente
    {
        Task<RespuestaDto<TResult>> HttpPost<TParam, TResult>(TParam _param, string _url, string _token);
        Task<RespuestaDto<TResult>> HttpPost<TParam, TResult>(TParam _param, string _url);
        Task<RespuestaDto<TResult>> HttpPost<TResult>(string _url, string _token);
        Task<RespuestaDto<TResult>> HttpGet<TResult>(string _param, string _token);
        Task<RespuestaDto<TResult>> HttpDelete<TResult>(string url, string _token);
    }
}
