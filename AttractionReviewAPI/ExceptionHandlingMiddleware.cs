using System.Net;
using System.Text.Json;

namespace AttractionReviewAPI;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошло необработанное исключение: {Message}", ex.Message);
            await HandleException(context, ex);
        }
    }

    private async Task HandleException(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var response = new ApiResponse<object>(
            message: "Произошло необработанное исключение",
            errorCode: "INTERNAL_ERROR");

        HttpStatusCode statusCode = HttpStatusCode.InternalServerError; // 500

        switch (exception)
        {
            case KeyNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                response.Message = "Запрошенный ресурс не найден";
                response.ErrorCode = "NOT_FOUND";
                break;

            case ArgumentException:
            case InvalidOperationException:
                statusCode = HttpStatusCode.BadRequest;
                response.Message = exception.Message;
                response.ErrorCode = "BAD_REQUEST";
                break;

            case UnauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                response.Message = "Доступ запрещен";
                response.ErrorCode = "UNAUTHORIZED";
                break;
        }

        context.Response.StatusCode = (int)statusCode;
        
        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });

        await context.Response.WriteAsync(json);
    }
}