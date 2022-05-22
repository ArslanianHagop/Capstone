namespace Capstone1.Server.Services.ChatService
{
	public interface IChatService
	{
        #region "Get Methods"
		Task<ServiceResponse<List<Chat>>> GetMessagesBetweenLoggedUserAndAnother(int userId);
		Task<ServiceResponse<int>> GetNewReceivedMessagesCountBetweenLoggedUserAndAnother(int senderId);
		Task<ServiceResponse<List<Chat>>> GetLoggedInUsersReceivedMessages();
		Task<ServiceResponse<List<User>>> GetLoggedInUsersChatsUsersPaginated(int page);
		Task<ServiceResponse<Dictionary<int, string>>> GetLoggedInUsersChatsLastMessages();
		Task<ServiceResponse<List<User>>> GetLoggedInUsersSearchedChatsUsers(string searchText, int page);
		Task<ServiceResponse<int>> GetLoggedInUsersSearchedChatsUsersCount(string searchText);
		Task<ServiceResponse<List<string>>> GetLoggedInUsersChatsUsersSearchSuggestions(string searchText);
		Task<ServiceResponse<Chat>> GetFirstNewReceivedMessage(int senderId);
		Task<ServiceResponse<List<Chat>>> ChangeIsNewOfChat(int senderId);
		#endregion
		#region "Post Methods"
		Task<ServiceResponse<List<Chat>>> SendMessage(Chat chat);
        #endregion
	}
}
