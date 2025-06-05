

namespace ProyectoEcommers.Models
{

using Comun.Dto;
using Comun.Enumeraciones;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


    public class DbComentarios : IDbComentarios
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly ModelContext _context;

        public DbComentarios(IConfiguration configuration, ILogger<DbAdministracion> logger, ModelContext context)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }
        //public async Task<int> Ins_Comentarios(DtoComentariosClientes DtoComentariosClientes)
        //{
        //    var Resultado = 0;
        //    DateTime ahoraUtc = DateTime.Now;
        //    string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");
        //    using (ModelContext db2 = new ModelContext())
        //    {

        //        try
        //        {
        //            comentarios_clientes result = new comentarios_clientes
        //            {
        //                Nombres = DtoComentariosClientes.Nombres,
        //                CorreoElectronico = DtoComentariosClientes.CorreoElectronico,
        //                NumeroTelefono = DtoComentariosClientes.NumeroTelefono,
        //                Comentarios = DtoComentariosClientes.Comentarios,
        //                fechaCreacion = Convert.ToDateTime(fechaHoraFormateada),
        //                maquinaCreacion = DtoComentariosClientes.maquinaCreacion,
        //                vigente = "SI",
        //                NumeroTelefonoConIndicativo = DtoComentariosClientes.NumeroTelefonoConIndicativo,
        //                EnvioCorreo = DtoComentariosClientes.EnvioCorreo,
        //                Atendio = DtoComentariosClientes.Atendio


        //            };
        //            db2.comentarios_clientes.Add(result);
        //            var Resul = await db2.SaveChangesAsync();
        //            db2.SaveChanges();
        //            if (Resul > 0)
        //            {
        //                Resultado = Resul;

        //            }
        //            else
        //            {
        //                Resultado = Resul;
        //            }


        //        }
        //        catch (Exception ex) { }

        //        return Resultado;
        //    }

        //}

        public async Task<int> Ins_Comentarios(DtoComentariosClientes DtoComentariosClientes)
        {
            var Resultado = 0;
            DateTime ahoraUtc = DateTime.Now;
            string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");

            try
            {
                // Usamos _context en lugar de crear una nueva instancia de ModelContext
                comentarios_clientes result = new comentarios_clientes
                {
                    Nombres = DtoComentariosClientes.Nombres,
                    CorreoElectronico = DtoComentariosClientes.CorreoElectronico,
                    NumeroTelefono = DtoComentariosClientes.NumeroTelefono,
                    Comentarios = DtoComentariosClientes.Comentarios,
                    fechaCreacion = Convert.ToDateTime(fechaHoraFormateada),
                    maquinaCreacion = DtoComentariosClientes.maquinaCreacion,
                    vigente = "SI",
                    NumeroTelefonoConIndicativo = DtoComentariosClientes.NumeroTelefonoConIndicativo,
                    EnvioCorreo = DtoComentariosClientes.EnvioCorreo,
                    Atendio = DtoComentariosClientes.Atendio
                };

                // Agregamos el objeto al contexto
                _context.comentarios_clientes.Add(result);

                // Guardamos los cambios de manera asíncrona
                var Resul = await _context.SaveChangesAsync();

                // Verificamos si se ha guardado correctamente
                if (Resul > 0)
                {
                    Resultado = Resul;
                }
                else
                {
                    Resultado = Resul;
                }
            }
            catch (Exception ex)
            {
                // Aquí podrías registrar el error si es necesario
                // Ejemplo: Log.Error(ex, "Error al insertar comentario del cliente.");
            }

            return Resultado;
        }


        //public async Task<int> Ins_Comentarios(DtoComentariosClientes DtoComentariosClientes)
        //{
        //    var Resultado = 0;
        //    DateTime ahoraUtc = DateTime.UtcNow;
        //    string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");

        //    try
        //    {
        //        // Usamos el contexto inyectado aquí, no es necesario crear un nuevo contexto
        //        var comentario = new comentarios_clientes
        //        {
        //            Nombres = DtoComentariosClientes.Nombres,
        //            CorreoElectronico = DtoComentariosClientes.CorreoElectronico,
        //            NumeroTelefono = DtoComentariosClientes.NumeroTelefono,
        //            Comentarios = DtoComentariosClientes.Comentarios,
        //            fechaCreacion = Convert.ToDateTime(fechaHoraFormateada),
        //            maquinaCreacion = DtoComentariosClientes.maquinaCreacion,
        //            vigente = "SI",
        //            NumeroTelefonoConIndicativo = DtoComentariosClientes.NumeroTelefonoConIndicativo,
        //            EnvioCorreo = DtoComentariosClientes.EnvioCorreo,
        //            Atendio = DtoComentariosClientes.Atendio
        //        };

        //        _context.comentarios_clientes.Add(comentario);
        //        Resultado = await _context.SaveChangesAsync();

        //        return Resultado;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Manejo de errores
        //        // Logger.LogError(ex, "Error al insertar comentario de cliente.");
        //        return Resultado; // Retornar el resultado si hubo error o no se insertó nada
        //    }
        //}

        public async Task<int> Ins_ComentarioEnvioCorreo(DtoEnvioCorreo DtoEnvioCorreo)
        {
            var Resultado = 0;
            DateTime ahoraUtc = DateTime.Now;
            string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");

            try
            {
                // Utilizamos _context en lugar de crear una nueva instancia de ModelContext
                int productoId1 = 1;
                int num1 = _context.envio_correo.Count();

                if (num1 > 0)
                {
                    // Obtenemos el valor máximo de idEnvioCorreo y le sumamos 1
                    productoId1 = _context.envio_correo.Max(x => x.idEnvioCorreo) + 1;
                }

                // Creamos un objeto result con los valores del DtoEnvioCorreo
                envio_correo result = new envio_correo
                {
                    idEnvioCorreo = productoId1,
                    idComentario = DtoEnvioCorreo.IdComentario,
                    CorreoEnviar = DtoEnvioCorreo.CorreoEnviar,
                    Asunto = DtoEnvioCorreo.Asunto,
                    Mensaje = DtoEnvioCorreo.Mensaje,
                    fechaCreacion = Convert.ToDateTime(fechaHoraFormateada),
                    MaquinaCreacion = DtoEnvioCorreo.MaquinaCreacion,
                    Vigente = "SI",
                    EnvioMasivo = "NO"
                };

                // Agregamos el nuevo objeto al contexto
                _context.envio_correo.Add(result);

                // Guardamos los cambios de manera asíncrona
                var Resul = await _context.SaveChangesAsync();

                // Si SaveChangesAsync devuelve un valor mayor que 0, es porque se guardó correctamente
                if (Resul > 0)
                {
                    Resultado = Resul;
                }
                else
                {
                    Resultado = Resul;
                }
            }
            catch (Exception ex)
            {
                // Aquí podrías registrar el error si es necesario
                // Ejemplo: Log.Error(ex, "Error al insertar el comentario de envío de correo.");
            }

            return Resultado;
        }


        //public async Task<int> Ins_ComentarioEnvioCorreo(DtoEnvioCorreo DtoEnvioCorreo)
        //{
        //    var Resultado = 0;
        //    DateTime ahoraUtc = DateTime.Now;
        //    string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");
        //    using (ModelContext db2 = new ModelContext())
        //    {

        //        try
        //        {

        //            int productoId1 = 1;
        //            int num1 = db2.envio_correo.Count();
        //            if (num1 > 0)
        //            {
        //                productoId1 = db2.envio_correo.Max(x => x.idEnvioCorreo) + 1;
        //            }

        //            envio_correo result = new envio_correo
        //            {
        //                idEnvioCorreo = productoId1,
        //                idComentario = DtoEnvioCorreo.IdComentario,
        //                CorreoEnviar = DtoEnvioCorreo.CorreoEnviar,
        //                Asunto = DtoEnvioCorreo.Asunto,
        //                Mensaje = DtoEnvioCorreo.Mensaje,
        //                fechaCreacion = Convert.ToDateTime(fechaHoraFormateada),
        //                MaquinaCreacion = DtoEnvioCorreo.MaquinaCreacion,
        //                Vigente = "SI",
        //                EnvioMasivo = "NO"
        //            };
        //            db2.envio_correo.Add(result);
        //            var Resul = await db2.SaveChangesAsync();
        //            db2.SaveChanges();
        //            if (Resul > 0)
        //            {
        //                Resultado = Resul;

        //            }
        //            else
        //            {
        //                Resultado = Resul;
        //            }


        //        }
        //        catch (Exception ex) { }

        //        return Resultado;
        //    }

        //}


        //public async Task<int> Ins_ComentarioEnvioCorreo(DtoEnvioCorreo DtoEnvioCorreo)
        //    {
        //        var Resultado = 0;
        //        DateTime ahoraUtc = DateTime.UtcNow;
        //        string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");

        //        try
        //        {
        //            // Usamos la inyección de _context en lugar de crear una nueva instancia manualmente.
        //            int productoId1 = 1;
        //            int num1 = await _context.envio_correo.CountAsync();
        //            if (num1 > 0)
        //            {
        //                productoId1 = await _context.envio_correo.MaxAsync(x => x.idEnvioCorreo) + 1;
        //            }

        //            // Crear el objeto de la entidad para insertar en la base de datos
        //            var result = new envio_correo
        //            {
        //                idEnvioCorreo = productoId1,
        //                idComentario = DtoEnvioCorreo.IdComentario,
        //                CorreoEnviar = DtoEnvioCorreo.CorreoEnviar,
        //                Asunto = DtoEnvioCorreo.Asunto,
        //                Mensaje = DtoEnvioCorreo.Mensaje,
        //                fechaCreacion = Convert.ToDateTime(fechaHoraFormateada),
        //                MaquinaCreacion = DtoEnvioCorreo.MaquinaCreacion,
        //                Vigente = "SI",
        //                EnvioMasivo = "NO"
        //            };

        //            // Agregar el nuevo comentario de envío de correo al contexto
        //            await _context.envio_correo.AddAsync(result);

        //            // Guardar los cambios en la base de datos
        //            Resultado = await _context.SaveChangesAsync();

        //            // Si el resultado es mayor que 0, significa que se realizó la inserción con éxito.
        //            return Resultado;
        //        }
        //        catch (Exception ex)
        //        {
        //            // Manejo de excepciones, se puede agregar logging si es necesario
        //            // Logger.LogError(ex, "Error al insertar el comentario de envío de correo.");
        //            return Resultado; // En caso de error, retornar el resultado (0 o el código de error deseado)
        //        }
        //    }


        public async Task<RespuestaDto<List<DtoMensajes>>> F_GetMensaje()
        {

            try
            {


                var Respuesta = new RespuestaDto<List<DtoMensajes>>();
                Respuesta.Codigo = EstadoOperacion.Bueno;
                Respuesta.Respuesta = new List<DtoMensajes>();
                var Slider = _context.ctr_mensajes
                .Where(x => x.VIGENTE == "SI")
                .ToList();

                if (Slider.Count > 0)
                {
                    foreach (var obj in Slider)
                    {
                        DtoMensajes? retorno = new DtoMensajes();
                        {
                            retorno.Mensaje = obj.MENSAJE;
                            retorno.TituloMensaje = obj.TITULOMENSAJES;                    
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
    }
}
