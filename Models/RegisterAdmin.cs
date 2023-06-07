using System.ComponentModel.DataAnnotations;

namespace BKBCollegeManagement.Models
{
    public class RegisterAdmin
    {

            [Required]
        
            public string FullName { get; set; }

            [Required]
            public string Username { get; set; }

            [Required]
            public string Password { get; set; }

           



        
    }


}
