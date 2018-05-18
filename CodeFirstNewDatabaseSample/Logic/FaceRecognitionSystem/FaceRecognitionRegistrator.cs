using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

using Data.Logic;

namespace Data.Logic.FaceRecognitionSystem
{
    public class FaceRecognitionRegistrator
    {
        // to do: продумать как можно лучше работать с массивами пользователей, 
        // чтобы не пришлось по сто раз извлекать для каждой регистрации в БД.
        public void RegisterUserList(List<Guid> imageIdList, Guid frsId)
        {
            // 

            var db = new FrcContext();
            var frs = db.FaceRecognitionSystems.Where(x => x.FaceRecognitionSystemId == frsId).FirstOrDefault();

            if (frs == null)
            {
                throw new Exception("Face recognition system is not exist");
            }

            switch (frs.Type)
            {
                case "LDA":
                    registerLDA(frs.TypeSystemId, db, imageIdList, frsId);
                    break;
                default:
                    throw new NotImplementedException();
            }

            db.Dispose();
        }

        private void registerLDA(Guid ldaId, FrcContext db, List<Guid> imageIdList, Guid frsId)
        {
            var ldaEntity = db.LDAs.Where(x => x.LDAId == ldaId).FirstOrDefault();

            if (ldaEntity == null)
            {
                throw new Exception("LDA entity is not exist");
            }

            var averageMatrixString = db.MatrixStrings.Where(x => x.MatrixStringId == ldaEntity.AverageImageMatrixId).FirstOrDefault();
            var leftMatrixString = db.MatrixStrings.Where(x => x.MatrixStringId == ldaEntity.LeftMatrixId).FirstOrDefault();
            var rightMatrixString = db.MatrixStrings.Where(x => x.MatrixStringId == ldaEntity.RightMatrixId).FirstOrDefault();

            var averageMatrix = MatrixString2Matrix(averageMatrixString);
            var leftMatrix = MatrixString2Matrix(leftMatrixString);
            var rightMatrix = MatrixString2Matrix(rightMatrixString);

            foreach (var imageId in imageIdList)
            {
                var imageEntity = db.Images.Where(x => x.ImageId == imageId).FirstOrDefault();

                if (imageEntity == null)
                {
                    continue;
                }
                var imageMatrix = DenseMatrix.OfArray(ImageHelper.ImageByteArray2pixelArray(imageEntity.ImageByteArray));
                var featureMatrix = leftMatrix * (imageMatrix - averageMatrix) * rightMatrix;
                var featureMatrixString = MatrixHelper.convertToMatrixString(featureMatrix);

                db.MatrixStrings.Add(featureMatrixString);
                var etalon = new Entities.Etalon
                {
                    EtalonId = Guid.NewGuid(),
                    FaceRecognitionSystemId = frsId,
                    FeatureMatrixId = featureMatrixString.MatrixStringId,
                    ImageId = imageEntity.ImageId,
                    RegistredDT = DateTime.UtcNow,
                    UserId = imageEntity.UserId,
                };

                db.Etalons.Add(etalon);
            }

            db.SaveChanges();
        }

        private void registerUserLDA()
        {

        }

        private DenseMatrix MatrixString2Matrix(Entities.MatrixString matrixString)
        {
            var valStrArray = matrixString.Value.Split(Constants.MATRIX_SEPARATOR);

            double[,] result = new double[matrixString.DimentionOne, matrixString.DimentionTwo];
            for (int i = 0; i < matrixString.DimentionOne; i++)
            {
                for (int j = 0; j < matrixString.DimentionTwo; j++)
                {
                    result[i, j] = Convert.ToDouble(valStrArray[i * matrixString.DimentionTwo + j]);
                }
            }

            return DenseMatrix.OfArray(result);
        }
    }
}
