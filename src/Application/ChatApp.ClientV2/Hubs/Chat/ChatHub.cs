using Microsoft.AspNetCore.SignalR;

namespace ChatApp.ClientV2.Hubs.Chat;

//A hub is a class that serves as a high-level pipeline that handles client-server communication
//The Hub class manages connections, groups, and messaging
public class ChatHub : Hub
{
    /// <summary>
    /// method can be called by a connected client to send a message to all clients
    /// </summary>
    /// <param name="user"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}