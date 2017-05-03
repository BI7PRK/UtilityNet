using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace UtilityNet.Http
{
    public class HttpResult
    {
        public HttpStatusCode StatusCode { get; set; }

        public string Response { get; set; }

        public string RequesUrl { get; set; }
    }
}
