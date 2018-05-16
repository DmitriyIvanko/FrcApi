using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class ImageDatabase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ImageDatabaseId { get; set; }
        public string DatabaseName { get; set; }
        public int ImageHeight { get; set; }
        public int ImageWidth { get; set; }
        public int TotalUser { get; set; }
        public int TotalImageForUser { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public bool isSameTotalImageForUser { get; set; }
        public bool isSameImageSize { get; set; }
    }
}
