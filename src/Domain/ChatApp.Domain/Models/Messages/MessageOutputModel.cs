namespace ChatApp.Domain.Models.Messages;

public class MessageOutputModel
{
    public string Content { get; set; }

    public bool Delivered { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool WasUpdated { get; set; }

    public string UserId { get; set; }
}