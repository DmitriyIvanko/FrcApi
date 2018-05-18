using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    class FaceRecognitionSystem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FaceRecognitionSystemId { get; set; }
        public string MnemonicDescription { get; set; }
        public string Type { get; set; }
        public Guid TypeSystemId { get; set; }
        public int InputImageHeight { get; set; }
        public int InputImageWidth { get; set; }
        public DateTime CreatedDT { get; set; }
    }
}
