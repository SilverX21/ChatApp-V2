using ChatApp.Domain.Models.Auth;
using ChatApp.Domain.Models.Base;

namespace ChatApp.API.Services.Auth;

public interface IAuthService
{
    /// <summary>
    /// Methods that registers the user in the application
    /// </summary>
    /// <param name="model">user to create</param>
    /// <returns>Model with status and the user created</returns>
    Task<BaseOutputModel<RegisterModel>> RegisterAsync(RegisterModel model);

    /// <summary>
    /// Methods that enables the user to login in the app
    /// </summary>
    /// <param name="model">user to authenticate</param>
    /// <returns>the user that logged in</returns>
    Task<BaseOutputModel<LoginModel>> LoginAsync(LoginModel model);

    /// <summary>
    /// Method that logs out the current user
    /// </summary>
    /// <returns>true or false if the user logged out successfully</returns>
    Task<bool> LogoutAsync();
}
