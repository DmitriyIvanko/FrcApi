using System;

namespace test_webapi.Dto
{
    public class RecognizeItemDto
    {
        public Guid FrsId { get; set; }
        public string ImageByteArray { get; set; }
        public string AdditionalLoad { get; set; }
    }
}