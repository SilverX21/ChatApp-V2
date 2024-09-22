using System.Net;
using ChatApp.Domain.Models.Base;
using ChatApp.Domain.Models.Messages;
using ChatApp.Domain.Models.User;
using ChatApp.Domain.Validators.MessageValidator;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ChatApp.API.Services.Messages;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace ChatApp.API.Controllers.Messages;

[Route("api/[controller]/[action]")]
[ApiController]
public class MessagesController : ControllerBase
{
    #region Constructor and interfaces

    private readonly IMessageService _messageService;
    private readonly ILogger _logger;
    private MessageValidator _messageValidator;
    private readonly UserManager<UserModel> _userManager;


    // Constructor
    public MessagesController(IMessageService messageService, ILogger logger, UserManager<UserModel> userManager)
    {
        _messageService = messageService;
        _logger = logger;
        _userManager = userManager;
        _messageValidator = new MessageValidator();
    }

    #endregion

    #region Public Methods

    [HttpPost]
    [ProducesResponseType<MessageModel>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] MessageModel message)
    {
        var validations = await ValidateMessageInputAsync(message);

        if (validations.Errors.Any())
        {
            _logger.Warning($"The message object in the input was not valid: {validations.ToString(";")}");
            return BadRequest(new BaseOutputModel<MessageModel>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Success = false,
                Response = null,
                Message = $"The input message was invalid, please validate the following: {validations.ToString("; ")}"
            });
        }

        var result = await _messageService.CreateMessage(message);

        return GenerateMessageResponse(result);
    }

    [HttpDelete("{messageId}")]
    [ProducesResponseType<MessageModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid messageId)
    {
        if (!string.IsNullOrWhiteSpace(messageId.ToString()))
        {
            _logger.Warning($"No messageId was defined to delete.");
            return BadRequest(new BaseOutputModel<MessageModel>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Success = false,
                Response = null,
                Message = $"Please define the message you want to delete."
            });
        }

        var result = await _messageService.DeleteMessage(messageId);

        return GenerateMessageResponse(result);
    }

    [HttpGet]
    [ProducesResponseType<MessageModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllMessages()
    {
        var sessionId = HttpContext.Session.Id;
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = User.Identity.Name;
        var user2 = User.Identity;
        var teste = _userManager.GetUserId(HttpContext.User);

        var result = await _messageService.GetAllMessages();

        return GenerateMessageResponse(result);
    }

    [HttpGet("{userId}")]
    [ProducesResponseType<MessageModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllMessagesByUser(Guid userId)
    {
        if (!string.IsNullOrWhiteSpace(userId.ToString()))
        {
            _logger.Warning($"No userId was defined to get all of the messages.");
            return BadRequest(new BaseOutputModel<MessageModel>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Success = false,
                Response = null,
                Message = $"Please define the user of the messages you want to fetch."
            });
        }

        var result = await _messageService.GetAllMessagesByUser(userId);

        return GenerateMessageResponse(result);
    }

    [HttpGet("{messageId}")]
    [ProducesResponseType<MessageModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetMessageById(Guid messageId)
    {
        if (!string.IsNullOrWhiteSpace(messageId.ToString()))
        {
            _logger.Warning($"No messageId was defined to get a given messages.");
            return BadRequest(new BaseOutputModel<MessageModel>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Success = false,
                Response = null,
                Message = $"Please define the message you want to fetch."
            });
        }

        var result = await _messageService.GetMessageById(messageId);

        return GenerateMessageResponse(result);
    }

    #endregion


    #region Private Methods

    /// <summary>
    /// Creates the right return for a given response from the server
    /// </summary>
    /// <typeparam name="T">Type of the Model that will be validated</typeparam>
    /// <param name="result">Result given from the server given the user input</param>
    /// <returns>IActionResult with the correct status code</returns>
    public IActionResult GenerateMessageResponse<T>(BaseOutputModel<T> result) where T : class

    {
        return result.StatusCode switch
        {
            HttpStatusCode.OK => Ok(result),
            HttpStatusCode.Created => Created(string.Empty, result),
            HttpStatusCode.BadRequest => BadRequest(result),
            HttpStatusCode.NotFound => NotFound(result),
            HttpStatusCode.InternalServerError => StatusCode(500, result),
            _ => BadRequest(result),
        };
    }

    /// <summary>
    /// Validates the message from a user input
    /// </summary>
    /// <param name="message">message from the user input</param>
    /// <returns>validation results with errors, if there are any</returns>
    private async Task<ValidationResult> ValidateMessageInputAsync(MessageModel message)
    {
        var validationResults = await _messageValidator.ValidateAsync(message);

        return validationResults;
    }

    #endregion
}