using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Drawing;

using Data.Logic;
using Data.Entities;

namespace Data.Logic.FaceRecognitionSystem
{
    public class FaceRecognitionSystemBuilder
    {
        private int totalTrainImageForUser;
        private int totalUserForTrain;
        private MnemonicDescriptionModel md;
        private List<List<Matrix<double>>> trainImageLists = new List<List<Matrix<double>>>();

        public FaceRecognitionSystemBuilder(MnemonicDescriptionModel _md)
        {
            md = _md;
        }

        public Guid Create()
        {
            var db = new FrcContext();
            var imdb = db.ImageDatabases.Where(x => x.DatabaseName == md.databaseName).FirstOrDefault();

            if (imdb == null)
            {
                throw new Exception("Image database is not exist");
            }

            totalTrainImageForUser = (int) Math.Floor(
                (double) imdb.TotalImageForUser * (Constants.HUNDRED_PERCENT - md.databaseTestImagesPercent) / Constants.HUNDRED_PERCENT);
            totalUserForTrain = (int) Math.Floor(
                (double) imdb.TotalUser * (Constants.HUNDRED_PERCENT - md.databaseTestUsersForOpenTaskPercent) / Constants.HUNDRED_PERCENT);

            if (totalTrainImageForUser == 0 || totalUserForTrain == 0)
            {
                throw new Exception("Not enaught images for user");
            }

            // to do: обращаться ко всем элементам не очень так как для больших БД может не хватить памяти:
            var userListOfListsForTrain = db.Users.OrderBy(x => x.Username).Take(totalUserForTrain)
                .Select(x => x.Images.OrderBy(y => y.ImageName).Take(totalTrainImageForUser)).ToList();

            foreach (var userImageList in userListOfListsForTrain)
            {
                List<Matrix<double>> userMatrixList = new List<Matrix<double>>();
                foreach (var userImage in userImageList)
                {
                    var imageMatrix = ImageHelper.ImageByteArray2pixelArray(userImage.ImageByteArray);
                    userMatrixList.Add(DenseMatrix.OfArray(imageMatrix));
                }
                trainImageLists.Add(userMatrixList);
            }

            Guid frsId;
            switch (md.trainName)
            {
                case "LDA":
                    frsId = trainLDA(imdb, db);
                    break;
                default:
                    throw new NotImplementedException();
            }

            // Записать все изображения пользователей, которые предназначены для тестирования БД на закрытом множестве.
            var userListOfListsForTest = db.Users.OrderBy(x => x.Username).Take(totalUserForTrain)
                .Select(x => x.Images.OrderBy(y => y.ImageName).Skip(totalTrainImageForUser)).ToList();

            var testList = userListOfListsForTest.SelectMany(x => x.Select(i => new DatabaseTestUser
            {
                DatabaseTestUserId = Guid.NewGuid(),
                FaceRecognitionSystemId = frsId,
                ImageId = i.ImageId,
            })).ToList();

            db.DatabaseTestUsers.AddRange(testList);
            db.SaveChanges();

            db.Dispose();

            // Зарегестрировать все изображения пользователей, которые учавствовали в обучении.
            var registrator = new FaceRecognitionRegistrator();
            var imageIdList = userListOfListsForTrain.SelectMany(x => x.Select(y => y.ImageId)).ToList();
            registrator.RegisterUserList(imageIdList, frsId);

            return frsId;
        }

        private Guid trainLDA(ImageDatabase imdb, FrcContext db)
        {
            if (!imdb.isSameImageSize || !imdb.isSameTotalImageForUser)
            {
                throw new NotImplementedException();
            }

            // 1) Находим средний образ для всей базы исходных данных:
            var averageMatrix = Matrix<double>.Build.Dense(imdb.ImageHeight, imdb.ImageWidth);

            foreach (var userMatrixList in trainImageLists)
            {
                foreach (var userImage in userMatrixList)
                {
                    averageMatrix = averageMatrix + userImage;
                }
            }
            var Xaverage = averageMatrix / (totalUserForTrain * totalTrainImageForUser);

            // 2) Находим средний образ для каждого класса:
            List<Matrix<double>> XclassAverageList = new List<Matrix<double>>();
            foreach (var userMatrixList in trainImageLists)
            {
                var averageClassMatrix = Matrix<double>.Build.Dense(imdb.ImageHeight, imdb.ImageWidth);
                foreach (var userImage in userMatrixList)
                {
                    averageClassMatrix = averageClassMatrix + userImage;
                }
                averageClassMatrix = averageClassMatrix / totalTrainImageForUser;
                XclassAverageList.Add(averageClassMatrix);
            }

            // 3) Вычислим матрицу внутриклассовой(w) ковариации относительно строк(R):
            var SwR = Matrix<double>.Build.Dense(imdb.ImageHeight, imdb.ImageHeight);
            for (int i = 0; i < trainImageLists.Count; i++)
            {
                foreach (var userImage in trainImageLists[i])
                {
                    SwR = SwR + (userImage - XclassAverageList[i]) * (userImage - XclassAverageList[i]).Transpose();
                }
            }

            // 4) Вычислим матрицу межклассовой(b) ковариации относительно строк(R):
            var SbR = Matrix<double>.Build.Dense(imdb.ImageHeight, imdb.ImageHeight);
            foreach (var XclassAverage in XclassAverageList)
            {
                SbR = SbR + (XclassAverage - Xaverage) * (XclassAverage - Xaverage).Transpose();
            }

            // 5)  Вычислим матрицу внутриклассовой (w) ковариации относительно столбцов (C):
            var SwC = Matrix<double>.Build.Dense(imdb.ImageWidth, imdb.ImageWidth);
            for (int i = 0; i < trainImageLists.Count; i++)
            {
                foreach (var userImage in trainImageLists[i])
                {
                    SwC = SwC + (userImage - XclassAverageList[i]).Transpose() * (userImage - XclassAverageList[i]);
                }
            }

            // 6) Вычислим матрицу межклассовой (b) ковариации относительно столбцов (C):
            var SbC = Matrix<double>.Build.Dense(imdb.ImageWidth, imdb.ImageWidth);
            foreach (var XclassAverage in XclassAverageList)
            {
                SbC = SbC + (XclassAverage - Xaverage).Transpose() * (XclassAverage - Xaverage);
            }

            // 7) Сформируем общую матрицу рассеяния относительно строк:
            var StotalR = SwR.Inverse() * SbR;
            //    Регуляризация (стр. 349):
            StotalR = StotalR + Constants.SMALL_VALUE * Matrix<double>.Build.Dense(imdb.ImageHeight, imdb.ImageHeight);

            // 8) Сформируем общую матрицу рассеяния относительно столбцов:
            var StotalC = SwC.Inverse() * SbC;
            //    Регуляризация (стр. 349):
            StotalC = StotalC + Constants.SMALL_VALUE * Matrix<double>.Build.Dense(imdb.ImageWidth, imdb.ImageWidth);

            // 9) Решим задачу на собственные значения:
            var ReigResult = StotalR.Evd();
            var CeigResult = StotalC.Evd();

            // 10) Пропускаем сортировку по eigenvalues и ограничение по 95%. Берём только то, что написано в мнемоническом описании:

            // 11) Подготовка матриц для преобразования Карунена-Лоева:
            List<Vector<double>> rVectorList = new List<Vector<double>>();
            for (int i = 0; i < md.trainMartixRightDimension; i++)
            {
                rVectorList.Add(ReigResult.EigenVectors.Column(i));
            }
            // to do: значения матриц R как-то оказываются слева. Нужно переименовывать везде, а лучше ещё раз уточнить формулы:
            var eigMatrixLeft = Matrix<double>.Build.DenseOfColumnVectors(rVectorList.ToArray()).Transpose(); 

            List<Vector<double>> cVectorList = new List<Vector<double>>();
            for (int i = 0; i < md.trainMartixLeftDimension; i++)
            {
                cVectorList.Add(CeigResult.EigenVectors.Column(i));
            }
            var eigMatrixRight = Matrix<double>.Build.DenseOfColumnVectors(cVectorList.ToArray());

            var averageImageMatrixString = MatrixHelper.convertToMatrixString(Xaverage);
            var leftMatrixString = MatrixHelper.convertToMatrixString(eigMatrixLeft);
            var rightMatrixString = MatrixHelper.convertToMatrixString(eigMatrixRight);

            db.MatrixStrings.Add(averageImageMatrixString);
            db.MatrixStrings.Add(leftMatrixString);
            db.MatrixStrings.Add(rightMatrixString);
            db.SaveChanges();

            var ldaEntity = new LDA
            {
                AverageImageMatrixId = averageImageMatrixString.MatrixStringId,
                LDAId = Guid.NewGuid(),
                LeftMatrixId = leftMatrixString.MatrixStringId,
                RightMatrixId = rightMatrixString.MatrixStringId,
            };

            db.LDAs.Add(ldaEntity);
            db.SaveChanges();

            var frs = new Entities.FaceRecognitionSystem
            {
                FaceRecognitionSystemId = Guid.NewGuid(),
                MnemonicDescription = md.originalDescription,
                Type = "LDA",
                TypeSystemId = ldaEntity.LDAId,
                InputImageHeight = imdb.ImageHeight,
                InputImageWidth = imdb.ImageWidth,
                CreatedDT = DateTime.UtcNow,
            };

            db.FaceRecognitionSystems.Add(frs);
            db.SaveChanges();

            return frs.FaceRecognitionSystemId;
        }
    }
}
