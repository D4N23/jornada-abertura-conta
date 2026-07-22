using OnboardingService.Domain.Applications;
using OnboardingService.Domain.Applications.Events;
using OnboardingService.Domain.Applications.Exceptions;
using OnboardingService.Domain.Applications.ValueObjects;

namespace OnboardingService.Domain.Tests.Applications;

public sealed class AccountOpeningApplicationTests
{
    [Fact]
    public void Start_ShouldCreateApplicationInInitialState()
    {
        var now = new DateTimeOffset(
            year: 2026,
            month: 7,
            day: 21,
            hour: 18,
            minute: 0,
            second: 0,
            offset: TimeSpan.Zero
        );

        var cpf = Cpf.From("529.982.247-25");

        var subjectKey = SubjectKey.From(
            "subject-key-example"
        );

        var correlationId = CorrelationId.From(
            "correlation-id-example"
        );

        var application =
            AccountOpeningApplication.Start(
                applicantCpf: cpf,
                subjectKey: subjectKey,
                correlationId: correlationId,
                now: now,
                expirationDays: 30
            );

        Assert.NotEqual(
            Guid.Empty,
            application.Id.Value
        );

        Assert.Equal(
            cpf,
            application.ApplicantCpf
        );

        Assert.Equal(
            subjectKey,
            application.SubjectKey
        );

        Assert.Equal(
            ApplicationStatus.Started,
            application.Status
        );

        Assert.Equal(
            JourneyStep.Introduction,
            application.CurrentStep
        );

        Assert.Equal(
            1,
            application.Version
        );

        Assert.Equal(
            now,
            application.CreatedAt
        );

        Assert.Equal(
            now,
            application.UpdatedAt
        );

        Assert.Equal(
            now.AddDays(30),
            application.Expiration.ExpiresAt
        );
    }

    [Fact]
    public void Start_ShouldRaiseApplicationStartedEvent()
    {
        var now = new DateTimeOffset(
            year: 2026,
            month: 7,
            day: 21,
            hour: 18,
            minute: 0,
            second: 0,
            offset: TimeSpan.Zero
        );

        var subjectKey = SubjectKey.From(
            "subject-key-example"
        );

        var correlationId = CorrelationId.From(
            "correlation-id-example"
        );

        var application =
            AccountOpeningApplication.Start(
                applicantCpf: Cpf.From(
                    "529.982.247-25"
                ),
                subjectKey: subjectKey,
                correlationId: correlationId,
                now: now
            );

        var domainEvent = Assert.Single(
            application.DomainEvents
        );

        var applicationStarted =
            Assert.IsType<AccountOpeningApplicationStarted>(
                domainEvent
            );

        Assert.NotEqual(
            Guid.Empty,
            applicationStarted.EventId
        );

        Assert.Equal(
            application.Id,
            applicationStarted.ApplicationId
        );

        Assert.Equal(
            subjectKey,
            applicationStarted.SubjectKey
        );

        Assert.Equal(
            ApplicationStatus.Started,
            applicationStarted.Status
        );

        Assert.Equal(
            JourneyStep.Introduction,
            applicationStarted.CurrentStep
        );

        Assert.Equal(
            1,
            applicationStarted.ApplicationVersion
        );

        Assert.Equal(
            correlationId,
            applicationStarted.CorrelationId
        );

        Assert.Equal(
            now,
            applicationStarted.OccurredAt
        );
    }

    [Fact]
    public void Submit_ShouldMoveApplicationToPersonalDataStep()
    {
        var createdAt = new DateTimeOffset(
            year: 2026,
            month: 7,
            day: 22,
            hour: 12,
            minute: 0,
            second: 0,
            offset: TimeSpan.Zero
        );

        var submittedAt = createdAt.AddMinutes(10);

        var application =
            AccountOpeningApplication.Start(
                applicantCpf: Cpf.From(
                    "529.982.247-25"
                ),
                subjectKey: SubjectKey.From(
                    "subject-key-example"
                ),
                correlationId: CorrelationId.New(),
                now: createdAt
            );

        application.Submit(
            correlationId: CorrelationId.New(),
            now: submittedAt
        );

        Assert.Equal(
            ApplicationStatus.PersonalDataPending,
            application.Status
        );

        Assert.Equal(
            JourneyStep.PersonalData,
            application.CurrentStep
        );

        Assert.Equal(
            2,
            application.Version
        );

        Assert.Equal(
            createdAt,
            application.CreatedAt
        );

        Assert.Equal(
            submittedAt,
            application.UpdatedAt
        );
    }

    [Fact]
    public void Submit_ShouldRaiseApplicationSubmittedEvent()
    {
        var createdAt = new DateTimeOffset(
            year: 2026,
            month: 7,
            day: 22,
            hour: 12,
            minute: 0,
            second: 0,
            offset: TimeSpan.Zero
        );

        var submittedAt = createdAt.AddMinutes(10);

        var correlationId = CorrelationId.From(
            "submit-correlation-id"
        );

        var application =
            AccountOpeningApplication.Start(
                applicantCpf: Cpf.From(
                    "529.982.247-25"
                ),
                subjectKey: SubjectKey.From(
                    "subject-key-example"
                ),
                correlationId: CorrelationId.New(),
                now: createdAt
            );

        application.ClearDomainEvents();

        application.Submit(
            correlationId: correlationId,
            now: submittedAt
        );

        var domainEvent = Assert.Single(
            application.DomainEvents
        );

        var applicationSubmitted =
            Assert.IsType<ApplicationSubmitted>(
                domainEvent
            );

        Assert.Equal(
            application.Id,
            applicationSubmitted.ApplicationId
        );

        Assert.Equal(
            application.SubjectKey,
            applicationSubmitted.SubjectKey
        );

        Assert.Equal(
            ApplicationStatus.PersonalDataPending,
            applicationSubmitted.Status
        );

        Assert.Equal(
            JourneyStep.PersonalData,
            applicationSubmitted.CurrentStep
        );

        Assert.Equal(
            2,
            applicationSubmitted.ApplicationVersion
        );

        Assert.Equal(
            correlationId,
            applicationSubmitted.CorrelationId
        );

        Assert.Equal(
            submittedAt,
            applicationSubmitted.OccurredAt
        );
    }

    [Fact]
    public void Submit_ShouldRejectApplicationAlreadySubmitted()
    {
        var now = new DateTimeOffset(
            year: 2026,
            month: 7,
            day: 22,
            hour: 12,
            minute: 0,
            second: 0,
            offset: TimeSpan.Zero
        );

        var application =
            AccountOpeningApplication.Start(
                applicantCpf: Cpf.From(
                    "529.982.247-25"
                ),
                subjectKey: SubjectKey.From(
                    "subject-key-example"
                ),
                correlationId: CorrelationId.New(),
                now: now
            );

        application.Submit(
            correlationId: CorrelationId.New(),
            now: now.AddMinutes(1)
        );

        var exception =
            Assert.Throws<ApplicationTransitionNotAllowedException>(
                () => application.Submit(
                    correlationId: CorrelationId.New(),
                    now: now.AddMinutes(2)
                )
            );

        Assert.Equal(
            ApplicationStatus.PersonalDataPending,
            exception.CurrentStatus
        );

        Assert.Equal(
            JourneyStep.PersonalData,
            exception.CurrentStep
        );

        Assert.Equal(
            2,
            application.Version
        );
    }

    [Fact]
    public void Submit_ShouldRejectExpiredApplication()
    {
        var createdAt = new DateTimeOffset(
            year: 2026,
            month: 7,
            day: 22,
            hour: 12,
            minute: 0,
            second: 0,
            offset: TimeSpan.Zero
        );

        var application =
            AccountOpeningApplication.Start(
                applicantCpf: Cpf.From(
                    "529.982.247-25"
                ),
                subjectKey: SubjectKey.From(
                    "subject-key-example"
                ),
                correlationId: CorrelationId.New(),
                now: createdAt,
                expirationDays: 1
            );

        Assert.Throws<ApplicationExpiredException>(
            () => application.Submit(
                correlationId: CorrelationId.New(),
                now: createdAt.AddDays(1)
            )
        );

        Assert.Equal(
            ApplicationStatus.Started,
            application.Status
        );

        Assert.Equal(
            JourneyStep.Introduction,
            application.CurrentStep
        );

        Assert.Equal(
            1,
            application.Version
        );

        Assert.Equal(
            createdAt,
            application.UpdatedAt
        );
    }

    [Fact]
    public void ClearDomainEvents_ShouldRemovePendingEvents()
    {
        var application =
            AccountOpeningApplication.Start(
                applicantCpf: Cpf.From(
                    "529.982.247-25"
                ),
                subjectKey: SubjectKey.From(
                    "subject-key-example"
                ),
                correlationId: CorrelationId.New(),
                now: DateTimeOffset.UtcNow
            );

        application.ClearDomainEvents();

        Assert.Empty(application.DomainEvents);
    }

    private static ApplicantDraft CreateApplicantDraft(
       DateOnly today
    )
    {
        return new ApplicantDraft(
            FullName: FullName.From(
                "Danilo Sampaio"
            ),
            BirthDate: BirthDate.From(
                value: new DateOnly(
                    year: 1995,
                    month: 1,
                    day: 15
                ),
                today: today
            ),
            Email: EmailAddress.From(
                "danilo@example.com"
            ),
            Phone: PhoneNumber.From(
                "11999999999"
            )
        );
    }

    [Fact]
    public void RecordApplicantData_ShouldMoveToContactVerification()
    {
        var createdAt = new DateTimeOffset(
            year: 2026,
            month: 7,
            day: 22,
            hour: 12,
            minute: 0,
            second: 0,
            offset: TimeSpan.Zero
        );

        var submittedAt = createdAt.AddMinutes(1);
        var recordedAt = createdAt.AddMinutes(5);

        var application =
            AccountOpeningApplication.Start(
                applicantCpf: Cpf.From(
                    "529.982.247-25"
                ),
                subjectKey: SubjectKey.From(
                    "subject-key-example"
                ),
                correlationId: CorrelationId.New(),
                now: createdAt
            );

        application.Submit(
            correlationId: CorrelationId.New(),
            now: submittedAt
        );

        var applicantDraft = CreateApplicantDraft(
            today: DateOnly.FromDateTime(
                recordedAt.UtcDateTime
            )
        );

        application.RecordApplicantData(
            applicantDraft: applicantDraft,
            correlationId: CorrelationId.New(),
            now: recordedAt
        );

        Assert.Equal(
            applicantDraft,
            application.ApplicantDraft
        );

        Assert.Equal(
            ApplicationStatus.ContactVerificationPending,
            application.Status
        );

        Assert.Equal(
            JourneyStep.ContactVerification,
            application.CurrentStep
        );

        Assert.Equal(
            3,
            application.Version
        );

        Assert.Equal(
            recordedAt,
            application.UpdatedAt
        );
    }

    [Fact]
    public void RecordApplicantData_ShouldRaiseApplicantDataRecordedEvent()
    {
        var createdAt = new DateTimeOffset(
            year: 2026,
            month: 7,
            day: 22,
            hour: 12,
            minute: 0,
            second: 0,
            offset: TimeSpan.Zero
        );

        var recordedAt = createdAt.AddMinutes(5);

        var application =
            AccountOpeningApplication.Start(
                applicantCpf: Cpf.From(
                    "529.982.247-25"
                ),
                subjectKey: SubjectKey.From(
                    "subject-key-example"
                ),
                correlationId: CorrelationId.New(),
                now: createdAt
            );

        application.Submit(
            correlationId: CorrelationId.New(),
            now: createdAt.AddMinutes(1)
        );

        application.ClearDomainEvents();

        var correlationId = CorrelationId.From(
            "record-applicant-data-correlation"
        );

        application.RecordApplicantData(
            applicantDraft: CreateApplicantDraft(
                today: DateOnly.FromDateTime(
                    recordedAt.UtcDateTime
                )
            ),
            correlationId: correlationId,
            now: recordedAt
        );

        var domainEvent = Assert.Single(
            application.DomainEvents
        );

        var applicantDataRecorded =
            Assert.IsType<ApplicantDataRecorded>(
                domainEvent
            );

        Assert.Equal(
            application.Id,
            applicantDataRecorded.ApplicationId
        );

        Assert.Equal(
            ApplicationStatus.ContactVerificationPending,
            applicantDataRecorded.Status
        );

        Assert.Equal(
            JourneyStep.ContactVerification,
            applicantDataRecorded.CurrentStep
        );

        Assert.Equal(
            3,
            applicantDataRecorded.ApplicationVersion
        );

        Assert.Equal(
            correlationId,
            applicantDataRecorded.CorrelationId
        );

        Assert.Equal(
            recordedAt,
            applicantDataRecorded.OccurredAt
        );
    }

    [Fact]
    public void RecordApplicantData_ShouldRejectWhenIntroductionWasNotSubmitted()
    {
        var now = new DateTimeOffset(
            year: 2026,
            month: 7,
            day: 22,
            hour: 12,
            minute: 0,
            second: 0,
            offset: TimeSpan.Zero
        );

        var application =
            AccountOpeningApplication.Start(
                applicantCpf: Cpf.From(
                    "529.982.247-25"
                ),
                subjectKey: SubjectKey.From(
                    "subject-key-example"
                ),
                correlationId: CorrelationId.New(),
                now: now
            );

        var applicantDraft = CreateApplicantDraft(
            today: DateOnly.FromDateTime(
                now.UtcDateTime
            )
        );

        var exception =
            Assert.Throws<ApplicationTransitionNotAllowedException>(
                () => application.RecordApplicantData(
                    applicantDraft: applicantDraft,
                    correlationId: CorrelationId.New(),
                    now: now.AddMinutes(1)
                )
            );

        Assert.Equal(
            ApplicationStatus.Started,
            exception.CurrentStatus
        );

        Assert.Equal(
            JourneyStep.Introduction,
            exception.CurrentStep
        );

        Assert.Null(application.ApplicantDraft);

        Assert.Equal(
            1,
            application.Version
        );
    }
}