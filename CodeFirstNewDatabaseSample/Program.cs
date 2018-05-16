using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using Data.Entities;
using Data.Logic.AddImageDatabase;

namespace Data
{
    class ImageDatabaseTool
    {
        static void Main(string[] args)
        {
            string imageDatabaseName = "ORL";
            string inputDatabaseDir = @"C:\Projects\frc\AT&T_ORL_database_JPEG\";

            var dbWriter = new AddImageDatabase(imageDatabaseName, inputDatabaseDir);
            dbWriter.startAddingDatabase();
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
