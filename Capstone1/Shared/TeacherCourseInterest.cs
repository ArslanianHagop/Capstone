using System.Text.Json.Serialization;

namespace Capstone1.Shared
{
    public class TeacherCourseInterest
    {
        [JsonIgnore]
        public TeacherCourse? TeacherCourse { get; set; }
        public int TeacherCourseId { get; set; }
        public Interest? Interest { get; set; }
        public int InterestId { get; set; }
    }
}
