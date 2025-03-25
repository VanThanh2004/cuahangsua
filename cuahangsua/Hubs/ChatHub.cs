using Microsoft.AspNetCore.SignalR;

namespace cuahangsua.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message, bool isFromAdmin)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message, isFromAdmin);
        }
    }
}