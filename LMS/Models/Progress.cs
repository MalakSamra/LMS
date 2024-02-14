using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Models
{
    public class Progress
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int User_Id { get; set; }
        [ForeignKey("Module")]
        public int Module_Id { get; set; }
        [Required]
        public string Completion_Status { get; set; } = "Not Completed";
        //Navigation Properties
        public User User { get; set; }
        public Module Module { get; set; }
    }
}
