namespace Capstone1.Shared
{
    public class UserDTO
    {
        public List<User> Users { get; set; } = new List<User>();
        public int Pages { get; set; }
        public int CurrentPage { get; set; }
    }
}
