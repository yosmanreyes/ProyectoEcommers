

namespace ProyectoEcommers.Models
{

    using Comun.Dto;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDbAdministracion
    {

        public Task<List<DtoMenu>> F_GetMenu(List<DtoUserRoles> P_Rol, Int64 P_Identificacion);
        public Task<List<DtoMenu>> F_GetMenuGeneral();
        public Task<DtoUsuario> F_GetValidaUser(DtoCredenciales V_Usuario, string V_Maquina);
        public Task<RespuestaDto<List<DtoDominios>>> GetRoles();
        public Task<RespuestaDto<List<DtoUsuario>>> GetUsuario(Int32 _Identifcacion);
        public Task<RespuestaDto<List<DtoUserRoles>>> GetUsuarioGrilla(Int32 _Identifcacion);
        public Task<RespuestaDto<List<DtoUserRoles>>> InsDesactivarUsuario(DtoUserRoles _roles);
        public Task<int> InsUsuario(DtoUsuario _roles);
        public Task<int> UpdUsuario(DtoUsuario _roles);
        public Task<RespuestaDto<List<DtoDominios>>> GetCorreos();
        public Task<RespuestaDto<List<DtoDominios>>> GetCorreosComentarios();
        public Task<RespuestaDto<List<DtoDominios>>> GetCorreosBandejaVentas();

    }
}
