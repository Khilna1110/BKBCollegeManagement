using System.ComponentModel.DataAnnotations;

namespace BKBCollegeManagement.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
       
        [StringLength(100)]
        public string CourseName { get; set; }
    }
}
