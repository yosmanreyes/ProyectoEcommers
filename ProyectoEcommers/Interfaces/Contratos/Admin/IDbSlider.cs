

namespace ProyectoEcommers.Models
{

    using Comun.Dto;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IDbSlider
    {
        public Task<int> F_GetImagen(DtoSlider Imagen);
        public Task<RespuestaDto<List<DtoSlider>>> F_GetSlide();
        public Task<RespuestaDto<List<DtoSlider>>> F_UpdateSlide(DtoSlider Imagen);

        public Task<int> F_GetImagenIzquierdo(DtoSliderizquierdo Imagen);
        public Task<RespuestaDto<List<DtoSliderizquierdo>>> F_GetSlideIzquierdo();
        public Task<RespuestaDto<List<DtoSliderizquierdo>>> F_UpdateSlideIzquierdo(DtoSliderizquierdo Imagen);

        public Task<int> F_GetImagenInferior(DtoSliderInferior Imagen);
        public Task<RespuestaDto<List<DtoSliderInferior>>> F_GetSlideInferior();
        public Task<RespuestaDto<List<DtoSliderInferior>>> F_UpdateSlideInferior(DtoSliderInferior Imagen);

        public Task<int> F_GetImagenComentarios(DtoSliderComentarios Imagen);
        public Task<RespuestaDto<List<DtoSliderComentarios>>> F_GetSlideComentarios();
        public Task<RespuestaDto<List<DtoSliderComentarios>>> F_UpdateSlideComentarios(DtoSliderComentarios Imagen);

        public Task<int> Ins_Mensaje(DtoMensajes Imagen);
        public Task<RespuestaDto<List<DtoMensajes>>> F_GetMensajeGrilla();

        public Task<RespuestaDto<List<DtoMensajes>>> InsIdMensaje(DtoMensajes Imagen);
    }
}
