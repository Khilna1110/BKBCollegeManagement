using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BKBCollegeManagement.Models
{
    public class User
    {
            [Key]
            public int UserId { get; set; }

            [Required]
            [StringLength(100)]
            public string FullName { get; set; }
            [Required]
            [StringLength(50)]
            public string Username { get; set; }
            [Required]
            [StringLength(10)]
        
            [JsonIgnore]
            public string PasswordHash { get; set; }

            [Required]
            [StringLength(10)]
            public string Role { get; set; }


    }
}
