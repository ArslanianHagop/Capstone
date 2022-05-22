namespace Capstone1.Shared
{
    public class PostDTO
    {
        public List<Post> Posts { get; set; } = new List<Post>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
