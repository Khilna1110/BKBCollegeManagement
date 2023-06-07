using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKBCollegeManagement.Models
{
    public class Subject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int SubjectId { get; set; }
        [Required]
        public string SubjectName { get; set; }

        [ForeignKey("CourseId")]
        public int CourseId { get; set; }
    }
}
