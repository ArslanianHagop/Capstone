namespace Capstone1.Client.Services.ChatService
{
	public class ChatService : IChatService
	{
		private readonly HttpClient _http;

		public ChatService(HttpClient http)
		{
			_http = http;
		}

        #region "Lists"
        public List<Chat> Chats { get; set; } = new List<Chat>();
		public List<User> LoggedInUsersChatsUsers { get; set; } = new List<User>();
        #endregion

        public event Action ChatChanged;

        #region "Get Methods"
        public async Task ChangeIsNewOfChat(int senderId)
        {
			var result = await _http.GetFromJsonAsync<ServiceResponse<List<Chat>>>($"api/chat/changeIsNewOfChat/{senderId}");
        }

		public async Task<Chat> GetFirstNewReceivedMessage(int senderId)
		{
			var result = await _http.GetFromJsonAsync<ServiceResponse<Chat>>($"api/chat/firstNewMessage/{senderId}");
			return result.Data;
		}

		public async Task<Dictionary<int, string>> GetLoggedInUsersChatsLastMessages()
        {
			var result = await _http.GetFromJsonAsync<ServiceResponse<Dictionary<int, string>>>("api/chat/loggedInUsersChatsLastMessages");
			return result.Data;
        }

		public async Task GetLoggedInUsersChatsUsersPaginated(int page)
		{
			var result = await _http.GetFromJsonAsync<ServiceResponse<List<User>>>($"api/chat/loggedInUsersChatsUsersPaginated/{page}");
			if (result != null && result.Data != null)
				LoggedInUsersChatsUsers = result.Data;
		}

        public async Task<List<string>> GetLoggedInUsersChatsUsersSearchSuggestions(string searchText)
        {
			var result = await _http.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/chat/loggedInUsersChatsUsersSearchSuggestions/{searchText}");
			return result.Data;
        }

		public async Task<ServiceResponse<List<Chat>>> GetLoggedInUsersReceivedMessages()
		{
			var result = await _http.GetFromJsonAsync<ServiceResponse<List<Chat>>>("api/chat/loggedInUsersReceivedMessages");
			return result;
		}

        public async Task GetLoggedInUsersSearchedChatsUsers(string searchText, int page)
        {
			var result = await _http.GetFromJsonAsync<ServiceResponse<List<User>>>($"api/chat/loggedInUsersSearchedChatsUsers/{searchText}/{page}");
			if (result != null && result.Data != null)
				LoggedInUsersChatsUsers = result.Data;

			ChatChanged.Invoke();
        }

        public async Task<int> GetLoggedInUsersSearchedChatsUsersCount(string searchText)
        {
			var result = await _http.GetFromJsonAsync<ServiceResponse<int>>($"api/chat/loggedInUsersSearchedChatsUsersCount/{searchText}");
			return result.Data;
        }

        public async Task GetMessagesBetweenLoggedUserAndAnother(int userId)
		{
			var result = await _http.GetFromJsonAsync<ServiceResponse<List<Chat>>>($"api/chat/loggedAndAnother/{userId}");
			if(result != null && result.Data != null)
				Chats = result.Data;
		}

		public async Task<int> GetNewReceivedMessagesCountBetweenLoggedUserAndAnother(int senderId)
		{
			var result = await _http.GetFromJsonAsync<ServiceResponse<int>>($"api/chat/newReceivedMessagesCount/{senderId}");
			return result.Data;
		}
		#endregion

		#region "Post Methods"
		public async Task SendMessage(Chat chat)
        {
			var result = await _http.PostAsJsonAsync("api/chat/send", chat);
        }
        #endregion
    }
}
