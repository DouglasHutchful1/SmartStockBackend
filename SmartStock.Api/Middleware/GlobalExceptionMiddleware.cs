using System.Net;
using System.Text.Json;

namespace SmartStock.Api.Middleware;

public class GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> _logger,RequestDelegate _next)
{

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new { error = new { code = "SERVER_ERROR", message = "An unexpected error occurred.", details = ex.Message } };
            var payload = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(payload);
        }
    }
}
