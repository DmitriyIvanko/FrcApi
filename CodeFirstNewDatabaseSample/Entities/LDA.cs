using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class LDA
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid LDAId { get; set; }
        // to do: add foregn keys for matrices:
        public Guid AverageImageMatrixId { get; set; }
        public Guid LeftMatrixId { get; set; }
        public Guid RightMatrixId { get; set; }
    }
}
