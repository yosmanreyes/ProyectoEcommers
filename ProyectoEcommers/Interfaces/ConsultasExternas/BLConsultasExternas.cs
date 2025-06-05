

namespace Negocio.Areas.ConsultasExternas
{

    using Comun.Dto;
    using Comun.Enumeraciones;
   
    using Microsoft.Extensions.Configuration;
    using Negocio.Contratos.ConsultasExternas;
    using ProyectoEcommers.Models;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    public class BLConsultasExternas : IBLConsultasExternas
    {
        private readonly IApiExternos _ApiExternos;
        private readonly IConfiguration _configuration;
        private readonly ModelContext _context;

        public BLConsultasExternas(IApiExternos apiExternos, IConfiguration configuration, ModelContext context)
        {
            _ApiExternos = apiExternos;
            _configuration = configuration;
            _context = context;
        }

        public async Task<RespuestaDto<List<DtoDominios>>> F_GetDominios(Int32 _numero)
        {

            try
            {
                var Respuesta = new RespuestaDto<List<DtoDominios>>();
                Respuesta.Codigo = EstadoOperacion.Bueno;
                Respuesta.Respuesta = new List<DtoDominios>();
                var Slider = _context.ctrl_dominios
                .Where(x => x.VIGENTE == 1 && x.PADRE_ID == _numero).OrderBy(x => x.DESCRIPCION)
                .ToList();

                if (Slider.Count > 0)
                {
                    foreach (var obj in Slider)
                    {
                        DtoDominios? retorno = new DtoDominios();
                        {
                            retorno.IdDominio = obj.ID_DOMINIO;
                            retorno.Descripcion = obj.DESCRIPCION.ToUpper();

                        };
                        Respuesta.Respuesta.Add(retorno);
                    }
                    return Respuesta;

                }
                else
                {
                    Respuesta.Codigo = EstadoOperacion.Malo;
                    return Respuesta;
                }
            }
            catch (Exception ex)
            {

                throw;
            }



        }
        public async Task<RespuestaDto<List<DtoDominios>>> F_GetPaises(Int32 _numero)
        {
            try
            {
                var Respuesta = new RespuestaDto<List<DtoDominios>>();
                Respuesta.Codigo = EstadoOperacion.Bueno;
                Respuesta.Respuesta = new List<DtoDominios>();
                var Slider = _context.ctr_paises
                .Where(x => x.VIGENTE == "SI" && x.LUGE_CODIGO == _numero && x.INDICATIVO != null).OrderBy(x => x.DESCRIPCION)
                .ToList();

                if (Slider.Count > 0)
                {
                    foreach (var obj in Slider)
                    {
                        DtoDominios? retorno = new DtoDominios();
                        {
                            retorno.IdDominio = obj.CODIGO;
                            retorno.Descripcion = obj.DESCRIPCION;
                            retorno.Abreviatura = obj.INDICATIVO;
                            retorno.CostoEnvio = obj.COSTO_ENVIO;

                        };
                        Respuesta.Respuesta.Add(retorno);
                    }
                    return Respuesta;

                }
                else
                {
                    Respuesta.Codigo = EstadoOperacion.Malo;
                    return Respuesta;
                }
            }
            catch (Exception ex)
            {

                throw;
            }



        }

        public async Task<RespuestaDto<List<DtoDominios>>> F_GetPaisesDepartamento(Int32 _numero)
        {
            try
            {
                var Respuesta = new RespuestaDto<List<DtoDominios>>();
                Respuesta.Codigo = EstadoOperacion.Bueno;
                Respuesta.Respuesta = new List<DtoDominios>();
                var Slider = _context.ctr_paises
                .Where(x => x.VIGENTE == "SI" && x.LUGE_CODIGO == _numero).OrderBy(x => x.DESCRIPCION)
                .ToList();

                if (Slider.Count > 0)
                {
                    foreach (var obj in Slider)
                    {
                        DtoDominios? retorno = new DtoDominios();
                        {
                            retorno.IdDominio = obj.CODIGO;
                            retorno.Descripcion = obj.DESCRIPCION;
                            retorno.CostoEnvio = obj.COSTO_ENVIO;

                        };
                        Respuesta.Respuesta.Add(retorno);
                    }
                    return Respuesta;

                }
                else
                {
                    Respuesta.Codigo = EstadoOperacion.Malo;
                    return Respuesta;
                }
            }
            catch (Exception ex)
            {

                throw;
            }



        }
        public async Task<RespuestaDto<List<DtoDominios>>> F_CostoDeEnvio(Int32 _numero)
        {
            try
            {
                var Respuesta = new RespuestaDto<List<DtoDominios>>();
                Respuesta.Codigo = EstadoOperacion.Bueno;
                Respuesta.Respuesta = new List<DtoDominios>();
                var Slider = _context.ctr_paises
                .Where(x => x.VIGENTE == "SI" && x.CODIGO == _numero).OrderBy(x => x.DESCRIPCION)
                .ToList();

                if (Slider.Count > 0)
                {
                    foreach (var obj in Slider)
                    {
                        DtoDominios? retorno = new DtoDominios();
                        {
                            retorno.IdDominio = obj.CODIGO;
                            retorno.Descripcion = obj.DESCRIPCION;
                            retorno.CostoEnvio = obj.COSTO_ENVIO;

                        };
                        Respuesta.Respuesta.Add(retorno);
                    }
                    return Respuesta;

                }
                else
                {
                    Respuesta.Codigo = EstadoOperacion.Malo;
                    return Respuesta;
                }
            }
            catch (Exception ex)
            {

                throw;
            }



        }
        public async Task<RespuestaDto<DtoDominios>> F_CostoDeEnvioDepartamento(Int32 _numero)
        {
            try
            {
                var Respuesta = new RespuestaDto<DtoDominios>();
                Respuesta.Codigo = EstadoOperacion.Bueno;
                Respuesta.Respuesta = new DtoDominios();
                var Slider = _context.ctr_paises
                .Where(x => x.VIGENTE == "SI" && x.LUGE_CODIGO == _numero).OrderBy(x => x.DESCRIPCION)
                .FirstOrDefault();

                if (Slider.CODIGO > 0)
                {

                    Respuesta.Respuesta = new()
                    {
                        IdDominio = Slider.CODIGO,
                        Descripcion = Slider.DESCRIPCION,
                        CostoEnvio = Slider.COSTO_ENVIO,

                        };
                     
                    return Respuesta;

                }
                else
                {
                    Respuesta.Codigo = EstadoOperacion.Malo;
                    return Respuesta;
                }
            }
            catch (Exception ex)
            {

                throw;
            }



        }
        public async Task<RespuestaDto<DtoDominios>> F_CostoDeEnviopais(Int32 _numero)
        {
            try
            {

                var Respuesta = new RespuestaDto<DtoDominios>();
                Respuesta.Codigo = EstadoOperacion.Bueno;
                Respuesta.Respuesta = new DtoDominios();
                if (_numero == 0) {
                    Respuesta.Codigo = EstadoOperacion.Malo;
                    return Respuesta;
                }
         
                var Slider = _context.ctr_paises
                .Where(x => x.VIGENTE == "SI" && x.CODIGO == _numero).OrderBy(x => x.DESCRIPCION)
                .FirstOrDefault();

                if (Slider.CODIGO > 0)
                {

                    Respuesta.Respuesta = new()
                    {
                        IdDominio = Slider.CODIGO,
                        Descripcion = Slider.DESCRIPCION,
                        CostoEnvio = Slider.COSTO_ENVIO,

                    };

                    return Respuesta;

                }
                else
                {
                    Respuesta.Codigo = EstadoOperacion.Malo;
                    return Respuesta;
                }
            }
            catch (Exception ex)
            {

                throw;
            }



        }
        

        public async Task<RespuestaDto<List<DtoDominios>>> F_GetPaisesId(Int32 _numero)
        {
            try
            {
                var Respuesta = new RespuestaDto<List<DtoDominios>>();
                Respuesta.Codigo = EstadoOperacion.Bueno;
                Respuesta.Respuesta = new List<DtoDominios>();
                var Slider = _context.ctr_paises
                .Where(x => x.VIGENTE == "SI" && x.CODIGO == _numero).OrderBy(x => x.DESCRIPCION)
                .ToList();

                if (Slider.Count > 0)
                {
                    foreach (var obj in Slider)
                    {
                        DtoDominios? retorno = new DtoDominios();
                        {
                            retorno.IdDominio = obj.CODIGO;
                            retorno.Descripcion = obj.DESCRIPCION;

                        };
                        Respuesta.Respuesta.Add(retorno);
                    }
                    return Respuesta;

                }
                else
                {
                    Respuesta.Codigo = EstadoOperacion.Malo;
                    return Respuesta;
                }
            }
            catch (Exception ex)
            {

                throw;
            }



        }
        public async Task<RespuestaDto<List<DtoSlider>>> F_GetCarruselImagenesAsyc()
        {
            try
            {
                var Respuesta = new RespuestaDto<List<DtoSlider>>();
                Respuesta.Codigo = EstadoOperacion.Bueno;
                Respuesta.Respuesta = new List<DtoSlider>();
                var Slider = _context.imagenes
                .Where(x => x.VIGENTE == 1)
                .ToList();

                if (Slider.Count > 0)
                {
                    foreach (var obj in Slider)
                    {
                        DtoSlider? retorno = new DtoSlider();
                        {
                            retorno.Consecutivo = Convert.ToInt64(obj.CONSECUTIVO);
                            retorno.Url = obj.URL;
                            retorno.FileName = obj.FILENAME;
                        };
                        Respuesta.Respuesta.Add(retorno);
                    }
                    return Respuesta;

                }
                else
                {
                    Respuesta.Codigo = EstadoOperacion.Malo;
                    return Respuesta;
                }
            }
            catch (Exception ex)
            {

                throw;
            }



        }
        public async Task<RespuestaDto<List<DtoSliderizquierdo>>> F_GetCarruselImagenesIzquierdoPonalAsyc()
        {
            try
            {
                var Respuesta = new RespuestaDto<List<DtoSliderizquierdo>>();
                Respuesta.Codigo = EstadoOperacion.Bueno;
                Respuesta.Respuesta = new List<DtoSliderizquierdo>();
                var Slider = _context.imagenesizquierda
                .Where(x => x.VIGENTE == 1)
                .ToList();

                if (Slider.Count > 0)
                {
                    foreach (var obj in Slider)
                    {
                        DtoSliderizquierdo? retorno = new DtoSliderizquierdo();
                        {
                            retorno.Consecutivo = Convert.ToInt64(obj.CONSECUTIVO);
                            retorno.Url = obj.URL;
                            retorno.FileName = obj.FILENAME;

                        };
                        Respuesta.Respuesta.Add(retorno);
                    }
                    return Respuesta;

                }
                else
                {
                    Respuesta.Codigo = EstadoOperacion.Malo;
                    return Respuesta;
                }
            }
            catch (Exception ex)
            {

                throw;
            }



        }
        public async Task<RespuestaDto<List<DtoSliderInferior>>> F_GetCarruselImagenesInferiorPonalAsyc()
        {

            try
            {


                var Respuesta = new RespuestaDto<List<DtoSliderInferior>>();
                Respuesta.Codigo = EstadoOperacion.Bueno;
                Respuesta.Respuesta = new List<DtoSliderInferior>();
                var Slider = _context.ImagenesInferior
                .Where(x => x.VIGENTE == 1)
                .ToList();

                if (Slider.Count > 0)
                {
                    foreach (var obj in Slider)
                    {
                        DtoSliderInferior? retorno = new DtoSliderInferior();
                        {
                            retorno.Consecutivo = Convert.ToInt64(obj.CONSECUTIVO);
                            retorno.Url = obj.URL;
                            retorno.FileName = obj.FILENAME;
                        };
                        Respuesta.Respuesta.Add(retorno);
                    }
                    return Respuesta;

                }
                else
                {
                    Respuesta.Codigo = EstadoOperacion.Malo;
                    return Respuesta;
                }
            }
            catch (Exception ex)
            {

                throw;
            }



        }
        public async Task<RespuestaDto<List<DtoSliderComentarios>>> F_GetCarruselImagenesComentariosPonalAsyc()
        {
            try
            {
                var Respuesta = new RespuestaDto<List<DtoSliderComentarios>>();
                Respuesta.Codigo = EstadoOperacion.Bueno;
                Respuesta.Respuesta = new List<DtoSliderComentarios>();
                var Slider = _context.imagenesComentarios
                .Where(x => x.VIGENTE == 1)
                .ToList();

                if (Slider.Count > 0)
                {
                    foreach (var obj in Slider)
                    {
                        DtoSliderComentarios? retorno = new DtoSliderComentarios();
                        {
                            retorno.Consecutivo = Convert.ToInt64(obj.CONSECUTIVO);
                            retorno.Url = obj.URL;
                            retorno.FileName = obj.FILENAME;
                            retorno.FechaComentario = Convert.ToString(obj.FECHA_COMENTARIO);
                            retorno.TituloComentario = obj.TITULOCOMENTARIO;
                            retorno.Comentario = obj.COMENTARIO;
                        };
                        Respuesta.Respuesta.Add(retorno);
                    }
                    return Respuesta;
                }
                else
                {
                    Respuesta.Codigo = EstadoOperacion.Malo;
                    return Respuesta;
                }
            }
            catch (Exception ex)
            {
                throw;
            }



        }
        public async Task<RespuestaDto<bool>> ObtenerOudSeviciosAsync(DtoCredenciales dtoCredenciales)
        {
            try
            {
                var token = await BLObtenerTokenPIPAsync();
                return await _ApiExternos.ObtenerOudSeviciosAsync(dtoCredenciales, token.Respuesta);
            }
            catch (Exception e)
            {
                throw new Exception("Error " + e.Message);
            }
        }
        public async Task<RespuestaDto<string>> BLObtenerTokenPIPAsync()
        {
            return await _ApiExternos.ObtenerTokenPIPAsync(new DtoUsuarioPip
            {
                usuario = _configuration.GetSection("UsuarioPip").Value,
                clave = _configuration.GetSection("ClavePip").Value,
                idServicio = 0
            });
        }



    }
}
