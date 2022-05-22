using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone1.Shared
{
    public class Teacher
    {
        public User? User { get; set; }
        [Required]
        [Key]
        [ForeignKey("User")]
        public int TeacherId { get; set; }
        public List<TeacherInterest> TeacherInterests { get; set; } = new List<TeacherInterest>();
        //Teacher has proposals and jobs but since they are 1-to-many relationship we don't need them here.
    }
}
