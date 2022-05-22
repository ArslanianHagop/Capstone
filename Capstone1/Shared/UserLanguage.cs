using System.Text.Json.Serialization;

namespace Capstone1.Shared
{
    public class UserLanguage
    {
        [JsonIgnore]
        public User? User { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public Language? Language { get; set; }
        public int LanguageId { get; set; }
    }
}
