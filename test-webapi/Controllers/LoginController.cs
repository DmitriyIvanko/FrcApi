﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CodeFirstNewDatabaseSample;

namespace test_webapi.Controllers
{
    public class LoginController : ApiController
    {
        [HttpPost]
        [ActionName("sign-in")]
        public int GetPing()
        {
            // var unitWork = new WorkWithImageDatabase();
            // unitWork.start();

            var a = 5;
            return a;
        }
    }
}
