namespace LMS.Models
{
    public class EnrolledCoursesViewModel
    {
        public int Crs_Id { get; set; }
        public string CourseTitle { get; set; }
        public string CourseDescription { get; set; }
        public int Module_Id { get; set; }
        public string ModuleTitle { get; set; }
        public string ModuleDescription { get; set; }
        public virtual ICollection<Progress> Progresses { get; set; } = new HashSet<Progress>();
    }
}
