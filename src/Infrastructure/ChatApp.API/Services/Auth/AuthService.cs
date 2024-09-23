using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using ChatApp.API.Data.DbContext;
using ChatApp.Domain.Models.Auth.Login;
using ChatApp.Domain.Models.Auth.Register;
using ChatApp.Domain.Models.Base;
using ChatApp.Domain.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ILogger = Serilog.ILogger;

namespace ChatApp.API.Services.Auth;

public class AuthService : IAuthService
{
    #region Private properties

    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;
    private readonly ILogger _logger;
    private readonly UserManager<UserModel> _userManager;
    private readonly string _secretKey;
    private readonly SignInManager<UserModel> _signInManager;

    public AuthService(ApplicationDbContext context, ILogger logger, UserManager<UserModel> userManager, IConfiguration configuration, SignInManager<UserModel> signInManager)
    {
        _configuration = configuration;
        _context = context;
        _logger = logger;
        _userManager = userManager;
        _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        _signInManager = signInManager;
    }

    #endregion Private properties

    #region Public Methods

    /// <inheritdoc/>
    public async Task<BaseOutputModel<RegisterOutputModel>> RegisterAsync(RegisterInputModel model)
    {
        try
        {
            if (model is null)
            {
                _logger.Warning("");
                return new BaseOutputModel<RegisterOutputModel>
                {
                    Message = "",
                    Response = null,
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            var user = _context.Users.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());

            if (user != null)
            {
                _logger.Warning("");
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

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                _logger.Warning("");
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
            _logger.Error($"");
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
                _logger.Warning("");
                return new BaseOutputModel<LoginOutputModel>
                {
                    Message = "",
                    Response = null,
                    StatusCode = HttpStatusCode.BadRequest,
                    Success = false
                };
            }

            var dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == model.Email.ToLower());

            if (dbUser is null)
            {
                _logger.Warning("");
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

            var isValid = await _userManager.CheckPasswordAsync(dbUser, model.Password);

            if (!isValid)
            {
                _logger.Warning("");
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
            var token = GenerateToken(dbUser);

            var loginResult = new BaseOutputModel<LoginOutputModel>
            {
                Message = "",
                Response = new LoginOutputModel()
                {
                    Email = dbUser.Email,
                    Token = token
                },
                StatusCode = HttpStatusCode.OK,
                Success = true
            };

            if (string.IsNullOrWhiteSpace(loginResult.Response.Email) || string.IsNullOrWhiteSpace(loginResult.Response.Token))
            {
                _logger.Warning("");
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
            _logger.Error("");
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
            _signInManager.SignOutAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error($"");
            return false;
        }
    }

    #endregion Public Methods

    #region Private methods

    /// <summary>
    /// Method used to generate a JWT token for a given user
    /// </summary>
    /// <param name="dbUser">user to generate the token</param>
    /// <returns>a string token</returns>
    private string GenerateToken(UserModel dbUser)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.ASCII.GetBytes(_secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("fullName", dbUser.Name),
                new Claim("userId", dbUser.Id),
                new Claim(ClaimTypes.Email, dbUser.Email),
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    #endregion Private methods
}