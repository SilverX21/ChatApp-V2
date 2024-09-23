using System.Net;
using ChatApp.API.Services.Auth;
using ChatApp.Domain.Helpers;
using ChatApp.Domain.Models.Auth.Login;
using ChatApp.Domain.Models.Auth.Register;
using ChatApp.Domain.Models.Base;
using ChatApp.Domain.Models.Messages;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers.Auth;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    #region Constructors and interfaces

    private readonly string _secretKey;
    private readonly IAuthService _authService;
    private readonly HttpResponseHelper _httpResponseHelper;

    public AuthController(IConfiguration configuration, IAuthService authService)
    {
        _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        _authService = authService;
        _httpResponseHelper = new HttpResponseHelper();
    }

    #endregion Constructors and interfaces

    #region Public methods

    /// <summary>
    /// Registers a user in the application
    /// </summary>
    /// <param name="model">user to register</param>
    /// <returns>Object with the success of the operation</returns>
    [HttpPost]
    [ProducesResponseType<MessageModel>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] RegisterInputModel model)
    {
        //create validator

        var result = await _authService.RegisterAsync(model);

        return _httpResponseHelper.GenerateMessageResponse(result);
    }

    /// <summary>
    /// Logs a given user into the application
    /// </summary>
    /// <param name="model">user to log in</param>
    /// <returns>login in the session with a jwt token</returns>
    [HttpPost]
    [ProducesResponseType<MessageModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginInputModel model)
    {
        var result = await _authService.LoginAsync(model);

        return _httpResponseHelper.GenerateMessageResponse(SetUserSession(result));
    }

    /// <summary>
    /// Logs out the current user
    /// </summary>
    /// <returns>true or false if the operation is successful</returns>
    [HttpPost]
    [ProducesResponseType<MessageModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Logout()
    {
        var result = await _authService.LogoutAsync();

        if (result is false)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    #endregion Public methods

    #region Private methods

    /// <summary>
    /// Sets the user id in session
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public BaseOutputModel<LoginOutputModel> SetUserSession(BaseOutputModel<LoginOutputModel> result)
    {
        if (!string.IsNullOrWhiteSpace(result?.Response?.Token))
        {
            var userId = HttpContext?.User?.Claims?.Where(x => x.Type == "userId")?.FirstOrDefault()?.Value;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                HttpContext?.Session?.SetString("currentUserId", userId);
            }
            else
            {
                result.Success = false;
                result.StatusCode = HttpStatusCode.BadRequest;
                result.Message = "It wasn't possible to generate the user session. Please try again.";
            }
        }
        else
        {
            result.Success = false;
            result.StatusCode = HttpStatusCode.BadRequest;
            result.Message = "It wasn't possible to generate the user session. Please try again.";
        }

        return result;
    }

    #endregion Private methods
}