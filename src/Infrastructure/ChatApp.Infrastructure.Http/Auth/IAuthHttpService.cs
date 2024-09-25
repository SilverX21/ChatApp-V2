using ChatApp.Domain.Models.Auth.Login;
using ChatApp.Domain.Models.Auth.Register;
using ChatApp.Domain.Models.Base;
using ChatApp.Domain.Models.Messages;
using Refit;

namespace ChatApp.Infrastructure.Http.Auth;

public interface IAuthHttpService
{
    // POST: Register a new user
    [Post("/api/auth/register")]
    Task<ApiResponse<BaseOutputModel<MessageModel>>> Register([Body] RegisterInputModel model);

    // POST: Log in a user and get JWT token
    [Post("/api/auth/login")]
    Task<ApiResponse<BaseOutputModel<LoginOutputModel>>> Login([Body] LoginInputModel model);

    // POST: Log out the current user
    [Post("/api/auth/logout")]
    Task<ApiResponse<bool>> Logout();
}