using System;
using System.Linq;
using Data.Entities;

namespace Data.Repositories
{
    public class ImageRepository
    {
        public Image Get(Guid imageId)
        {
            var db = new FrcContext();
            var imageEntity = db.Images.Where(x => x.ImageId == imageId).FirstOrDefault();
            db.Dispose();

            return imageEntity;
        }
    }
}
