using System;
using System.Collections.Generic;
using System.Linq;
using Data.Entities;

namespace Data.Repositories
{
    public class DatabaseTestUserRepository
    {
        public List<DatabaseTestUser> GetList(Guid frsId)
        {
            var db = new FrcContext();
            var frsList = db.DatabaseTestUsers.Where(x=> x.FaceRecognitionSystemId == frsId).ToList();
            db.Dispose();

            return frsList;
        }
    }
}
