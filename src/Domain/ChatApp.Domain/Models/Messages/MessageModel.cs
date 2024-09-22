using System.ComponentModel.DataAnnotations;

namespace ChatApp.Domain.Models.Messages;

public class MessageModel
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [MaxLength(1000)]
    [Required]
    public string Content { get; set; } = string.Empty;

    public bool WasEdited { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime EditedAt { get; set; } = DateTime.Now;

    public string UserId { get; set; }
}