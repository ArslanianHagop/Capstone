using System.Text.Json.Serialization;

namespace Capstone1.Shared
{
    public class PostInterest
    {
        [JsonIgnore]
        public Post? Post { get; set; }
        public int PostId { get; set; }
        public Interest? Interest { get; set; }
        public int InterestId { get; set; }
    }
}
