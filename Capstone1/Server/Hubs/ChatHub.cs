using Microsoft.AspNetCore.SignalR;

namespace Capstone1.Server.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinGroup(string group)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, group);
        }

        public async Task AddToGroup(string groupName, Chat chat)
        {
            await Clients.Group(groupName).SendAsync("Send", chat);

            await Clients.All.SendAsync("IsNew", chat);
        }

        public async Task ChatButtonColor(int recipientId)
		{
            await Clients.All.SendAsync("IsNewButton", recipientId);
        }

        public async Task MessagesSeen(int recipientId)
        {
            await Clients.All.SendAsync("IsNewMessages", recipientId);
        }

        public async Task ForTyping(int senderId, int recipientId, bool typingFlag)
        {
            await Clients.All.SendAsync("typing", senderId, recipientId, typingFlag);
        }
    }
}
