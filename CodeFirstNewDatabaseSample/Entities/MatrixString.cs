using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class MatrixString
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MatrixStringId { get; set; }
        public int DimentionOne { get; set; }
        public int? DimentionTwo { get; set; }
        public string Value { get; set; }
    }
}
