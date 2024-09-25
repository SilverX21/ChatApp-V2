using System.Net;
using ChatApp.Domain.Models.Base;
using ChatApp.Domain.Models.Messages;
using ChatApp.Domain.Models.User;
using ChatApp.Domain.Validators.MessageValidator;
using Microsoft.AspNetCore.Identity;
using ChatApp.API.Services.Messages;
using ChatApp.Domain.Helpers;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
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
    private readonly HttpResponseHelper _httpResponseHelper;

    public MessagesController(IMessageService messageService, ILogger logger, UserManager<UserModel> userManager)
    {
        _messageService = messageService;
        _logger = logger;
        _httpResponseHelper = new HttpResponseHelper();
        _messageValidator = new MessageValidator();
    }

    #endregion Constructor and interfaces

    #region Public Methods

    [HttpPost]
    [Authorize]
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

        return _httpResponseHelper.GenerateMessageResponse(result);
    }

    [HttpDelete("{messageId}")]
    [Authorize]
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

        var userId = HttpContext?.User?.Claims?.Where(x => x.Type == "userId")?.FirstOrDefault()?.Value;

        var result = await _messageService.DeleteMessage(messageId);

        return _httpResponseHelper.GenerateMessageResponse(result);
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType<MessageModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllMessages()
    {
        var result = await _messageService.GetAllMessages();

        return _httpResponseHelper.GenerateMessageResponse(result);
    }

    [HttpGet("{userId}")]
    [Authorize]
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

        return _httpResponseHelper.GenerateMessageResponse(result);
    }

    [HttpGet("{messageId}")]
    [Authorize]
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

        return _httpResponseHelper.GenerateMessageResponse(result);
    }

    #endregion Public Methods

    #region Private Methods

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

    #endregion Private Methods
}