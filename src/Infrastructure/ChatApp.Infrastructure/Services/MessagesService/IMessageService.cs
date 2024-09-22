using ChatApp.Domain.Models.Base;
using ChatApp.Domain.Models.Messages;

namespace ChatApp.Infrastructure.Services.MessagesService;
public interface IMessageService
{
    /// <summary>
    /// Gets a message by the id
    /// </summary>
    /// <param name="messageId">message id to fetch</param>
    /// <returns>if the message creation was successful or not</returns>
    Task<BaseOutputModel<MessageModel>> GetMessageById(Guid messageId);

    /// <summary>
    /// Gets all the messages from a single user
    /// </summary>
    /// <param name="userId">user messages to fetch</param>
    /// <returns>list of messages</returns>
    Task<BaseOutputModel<List<MessageModel>>> GetAllMessagesByUser(Guid userId);

    /// <summary>
    /// Gets all the messages in the database
    /// </summary>
    /// <returns>list of messages</returns>
    Task<BaseOutputModel<List<MessageModel>>> GetAllMessages();

    /// <summary>
    /// Creates a message
    /// </summary>
    /// <param name="message">message to create in the database</param>
    /// <returns>if the message creation was successful or not</returns>
    Task<BaseOutputModel<MessageModel>> CreateMessage(MessageModel message);

    /// <summary>
    /// Deletes a given message by its id
    /// </summary>
    /// <param name="messageId">message to delete</param>
    /// <returns>if the message deletion was successful or not</returns>
    Task<BaseOutputModel<MessageModel>> DeleteMessage(Guid messageId);
}