using System.Collections.Generic;
using System.Linq;
using Data.Entities;

namespace Data.Repositories
{
    public class FrsRepository
    {
        public List<FaceRecognitionSystem> GetFrsList()
        {
            var db = new FrcContext();
            var frsList = db.FaceRecognitionSystems.ToList();
            db.Dispose();

            return frsList;
        }
    }
}
