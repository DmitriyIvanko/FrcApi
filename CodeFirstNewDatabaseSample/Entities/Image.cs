using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirstNewDatabaseSample.Entities
{
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ImageId { get; set; }
        public byte[] ImageByteArray { get; set; }
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public string Format { get; set; }
        public string ImageName { get; set; }
        public virtual User User { get; set; }
    }
}
