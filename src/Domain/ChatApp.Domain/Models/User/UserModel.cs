using System.ComponentModel.DataAnnotations;
using ChatApp.Domain.Models.Messages;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Domain.Models.User;

public class UserModel : IdentityUser
{
    [MaxLength(5)]
    public string? Initials { get; set; }

    [MaxLength(150)]
    public string? Name { get; set; }

    public ICollection<MessageModel> Messages { get; set; }
}