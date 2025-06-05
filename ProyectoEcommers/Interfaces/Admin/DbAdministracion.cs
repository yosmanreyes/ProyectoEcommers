
namespace ProyectoEcommers.Models
{

    using Comun.Dto;
    using Comun.Enumeraciones;
   
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using System.Data;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class DbAdministracion : IDbAdministracion
    {
        #region Propiedades
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly ModelContext _context;
        #endregion

        #region Constructor
        public DbAdministracion(IConfiguration configuration, ILogger<DbAdministracion> logger, ModelContext context)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }
        #endregion

        #region Metodos Base de Datos        
        public async Task<DtoUsuario> F_GetValidaUser(DtoCredenciales V_Usuario, string V_Maquina)
        {

            try
            {
                DtoUsuario retorno = new DtoUsuario();
                var usuarios = _context.Ctr_usuarios
                .Where(u => u.Username == V_Usuario.UsuarioEmpresarial && u.Bloqueado == 1 && u.Clave == V_Usuario.ClaveEmpresarial)
                .ToList();

                foreach (var usuario in usuarios)
                {
                    retorno.Usuario = usuario.Username;
                    retorno.IdUsuario = usuario.IdUsuario;
                    retorno.Funcionario = usuario.Funcionario.ToUpper();
                    retorno.Apellidos = usuario.Apellidos.ToUpper();
                    retorno.Identificacion = Convert.ToUInt32(usuario.Identificacion);
                    retorno.Cargo = Convert.ToString(usuario.Cargo);
                    retorno.Correo = usuario.Correo.ToUpper();
                    retorno.Celular = Convert.ToInt64(usuario.Celular);
                    retorno.CantidadVentaProductos = _context.mi_compra_realizada.Where(x => x.Estado_producto == "" || x.Estado_producto == null && (x.Cancelada == "NO" || x.Cancelada == null)).Count();
                    retorno.CantidadMensajes = _context.comentarios_clientes.Where(x => x.Atendio == "NO").Count();
                }

                retorno.DtoUserRoles = new List<DtoUserRoles>();
                var Roles = _context.ctr_roles_user
                .Where(u => u.ID_USUARIO == retorno.IdUsuario && (u.FK_USUARIO.Bloqueado == 1) && u.VIGENTE == "SI")
                .ToList();
                foreach (var rol in Roles)
                {

                    DtoUserRoles Obj = new DtoUserRoles
                    {
                        IdRol = rol.ID_ROL,
                        Descripcion = _context.ctr_roles.OrderByDescending(t => t.ID_ROL == rol.ID_ROL).First().DESCRIPCION
                    };
                    retorno.DtoUserRoles.Add(Obj);
                }
                return retorno;
            }
            catch (Exception ex)
            {

                throw;
            }



        }
        //public async Task<List<DtoMenu>> F_GetMenu(List<DtoUserRoles> V_Rol, Int64 V_Identificacion)
        //{
        //    List<DtoMenu> accionesTemp = new List<DtoMenu>();
        //    using (ModelContext db2 = new ModelContext())
        //    {
        //        foreach (var item in V_Rol)
        //        {
        //            var lista = db2.ctr_menu_roles.Where(x => x.ID_ROL == item.IdRol && (x.FK_MENU.VIGENTE == 1)).Select(a => new DtoMenu
        //            {
        //                IDMENU = a.FK_MENU.ID_MENU,
        //                PADREID = a.FK_MENU.PADREID,
        //                POSICION = Convert.ToInt32(a.FK_MENU.POSICION),
        //                ICONO = a.FK_MENU.ICONO,
        //                DETALLE = a.FK_MENU.DETALLE,
        //                TIPO = a.FK_MENU.TIPO,
        //                DESCRIPCION = a.FK_MENU.DESCRIPCION,
        //                CONTROLADOR = a.FK_MENU.CONTROLADOR,
        //                VISTA = a.FK_MENU.VISTA,
        //                AREA = a.FK_MENU.AREA
        //            }).ToList();

        //            foreach (var item1 in lista)
        //            {
        //                if (!accionesTemp.Any(x => x.IDMENU == item1.IDMENU))
        //                    accionesTemp.Add(item1);
        //            }
        //        }
        //        return accionesTemp;
        //    }
        //}
        public async Task<List<DtoMenu>> F_GetMenu(List<DtoUserRoles> V_Rol, Int64 V_Identificacion)
        {
            List<DtoMenu> accionesTemp = new List<DtoMenu>();

            // Iterar sobre los roles para obtener el menú
            foreach (var item in V_Rol)
            {
                // Hacer la consulta asincrónica usando ToListAsync()
                var lista =  _context.ctr_menu_roles
                    .Where(x => x.ID_ROL == item.IdRol && x.FK_MENU.VIGENTE == 1)
                    .Select(a => new DtoMenu
                    {
                        IDMENU = a.FK_MENU.ID_MENU,
                        PADREID = a.FK_MENU.PADREID,
                        POSICION = Convert.ToInt32(a.FK_MENU.POSICION),
                        ICONO = a.FK_MENU.ICONO,
                        DETALLE = a.FK_MENU.DETALLE,
                        TIPO = a.FK_MENU.TIPO,
                        DESCRIPCION = a.FK_MENU.DESCRIPCION,
                        CONTROLADOR = a.FK_MENU.CONTROLADOR,
                        VISTA = a.FK_MENU.VISTA,
                        AREA = a.FK_MENU.AREA
                    })
                    .ToList(); // Método asincrónico adecuado

                // Agregar elementos a la lista sin duplicados
                foreach (var item1 in lista)
                {
                    if (!accionesTemp.Any(x => x.IDMENU == item1.IDMENU))
                        accionesTemp.Add(item1);
                }
            }

            return accionesTemp;
        }




        //public async Task<List<DtoMenu>> F_GetMenuGeneral()
        //{
        //    List<DtoMenu> accionesTemp = new List<DtoMenu>();
        //    using (ModelContext db2 = new ModelContext())
        //    {
        //        var lista = db2.ctr_menu.Where(x => x.PADREID == 0).Select(a => new DtoMenu
        //        {
        //            IDMENU = a.ID_MENU,
        //            PADREID = a.PADREID,
        //            POSICION = Convert.ToInt32(a.POSICION),
        //            ICONO = a.ICONO,
        //            DETALLE = a.DETALLE,
        //            TIPO = a.TIPO,
        //            DESCRIPCION = a.DESCRIPCION,
        //            CONTROLADOR = a.CONTROLADOR,
        //            VISTA = a.VISTA,
        //            AREA = a.AREA
        //        }).ToList();

        //        return lista;
        //    }
        //}

        public async Task<List<DtoMenu>> F_GetMenuGeneral()
        {
            // Usamos una lista para almacenar las acciones de menús
            List<DtoMenu> accionesTemp = new List<DtoMenu>();

            // Realizamos la consulta asíncrona
            var lista =  _context.ctr_menu
                .Where(x => x.PADREID == 0) // Filtramos los menús con PADREID igual a 0
                .Select(a => new DtoMenu
                {
                    IDMENU = a.ID_MENU,
                    PADREID = a.PADREID,
                    POSICION = Convert.ToInt32(a.POSICION),
                    ICONO = a.ICONO,
                    DETALLE = a.DETALLE,
                    TIPO = a.TIPO,
                    DESCRIPCION = a.DESCRIPCION,
                    CONTROLADOR = a.CONTROLADOR,
                    VISTA = a.VISTA,
                    AREA = a.AREA
                }).ToList(); // Realizamos la consulta de forma asíncrona

            // Retornamos la lista de menús obtenida
            return lista;
        }
        static IEnumerable<int> GetRolesFromTemp(string temp)
        {
            var regex = new Regex(@"\d+");
            var matches = regex.Matches(temp);

            return matches.Select(match => int.Parse(match.Value));
        }
        public async Task<RespuestaDto<List<DtoDominios>>> GetRoles()
        {
            try
            {
                var Respuesta = new RespuestaDto<List<DtoDominios>>();
                Respuesta.Codigo = EstadoOperacion.Bueno;
                Respuesta.Respuesta = new List<DtoDominios>();
                var Slider = _context.ctr_roles
                .Where(x => x.VIGENTE == 1).OrderBy(x => x.DESCRIPCION)
                .ToList();

                if (Slider.Count > 0)
                {
                    foreach (var obj in Slider)
                    {
                        DtoDominios? retorno = new DtoDominios();
                        {
                            retorno.IdDominio = obj.ID_ROL;
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
        public async Task<RespuestaDto<List<DtoUsuario>>> GetUsuario(Int32 _Identifcacion)
        {

            try
            {


                var Respuesta = new RespuestaDto<List<DtoUsuario>>();
                Respuesta.Codigo = EstadoOperacion.Bueno;
                Respuesta.Respuesta = new List<DtoUsuario>();
                //List<RespuestaDto> = new List<RespuestaDto>();

                var Slider = _context.Ctr_usuarios
                .Where(x => x.Identificacion == Convert.ToString(_Identifcacion)).OrderBy(x => x.Identificacion)
                .ToList();

                if (Slider.Count > 0)
                {
                    foreach (var obj in Slider)
                    {
                        DtoUsuario? retorno = new DtoUsuario();
                        {
                            retorno.Identificacion = Convert.ToInt64(obj.Identificacion);
                            retorno.Apellidos = obj.Apellidos;
                            retorno.Nombres = obj.Funcionario;
                            retorno.FechaCreacion = Convert.ToString(obj.Fecha_creacion);
                            retorno.Usuario = Convert.ToString(obj.Usuario_creacion);
                            retorno.Correo = obj.Correo;
                            retorno.Celular = Convert.ToInt64(obj.Celular);
                            retorno.DireccionResidencia = obj.Direccion_residencia;
                            retorno.Cliente = obj.Cliente;
                            retorno.Vigente = obj.Vigente;
                            retorno.Clave = obj.Clave;
                            retorno.FechaNacimiento = Convert.ToString(obj.Fecha_Nacimiento);
                            retorno.FechaVigencia = Convert.ToString(obj.Fecha_vigencia);
                            retorno.IdUsuario = obj.IdUsuario;


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
        public async Task<RespuestaDto<List<DtoUserRoles>>> GetUsuarioGrilla(Int32 _Identifcacion)
        {
            try
            {
                var Respuesta = new RespuestaDto<List<DtoUserRoles>>();
                Respuesta.Codigo = EstadoOperacion.Bueno;
                Respuesta.Respuesta = new List<DtoUserRoles>();
                DtoUserRoles? retorno = new DtoUserRoles();
                var Slider = _context.Ctr_usuarios
                .Where(x => x.Identificacion == Convert.ToString(_Identifcacion)).OrderBy(x => x.Identificacion)
                .ToList();

                if (Slider.Count > 0)
                {
                    foreach (var obj in Slider)
                    {
                        {
                            retorno.IdUsuario = obj.IdUsuario;
                        };
                    }
                    var Rol = _context.ctr_roles_user.Where(x => x.ID_USUARIO == retorno.IdUsuario).ToList();

                    foreach (var item in Rol)
                    {
                        DtoUserRoles? retorno2 = new DtoUserRoles();
                        {
                            retorno2.IdUserRol = item.ID_USER_ROL;
                            retorno2.IdUsuario = item.ID_USUARIO;
                            retorno2.IdRol = item.ID_ROL;
                            retorno2.Descripcion = _context.ctr_roles.OrderByDescending(t => t.ID_ROL == item.ID_ROL).First().DESCRIPCION;
                            retorno2.FechaCreacion = Convert.ToString(item.FECHA_CREACION);
                            retorno2.Vigente = item.VIGENTE;

                        }
                        Respuesta.Respuesta.Add(retorno2);

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
        public async Task<RespuestaDto<List<DtoUserRoles>>> InsDesactivarUsuario(DtoUserRoles _Roles)
        {
            var respuesta = 0;
            RespuestaDto<List<DtoUserRoles>> retorno = new RespuestaDto<List<DtoUserRoles>>();

            var query = from ord in _context.ctr_roles_user
                        where ord.ID_ROL == _Roles.IdRol && ord.ID_USER_ROL == _Roles.IdUserRol && ord.ID_USUARIO == _Roles.IdUsuario
                        select ord;

            foreach (var ord in query)
            {
                ord.VIGENTE = _Roles.Vigente;
            }

            try
            {
                respuesta = _context.SaveChanges();

                if (respuesta > 0)
                {
                    retorno.Codigo = EstadoOperacion.Bueno;
                    retorno.Mensaje = "El registro fue registrado exitosamente";
                }
                else
                {
                    retorno.Codigo = EstadoOperacion.Malo;
                    retorno.Mensaje = "No es posible Realizar el registro";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return retorno;
        }
        //public async Task<int> InsUsuario(DtoUsuario _obj)
        //{


        //    var Resultado = 0;
        //   DateTime ahoraUtc = DateTime.Now ;
        //    string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");

        //    DtoUsuario? retorno = new DtoUsuario();
        //    var Usuarios = _context.Ctr_usuarios
        //  .Where(x => x.Identificacion == Convert.ToString(_obj.Identificacion)).OrderBy(x => x.Identificacion)
        //        .ToList();

        //    DtoUsuario? retornoRol = new DtoUsuario();
        //    var UsuariosRol = _context.ctr_roles_user
        //  .Where(x => x.ID_ROL == Convert.ToInt32(_obj.IdRol) && x.ID_USUARIO == _obj.IdUsuario).OrderBy(x => x.ID_ROL)
        //        .ToList();

        //    using (ModelContext db = new ModelContext())
        //    {
        //        if (Usuarios.Count > 0)
        //        {

        //            if (UsuariosRol.Count > 0)
        //            {
        //                var validar = -1;
        //                return validar;
        //            }
        //            int numero = 1;
        //            int valida = db.ctr_roles_user.Count();
        //            if (valida > 0)
        //            {
        //                numero = db.ctr_roles_user.Max(s => s.ID_USER_ROL) + 1;
        //            }

        //            ctr_roles_user res = new ctr_roles_user
        //            {
        //                ID_USER_ROL = numero,
        //                ID_USUARIO = _obj.IdUsuario,
        //                ID_ROL = Convert.ToInt32(_obj.IdRol),
        //                FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
        //                USUARIO_CREACION = Convert.ToInt32(_obj.Usuario),
        //                MAQUINA_CREACION = _obj.Maquina,
        //                VIGENTE = _obj.Vigente,

        //            };
        //            db.ctr_roles_user.Add(res);
        //            var Resu = await db.SaveChangesAsync();
        //            db.SaveChanges();
        //            return Resu;
        //        }
        //    }

        //    if (_obj.IdUsuario > 0)
        //    {
        //        using (ModelContext db1 = new ModelContext())
        //        {

        //            try
        //            {
        //                Ctr_usuarios result = new Ctr_usuarios
        //                {
        //                    IdUsuario = _obj.IdUsuario,
        //                    Bloqueado = _obj.Bloqueado,
        //                    Identificacion = Convert.ToString(_obj.Identificacion),
        //                    Correo = _obj.Correo,
        //                    Fecha_creacion = Convert.ToDateTime(fechaHoraFormateada),
        //                    Maquina_creacion = _obj.Maquina,
        //                    Vigente = _obj.Vigente,
        //                    Fecha_Nacimiento = Convert.ToDateTime(_obj.FechaNacimiento),
        //                    Fecha_vigencia = Convert.ToDateTime(fechaHoraFormateada),
        //                    Apellidos = _obj.Apellidos,
        //                    Funcionario = _obj.Nombres,
        //                    Direccion_residencia = _obj.DireccionResidencia,
        //                    Cliente = _obj.Cliente,
        //                    Clave = _obj.Clave,
        //                    CODIGOCARGO = 1,
        //                    Celular = Convert.ToString(_obj.Celular),
        //                    Username = Convert.ToString(_obj.Identificacion),
        //                    Usuario_creacion = Convert.ToInt32(_obj.Usuario)
        //                };
        //                db1.Ctr_usuarios.Update(result);
        //                var Resul = await db1.SaveChangesAsync();
        //                db1.SaveChanges();
        //                if (Resul > 0)
        //                {
        //                    int numero = 1;
        //                    int valida = db1.ctr_roles_user.Count();
        //                    if (valida > 0)
        //                    {
        //                        numero = db1.ctr_roles_user.Max(s => s.ID_USER_ROL) + 1;
        //                    }

        //                    ctr_roles_user resultado = new ctr_roles_user
        //                    {
        //                        ID_USER_ROL = numero,
        //                        ID_USUARIO = _obj.IdUsuario,
        //                        ID_ROL = Convert.ToInt32(_obj.IdRol),
        //                        FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
        //                        USUARIO_CREACION = Convert.ToInt32(_obj.Usuario),
        //                        MAQUINA_CREACION = _obj.Maquina,
        //                        VIGENTE = _obj.Vigente,

        //                    };
        //                    db1.ctr_roles_user.Update(resultado);
        //                    var Resulta = await db1.SaveChangesAsync();
        //                    db1.SaveChanges();
        //                    Resultado = -2;

        //                }
        //                else
        //                {
        //                    Resultado = Resul;
        //                }


        //            }
        //            catch (Exception ex) { }

        //            return Resultado;
        //        }

        //    }
        //    else
        //    {
        //        using (ModelContext db2 = new ModelContext())
        //        {

        //            try
        //            {

        //                int UsuarioId = 1;
        //                int num = db2.Ctr_usuarios.Count();

        //                if (num > 0)
        //                {
        //                    UsuarioId = db2.Ctr_usuarios.Max(x => x.IdUsuario) + 1;
        //                }

        //                Ctr_usuarios result = new Ctr_usuarios
        //                {
        //                    IdUsuario = UsuarioId,
        //                    Bloqueado = _obj.Bloqueado,
        //                    Identificacion = Convert.ToString(_obj.Identificacion),
        //                    Correo = _obj.Correo,
        //                    Fecha_creacion = Convert.ToDateTime(fechaHoraFormateada),
        //                    Maquina_creacion = _obj.Maquina,
        //                    Vigente = _obj.Vigente,
        //                    Fecha_Nacimiento = Convert.ToDateTime(_obj.FechaNacimiento),
        //                    Fecha_vigencia = Convert.ToDateTime(fechaHoraFormateada),
        //                    Apellidos = _obj.Apellidos,
        //                    Funcionario = _obj.Nombres,
        //                    Direccion_residencia = _obj.DireccionResidencia,
        //                    Cliente = _obj.Cliente,
        //                    Clave = _obj.Clave,
        //                    Celular = Convert.ToString(_obj.Celular),
        //                    CODIGOCARGO = 1,
        //                    Username = Convert.ToString(_obj.Identificacion),
        //                    Usuario_creacion = Convert.ToInt32(_obj.Usuario)
        //                };
        //                db2.Ctr_usuarios.Add(result);
        //                var Resul = await db2.SaveChangesAsync();
        //                db2.SaveChanges();
        //                if (Resul > 0)
        //                {
        //                    int numero = 1;
        //                    int valida = db2.ctr_roles_user.Count();
        //                    if (valida > 0)
        //                    {
        //                        numero = db2.ctr_roles_user.Max(s => s.ID_USER_ROL) + 1;
        //                    }

        //                    ctr_roles_user resultado = new ctr_roles_user
        //                    {
        //                        ID_USER_ROL = numero,
        //                        ID_USUARIO = UsuarioId,
        //                        ID_ROL = Convert.ToInt32(_obj.IdRol),
        //                        FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
        //                        USUARIO_CREACION = Convert.ToInt32(_obj.Usuario),
        //                        MAQUINA_CREACION = _obj.Maquina,
        //                        VIGENTE = _obj.Vigente,

        //                    };
        //                    db2.ctr_roles_user.Add(resultado);
        //                    var Resultad = await db2.SaveChangesAsync();
        //                    db2.SaveChanges();
        //                    Resultado = Resultad;

        //                }
        //                else
        //                {
        //                    Resultado = Resul;
        //                }


        //            }
        //            catch (Exception ex) { }

        //            return Resultado;
        //        }
        //    }
        //}

        public async Task<int> InsUsuario(DtoUsuario _obj)
        {
            var Resultado = 0;
            DateTime ahoraUtc = DateTime.Now;
            string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");

            DtoUsuario? retorno = new DtoUsuario();
            var Usuarios = await _context.Ctr_usuarios
                .Where(x => x.Identificacion == Convert.ToString(_obj.Identificacion))
                .OrderBy(x => x.Identificacion)
                .ToListAsync();

            DtoUsuario? retornoRol = new DtoUsuario();
            var UsuariosRol = await _context.ctr_roles_user
                .Where(x => x.ID_ROL == Convert.ToInt32(_obj.IdRol) && x.ID_USUARIO == _obj.IdUsuario)
                .OrderBy(x => x.ID_ROL)
                .ToListAsync();

            if (Usuarios.Count > 0)
            {
                if (UsuariosRol.Count > 0)
                {
                    return -1; // El usuario ya tiene el rol asignado
                }

                // Crear un nuevo rol para el usuario
                int numero = 1;
                int valida = await _context.ctr_roles_user.CountAsync();
                if (valida > 0)
                {
                    numero = await _context.ctr_roles_user.MaxAsync(s => s.ID_USER_ROL) + 1;
                }

                var rolUsuario = new ctr_roles_user
                {
                    ID_USER_ROL = numero,
                    ID_USUARIO = _obj.IdUsuario,
                    ID_ROL = Convert.ToInt32(_obj.IdRol),
                    FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
                    USUARIO_CREACION = Convert.ToInt32(_obj.Usuario),
                    MAQUINA_CREACION = _obj.Maquina,
                    VIGENTE = _obj.Vigente,
                };

                _context.ctr_roles_user.Add(rolUsuario);
                var res = await _context.SaveChangesAsync();
                return res;
            }

            if (_obj.IdUsuario > 0)
            {
                // Actualizar el usuario existente
                var usuarioExistente = await _context.Ctr_usuarios.FindAsync(_obj.IdUsuario);
                if (usuarioExistente != null)
                {
                    usuarioExistente.Bloqueado = _obj.Bloqueado;
                    usuarioExistente.Identificacion = Convert.ToString(_obj.Identificacion);
                    usuarioExistente.Correo = _obj.Correo;
                    usuarioExistente.Fecha_creacion = Convert.ToDateTime(fechaHoraFormateada);
                    usuarioExistente.Maquina_creacion = _obj.Maquina;
                    usuarioExistente.Vigente = _obj.Vigente;
                    usuarioExistente.Fecha_Nacimiento = Convert.ToDateTime(_obj.FechaNacimiento);
                    usuarioExistente.Fecha_vigencia = Convert.ToDateTime(fechaHoraFormateada);
                    usuarioExistente.Apellidos = _obj.Apellidos;
                    usuarioExistente.Funcionario = _obj.Nombres;
                    usuarioExistente.Direccion_residencia = _obj.DireccionResidencia;
                    usuarioExistente.Cliente = _obj.Cliente;
                    usuarioExistente.Clave = _obj.Clave;
                    usuarioExistente.Celular = Convert.ToString(_obj.Celular);
                    usuarioExistente.Username = Convert.ToString(_obj.Identificacion);
                    usuarioExistente.Usuario_creacion = Convert.ToInt32(_obj.Usuario);

                    _context.Ctr_usuarios.Update(usuarioExistente);
                    var res = await _context.SaveChangesAsync();
                    if (res > 0)
                    {
                        int numero = 1;
                        int valida = await _context.ctr_roles_user.CountAsync();
                        if (valida > 0)
                        {
                            numero = await _context.ctr_roles_user.MaxAsync(s => s.ID_USER_ROL) + 1;
                        }

                        var rolUsuario = new ctr_roles_user
                        {
                            ID_USER_ROL = numero,
                            ID_USUARIO = _obj.IdUsuario,
                            ID_ROL = Convert.ToInt32(_obj.IdRol),
                            FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
                            USUARIO_CREACION = Convert.ToInt32(_obj.Usuario),
                            MAQUINA_CREACION = _obj.Maquina,
                            VIGENTE = _obj.Vigente,
                        };

                        _context.ctr_roles_user.Update(rolUsuario);
                        var res2 = await _context.SaveChangesAsync();
                        Resultado = -2;
                    }
                    else
                    {
                        Resultado = res;
                    }
                }

                return Resultado;
            }
            else
            {
                // Crear un nuevo usuario
                int UsuarioId = 1;
                int num = await _context.Ctr_usuarios.CountAsync();

                if (num > 0)
                {
                    UsuarioId = await _context.Ctr_usuarios.MaxAsync(x => x.IdUsuario) + 1;
                }

                var nuevoUsuario = new Ctr_usuarios
                {
                    IdUsuario = UsuarioId,
                    Bloqueado = _obj.Bloqueado,
                    Identificacion = Convert.ToString(_obj.Identificacion),
                    Correo = _obj.Correo,
                    Fecha_creacion = Convert.ToDateTime(fechaHoraFormateada),
                    Maquina_creacion = _obj.Maquina,
                    Vigente = _obj.Vigente,
                    Fecha_Nacimiento = Convert.ToDateTime(_obj.FechaNacimiento),
                    Fecha_vigencia = Convert.ToDateTime(fechaHoraFormateada),
                    Apellidos = _obj.Apellidos,
                    Funcionario = _obj.Nombres,
                    Direccion_residencia = _obj.DireccionResidencia,
                    Cliente = _obj.Cliente,
                    Clave = _obj.Clave,
                    Celular = Convert.ToString(_obj.Celular),
                    CODIGOCARGO = 1,
                    Username = Convert.ToString(_obj.Identificacion),
                    Usuario_creacion = Convert.ToInt32(_obj.Usuario)
                };

                _context.Ctr_usuarios.Add(nuevoUsuario);
                var res = await _context.SaveChangesAsync();

                if (res > 0)
                {
                    int numero = 1;
                    int valida = await _context.ctr_roles_user.CountAsync();
                    if (valida > 0)
                    {
                        numero = await _context.ctr_roles_user.MaxAsync(s => s.ID_USER_ROL) + 1;
                    }

                    var rolUsuario = new ctr_roles_user
                    {
                        ID_USER_ROL = numero,
                        ID_USUARIO = UsuarioId,
                        ID_ROL = Convert.ToInt32(_obj.IdRol),
                        FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
                        USUARIO_CREACION = Convert.ToInt32(_obj.Usuario),
                        MAQUINA_CREACION = _obj.Maquina,
                        VIGENTE = _obj.Vigente,
                    };

                    _context.ctr_roles_user.Add(rolUsuario);
                    var res2 = await _context.SaveChangesAsync();
                    return res2;
                }

                return res;
            }
        }


        //public async Task<int> UpdUsuario(DtoUsuario _obj)
        //{
        //    var Resultado = 0;
        //   DateTime ahoraUtc = DateTime.Now ;
        //    string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");

        //    DtoUsuario? retorno = new DtoUsuario();
        //    var Usuarios = _context.Ctr_usuarios
        //  .Where(x => x.Identificacion == Convert.ToString(_obj.Identificacion)).OrderBy(x => x.Identificacion)
        //        .ToList();

        //    if (_obj.IdUsuario > 0)
        //    {
        //        using (ModelContext db1 = new ModelContext())
        //        {

        //            try
        //            {
        //                Ctr_usuarios result = new Ctr_usuarios
        //                {
        //                    IdUsuario = _obj.IdUsuario,
        //                    Bloqueado = _obj.Bloqueado,
        //                    Identificacion = Convert.ToString(_obj.Identificacion),
        //                    Correo = _obj.Correo,
        //                    Fecha_modifica = Convert.ToDateTime(fechaHoraFormateada),
        //                    Maquina_modifica = _obj.Maquina,
        //                    Vigente = _obj.Vigente,
        //                    Fecha_Nacimiento = Convert.ToDateTime(_obj.FechaNacimiento),
        //                    Fecha_vigencia = Convert.ToDateTime(fechaHoraFormateada),
        //                    Apellidos = _obj.Apellidos,
        //                    Funcionario = _obj.Nombres,
        //                    Direccion_residencia = _obj.DireccionResidencia,
        //                    Cliente = _obj.Cliente,
        //                    Clave = _obj.Clave,
        //                    CODIGOCARGO = 1,
        //                    Celular = Convert.ToString(_obj.Celular),
        //                    Username = Convert.ToString(_obj.Identificacion),
        //                    Usuario_modifica = Convert.ToInt32(_obj.Usuario)
        //                };
        //                db1.Ctr_usuarios.Update(result);
        //                var Resul = await db1.SaveChangesAsync();
        //                db1.SaveChanges();
        //                if (Resul > 0)
        //                {

        //                    DtoUsuario? retornoRol = new DtoUsuario();
        //                    var UsuariosRol = _context.ctr_roles_user
        //                  .Where(x => x.ID_USUARIO == _obj.IdUsuario && x.VIGENTE == "SI").OrderBy(x => x.ID_ROL)
        //                       .ToList();


        //                    foreach (var ord in UsuariosRol)
        //                    {
        //                        ord.VIGENTE = _obj.Vigente;
        //                    }

        //                    var respuesta1 = _context.SaveChanges();
        //                    Resultado = 1;

        //                }
        //                else
        //                {
        //                    Resultado = Resul;
        //                }


        //            }
        //            catch (Exception ex) { }

        //            return Resultado;
        //        }

        //    }
        //    else
        //    {
        //        return 0;
        //    }

        //}

        public async Task<int> UpdUsuario(DtoUsuario _obj)
        {
            int Resultado = 0;
            DateTime ahoraUtc = DateTime.Now;
            string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");

            DtoUsuario? retorno = new DtoUsuario();
            var Usuarios = _context.Ctr_usuarios
                .Where(x => x.Identificacion == Convert.ToString(_obj.Identificacion))
                .OrderBy(x => x.Identificacion)
                .ToList();

            if (_obj.IdUsuario > 0)
            {
                try
                {
                    // Ya no necesitamos crear un nuevo ModelContext aquí, lo usamos directamente desde la inyección
                    Ctr_usuarios result = new Ctr_usuarios
                    {
                        IdUsuario = _obj.IdUsuario,
                        Bloqueado = _obj.Bloqueado,
                        Identificacion = Convert.ToString(_obj.Identificacion),
                        Correo = _obj.Correo,
                        Fecha_modifica = Convert.ToDateTime(fechaHoraFormateada),
                        Maquina_modifica = _obj.Maquina,
                        Vigente = _obj.Vigente,
                        Fecha_Nacimiento = Convert.ToDateTime(_obj.FechaNacimiento),
                        Fecha_vigencia = Convert.ToDateTime(fechaHoraFormateada),
                        Apellidos = _obj.Apellidos,
                        Funcionario = _obj.Nombres,
                        Direccion_residencia = _obj.DireccionResidencia,
                        Cliente = _obj.Cliente,
                        Clave = _obj.Clave,
                        CODIGOCARGO = 1,
                        Celular = Convert.ToString(_obj.Celular),
                        Username = Convert.ToString(_obj.Identificacion),
                        Usuario_modifica = Convert.ToInt32(_obj.Usuario)
                    };

                    _context.Ctr_usuarios.Update(result);  // Usamos _context, ya que está inyectado
                    var Resul = await _context.SaveChangesAsync();

                    if (Resul > 0)
                    {
                        var UsuariosRol = _context.ctr_roles_user
                            .Where(x => x.ID_USUARIO == _obj.IdUsuario && x.VIGENTE == "SI")
                            .OrderBy(x => x.ID_ROL)
                            .ToList();

                        foreach (var ord in UsuariosRol)
                        {
                            ord.VIGENTE = _obj.Vigente;
                        }

                        _context.SaveChanges();
                        Resultado = 1;
                    }
                    else
                    {
                        Resultado = Resul;
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones (log, etc.)
                }

                return Resultado;
            }
            else
            {
                return 0;
            }
        }
    
    //Consulta los correos De Comentarios Compras y en general
    public async Task<RespuestaDto<List<DtoDominios>>> GetCorreos()
        {
            try
            {
                var Respuesta = new RespuestaDto<List<DtoDominios>>();
                Respuesta.Codigo = EstadoOperacion.Bueno;
                Respuesta.Respuesta = new List<DtoDominios>();
                var resultado = _context.mi_compra_realizada.OrderBy(x => x.Correo_Electronico).Select(s => s.Correo_Electronico).Distinct().ToList();

                var resultado2 = _context.comentarios_clientes.Where(s => s.vigente == "SI").OrderBy(x => x.CorreoElectronico).Select(s => s.CorreoElectronico).Distinct().ToList();

                if (resultado.Count > 0)
                {
                    var contador = 0;
                    foreach (var obj in resultado)
                    {
                        contador++;
                        DtoDominios? retorno = new DtoDominios();
                        {
                            retorno.IdDominio = contador;
                            retorno.Descripcion = obj;

                        };
                        Respuesta.Respuesta.Add(retorno);
                    }

                    foreach (var obj in resultado2)
                    {
                        contador++;
                        DtoDominios? retorno = new DtoDominios();
                        {
                            retorno.IdDominio = contador;
                            retorno.Descripcion = obj;

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
        public async Task<RespuestaDto<List<DtoDominios>>> GetCorreosComentarios()
        {

            try
            {
                var Respuesta = new RespuestaDto<List<DtoDominios>>();
                Respuesta.Codigo = EstadoOperacion.Bueno;
                Respuesta.Respuesta = new List<DtoDominios>();
                var resultado = _context.comentarios_clientes.Where(s => s.vigente == "SI").OrderBy(x => x.CorreoElectronico).Select(s => s.CorreoElectronico).Distinct().ToList();

                if (resultado.Count > 0)
                {
                    var contador = 0;

                    foreach (var obj in resultado)
                    {
                        contador++;
                        DtoDominios? retorno = new DtoDominios();
                        {
                            retorno.IdDominio = contador;
                            retorno.Descripcion = obj;

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
        public async Task<RespuestaDto<List<DtoDominios>>> GetCorreosBandejaVentas()
        {
            try
            {
                var Respuesta = new RespuestaDto<List<DtoDominios>>();
                Respuesta.Codigo = EstadoOperacion.Bueno;
                Respuesta.Respuesta = new List<DtoDominios>();


                var resultado = _context.mi_compra_realizada.OrderBy(x => x.Correo_Electronico).Select(s => s.Correo_Electronico).Distinct().ToList();


                if (resultado.Count > 0)
                {
                    var contador = 0;
                    foreach (var obj in resultado)
                    {
                        contador++;
                        DtoDominios? retorno = new DtoDominios();
                        {
                            retorno.IdDominio = contador;
                            retorno.Descripcion = obj;

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
        #endregion
    }

}
