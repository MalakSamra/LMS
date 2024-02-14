using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Models
{
    public class Module
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [ForeignKey("Course")]
        public int Crs_Id { get; set; }
        //Navigation Property
        public Course Course { get; set; }
        public virtual ICollection<Progress> Progresses { get; set; } = new HashSet<Progress>();
    }
}
