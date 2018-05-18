using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class Etalon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid EtalonId { get; set; }
        public Guid UserId { get; set; }
        public Guid ImageId { get; set; }
        public Guid FaceRecognitionSystemId { get; set; }
        public Guid FeatureMatrixId { get; set; }
        public DateTime RegistredDT { get; set; }
    }
}
