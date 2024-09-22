using System.ComponentModel.DataAnnotations;

namespace ChatApp.Domain.Models.Auth;
public class RegisterModel
{
    public string UserName { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }

    [EmailAddress]
    public string Email { get; set; }
    //public string Role { get; set; }
}
