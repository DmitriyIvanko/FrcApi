using System;
using System.Collections.Generic;
using System.Linq;
using Data.Entities;
using System.IO;

namespace Data.Logic.AddImageDatabase
{
    public class AddImageDatabase
    {
        private string inputDatabaseDir;
        private string imageDatabaseName;
        private ImageDatabase imageDatabase;
        private List<User> userList = new List<User>();
        private List<Image> imageList = new List<Image>();

        public AddImageDatabase(string _imageDatabaseName, string _inputDatabaseDir)
        {
            imageDatabaseName = _imageDatabaseName;
            inputDatabaseDir = _inputDatabaseDir;
        }

        public void startAddingDatabase()
        {
            var db = new FrcContext();

            proccessDatabase(db);

            Console.ReadKey();
            db.Dispose();
        }

        private void proccessDatabase(FrcContext db)
        {
            var orl = db.ImageDatabases.Where(x => x.DatabaseName == imageDatabaseName).FirstOrDefault();

            if (orl != null)
            {
                Console.WriteLine("The database {0} already added;", imageDatabaseName);
                return;
            }

            analysisDatabase(inputDatabaseDir, imageDatabaseName);

            db.ImageDatabases.Add(imageDatabase);
            db.Users.AddRange(userList.AsEnumerable());
            db.Images.AddRange(imageList.AsEnumerable());
            db.SaveChanges();

            Console.WriteLine("The database {0} successfully added;", imageDatabaseName);
        }

        private void analysisDatabase(string inputDatabaseDir, string imageDatabaseName)
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

            if (isSameValueInArray(imagesUserList))
            {
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

        private bool isSameValueInArray(List<int> array)
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
    }
}
