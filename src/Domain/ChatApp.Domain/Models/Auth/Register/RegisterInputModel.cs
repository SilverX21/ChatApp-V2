using System.ComponentModel.DataAnnotations;

namespace ChatApp.Domain.Models.Auth.Register;

public class RegisterInputModel
{
    [Required(ErrorMessage = "The Username is required!")]
    public string UserName { get; set; }

    public string Name { get; set; }

    [Required(ErrorMessage = "The Password is required!")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "The Email is required!")]
    [EmailAddress]
    public string Email { get; set; }
}