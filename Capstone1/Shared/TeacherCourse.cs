using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone1.Shared
{
    public class TeacherCourse
    {
        public int Id { get; set; }
        public Teacher? Teacher { get; set; }
        public int TeacherId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public List<TeacherCourseInterest> TeacherCourseInterests { get; set; } = new List<TeacherCourseInterest>();
        public string CourseLink { get; set; } = string.Empty;
        public bool Visible { get; set; } = true;
        public bool Deleted { get; set; } = false;
        [NotMapped]
        public bool Editing { get; set; } = false;
        [NotMapped]
        public bool IsNew { get; set; } = false;
    }
}
