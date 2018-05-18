using System;
using System.Collections.Generic;
using System.Web.Http;

using Data.Entities;
using Data.Repositories;

namespace test_webapi.Controllers
{
    public class DatabaseTestUserController : ApiController
    {
        [HttpGet]
        [ActionName("get-list")]
        public List<DatabaseTestUser> GetList([FromUri] Guid frsId)
        {
            var repository = new DatabaseTestUserRepository();
            var databaseTestUserList =  repository.GetList(frsId);
            return databaseTestUserList;
        }
    }
}
