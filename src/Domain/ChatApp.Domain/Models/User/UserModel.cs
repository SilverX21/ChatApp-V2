using Microsoft.AspNetCore.Identity;

namespace ChatApp.Domain.Models.User;
public class UserModel : IdentityUser
{
    public string? Initials { get; set; }
}
