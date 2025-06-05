
namespace ProyectoEcommers.Models
{


    using Comun.Dto;
    using System.Collections.Generic;
    using System.Threading.Tasks;


    public interface IDbComentarios
    {
        public Task<int> Ins_Comentarios(DtoComentariosClientes DtoComentariosClientes);
        public Task<RespuestaDto<List<DtoMensajes>>> F_GetMensaje();
        public Task<int> Ins_ComentarioEnvioCorreo(DtoEnvioCorreo DtoComentariosClientes);
        
    }
}
