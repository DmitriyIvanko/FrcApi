using System;

namespace test_webapi.Dto
{
    public class RecognizeFeatureDto
    {
        public Guid FrsId { get; set; }
        public string FeatureMatrixString { get; set; }
        public int DimentionOne { get; set; }
        public int DimentionTwo { get; set; }
    }
}