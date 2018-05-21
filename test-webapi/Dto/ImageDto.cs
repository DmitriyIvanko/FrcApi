using System;

namespace test_webapi.Dto
{
    public class ImageDto
    {
        public Guid ImageId { get; set; }
        public string ImageByteArray { get; set; }
        public Guid UserId { get; set; }
        public string Format { get; set; }
        public string ImageName { get; set; }
    }
}