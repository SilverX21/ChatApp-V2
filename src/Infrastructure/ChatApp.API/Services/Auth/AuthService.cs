using System.Net;
using ChatApp.API.Data.DbContext;
using ChatApp.Domain.Models.Auth;
using ChatApp.Domain.Models.Base;
using ChatApp.Domain.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;

namespace ChatApp.API.Services.Auth;

public class AuthService(ApplicationDbContext context, ILogger logger, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager) : IAuthService
{

    #region Public Methods

    /// <inheritdoc/>
    public async Task<BaseOutputModel<RegisterModel>> RegisterAsync(RegisterModel model)
    {
        try
        {
            if (model is null)
            {
                logger.Warning("");
                return new BaseOutputModel<RegisterModel>
                {
                    Message = "",
                    Response = null,
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            var user = context.Users.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());

            if (user != null)
            {
                logger.Warning("");
                return new BaseOutputModel<RegisterModel>
                {
                    Message = "",
                    Response = null,
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            UserModel newUser = new()
            {
                UserName = model.UserName,
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                Name = model.Name
            };

            var result = await userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                logger.Warning("");
                return new BaseOutputModel<RegisterModel>
                {
                    Message = "",
                    Response = null,
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            return new BaseOutputModel<RegisterModel>
            {
                Message = "",
                Response = null,
                StatusCode = HttpStatusCode.Created,
                Success = true
            };
        }
        catch (Exception ex)
        {
            logger.Error($"");
            return new BaseOutputModel<RegisterModel>
            {
                Message = "",
                Response = null,
                StatusCode = HttpStatusCode.InternalServerError,
                Success = false
            };
        }
    }

    /// <inheritdoc/>
    public async Task<BaseOutputModel<LoginModel>> LoginAsync(LoginModel model)
    {
        try
        {
            if (model is null)
            {
                logger.Warning("");
                return new BaseOutputModel<LoginModel>
                {
                    Message = "",
                    Response = null,
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            var user = await context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == model.Email.ToLower());

            if (user is null)
            {
                logger.Warning("");
                return new BaseOutputModel<LoginModel>
                {
                    Message = "",
                    Response = null,
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            var isValid = await userManager.CheckPasswordAsync(user, model.Password);

            if (!isValid)
            {
                logger.Warning("");
                return new BaseOutputModel<LoginModel>
                {
                    Message = "",
                    Response = null,
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            //TODO: JWT Token
            var loginResult = new BaseOutputModel<LoginModel>
            {
                Message = "",
                Response = new LoginModel()
                {
                    Email = user.Email,
                    Token = "TODO: REPLACE THIS WITH ACTUAL TOKEN"
                },
                StatusCode = HttpStatusCode.OK,
                Success = false
            };

            if (string.IsNullOrWhiteSpace(loginResult.Response.Email) || string.IsNullOrWhiteSpace(loginResult.Response.Token))
            {
                logger.Error("");
                return new BaseOutputModel<LoginModel>
                {
                    Message = "Please, check if the user or the password are correct.",
                    Response = null,
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            return loginResult;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> LogoutAsync()
    {
        throw new NotImplementedException();
    }

    #endregion
}
