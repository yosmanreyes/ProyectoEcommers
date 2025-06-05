

namespace Negocio.Contratos.ConsultasExternas
{


    using Comun.Dto;
    using ProyectoEcommers.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IBLConsultasExternas
    {

        public Task<RespuestaDto<List<DtoDominios>>> F_GetDominios(Int32 _numerico);
        public Task<RespuestaDto<List<DtoDominios>>> F_GetPaises(Int32 _numerico);
        public Task<RespuestaDto<List<DtoDominios>>> F_GetPaisesDepartamento(Int32 _numerico);
        public Task<RespuestaDto<List<DtoDominios>>> F_CostoDeEnvio(Int32 _numerico);
        public Task<RespuestaDto<DtoDominios>> F_CostoDeEnvioDepartamento(Int32 _numerico);
        public Task<RespuestaDto<DtoDominios>> F_CostoDeEnviopais(Int32 _numerico);        
        public Task<RespuestaDto<List<DtoSlider>>> F_GetCarruselImagenesAsyc();
        public Task<RespuestaDto<List<DtoSliderizquierdo>>> F_GetCarruselImagenesIzquierdoPonalAsyc();
        public Task<RespuestaDto<List<DtoSliderInferior>>> F_GetCarruselImagenesInferiorPonalAsyc();
        public Task<RespuestaDto<List<DtoSliderComentarios>>> F_GetCarruselImagenesComentariosPonalAsyc();
        public Task<RespuestaDto<bool>> ObtenerOudSeviciosAsync(DtoCredenciales dtoCredenciales);



        

    }
}
