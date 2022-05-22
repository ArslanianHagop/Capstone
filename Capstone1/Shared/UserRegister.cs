using System.ComponentModel.DataAnnotations;

namespace Capstone1.Shared
{
    public class UserRegister
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required, StringLength(10, ErrorMessage = "First Name is too long.")]
        public string FirstName { get; set; } = string.Empty;
        [Required, StringLength(15, ErrorMessage = "Last Name is too long.")]
        public string LastName { get; set; } = string.Empty;
        public string ProfilePic { get; set; } = string.Empty;
        public List<UserLanguage> UserLanguages { get; set; } = new List<UserLanguage>();
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
