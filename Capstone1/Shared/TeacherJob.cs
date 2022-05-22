using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Capstone1.Shared
{
    public class TeacherJob
    {
        public int Id { get; set; }
        [JsonIgnore]
        public Teacher? Teacher { get; set; }
        public int TeacherId { get; set; }
        [Required]
        public string Position { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string FromMonth { get; set; } = string.Empty;
        public int FromYear { get; set; }
        public string ToMonth { get; set; } = string.Empty;
        public string ToYear { get; set; } = string.Empty;
        public bool Visible { get; set; } = true;
        public bool Deleted { get; set; } = false;
        [NotMapped]
        public bool Editing { get; set; } = false;
        [NotMapped]
        public bool IsNew { get; set; } = false;
    }
}
