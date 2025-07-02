// Ignore Spelling: Datacom

using DatacomTest.Server.Models;
using DatacomTest.Server.Repositories.Interfaces;
using DatacomTest.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatacomTest.Server.Services;

public class ApplicationService : IApplicationService
{
    private readonly ILogger<ApplicationService> _logger;
    private readonly IRepositoryApplications _repositoryApplications;
    private readonly IValidationService _validationService;

    public ApplicationService(IRepositoryApplications repositoryApplications
        , IValidationService validationService, ILogger<ApplicationService> logger)
    {
        _repositoryApplications = repositoryApplications;
        _validationService = validationService;
        _logger = logger;
    }

    public async Task<ServiceResponse<Application>> AddAsync(Application application)
    {
        _logger.LogInformation($"Creating new application: {application}");
        ServiceResponse<Application> response = new();

        try
        {
            ValidationResponse isApplicationValid = _validationService.ValidateApplication(application);
            if (!isApplicationValid.IsValid)
            {
                _logger.LogWarning($"Application validation failed: {isApplicationValid.ErrorsAsString}");
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = isApplicationValid.ErrorsAsString;

                return response;
            }

            Application result = await _repositoryApplications.AddAsync(application);
            response.StatusCode = StatusCodes.Status201Created;
            response.Message = "Application created successfully.";
            response.Data = result;
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while creating the application for application: {application}");
            response.StatusCode = StatusCodes.Status500InternalServerError;
            response.Message = "An error occurred while creating the application.";
            return response;
        }
    }

    public async Task<ServiceResponse<IEnumerable<Application>>> GetAllAsync(int? pageNumber = null, int? pageSize = null)
    {
        _logger.LogInformation("Retrieving all applications");
        ServiceResponse<IEnumerable<Application>> response = new();

        try
        {
            IQueryable<Application> query = _repositoryApplications.GetAllQueryable();
            // Supports backend-driven pagination for efficient handling of large datasets. The
            // application implements pagination on both the backend (via query parameters) and the
            // frontend (UI).
            if (pageNumber.HasValue && pageSize.HasValue)
            {
                query = query
                            .OrderBy(ap => ap.DateApplied)
                            .ThenBy(ap => ap.CreatedAt)
                            .Skip((pageNumber.Value - 1) * pageSize.Value)
                            .Take(pageSize.Value);
            }
            IEnumerable<Application> applications = await query.ToListAsync();
            response.Data = applications;
            response.StatusCode = StatusCodes.Status200OK;
            response.Message = "Applications retrieved successfully.";
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving applications");
            response.StatusCode = StatusCodes.Status500InternalServerError;
            response.Message = "An error occurred while retrieving applications.";
            return response;
        }
    }

    public async Task<ServiceResponse<Application>> GetByIdAsync(int id)
    {
        _logger.LogInformation($"Retrieving application with ID: {id}");
        ServiceResponse<Application> response = new();

        try
        {
            Application? application = await _repositoryApplications.GetByIdAsync(id);
            if (application == null)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = "Application not found.";
                return response;
            }
            response.Data = application;
            response.StatusCode = StatusCodes.Status200OK;
            response.Message = "Application retrieved successfully.";
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while retrieving the application with ID: {id}");
            response.StatusCode = StatusCodes.Status500InternalServerError;
            response.Message = "An error occurred while retrieving the application.";
            return response;
        }
    }

    public async Task<ServiceResponse<Application>> UpdateAsync(Application application)
    {
        _logger.LogInformation($"Updating application: {application}");
        ServiceResponse<Application> response = new();
        try
        {
            // Check if the application exists
            Application? existing = await _repositoryApplications.GetByIdAsync(application.Id);
            if (existing == null)
            {
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Message = "Application not found.";
                return response;
            }

            ValidationResponse isApplicationValid = _validationService.ValidateApplication(application);
            if (!isApplicationValid.IsValid)
            {
                _logger.LogWarning($"Application validation failed: {isApplicationValid.ErrorsAsString}");
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Message = isApplicationValid.ErrorsAsString;
                return response;
            }

            existing.CompanyName = application.CompanyName;
            existing.Position = application.Position;
            existing.Status = application.Status;
            existing.DateApplied = application.DateApplied;
            existing.UpdatedAt = DateTime.UtcNow;

            await _repositoryApplications.SaveChangesAsync();

            response.StatusCode = StatusCodes.Status200OK;
            response.Message = "Application updated successfully.";
            response.Data = existing;
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while updating the application: {application}");
            response.StatusCode = StatusCodes.Status500InternalServerError;
            response.Message = "An error occurred while updating the application.";
            return response;
        }
    }
}