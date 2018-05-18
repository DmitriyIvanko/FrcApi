using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities;

namespace Data.Repositories
{
    public class FrsRepository
    {
        public List<FaceRecognitionSystem> GetFrsList()
        {
            var db = new FrcContext();
            var frsList = db.FaceRecognitionSystems.Where(x => true).ToList();
            db.Dispose();

            return frsList;
        }
    }
}
