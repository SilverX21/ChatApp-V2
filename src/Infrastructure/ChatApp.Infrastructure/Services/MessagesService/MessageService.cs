using ChatApp.Domain.Models.Base;
using ChatApp.Domain.Models.Messages;
using ChatApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;

namespace ChatApp.Infrastructure.Services.MessagesService;
public class MessageService(ApplicationDbContext context, ILogger logger) : IMessageService
{
    /// <inheritdoc/>
    public async Task<BaseOutputModel<MessageModel>> CreateMessage(MessageModel message)
    {
        try
        {
            if (message is null)
            {
                logger.Warning("The message object to create is null. Please try again.");
                return new BaseOutputModel<MessageModel>
                {
                    Success = false,
                    Message = "There is nothing to create, the message is null.",
                    StatusCode = 400
                };
            }

            var result = await context.Messages.AddAsync(message);
            await context.SaveChangesAsync();
            logger.Information($"Message create! messageId: {result.Entity.Id}");

            return new BaseOutputModel<MessageModel>
            {
                Success = true,
                Message = "The message was created!",
                StatusCode = 201
            };
        }
        catch (Exception ex)
        {
            logger.Error($"There was an error while trying to create the message. Error: {ex.Message}");
            return new BaseOutputModel<MessageModel>
            {
                Success = false,
                Message = "There was an error while trying to create the message. please try again later.",
                StatusCode = 500
            };
        }
    }

    /// <inheritdoc/>
    public async Task<BaseOutputModel<MessageModel>> DeleteMessage(Guid messageId)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(messageId.ToString()))
            {
                logger.Warning("The message object to create is null. Please try again.");
                return new BaseOutputModel<MessageModel>
                {
                    Success = false,
                    Message = "There is nothing to create, the message is null.",
                    StatusCode = 400
                };
            }

            var message = context.Messages.FirstOrDefaultAsync(x => x.Id == messageId).Result;

            if (message == null)
            {
                logger.Warning("The message wasn't found. Please try again.");
                return new BaseOutputModel<MessageModel>
                {
                    Success = false,
                    Message = "There message requested wasn't found.",
                    StatusCode = 404
                };
            }

            var result = context.Messages.Remove(message);
            await context.SaveChangesAsync();

            logger.Information($"Message was deleted with success!");

            return new BaseOutputModel<MessageModel>
            {
                Success = true,
                Message = "The message was deleted!",
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            logger.Error($"There was an error while trying to create the message. Error: {ex.Message}");
            return new BaseOutputModel<MessageModel>
            {
                Success = false,
                Message = "There was an error while trying to create the message. please try again later.",
                StatusCode = 500
            };
        }
    }

    /// <inheritdoc/>
    public async Task<BaseOutputModel<List<MessageModel>>> GetAllMessages()
    {
        try
        {
            var result = await context.Messages.ToListAsync();

            if (!result?.Any() is true)
            {
                logger.Warning($"There are no messages in the database.");
                return new BaseOutputModel<List<MessageModel>>()
                {
                    Response = Enumerable.Empty<MessageModel>().ToList(),
                    Success = false,
                    Message = "There was an error while trying to fetch all messages! Please try again later.",
                    StatusCode = 404
                };
            }

            logger.Information($"All of the messages were fetched!");

            return new BaseOutputModel<List<MessageModel>>()
            {
                Response = result,
                Success = true,
                Message = "All of the messages were fetched!",
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            logger.Error($"There was an error while trying to fetch all messages. Error: {ex.Message}");
            return new BaseOutputModel<List<MessageModel>>()
            {
                Response = null,
                Success = false,
                Message = "There was an error while trying to fetch all messages! Please try again later.",
                StatusCode = 500
            };
        }
    }

    /// <inheritdoc/>
    public async Task<BaseOutputModel<List<MessageModel>>> GetAllMessagesByUser(Guid userId)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(userId.ToString()))
            {
                logger.Warning("Please provide the user that you want to fetch the messages.");
                return new BaseOutputModel<List<MessageModel>>
                {
                    Response = null,
                    Success = false,
                    Message = "Please provide the user that you want to fetch the messages.",
                    StatusCode = 400
                };
            }


            var result = await context.Messages.Where(x => x.UserId == userId.ToString()).ToListAsync();

            if (!result?.Any() is true)
            {
                logger.Warning($"There are no messages in the database. Timestamp {DateTime.Now}");
                return new BaseOutputModel<List<MessageModel>>()
                {
                    Response = Enumerable.Empty<MessageModel>().ToList(),
                    Success = false,
                    Message = "There was an error while trying to fetch all messages! Please try again later.",
                    StatusCode = 404
                };
            }

            logger.Information($"All of the messages were fetched!");

            return new BaseOutputModel<List<MessageModel>>()
            {
                Response = result,
                Success = true,
                Message = "All of the messages from the user provided were fetched!",
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            logger.Error($"There was an error while trying to get the given user messages. Error: {ex.Message}");
            return new BaseOutputModel<List<MessageModel>>()
            {
                Response = null,
                Success = false,
                Message = "There was an error while trying to get the given user messages. Please try again later!",
                StatusCode = 500
            };
        }
    }

    /// <inheritdoc/>
    public async Task<BaseOutputModel<MessageModel>> GetMessageById(Guid messageId)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(messageId.ToString()))
            {
                logger.Warning("The message object to create is null. Please try again.");
                return new BaseOutputModel<MessageModel>
                {
                    Success = false,
                    Message = "There is nothing to create, the message is null.",
                    StatusCode = 400
                };
            }

            var result = await context.Messages.FirstOrDefaultAsync(x => x.Id == messageId);

            if (result == null)
            {
                logger.Warning($"It wasn't found a message with the id: {messageId}");
                return new BaseOutputModel<MessageModel>
                {
                    Success = false,
                    Message = "There isn't a message the provided id.",
                    StatusCode = 404
                };
            }


            return new BaseOutputModel<MessageModel>
            {
                Response = result,
                Success = true,
                Message = "Message found.",
                StatusCode = 200
            };
        }
        catch (Exception ex)
        {
            logger.Error($"There was an error while trying to fetch the message with the id: {messageId}. Error: {ex.Message}");
            return new BaseOutputModel<MessageModel>
            {
                Response = null,
                Success = false,
                Message = "There was an error while trying to fetch the message with the given id. Please try again",
                StatusCode = 500
            };
        }
    }
}