namespace DatacomTest.Server.Models;

public class ServiceResponse<T>
{
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; } = StatusCodes.Status200OK;// Default to OK
    public bool Success => StatusCode == StatusCodes.Status200OK;
}