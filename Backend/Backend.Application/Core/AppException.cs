namespace Backend.Application.Core;

public class AppException
{
    public AppException(int statusCode, string message, string details = null)
    {
        status = statusCode;
        Message = message;
        Details = details;
    }

    public int status { get; set; }
    public string Message { get; set; }
    public string Details { get; set; }
}