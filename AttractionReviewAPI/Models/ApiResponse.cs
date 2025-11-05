namespace AttractionReviewAPI;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public string ErrorCode { get; set; }

    /*
     * конструктор успешного ответа
     */
    public ApiResponse(T data, string message = "Success")
    {
        Success = true;
        Message = message;
        Data = data;
    }
    
    /*
     * конструктор ответа-ошибка
     */
    public ApiResponse(string message, string? errorCode)
    {
        Success = false;
        Message = message;
        ErrorCode = errorCode;
    }
    
    public static ApiResponse<T> Ok(T data, string message = "Success") => new(data, message);
    public static ApiResponse<T> Fail(string message, string? errorCode = null) => new(message, errorCode);
}