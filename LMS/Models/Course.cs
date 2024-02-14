using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }

        //Navigation Property
        public virtual ICollection<Module> Modules { get; set; } = new HashSet<Module>();
    }
}
