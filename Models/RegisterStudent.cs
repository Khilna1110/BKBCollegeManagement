using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKBCollegeManagement.Models
{
    public class RegisterStudent
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        [StringLength(10)]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string FullName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public int CourseId { get; set; }

        public DateTime LastModified { get; set; }



    }
}
