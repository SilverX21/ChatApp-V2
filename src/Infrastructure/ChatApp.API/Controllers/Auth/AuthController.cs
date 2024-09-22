using ChatApp.API.Services.Auth;
using ChatApp.Domain.Helpers;
using ChatApp.Domain.Models.Auth;
using ChatApp.Domain.Models.Messages;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers.Auth;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly string _secretKey;
    private readonly IAuthService _authService;
    private readonly HttpResponseHelper _httpResponseHelper;

    public AuthController(IConfiguration configuration, IAuthService authService)
    {
        _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        _authService = authService;
        _httpResponseHelper = new HttpResponseHelper();
    }

    // GET: api/<LoginController>
    [HttpPost]
    [ProducesResponseType<MessageModel>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        //create validator

        var result = await _authService.RegisterAsync(model);

        return _httpResponseHelper.GenerateMessageResponse(result);
    }

    // GET api/<LoginController>/5
    [HttpPost]
    [ProducesResponseType<MessageModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {

        var result = await _authService.LoginAsync(model);

        return _httpResponseHelper.GenerateMessageResponse(result);
    }

    // POST api/<LoginController>
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
}
