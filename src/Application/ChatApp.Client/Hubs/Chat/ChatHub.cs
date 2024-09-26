using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Client.Hubs.Chat;

public class ChatHub : Hub
{
    public Task SendMessageToAll(string user, string message)
    {
        return Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}