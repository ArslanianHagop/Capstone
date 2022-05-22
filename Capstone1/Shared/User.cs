using System.ComponentModel.DataAnnotations.Schema;

namespace Capstone1.Shared
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ProfilePic { get; set; } = string.Empty;
        public List<UserLanguage> UserLanguages { get; set; } = new List<UserLanguage>();
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string Role { get; set; } = string.Empty;
        public bool Visible { get; set; } = true;
        public bool Deleted { get; set; } = false;
        [NotMapped]
        public bool Typing { get; set; } = false;
    }
}
