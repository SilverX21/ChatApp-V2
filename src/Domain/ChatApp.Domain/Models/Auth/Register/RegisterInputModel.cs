using System.ComponentModel.DataAnnotations;

namespace ChatApp.Domain.Models.Auth.Register;
public class RegisterInputModel
{
    public string UserName { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }

    [EmailAddress]
    public string Email { get; set; }
}
