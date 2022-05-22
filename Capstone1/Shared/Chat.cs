using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone1.Shared
{
    public class Chat
    {
        public int Id { get; set; }
        public User? Recipient { get; set; }
        public User? Sender { get; set; }
        [ForeignKey("Sender")]
        public int SenderId { get; set; }
        [ForeignKey("Recipient")]
        public int RecipientId { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime DateSent { get; set; } = DateTime.Now;
        public bool Visible { get; set; } = true;
        public bool Deleted { get; set; } = false;
        public bool IsNew { get; set; } = true;
    }
}
