using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using test_webapi.Dto;

namespace test_webapi.Controllers
{
    public class PingController : ApiController
    {
        [HttpGet]
        [ActionName("simple")]
        public int Simple()
        {
            return 0;
        }

        [HttpPost]
        [ActionName("with-load")]
        public int WithLoad([FromBody] PingDto pingDto)
        {
            return 0;
        }
    }
}
