namespace DatacomTest.Server.Models;

public class ValidationResponse
{
    public List<string> Errors { get; set; } = [];
    public string ErrorsAsString => string.Join(", ", Errors);
    public bool IsValid => !Errors.Any();
}