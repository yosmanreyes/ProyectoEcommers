using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comun.Dto
{
    public class ApiGatewayUrl
    {

        public ApiGatewayUrl(string url)
        {

        }
        public ApiGatewayUrl()
        {

        }



        #region Propiedades para Api Externas

        public string PipBaseUrl { get; set; }
        public string PostPipToken { get; set; }
        public string? CarruselImagenes { get; set; }
        public string? Oud { get; set; }

        #endregion

    }

}
