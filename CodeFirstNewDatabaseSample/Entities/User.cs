using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirstNewDatabaseSample.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        [ForeignKey("ImageDatabase")]
        public Guid? ImageDatabaseId { get; set; }
        public virtual ImageDatabase ImageDatabase { get; set; }
    }
}
