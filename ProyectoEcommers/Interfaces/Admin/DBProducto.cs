

namespace ProyectoEcommers.Models
{
 
    using Comun.Dto;
    using Comun.Enumeraciones;

    using FFImageLoading;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Formats.Jpeg;
    using SixLabors.ImageSharp.Processing;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using ImageSharpImage = SixLabors.ImageSharp.Image;

    public class DBProducto : IDbProducto
    {

        #region Propiedades
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly ModelContext _context;
        #endregion
        #region Constructor
        public DBProducto(IConfiguration configuration, ILogger<DbAdministracion> logger, ModelContext context)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }
        #endregion
        //public async Task<int> F_InsProducto(DtoProductos _producto)
        //{
        //    using (ModelContext db2 = new ModelContext())
        //    {
        //        try
        //        {
        //            DateTime ahoraUtc = DateTime.Now;
        //            //DateTime ahoraUtc = DateTime.UtcNow;
        //            string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");


        //            if (_producto.PRODUCTO_ID != 0)
        //            {

        //                db2.productos.Update(new productos
        //                {
        //                    PRODUCTO_ID = _producto.PRODUCTO_ID,
        //                    MARCA_ID = _producto.MARCA_ID,
        //                    CATEGORIA_ID = _producto.CATEGORIA_ID,
        //                    NOMBRE = _producto.NOMBRE,
        //                    DESCRIPCION = _producto.DESCRIPCION,
        //                    CANTIDAD = _producto.CANTIDAD,
        //                    CANTIDAD_TOTAL = _producto.CANTIDAD,
        //                    PRECIO = _producto.PRECIO,
        //                    VIGENTE = 1,
        //                    FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
        //                    MAQUINA_CREACION = _producto.MAQUINA_CREACION,
        //                    ACTUALIZADO_POR = _producto.ACTUALIZADO_POR,
        //                });
        //                if (await db2.SaveChangesAsync() > 0)
        //                {
        //                    foreach (var item in _producto.ListaImagenes)
        //                    {
        //                        byte[] imageBytes = Convert.FromBase64String(item.FOTO);
        //                        var folderPath = _configuration.GetSection("AppSettings").GetRequiredSection("ImagenProducto").Value;
        //                        ImageService imageService = new ImageService();
        //                        string filePath = Path.Combine(folderPath, item.FILENAME);
        //                        File.WriteAllBytes(filePath, imageBytes);

        //                        db2.imagenes_producto.Add(new imagenes_producto
        //                        {
        //                            PRODUCTO_ID = _producto.PRODUCTO_ID,
        //                            CONTENT_TYPE = item.CONTENT_TYPE,
        //                            FILENAME = item.FILENAME,
        //                            FOTO = null,
        //                            CREADO_POR = item.CREADO_POR,
        //                            FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
        //                            URL = item.URL,
        //                            VIGENTE = 1
        //                        });

        //                        await db2.SaveChangesAsync();
        //                    }
        //                    return 1;
        //                }
        //                else
        //                {
        //                    return 0;
        //                }
        //            }
        //            else
        //            {
        //                int productoId = 1;
        //                int num = db2.productos.Count();

        //                if (num > 0)
        //                {
        //                    productoId = db2.productos.Max(x => x.PRODUCTO_ID) + 1;
        //                }

        //                db2.productos.Add(new productos
        //                {
        //                    PRODUCTO_ID = productoId,
        //                    MARCA_ID = _producto.MARCA_ID,
        //                    CATEGORIA_ID = _producto.CATEGORIA_ID,
        //                    NOMBRE = _producto.NOMBRE,
        //                    DESCRIPCION = _producto.DESCRIPCION,
        //                    CANTIDAD = _producto.CANTIDAD,
        //                    CANTIDAD_TOTAL = _producto.CANTIDAD,
        //                    PRECIO = _producto.PRECIO,
        //                    VIGENTE = 1,
        //                    FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
        //                    MAQUINA_CREACION = _producto.MAQUINA_CREACION,
        //                    ACTUALIZADO_POR = _producto.ACTUALIZADO_POR,
        //                });

        //                if (await db2.SaveChangesAsync() > 0)
        //                {
        //                    foreach (var item in _producto.ListaImagenes)
        //                    {
        //                        byte[] imageBytes = Convert.FromBase64String(item.FOTO);
        //                        var folderPath = _configuration.GetSection("AppSettings").GetRequiredSection("ImagenProducto").Value;
        //                        ImageService imageService = new ImageService();
        //                        string filePath = Path.Combine(folderPath, item.FILENAME);
        //                        File.WriteAllBytes(filePath, imageBytes);

        //                        db2.imagenes_producto.Add(new imagenes_producto
        //                        {
        //                            PRODUCTO_ID = productoId,
        //                            CONTENT_TYPE = item.CONTENT_TYPE,
        //                            FILENAME = item.FILENAME,
        //                            FOTO = null,
        //                            CREADO_POR = item.CREADO_POR,
        //                            FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
        //                            URL = item.URL,
        //                            VIGENTE = 1
        //                        });

        //                        await db2.SaveChangesAsync();
        //                    }

        //                    return 1;
        //                }
        //                else
        //                {
        //                    return 0;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return 0;
        //        }
        //    }
        //}


        public async Task<int> F_InsProducto(DtoProductos _producto)
        {
            try
            {
                DateTime ahoraUtc = DateTime.Now;
                string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");

                if (_producto.PRODUCTO_ID != 0)
                {
                    // Actualizar producto existente
                    _context.productos.Update(new productos
                    {
                        PRODUCTO_ID = _producto.PRODUCTO_ID,
                        MARCA_ID = _producto.MARCA_ID,
                        CATEGORIA_ID = _producto.CATEGORIA_ID,
                        NOMBRE = _producto.NOMBRE,
                        DESCRIPCION = _producto.DESCRIPCION,
                        CANTIDAD = _producto.CANTIDAD,
                        CANTIDAD_TOTAL = _producto.CANTIDAD,
                        PRECIO = _producto.PRECIO,
                        VIGENTE = 1,
                        FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
                        MAQUINA_CREACION = _producto.MAQUINA_CREACION,
                        ACTUALIZADO_POR = _producto.ACTUALIZADO_POR,
                    });

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        foreach (var item in _producto.ListaImagenes)
                        {
                            byte[] imageBytes = Convert.FromBase64String(item.FOTO);
                            var folderPath = _configuration.GetSection("AppSettings").GetRequiredSection("ImagenProducto").Value;
                            ImageService imageService = new ImageService();
                            string filePath = Path.Combine(folderPath, item.FILENAME);
                            File.WriteAllBytes(filePath, imageBytes);

                            _context.imagenes_producto.Add(new imagenes_producto
                            {
                                PRODUCTO_ID = _producto.PRODUCTO_ID,
                                CONTENT_TYPE = item.CONTENT_TYPE,
                                FILENAME = item.FILENAME,
                                FOTO = null,
                                CREADO_POR = item.CREADO_POR,
                                FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
                                URL = item.URL,
                                VIGENTE = 1
                            });

                            await _context.SaveChangesAsync();
                        }
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    // Si no existe un producto (nuevo)
                    int productoId = 1;
                    int num = _context.productos.Count();

                    if (num > 0)
                    {
                        productoId = _context.productos.Max(x => x.PRODUCTO_ID) + 1;
                    }

                    _context.productos.Add(new productos
                    {
                        PRODUCTO_ID = productoId,
                        MARCA_ID = _producto.MARCA_ID,
                        CATEGORIA_ID = _producto.CATEGORIA_ID,
                        NOMBRE = _producto.NOMBRE,
                        DESCRIPCION = _producto.DESCRIPCION,
                        CANTIDAD = _producto.CANTIDAD,
                        CANTIDAD_TOTAL = _producto.CANTIDAD,
                        PRECIO = _producto.PRECIO,
                        VIGENTE = 1,
                        FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
                        MAQUINA_CREACION = _producto.MAQUINA_CREACION,
                        ACTUALIZADO_POR = _producto.ACTUALIZADO_POR,
                    });

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        foreach (var item in _producto.ListaImagenes)
                        {
                            byte[] imageBytes = Convert.FromBase64String(item.FOTO);
                            var folderPath = _configuration.GetSection("AppSettings").GetRequiredSection("ImagenProducto").Value;
                            ImageService imageService = new ImageService();
                            string filePath = Path.Combine(folderPath, item.FILENAME);
                            File.WriteAllBytes(filePath, imageBytes);

                            _context.imagenes_producto.Add(new imagenes_producto
                            {
                                PRODUCTO_ID = productoId,
                                CONTENT_TYPE = item.CONTENT_TYPE,
                                FILENAME = item.FILENAME,
                                FOTO = null,
                                CREADO_POR = item.CREADO_POR,
                                FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
                                URL = item.URL,
                                VIGENTE = 1
                            });

                            await _context.SaveChangesAsync();
                        }

                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                return 0;
            }
        }


        private async Task<int> ObtenerNuevoProductoIdAsync()
        {
            // Obtener un nuevo ID para el producto, incrementando el último ID
            var maxId = await _context.productos.MaxAsync(p => p.PRODUCTO_ID);
            return maxId + 1;
        }

        private async Task GuardarImagenesProducto(DtoProductos _producto, string fechaHoraFormateada)
        {
            var folderPath = _configuration.GetSection("AppSettings").GetRequiredSection("ImagenProducto").Value;

            foreach (var item in _producto.ListaImagenes)
            {
                // Convertir la foto de base64 a byte[]
                byte[] imageBytes = Convert.FromBase64String(item.FOTO);

                // Definir la ruta donde guardar la imagen
                string filePath = Path.Combine(folderPath, item.FILENAME);

                // Guardar la imagen en el sistema de archivos
                File.WriteAllBytes(filePath, imageBytes);

                // Crear la entrada en la base de datos para la imagen
                _context.imagenes_producto.Add(new imagenes_producto
                {
                    PRODUCTO_ID = _producto.PRODUCTO_ID,
                    CONTENT_TYPE = item.CONTENT_TYPE,
                    FILENAME = item.FILENAME,
                    FOTO = null, // No guardamos la imagen en la base de datos, solo la ruta
                    CREADO_POR = item.CREADO_POR,
                    FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
                    URL = item.URL,
                    VIGENTE = 1
                });
            }

            // Guardar todas las imágenes a la vez
            await _context.SaveChangesAsync();
        }

        public async Task<List<DtoProductos>> F_GetListaProductos(int marca, int categoria)
        {
            try
            {
                IQueryable<productos> consulta = _context.productos.Where(a => a.VIGENTE == 1 && a.CANTIDAD_TOTAL >= 1);
                if (marca > 0)
                {
                    consulta = consulta.Where(a => a.MARCA_ID == marca);
                }

                if (categoria > 0)
                {
                    consulta = consulta.Where(a => a.CATEGORIA_ID == categoria);
                }
                List<DtoProductos> ListaProductos = await consulta.Where(a => a.VIGENTE == 1 && a.CANTIDAD_TOTAL >= 1).Select(a => new DtoProductos
                {
                    PRODUCTO_ID = a.PRODUCTO_ID,
                    MARCA_ID = a.MARCA_ID,
                    CATEGORIA_ID = a.CATEGORIA_ID,
                    NOMBRE = a.NOMBRE,
                    DESCRIPCION = a.DESCRIPCION,
                    CANTIDAD = a.CANTIDAD,
                    CANTIDAD_TOTAL = a.CANTIDAD_TOTAL,
                    PRECIO = a.PRECIO,
                    VIGENTE = a.VIGENTE,
                    FECHA_CREACION = a.FECHA_CREACION,
                    MAQUINA_CREACION = a.MAQUINA_CREACION,
                    ACTUALIZADO_POR = a.ACTUALIZADO_POR,
                    Marca = _context.ctrl_dominios.OrderByDescending(s => s.ID_DOMINIO == a.MARCA_ID).First().DESCRIPCION,
                    Categoria = _context.ctrl_dominios.OrderByDescending(s => s.ID_DOMINIO == a.CATEGORIA_ID).First().DESCRIPCION,

                }).ToListAsync();

                foreach (var item in ListaProductos)
                {
                    item.ListaImagenes = _context.imagenes_producto.Where(a => a.PRODUCTO_ID == item.PRODUCTO_ID && a.VIGENTE == 1).Select(a => new DtoImagenesProducto
                    {
                        IMAGEN_ID = a.IMAGEN_ID,
                        PRODUCTO_ID = a.PRODUCTO_ID,
                        CONTENT_TYPE = a.CONTENT_TYPE,
                        FILENAME = a.FILENAME,
                        //FOTO = a.FOTO,
                        CREADO_POR = a.CREADO_POR,
                        FECHA_CREACION = a.FECHA_CREACION,
                        URL = a.URL,
                        VIGENTE = a.VIGENTE

                    }).ToList();
                }

                return ListaProductos;

            }
            catch (Exception e)
            {
                throw new Exception("Error " + e.Message);
            }
        }
        public async Task<List<DtoProductos>> F_GetProductosGrillaTotal()
        {
            try
            {
                List<DtoProductos> ListaProductos = await _context.productos.Select(a => new DtoProductos
                {
                    PRODUCTO_ID = a.PRODUCTO_ID,
                    MARCA_ID = a.MARCA_ID,
                    CATEGORIA_ID = a.CATEGORIA_ID,
                    NOMBRE = a.NOMBRE,
                    DESCRIPCION = a.DESCRIPCION,
                    CANTIDAD = a.CANTIDAD,
                    CANTIDAD_TOTAL = a.CANTIDAD_TOTAL,
                    PRECIO = a.PRECIO,
                    VIGENTE = a.VIGENTE,
                    FECHA_CREACION = a.FECHA_CREACION,
                    MAQUINA_CREACION = a.MAQUINA_CREACION,
                    ACTUALIZADO_POR = a.ACTUALIZADO_POR,
                    Marca = _context.ctrl_dominios.Where(s => s.ID_DOMINIO == a.MARCA_ID).FirstOrDefault().DESCRIPCION,
                    Categoria = _context.ctrl_dominios.Where(x => x.ID_DOMINIO == a.CATEGORIA_ID).FirstOrDefault().DESCRIPCION,

                }).ToListAsync();

                foreach (var item in ListaProductos)
                {
                    item.ListaImagenes = _context.imagenes_producto.Where(a => a.PRODUCTO_ID == item.PRODUCTO_ID && a.VIGENTE == 1).Select(a => new DtoImagenesProducto
                    {
                        IMAGEN_ID = a.IMAGEN_ID,
                        PRODUCTO_ID = a.PRODUCTO_ID,
                        CONTENT_TYPE = a.CONTENT_TYPE,
                        FILENAME = a.FILENAME,
                        CREADO_POR = a.CREADO_POR,
                        FECHA_CREACION = a.FECHA_CREACION,
                        URL = a.URL,
                        VIGENTE = a.VIGENTE

                    }).ToList();
                }

                return ListaProductos;
            }
            catch (Exception e)
            {
                throw new Exception("Error " + e.Message);
            }
        }
        public async Task<List<DtoProductos>> F_GetProductosGrilla()
        {
            try
            {
                List<DtoProductos> ListaProductos = await _context.productos.Where(a => a.VIGENTE == 1).Select(a => new DtoProductos
                {
                    PRODUCTO_ID = a.PRODUCTO_ID,
                    MARCA_ID = a.MARCA_ID,
                    CATEGORIA_ID = a.CATEGORIA_ID,
                    NOMBRE = a.NOMBRE,
                    DESCRIPCION = a.DESCRIPCION,
                    CANTIDAD = a.CANTIDAD,
                    CANTIDAD_TOTAL = a.CANTIDAD_TOTAL,
                    PRECIO = a.PRECIO,
                    VIGENTE = a.VIGENTE,
                    FECHA_CREACION = a.FECHA_CREACION,
                    MAQUINA_CREACION = a.MAQUINA_CREACION,
                    ACTUALIZADO_POR = a.ACTUALIZADO_POR,
                    Marca = _context.ctrl_dominios.Where(s => s.ID_DOMINIO == a.MARCA_ID).FirstOrDefault().DESCRIPCION,
                    Categoria = _context.ctrl_dominios.Where(x => x.ID_DOMINIO == a.CATEGORIA_ID).FirstOrDefault().DESCRIPCION,

                }).ToListAsync();

                foreach (var item in ListaProductos)
                {
                    item.ListaImagenes = _context.imagenes_producto.Where(a => a.PRODUCTO_ID == item.PRODUCTO_ID && a.VIGENTE == 1).Select(a => new DtoImagenesProducto
                    {
                        IMAGEN_ID = a.IMAGEN_ID,
                        PRODUCTO_ID = a.PRODUCTO_ID,
                        CONTENT_TYPE = a.CONTENT_TYPE,
                        FILENAME = a.FILENAME,
                        CREADO_POR = a.CREADO_POR,
                        FECHA_CREACION = a.FECHA_CREACION,
                        URL = a.URL,
                        VIGENTE = a.VIGENTE

                    }).ToList();
                }

                return ListaProductos;
            }
            catch (Exception e)
            {
                throw new Exception("Error " + e.Message);
            }
        }
        public async Task<List<DtoProductos>> F_GetProductosGrillaDesactivado()
        {
            try
            {
                List<DtoProductos> ListaProductos = await _context.productos.Where(a => a.VIGENTE == 0).Select(a => new DtoProductos
                {
                    PRODUCTO_ID = a.PRODUCTO_ID,
                    MARCA_ID = a.MARCA_ID,
                    CATEGORIA_ID = a.CATEGORIA_ID,
                    NOMBRE = a.NOMBRE,
                    DESCRIPCION = a.DESCRIPCION,
                    CANTIDAD = a.CANTIDAD,
                    CANTIDAD_TOTAL = a.CANTIDAD_TOTAL,
                    PRECIO = a.PRECIO,
                    VIGENTE = a.VIGENTE,
                    FECHA_CREACION = a.FECHA_CREACION,
                    MAQUINA_CREACION = a.MAQUINA_CREACION,
                    ACTUALIZADO_POR = a.ACTUALIZADO_POR,
                    Marca = _context.ctrl_dominios.Where(s => s.ID_DOMINIO == a.MARCA_ID).FirstOrDefault().DESCRIPCION,
                    Categoria = _context.ctrl_dominios.Where(x => x.ID_DOMINIO == a.CATEGORIA_ID).FirstOrDefault().DESCRIPCION,

                }).ToListAsync();

                foreach (var item in ListaProductos)
                {
                    item.ListaImagenes = _context.imagenes_producto.Where(a => a.PRODUCTO_ID == item.PRODUCTO_ID && a.VIGENTE == 1).Select(a => new DtoImagenesProducto
                    {
                        IMAGEN_ID = a.IMAGEN_ID,
                        PRODUCTO_ID = a.PRODUCTO_ID,
                        CONTENT_TYPE = a.CONTENT_TYPE,
                        FILENAME = a.FILENAME,
                        CREADO_POR = a.CREADO_POR,
                        FECHA_CREACION = a.FECHA_CREACION,
                        URL = a.URL,
                        VIGENTE = a.VIGENTE

                    }).ToList();
                }

                return ListaProductos;
            }
            catch (Exception e)
            {
                throw new Exception("Error " + e.Message);
            }
        }
        public async Task<List<DtoImagenesProducto>> F_GetImagenesGrilla(int _idProducto)
        {
            try
            {
                List<DtoImagenesProducto> ListaProductos = await _context.imagenes_producto.Where(a => a.PRODUCTO_ID == _idProducto && a.VIGENTE == 1).Select(a => new DtoImagenesProducto
                {
                    IMAGEN_ID = a.IMAGEN_ID,
                    PRODUCTO_ID = a.PRODUCTO_ID,
                    CONTENT_TYPE = a.CONTENT_TYPE,
                    FILENAME = a.FILENAME,
                    CREADO_POR = a.CREADO_POR,
                    FECHA_CREACION = Convert.ToDateTime(a.FECHA_CREACION),
                    URL = a.URL,
                    VIGENTE = a.VIGENTE
                }).ToListAsync();
                return ListaProductos;
            }
            catch (Exception e)
            {
                throw new Exception("Error " + e.Message);
            }
        }
        //public async Task<int> F_InsCompraProducto(DtoMiCompraRealizada _obj)
        //{
        //    DateTime ahoraUtc = DateTime.Now;
        //    string fechaHoraFormateada = ahoraUtc.ToString("dd/MM/yyyy HH:mm:ss");

        //    using (ModelContext db2 = new ModelContext())
        //    {
        //        int Resu = 0;
        //        int Resultado = 0;

        //        var verificaciones = await db2.mi_compra_realizada
        //                                      .AsNoTracking()
        //                                      .Where(x => x.id_compra_realizada == _obj.id_compra_realizada)
        //                                      .OrderByDescending(x => x.id_compra_realizada)
        //                                      .FirstOrDefaultAsync();

        //        if (verificaciones != null)
        //        {
        //            Resultado = await ActualizarCompra(db2, _obj, verificaciones.id_compra_realizada, fechaHoraFormateada);
        //        }
        //        else
        //        {
        //            Resultado = await CrearNuevaCompra(db2, _obj, fechaHoraFormateada);
        //        }

        //        return Resultado;
        //    }
        //}

        public async Task<int> F_InsCompraProducto(DtoMiCompraRealizada _obj)
        {
            try
            {

    
            // Obtener la fecha actual en formato deseado
            DateTime ahoraUtc = DateTime.Now;
            string fechaHoraFormateada = ahoraUtc.ToString("dd/MM/yyyy HH:mm:ss");

            int Resultado = 0;

            // Verificar si la compra ya existe en la base de datos
            var verificaciones = await _context.mi_compra_realizada
                .AsNoTracking()
                .Where(x => x.id_compra_realizada == _obj.id_compra_realizada)
                .OrderByDescending(x => x.id_compra_realizada)
                .FirstOrDefaultAsync();

            // Si la compra ya existe, actualizarla
            if (verificaciones != null)
            {
                Resultado = await ActualizarCompra(_context, _obj, verificaciones.id_compra_realizada, fechaHoraFormateada);
            }
            else
            {
                // Si la compra no existe, crear una nueva
                Resultado = await CrearNuevaCompra(_context, _obj, fechaHoraFormateada);
            }

            return Resultado;
            }
            catch (Exception ex)
            {
              
                throw;
            }
        }

        private async Task<int> ActualizarCompraMercadoLibre(ModelContext db2, string preferenceId, int NumeroCompra)
        {
            // Obtener el registro que corresponde a la compra
            var compra = await db2.mi_compra_realizada
                                  .FirstOrDefaultAsync(c => c.id_compra_realizada == NumeroCompra);

            if (compra == null)
            {
                // Si no se encuentra la compra, retornar 0 o alguna otra forma de manejo de error
                return 0;
            }

            // Actualizar solo los campos deseados
            compra.preferenceId_Consulta_Inicial = preferenceId;

            // Actualizar el estado solo de los campos modificados
            db2.Entry(compra).Property(c => c.preferenceId_Consulta_Inicial).IsModified = true;
            //db2.Entry(compra).Property(c => c.id_compra_realizada).IsModified = true;

            // Guardar los cambios en la base de datos
            int Resu = await db2.SaveChangesAsync();

            // Retornar el ID de la compra si se actualizó correctamente
            return Resu > 0 ? compra.id_compra_realizada : 0;
        }



        private async Task<int> ActualizarCompra(ModelContext db2, DtoMiCompraRealizada _obj, int ResulIdComprarealizada, string fechaHoraFormateada)
        {
            var Indicativo = db2.ctr_paises
                                 .Where(x => x.DESCRIPCION == _obj.Pais)
                                 .Select(x => x.INDICATIVO)
                                 .FirstOrDefault();
            var IndicativoId = Indicativo != null ? Indicativo + _obj.Celular : _obj.Celular;

            mi_compra_realizada res = new mi_compra_realizada
            {
                id_compra_realizada = ResulIdComprarealizada,
                Pais = _obj.Pais,
                Nombres = _obj.Nombres,
                Apellidos = _obj.Apellidos,
                Nombre_Empresa = _obj.Nombre_Empresa,
                Direccion = _obj.Direccion,
                Opcional_Direccion = _obj.Opcional_Direccion,
                Departamento = _obj.Departamento,
                Ciudad = _obj.Ciudad,
                Codigo_Postal = _obj.Codigo_Postal,
                Celular = _obj.Celular,
                Correo_Electronico = _obj.Correo_Electronico,
                Comentario = _obj.Comentario,
                Fecha_Creacion = Convert.ToDateTime(fechaHoraFormateada),
                Cancelada = "NO",
                CelularConIndicativo = IndicativoId,
                CostoEnvio = _obj.CostoEnvio,
                EntregaMercanciaLocal = _obj.EntregaMercanciaLocal,
            };

            db2.Entry(res).State = EntityState.Modified;
            int Resu = await db2.SaveChangesAsync();

            return Resu > 0 ? res.id_compra_realizada : 0;
        }

        private async Task<int> CrearNuevaCompra(ModelContext db2, DtoMiCompraRealizada _obj, string fechaHoraFormateada)
        {
            int productoId = db2.mi_compra_realizada.Any() ? db2.mi_compra_realizada.Max(x => x.id_compra_realizada) + 1 : 1;
            var Indicativo = db2.ctr_paises
                                 .Where(x => x.DESCRIPCION == _obj.Pais)
                                 .Select(x => x.INDICATIVO)
                                 .FirstOrDefault();
            var IndicativoId = Indicativo != null ? Indicativo + _obj.Celular : _obj.Celular;

            mi_compra_realizada res = new mi_compra_realizada
            {
                id_compra_realizada = productoId,
                Pais = _obj.Pais,
                Nombres = _obj.Nombres,
                Apellidos = _obj.Apellidos,
                Nombre_Empresa = _obj.Nombre_Empresa,
                Direccion = _obj.Direccion,
                Opcional_Direccion = _obj.Opcional_Direccion,
                Departamento = _obj.Departamento,
                Ciudad = _obj.Ciudad,
                Codigo_Postal = _obj.Codigo_Postal,
                Celular = _obj.Celular,
                Correo_Electronico = _obj.Correo_Electronico,
                Comentario = _obj.Comentario,
                Fecha_Creacion = Convert.ToDateTime(fechaHoraFormateada),
                Cancelada = "NO",
                CelularConIndicativo = IndicativoId,
                CostoEnvio = _obj.CostoEnvio,
                EntregaMercanciaLocal = _obj.EntregaMercanciaLocal
            };

            db2.mi_compra_realizada.Add(res);
            int Resu = await db2.SaveChangesAsync();

            return Resu > 0 ? res.id_compra_realizada : 0;
        }
        //public async Task<int> F_InsCompraProducto(DtoMiCompraRealizada _obj)
        //{

        //    DateTime ahoraUtc = DateTime.Now;
        //    string fechaHoraFormateada = ahoraUtc.ToString("dd/MM/yyyy HH:mm:ss");
        //    using (ModelContext db2 = new ModelContext())
        //    {
        //        var Resu = 0;
        //        var Resultado = 0;
        //        // Inicializa el resultado por defecto
        //        int ResulIdComprarealizada = 0;

        //        // Realiza la consulta y maneja el caso donde no hay resultados
        //        var verificaciones = db2.mi_compra_realizada
        //                                .Where(x => x.id_compra_realizada == _obj.id_compra_realizada)
        //                                .OrderByDescending(x => x.id_compra_realizada)
        //                                .FirstOrDefault();

        //        if (verificaciones == null)
        //        {
        //            ResulIdComprarealizada = 0;
        //        }
        //        else
        //        {
        //            ResulIdComprarealizada = _obj.id_compra_realizada;
        //        }
        //        if (ResulIdComprarealizada > 0)
        //        {


        //            var IndicativoId = "";
        //            var Indicativo = db2.ctr_paises.OrderByDescending(x => x.DESCRIPCION == _obj.Pais).First().INDICATIVO;
        //            if (Indicativo != null)
        //            {
        //                IndicativoId = Indicativo + _obj.Celular;
        //            }
        //            else
        //            {
        //                IndicativoId = _obj.Celular;
        //            }


        //            mi_compra_realizada res = new mi_compra_realizada
        //            {
        //                id_compra_realizada = ResulIdComprarealizada,
        //                Pais = _obj.Pais,
        //                Nombres = _obj.Nombres,
        //                Apellidos = _obj.Apellidos,
        //                Nombre_Empresa = _obj.Nombre_Empresa,
        //                Direccion = _obj.Direccion,
        //                Opcional_Direccion = _obj.Opcional_Direccion,
        //                Departamento = _obj.Departamento,
        //                Ciudad = _obj.Ciudad,
        //                Codigo_Postal = _obj.Codigo_Postal,
        //                Celular = _obj.Celular,
        //                Correo_Electronico = _obj.Correo_Electronico,
        //                Comentario = _obj.Comentario,
        //                Fecha_Creacion = Convert.ToDateTime(fechaHoraFormateada),
        //                Cancelada = "NO",
        //                CelularConIndicativo = IndicativoId,
        //                CostoEnvio = _obj.CostoEnvio,


        //            };

        //            db2.mi_compra_realizada.Update(res);
        //            Resu = await db2.SaveChangesAsync();
        //            db2.SaveChanges();
        //            Resultado = res.id_compra_realizada;


        //        }
        //        else {
        //        int productoId = 1;
        //        int num = db2.mi_compra_realizada.Count();

        //        if (num > 0)
        //        {
        //            productoId = db2.mi_compra_realizada.Max(x => x.id_compra_realizada) + 1;
        //        }

        //        var IndicativoId = "";
        //        var Indicativo = db2.ctr_paises.OrderByDescending(x => x.DESCRIPCION == _obj.Pais).First().INDICATIVO;
        //        if (Indicativo != null)
        //        {
        //            IndicativoId = Indicativo + _obj.Celular;
        //        }
        //        else
        //        {
        //            IndicativoId = _obj.Celular;
        //        }


        //        mi_compra_realizada res = new mi_compra_realizada
        //        {
        //            id_compra_realizada = productoId,
        //            Pais = _obj.Pais,
        //            Nombres = _obj.Nombres,
        //            Apellidos = _obj.Apellidos,
        //            Nombre_Empresa = _obj.Nombre_Empresa,
        //            Direccion = _obj.Direccion,
        //            Opcional_Direccion = _obj.Opcional_Direccion,
        //            Departamento = _obj.Departamento,
        //            Ciudad = _obj.Ciudad,
        //            Codigo_Postal = _obj.Codigo_Postal,
        //            Celular = _obj.Celular,
        //            Correo_Electronico = _obj.Correo_Electronico,
        //            Comentario = _obj.Comentario,
        //            Fecha_Creacion = Convert.ToDateTime(fechaHoraFormateada),
        //            Cancelada = "NO",
        //            CelularConIndicativo = IndicativoId,
        //            CostoEnvio = _obj.CostoEnvio,


        //        };

        //        db2.mi_compra_realizada.Add(res);
        //         Resu = await db2.SaveChangesAsync();
        //        db2.SaveChanges();
        //        Resultado = res.id_compra_realizada;
        //        }
        //        if (Resu > 0)
        //        {
        //            return Resultado;
        //        }
        //        else
        //        {
        //            return Resultado;
        //        };



        //    }
        //}
        //public async Task<int> F_InsCarrito(List<DtoMiCarrito> _producto)
        //{
        //    DateTime ahoraUtc = DateTime.Now;
        //    string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");
        //    using (ModelContext db2 = new ModelContext())
        //    {

        //        try

        //        {
        //            var Resu = 0;

        //            // Eliminar productos existentes en el carrito
        //            var compraRealizadaIds = _producto.Select(p => p.id_compra_realizada).Distinct().ToList();
        //            var productosExistentes = await db2.mi_carrito
        //                                                .Where(x => compraRealizadaIds.Contains(x.id_compra_realizada))
        //                                                .ToListAsync();

        //            if (productosExistentes.Any())
        //            {
        //                foreach (var item in productosExistentes)
        //                {
        //                    var produc = db2.productos.Where(x => x.PRODUCTO_ID == item.id_producto).First();
        //                    if (produc != null)
        //                    {
        //                        produc.CANTIDAD_TOTAL = (produc.CANTIDAD_TOTAL + item.Cantidad);
        //                    }
        //                }
        //                db2.mi_carrito.RemoveRange(productosExistentes);
        //                await db2.SaveChangesAsync();
        //            }





        //            foreach (var item in _producto)
        //            {
        //                int productoId1 = 1;
        //                var maxIdCarrito = db2.mi_carrito.Select(x => (int?)x.id_carrito).Max(); // Obtener el máximo id_carrito o null si la colección está vacía
        //                if (maxIdCarrito.HasValue)
        //                {
        //                    productoId1 = maxIdCarrito.Value + 1; // Incrementar el máximo id_carrito encontrado
        //                }
        //                //int productoId1 = 1;
        //                //int num1 = db2.mi_carrito.Count();
        //                //if (num1 > 0)
        //                //{
        //                //    productoId1 = db2.mi_carrito.Max(x => x.id_carrito) + 1;
        //                //}

        //                db2.mi_carrito.Add(new mi_carrito
        //                {
        //                    id_carrito = productoId1,
        //                    id_compra_realizada = item.id_compra_realizada,
        //                    id_producto = Convert.ToInt32(item.id_producto),
        //                    Descripcion_Producto = item.Descripcion_Producto,
        //                    Cantidad = Convert.ToInt32(item.Cantidad),
        //                    precio_Unitario = Convert.ToInt32(item.precio_Unitario),
        //                    Precio_Total = Convert.ToInt32(item.Precio_Total),
        //                    Precio_Envio = item.Precio_Envio ?? 0,
        //                    Descuento = Convert.ToInt32(item.Descuento),
        //                    Cancelado = item.Cancelado,

        //                });
        //                //var produc = db2.productos.Where(x => x.PRODUCTO_ID == item.id_producto).First();
        //                //produc.CANTIDAD_TOTAL = (produc.CANTIDAD_TOTAL - item.Cantidad);

        //              //var produc = db2.productos.Where(x => x.PRODUCTO_ID == item.id_producto).First();
        //              //produc.CANTIDAD_TOTAL = (produc.CANTIDAD_TOTAL - item.Cantidad);

        //                var produc = await db2.productos.FirstOrDefaultAsync(x => x.PRODUCTO_ID == item.id_producto);
        //                if (produc != null)
        //                {
        //                    produc.CANTIDAD_TOTAL = (produc.CANTIDAD_TOTAL - item.Cantidad);
        //                }

        //                Resu = await db2.SaveChangesAsync();

        //            }
        //            if (Resu > 0)
        //            {
        //                return Resu;
        //            }
        //            else
        //            {
        //                return Resu;
        //            };
        //        }
        //        catch (DbUpdateException ex)
        //        {
        //            Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
        //            throw;
        //        }

        //    }



        //}


        public async Task<int> F_InsCarrito(List<DtoMiCarrito> _producto)
        {
            int resultado = 0;
            DateTime ahoraUtc = DateTime.Now;
            string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");

            // Comienza una transacción para asegurar que todo se realice de forma atómica
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Eliminar productos existentes en el carrito que coincidan con las compras realizadas
                    var compraRealizadaIds = _producto.Select(p => p.id_compra_realizada).Distinct().ToList();
                    var productosExistentes = await _context.mi_carrito
                                                            .Where(x => compraRealizadaIds.Contains(x.id_compra_realizada))
                                                            .ToListAsync();

                    if (productosExistentes.Any())
                    {
                        // Actualizar la cantidad de productos en inventario
                        foreach (var item in productosExistentes)
                        {
                            var producto = await _context.productos
                                                         .FirstOrDefaultAsync(x => x.PRODUCTO_ID == item.id_producto);

                            if (producto != null)
                            {
                                producto.CANTIDAD_TOTAL += item.Cantidad;  // Incrementar la cantidad del producto
                            }
                        }

                        // Eliminar los productos ya existentes en el carrito
                        _context.mi_carrito.RemoveRange(productosExistentes);
                        await _context.SaveChangesAsync();  // Guardar cambios en la base de datos
                    }

                    // Añadir productos nuevos al carrito y actualizar cantidades en inventario
                    foreach (var item in _producto)
                    {
                        int idCarrito = 1;
                        var maxIdCarrito = await _context.mi_carrito
                            .Select(x => (int?)x.id_carrito)
                            .MaxAsync(); // Obtener el ID máximo del carrito

                        if (maxIdCarrito.HasValue)
                        {
                            idCarrito = maxIdCarrito.Value + 1;  // Incrementar el máximo ID
                        }

                        // Crear un nuevo producto en el carrito
                        _context.mi_carrito.Add(new mi_carrito
                        {
                            id_carrito = idCarrito,
                            id_compra_realizada = item.id_compra_realizada,
                            id_producto = Convert.ToInt32(item.id_producto),
                            Descripcion_Producto = item.Descripcion_Producto,
                            Cantidad = Convert.ToInt32(item.Cantidad),
                            precio_Unitario = Convert.ToInt32(item.precio_Unitario),
                            Precio_Total = Convert.ToInt32(item.Precio_Total),
                            Precio_Envio = item.Precio_Envio ?? 0,
                            Descuento = Convert.ToInt32(item.Descuento),
                            Cancelado = item.Cancelado,
                        });

                        // Actualizar las cantidades de los productos en inventario
                        var producto = await _context.productos
                            .FirstOrDefaultAsync(x => x.PRODUCTO_ID == item.id_producto);

                        if (producto != null)
                        {
                            producto.CANTIDAD_TOTAL -= item.Cantidad;  // Decrementar la cantidad del producto
                        }

                        // Guardar los cambios en la base de datos
                        resultado = await _context.SaveChangesAsync();
                    }

                    // Si todo salió bien, confirmar la transacción
                    if (resultado > 0)
                    {
                        await transaction.CommitAsync();
                        return resultado;  // Retornar el resultado si es positivo
                    }
                    else
                    {
                        await transaction.RollbackAsync();  // Si algo salió mal, revertir la transacción
                        return 0;  // Retornar 0 si no se guardó ningún cambio
                    }
                }
                catch (DbUpdateException ex)
                {
                    // Manejo de excepciones en caso de que ocurra un error al actualizar la base de datos
                    Console.WriteLine($"Error: {ex.Message}");
                    await transaction.RollbackAsync();  // Revertir la transacción si ocurre un error
                    throw;  // Re-lanzar la excepción para ser manejada en otro lugar
                }
            }
        }

        
        public async Task<int> InsProductosMercadoLibre(string preferenceId, int NumeroCompra)
        {
            // Obtener la fecha actual en formato deseado
            DateTime ahoraUtc = DateTime.Now;
            string fechaHoraFormateada = ahoraUtc.ToString("dd/MM/yyyy HH:mm:ss");

            int Resultado = 0;

            // Verificar si la compra ya existe en la base de datos
            var verificaciones = await _context.mi_compra_realizada
                .AsNoTracking()
                .Where(x => x.id_compra_realizada == NumeroCompra)
                .OrderByDescending(x => x.id_compra_realizada)
                .FirstOrDefaultAsync();

            // Si la compra ya existe, actualizarla
            if (verificaciones != null)
            {
                Resultado = await ActualizarCompraMercadoLibre(_context, preferenceId, NumeroCompra);
            }
         

            return Resultado;

        }


        //public async Task<int> F_InsCarritoC(List<DtoMiCarrito> _producto)
        //{
        //    DateTime ahoraUtc = DateTime.Now;
        //    string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");

        //    using (ModelContext db2 = new ModelContext())
        //    using (var transaction = await db2.Database.BeginTransactionAsync())
        //    {
        //        try
        //        {
        //            var Resu = 0;

        //            // Eliminar productos existentes en el carrito
        //            var compraRealizadaIds = _producto.Select(p => p.id_compra_realizada).Distinct().ToList();
        //            var productosExistentes = await db2.mi_carrito
        //                                                .Where(x => compraRealizadaIds.Contains(x.id_compra_realizada))
        //                                                .ToListAsync();

        //            if (productosExistentes.Any())
        //            {
        //                db2.mi_carrito.RemoveRange(productosExistentes);
        //                await db2.SaveChangesAsync();
        //            }

        //            // Añadir productos nuevos al carrito
        //            foreach (var item in _producto)
        //            {
        //                // Generar un nuevo id_carrito único
        //                int productoId1 = 1;
        //                var maxIdCarrito = db2.mi_carrito.Select(x => (int?)x.id_carrito).Max(); // Obtener el máximo id_carrito o null si la colección está vacía
        //                if (maxIdCarrito.HasValue)
        //                {
        //                    productoId1 = maxIdCarrito.Value + 1; // Incrementar el máximo id_carrito encontrado
        //                }

        //                var nuevoCarrito = new mi_carrito
        //                {
        //                    id_carrito = productoId1,
        //                    id_compra_realizada = item.id_compra_realizada,
        //                    id_producto = Convert.ToInt32(item.id_producto),
        //                    Descripcion_Producto = item.Descripcion_Producto,
        //                    Cantidad = Convert.ToInt32(item.Cantidad),
        //                    precio_Unitario = Convert.ToInt32(item.precio_Unitario),
        //                    Precio_Total = Convert.ToInt32(item.Precio_Total),
        //                    Precio_Envio = Convert.ToInt32(item.Precio_Envio),
        //                    Descuento = Convert.ToInt32(item.Descuento),
        //                    Cancelado = item.Cancelado
        //                };

        //                db2.mi_carrito.Add(nuevoCarrito);
        //            }

        //            Resu = await db2.SaveChangesAsync();
        //            await transaction.CommitAsync();

        //            return Resu;
        //        }
        //        catch (Exception ex)
        //        {
        //            await transaction.RollbackAsync();
        //            // Aquí puedes registrar el error o hacer alguna acción adicional
        //            throw new Exception("Error al procesar el carrito de compras", ex);
        //        }
        //    }
        //}

        public async Task<int> F_InsCarritoC(List<DtoMiCarrito> _producto)
        {
            DateTime ahoraUtc = DateTime.Now;
            string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");

            // Iniciamos una transacción
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    int Resu = 0;

                    // Eliminar productos existentes en el carrito para la compra realizada
                    var compraRealizadaIds = _producto.Select(p => p.id_compra_realizada).Distinct().ToList();
                    var productosExistentes = await _context.mi_carrito
                        .Where(x => compraRealizadaIds.Contains(x.id_compra_realizada))
                        .ToListAsync();

                    // Si existen productos en el carrito, los eliminamos
                    if (productosExistentes.Any())
                    {
                        _context.mi_carrito.RemoveRange(productosExistentes);
                        await _context.SaveChangesAsync();
                    }

                    // Añadir productos nuevos al carrito
                    foreach (var item in _producto)
                    {
                        // Generar un nuevo id_carrito único
                        int productoId1 = 1;
                        var maxIdCarrito = await _context.mi_carrito
                            .Select(x => (int?)x.id_carrito)
                            .MaxAsync(); // Obtener el máximo id_carrito o null si la colección está vacía

                        if (maxIdCarrito.HasValue)
                        {
                            productoId1 = maxIdCarrito.Value + 1; // Incrementar el máximo id_carrito encontrado
                        }

                        // Crear el nuevo producto para el carrito
                        var nuevoCarrito = new mi_carrito
                        {
                            id_carrito = productoId1,
                            id_compra_realizada = item.id_compra_realizada,
                            id_producto = Convert.ToInt32(item.id_producto),
                            Descripcion_Producto = item.Descripcion_Producto,
                            Cantidad = Convert.ToInt32(item.Cantidad),
                            precio_Unitario = Convert.ToInt32(item.precio_Unitario),
                            Precio_Total = Convert.ToInt32(item.Precio_Total),
                            Precio_Envio = Convert.ToInt32(item.Precio_Envio),
                            Descuento = Convert.ToInt32(item.Descuento),
                            Cancelado = item.Cancelado
                        };

                        // Añadir el nuevo carrito al contexto
                        _context.mi_carrito.Add(nuevoCarrito);
                    }

                    // Guardar los cambios en la base de datos
                    Resu = await _context.SaveChangesAsync();

                    // Si todo fue exitoso, confirmamos la transacción
                    await transaction.CommitAsync();

                    return Resu; // Retornamos el resultado de la operación
                }
                catch (Exception ex)
                {
                    // En caso de error, revertimos la transacción
                    await transaction.RollbackAsync();
                    // Puedes registrar el error o hacer alguna acción adicional
                    throw new Exception("Error al procesar el carrito de compras", ex);
                }
            }
        }
        public async Task<RespuestaDto<List<DtoProductos>>> F_UpdateProducto(DtoProductos Producto)

        {
            var respuesta = 0;
            RespuestaDto<List<DtoProductos>> retorno = new RespuestaDto<List<DtoProductos>>();

            var query = from ord in _context.productos
                        where ord.PRODUCTO_ID == Producto.PRODUCTO_ID
                        select ord;

            foreach (var ord in query)
            {
                ord.VIGENTE = Producto.VIGENTE;
            }

            try
            {
                respuesta = await _context.SaveChangesAsync();

                if (respuesta > 0)
                {
                    retorno.Codigo = EstadoOperacion.Bueno;
                }
                else
                {
                    retorno.Codigo = EstadoOperacion.Malo;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return retorno;
        }
        public async Task<RespuestaDto<List<DtoImagenesProducto>>> F_UpdateImagenProducto(DtoImagenesProducto Producto)

        {
            var respuesta = 0;
            RespuestaDto<List<DtoImagenesProducto>> retorno = new RespuestaDto<List<DtoImagenesProducto>>();

            var query = from ord in _context.imagenes_producto
                        where ord.IMAGEN_ID == Convert.ToInt32(Producto.ImagenId)
                        select ord;

            foreach (var ord in query)
            {
                ord.VIGENTE = Producto.VIGENTE;
            }

            try
            {
                respuesta = _context.SaveChanges();
                if (respuesta > 0)
                {
                    retorno.Codigo = EstadoOperacion.Bueno;
                }
                else
                {
                    retorno.Codigo = EstadoOperacion.Malo;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return retorno;
        }
        public async Task<List<DtoProductos>> F_GetBuscaIntelEmple(string search)
        {
            try
            {
                List<DtoProductos> ListaProductos = await _context.productos.Where(a => a.VIGENTE == 1 && a.CANTIDAD_TOTAL >= 1 &&
                (a.NOMBRE.ToLower().Contains(search.ToLower()) || a.DESCRIPCION.ToLower().Contains(search.ToLower()) ||
                a.FK_MARCA.DESCRIPCION.ToLower().Contains(search.ToLower()) || a.FK_CATEGORIA.DESCRIPCION.ToLower().Contains(search.ToLower())))
                    .Select(a => new DtoProductos
                    {
                        PRODUCTO_ID = a.PRODUCTO_ID,
                        MARCA_ID = a.MARCA_ID,
                        CATEGORIA_ID = a.CATEGORIA_ID,
                        NOMBRE = a.NOMBRE,
                        DESCRIPCION = a.DESCRIPCION,
                        CANTIDAD = a.CANTIDAD,
                        CANTIDAD_TOTAL = a.CANTIDAD_TOTAL,
                        PRECIO = a.PRECIO,
                        VIGENTE = a.VIGENTE,
                        FECHA_CREACION = a.FECHA_CREACION,
                        MAQUINA_CREACION = a.MAQUINA_CREACION,
                        ACTUALIZADO_POR = a.ACTUALIZADO_POR,
                        Marca = _context.ctrl_dominios.OrderByDescending(s => s.ID_DOMINIO == a.MARCA_ID).First().DESCRIPCION,
                        Categoria = _context.ctrl_dominios.OrderByDescending(s => s.ID_DOMINIO == a.CATEGORIA_ID).First().DESCRIPCION,

                    }).ToListAsync();

                foreach (var item in ListaProductos)
                {
                    item.ListaImagenes = _context.imagenes_producto.Where(a => a.PRODUCTO_ID == item.PRODUCTO_ID && a.VIGENTE == 1).Select(a => new DtoImagenesProducto
                    {
                        IMAGEN_ID = a.IMAGEN_ID,
                        PRODUCTO_ID = a.PRODUCTO_ID,
                        CONTENT_TYPE = a.CONTENT_TYPE,
                        FILENAME = a.FILENAME,
                        //FOTO = a.FOTO,
                        CREADO_POR = a.CREADO_POR,
                        FECHA_CREACION = a.FECHA_CREACION,
                        URL = a.URL,
                        VIGENTE = a.VIGENTE

                    }).ToList();
                }
                return ListaProductos;
            }
            catch (Exception e)
            {
                throw new Exception("Error " + e.Message);
            }
        }
        public async Task<List<DtoMiCompraRealizada>> F_GetProductosMisCompras(string correo)
        {
            try
            {
                //IQueryable<mi_compra_realizada> consulta = _context.mi_compra_realizada.Where(a => a.Correo_Electronico == correo);
                //if (consulta != null)
                //{

                List<DtoMiCompraRealizada> ListaMiCompra = await _context.mi_compra_realizada.Where(x => x.Correo_Electronico == correo).Select(a => new DtoMiCompraRealizada
                {
                    id_compra_realizada = a.id_compra_realizada,
                    Pais = a.Pais,
                    Nombres = a.Nombres,
                    Apellidos = a.Apellidos,
                    Nombre_Empresa = a.Nombre_Empresa,
                    Direccion = a.Direccion,
                    Opcional_Direccion = a.Opcional_Direccion,
                    Departamento = a.Departamento,
                    Ciudad = a.Ciudad,
                    Codigo_Postal = a.Codigo_Postal,
                    Celular = a.Celular,
                    Correo_Electronico = a.Correo_Electronico,
                    Comentario = a.Comentario,
                    Fecha_Creacion = a.Fecha_Creacion,
                    Vigente = a.Vigente,
                    Cancelada = a.Cancelada,
                    Enviado_Satisfactoriamente = a.Enviado_Satisfactoriamente,
                    Empresa_Entrega = a.Empresa_Entrega,
                    Fecha_Envio_producto = a.Fecha_Envio_producto,
                    Fecha_Llegada_producto = a.Fecha_Llegada_producto,
                    Estado_producto = a.Estado_producto,
                    Identifacion_modifica = a.Identifacion_modifica,
                    //Producto = _context.productos.OrderByDescending(x => x.id_compra_realizada == a.id_compra_realizada).First().pr.ToString(),,
                    Cantidad = _context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Sum(s => s.Cantidad).ToString(),
                    CostoEnvio = a.CostoEnvio,
                    PrecioTotal = _context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Sum(s => s.Precio_Total).ToString(),
                    PrecioFinal = (Convert.ToDecimal(_context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Sum(s => s.Precio_Total).ToString()) + Convert.ToDecimal(a.CostoEnvio))

                }).ToListAsync();

                //CostoEnvio = _context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Max(s => s.Precio_Envio).ToString(),
                foreach (var item in ListaMiCompra)
                {
                    item.ListaMisCompras = _context.mi_carrito.Where(a => a.id_compra_realizada == item.id_compra_realizada && a.Cancelado == "NO").Select(a => new DtoMiCarrito
                    {
                        id_carrito = a.id_carrito,
                        id_compra_realizada = a.id_compra_realizada,
                        id_producto = a.id_producto,
                        Descripcion_Producto = a.Descripcion_Producto,
                        Producto = _context.productos.OrderByDescending(x => x.PRODUCTO_ID == a.id_producto).First().NOMBRE,
                        Cantidad = a.Cantidad,
                        precio_Unitario = a.precio_Unitario,
                        Precio_Envio = a.Precio_Envio,
                        Precio_Total = a.Precio_Total,
                        Descuento = a.Descuento,
                        Cancelado = a.Cancelado,
                        Marca = _context.ctrl_dominios.OrderByDescending(s => s.ID_DOMINIO == _context.productos.OrderByDescending(x => x.PRODUCTO_ID == a.id_producto).First().MARCA_ID).First().DESCRIPCION,
                        Categoria = _context.ctrl_dominios.OrderByDescending(s => s.ID_DOMINIO == _context.productos.OrderByDescending(x => x.PRODUCTO_ID == a.id_producto).First().CATEGORIA_ID).First().DESCRIPCION,



                    }).ToList();

                    foreach (var item2 in item.ListaMisCompras)
                    {
                        item2.ListaImagenesC = _context.imagenes_producto.Where(z => z.PRODUCTO_ID == item2.id_producto && z.VIGENTE == 1).Select(a => new DtoImagenesProducto
                        {
                            IMAGEN_ID = a.IMAGEN_ID,
                            PRODUCTO_ID = a.PRODUCTO_ID,
                            CONTENT_TYPE = a.CONTENT_TYPE,
                            FILENAME = a.FILENAME,
                            //FOTO = a.FOTO,
                            CREADO_POR = a.CREADO_POR,
                            FECHA_CREACION = a.FECHA_CREACION,
                            URL = a.URL,
                            VIGENTE = a.VIGENTE

                        }).ToList();
                    }
                }
                //foreach (var item2 in ListaMiCompra)
                //{
                //    item2.ListaImagenesC = _context.imagenes_producto.Where(a => a.PRODUCTO_ID == item2.id_producto && a.VIGENTE == 1).Select(a => new DtoImagenesProducto
                //    {
                //        IMAGEN_ID = a.IMAGEN_ID,
                //        PRODUCTO_ID = a.PRODUCTO_ID,
                //        CONTENT_TYPE = a.CONTENT_TYPE,
                //        FILENAME = a.FILENAME,
                //        //FOTO = a.FOTO,
                //        CREADO_POR = a.CREADO_POR,
                //        FECHA_CREACION = a.FECHA_CREACION,
                //        URL = a.URL,
                //        VIGENTE = a.VIGENTE

                //    }).ToList();
                //}

                return ListaMiCompra;
                //}
                //else
                //{
                //    return null;
                //}

            }
            catch (Exception e)
            {
                throw new Exception("Error " + e.Message);
            }
        }
        public async Task<List<DtoMiCompraRealizada>> F_GetProductosMisComprasId(Int32 idCompra)
        {
            try
            {
                //IQueryable<mi_compra_realizada> consulta = _context.mi_compra_realizada.Where(a => a.Correo_Electronico == correo);
                //if (consulta != null)
                //{

                List<DtoMiCompraRealizada> ListaMiCompra = await _context.mi_compra_realizada.Where(x => x.id_compra_realizada == idCompra).Select(a => new DtoMiCompraRealizada
                {
                    id_compra_realizada = a.id_compra_realizada,
                    Pais = a.Pais,
                    Nombres = a.Nombres,
                    Apellidos = a.Apellidos,
                    Nombre_Empresa = a.Nombre_Empresa,
                    Direccion = a.Direccion,
                    Opcional_Direccion = a.Opcional_Direccion,
                    Departamento = a.Departamento,
                    Ciudad = a.Ciudad,
                    Codigo_Postal = a.Codigo_Postal,
                    Celular = a.Celular,
                    Correo_Electronico = a.Correo_Electronico,
                    Comentario = a.Comentario,
                    Fecha_Creacion = a.Fecha_Creacion,
                    Vigente = a.Vigente,
                    Cancelada = a.Cancelada,
                    Enviado_Satisfactoriamente = a.Enviado_Satisfactoriamente,
                    Empresa_Entrega = a.Empresa_Entrega,
                    Fecha_Envio_producto = a.Fecha_Envio_producto,
                    Fecha_Llegada_producto = a.Fecha_Llegada_producto,
                    Estado_producto = a.Estado_producto,
                    Identifacion_modifica = a.Identifacion_modifica,
                    //Producto = _context.productos.OrderByDescending(x => x.id_compra_realizada == a.id_compra_realizada).First().pr.ToString(),,
                    Cantidad = _context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Sum(s => s.Cantidad).ToString(),
                    CostoEnvio = a.CostoEnvio,
                    PrecioTotal = _context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Sum(s => s.Precio_Total).ToString(),
                    PrecioFinal = (Convert.ToDecimal(_context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Sum(s => s.Precio_Total).ToString()) + Convert.ToDecimal(a.CostoEnvio)),
                    preferenceId = a.preferenceId,
                    merchant_order_id = a.merchant_order_id,
                    status = a.status
                    

                }).ToListAsync();

                //CostoEnvio = _context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Max(s => s.Precio_Envio).ToString(),
                foreach (var item in ListaMiCompra)
                {
                    item.ListaMisCompras = _context.mi_carrito.Where(a => a.id_compra_realizada == item.id_compra_realizada && a.Cancelado == "NO").Select(a => new DtoMiCarrito
                    {
                        id_carrito = a.id_carrito,
                        id_compra_realizada = a.id_compra_realizada,
                        id_producto = a.id_producto,
                        Descripcion_Producto = a.Descripcion_Producto,
                        Producto = _context.productos.OrderByDescending(x => x.PRODUCTO_ID == a.id_producto).First().NOMBRE,
                        Cantidad = a.Cantidad,
                        precio_Unitario = a.precio_Unitario,
                        Precio_Envio = a.Precio_Envio,
                        Precio_Total = a.Precio_Total,
                        Descuento = a.Descuento,
                        Cancelado = a.Cancelado,


                    }).ToList();

                    foreach (var item2 in item.ListaMisCompras)
                    {
                        item2.ListaImagenesC = _context.imagenes_producto.Where(z => z.PRODUCTO_ID == item2.id_producto).Select(a => new DtoImagenesProducto
                        {
                            IMAGEN_ID = a.IMAGEN_ID,
                            PRODUCTO_ID = a.PRODUCTO_ID,
                            CONTENT_TYPE = a.CONTENT_TYPE,
                            FILENAME = a.FILENAME,
                            //FOTO = a.FOTO,
                            CREADO_POR = a.CREADO_POR,
                            FECHA_CREACION = a.FECHA_CREACION,
                            URL = a.URL,
                            VIGENTE = a.VIGENTE

                        }).ToList();
                    }
                }
                //foreach (var item2 in ListaMiCompra)
                //{
                //    item2.ListaImagenesC = _context.imagenes_producto.Where(a => a.PRODUCTO_ID == item2.id_producto && a.VIGENTE == 1).Select(a => new DtoImagenesProducto
                //    {
                //        IMAGEN_ID = a.IMAGEN_ID,
                //        PRODUCTO_ID = a.PRODUCTO_ID,
                //        CONTENT_TYPE = a.CONTENT_TYPE,
                //        FILENAME = a.FILENAME,
                //        //FOTO = a.FOTO,
                //        CREADO_POR = a.CREADO_POR,
                //        FECHA_CREACION = a.FECHA_CREACION,
                //        URL = a.URL,
                //        VIGENTE = a.VIGENTE

                //    }).ToList();
                //}

                return ListaMiCompra;
                //}
                //else
                //{
                //    return null;
                //}

            }
            catch (Exception e)
            {
                throw new Exception("Error " + e.Message);
            }
        }        
        public async Task<List<DtoMiCompraRealizada>> F_GetProductosVentasFactura(int Id_compra)
        {
            try
            {
                List<DtoMiCompraRealizada> ListaMiCompra = await _context.mi_compra_realizada.Where(x => x.id_compra_realizada == Id_compra).Select(a => new DtoMiCompraRealizada
                {
                    id_compra_realizada = a.id_compra_realizada,
                    Pais = a.Pais,
                    Nombres = a.Nombres,
                    Apellidos = a.Apellidos,
                    Nombre_Empresa = a.Nombre_Empresa,
                    Direccion = a.Direccion + "  " + a.Opcional_Direccion,
                    Opcional_Direccion = a.Opcional_Direccion,
                    Departamento = a.Departamento,
                    Ciudad = a.Ciudad,
                    Codigo_Postal = a.Codigo_Postal,
                    Celular = a.Celular,
                    Correo_Electronico = a.Correo_Electronico,
                    Comentario = a.Comentario,
                    Fecha_Creacion = a.Fecha_Creacion,
                    Vigente = a.Vigente,
                    Cancelada = a.Cancelada,
                    Enviado_Satisfactoriamente = a.Enviado_Satisfactoriamente,
                    Empresa_Entrega = a.Empresa_Entrega,
                    Fecha_Envio_producto = a.Fecha_Envio_producto,
                    Fecha_Llegada_producto = a.Fecha_Llegada_producto,
                    Estado_producto = a.Estado_producto,
                    Identifacion_modifica = a.Identifacion_modifica,
                    CelularConIndicativo = a.CelularConIndicativo,
                    Cantidad = _context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Sum(s => s.Cantidad).ToString(),
                    CostoEnvio =a.CostoEnvio,
                    PrecioTotal = _context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Sum(s => s.Precio_Total).ToString(),
                    PrecioGeneral = (Convert.ToInt64(_context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Max(s => s.Precio_Envio).ToString()) + Convert.ToInt64(_context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Sum(s => s.Precio_Total).ToString())),
                    EnvioCorreo = a.EnvioCorreo,
                    PrecioFinal = (Convert.ToDecimal(_context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Sum(s => s.Precio_Total).ToString()) + Convert.ToDecimal(a.CostoEnvio))

                    //CostoEnvio = Convert.ToInt32(_context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Max(s => s.Precio_Envio).ToString()),
                }).ToListAsync();


                foreach (var item in ListaMiCompra)
                {
                    item.ListaMisCompras = _context.mi_carrito.Where(a => a.id_compra_realizada == item.id_compra_realizada && a.Cancelado == "NO").Select(a => new DtoMiCarrito
                    {
                        id_carrito = a.id_carrito,
                        id_compra_realizada = a.id_compra_realizada,
                        id_producto = a.id_producto,
                        Descripcion_Producto = a.Descripcion_Producto,
                        Producto = _context.productos.OrderByDescending(x => x.PRODUCTO_ID == a.id_producto).First().NOMBRE,
                        Cantidad = a.Cantidad,
                        precio_Unitario = a.precio_Unitario,
                        Precio_Envio = a.Precio_Envio,
                        Precio_Total = a.Precio_Total,
                        Descuento = a.Descuento,
                        Cancelado = a.Cancelado
                    }).ToList();

                    foreach (var item2 in item.ListaMisCompras)
                    {
                        item2.ListaImagenesC = _context.imagenes_producto.Where(z => z.PRODUCTO_ID == item2.id_producto).Select(a => new DtoImagenesProducto
                        {
                            IMAGEN_ID = a.IMAGEN_ID,
                            PRODUCTO_ID = a.PRODUCTO_ID,
                            CONTENT_TYPE = a.CONTENT_TYPE,
                            FILENAME = a.FILENAME,
                            CREADO_POR = a.CREADO_POR,
                            FECHA_CREACION = a.FECHA_CREACION,
                            URL = a.URL,
                            VIGENTE = a.VIGENTE

                        }).ToList();
                    }
                }
                return ListaMiCompra;
            }
            catch (Exception e)
            {
                throw new Exception("Error " + e.Message);
            }
        }
        public async Task<List<DtoMiCompraRealizada>> F_GetProductosVentas()
        {
            try
            {
                List<DtoMiCompraRealizada> ListaMiCompra = await _context.mi_compra_realizada.Where(x => (x.Estado_producto == null) && (x.Cancelada == "NO" || x.Cancelada == null)).Select(a => new DtoMiCompraRealizada
                {
                    id_compra_realizada = a.id_compra_realizada,
                    Pais = a.Pais,
                    Nombres = a.Nombres,
                    Apellidos = a.Apellidos,
                    Nombre_Empresa = a.Nombre_Empresa,
                    Direccion = a.Direccion + "  " + a.Opcional_Direccion,
                    DireccionS = a.Direccion,
                    Opcional_Direccion = a.Opcional_Direccion,
                    Departamento = a.Departamento,
                    Ciudad = a.Ciudad,
                    Codigo_Postal = a.Codigo_Postal,
                    Celular = a.Celular,
                    Correo_Electronico = a.Correo_Electronico,
                    Comentario = a.Comentario,
                    Fecha_Creacion = a.Fecha_Creacion,
                    Vigente = a.Vigente,
                    Cancelada = a.Cancelada,
                    Enviado_Satisfactoriamente = a.Enviado_Satisfactoriamente,
                    Empresa_Entrega = a.Empresa_Entrega,
                    Fecha_Envio_producto = a.Fecha_Envio_producto,
                    Fecha_Llegada_producto = a.Fecha_Llegada_producto,
                    Estado_producto = a.Estado_producto,
                    Identifacion_modifica = a.Identifacion_modifica,
                    CelularConIndicativo = a.CelularConIndicativo,
                    Cantidad = _context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Sum(s => s.Cantidad).ToString(),
                    CostoEnvio = a.CostoEnvio,
                    //CostoEnvio = _context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Max(s => s.Precio_Envio).ToString(),
                    PrecioTotal = _context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Sum(s => s.Precio_Total).ToString(),
                    PrecioGeneral = (Convert.ToInt64(_context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Max(s => s.Precio_Envio).ToString()) + Convert.ToInt64(_context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Sum(s => s.Precio_Total).ToString())),
                    EnvioCorreo = a.EnvioCorreo,
                    FechaInicio = _context.mi_compra_realizada.Min(s => s.Fecha_Creacion).ToString(),
                    FechaFin = _context.mi_compra_realizada.Max(s => s.Fecha_Creacion).ToString(),
                    preferenceId = a.preferenceId,
                    status = a.status,
                    external_reference = a.external_reference ,
                    merchant_order_id = a.merchant_order_id,
                    EntregaMercanciaLocal = a.EntregaMercanciaLocal,
                    PagoNequi = a.PagoNequi,
                    PagoDaviplata = a.PagoDaviplata,
                    OtroMedioPago = a.OtroMedioPago,
                    PagoEfectivo = a.PagoEfectivo,


                }).ToListAsync();


                foreach (var item in ListaMiCompra)
                {
                    item.ListaMisCompras = _context.mi_carrito.Where(a => a.id_compra_realizada == item.id_compra_realizada && a.Cancelado == "NO").Select(a => new DtoMiCarrito
                    {
                        id_carrito = a.id_carrito,
                        id_compra_realizada = a.id_compra_realizada,
                        id_producto = a.id_producto,
                        Descripcion_Producto = a.Descripcion_Producto,
                        Producto = _context.productos.OrderByDescending(x => x.PRODUCTO_ID == a.id_producto).First().NOMBRE,
                        Cantidad = a.Cantidad,
                        precio_Unitario = a.precio_Unitario,
                        Precio_Envio = a.Precio_Envio,
                        Precio_Total = a.Precio_Total,
                        Descuento = a.Descuento,
                        Cancelado = a.Cancelado
                    }).ToList();

                    foreach (var item2 in item.ListaMisCompras)
                    {
                        item2.ListaImagenesC = _context.imagenes_producto.Where(z => z.PRODUCTO_ID == item2.id_producto).Select(a => new DtoImagenesProducto
                        {
                            IMAGEN_ID = a.IMAGEN_ID,
                            PRODUCTO_ID = a.PRODUCTO_ID,
                            CONTENT_TYPE = a.CONTENT_TYPE,
                            FILENAME = a.FILENAME,
                            CREADO_POR = a.CREADO_POR,
                            FECHA_CREACION = a.FECHA_CREACION,
                            URL = a.URL,
                            VIGENTE = a.VIGENTE

                        }).ToList();
                    }
                }
                return ListaMiCompra;
            }
            catch (Exception e)
            {
                throw new Exception("Error " + e.Message);
            }
        }
        public async Task<List<DtoMiCompraRealizada>> F_GetProductosTotal()
        {
            try
            {
                List<DtoMiCompraRealizada> ListaMiCompra = await _context.mi_compra_realizada.Select(a => new DtoMiCompraRealizada
                {
                    id_compra_realizada = a.id_compra_realizada,
                    Pais = a.Pais,
                    Nombres = a.Nombres,
                    Apellidos = a.Apellidos,
                    Nombre_Empresa = a.Nombre_Empresa,
                    Direccion = a.Direccion + "  " + a.Opcional_Direccion,
                    Opcional_Direccion = a.Opcional_Direccion,
                    Departamento = a.Departamento,
                    Ciudad = a.Ciudad,
                    Codigo_Postal = a.Codigo_Postal,
                    Celular = a.Celular,
                    Correo_Electronico = a.Correo_Electronico,
                    Comentario = a.Comentario,
                    Fecha_Creacion = a.Fecha_Creacion,
                    Vigente = a.Vigente,
                    Cancelada = a.Cancelada,
                    Enviado_Satisfactoriamente = a.Enviado_Satisfactoriamente,
                    Empresa_Entrega = a.Empresa_Entrega,
                    Fecha_Envio_producto = a.Fecha_Envio_producto,
                    Fecha_Llegada_producto = a.Fecha_Llegada_producto,
                    Estado_producto = a.Estado_producto,
                    Identifacion_modifica = a.Identifacion_modifica,
                    CelularConIndicativo = a.CelularConIndicativo,
                    Cantidad = _context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Sum(s => s.Cantidad).ToString(),
                    CostoEnvio = a.CostoEnvio,
                    //CostoEnvio = _context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Max(s => s.Precio_Envio).ToString(),
                    PrecioTotal = _context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Sum(s => s.Precio_Total).ToString(),
                    PrecioGeneral = (Convert.ToInt64(_context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Max(s => s.Precio_Envio).ToString()) + Convert.ToInt64(_context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Sum(s => s.Precio_Total).ToString())),
                    EnvioCorreo = a.EnvioCorreo,
                    FechaInicio = _context.mi_compra_realizada.Min(s => s.Fecha_Creacion).ToString(),
                    FechaFin = _context.mi_compra_realizada.Max(s => s.Fecha_Creacion).ToString(),
                    preferenceId = a.preferenceId,
                    status = a.status,
                    external_reference = a.external_reference,
                    merchant_order_id = a.merchant_order_id,



                }).ToListAsync();


                foreach (var item in ListaMiCompra)
                {
                    item.ListaMisCompras = _context.mi_carrito.Where(a => a.id_compra_realizada == item.id_compra_realizada && a.Cancelado == "NO").Select(a => new DtoMiCarrito
                    {
                        id_carrito = a.id_carrito,
                        id_compra_realizada = a.id_compra_realizada,
                        id_producto = a.id_producto,
                        Descripcion_Producto = a.Descripcion_Producto,
                        Producto = _context.productos.OrderByDescending(x => x.PRODUCTO_ID == a.id_producto).First().NOMBRE,
                        Cantidad = a.Cantidad,
                        precio_Unitario = a.precio_Unitario,
                        Precio_Envio = a.Precio_Envio,
                        Precio_Total = a.Precio_Total,
                        Descuento = a.Descuento,
                        Cancelado = a.Cancelado
                    }).ToList();

                    foreach (var item2 in item.ListaMisCompras)
                    {
                        item2.ListaImagenesC = _context.imagenes_producto.Where(z => z.PRODUCTO_ID == item2.id_producto).Select(a => new DtoImagenesProducto
                        {
                            IMAGEN_ID = a.IMAGEN_ID,
                            PRODUCTO_ID = a.PRODUCTO_ID,
                            CONTENT_TYPE = a.CONTENT_TYPE,
                            FILENAME = a.FILENAME,
                            CREADO_POR = a.CREADO_POR,
                            FECHA_CREACION = a.FECHA_CREACION,
                            URL = a.URL,
                            VIGENTE = a.VIGENTE

                        }).ToList();
                    }
                }
                return ListaMiCompra;
            }
            catch (Exception e)
            {
                throw new Exception("Error " + e.Message);
            }
        }
        public async Task<List<DtoMiCompraRealizada>> F_GetProductosVentasRealizadas()
        {
            try
            {
                List<DtoMiCompraRealizada> ListaMiCompra = await _context.mi_compra_realizada.Where(x => (x.Estado_producto != null) || x.Cancelada != "NO").Select(a => new DtoMiCompraRealizada
                {
                    id_compra_realizada = a.id_compra_realizada,
                    Pais = a.Pais,
                    Nombres = a.Nombres,
                    Apellidos = a.Apellidos,
                    Nombre_Empresa = a.Nombre_Empresa,
                    Direccion = a.Direccion + "  " + a.Opcional_Direccion,
                    DireccionS = a.Direccion,
                    Opcional_Direccion = a.Opcional_Direccion,
                    Departamento = a.Departamento,
                    Ciudad = a.Ciudad,
                    Codigo_Postal = a.Codigo_Postal,
                    Celular = a.Celular,
                    Correo_Electronico = a.Correo_Electronico,
                    Comentario = a.Comentario,
                    Fecha_Creacion = Convert.ToDateTime(a.Fecha_Creacion),
                    Vigente = a.Vigente,
                    Cancelada = a.Cancelada,
                    Enviado_Satisfactoriamente = a.Enviado_Satisfactoriamente,
                    Empresa_Entrega = a.Empresa_Entrega,
                    Fecha_Envio_producto = a.Fecha_Envio_producto,
                    Fecha_Llegada_producto = a.Fecha_Llegada_producto,
                    Estado_producto = a.Estado_producto,
                    Identifacion_modifica = a.Identifacion_modifica,
                    CelularConIndicativo = a.CelularConIndicativo,
                    Cantidad = _context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Sum(s => s.Cantidad).ToString(),
                    CostoEnvio = a.CostoEnvio,
                    //CostoEnvio = _context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Max(s => s.Precio_Envio).ToString(),
                    PrecioTotal = _context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Sum(s => s.Precio_Total).ToString(),
                    PrecioGeneral = (Convert.ToInt64(_context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Max(s => s.Precio_Envio).ToString()) + Convert.ToInt64(_context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Sum(s => s.Precio_Total).ToString())),
                    EnvioCorreo = a.EnvioCorreo,
                    FechaInicio = _context.mi_compra_realizada.Min(s => s.Fecha_Creacion).ToString(),
                    FechaFin = _context.mi_compra_realizada.Max(s => s.Fecha_Creacion).ToString(),
                    preferenceId = a.preferenceId,
                    status = a.status,
                    external_reference = a.external_reference,
                    merchant_order_id = a.merchant_order_id,
                    EntregaMercanciaLocal = a.EntregaMercanciaLocal,
                    PagoNequi = a.PagoNequi,
                    PagoDaviplata = a.PagoDaviplata,
                    OtroMedioPago = a.OtroMedioPago,
                    PagoEfectivo = a.PagoEfectivo,

                }).ToListAsync();


                foreach (var item in ListaMiCompra)
                {
                    item.ListaMisCompras = _context.mi_carrito.Where(a => a.id_compra_realizada == item.id_compra_realizada && a.Cancelado == "NO").Select(a => new DtoMiCarrito
                    {
                        id_carrito = a.id_carrito,
                        id_compra_realizada = a.id_compra_realizada,
                        id_producto = a.id_producto,
                        Descripcion_Producto = a.Descripcion_Producto,
                        Producto = _context.productos.OrderByDescending(x => x.PRODUCTO_ID == a.id_producto).First().NOMBRE,
                        Cantidad = a.Cantidad,
                        precio_Unitario = a.precio_Unitario,
                        Precio_Envio = a.Precio_Envio,
                        Precio_Total = a.Precio_Total,
                        Descuento = a.Descuento,
                        Cancelado = a.Cancelado
                    }).ToList();

                    foreach (var item2 in item.ListaMisCompras)
                    {
                        item2.ListaImagenesC = _context.imagenes_producto.Where(z => z.PRODUCTO_ID == item2.id_producto).Select(a => new DtoImagenesProducto
                        {
                            IMAGEN_ID = a.IMAGEN_ID,
                            PRODUCTO_ID = a.PRODUCTO_ID,
                            CONTENT_TYPE = a.CONTENT_TYPE,
                            FILENAME = a.FILENAME,
                            CREADO_POR = a.CREADO_POR,
                            FECHA_CREACION = a.FECHA_CREACION,
                            URL = a.URL,
                            VIGENTE = a.VIGENTE

                        }).ToList();
                    }
                }
                return ListaMiCompra;
            }
            catch (Exception e)
            {
                throw new Exception("Error " + e.Message);
            }
        }
        public async Task<List<DtoMiCompraRealizada>> F_GetProductosVentasPdf()
        {
            try
            {
                List<DtoMiCompraRealizada> ListaMiCompra = await _context.mi_compra_realizada.Where(x => (x.Estado_producto != "NO") || x.Cancelada != "NO").Select(a => new DtoMiCompraRealizada
                {
                    id_compra_realizada = a.id_compra_realizada,
                    Pais = a.Pais,
                    Nombres = a.Nombres,
                    Apellidos = a.Apellidos,
                    Nombre_Empresa = a.Nombre_Empresa,
                    Direccion = a.Direccion + "  " + a.Opcional_Direccion,
                    Opcional_Direccion = a.Opcional_Direccion,
                    Departamento = a.Departamento,
                    Ciudad = a.Ciudad,
                    Codigo_Postal = a.Codigo_Postal,
                    Celular = a.Celular,
                    Correo_Electronico = a.Correo_Electronico,
                    Comentario = a.Comentario,
                    Fecha_Creacion = Convert.ToDateTime(a.Fecha_Creacion),
                    Vigente = a.Vigente,
                    Cancelada = a.Cancelada,
                    Enviado_Satisfactoriamente = a.Enviado_Satisfactoriamente,
                    Empresa_Entrega = a.Empresa_Entrega,
                    Fecha_Envio_producto = a.Fecha_Envio_producto,
                    Fecha_Llegada_producto = a.Fecha_Llegada_producto,
                    Estado_producto = a.Estado_producto,
                    Identifacion_modifica = a.Identifacion_modifica,
                    CelularConIndicativo = a.CelularConIndicativo,
                    Cantidad = _context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Sum(s => s.Cantidad).ToString(),
                    //CostoEnvio = _context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Max(s => s.Precio_Envio).ToString(),
                    CostoEnvio = a.CostoEnvio,
                    PrecioTotal = _context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Sum(s => s.Precio_Total).ToString(),
                    PrecioGeneral = (Convert.ToInt64(_context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Max(s => s.Precio_Envio).ToString()) + Convert.ToInt64(_context.mi_carrito.Where(x => x.id_compra_realizada == a.id_compra_realizada).Sum(s => s.Precio_Total).ToString())),
                    EnvioCorreo = a.EnvioCorreo,
                    FechaInicio = _context.mi_compra_realizada.Min(s => s.Fecha_Creacion).ToString(),
                    FechaFin = _context.mi_compra_realizada.Max(s => s.Fecha_Creacion).ToString(),
                    preferenceId = a.preferenceId,
                    status = a.status,
                    external_reference = a.external_reference,
                    merchant_order_id = a.merchant_order_id,


                }).ToListAsync();


                foreach (var item in ListaMiCompra)
                {
                    item.ListaMisCompras = _context.mi_carrito.Where(a => a.id_compra_realizada == item.id_compra_realizada && a.Cancelado == "NO").Select(a => new DtoMiCarrito
                    {
                        id_carrito = a.id_carrito,
                        id_compra_realizada = a.id_compra_realizada,
                        id_producto = a.id_producto,
                        Descripcion_Producto = a.Descripcion_Producto,
                        Producto = _context.productos.OrderByDescending(x => x.PRODUCTO_ID == a.id_producto).First().NOMBRE,
                        Cantidad = a.Cantidad,
                        precio_Unitario = a.precio_Unitario,
                        Precio_Envio = a.Precio_Envio,
                        Precio_Total = a.Precio_Total,
                        Descuento = a.Descuento,
                        Cancelado = a.Cancelado
                    }).ToList();

                    foreach (var item2 in item.ListaMisCompras)
                    {
                        item2.ListaImagenesC = _context.imagenes_producto.Where(z => z.PRODUCTO_ID == item2.id_producto).Select(a => new DtoImagenesProducto
                        {
                            IMAGEN_ID = a.IMAGEN_ID,
                            PRODUCTO_ID = a.PRODUCTO_ID,
                            CONTENT_TYPE = a.CONTENT_TYPE,
                            FILENAME = a.FILENAME,
                            CREADO_POR = a.CREADO_POR,
                            FECHA_CREACION = a.FECHA_CREACION,
                            URL = a.URL,
                            VIGENTE = a.VIGENTE

                        }).ToList();
                    }
                }
                return ListaMiCompra;
            }
            catch (Exception e)
            {
                throw new Exception("Error " + e.Message);
            }
        }
        public async Task<List<DtoComentariosClientes>> F_GetComentarios()
        {
            try
            {
                List<DtoComentariosClientes> ListaComentarios = await _context.comentarios_clientes.Select(s => new DtoComentariosClientes
                {
                    idComentario = s.idComentario,
                    Nombres = s.Nombres,
                    NumeroTelefono = s.NumeroTelefono,
                    vigente = s.vigente,
                    Comentarios = s.Comentarios,
                    CorreoElectronico = s.CorreoElectronico,
                    fechaCreacion = s.fechaCreacion,
                    NumeroTelefonoConIndicativo = s.NumeroTelefonoConIndicativo,
                    Atendio = s.Atendio,
                    EnvioCorreo = s.EnvioCorreo
                }).ToListAsync();
                return ListaComentarios;
            }
            catch (Exception e)
            {
                throw new Exception("Error " + e.Message);
            }
        }
        public async Task<RespuestaDto<List<DtoMiCompraRealizada>>> UpdateProductoVenta(int _IdVentaProducto, string _descripcion, string V_Maquina, int V_Usuario, string _columna)

        {
            var respuesta = 0;
            RespuestaDto<List<DtoMiCompraRealizada>> retorno = new RespuestaDto<List<DtoMiCompraRealizada>>();
            var result = _context.mi_compra_realizada.Where(x => x.id_compra_realizada == _IdVentaProducto).FirstOrDefault();
            if (_columna == "pais")
            {
                result.Pais = _descripcion is null ? null : _descripcion.ToUpper();
            }
            if (_columna == "departamento")
            {
                result.Departamento = _descripcion is null ? null : _descripcion.ToUpper();
            }
            if (_columna == "ciudad")
            {
                result.Ciudad = _descripcion is null ? null : _descripcion.ToUpper();
            }
            if (_columna == "codigo_Postal")
            {
                result.Codigo_Postal = _descripcion is null ? null : _descripcion.ToUpper();
            }
            if (_columna == "celular")
            {
                result.Celular = _descripcion is null ? null : _descripcion.ToUpper();
            }
            if (_columna == "correo_Electronico")
            {
                result.Correo_Electronico = _descripcion is null ? null : _descripcion.ToUpper();
            }
            if (_columna == "comentario")
            {
                result.Comentario = _descripcion is null ? null : _descripcion.ToUpper();
            }
            if (_columna == "nombres")
            {
                result.Nombres = _descripcion is null ? null : _descripcion.ToUpper();
            }
            if (_columna == "apellidos")
            {
                result.Apellidos = _descripcion is null ? null : _descripcion.ToUpper();
            }
            if (_columna == "direccion")
            {
                result.Direccion = _descripcion is null ? null : _descripcion.ToUpper();
            }
            if (_columna == "opcional_Direccion")
            {
                result.Opcional_Direccion = _descripcion is null ? null : _descripcion.ToUpper();
            }
            if (_columna == "fecha_CreacionS")
            {
                result.Fecha_Creacion = _descripcion is null ? null : Convert.ToDateTime(_descripcion);
            }
            if (_columna == "fecha_Envio_productoS")
            {

                result.Fecha_Envio_producto = _descripcion is null ? null : Convert.ToDateTime(_descripcion);
            }
            if (_columna == "fecha_Llegada_productoS")
            {
                result.Fecha_Llegada_producto = _descripcion is null ? null : Convert.ToDateTime(_descripcion);
            }
            if (_columna == "empresa_Entrega")
            {
                result.Empresa_Entrega = _descripcion is null ? null : _descripcion.ToUpper();
            }
            if (_columna == "enviado_Satisfactoriamente")
            {
                result.Enviado_Satisfactoriamente = _descripcion is null ? null : _descripcion.ToUpper();
            }
            if (_columna == "estado_producto")
            {
                result.Estado_producto = _descripcion is null ? null : _descripcion.ToUpper();
            }
            if (_columna == "cancelada")
            {
                result.Cancelada = _descripcion is null ? null : _descripcion.ToUpper();
            }
            if (_columna == "entregaMercanciaLocal")
            {
                result.EntregaMercanciaLocal = _descripcion is null ? null : _descripcion.ToUpper();
            }
            if (_columna == "pagoNequi")
            {
                result.PagoNequi = _descripcion is null ? null : _descripcion.ToUpper();
            }
            if (_columna == "pagoDaviplata")
            {
                result.PagoDaviplata = _descripcion is null ? null : _descripcion.ToUpper();
            }
            if (_columna == "otroMedioPago")
            {
                result.OtroMedioPago = _descripcion is null ? null : _descripcion.ToUpper();
            }
            if (_columna == "pagoEfectivo")
            {
                result.PagoEfectivo = _descripcion is null ? null : _descripcion.ToUpper();
            }

            _context.Entry(result).State = EntityState.Modified;
            try
            {
                respuesta = await _context.SaveChangesAsync();
                if (respuesta > 0)
                {
                    var CantidadVenta = _context.mi_compra_realizada.Where(x => x.Estado_producto == "" || x.Estado_producto == null && (x.Cancelada == "NO" || x.Cancelada == null)).Count();
                    retorno.Codigo = EstadoOperacion.Bueno;
                    retorno.Id = CantidadVenta;
                }
                else
                {
                    retorno.Codigo = EstadoOperacion.Malo;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return retorno;
        }
        public async Task<List<DtoComentariosClientes>> F_GetConsultaCorreos()
        {
            try
            {
                var Respuesta = new List<DtoComentariosClientes>();
                List<DtoComentariosClientes> ListaMicompra = await _context.mi_compra_realizada.Select(s => new DtoComentariosClientes
                {
                    idComentario = s.id_compra_realizada,
                    Nombres = s.Nombres,
                    NumeroTelefono = s.Celular,
                    vigente = s.Vigente,
                    CorreoElectronico = s.Correo_Electronico,
                    NumeroTelefonoConIndicativo = s.CelularConIndicativo,
                    EnvioCorreo = s.EnvioCorreo,
                    idEnvio = 1
                }).ToListAsync();

                List<DtoComentariosClientes> ListaComentarios = await _context.comentarios_clientes.Select(s => new DtoComentariosClientes
                {
                    idComentario = s.idComentario,
                    Nombres = s.Nombres,
                    NumeroTelefono = s.NumeroTelefono,
                    vigente = s.vigente,
                    Comentarios = s.Comentarios,
                    CorreoElectronico = s.CorreoElectronico,
                    fechaCreacion = s.fechaCreacion,
                    NumeroTelefonoConIndicativo = s.NumeroTelefonoConIndicativo,
                    EnvioCorreo = s.EnvioCorreo,
                    idEnvio = 2
                }).ToListAsync();

                if (ListaMicompra.Count > 0)
                {
                    var contador = 0;
                    foreach (var obj2 in ListaMicompra)
                    {
                        contador++;
                        DtoComentariosClientes? retorno = new DtoComentariosClientes();
                        {
                            retorno.idComentario = contador;
                            retorno.idComentario2 = obj2.idComentario;
                            retorno.Nombres = obj2.Nombres;
                            retorno.NumeroTelefono = obj2.NumeroTelefono;
                            retorno.vigente = obj2.vigente;
                            retorno.CorreoElectronico = obj2.CorreoElectronico;
                            retorno.NumeroTelefonoConIndicativo = obj2.NumeroTelefonoConIndicativo;
                            retorno.EnvioCorreo = obj2.EnvioCorreo;
                            retorno.idEnvio = obj2.idEnvio;
                        };
                        Respuesta.Add(retorno);
                    }

                    foreach (var obj in ListaComentarios)
                    {
                        contador++;
                        DtoComentariosClientes? retorno = new DtoComentariosClientes();
                        {
                            retorno.idComentario = contador;
                            retorno.idComentario2 = obj.idComentario;
                            retorno.Nombres = obj.Nombres;
                            retorno.NumeroTelefono = obj.NumeroTelefono;
                            retorno.vigente = obj.vigente;
                            retorno.CorreoElectronico = obj.CorreoElectronico;
                            retorno.NumeroTelefonoConIndicativo = obj.NumeroTelefonoConIndicativo;
                            retorno.EnvioCorreo = obj.EnvioCorreo;
                            retorno.idEnvio = obj.idEnvio;

                        };
                        Respuesta.Add(retorno);
                    }
                    return Respuesta;
                }
                else
                {

                    return Respuesta;
                }

            }
            catch (Exception e)
            {
                throw new Exception("Error " + e.Message);
            }
        }
        public async Task<RespuestaDto<List<DtoDominios>>> GetCorreos()
        {
            try
            {
                var Respuesta = new RespuestaDto<List<DtoDominios>>();
                Respuesta.Codigo = EstadoOperacion.Bueno;
                Respuesta.Respuesta = new List<DtoDominios>();
                //List<RespuestaDto> = new List<RespuestaDto>();

                var resultado = _context.mi_compra_realizada.OrderBy(x => x.Correo_Electronico).ToList();

                var resultado2 = _context.comentarios_clientes.Where(s => s.vigente == "SI").OrderBy(x => x.CorreoElectronico).ToList();

                if (resultado.Count > 0)
                {
                    var contador = 0;
                    foreach (var obj in resultado)
                    {
                        contador++;
                        DtoDominios? retorno = new DtoDominios();
                        {
                            retorno.IdDominio = contador;
                            retorno.Descripcion = obj.Correo_Electronico;

                        };
                        Respuesta.Respuesta.Add(retorno);
                    }
                    foreach (var obj in resultado2)
                    {
                        contador++;
                        DtoDominios? retorno = new DtoDominios();
                        {
                            retorno.IdDominio = contador;
                            retorno.Descripcion = obj.CorreoElectronico;
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
        public async Task<List<DtoDominios>> F_GetProductosGrillaMarca()
        {
            try
            {
                List<DtoDominios> ListaProductos = await _context.ctrl_dominios.Where(a => a.PADRE_ID == 2).Select(a => new DtoDominios
                {
                    IdDominio = a.ID_DOMINIO,
                    Descripcion = a.DESCRIPCION,
                    Vigente = a.VIGENTE

                }).ToListAsync();



                return ListaProductos;
            }
            catch (Exception e)
            {
                throw new Exception("Error " + e.Message);
            }
        }
        public async Task<RespuestaDto<List<DtoDominios>>> F_UpdateMarca(DtoDominios Producto)

        {
            RespuestaDto<List<DtoDominios>> retorno = new RespuestaDto<List<DtoDominios>>();
            try
            {
                var query = from ord in _context.ctrl_dominios
                            where ord.ID_DOMINIO == Producto.IdDominio && ord.PADRE_ID == 2
                            select ord;

                foreach (var item in query)
                {
                    item.VIGENTE = Producto.Vigente;
                    item.DESCRIPCION = Producto.Descripcion;
                }
                var Resul = await _context.SaveChangesAsync();
                var numero = Resul;

                if (numero > 0)
                {

                    retorno.Codigo = EstadoOperacion.Bueno;
                    retorno.Mensaje = "Marca Modificada Satisfactoriamente";
                }
                else
                {
                    retorno.Codigo = EstadoOperacion.Malo;
                    retorno.Mensaje = "Por favor validar, no es posible Guardar";
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return retorno;
        }
        public async Task<RespuestaDto<List<DtoDominios>>> F_AddMarca(DtoDominios Producto)

        {
            RespuestaDto<List<DtoDominios>> retorno = new RespuestaDto<List<DtoDominios>>();

            try
            {
                int productoId = 1;
                int num = _context.ctrl_dominios.Count();

                if (num > 0)
                {
                    productoId = _context.ctrl_dominios.Max(x => x.ID_DOMINIO) + 1;
                }

                ctrl_dominios resultado = new ctrl_dominios()
                {
                    ID_DOMINIO = productoId,
                    DESCRIPCION = Producto.Descripcion,
                    PADRE_ID = 2,
                    VIGENTE = Producto.Vigente
                };


                _context.ctrl_dominios.Add(resultado);
                var resp = await _context.SaveChangesAsync();
                var valida = resp;
                if (valida > 0)
                {
                    retorno.Codigo = EstadoOperacion.Bueno;
                    retorno.Mensaje = "Marca guardada Satisfactoriamente";
                }
                else
                {
                    retorno.Codigo = EstadoOperacion.Malo;
                    retorno.Mensaje = "No pudo se gurdado por favor revise";
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return retorno;
        }
        public async Task<List<DtoDominios>> F_GetProductosGrillaCategoria()
        {
            try
            {
                List<DtoDominios> ListaProductos = await _context.ctrl_dominios.Where(a => a.PADRE_ID == 32).Select(a => new DtoDominios
                {
                    IdDominio = a.ID_DOMINIO,
                    Descripcion = a.DESCRIPCION,
                    Vigente = a.VIGENTE
                }).ToListAsync();



                return ListaProductos;
            }
            catch (Exception e)
            {
                throw new Exception("Error " + e.Message);
            }
        }
        public async Task<RespuestaDto<List<DtoDominios>>> F_UpdateCategoria(DtoDominios Producto)

        {
            RespuestaDto<List<DtoDominios>> retorno = new RespuestaDto<List<DtoDominios>>();
            try
            {
                var query = from ord in _context.ctrl_dominios
                            where ord.ID_DOMINIO == Producto.IdDominio && ord.PADRE_ID == 32
                            select ord;

                foreach (var item in query)
                {
                    item.VIGENTE = Producto.Vigente;
                    item.DESCRIPCION = Producto.Descripcion;

                }
                var respuesta = await _context.SaveChangesAsync();

                var numero = respuesta;
                if (numero > 0)
                {
                    retorno.Codigo = EstadoOperacion.Bueno;
                    retorno.Mensaje = "Categoria Modificada Satisfactoriamente";
                }
                else
                {
                    retorno.Codigo = EstadoOperacion.Malo;
                    retorno.Mensaje = "Por favor validar, no es posible Guardar";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return retorno;
        }
        public async Task<RespuestaDto<List<DtoDominios>>> F_AddCategoria(DtoDominios Producto)

        {
            var respuesta = 0;
            RespuestaDto<List<DtoDominios>> retorno = new RespuestaDto<List<DtoDominios>>();
            try
            {
                int productoId = 1;
                int num = _context.ctrl_dominios.Count();

                if (num > 0)
                {
                    productoId = _context.ctrl_dominios.Max(x => x.ID_DOMINIO) + 1;
                }


                var query = from ord in _context.ctrl_dominios
                            where ord.ID_DOMINIO == Producto.IdDominio && ord.PADRE_ID == 32
                            select ord;

                foreach (var ord in query)
                {
                    ord.ID_DOMINIO = productoId;
                    ord.PADRE_ID = 32;
                    ord.DESCRIPCION = Producto.Descripcion;
                    ord.VIGENTE = Producto.Vigente;
                }


                respuesta = await _context.SaveChangesAsync();
                var valida = respuesta;
                if (valida > 0)
                {
                    retorno.Codigo = EstadoOperacion.Bueno;
                    retorno.Mensaje = "Marca guardada Satisfactoriamente";
                }
                else
                {
                    retorno.Codigo = EstadoOperacion.Malo;
                    retorno.Mensaje = "No pudo se gurdado por favor revise";
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return retorno;
        }
        public async Task<RespuestaDto<List<DtoComentariosClientes>>> F_UpdateComentarios(DtoComentariosClientes Producto)

        {
            var respuesta = 0;
            RespuestaDto<List<DtoComentariosClientes>> retorno = new RespuestaDto<List<DtoComentariosClientes>>();

            var query = from ord in _context.comentarios_clientes
                        where ord.idComentario == Convert.ToInt32(Producto.idComentario)
                        select ord;

            foreach (var ord in query)
            {
                ord.Atendio = "SI";
            }

            try
            {
                respuesta = await _context.SaveChangesAsync();
                if (respuesta > 0)
                {
                    var CantidadMensa = _context.comentarios_clientes.Where(x => x.Atendio == "NO").Count();
                    retorno.Codigo = EstadoOperacion.Bueno;
                    retorno.Id = CantidadMensa;
                }
                else
                {
                    retorno.Codigo = EstadoOperacion.Malo;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return retorno;
        }
        public async Task<RespuestaDto<List<DtoComentariosClientes>>> F_UpdateEnvioCorreo(int Producto)

        {
            var respuesta = 0;
            RespuestaDto<List<DtoComentariosClientes>> retorno = new RespuestaDto<List<DtoComentariosClientes>>();

            var query = from ord in _context.comentarios_clientes
                        where ord.idComentario == Convert.ToInt32(Producto)
                        select ord;

            foreach (var ord in query)
            {
                ord.EnvioCorreo = "SI";
            }
            try
            {
                respuesta = await _context.SaveChangesAsync();
                if (respuesta > 0)
                {
                    retorno.Codigo = EstadoOperacion.Bueno;

                }
                else
                {
                    retorno.Codigo = EstadoOperacion.Malo;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return retorno;
        }
        public async Task<RespuestaDto<List<DtoComentariosClientes>>> F_UpdateEnvioCorreoBandeja(int Producto)

        {
            var respuesta = 0;
            RespuestaDto<List<DtoComentariosClientes>> retorno = new RespuestaDto<List<DtoComentariosClientes>>();

            var query2 = from ord in _context.mi_compra_realizada
                         where ord.id_compra_realizada == Convert.ToInt32(Producto)
                         select ord;

            foreach (var ord in query2)
            {
                ord.EnvioCorreo = "SI";
            }


            try
            {
                respuesta = await _context.SaveChangesAsync();
                if (respuesta > 0)
                {
                    retorno.Codigo = EstadoOperacion.Bueno;

                }
                else
                {
                    retorno.Codigo = EstadoOperacion.Malo;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return retorno;
        }
        public async Task<RespuestaDto<List<DtoComentariosClientes>>> F_totalComentarios()
        {

            RespuestaDto<List<DtoComentariosClientes>> retorno = new RespuestaDto<List<DtoComentariosClientes>>();

            var CantidadMensa = _context.comentarios_clientes.Where(x => x.Atendio == "NO").Count();

            try
            {

                if (CantidadMensa > 0)
                {
                    retorno.Codigo = EstadoOperacion.Bueno;
                    retorno.Id = CantidadMensa;
                }
                else
                {
                    retorno.Codigo = EstadoOperacion.Malo;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return retorno;
        }
        public async Task<RespuestaDto<List<DtoMiCompraRealizada>>> F_totalVentasPorEnviar()
        {

            RespuestaDto<List<DtoMiCompraRealizada>> retorno = new RespuestaDto<List<DtoMiCompraRealizada>>();

            var CantidadVenta = _context.mi_compra_realizada.Where(x => x.Estado_producto == "" || x.Estado_producto == null && (x.Cancelada == "NO" || x.Cancelada == null)).Count();

            try
            {

                if (CantidadVenta > 0)
                {
                    retorno.Codigo = EstadoOperacion.Bueno;
                    retorno.Id = CantidadVenta;
                }
                else
                {
                    retorno.Codigo = EstadoOperacion.Malo;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return retorno;
        }
        public async Task<List<DtoCiudadEnvia>> F_GetGrillaCiudadEnvia(int numero)
        {
            try
            {

                if (numero == 0)
                {
                    List<DtoCiudadEnvia> ListaProductos = await _context.ctr_paises.Select(a => new DtoCiudadEnvia
                    {
                        CODIGO = a.CODIGO,
                        DESCRIPCION = a.DESCRIPCION,
                        ABREVIATURA = a.ABREVIATURA,
                        TIPO = a.TIPO,
                        ZONA = a.ZONA,
                        LUGE_CODIGO = a.LUGE_CODIGO,
                        VIGENTE = a.VIGENTE,
                        INDICATIVO = a.INDICATIVO,
                        COSTO_ENVIO = a.COSTO_ENVIO

                    }).ToListAsync();



                    return ListaProductos;

                }
                else
                {

                    List<DtoCiudadEnvia> ListaProductos = await _context.ctr_paises.Where(x => x.LUGE_CODIGO == numero).Select(a => new DtoCiudadEnvia
                    {
                        CODIGO = a.CODIGO,
                        DESCRIPCION = a.DESCRIPCION,
                        ABREVIATURA = a.ABREVIATURA,
                        TIPO = a.TIPO,
                        ZONA = a.ZONA,
                        LUGE_CODIGO = a.LUGE_CODIGO,
                        VIGENTE = a.VIGENTE,
                        INDICATIVO = a.INDICATIVO,
                        COSTO_ENVIO = a.COSTO_ENVIO

                    }).ToListAsync();



                    return ListaProductos;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error " + e.Message);
            }
        }
        public async Task<RespuestaDto<List<DtoCiudadEnvia>>> UpdateCiudad(DtoCiudadEnvia Producto)

        {
            RespuestaDto<List<DtoCiudadEnvia>> retorno = new RespuestaDto<List<DtoCiudadEnvia>>();
            try
            {
                var query = from ord in _context.ctr_paises
                            where ord.CODIGO == Producto.CODIGO
                            select ord;

                foreach (var item in query)
                {
                    item.VIGENTE = Producto.VIGENTE;
                    //item.DESCRIPCION = Producto.Descripcion;
                }
                var Resul = await _context.SaveChangesAsync();
                var numero = Resul;

                if (numero > 0)
                {

                    retorno.Codigo = EstadoOperacion.Bueno;
                    retorno.Mensaje = "Marca Modificada Satisfactoriamente";
                }
                else
                {
                    retorno.Codigo = EstadoOperacion.Malo;
                    retorno.Mensaje = "Por favor validar, no es posible Guardar";
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return retorno;
        }
        public async Task<RespuestaDto<List<DtoCiudadEnvia>>> F_AddCiudad(DtoCiudadEnvia Producto)

        {
            RespuestaDto<List<DtoCiudadEnvia>> retorno = new RespuestaDto<List<DtoCiudadEnvia>>();

            try
            {
                int productoId = 1;
                int num = _context.ctr_paises.Count();

                if (num > 0)
                {
                    productoId = _context.ctr_paises.Max(x => x.CODIGO) + 1;
                }

                ctr_paises resultado = new ctr_paises()
                {
                    CODIGO = productoId,
                    DESCRIPCION = Producto.DESCRIPCION,
                    ABREVIATURA = Producto.ABREVIATURA,
                    TIPO = Producto.TIPO,
                    ZONA = Producto.ZONA,
                    LUGE_CODIGO = Producto.LUGE_CODIGO,
                    VIGENTE = Producto.VIGENTE,
                    CREADO_POR = Producto.CREADO_POR,
                    MAQUINA_CREACION = Producto.MAQUINA_CREACION,
                    FECHA_CREACION = Producto.FECHA_CREACION,
                    INDICATIVO = Producto.INDICATIVO,
                    COSTO_ENVIO = Producto.COSTO_ENVIO,
                };


                _context.ctr_paises.Add(resultado);
                var resp = await _context.SaveChangesAsync();
                var valida = resp;
                if (valida > 0)
                {
                    retorno.Codigo = EstadoOperacion.Bueno;
                    retorno.Mensaje = "Marca guardada Satisfactoriamente";
                }
                else
                {
                    retorno.Codigo = EstadoOperacion.Malo;
                    retorno.Mensaje = "No pudo se gurdado por favor revise";
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return retorno;
        }
        public async Task<RespuestaDto<List<DtoCiudadEnvia>>> F_UpdateCiudad(DtoCiudadEnvia Producto)

        {
            RespuestaDto<List<DtoCiudadEnvia>> retorno = new RespuestaDto<List<DtoCiudadEnvia>>();
            try
            {
                var query = from ord in _context.ctr_paises
                            where ord.CODIGO == Producto.CODIGO
                            select ord;

                foreach (var item in query)
                {

                    item.CODIGO = Producto.CODIGO;
                    item.DESCRIPCION = Producto.DESCRIPCION;
                    item.ABREVIATURA = Producto.ABREVIATURA;
                    item.TIPO = Producto.TIPO;
                    item.ZONA = Producto.ZONA;
                    item.LUGE_CODIGO = Producto.LUGE_CODIGO;
                    item.VIGENTE = Producto.VIGENTE;
                    item.CREADO_POR = Producto.CREADO_POR;
                    item.MAQUINA_CREACION = Producto.MAQUINA_CREACION;
                    item.FECHA_CREACION = Producto.FECHA_CREACION;
                    item.INDICATIVO = Producto.INDICATIVO;
                    item.COSTO_ENVIO = Producto.COSTO_ENVIO;
                }
                var Resul = await _context.SaveChangesAsync();
                var numero = Resul;

                if (numero > 0)
                {

                    retorno.Codigo = EstadoOperacion.Bueno;
                    retorno.Mensaje = "Marca Modificada Satisfactoriamente";
                }
                else
                {
                    retorno.Codigo = EstadoOperacion.Malo;
                    retorno.Mensaje = "Por favor validar, no es posible Guardar";
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return retorno;
        }


        //public async Task<int> F_InsPreferenceId(Int32 idCompra, string PreferenceId, string Idstatus, string Idexternal_reference, string Idmerchant_order_id)
        //{
        //    using (ModelContext db2 = new ModelContext())
        //    {
        //        var compra = await db2.mi_compra_realizada
        //                              .Where(x => x.id_compra_realizada == idCompra)
        //                              .FirstOrDefaultAsync();

        //        if (compra != null)
        //        {
        //            compra.preferenceId = PreferenceId;
        //            compra.status = Idstatus;
        //            compra.external_reference = Idexternal_reference;
        //            compra.merchant_order_id = Idmerchant_order_id;

        //            db2.Entry(compra).State = EntityState.Modified;
        //            int Resu = await db2.SaveChangesAsync();

        //            return Resu > 0 ? compra.id_compra_realizada : 0;
        //        }

        //        return 0;
        //    }
        //}

        public async Task<int> F_InsPreferenceId(Int32 idCompra, string PreferenceId, string Idstatus, string Idexternal_reference, string Idmerchant_order_id)
        {
            // Buscar la compra en la base de datos
            var compra = await _context.mi_compra_realizada
                                  .Where(x => x.id_compra_realizada == idCompra)
                                  .FirstOrDefaultAsync();

            if (compra != null)
            {
                // Actualizar los valores de la compra
                compra.preferenceId = PreferenceId;
                compra.status = Idstatus;
                compra.external_reference = Idexternal_reference;
                compra.merchant_order_id = Idmerchant_order_id;

                // Marcar la entidad como modificada
                _context.Entry(compra).State = EntityState.Modified;
                int Resu = await _context.SaveChangesAsync(); // Guardar los cambios en la base de datos

                // Si la operación fue exitosa, devolver el id de la compra
                return Resu > 0 ? compra.id_compra_realizada : 0;
            }

            // Si no se encontró la compra, devolver 0
            return 0;
        }



        //     public async Task<int> F_UpdateCompra(DtoMiCompraRealizada dto)
        //{
        //    var respuesta = 0;

        //    try
        //    {
        //        // Buscar el registro existente de compra
        //        var compraExistente = await _context.mi_compra_realizada
        //            .FirstOrDefaultAsync(x => x.id_compra_realizada == dto.id_compra_realizada);

        //        if (compraExistente == null)
        //        {
        //            respuesta = 0;
        //            return respuesta;
        //        }

        //        // Verificar si el campo del DTO no es nulo o vacío antes de actualizar
        //        if (!string.IsNullOrEmpty(dto.Pais)) compraExistente.Pais = dto.Pais;
        //        if (!string.IsNullOrEmpty(dto.Nombres)) compraExistente.Nombres = dto.Nombres;
        //        if (!string.IsNullOrEmpty(dto.Apellidos)) compraExistente.Apellidos = dto.Apellidos;
        //        if (!string.IsNullOrEmpty(dto.Nombre_Empresa)) compraExistente.Nombre_Empresa = dto.Nombre_Empresa;
        //        if (!string.IsNullOrEmpty(dto.Direccion)) compraExistente.Direccion = dto.Direccion;
        //        if (!string.IsNullOrEmpty(dto.Opcional_Direccion)) compraExistente.Opcional_Direccion = dto.Opcional_Direccion;
        //        if (!string.IsNullOrEmpty(dto.Departamento)) compraExistente.Departamento = dto.Departamento;
        //        if (!string.IsNullOrEmpty(dto.Ciudad)) compraExistente.Ciudad = dto.Ciudad;
        //        if (!string.IsNullOrEmpty(dto.Codigo_Postal)) compraExistente.Codigo_Postal = dto.Codigo_Postal;
        //        if (!string.IsNullOrEmpty(dto.Celular)) compraExistente.Celular = dto.Celular;
        //        if (!string.IsNullOrEmpty(dto.Correo_Electronico)) compraExistente.Correo_Electronico = dto.Correo_Electronico;
        //        if (!string.IsNullOrEmpty(dto.Comentario)) compraExistente.Comentario = dto.Comentario;
        //        if (!string.IsNullOrEmpty(dto.Maquina_Creacion)) compraExistente.Maquina_Creacion = dto.Maquina_Creacion;
        //        if (!string.IsNullOrEmpty(dto.Vigente)) compraExistente.Vigente = dto.Vigente;
        //        if (!string.IsNullOrEmpty(dto.Cancelada)) compraExistente.Cancelada = dto.Cancelada;
        //        if (!string.IsNullOrEmpty(dto.Enviado_Satisfactoriamente)) compraExistente.Enviado_Satisfactoriamente = dto.Enviado_Satisfactoriamente;
        //        if (!string.IsNullOrEmpty(dto.Empresa_Entrega)) compraExistente.Empresa_Entrega = dto.Empresa_Entrega;

        //        if (dto.Fecha_Llegada_producto.HasValue) compraExistente.Fecha_Llegada_producto = dto.Fecha_Llegada_producto;
        //        if (dto.Fecha_Envio_producto.HasValue) compraExistente.Fecha_Envio_producto = dto.Fecha_Envio_producto;

        //        if (!string.IsNullOrEmpty(dto.Estado_producto)) compraExistente.Estado_producto = dto.Estado_producto;
        //        if (!string.IsNullOrEmpty(dto.Identifacion_modifica)) compraExistente.Identifacion_modifica = dto.Identifacion_modifica;
        //        if (!string.IsNullOrEmpty(dto.Maquina_modifica)) compraExistente.Maquina_modifica = dto.Maquina_modifica;

        //        if (!string.IsNullOrEmpty(dto.CelularConIndicativo)) compraExistente.CelularConIndicativo = dto.CelularConIndicativo;
        //        if (!string.IsNullOrEmpty(dto.EnvioCorreo)) compraExistente.EnvioCorreo = dto.EnvioCorreo;
        //        if (dto.CostoEnvio.HasValue) compraExistente.CostoEnvio = dto.CostoEnvio;

        //        if (!string.IsNullOrEmpty(dto.preferenceId)) compraExistente.preferenceId = dto.preferenceId;
        //        if (!string.IsNullOrEmpty(dto.status)) compraExistente.status = dto.status;
        //        if (!string.IsNullOrEmpty(dto.external_reference)) compraExistente.external_reference = dto.external_reference;
        //        if (!string.IsNullOrEmpty(dto.merchant_order_id)) compraExistente.merchant_order_id = dto.merchant_order_id;
        //        if (!string.IsNullOrEmpty(dto.preferenceId_Consulta_Inicial)) compraExistente.preferenceId_Consulta_Inicial = dto.preferenceId_Consulta_Inicial;

        //        // Nuevos campos agregados
        //        if (!string.IsNullOrEmpty(dto.EntregaMercanciaLocal)) compraExistente.EntregaMercanciaLocal = dto.EntregaMercanciaLocal;
        //        if (!string.IsNullOrEmpty(dto.PagoNequi)) compraExistente.PagoNequi = dto.PagoNequi;
        //        if (!string.IsNullOrEmpty(dto.PagoDaviplata)) compraExistente.PagoDaviplata = dto.PagoDaviplata;
        //        if (!string.IsNullOrEmpty(dto.OtroMedioPago)) compraExistente.OtroMedioPago = dto.OtroMedioPago;
        //        if (!string.IsNullOrEmpty(dto.PagoEfectivo)) compraExistente.PagoEfectivo = dto.PagoEfectivo;

        //        // Guardar los cambios en la base de datos
        //        int Resu = await _context.SaveChangesAsync();

        //        // Si la operación fue exitosa, devolver el id de la compra
        //        return respuesta > 0 ? compraExistente.id_compra_realizada : 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log de error para ayudar a depurar el fallo
        //        Console.WriteLine(ex.Message);
        //        respuesta = 0;
        //    }

        //    return respuesta;
        //}
        public async Task<int> F_UpdateCompra(DtoMiCompraRealizada dto)
        {
            var respuesta = 0;

            try
            {
                // Buscar el registro existente de compra
                var compraExistente = await _context.mi_compra_realizada
                    .FirstOrDefaultAsync(x => x.id_compra_realizada == dto.id_compra_realizada);

                if (compraExistente == null)
                {
                    respuesta = 0;
                    return respuesta;
                }

                // Actualizar los campos de la tabla 'mi_compra_realizada'
                if (!string.IsNullOrEmpty(dto.Pais)) compraExistente.Pais = dto.Pais;
                if (!string.IsNullOrEmpty(dto.Nombres)) compraExistente.Nombres = dto.Nombres;
                if (!string.IsNullOrEmpty(dto.Apellidos)) compraExistente.Apellidos = dto.Apellidos;
                if (!string.IsNullOrEmpty(dto.Nombre_Empresa)) compraExistente.Nombre_Empresa = dto.Nombre_Empresa;
                if (!string.IsNullOrEmpty(dto.Direccion)) compraExistente.Direccion = dto.Direccion;
                if (!string.IsNullOrEmpty(dto.Opcional_Direccion)) compraExistente.Opcional_Direccion = dto.Opcional_Direccion;
                if (!string.IsNullOrEmpty(dto.Departamento)) compraExistente.Departamento = dto.Departamento;
                if (!string.IsNullOrEmpty(dto.Ciudad)) compraExistente.Ciudad = dto.Ciudad;
                if (!string.IsNullOrEmpty(dto.Codigo_Postal)) compraExistente.Codigo_Postal = dto.Codigo_Postal;
                if (!string.IsNullOrEmpty(dto.Celular)) compraExistente.Celular = dto.Celular;
                if (!string.IsNullOrEmpty(dto.Correo_Electronico)) compraExistente.Correo_Electronico = dto.Correo_Electronico;
                if (!string.IsNullOrEmpty(dto.Comentario)) compraExistente.Comentario = dto.Comentario;
                if (!string.IsNullOrEmpty(dto.Maquina_Creacion)) compraExistente.Maquina_Creacion = dto.Maquina_Creacion;
                if (!string.IsNullOrEmpty(dto.Vigente)) compraExistente.Vigente = dto.Vigente;
                if (!string.IsNullOrEmpty(dto.Cancelada)) compraExistente.Cancelada = dto.Cancelada;
                if (!string.IsNullOrEmpty(dto.Enviado_Satisfactoriamente)) compraExistente.Enviado_Satisfactoriamente = dto.Enviado_Satisfactoriamente;
                if (!string.IsNullOrEmpty(dto.Empresa_Entrega)) compraExistente.Empresa_Entrega = dto.Empresa_Entrega;
                if (dto.Fecha_Llegada_producto.HasValue) compraExistente.Fecha_Llegada_producto = dto.Fecha_Llegada_producto;
                if (dto.Fecha_Envio_producto.HasValue) compraExistente.Fecha_Envio_producto = dto.Fecha_Envio_producto;
                if (!string.IsNullOrEmpty(dto.Estado_producto)) compraExistente.Estado_producto = dto.Estado_producto;
                if (!string.IsNullOrEmpty(dto.Identifacion_modifica)) compraExistente.Identifacion_modifica = dto.Identifacion_modifica;
                if (!string.IsNullOrEmpty(dto.Maquina_modifica)) compraExistente.Maquina_modifica = dto.Maquina_modifica;
                if (!string.IsNullOrEmpty(dto.CelularConIndicativo)) compraExistente.CelularConIndicativo = dto.CelularConIndicativo;
                if (!string.IsNullOrEmpty(dto.EnvioCorreo)) compraExistente.EnvioCorreo = dto.EnvioCorreo;
                if (dto.CostoEnvio.HasValue) compraExistente.CostoEnvio = dto.CostoEnvio;
                if (!string.IsNullOrEmpty(dto.preferenceId)) compraExistente.preferenceId = dto.preferenceId;
                if (!string.IsNullOrEmpty(dto.status)) compraExistente.status = dto.status;
                if (!string.IsNullOrEmpty(dto.external_reference)) compraExistente.external_reference = dto.external_reference;
                if (!string.IsNullOrEmpty(dto.merchant_order_id)) compraExistente.merchant_order_id = dto.merchant_order_id;
                if (!string.IsNullOrEmpty(dto.preferenceId_Consulta_Inicial)) compraExistente.preferenceId_Consulta_Inicial = dto.preferenceId_Consulta_Inicial;
                if (!string.IsNullOrEmpty(dto.EntregaMercanciaLocal)) compraExistente.EntregaMercanciaLocal = dto.EntregaMercanciaLocal;
                if (!string.IsNullOrEmpty(dto.PagoNequi)) compraExistente.PagoNequi = dto.PagoNequi;
                if (!string.IsNullOrEmpty(dto.PagoDaviplata)) compraExistente.PagoDaviplata = dto.PagoDaviplata;
                if (!string.IsNullOrEmpty(dto.OtroMedioPago)) compraExistente.OtroMedioPago = dto.OtroMedioPago;
                if (!string.IsNullOrEmpty(dto.PagoEfectivo)) compraExistente.PagoEfectivo = dto.PagoEfectivo;

                // **Actualizar solo el campo 'Precio_Envio' de la tabla mi_carrito**
                var carritoItems = _context.mi_carrito
                    .Where(a => a.id_compra_realizada == dto.id_compra_realizada && a.Cancelado == "NO")
                    .ToList();

                foreach (var carrito in carritoItems)
                {
                    // Aquí asumo que el nuevo Precio_Envio es parte de dto y lo tienes como parámetro o lo pasas
                    // Si el campo Precio_Envio es parte del dto y quieres actualizar el carrito con el nuevo valor
                    if (dto.CostoEnvio.HasValue && carrito.Precio_Envio != dto.CostoEnvio.Value)
                    {
                        carrito.Precio_Envio = Convert.ToInt32(dto.CostoEnvio);
                    }
                }

                // Guardar los cambios en la base de datos
                int Resu = await _context.SaveChangesAsync();

                // Si la operación fue exitosa, devolver el id de la compra
                return Resu > 0 ? compraExistente.id_compra_realizada : 0;
            }
            catch (Exception ex)
            {
                // Log de error para ayudar a depurar el fallo
                Console.WriteLine(ex.Message);
                respuesta = 0;
            }

            return respuesta;
        }





    }
}
