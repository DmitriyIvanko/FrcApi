using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using CodeFirstNewDatabaseSample.Entities;

namespace CodeFirstNewDatabaseSample
{
    class Program
    {
        public static ImageDatabase imageDatabase;
        public static List<User> userList = new List<User>();
        public static List<Entities.Image> imageList = new List<Entities.Image>();

        static void Main(string[] args)
        {
            string imageDatabaseName = "ORL";
            string inputDatabaseDir = @"C:\Projects\frc\AT&T_ORL_database_JPEG\";

            var db = new FrcContext();
            var orl = db.ImageDatabases.Where(x => x.DatabaseName == imageDatabaseName).FirstOrDefault();

            if (orl != null)
            {
                Console.WriteLine("The database {1} already added;", imageDatabaseName);
                Console.ReadKey();

                db.Dispose();
                return;
            }

            analysisDatabase(inputDatabaseDir, imageDatabaseName);

            db.ImageDatabases.Add(imageDatabase);
            db.Users.AddRange(userList.AsEnumerable());
            db.Images.AddRange(imageList.AsEnumerable());
            db.SaveChanges();

            db.Dispose();


            //     string[] imageClassDirArray = Directory.GetDirectories(inputDatabaseDir);
            //     var imageDatabase = new ImageDatabase{
            //         DatabaseName = ImageDatabaseName,
            //         ImageDatabaseId = Guid.NewGuid(),
            //     };
            //     db.ImageDatabases.Add(imageDatabase);
            //     db.SaveChanges();
            //    
            //     foreach (string imageClassDir in imageClassDirArray)
            //     {
            //         string imageClassName = imageClassDir.Substring(imageClassDir.LastIndexOf('\\') + 1);
            //    
            //         string[] imageFileDirArray = Directory.GetFiles(inputDatabaseDir + imageClassName);
            //    
            //         var user = new User
            //         {
            //             Username = imageClassName,
            //             ImageDatabaseId = imageDatabase.ImageDatabaseId,
            //         };
            //         db.Users.Add(user);
            //         db.SaveChanges();
            //    
            //         foreach (string imageFile in imageFileDirArray)
            //         {
            //             Image img = Image.FromFile(imageFile);
            //             byte[] arr;
            //             using (MemoryStream ms = new MemoryStream())
            //             {
            //                 img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            //                 arr = ms.ToArray();
            //             }
            //    
            //             var photoImage = new Photo
            //             {
            //                 Format = "JPEG",
            //                 Image = arr,
            //                 UserId = user.UserId,
            //             };
            //             db.Photos.Add(photoImage);
            //             db.SaveChanges();
            //         }
            //     }
            //
            //     Console.WriteLine("Press any key to exit...");
            //     Console.ReadKey();
            // }
        }

        public static void analysisDatabase(string inputDatabaseDir, string imageDatabaseName)
        {
            string[] imageClassDirArray = Directory.GetDirectories(inputDatabaseDir);
            imageDatabase = new ImageDatabase
            {
                DatabaseName = imageDatabaseName,
                ImageDatabaseId = Guid.NewGuid(),
                TotalUser = imageClassDirArray.Length,
            };

            List<int> imagesUserList = new List<int>();
            List<int> imageHeightList = new List<int>();
            List<int> imageWidthList = new List<int>();
            for (int i = 0; i < imageClassDirArray.Length; i++)
            {
                string imageUserName = imageClassDirArray[i].Substring(imageClassDirArray[i].LastIndexOf('\\') + 1);
                string[] imageFileDirArray = Directory.GetFiles(inputDatabaseDir + imageUserName);
                imagesUserList.Add(imageFileDirArray.Length);
                var user = new User
                {
                    Username = imageUserName,
                    ImageDatabaseId = imageDatabase.ImageDatabaseId,
                    UserId = Guid.NewGuid(),
                };
                userList.Add(user);

                for (int j = 0; j < imageFileDirArray.Length; j++)
                {
                    string imageName = imageFileDirArray[j].Substring(imageFileDirArray[j].LastIndexOf('\\') + 1);
                    System.Drawing.Image img = System.Drawing.Image.FromFile(imageFileDirArray[j]);
                    imageHeightList.Add(img.Height);
                    imageWidthList.Add(img.Width);
                    byte[] arr;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        arr = ms.ToArray();
                    }

                    var photoImage = new Entities.Image
                    {
                        Format = "JPEG",
                        ImageByteArray = arr,
                        UserId = user.UserId,
                        ImageId = Guid.NewGuid(),
                        ImageName = imageName,
                    };
                    imageList.Add(photoImage);
                }
            }

            if (isSameValueInArray(imagesUserList)) {
                imageDatabase.isSameTotalImageForUser = true;
                imageDatabase.TotalImageForUser = imagesUserList.First();
            }

            if (isSameValueInArray(imageHeightList) && isSameValueInArray(imageWidthList))
            {
                imageDatabase.isSameImageSize = true;
                imageDatabase.ImageHeight = imageHeightList.First();
                imageDatabase.ImageWidth = imageWidthList.First();
            }
        }

        public static bool isSameValueInArray(List<int> array)
        {
            if (array == null || array.Count == 0)
            {
                return false;
            }

            var etalon = array[0];

            foreach (var value in array)
            {
                if (value != etalon)
                {
                    return false;
                }
            }

            return true;
        }

        // public class WorkWithImageDatabase
        // {
        //     public void start()
        //     {
        //         var DB = "ORL";
        //         var T = 30;
        //         var OT = 25;
        //         var TH = 50;
        //         int H = 112;
        //         int W = 92;
        //         int trainPhotoUserNum = 0;
        // 
        //         using (var db = new BloggingContext())
        //         {
        //             // var selectedIdb =  db.ImageDatabases.Where(x => x.DatabaseName == DB).FirstOrDefault();
        //             // 
        //             // var userList = db.Users.Where(x => x.ImageDatabaseId == selectedIdb.ImageDatabaseId).ToList();
        //             // var trainClassesNum = (int) Math.Round((double) userList.Count / 100 * (100 - OT));
        //             // var trainUserList = userList.Take(trainClassesNum).ToList();
        //             // 
        //             // // List<List<Photo>> trainClassPhotoList = new List<List<Photo>>();
        //             // Matrix<double>[,] trainMatrix = new Matrix<double>[,];
        // 
        //             // foreach (var trainUser in trainUserList)
        //             // {
        //             //     var photoUserList = db.Photos.Where(x => x.UserId == trainUser.UserId).ToList();
        //             //     trainPhotoUserNum = (int)Math.Round((double)photoUserList.Count / 100 * (100 - T));
        //             //     var trainPhotoUserList = photoUserList.Take(trainPhotoUserNum).ToList();
        //             //     
        //             //     foreach (var trainPhotoUser in trainPhotoUserList)
        //             //     {
        //             //         DenseMatrix.OfArray(imageByteToPixelArray(trainPhotoUser));
        //             //     }
        //             //     // trainClassPhotoList.Add(trainPhotoUserList);
        //             //     // trainPhotoList = trainPhotoList.Concat(trainPhotoUserList).ToList();
        //             // }
        // 
        //             for (int i = 0; i < trainUserList.Count(); i++)
        //             {
        //                 var photoUserList = db.Photos.Where(x => x.UserId == trainUserList[i].UserId).ToList();
        //                 trainPhotoUserNum = (int)Math.Round((double)photoUserList.Count / 100 * (100 - T));
        //                 var trainPhotoUserList = photoUserList.Take(trainPhotoUserNum).ToList();
        // 
        //             }
        // 
        //             // foreach (var classList in trainClassPhotoList)
        //             // {
        //             //     var XclassAverage = getAverage(classList);
        //             //     foreach (var classPhoto in classList)
        //             //     {
        //             //         // classPhoto;
        //             //     }
        //             // }
        //             // var Xaverage = getAverage(trainPhotoList);
        //         }
        //     }
        // 
        //     public double[,] imageByteToPixelArray(Photo photo)
        //     {
        //         double[,] result = new double[0, 0];
        //         using (var ms = new MemoryStream(photo.Image))
        //         {
        //             Bitmap trainPhotoImage = new Bitmap(Image.FromStream(ms));
        //             result = new double[trainPhotoImage.Height, trainPhotoImage.Width];
        //             for (int x = 0; x < trainPhotoImage.Width; x++)
        //             {
        //                 for (int y = 0; y < trainPhotoImage.Height; y++)
        //                 {
        //                     Color pixelColor = trainPhotoImage.GetPixel(x, y);
        //                     result[y, x] = pixelColor.R;
        //                 }
        //             }
        //         }
        // 
        //         return result;
        //             // Bitmap trainPhotoImage = new Bitmap(Image.FromStream(photo.Image));
        //     }
        // 
        //     // private double[,] getAverage(IEnumerable<Photo> photoList)
        //     // {
        //     //     int H = 112;
        //     //     int W = 92;
        //     // 
        //     //     double[,] averageImage = new double[H, W];
        //     //     foreach (var photo in photoList)
        //     //     {
        //     //         using (var ms = new MemoryStream(photo.Image))
        //     //         {
        //     //             Bitmap trainPhotoImage = new Bitmap(Image.FromStream(ms));
        //     // 
        //     //             for (int x = 0; x < trainPhotoImage.Width; x++)
        //     //             {
        //     //                 for (int y = 0; y < trainPhotoImage.Height; y++)
        //     //                 {
        //     //                     Color pixelColor = trainPhotoImage.GetPixel(x, y);
        //     //                     averageImage[y, x] = averageImage[y, x] + (double) pixelColor.R / photoList.Count();
        //     //                 }
        //     //             }
        //     //         }
        //     //     }
        //     // 
        //     //     return averageImage;
        //     // }
    }
}
