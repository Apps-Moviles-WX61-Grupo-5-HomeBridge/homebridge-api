using System.Net;
using _3_Shared.Middleware.Exceptions;

namespace _1_API.Middleware;

public class ErrorHandlerMiddleware
{
    //  @Dependencies
    private readonly RequestDelegate _next;
    
    //  @Constructor
    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        this._next = next;
    }
    
    //  @Methods
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception e)
    {
        var code = HttpStatusCode.InternalServerError;
        var result = e.Message;

        code = e switch
        {
            UserAlreadyExistsException => HttpStatusCode.BadRequest,
            _ => code
        };

        context.Response.ContentType = "text/plain";
        context.Response.StatusCode = (int) code;
        await context.Response.WriteAsync(result);
    }
}