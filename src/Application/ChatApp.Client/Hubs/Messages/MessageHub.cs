using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Client.Hubs.Messages;

public class MessageHub : Hub
{
    public Task SendMessageToAll(string message)
    {
        return Clients.All.SendAsync("ReceiveMessage", message);
    }
}