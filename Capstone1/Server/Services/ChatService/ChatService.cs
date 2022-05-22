namespace Capstone1.Server.Services.ChatService
{
	public class ChatService : IChatService
	{
		private readonly DataContext _context;
		private readonly IAuthService _authService;

		public ChatService(DataContext context, IAuthService authService)
		{
			_context = context;
			_authService = authService;
		}

		#region "Get Methods"
		public async Task<ServiceResponse<List<Chat>>> ChangeIsNewOfChat(int senderId)
		{
			var currentUserId = await _authService.GetUserId();
			var chats = await _context.Chats
				.Where(c => c.SenderId == senderId
				&& c.RecipientId == currentUserId && !c.Deleted && c.Visible
				&& c.IsNew).ToListAsync();

			foreach (var chat in chats)
			{
				var dbChat = await _context.Chats.FindAsync(chat.Id);
				if (dbChat != null)
					dbChat.IsNew = false;
			}

			await _context.SaveChangesAsync();

			//if (chats == null || chats.Count == 0)
			//{
			//	return new ServiceResponse<List<Chat>>
			//	{
			//		Success = false,
			//		Message = "Couldn't change as there were no new messages between these users."
			//	};
			//}

			return new ServiceResponse<List<Chat>> { Data = (await GetMessagesBetweenLoggedUserAndAnother(senderId)).Data };
		}

		public async Task<ServiceResponse<Chat>> GetFirstNewReceivedMessage(int senderId)
		{
			var currentUserId = await _authService.GetUserId();
			List<Chat> chats = await _context.Chats
				.Where(c => c.SenderId == senderId && c.RecipientId == currentUserId
				&& !c.Deleted && c.Visible && c.IsNew).ToListAsync();

			if (chats == null || chats.Count == 0)
			{
				return new ServiceResponse<Chat>
				{
					Success = false,
					Message = "No new messages were found between these two users."
				};
			}

			return new ServiceResponse<Chat> { Data = chats[0] };
		}

		public async Task<ServiceResponse<Dictionary<int, string>>> GetLoggedInUsersChatsLastMessages()
		{
			var currentUserId = await _authService.GetUserId();
			var chats = await _context.Chats
				.Where(c => (c.SenderId == currentUserId
				|| c.RecipientId == currentUserId) && !c.Deleted && c.Visible).ToListAsync();

			chats = chats.OrderByDescending(c => c.Id)
				.DistinctBy(c => new { c.SenderId, c.RecipientId }).ToList();

			Dictionary<int, string> result = new Dictionary<int, string>();

			foreach (var chat in chats)
			{
				if (chat.SenderId == currentUserId)
				{
					var maxId = chats.FirstOrDefault(c => (c.SenderId == chat.RecipientId || c.RecipientId == chat.RecipientId) && c.Id == chats.Max(i => i.Id));
					string lastMessage = maxId.Message;
					maxId.Id = -1;
					if (lastMessage != null && !result.ContainsKey(chat.RecipientId))
						result.Add(chat.RecipientId, lastMessage);
				}
				else if (chat.RecipientId == currentUserId)
				{
					var maxId = chats.FirstOrDefault(c => (c.SenderId == chat.SenderId || c.RecipientId == chat.SenderId) && c.Id == chats.Max(i => i.Id));
					string lastMessage = maxId.Message;
					maxId.Id = -1;
					if (lastMessage != null && !result.ContainsKey(chat.SenderId))
						result.Add(chat.SenderId, lastMessage);
				}
			}

			if (result == null && result.Count == 0)
			{
				return new ServiceResponse<Dictionary<int, string>>
				{
					Success = false,
					Message = "This logged in user has no chats."
				};
			}

			return new ServiceResponse<Dictionary<int, string>> { Data = result };
		}

		private async Task<ServiceResponse<List<User>>> GetLoggedInUsersChatsUsers()
		{
			var currentUserId = await _authService.GetUserId();
			var chats = await _context.Chats
				.Where(c => (c.SenderId == currentUserId
				|| c.RecipientId == currentUserId) && !c.Deleted && c.Visible).ToListAsync();

			chats = chats.DistinctBy(c => new { c.SenderId, c.RecipientId }).ToList();

			List<User> result = new List<User>();

			foreach (var chat in chats)
			{
				if (chat.SenderId == currentUserId)
				{
					User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == chat.RecipientId && !u.Deleted && u.Visible);
					if (user != null && !result.Contains(user))
						result.Add(user);
				}
				else if (chat.RecipientId == currentUserId)
				{
					User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == chat.SenderId);
					if (user != null && !result.Contains(user))
						result.Add(user);
				}
			}

			if (result == null || result.Count == 0)
			{
				return new ServiceResponse<List<User>>
				{
					Success = false,
					Message = "This user has not messaged anyone."
				};
			}

			return new ServiceResponse<List<User>> { Data = result };
		}

		public async Task<ServiceResponse<List<User>>> GetLoggedInUsersChatsUsersPaginated(int page)
		{
			int pageResults = 5;
			var users = (await GetLoggedInUsersChatsUsers()).Data;
			if (users != null)
				users = users.Skip((page - 1) * pageResults).Take(pageResults).ToList();

			if (users == null || users.Count == 0)
			{
				return new ServiceResponse<List<User>>
				{
					Success = false,
					Message = "This user hasn't messaged anyone yet."
				};
			}

			return new ServiceResponse<List<User>> { Data = users };
		}

		public async Task<ServiceResponse<List<string>>> GetLoggedInUsersChatsUsersSearchSuggestions(string searchText)
		{
			List<string> result = new List<string>();
			var users = (await GetLoggedInUsersChatsUsers()).Data;
			if (users != null)
			{
				users = users
				.Where(u => u.FirstName.ToLower().Contains(searchText.ToLower())
				|| u.LastName.ToLower().Contains(searchText.ToLower())
				|| (u.FirstName + " " + u.LastName).ToLower().Contains(searchText.ToLower())).ToList();

				foreach (var user in users)
				{
					result.Add(user.FirstName + " " + user.LastName);
				}
			}

			return new ServiceResponse<List<string>> { Data = result };

		}

		public async Task<ServiceResponse<List<Chat>>> GetLoggedInUsersReceivedMessages()
		{
			var currentUserId = await _authService.GetUserId();
			var chats = await _context.Chats.Where(c => c.RecipientId == currentUserId && !c.Deleted && c.Visible).ToListAsync();

			if (chats == null || chats.Count == 0)
			{
				return new ServiceResponse<List<Chat>>
				{
					Success = false,
					Message = "This user hasn't received any messages."
				};
			}

			return new ServiceResponse<List<Chat>> { Data = chats };
		}

		public async Task<ServiceResponse<List<User>>> GetLoggedInUsersSearchedChatsUsers(string searchText, int page)
		{
			int pageResults = 5;
			var users = (await GetLoggedInUsersChatsUsers()).Data;
			if (users != null)
			{
				users = users
				.Where(u => u.FirstName.ToLower().Contains(searchText.ToLower())
				|| u.LastName.ToLower().Contains(searchText.ToLower())
				|| (u.FirstName + " " + u.LastName).ToLower().Contains(searchText.ToLower())).ToList();
			}

			if (users == null || users.Count == 0)
			{
				return new ServiceResponse<List<User>>
				{
					Success = false,
					Message = "There are no users that match your search."
				};
			}

			users = users.Skip((page - 1) * pageResults).Take(pageResults).ToList();

			return new ServiceResponse<List<User>> { Data = users };
		}

		public async Task<ServiceResponse<int>> GetLoggedInUsersSearchedChatsUsersCount(string searchText)
		{
			int result = 0;
			var users = (await GetLoggedInUsersChatsUsers()).Data;
			if (users != null)
			{
				result = users
				.Where(u => u.FirstName.ToLower().Contains(searchText.ToLower())
				|| u.LastName.ToLower().Contains(searchText.ToLower())
				|| (u.FirstName + " " + u.LastName).ToLower().Contains(searchText.ToLower())).Count();
			}

			return new ServiceResponse<int> { Data = result };
		}

		public async Task<ServiceResponse<List<Chat>>> GetMessagesBetweenLoggedUserAndAnother(int userId)
		{
			var currentUserId = await _authService.GetUserId();
			var chats = await _context.Chats
				.Where(c => ((c.SenderId == currentUserId && c.RecipientId == userId)
				|| (c.RecipientId == currentUserId && c.SenderId == userId)) && !c.Deleted && c.Visible).ToListAsync();

			return new ServiceResponse<List<Chat>> { Data = chats };
		}

		public async Task<ServiceResponse<int>> GetNewReceivedMessagesCountBetweenLoggedUserAndAnother(int senderId)
		{
			var currentUserId = await _authService.GetUserId();
			int result = await _context.Chats
				.Where(c => !c.Deleted && c.Visible
				&& c.RecipientId == currentUserId && c.SenderId == senderId
				&& c.IsNew).CountAsync();

			return new ServiceResponse<int> { Data = result };
		}
		#endregion

		#region "Post Methods"
		public async Task<ServiceResponse<List<Chat>>> SendMessage(Chat chat)
		{
			chat.SenderId = await _authService.GetUserId();
			_context.Chats.Add(chat);
			await _context.SaveChangesAsync();

			return new ServiceResponse<List<Chat>> { Data = (await GetMessagesBetweenLoggedUserAndAnother(chat.RecipientId)).Data };
		}
        #endregion

	}
}
