using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone1.Shared
{
    public class Student
    {
        public User? User { get; set; }
        [Required]
        [Key]
        [ForeignKey("User")]
        public int StudentId { get; set; }
        public List<StudentInterest> StudentInterests { get; set; } = new List<StudentInterest>();
        //Posts exists but as a student can have many posts, 1 to many relationship, Post model has StudentId
    }
}
