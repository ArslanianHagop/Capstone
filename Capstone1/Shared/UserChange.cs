namespace Capstone1.Shared
{
    public class UserChange
	{
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string ProfilePic { get; set; } = string.Empty;
		public bool Visible { get; set; } = true;
		public List<UserLanguage> UserLanguages { get; set; } = new List<UserLanguage>();
	}
}
