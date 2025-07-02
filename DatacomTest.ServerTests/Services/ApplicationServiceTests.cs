using DatacomTest.Server.Models;
using DatacomTest.Server.Repositories.Interfaces;
using DatacomTest.Server.Services;
using DatacomTest.Server.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace DatacomTest.ServerTests.Services;

[TestClass]
public class ApplicationServiceTests
{
    [TestMethod]
    public async Task AddAsync_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        Mock<IRepositoryApplications> mockRepo = new();
        Mock<ILogger<ValidationService>> mockLoggerValidation = new();
        ValidationService realValidation = new(mockLoggerValidation.Object);
        Mock<ILogger<ApplicationService>> mockLogger = new();

        // Create an invalid application (missing required CompanyName)
        Application invalidApp = new()
        {
            Id = 0,
            CompanyName = "", // Invalid: required
            Position = "Developer",
            Status = 0,
            DateApplied = DateTime.UtcNow
        };

        ApplicationService service = new(mockRepo.Object, realValidation, mockLogger.Object);

        // Act
        ServiceResponse<Application> result = await service.AddAsync(invalidApp);

        // Assert
        Assert.AreEqual(400, result.StatusCode);
        Assert.IsTrue(!string.IsNullOrEmpty(result.Message));
        Assert.IsNull(result.Data);
    }

    [TestMethod]
    public async Task GetByIdAsync_ShouldReturnApplication_WhenExists()
    {
        // Arrange
        Mock<IRepositoryApplications> mockRepo = new();
        Mock<IValidationService> mockValidation = new();
        Mock<ILogger<ApplicationService>> mockLogger = new();

        Application app = new()
        {
            Id = 1,
            CompanyName = "TestCo",
            Position = "Engineer",
            Status = 0,
            DateApplied = DateTime.UtcNow
        };

        _ = mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(app);

        ApplicationService service = new(mockRepo.Object, mockValidation.Object, mockLogger.Object);

        // Act
        ServiceResponse<Application> result = await service.GetByIdAsync(1);

        // Assert
        Assert.AreEqual(200, result.StatusCode);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual("TestCo", result.Data.CompanyName);
        Assert.AreEqual("Engineer", result.Data.Position);
        Assert.AreEqual(0, result.Data.Status);
    }

    [TestMethod]
    public async Task UpdateAsync_ShouldUpdateApplication_WhenValid()
    {
        // Arrange
        Mock<IRepositoryApplications> mockRepo = new();
        Mock<IValidationService> mockValidation = new();
        Mock<ILogger<ApplicationService>> mockLogger = new();

        Application existingApp = new()
        {
            Id = 1,
            CompanyName = "OldCo",
            Position = "Dev",
            Status = 0,
            DateApplied = DateTime.UtcNow.AddDays(-1)
        };

        Application updatedApp = new()
        {
            Id = 1,
            CompanyName = "NewCo",
            Position = "Lead Dev",
            Status = 1,
            DateApplied = DateTime.UtcNow
        };

        _ = mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingApp);
        _ = mockValidation.Setup(v => v.ValidateApplication(It.IsAny<Application>()))
            .Returns(new ValidationResponse { Errors = [] });
        _ = mockRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

        ApplicationService service = new(mockRepo.Object, mockValidation.Object, mockLogger.Object);

        // Act
        ServiceResponse<Application> result = await service.UpdateAsync(updatedApp);

        // Assert
        Assert.AreEqual(200, result.StatusCode);
        Assert.IsNotNull(result.Data);
        Assert.AreEqual("NewCo", result.Data.CompanyName);
        Assert.AreEqual("Lead Dev", result.Data.Position);
        Assert.AreEqual(1, result.Data.Status);
    }
}