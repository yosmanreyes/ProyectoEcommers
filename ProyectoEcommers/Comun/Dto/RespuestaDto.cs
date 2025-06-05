using Comun.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comun.Dto
{
    public class RespuestaDto<T>
    {
        #region Propiedades
        public EstadoOperacion Codigo { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public bool Estado { get => Codigo == EstadoOperacion.Bueno ? true : false; set { } }
        public T Respuesta { get; set; }

        public int Id { get; set; }


        #endregion
    }
}
