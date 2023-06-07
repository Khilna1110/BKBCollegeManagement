using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BKBCollegeManagement.Models
{
    public class Message
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int MessageId { get; set; }
        [Required]
        [StringLength(255)]
        public string MessageContent { get; set; }

        public string SenderName { get; set; }

        [Required]
        [ForeignKey("UserId")]
        public int ReceiverId { get; set; }
        public DateTime CreatedDate { get; set; }


    }
}
