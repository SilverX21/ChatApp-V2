using System.ComponentModel.DataAnnotations;

namespace ChatApp.Domain.Models.Messages;

public class MessageModel
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [MaxLength(1000)]
    [Required]
    public string Content { get; set; }

    public bool WasEdited { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime EditedAt { get; set; }

    public string UserId { get; set; }
}