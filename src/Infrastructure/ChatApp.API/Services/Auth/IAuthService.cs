using ChatApp.Domain.Models.Auth.Login;
using ChatApp.Domain.Models.Auth.Register;
using ChatApp.Domain.Models.Base;
using ChatApp.Domain.Models.User;

namespace ChatApp.API.Services.Auth;

public interface IAuthService
{
    /// <summary>
    /// Methods that registers the user in the application
    /// </summary>
    /// <param name="model">user to create</param>
    /// <returns>Model with status and the user created</returns>
    Task<BaseOutputModel<RegisterOutputModel>> RegisterAsync(RegisterInputModel model);

    /// <summary>
    /// Methods that enables the user to login in the app
    /// </summary>
    /// <param name="model">user to authenticate</param>
    /// <returns>the user that logged in</returns>
    Task<BaseOutputModel<LoginOutputModel>> LoginAsync(LoginInputModel model);

    /// <summary>
    /// Method that logs out the current user
    /// </summary>
    /// <returns>true or false if the user logged out successfully</returns>
    Task<bool> LogoutAsync();

    /// <summary>
    /// Signs in the user after creating the login credentials
    /// </summary>
    /// <param name="user">user to authenticate</param>
    /// <returns>true or false if successful or not</returns>
    Task<bool> SignInAsync(UserModel user);
}