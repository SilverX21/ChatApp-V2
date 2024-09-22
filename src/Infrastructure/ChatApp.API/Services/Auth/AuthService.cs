using System.Net;
using ChatApp.API.Data.DbContext;
using ChatApp.Domain.Models.Auth;
using ChatApp.Domain.Models.Auth.Login;
using ChatApp.Domain.Models.Auth.Register;
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
    public async Task<BaseOutputModel<RegisterOutputModel>> RegisterAsync(RegisterInputModel model)
    {
        try
        {
            if (model is null)
            {
                logger.Warning("");
                return new BaseOutputModel<RegisterOutputModel>
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
                return new BaseOutputModel<RegisterOutputModel>
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
                return new BaseOutputModel<RegisterOutputModel>
                {
                    Message = "",
                    Response = null,
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            return new BaseOutputModel<RegisterOutputModel>
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
            return new BaseOutputModel<RegisterOutputModel>
            {
                Message = "",
                Response = new RegisterOutputModel()
                {
                    Email = model.Email,
                    UserName = model.UserName
                },
                StatusCode = HttpStatusCode.InternalServerError,
                Success = false
            };
        }
    }

    /// <inheritdoc/>
    public async Task<BaseOutputModel<LoginOutputModel>> LoginAsync(LoginInputModel model)
    {
        try
        {
            if (model is null)
            {
                logger.Warning("");
                return new BaseOutputModel<LoginOutputModel>
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
                return new BaseOutputModel<LoginOutputModel>
                {
                    Message = "",
                    Response = new LoginOutputModel
                    {
                        Email = model.Email
                    },
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            var isValid = await userManager.CheckPasswordAsync(user, model.Password);

            if (!isValid)
            {
                logger.Warning("");
                return new BaseOutputModel<LoginOutputModel>
                {
                    Message = "",
                    Response = new LoginOutputModel
                    {
                        Email = model.Email
                    },
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            //TODO: JWT Token
            var loginResult = new BaseOutputModel<LoginOutputModel>
            {
                Message = "",
                Response = new LoginOutputModel()
                {
                    Email = user.Email,
                    Token = "TODO: REPLACE THIS WITH ACTUAL TOKEN"
                },
                StatusCode = HttpStatusCode.OK,
                Success = false
            };

            if (string.IsNullOrWhiteSpace(loginResult.Response.Email) || string.IsNullOrWhiteSpace(loginResult.Response.Token))
            {
                logger.Warning("");
                return new BaseOutputModel<LoginOutputModel>
                {
                    Message = "Please, check if the user or the password are correct.",
                    Response = new LoginOutputModel
                    {
                        Email = model.Email
                    },
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            return loginResult;
        }
        catch (Exception ex)
        {
            logger.Error("");
            return new BaseOutputModel<LoginOutputModel>
            {
                Message = "Please, check if the user or the password are correct.",
                Response = new LoginOutputModel
                {
                    Email = model.Email
                },
                StatusCode = HttpStatusCode.BadRequest,
                Success = false
            };
        }
    }

    /// <inheritdoc/>
    public async Task<bool> LogoutAsync()
    {
        try
        {
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    #endregion
}
