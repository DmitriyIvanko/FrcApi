using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace test_webapi.Dto
{
    public class FrsParameterDto
    {
        public Guid FrsId { get; set; }
        public MatrixStringDto AverageImageMatrix { get; set; }
        public MatrixStringDto LeftMatrix { get; set; }
        public MatrixStringDto RightMatrix { get; set; }
    }
}