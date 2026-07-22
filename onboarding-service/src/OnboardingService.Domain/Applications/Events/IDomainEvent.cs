namespace OnboardingService.Domain.Applications.Events;

public interface IDomainEvent
{
    Guid EventId{get;}

    DateTimeOffset OccurredAt{get;}
}