namespace ChatApp.Domain.Models.Messages;
public class MessageOutputModel
{
    public bool Delivered { get; set; }

    public bool WasUpdated { get; set; }

    public string UserId { get; set; }
}
