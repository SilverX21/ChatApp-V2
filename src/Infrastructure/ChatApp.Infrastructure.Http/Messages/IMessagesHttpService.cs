using ChatApp.Domain.Models.Base;
using ChatApp.Domain.Models.Messages;
using Refit;

namespace ChatApp.Infrastructure.Http.Messages;

public interface IMessagesHttpService
{
    // POST: Create a new message
    [Post("/api/messages/create")]
    Task<ApiResponse<BaseOutputModel<MessageOutputModel>>> CreateMessage([Body] MessageInputModel message);

    // DELETE: Delete a message by ID
    [Delete("/api/messages/delete/{messageId}")]
    Task<ApiResponse<BaseOutputModel<MessageOutputModel>>> DeleteMessage(Guid messageId);

    // GET: Get all messages
    [Get("/api/messages/getallmessages")]
    Task<ApiResponse<List<MessageModel>>> GetAllMessages();

    // GET: Get all messages by user ID
    [Get("/api/messages/getallmessagesbyuser/{userId}")]
    Task<ApiResponse<List<MessageModel>>> GetAllMessagesByUser(Guid userId);

    // GET: Get a message by ID
    [Get("/api/messages/getmessagebyid/{messageId}")]
    Task<ApiResponse<MessageModel>> GetMessageById(Guid messageId);

    [Get("/api/messages/getallmessages")]
    Task<ApiResponse<List<MessageModel>>> GetAllMessages([Header("Authorization")] string authorization);
}