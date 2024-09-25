using System.ComponentModel.DataAnnotations;

namespace ChatApp.Domain.Models.Auth.Login;

public class LoginInputModel
{
    [Required(ErrorMessage = "The Email is required!")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "The Password is required!")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}