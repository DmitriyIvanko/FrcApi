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

namespace CodeFirstNewDatabaseSample
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new BloggingContext())
            {
                // Добавление БД:
                string ImageDatabaseName = "ORL";
                string inputDatabaseDir = @"C:\Projects\frc\AT&T_ORL_database_JPEG\";
                string[] imageClassDirArray = Directory.GetDirectories(inputDatabaseDir);
               
                var imageDatabase = new ImageDatabase{
                    DatabaseName = ImageDatabaseName,
                };
                db.ImageDatabases.Add(imageDatabase);
                db.SaveChanges();
               
                foreach (string imageClassDir in imageClassDirArray)
                {
                    string imageClassName = imageClassDir.Substring(imageClassDir.LastIndexOf('\\') + 1);
               
                    string[] imageFileDirArray = Directory.GetFiles(inputDatabaseDir + imageClassName);
               
                    var user = new User
                    {
                        Username = imageClassName,
                        ImageDatabaseId = imageDatabase.ImageDatabaseId,
                    };
                    db.Users.Add(user);
                    db.SaveChanges();
               
                    foreach (string imageFile in imageFileDirArray)
                    {
                        Image img = Image.FromFile(imageFile);
                        byte[] arr;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            arr = ms.ToArray();
                        }
               
                        var photoImage = new Photo
                        {
                            Format = "JPEG",
                            Image = arr,
                            UserId = user.UserId,
                        };
                        db.Photos.Add(photoImage);
                        db.SaveChanges();
                    }
                }

                // Create and save a new Blog 
                Console.Write("Enter a name for a new Blog: ");
                var name = Console.ReadLine();

                var blog = new Blog { Name = name };
                db.Blogs.Add(blog);
                db.SaveChanges();

                // Display all Blogs from the database 
                var query = from b in db.Blogs
                            orderby b.Name
                            select b;

                Console.WriteLine("All blogs in the database:");
                foreach (var item in query)
                {
                    Console.WriteLine(item.Name);
                }


                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }

    public class WorkWithImageDatabase
    {
        public void start()
        {
            var DB = "ORL";
            var T = 30;
            var OT = 25;
            var TH = 50;
            int H = 112;
            int W = 92;
            int trainPhotoUserNum = 0;

            using (var db = new BloggingContext())
            {
                var selectedIdb =  db.ImageDatabases.Where(x => x.DatabaseName == DB).FirstOrDefault();

                var userList = db.Users.Where(x => x.ImageDatabaseId == selectedIdb.ImageDatabaseId).ToList();
                var trainClassesNum = (int) Math.Round((double) userList.Count / 100 * (100 - OT));
                var trainUserList = userList.Take(trainClassesNum);

                List<Photo> trainPhotoList = new List<Photo>();
                List<List<Photo>> trainClassPhotoList = new List<List<Photo>>();

                foreach (var trainUser in trainUserList)
                {
                    var photoUserList = db.Photos.Where(x => x.UserId == trainUser.UserId).ToList();
                    trainPhotoUserNum = (int)Math.Round((double)photoUserList.Count / 100 * (100 - T));
                    var trainPhotoUserList = photoUserList.Take(trainPhotoUserNum).ToList();
                    trainClassPhotoList.Add(trainPhotoUserList);
                    trainPhotoList = trainPhotoList.Concat(trainPhotoUserList).ToList();
                }

                foreach (var classList in trainClassPhotoList)
                {
                    var XclassAverage = getAverage(classList);
                    foreach (var classPhoto in classList)
                    {
                        // classPhoto;
                    }
                }
                // var Xaverage = getAverage(trainPhotoList);
            }
        }

        private double[,] getAverage(IEnumerable<Photo> photoList)
        {
            int H = 112;
            int W = 92;

            double[,] averageImage = new double[H, W];
            foreach (var photo in photoList)
            {
                using (var ms = new MemoryStream(photo.Image))
                {
                    Bitmap trainPhotoImage = new Bitmap(Image.FromStream(ms));

                    for (int x = 0; x < trainPhotoImage.Width; x++)
                    {
                        for (int y = 0; y < trainPhotoImage.Height; y++)
                        {
                            Color pixelColor = trainPhotoImage.GetPixel(x, y);
                            averageImage[y, x] = averageImage[y, x] + (double) pixelColor.R / photoList.Count();
                        }
                    }
                }
            }

            return averageImage;
        }
    }



    public class Blog
    {
        public int BlogId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public virtual List<Post> Posts { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
    }

    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        [ForeignKey("ImageDatabase")]
        public Guid? ImageDatabaseId { get; set; }
        public virtual ImageDatabase ImageDatabase { get; set; }
    }

    public class Photo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PhotoId { get; set; }
        public byte[] Image { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public string Format { get; set; }
        public virtual User User { get; set; }
    }

    public class ImageDatabase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ImageDatabaseId { get; set; }
        public string DatabaseName { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }

    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<ImageDatabase> ImageDatabases { get; set; }

        public BloggingContext()
            : base("name=BloggingCompactDatabase")
        {
        }
    }
}
