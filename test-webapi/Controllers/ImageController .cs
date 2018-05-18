using System;
using System.Collections.Generic;
using System.Web.Http;

using test_webapi.Dto;
using Data.Entities;
using Data.Repositories;

namespace test_webapi.Controllers
{
    public class ImageController : ApiController
    {
        [HttpGet]
        [ActionName("get")]
        public ImageDto Get([FromUri] Guid imageId)
        {
            var repository = new ImageRepository();
            var image =  repository.Get(imageId);
            return new ImageDto {
                Format = image.Format,
                ImageByteArray = Convert.ToBase64String(image.ImageByteArray),
                ImageId = image.ImageId,
                ImageName = image.ImageName,
                UserId = image.UserId
            };
        }
    }
}
