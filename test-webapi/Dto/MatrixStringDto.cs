using System;

namespace test_webapi.Dto
{
    public class MatrixStringDto
    {
        public Guid MatrixStringId { get; set; }
        public int DimentionOne { get; set; }
        public int DimentionTwo { get; set; }
        public string Value { get; set; }
    }
}