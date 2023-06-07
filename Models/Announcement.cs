using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKBCollegeManagement.Models
{
    public class Announcement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int AnnouncementId { get; set; }
        [Required]
        public string AnnouncementText { get; set; }

        [Required]
        public string PostedBy { get; set; }

        public DateTime PostedAt { get; set; }
    }
}
