using OnboardingService.Domain.Applications;

namespace OnboardingService.Application.Applications.StartApplication;

public sealed record StartApplicationResult(
    Guid ApplicationId,
    ApplicationStatus Status,
    JourneyStep CurrentStep,
    long Version,
    DateTimeOffset ExpiresAt,
    bool Created
)
{
    public static StartApplicationResult From(
        AccountOpeningApplication application,
        bool created
    )
    {
        return new StartApplicationResult(
            ApplicationId: application.Id.Value,
            Status: application.Status,
            CurrentStep: application.CurrentStep,
            Version: application.Version,
            ExpiresAt: application.Expiration.ExpiresAt,
            Created: created
        );
    }    
}