using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone1.Shared
{
    public class Proposal
    {
        public int Id { get; set; }
        public Teacher? Teacher { get; set; }
        public int TeacherId { get; set; }
        public Post? Post { get; set; }
        public int PostId { get; set; }
        public string Description { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public decimal Budget { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public bool Visible { get; set; } = true;
        public bool Deleted { get; set; } = false;
        public bool IsAccepted { get; set; } = false;
        [NotMapped]
        public bool Editing { get; set; } = false;
        [NotMapped]
        public bool IsNew { get; set; } = false;
    }
}
