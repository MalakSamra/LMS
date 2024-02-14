using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string User_Name { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
        public virtual ICollection<Progress> Progresses { get; set; } = new HashSet<Progress>();
    }
}
