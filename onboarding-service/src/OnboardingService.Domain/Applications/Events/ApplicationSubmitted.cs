using OnboardingService.Domain.Applications.ValueObjects;
using ApplicationId = OnboardingService.Domain.Applications.ValueObjects.ApplicationId;

namespace OnboardingService.Domain.Applications.Events;

public sealed record ApplicationSubmitted(
    Guid EventId,
    ApplicationId ApplicationId,
    SubjectKey SubjectKey,
    ApplicationStatus Status,
    JourneyStep CurrentStep,
    long ApplicationVersion,
    CorrelationId CorrelationId,
    DateTimeOffset OccurredAt
) : IDomainEvent
{
    public DateTimeOffset OccurretAt => throw new NotImplementedException();
}