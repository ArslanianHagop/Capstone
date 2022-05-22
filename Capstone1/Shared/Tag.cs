using System.ComponentModel.DataAnnotations;

namespace Capstone1.Shared
{
    public class Tag
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public Post? Post { get; set; }
        public int PostId { get; set; }
    }
}
