using OnboardingService.Domain.Applications.Events;
using OnboardingService.Domain.Applications.Exceptions;
using OnboardingService.Domain.Applications.ValueObjects;
using ApplicationId = OnboardingService.Domain.Applications.ValueObjects.ApplicationId;

namespace OnboardingService.Domain.Applications;

public sealed class AccountOpeningApplication
{
    private readonly List<IDomainEvent>_domainEvents =[];

    private AccountOpeningApplication()
    {}

    public ApplicationId Id {get; private set;}

    public SubjectKey SubjectKey {get; private set;}

    public Cpf ApplicantCpf {get; private set;}

    public ApplicationStatus Status {get; private set;}

    public JourneyStep CurrentStep {get; private set;}

    public ApplicationExpiration Expiration {get; private set;}

    public long Version {get; private set;}

    public DateTimeOffset CreatedAt {get; private set;}

    public DateTimeOffset UpdatedAt { get; private set;}

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public static AccountOpeningApplication Start(
        Cpf applicantCpf,
        SubjectKey subjectKey,
        CorrelationId correlationId,
        DateTimeOffset now,
        int expirationDays = 30
    )
    {
        var occurredAt = now.ToUniversalTime();
        var application = new AccountOpeningApplication
        {
            Id = ApplicationId.New(),
            ApplicantCpf = applicantCpf,
            SubjectKey = subjectKey,
            Status  = ApplicationStatus.Started,
            CurrentStep = JourneyStep.Introduction,
            Expiration = ApplicationExpiration.InDays(
                occurredAt,
                expirationDays
            ),
            Version = 1,
            CreatedAt = occurredAt,
            UpdatedAt = occurredAt
        };

        application.RaiseDomainEvent(
            new AccountOpeningApplicationStarted(
                EventId: Guid.NewGuid(),
                ApplicationId: application.Id,
                SubjectKey: application.SubjectKey,
                Status: application.Status,
                CurrentStep: application.CurrentStep,
                ApplicationVersion: application.Version,
                CorrelationId: correlationId,
                OccurredAt: occurredAt
            )
        );

        return application;
    }


    public void Submit(
        CorrelationId correlationId,
        DateTimeOffset now
    )
    {
        var occurredAt = now.ToUniversalTime();

        EnsureNotExpired(occurredAt);
        EnsureCanSubmit();

        Status = ApplicationStatus.PersonalDataPending;
        CurrentStep = JourneyStep.PersonalData;
        Version++;
        UpdatedAt = occurredAt;

        RaiseDomainEvent(
            new ApplicationSubmitted(
                 EventId: Guid.NewGuid(),
                ApplicationId: Id,
                SubjectKey: SubjectKey,
                Status: Status,
                CurrentStep: CurrentStep,
                ApplicationVersion: Version,
                CorrelationId: correlationId,
                OccurredAt: occurredAt
            )
        );
    }

    private void EnsureCanSubmit()
    {
        var isValidState = Status == ApplicationStatus.Started && CurrentStep == JourneyStep.Introduction;

        if (!isValidState)
        {
            throw new ApplicationTransitionNotAllowedException(
                operation: nameof(Submit),
                currentStatus: Status,
                currentStep: CurrentStep
            );
        }
    }

    private void EnsureNotExpired(DateTimeOffset instant)
    {
        if(Expiration.IsExpiredAt(instant))
        {
            throw new ApplicationExpiredException(
                applicationId: Id,
                expiresAt: Expiration.ExpiresAt
            );
        }
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();    
    }

    private void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}