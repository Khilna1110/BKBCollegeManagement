using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKBCollegeManagement.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int StudentId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        [ForeignKey("CourseId")]
        public int CourseId { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }

        public DateTime LastModified { get; set; }
    }
}
