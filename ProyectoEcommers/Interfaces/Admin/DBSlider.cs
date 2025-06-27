

namespace ProyectoEcommers.Models
{

    using Comun.Dto;
    using Comun.Enumeraciones;
    using FFImageLoading;
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
    using static System.Net.Mime.MediaTypeNames;
    using ImageSharpImage = SixLabors.ImageSharp.Image;

    public class DBSlider : IDbSlider
    {

        #region Propiedades
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly ModelContext _context;
        #endregion

        #region Constructor
        public DBSlider(IConfiguration configuration, ILogger<DbAdministracion> logger, ModelContext context)
        {
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }

        #endregion
        //public async Task<int> F_GetImagen(DtoSlider Imagen)
        //{
        //    var Reultado = 0;
        //   DateTime ahoraUtc = DateTime.Now ;
        //    string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");
        //    using (ModelContext db2 = new ModelContext())
        //    {
        //        try
        //        {
        //            imagenes result = new imagenes
        //            {
        //                CONTENT_TYPE = Imagen.ContentType,
        //                FILENAME = Imagen.FileName,
        //                //FOTO = Convert.FromBase64String(Imagen.FotoBase64),
        //                FOTO = null,
        //                CREADO_POR = Convert.ToInt32(Imagen.Usuario),
        //                FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
        //                URL = Imagen.Ruta,
        //                VIGENTE = 1
        //            };

        //            db2.imagenes.Add(result);
        //            var Resul = await db2.SaveChangesAsync();
        //            db2.SaveChanges();

        //            if (Resul > 0)
        //            {
        //                Reultado = result.CONSECUTIVO;
        //                byte[] imageBytes = Convert.FromBase64String(Imagen.FotoBase64);
        //                var folderPath = _configuration.GetSection("AppSettings").GetRequiredSection("ImagenRuta").Value;
        //                string fileName = Reultado + ".jpg";
        //                ImageService imageService = new ImageService();
        //                string filePath = Path.Combine(folderPath, fileName);
        //                File.WriteAllBytes(filePath, imageBytes);
        //            }
        //            else
        //            {
        //                Reultado = Resul;
        //            }
        //        }
        //        catch (Exception ex) { }

        //        return Reultado;
        //    }
        //}

        //DE ESTA FORMA ELIMINA LA METADATA Y REDIMECIOAN TAMAÑO
        //public async Task<int> F_GetImagen(DtoSlider Imagen)
        //{
        //    var Reultado = 0;
        //    DateTime ahoraUtc = DateTime.Now;
        //    string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");
        //    using (ModelContext db2 = new ModelContext())
        //    {
        //        try
        //        {
        //            imagenes result = new imagenes
        //            {
        //                CONTENT_TYPE = Imagen.ContentType,
        //                FILENAME = Imagen.FileName,
        //                FOTO = null, // No guardar la imagen en la base de datos
        //                CREADO_POR = Convert.ToInt32(Imagen.Usuario),
        //                FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
        //                URL = Imagen.Ruta,
        //                VIGENTE = 1
        //            };

        //            db2.imagenes.Add(result);
        //            var Resul = await db2.SaveChangesAsync();
        //            db2.SaveChanges();

        //            if (Resul > 0)
        //            {
        //                Reultado = result.CONSECUTIVO;
        //                byte[] imageBytes = Convert.FromBase64String(Imagen.FotoBase64);
        //                var folderPath = _configuration.GetSection("AppSettings").GetRequiredSection("ImagenRuta").Value;
        //                string fileName = Reultado + ".jpg";
        //                string filePath = Path.Combine(folderPath, fileName);

        //                // Procesar la imagen para eliminar metadata y reducir tamaño
        //                using (ImageSharpImage image = ImageSharpImage.Load(imageBytes))
        //                {
        //                    // Eliminar la metadata
        //                    image.Metadata.ExifProfile = null;

        //                    // Opcional: Redimensionar la imagen si es necesario
        //                    image.Mutate(x => x.Resize(new ResizeOptions
        //                    {
        //                        Size = new Size(800, 600),
        //                        Mode = ResizeMode.Max
        //                    }));

        //                    // Guardar la imagen sin metadata
        //                    image.Save(filePath, new JpegEncoder { Quality = 75 });
        //                }
        //            }
        //            else
        //            {
        //                Reultado = Resul;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Manejo de excepciones
        //            // Puedes registrar el error o manejarlo según tus necesidades
        //        }

        //        return Reultado;
        //    }
        //}

        //public async Task<int> F_GetImagen(DtoSlider Imagen)
        //{
        //    var Reultado = 0;
        //    DateTime ahoraUtc = DateTime.Now;
        //    string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");
        //    using (ModelContext db2 = new ModelContext())
        //    {
        //        try
        //        {
        //            imagenes result = new imagenes
        //            {
        //                CONTENT_TYPE = Imagen.ContentType,
        //                FILENAME = Imagen.FileName,
        //                FOTO = null, // No guardar la imagen en la base de datos
        //                CREADO_POR = Convert.ToInt32(Imagen.Usuario),
        //                FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
        //                URL = Imagen.Ruta,
        //                VIGENTE = 1
        //            };

        //            db2.imagenes.Add(result);
        //            var Resul = await db2.SaveChangesAsync();
        //            db2.SaveChanges();

        //            if (Resul > 0)
        //            {
        //                Reultado = result.CONSECUTIVO;
        //                byte[] imageBytes = Convert.FromBase64String(Imagen.FotoBase64);
        //                var folderPath = _configuration.GetSection("AppSettings").GetRequiredSection("ImagenRuta").Value;
        //                string fileName = Reultado + ".jpg";
        //                string filePath = Path.Combine(folderPath, fileName);

        //                // Procesar la imagen para eliminar metadata y redimensionar
        //                using (ImageSharpImage image = ImageSharpImage.Load(imageBytes))
        //                {
        //                    // Eliminar la metadata
        //                    image.Metadata.ExifProfile = null;

        //                    // Redimensionar la imagen al tamaño deseado (855x525)
        //                    image.Mutate(x => x.Resize(new ResizeOptions
        //                    {
        //                        Mode = ResizeMode.Crop,
        //                        Size = new Size(855, 525)
        //                    }));

        //                    // Guardar la imagen sin metadata
        //                    image.Save(filePath, new JpegEncoder { Quality = 75 });
        //                }
        //            }
        //            else
        //            {
        //                Reultado = Resul;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Manejo de excepciones
        //            // Puedes registrar el error o manejarlo según tus necesidades
        //        }

        //        return Reultado;
        //    }
        //}

        public async Task<int> F_GetImagen(DtoSlider Imagen)
        {
            int Reultado = 0;
            DateTime ahoraUtc = DateTime.Now;
            string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");

            try
            {
                // Crear la entidad de imagen que se va a guardar en la base de datos
                imagenes result = new imagenes
                {
                    CONTENT_TYPE = Imagen.ContentType,
                    FILENAME = Imagen.FileName,
                    FOTO = null, // No se guarda la imagen directamente en la base de datos
                    CREADO_POR = Convert.ToInt32(Imagen.Usuario),
                    FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
                    URL = Imagen.Ruta,
                    VIGENTE = 1
                };

                // Agregar la imagen a la base de datos
                _context.imagenes.Add(result);
                var Resul = await _context.SaveChangesAsync(); // Guardamos los cambios en la base de datos

                if (Resul > 0)
                {
                    // Si se guardó correctamente, se obtiene el ID de la imagen recién guardada
                    Reultado = result.CONSECUTIVO;

                    // Convertir la imagen en base64 a bytes
                    byte[] imageBytes = Convert.FromBase64String(Imagen.FotoBase64);

                    // Obtener la ruta de almacenamiento de la imagen desde la configuración
                    var folderPath = _configuration.GetSection("AppSettings").GetRequiredSection("ImagenRuta").Value;

                    // Crear el nombre del archivo (usando el ID de la imagen guardada)
                    string fileName = Reultado + ".jpg";
                    string filePath = Path.Combine(folderPath, fileName);

                    // Procesar la imagen (eliminar metadata y redimensionar)
                    using (ImageSharpImage image = ImageSharpImage.Load(imageBytes))
                    {
                        // Eliminar la metadata de la imagen
                        image.Metadata.ExifProfile = null;

                        // Redimensionar la imagen a las dimensiones deseadas (855x525)
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Mode = ResizeMode.Crop,
                            Size = new Size(5088, 3392)
                        }));

                        // Guardar la imagen procesada en la ruta especificada
                        image.Save(filePath, new JpegEncoder { Quality = 75 });
                    }
                }
                else
                {
                    Reultado = Resul; // En caso de error, retornamos el resultado de la operación
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                // Aquí puedes registrar el error o manejarlo según sea necesario
                // Por ejemplo: _logger.LogError(ex, "Error procesando la imagen.");
                Reultado = 0; // Si ocurre un error, retornamos 0
            }

            return Reultado; // Retornamos el resultado de la operación
        }

        public async Task<RespuestaDto<List<DtoSlider>>> F_GetSlide()
        {
            try
            {
                RespuestaDto<List<DtoSlider>> retorno = new RespuestaDto<List<DtoSlider>>();
                var imagen = _context.imagenes
                .Where(u => u.VIGENTE == 1)
                .ToList();
                if (imagen.Count > 0)
                {
                    retorno.Codigo = EstadoOperacion.Bueno;
                    retorno.Respuesta = new List<DtoSlider>();
                    foreach (var img in imagen)
                    {
                        DtoSlider Obj = new DtoSlider
                        {
                            Consecutivo = img.CONSECUTIVO,
                            FileName = img.FILENAME,
                            Vigente = Convert.ToInt32(img.VIGENTE),
                            Identificacion = Convert.ToUInt32(img.CREADO_POR)

                        };
                        retorno.Respuesta.Add(Obj);
                    }
                }
                else retorno.Codigo = EstadoOperacion.Malo;

                return retorno;
            }
            catch (Exception e)
            {
                throw new Exception("Error " + e.Message);
            }
        }
        public async Task<RespuestaDto<List<DtoSlider>>> F_UpdateSlide(DtoSlider Imagen)
        {
            var respuesta = 0;
            RespuestaDto<List<DtoSlider>> retorno = new RespuestaDto<List<DtoSlider>>();

            var query = from ord in _context.imagenes
                        where ord.CONSECUTIVO == Imagen.Consecutivo
                        select ord;

            foreach (var ord in query)
            {
                ord.VIGENTE = Imagen.Vigente;
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

        //Slider Izquierdo
        //public async Task<int> F_GetImagenIzquierdo(DtoSliderizquierdo Imagen)
        //{
        //    var Reultado = 0;
        //    DateTime ahoraUtc = DateTime.Now;
        //    string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");
        //    using (ModelContext db2 = new ModelContext())
        //    {
        //        try
        //        {
        //            imagenesizquierda result = new imagenesizquierda
        //            {
        //                CONTENT_TYPE = Imagen.ContentType,
        //                FILENAME = Imagen.FileName,
        //                //FOTO = Convert.FromBase64String(Imagen.FotoBase64),
        //                FOTO = null,
        //                CREADO_POR = Convert.ToInt32(Imagen.Usuario),
        //                FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
        //                URL = Imagen.Ruta,
        //                VIGENTE = 1
        //            };

        //            db2.imagenesizquierda.Add(result);
        //            var Resul = await db2.SaveChangesAsync();
        //            db2.SaveChanges();

        //            if (Resul > 0)
        //            {
        //                Reultado = result.CONSECUTIVO;
        //                byte[] imageBytes = Convert.FromBase64String(Imagen.FotoBase64);
        //                var folderPath = _configuration.GetSection("AppSettings").GetRequiredSection("ImagenRutaIzquierda").Value;
        //                string fileName = Reultado + ".jpg";
        //                ImageService imageService = new ImageService();
        //                string filePath = Path.Combine(folderPath, fileName);
        //                File.WriteAllBytes(filePath, imageBytes);
        //            }
        //            else
        //            {
        //                Reultado = Resul;
        //            }
        //        }
        //        catch (Exception ex) { }

        //        return Reultado;
        //    }
        //}


        //public async Task<int> F_GetImagenIzquierdo(DtoSliderizquierdo Imagen)
        //{
        //    var Reultado = 0;
        //    DateTime ahoraUtc = DateTime.Now;
        //    string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");
        //    using (ModelContext db2 = new ModelContext())
        //    {
        //        try
        //        {
        //            imagenesizquierda result = new imagenesizquierda
        //            {
        //                CONTENT_TYPE = Imagen.ContentType,
        //                FILENAME = Imagen.FileName,
        //                FOTO = null, // No guardar la imagen en la base de datos
        //                CREADO_POR = Convert.ToInt32(Imagen.Usuario),
        //                FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
        //                URL = Imagen.Ruta,
        //                VIGENTE = 1
        //            };

        //            db2.imagenesizquierda.Add(result);
        //            var Resul = await db2.SaveChangesAsync();
        //            db2.SaveChanges();

        //            if (Resul > 0)
        //            {
        //                Reultado = result.CONSECUTIVO;
        //                byte[] imageBytes = Convert.FromBase64String(Imagen.FotoBase64);
        //                var folderPath = _configuration.GetSection("AppSettings").GetRequiredSection("ImagenRutaIzquierda").Value;
        //                string fileName = Reultado + ".jpg";
        //                string filePath = Path.Combine(folderPath, fileName);

        //                // Procesar la imagen para eliminar metadata y redimensionar
        //                using (ImageSharpImage image = ImageSharpImage.Load(imageBytes))
        //                {
        //                    // Eliminar la metadata
        //                    image.Metadata.ExifProfile = null;

        //                    // Redimensionar la imagen al tamaño deseado (335x252)
        //                    image.Mutate(x => x.Resize(new ResizeOptions
        //                    {
        //                        Mode = ResizeMode.Crop,
        //                        Size = new Size(335, 252)
        //                    }));

        //                    // Guardar la imagen sin metadata
        //                    image.Save(filePath, new JpegEncoder { Quality = 75 });
        //                }
        //            }
        //            else
        //            {
        //                Reultado = Resul;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Manejo de excepciones
        //            // Puedes registrar el error o manejarlo según tus necesidades
        //        }

        //        return Reultado;
        //    }
        //}
        public async Task<int> F_GetImagenIzquierdo(DtoSliderizquierdo Imagen)
        {
            var Reultado = 0;
            DateTime ahoraUtc = DateTime.Now;
            string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");

            try
            {
                // Crear un nuevo objeto imagen en la base de datos
                imagenesizquierda result = new imagenesizquierda
                {
                    CONTENT_TYPE = Imagen.ContentType,
                    FILENAME = Imagen.FileName,
                    FOTO = null, // No guardamos la imagen en la base de datos, solo la referencia
                    CREADO_POR = Convert.ToInt32(Imagen.Usuario),
                    FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
                    URL = Imagen.Ruta,
                    VIGENTE = 1
                };

                // Agregar la nueva imagen a la base de datos
                _context.imagenesizquierda.Add(result);
                var Resul = await _context.SaveChangesAsync(); // Guardar los cambios de la imagen en la base de datos

                // Verificar si la imagen fue guardada correctamente
                if (Resul > 0)
                {
                    Reultado = result.CONSECUTIVO; // Obtener el ID generado de la imagen insertada

                    // Convertir la imagen Base64 a un array de bytes
                    byte[] imageBytes = Convert.FromBase64String(Imagen.FotoBase64);

                    // Obtener la ruta de la carpeta donde se guardarán las imágenes
                    var folderPath = _configuration.GetSection("AppSettings")
                                                    .GetRequiredSection("ImagenRutaIzquierda")
                                                    .Value;

                    string fileName = Reultado + ".jpg"; // Usamos el ID como nombre de archivo
                    string filePath = Path.Combine(folderPath, fileName);

                    // Procesar la imagen para eliminar metadata y redimensionarla
                    using (ImageSharpImage image = ImageSharpImage.Load(imageBytes))
                    {
                        // Eliminar la metadata (Exif) de la imagen
                        image.Metadata.ExifProfile = null;

                        // Redimensionar la imagen a un tamaño específico (335x252)
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Mode = ResizeMode.Crop,
                            Size = new Size(1080, 1920)
                        }));

                        // Guardar la imagen procesada sin metadata en el disco
                        image.Save(filePath, new JpegEncoder { Quality = 75 });
                    }
                }
                else
                {
                    // Si no se guardó, asignamos el resultado a un código de error
                    Reultado = Resul;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error " + ex.Message, ex.InnerException);
                // Manejo de excepciones (podrías registrar el error)
                //Console.WriteLine($"Error al guardar la imagen: {ex.Message}");
                //Reultado = -1; // Error genérico
            }

            return Reultado;
        }

        public async Task<RespuestaDto<List<DtoSliderizquierdo>>> F_GetSlideIzquierdo()
        {
            try
            {
                RespuestaDto<List<DtoSliderizquierdo>> retorno = new RespuestaDto<List<DtoSliderizquierdo>>();
                var imagen = _context.imagenesizquierda
                .Where(u => u.VIGENTE == 1)
                .ToList();
                if (imagen.Count > 0)
                {
                    retorno.Codigo = EstadoOperacion.Bueno;
                    retorno.Respuesta = new List<DtoSliderizquierdo>();
                    foreach (var img in imagen)
                    {
                        DtoSliderizquierdo Obj = new DtoSliderizquierdo
                        {
                            Consecutivo = img.CONSECUTIVO,
                            FileName = img.FILENAME,
                            Vigente = Convert.ToInt32(img.VIGENTE),
                            Identificacion = Convert.ToUInt32(img.CREADO_POR)
                        };
                        retorno.Respuesta.Add(Obj);
                    }
                }
                else
                    retorno.Codigo = EstadoOperacion.Malo;

                return retorno;
            }
            catch (Exception e)
            {
                throw new Exception("Error " + e.Message);
            }
        }
        public async Task<RespuestaDto<List<DtoSliderizquierdo>>> F_UpdateSlideIzquierdo(DtoSliderizquierdo Imagen)
        {
            var respuesta = 0;
            RespuestaDto<List<DtoSliderizquierdo>> retorno = new RespuestaDto<List<DtoSliderizquierdo>>();

            var query = from ord in _context.imagenesizquierda
                        where ord.CONSECUTIVO == Imagen.Consecutivo
                        select ord;

            foreach (var ord in query)
            {
                ord.VIGENTE = Imagen.Vigente;
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

        //Slider Inferior
        //public async Task<int> F_GetImagenInferior(DtoSliderInferior Imagen)
        //{
        //    var Reultado = 0;
        //    DateTime ahoraUtc = DateTime.Now;
        //    string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");
        //    using (ModelContext db2 = new ModelContext())
        //    {
        //        try
        //        {
        //            ImagenesInferior result = new ImagenesInferior
        //            {
        //                CONTENT_TYPE = Imagen.ContentType,
        //                FILENAME = Imagen.FileName,
        //                FOTO = null, // No guardar la imagen en la base de datos
        //                CREADO_POR = Convert.ToInt32(Imagen.Usuario),
        //                FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
        //                URL = Imagen.Ruta,
        //                VIGENTE = 1
        //            };

        //            db2.ImagenesInferior.Add(result);
        //            var Resul = await db2.SaveChangesAsync();
        //            db2.SaveChanges();

        //            if (Resul > 0)
        //            {
        //                Reultado = result.CONSECUTIVO;
        //                byte[] imageBytes = Convert.FromBase64String(Imagen.FotoBase64);
        //                var folderPath = _configuration.GetSection("AppSettings").GetRequiredSection("ImagenRutaInferior").Value;
        //                string fileName = Reultado + ".jpg";
        //                string filePath = Path.Combine(folderPath, fileName);

        //                // Procesar la imagen para eliminar metadata y redimensionar
        //                using (ImageSharpImage image = ImageSharpImage.Load(imageBytes))
        //                {
        //                    // Eliminar la metadata
        //                    image.Metadata.ExifProfile = null;

        //                    // Redimensionar la imagen al tamaño deseado (486x220)
        //                    image.Mutate(x => x.Resize(new ResizeOptions
        //                    {
        //                        Mode = ResizeMode.Crop,
        //                        Size = new Size(486, 220)
        //                    }));

        //                    // Guardar la imagen sin metadata
        //                    image.Save(filePath, new JpegEncoder { Quality = 75 });
        //                }
        //            }
        //            else
        //            {
        //                Reultado = Resul;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Manejo de excepciones
        //            // Puedes registrar el error o manejarlo según tus necesidades
        //        }

        //        return Reultado;
        //    }
        //}

        public async Task<int> F_GetImagenInferior(DtoSliderInferior Imagen)
        {
            var Reultado = 0;
            DateTime ahoraUtc = DateTime.Now;
            string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");

            try
            {
                // Crear un nuevo objeto de imagen en la base de datos
                ImagenesInferior result = new ImagenesInferior
                {
                    CONTENT_TYPE = Imagen.ContentType,
                    FILENAME = Imagen.FileName,
                    FOTO = null, // No guardamos la imagen en la base de datos, solo la referencia
                    CREADO_POR = Convert.ToInt32(Imagen.Usuario),
                    FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
                    URL = Imagen.Ruta,
                    VIGENTE = 1
                };

                // Agregar la nueva imagen en la tabla correspondiente
                _context.ImagenesInferior.Add(result);
                var Resul = await _context.SaveChangesAsync(); // Guardamos los cambios

                // Si la imagen se guardó correctamente en la base de datos
                if (Resul > 0)
                {
                    Reultado = result.CONSECUTIVO; // Obtener el ID de la imagen insertada

                    // Convertir la imagen de Base64 a bytes
                    byte[] imageBytes = Convert.FromBase64String(Imagen.FotoBase64);

                    // Obtener la ruta desde la configuración
                    var folderPath = _configuration.GetSection("AppSettings")
                                                    .GetRequiredSection("ImagenRutaInferior")
                                                    .Value;

                    string fileName = Reultado + ".jpg"; // Usar el ID de la imagen como nombre del archivo
                    string filePath = Path.Combine(folderPath, fileName);

                    // Procesar la imagen (eliminar metadata y redimensionar)
                    using (ImageSharpImage image = ImageSharpImage.Load(imageBytes))
                    {
                        // Eliminar la metadata (Exif)
                        image.Metadata.ExifProfile = null;

                        // Redimensionar la imagen a 486x220
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Mode = ResizeMode.Crop,
                            Size = new Size(1080, 1920)
                        }));

                        // Guardar la imagen procesada en el archivo
                        image.Save(filePath, new JpegEncoder { Quality = 75 });
                    }
                }
                else
                {
                    // Si no se guardó la imagen, devolvemos el resultado
                    Reultado = Resul;
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones (puedes registrar el error si es necesario)
                Console.WriteLine($"Error al guardar la imagen: {ex.Message}");
                Reultado = -1; // Devolvemos un código de error genérico
            }

            return Reultado;
        }
        //public async Task<int> F_GetImagenInferior(DtoSliderInferior Imagen)
        //{
        //    var Reultado = 0;
        //   DateTime ahoraUtc = DateTime.Now ;
        //    string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");
        //    using (ModelContext db2 = new ModelContext())
        //    {

        //        try
        //        {
        //            ImagenesInferior result = new ImagenesInferior
        //            {
        //                CONTENT_TYPE = Imagen.ContentType,
        //                FILENAME = Imagen.FileName,
        //                FOTO = null,
        //                CREADO_POR = Convert.ToInt32(Imagen.Usuario),
        //                FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
        //                URL = Imagen.Ruta,
        //                VIGENTE = 1



        //            };
        //            db2.ImagenesInferior.Add(result);
        //            var Resul = await db2.SaveChangesAsync();
        //            db2.SaveChanges();
        //            if (Resul > 0)
        //            {
        //                Reultado = result.CONSECUTIVO;
        //                byte[] imageBytes = Convert.FromBase64String(Imagen.FotoBase64);
        //                var folderPath = _configuration.GetSection("AppSettings").GetRequiredSection("ImagenRutaInferior").Value;
        //                string fileName = Reultado + ".jpg";
        //                ImageService imageService = new ImageService();
        //                string filePath = Path.Combine(folderPath, fileName);
        //                File.WriteAllBytes(filePath, imageBytes);
        //            }
        //            else
        //            {
        //                Reultado = Resul;
        //            }


        //        }
        //        catch (Exception ex) { }

        //        return Reultado;
        //    }

        //}
        public async Task<RespuestaDto<List<DtoSliderInferior>>> F_GetSlideInferior()

        {
            try
            {

                RespuestaDto<List<DtoSliderInferior>> retorno = new RespuestaDto<List<DtoSliderInferior>>();
                var imagen = _context.ImagenesInferior
                .Where(u => u.VIGENTE == 1)
                .ToList();
                if (imagen.Count > 0)
                {
                    retorno.Codigo = EstadoOperacion.Bueno;
                    retorno.Respuesta = new List<DtoSliderInferior>();
                    foreach (var img in imagen)
                    {
                        DtoSliderInferior Obj = new DtoSliderInferior
                        {
                            Consecutivo = img.CONSECUTIVO,
                            FileName = img.FILENAME,
                            Vigente = Convert.ToInt32(img.VIGENTE),
                            Identificacion = Convert.ToUInt32(img.CREADO_POR)

                        };
                        retorno.Respuesta.Add(Obj);
                    }
                }
                else retorno.Codigo = EstadoOperacion.Malo;

                return retorno;
            }
            catch (Exception e)
            {

                throw new Exception("Error " + e.Message);
            }
        }
        public async Task<RespuestaDto<List<DtoSliderInferior>>> F_UpdateSlideInferior(DtoSliderInferior Imagen)

        {
            var respuesta = 0;
            RespuestaDto<List<DtoSliderInferior>> retorno = new RespuestaDto<List<DtoSliderInferior>>();

            var query = from ord in _context.ImagenesInferior
                        where ord.CONSECUTIVO == Imagen.Consecutivo
                        select ord;

            foreach (var ord in query)
            {

                ord.VIGENTE = Imagen.Vigente;
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

        //Slider Comentarios
        //public async Task<int> F_GetImagenComentarios(DtoSliderComentarios Imagen)
        //{
        //    var Reultado = 0;
        //   DateTime ahoraUtc = DateTime.Now ;
        //    string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");
        //    using (ModelContext db2 = new ModelContext())
        //    {

        //        try
        //        {
        //            imagenesComentarios result = new imagenesComentarios
        //            {
        //                CONTENT_TYPE = Imagen.ContentType,
        //                FILENAME = Imagen.FileName,
        //                //FOTO = Convert.FromBase64String(Imagen.FotoBase64),
        //                FOTO = null,
        //                CREADO_POR = Convert.ToInt32(Imagen.Usuario),
        //                FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
        //                URL = Imagen.Ruta,
        //                VIGENTE = 1,
        //                FECHA_COMENTARIO = Convert.ToDateTime(Imagen.FechaComentario),
        //                TITULOCOMENTARIO = Imagen.TituloComentario,
        //                COMENTARIO = Imagen.Comentario




        //            };
        //            db2.imagenesComentarios.Add(result);
        //            var Resul = await db2.SaveChangesAsync();
        //            db2.SaveChanges();
        //            if (Resul > 0)
        //            {
        //                Reultado = result.CONSECUTIVO;
        //                byte[] imageBytes = Convert.FromBase64String(Imagen.FotoBase64);
        //                var folderPath = _configuration.GetSection("AppSettings").GetRequiredSection("ImagenRutaComentarios").Value;
        //                string fileName = Reultado + ".jpg";
        //                ImageService imageService = new ImageService();
        //                string filePath = Path.Combine(folderPath, fileName);
        //                File.WriteAllBytes(filePath, imageBytes);
        //            }
        //            else
        //            {
        //                Reultado = Resul;
        //            }


        //        }
        //        catch (Exception ex) { }

        //        return Reultado;
        //    }

        //}
        //public async Task<int> F_GetImagenComentarios(DtoSliderComentarios Imagen)
        //{
        //    var Reultado = 0;
        //    DateTime ahoraUtc = DateTime.Now;
        //    string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");
        //    using (ModelContext db2 = new ModelContext())
        //    {
        //        try
        //        {
        //            imagenesComentarios result = new imagenesComentarios
        //            {
        //                CONTENT_TYPE = Imagen.ContentType,
        //                FILENAME = Imagen.FileName,
        //                FOTO = null, // No guardar la imagen en la base de datos
        //                CREADO_POR = Convert.ToInt32(Imagen.Usuario),
        //                FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
        //                URL = Imagen.Ruta,
        //                VIGENTE = 1,
        //                FECHA_COMENTARIO = Convert.ToDateTime(Imagen.FechaComentario),
        //                TITULOCOMENTARIO = Imagen.TituloComentario,
        //                COMENTARIO = Imagen.Comentario
        //            };

        //            db2.imagenesComentarios.Add(result);
        //            var Resul = await db2.SaveChangesAsync();
        //            db2.SaveChanges();

        //            if (Resul > 0)
        //            {
        //                Reultado = result.CONSECUTIVO;
        //                byte[] imageBytes = Convert.FromBase64String(Imagen.FotoBase64);
        //                var folderPath = _configuration.GetSection("AppSettings").GetRequiredSection("ImagenRutaComentarios").Value;
        //                string fileName = Reultado + ".jpg";
        //                string filePath = Path.Combine(folderPath, fileName);

        //                // Procesar la imagen para eliminar metadata y redimensionar
        //                using (ImageSharpImage image = ImageSharpImage.Load(imageBytes))
        //                {
        //                    // Eliminar la metadata
        //                    image.Metadata.ExifProfile = null;

        //                    // Redimensionar la imagen al tamaño deseado (220x132)
        //                    image.Mutate(x => x.Resize(new ResizeOptions
        //                    {
        //                        Mode = ResizeMode.Crop,
        //                        Size = new Size(220, 132)
        //                    }));

        //                    // Guardar la imagen sin metadata
        //                    image.Save(filePath, new JpegEncoder { Quality = 75 });
        //                }
        //            }
        //            else
        //            {
        //                Reultado = Resul;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Manejo de excepciones
        //            // Puedes registrar el error o manejarlo según tus necesidades
        //        }

        //        return Reultado;
        //    }
        //}
        public async Task<int> F_GetImagenComentarios(DtoSliderComentarios Imagen)
        {
            var Reultado = 0;
            DateTime ahoraUtc = DateTime.Now;
            string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");

            try
            {
                // Crear un nuevo objeto para insertar en la base de datos
                imagenesComentarios result = new imagenesComentarios
                {
                    CONTENT_TYPE = Imagen.ContentType,
                    FILENAME = Imagen.FileName,
                    FOTO = null, // No guardar la imagen en la base de datos
                    CREADO_POR = Convert.ToInt32(Imagen.Usuario),
                    FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
                    URL = Imagen.Ruta,
                    VIGENTE = 1,
                    FECHA_COMENTARIO = Convert.ToDateTime(Imagen.FechaComentario),
                    TITULOCOMENTARIO = Imagen.TituloComentario,
                    COMENTARIO = Imagen.Comentario
                };

                // Agregar la nueva imagen en la tabla
                _context.imagenesComentarios.Add(result);
                var Resul = await _context.SaveChangesAsync(); // Guardar cambios en la base de datos

                // Si la imagen se guardó correctamente
                if (Resul > 0)
                {
                    Reultado = result.CONSECUTIVO; // Obtener el ID de la imagen insertada

                    // Convertir la imagen de Base64 a bytes
                    byte[] imageBytes = Convert.FromBase64String(Imagen.FotoBase64);

                    // Obtener la ruta desde la configuración
                    var folderPath = _configuration.GetSection("AppSettings")
                                                    .GetRequiredSection("ImagenRutaComentarios")
                                                    .Value;

                    string fileName = Reultado + ".jpg"; // Usar el ID generado como nombre del archivo
                    string filePath = Path.Combine(folderPath, fileName);

                    // Procesar la imagen: eliminar metadata y redimensionar
                    using (ImageSharpImage image = ImageSharpImage.Load(imageBytes))
                    {
                        // Eliminar la metadata (Exif)
                        image.Metadata.ExifProfile = null;

                        // Redimensionar la imagen al tamaño deseado (220x132)
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Mode = ResizeMode.Crop,
                            Size = new Size(220, 132)
                        }));

                        // Guardar la imagen procesada en el archivo
                        image.Save(filePath, new JpegEncoder { Quality = 75 });
                    }
                }
                else
                {
                    Reultado = Resul; // Si no se guardó la imagen, devolvemos el resultado
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                // Puedes registrar el error o manejarlo según tus necesidades
                Console.WriteLine($"Error al guardar la imagen: {ex.Message}");
                Reultado = -1; // Devolvemos un código de error genérico
            }

            return Reultado;
        }


        public async Task<RespuestaDto<List<DtoSliderComentarios>>> F_GetSlideComentarios()

        {
            try
            {

                RespuestaDto<List<DtoSliderComentarios>> retorno = new RespuestaDto<List<DtoSliderComentarios>>();
                var imagen = _context.imagenesComentarios
                .Where(u => u.VIGENTE == 1)
                .ToList();
                if (imagen.Count > 0)
                {
                    retorno.Codigo = EstadoOperacion.Bueno;
                    retorno.Respuesta = new List<DtoSliderComentarios>();
                    foreach (var img in imagen)
                    {
                        DtoSliderComentarios Obj = new DtoSliderComentarios
                        {
                            Consecutivo = img.CONSECUTIVO,
                            FileName = img.FILENAME,
                            Vigente = Convert.ToInt32(img.VIGENTE),
                            Identificacion = Convert.ToUInt32(img.CREADO_POR)

                        };
                        retorno.Respuesta.Add(Obj);
                    }
                }
                else retorno.Codigo = EstadoOperacion.Malo;

                return retorno;
            }
            catch (Exception e)
            {

                throw new Exception("Error " + e.Message);
            }
        }
        public async Task<RespuestaDto<List<DtoSliderComentarios>>> F_UpdateSlideComentarios(DtoSliderComentarios Imagen)

        {
            var respuesta = 0;
            RespuestaDto<List<DtoSliderComentarios>> retorno = new RespuestaDto<List<DtoSliderComentarios>>();

            var query = from ord in _context.imagenesComentarios
                        where ord.CONSECUTIVO == Imagen.Consecutivo
                        select ord;

            foreach (var ord in query)
            {

                ord.VIGENTE = Imagen.Vigente;
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

        //slider Mensajes
        //public async Task<int> Ins_Mensaje(DtoMensajes _mensaje)
        //{
        //    var Reultado = 0;

        //   DateTime ahoraUtc = DateTime.Now ;
        //    string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");
        //    using (ModelContext db2 = new ModelContext())
        //    {

        //        try
        //        {
        //            ctr_mensajes result = new ctr_mensajes
        //            {
        //                MAQUINA_CREACION = _mensaje.MaquinaCreacion,
        //                CREADO_POR = _mensaje.CreadoPor,
        //                FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
        //                VIGENTE = _mensaje.Vigente,
        //                FECHA_MENSAJE = Convert.ToDateTime(fechaHoraFormateada),
        //                TITULOMENSAJES = _mensaje.TituloMensaje,
        //                MENSAJE = _mensaje.Mensaje
        //            };
        //            db2.ctr_mensajes.Add(result);
        //            var Resul = await db2.SaveChangesAsync();

        //            if (Resul > 0)
        //            {
        //                Reultado = Resul;
        //            }
        //            else
        //            {
        //                Reultado = Resul;

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //        return Reultado;
        //    }

        //}

        public async Task<int> Ins_Mensaje(DtoMensajes _mensaje)
        {
            var Reultado = 0;

            DateTime ahoraUtc = DateTime.Now;
            string fechaHoraFormateada = ahoraUtc.ToString("yyyy-MM-dd HH:mm:ss");

            try
            {
                // Crear un nuevo mensaje para insertar en la base de datos
                ctr_mensajes result = new ctr_mensajes
                {
                    MAQUINA_CREACION = _mensaje.MaquinaCreacion,
                    CREADO_POR = _mensaje.CreadoPor,
                    FECHA_CREACION = Convert.ToDateTime(fechaHoraFormateada),
                    VIGENTE = _mensaje.Vigente,
                    FECHA_MENSAJE = Convert.ToDateTime(fechaHoraFormateada),
                    TITULOMENSAJES = _mensaje.TituloMensaje,
                    MENSAJE = _mensaje.Mensaje
                };

                // Agregar el mensaje a la base de datos
                _context.ctr_mensajes.Add(result);
                var Resul = await _context.SaveChangesAsync();  // Guardar los cambios en la base de datos

                // Si se guardó correctamente
                if (Resul > 0)
                {
                    Reultado = Resul;  // Devuelve el resultado si fue exitoso
                }
                else
                {
                    Reultado = Resul;  // Si no fue exitoso, devolvemos el mismo resultado
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones, por ejemplo, puedes registrar el error
                Console.WriteLine($"Error al guardar el mensaje: {ex.Message}");
                Reultado = -1;  // Código de error genérico
            }

            return Reultado;
        }

        public async Task<RespuestaDto<List<DtoMensajes>>> F_GetMensajeGrilla()

        {
            try
            {

                RespuestaDto<List<DtoMensajes>> retorno = new RespuestaDto<List<DtoMensajes>>();
                var imagen = _context.ctr_mensajes
                .Where(u => u.VIGENTE == "SI")
                .ToList();
                if (imagen.Count > 0)
                {
                    retorno.Codigo = EstadoOperacion.Bueno;
                    retorno.Respuesta = new List<DtoMensajes>();
                    foreach (var img in imagen)
                    {
                        DtoMensajes Obj = new DtoMensajes
                        {
                            Consecutivo = img.CONSECUTIVO,
                            Mensaje = img.MENSAJE,
                            TituloMensaje = img.TITULOMENSAJES,
                            Vigente = img.VIGENTE
                        };
                        retorno.Respuesta.Add(Obj);
                    }
                }
                else retorno.Codigo = EstadoOperacion.Malo;

                return retorno;
            }
            catch (Exception e)
            {
                throw new Exception("Error " + e.Message);
            }
        }
        public async Task<RespuestaDto<List<DtoMensajes>>> InsIdMensaje(DtoMensajes obj)

        {
            var respuesta = 0;
            RespuestaDto<List<DtoMensajes>> retorno = new RespuestaDto<List<DtoMensajes>>();

            var query = from ord in _context.ctr_mensajes
                        where ord.CONSECUTIVO == obj.Consecutivo
                        select ord;

            foreach (var ord in query)
            {

                ord.MAQUINA_ACTUALIZA = obj.MaquinaCreacion;
                ord.VIGENTE = obj.Vigente;
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



    }
}
