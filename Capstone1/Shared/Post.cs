using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone1.Shared
{
    public class Post
    {
        public int Id { get; set; }
        public Student? Student { get; set; }
        public int StudentId { get; set; }
        public string Title { get; set; } = string.Empty;
        [Column(TypeName="decimal(18,2)")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public decimal Budget { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public List<PostInterest> PostInterests { get; set; } = new List<PostInterest>();
        //Tags exists but as a Post can have many Tags, 1 to many relationship, Tag model has PostId

        public bool Visible { get; set; } = true;
        public bool Deleted { get; set; } = false;
        public bool IsAccepted { get; set; } = false;
        [NotMapped]
        public bool Editing { get; set; } = false;
        [NotMapped]
        public bool IsNew { get; set; } = false;
    }
}
