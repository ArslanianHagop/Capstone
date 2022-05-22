using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Capstone1.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ChatController : ControllerBase
	{
		private readonly IChatService _chatService;

		public ChatController(IChatService chatService)
		{
			_chatService = chatService;
		}

        #region "Get Methods"
        [HttpGet("loggedAndAnother/{userId}")]
		public async Task<ActionResult<ServiceResponse<List<Chat>>>> GetMessagesBetweenLoggedUserAndAnother(int userId)
		{
			var response = await _chatService.GetMessagesBetweenLoggedUserAndAnother(userId);
			return Ok(response);
		}

		[HttpGet("loggedInUsersReceivedMessages")]
		public async Task<ActionResult<ServiceResponse<List<Chat>>>> GetLoggedInUsersReceivedMessages()
		{
			var response = await _chatService.GetLoggedInUsersReceivedMessages();
			return Ok(response);
		}

		[HttpGet("loggedInUsersChatsLastMessages")]
		public async Task<ActionResult<ServiceResponse<Dictionary<int, string>>>> GetLoggedInUsersChatsLastMessages()
        {
			var response = await _chatService.GetLoggedInUsersChatsLastMessages();
			return Ok(response);
        }

		[HttpGet("loggedInUsersChatsUsersPaginated/{page}")]
		public async Task<ActionResult<ServiceResponse<List<User>>>> GetLoggedInUsersChatsUsersPaginated(int page)
		{
			var response = await _chatService.GetLoggedInUsersChatsUsersPaginated(page);
			return Ok(response);
		}

		[HttpGet("loggedInUsersSearchedChatsUsers/{searchText}/{page}")]
		public async Task<ActionResult<ServiceResponse<List<User>>>> GetLoggedInUsersSearchedChatsUsers(string searchText, int page)
		{
			var response = await _chatService.GetLoggedInUsersSearchedChatsUsers(searchText, page);
			return Ok(response);
		}

		[HttpGet("loggedInUsersChatsUsersSearchSuggestions/{searchText}")]
		public async Task<ActionResult<ServiceResponse<List<string>>>> GetLoggedInUsersChatsUsersSearchSuggestions(string searchText)
        {
			var response = await _chatService.GetLoggedInUsersChatsUsersSearchSuggestions(searchText);
			return Ok(response);
        }

		[HttpGet("changeIsNewOfChat/{senderId}")]
		public async Task<ActionResult<ServiceResponse<List<Chat>>>> ChangeIsNewOfChat(int senderId)
        {
			var response = await _chatService.ChangeIsNewOfChat(senderId);
			return Ok(response);
		}

		[HttpGet("loggedInUsersSearchedChatsUsersCount/{searchText}")]
		public async Task<ActionResult<ServiceResponse<int>>> GetLoggedInUsersSearchedChatsUsersCount(string searchText)
        {
			var response = await _chatService.GetLoggedInUsersSearchedChatsUsersCount(searchText);
			return Ok(response);
		}

		[HttpGet("firstNewMessage/{senderId}")]
		public async Task<ActionResult<ServiceResponse<Chat>>> GetFirstNewReceivedMessage(int senderId)
		{
			var response = await _chatService.GetFirstNewReceivedMessage(senderId);
			return Ok(response);
		}

		[HttpGet("newReceivedMessagesCount/{senderId}")]
		public async Task<ActionResult<ServiceResponse<int>>> GetNewReceivedMessagesCountBetweenLoggedUserAndAnotherAsync(int senderId)
		{
			var response = await _chatService.GetNewReceivedMessagesCountBetweenLoggedUserAndAnother(senderId);
			return Ok(response);

		}
		#endregion

		[HttpPost("send")]
		public async Task<ActionResult<ServiceResponse<List<Chat>>>> SendMessage(Chat chat)
		{
			var response = await _chatService.SendMessage(chat);
			return Ok(response);
		}
	}
}
