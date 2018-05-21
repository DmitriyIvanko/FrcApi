using System;
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

        public MatrixString[] GetFrsParemeter(Guid frsId)
        {
            var db = new FrcContext();
            var frs = db.FaceRecognitionSystems.Where(x => x.FaceRecognitionSystemId == frsId).FirstOrDefault();

            if (frs == null)
            {
                throw new Exception("Face recognition system is not exist");
            }

            MatrixString[] result;
            switch (frs.Type)
            {
                case "LDA":
                    result = paramLDA(frs.TypeSystemId, db, frsId);
                    break;
                default:
                    throw new NotImplementedException();
            }

            db.Dispose();

            return result;
        }

        private MatrixString[] paramLDA(Guid ldaId, FrcContext db, Guid frsId)
        {
            var ldaEntity = db.LDAs.Where(x => x.LDAId == ldaId).FirstOrDefault();

            if (ldaEntity == null)
            {
                throw new Exception("LDA entity is not exist");
            }

            var averageMatrixString = db.MatrixStrings.Where(x => x.MatrixStringId == ldaEntity.AverageImageMatrixId).FirstOrDefault();
            var leftMatrixString = db.MatrixStrings.Where(x => x.MatrixStringId == ldaEntity.LeftMatrixId).FirstOrDefault();
            var rightMatrixString = db.MatrixStrings.Where(x => x.MatrixStringId == ldaEntity.RightMatrixId).FirstOrDefault();

            return new MatrixString[] { averageMatrixString, leftMatrixString, rightMatrixString };
        }
    }
}
