using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Domain.Models.Messages;

public class MessageModel
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(1000)]
    [Required]
    public string Content { get; set; }

    public bool WasEdited { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime EditedAt { get; set; }

    public string UserId { get; set; }

    [NotMapped]
    [ForeignKey("UserId")]
    public string UserModel { get; set; }
}