using System.ComponentModel.DataAnnotations;

namespace ChatApp.Domain.Models.Auth.Register;
public class RegisterOutputModel
{
    public string UserName { get; set; }

    [EmailAddress]
    public string Email { get; set; }
}
