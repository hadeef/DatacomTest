using System.ComponentModel.DataAnnotations;

namespace DatacomTest.Server.Models;

public class Application
{
    [Required]
    public string CompanyName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    [Required]
    public DateTime DateApplied { get; set; } = DateTime.UtcNow;

    //0 for Id is reserved for new applications that haven't been saved to the database yet.
    [Key]
    [Range(0, int.MaxValue, ErrorMessage = "Id must be greater than 0.")]
    public int Id { get; set; }

    [Required]
    public string Position { get; set; } = string.Empty;

    [Required]
    [Range(0, 3, ErrorMessage = "Status must be 0 (Applied), 1 (Interview), 2 (Offer), or 3 (Rejected).")]
    public int Status { get; set; } = 0;

    public ApplicationStatus StatusEnum => (ApplicationStatus)Status;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // This is for getting proper result when using ToString() method in debugging or logging scenarios.
    public override string ToString()
    {
        return $"Id:{Id}, Position:{Position}, Company Name:{CompanyName} , Status:{StatusEnum}, DateApplied:{DateApplied:u}";
    }
}