using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace test_webapi.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        [ActionName("ping")]
        public int GetFrsList()
        {
            return 0;
        }
    }
}
