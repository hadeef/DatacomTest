// Ignore Spelling: Datacom

using DatacomTest.Server.Models;
using DatacomTest.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DatacomTest.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly ILogger<ApplicationController> _logger;

        public ApplicationController(IApplicationService applicationService, ILogger<ApplicationController> logger)
        {
            _applicationService = applicationService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateApplication([FromBody] Application application)
        {
            _logger.LogInformation($"Request received to create application: {application}");
            ServiceResponse<Application> response = await _applicationService.AddAsync(application);

            return response.StatusCode switch
            {
                StatusCodes.Status201Created => CreatedAtAction(nameof(GetApplicationById), new { id = response.Data!.Id }, response.Data),
                StatusCodes.Status400BadRequest => BadRequest(response.Message),
                StatusCodes.Status500InternalServerError => Problem(response.Message),
                _ => Problem("An unexpected error occurred.")
            };
        }

        [HttpGet]
        public async Task<IActionResult> GetAllApplications(int? pageNumber = null, int? pageSize = null)
        {
            _logger.LogInformation("Request received to get all applications.");
            ServiceResponse<IEnumerable<Application>> response = await _applicationService.GetAllAsync(pageNumber, pageSize);

            return response.StatusCode switch
            {
                StatusCodes.Status200OK => Ok(response.Data),
                StatusCodes.Status400BadRequest => BadRequest(response.Message),
                StatusCodes.Status404NotFound => NotFound(response.Message),
                StatusCodes.Status500InternalServerError => Problem(response.Message),
                _ => Problem("An unexpected error occurred.")
            };
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetApplicationById(int id)
        {
            _logger.LogInformation($"Request received to get application by ID: {id}");
            ServiceResponse<Application> response = await _applicationService.GetByIdAsync(id);
            return response.StatusCode switch
            {
                StatusCodes.Status200OK => Ok(response.Data),
                StatusCodes.Status400BadRequest => BadRequest(response.Message),
                StatusCodes.Status404NotFound => NotFound(response.Message),
                StatusCodes.Status500InternalServerError => Problem(response.Message),
                _ => Problem($"An unexpected error occurred.")
            };
        }

        [HttpPut]
        public async Task<IActionResult> UpdateApplication([FromBody] Application application)
        {
            ServiceResponse<Application> response = await _applicationService.UpdateAsync(application);

            return response.StatusCode switch
            {
                StatusCodes.Status200OK => Ok(response.Data),
                StatusCodes.Status400BadRequest => BadRequest(response.Message),
                StatusCodes.Status404NotFound => NotFound(response.Message),
                StatusCodes.Status500InternalServerError => Problem(response.Message),
                _ => Problem("An unexpected error occurred.")
            };
        }
    }
}