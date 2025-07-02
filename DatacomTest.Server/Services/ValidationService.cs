// Ignore Spelling: Datacom

using DatacomTest.Server.Models;
using DatacomTest.Server.Services.Interfaces;

namespace DatacomTest.Server.Services;

public class ValidationService : IValidationService
{
    private readonly ILogger<ValidationService> _logger;

    public ValidationService(ILogger<ValidationService> logger)
    {
        _logger = logger;
    }

    public ValidationResponse ValidateApplication(Application application)
    {
        //Already overridden the .ToString() method on Application to provide a better logging experience
        _logger.LogInformation($"Validating application data, application:{application}");

        ValidationResponse response = new();

        if (application == null)
        {
            response.Errors.Add("Application data cannot be null.");
            return response;
        }

        if (application.Id < 0)
        {
            response.Errors.Add("Application id is invalid.");
        }

        if (string.IsNullOrWhiteSpace(application.CompanyName))
        {
            response.Errors.Add("Company name is required.");
        }

        if (string.IsNullOrWhiteSpace(application.Position))
        {
            response.Errors.Add("Position is required.");
        }

        if (application.DateApplied == default)
        {
            response.Errors.Add("Date applied is required.");
        }

        if (application.Status is < 0 or > 3)
        {
            response.Errors.Add("Status must be  0 (Applied), 1 (Interview), 2 (Offer), or 3 (Rejected).");
        }

        if (application.CreatedAt == default)
        {
            response.Errors.Add("CreatedAt is required.");
        }

        if (response.IsValid)
        {
            _logger.LogInformation($"Application data is valid, application:{application}");
        }
        else
        {
            _logger.LogWarning($"Application data validation failed: {response.ErrorsAsString}");
        }
        return response;
    }
}