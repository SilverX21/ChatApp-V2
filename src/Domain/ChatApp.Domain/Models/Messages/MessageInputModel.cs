using System.ComponentModel.DataAnnotations;

namespace ChatApp.Domain.Models.Messages;

public class MessageInputModel
{
    [MaxLength(1000)]
    [Required]
    public string Content { get; set; }

    public string UserId { get; set; }
}