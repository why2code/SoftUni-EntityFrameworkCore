using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.AccessControl;

namespace P01_StudentSystem.Data.Models
{
    public class Homework
    {
        [Key]
        public int HomeworkId { get; set; }

        [Column(TypeName = "varchar(255)")]
        public string Content { get; set; } = null!;

        public ContentTypeEnum ContentType { get; set; }

        public DateTime SubmissionTime { get; set; }


        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }

        public Student Student { get; set; } = null!;



        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;


        public enum ContentTypeEnum
        {
            Application,
            Pdf,
            Zip
        } 

    }
}
