using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class DatabaseTestUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid DatabaseTestUserId { get; set; }
        public Guid FaceRecognitionSystemId { get; set; }
        public Guid ImageId { get; set; }
    }
}
