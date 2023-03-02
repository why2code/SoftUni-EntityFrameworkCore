using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_StudentSystem.Data.Models
{
    public class Resource
    {
        [Key]
        public int ResourceId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(2048)]
        [Column(TypeName = "varchar(2048)")]
        public string Url { get; set; } = null!;

        public ResourceTypeEnum ResourceType { get; set; }


        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public enum ResourceTypeEnum
        {
            Video,
            Presentation, 
            Document,
            Other
        }

    }
}
