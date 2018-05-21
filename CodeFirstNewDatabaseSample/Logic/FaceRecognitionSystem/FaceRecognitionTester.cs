using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;

using Data.Entities;

namespace Data.Logic.FaceRecognitionSystem
{
    public class FaceRecognitionTester
    {
        // Вообще лучше инкапсулировать всё что связано с LDA в отдельный модуль.
        public void TestFromDatabase(Guid frsId)
        {
            var db = new FrcContext();
            var frs = db.FaceRecognitionSystems.Where(x => x.FaceRecognitionSystemId == frsId).FirstOrDefault();

            if (frs == null)
            {
                throw new Exception("Face recognition system is not exist");
            }

            switch (frs.Type)
            {
                case "LDA":
                    testLDA(frs.TypeSystemId, db, frsId);
                    break;
                default:
                    throw new NotImplementedException();
            }

            db.Dispose();
        }

        public User TestFromImage(Guid frsId, string imageByteArray)
        {
            var db = new FrcContext();
            var frs = db.FaceRecognitionSystems.Where(x => x.FaceRecognitionSystemId == frsId).FirstOrDefault();

            if (frs == null)
            {
                throw new Exception("Face recognition system is not exist");
            }

            User result;
            switch (frs.Type)
            {
                case "LDA":
                    result = recognizeLDA(frs.TypeSystemId, db, frsId, imageByteArray);
                    break;
                default:
                    throw new NotImplementedException();
            }

            db.Dispose();
            return result;
        }

        private User recognizeLDA(Guid ldaId, FrcContext db, Guid frsId, string imageByteArray)
        {
            var systemEtalonCount = 1;
            // Копируется в testLDA нужно отрефакторить в более лучшем стиле.
            var ldaEntity = db.LDAs.Where(x => x.LDAId == ldaId).FirstOrDefault();

            if (ldaEntity == null)
            {
                throw new Exception("LDA entity is not exist");
            }

            var averageMatrixString = db.MatrixStrings.Where(x => x.MatrixStringId == ldaEntity.AverageImageMatrixId).FirstOrDefault();
            var leftMatrixString = db.MatrixStrings.Where(x => x.MatrixStringId == ldaEntity.LeftMatrixId).FirstOrDefault();
            var rightMatrixString = db.MatrixStrings.Where(x => x.MatrixStringId == ldaEntity.RightMatrixId).FirstOrDefault();

            var averageMatrix = MatrixHelper.MatrixString2Matrix(averageMatrixString);
            var leftMatrix = MatrixHelper.MatrixString2Matrix(leftMatrixString);
            var rightMatrix = MatrixHelper.MatrixString2Matrix(rightMatrixString);

            var ba = Convert.FromBase64String(imageByteArray);
            var imageMatrix = DenseMatrix.OfArray(ImageHelper.ImageByteArray2pixelArray(ba));
            var featureMatrix = leftMatrix * (imageMatrix - averageMatrix) * rightMatrix;
            var etalonList = db.Etalons.Where(e => e.FaceRecognitionSystemId == frsId).ToList();
            var etalonForUserTempList = etalonList.GroupBy(x => x.UserId).SelectMany(x => x.Take(systemEtalonCount)).ToList();
            var userId = CompareToEtalonList(featureMatrix, etalonForUserTempList, db);
            return db.Users.Where(x => x.UserId == userId).FirstOrDefault();
        }

        private void testLDA(Guid ldaId, FrcContext db, Guid frsId)
        {
            // копируется в FacerecognitionRegistrator нужно вынести в отдельный файл.
            var ldaEntity = db.LDAs.Where(x => x.LDAId == ldaId).FirstOrDefault();

            if (ldaEntity == null)
            {
                throw new Exception("LDA entity is not exist");
            }

            var averageMatrixString = db.MatrixStrings.Where(x => x.MatrixStringId == ldaEntity.AverageImageMatrixId).FirstOrDefault();
            var leftMatrixString = db.MatrixStrings.Where(x => x.MatrixStringId == ldaEntity.LeftMatrixId).FirstOrDefault();
            var rightMatrixString = db.MatrixStrings.Where(x => x.MatrixStringId == ldaEntity.RightMatrixId).FirstOrDefault();

            var averageMatrix = MatrixHelper.MatrixString2Matrix(averageMatrixString);
            var leftMatrix = MatrixHelper.MatrixString2Matrix(leftMatrixString);
            var rightMatrix = MatrixHelper.MatrixString2Matrix(rightMatrixString);

            var testDatabaseUserList = db.DatabaseTestUsers.Where(x => x.FaceRecognitionSystemId == frsId).ToList();
            // лучше добавить и использовать FK:
            List<Guid> test = testDatabaseUserList.Select(x => x.ImageId).ToList();
            var imageList = db.Images.Where(x => test.Any(t => t == x.ImageId)).OrderBy(x => x.User.Username).ToList();

            var etalonList = db.Etalons.Where(e => e.FaceRecognitionSystemId == frsId).ToList();

            var maxEtalonsForUser = etalonList.GroupBy(x => x.UserId).Select(x => x.Count()).FirstOrDefault();
            List<double> resultList = new List<double>();
            for (int i = 0; i < maxEtalonsForUser; i++)
            {
                var etalonForUserTempList = etalonList.GroupBy(x => x.UserId).SelectMany(x => x.Take(i + 1)).ToList();
                var performance = calcPerformance(imageList, leftMatrix, averageMatrix, rightMatrix, db, etalonForUserTempList);
                resultList.Add(performance);
            }
        }

        private double calcPerformance(List<Image> imageList, DenseMatrix leftMatrix, DenseMatrix averageMatrix, DenseMatrix rightMatrix, FrcContext db, List<Etalon> etalonForUserTempList)
        {
            // тестируем изображения лиц:
            int correct = 0;
            foreach (var image in imageList)
            {
                var imageMatrix = DenseMatrix.OfArray(ImageHelper.ImageByteArray2pixelArray(image.ImageByteArray));
                var featureMatrix = leftMatrix * (imageMatrix - averageMatrix) * rightMatrix;
                var userId = CompareToEtalonList(featureMatrix, etalonForUserTempList, db);
                if (userId == image.UserId)
                {
                    correct += 1;
                }
            }
            double performance = (double) correct / imageList.Count * Constants.HUNDRED_PERCENT;
            return performance;
        }

        private Guid CompareToEtalonList(DenseMatrix featureMatrix,  List<Etalon> etalonForUserTempList, FrcContext db)
        {
            var test = etalonForUserTempList.Select(x => x.FeatureMatrixId).ToList();
            var matrixStringList = db.MatrixStrings.Where(x => test.Any(y => y == x.MatrixStringId)).ToList();

            List<double> comparisonDist = new List<double>();
            foreach (var etalonMatrix in matrixStringList)
            {
                var etalon = MatrixHelper.MatrixString2Matrix(etalonMatrix);
                var compare = etalon - featureMatrix;
                var sqrDist = compare.AsColumnMajorArray().Sum(x => x * x);
                comparisonDist.Add(sqrDist);
            }

            var index = comparisonDist.IndexOf(comparisonDist.Min());
            var answerMatrixId = matrixStringList[index].MatrixStringId;
            return db.Etalons.Where(x => x.FeatureMatrixId == answerMatrixId).FirstOrDefault().UserId;
        }
    }
}
