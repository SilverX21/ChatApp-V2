using ChatApp.Domain.Models.Base;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Domain.Helpers;
public class HttpResponseHelper : ControllerBase
{
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
}
