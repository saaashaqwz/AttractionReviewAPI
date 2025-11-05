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

    public void Invoke(HttpContext context)
    {
        try
        {
            _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "произошло необработанное исключение: {Message}", ex.Message);
            HandleException(context, ex);
        }
    }

    private void HandleException(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var response = new ApiResponse<object>(
            message: "произошло необработанное исключение",
            errorCode: "INTERNAL_ERROR");

        HttpStatusCode statusCode = HttpStatusCode.InternalServerError; // 500

        switch (exception)
        {
            case KeyNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                response.Message = "запрошенный ресурс не найден";
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
                response.Message = "доступ запрещен";
                response.ErrorCode = "UNAUTHORIZED";
                break;
        }

        context.Response.StatusCode = (int)statusCode;
        
        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        });

        context.Response.WriteAsync(json);
    }
}