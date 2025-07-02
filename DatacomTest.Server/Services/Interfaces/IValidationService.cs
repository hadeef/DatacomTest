// Ignore Spelling: Datacom


// Ignore Spelling: Datacom

using DatacomTest.Server.Models;

namespace DatacomTest.Server.Services.Interfaces
{
    public interface IValidationService
    {
        ValidationResponse ValidateApplication(Application application);
    }
}