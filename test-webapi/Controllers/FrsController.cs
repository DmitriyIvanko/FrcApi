﻿using System.Collections.Generic;
using System.Web.Http;

using Data.Entities;
using Data.Repositories;

namespace test_webapi.Controllers
{
    public class FrsController : ApiController
    {
        [HttpGet]
        [ActionName("get-frs-list")]
        public List<FaceRecognitionSystem> GetFrsList()
        {
            var repository = new FrsRepository();
            var frsList =  repository.GetFrsList();
            return frsList;
        }
    }
}