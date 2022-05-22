namespace Capstone1.Client.Services.ChatService
{
	public interface IChatService
	{
		event Action ChatChanged;
        #region "Lists"
        List<Chat> Chats { get; set; }
		List<User> LoggedInUsersChatsUsers { get; set; }
        #endregion
        #region "Get Methods"
        Task GetMessagesBetweenLoggedUserAndAnother(int userId);
		Task<int> GetNewReceivedMessagesCountBetweenLoggedUserAndAnother(int senderId);
		Task<ServiceResponse<List<Chat>>> GetLoggedInUsersReceivedMessages();
		Task<Dictionary<int, string>> GetLoggedInUsersChatsLastMessages();
		Task GetLoggedInUsersChatsUsersPaginated(int page);
		Task GetLoggedInUsersSearchedChatsUsers(string searchText, int page);
		Task<List<string>> GetLoggedInUsersChatsUsersSearchSuggestions(string searchText);
        Task<int> GetLoggedInUsersSearchedChatsUsersCount(string searchText);
        Task<Chat> GetFirstNewReceivedMessage(int senderId);
		Task ChangeIsNewOfChat(int senderId);
		#endregion
		#region "Post Methods"
		Task SendMessage(Chat chat);
        #endregion
	}
}
