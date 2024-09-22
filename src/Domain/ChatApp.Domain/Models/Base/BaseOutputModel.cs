using System.Net;

namespace ChatApp.Domain.Models.Base;
public class BaseOutputModel<T> where T : class
{
    public bool Success { get; set; }

    public T? Response { get; set; }

    public string Message { get; set; } = string.Empty;

    public HttpStatusCode StatusCode { get; set; }
}