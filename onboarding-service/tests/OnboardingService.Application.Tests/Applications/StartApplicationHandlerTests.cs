using OnboardingService.Application.Abstractions;
using OnboardingService.Application.Applications.StartApplication;
using OnboardingService.Domain.Applications;
using OnboardingService.Domain.Applications.ValueObjects;

namespace OnboardingService.Application.Tests.Applications;

public sealed class StartApplicationHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldCreateApplicationWhenNoneExists()
    {
        var now = new DateTimeOffset(
            year: 2026,
            month: 7,
            day: 22,
            hour: 18,
            minute: 0,
            second: 0,
            offset: TimeSpan.Zero
        );

        var repository = new FakeAccountOpeningApplicationRepository();

        var unitOfWork = new FakeUnitOfWork();

        var subjectKeyFactory = new FakeSubjectKeyFactory(SubjectKey.From("subject-key-example"));

        var handler = new StartApplicationHandler(
            repository: repository,
            subjectKeyFactory: subjectKeyFactory,
            unitOfWork: unitOfWork,
            timeProvider: new FixedTimeProvider(now)
        );

        var result = await handler.HandleAsync(
            new StartApplicationCommand(
                Cpf: "529.982.247-25",
                CorrelationId: "correlation-id-example"
            )
        );

        Assert.True(result.Created);

        Assert.NotEqual(Guid.Empty,result.ApplicationId);

        Assert.Equal(ApplicationStatus.Started,result.Status);

        Assert.Equal(JourneyStep.Introduction,result.CurrentStep);

        Assert.Equal(1,result.Version);

        Assert.Equal(now.AddDays(30),result.ExpiresAt);

        var addedApplication = Assert.Single(repository.AddedApplications);

        Assert.Equal(result.ApplicationId,addedApplication.Id.Value);

        Assert.Equal(1,unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnExistingActiveApplication()
    {
        var now = new DateTimeOffset(
            year: 2026,
            month: 7,
            day: 22,
            hour: 18,
            minute: 0,
            second: 0,
            offset: TimeSpan.Zero
        );

        var subjectKey = SubjectKey.From("subject-key-example");

        var existingApplication =
            AccountOpeningApplication.Start(
                applicantCpf: Cpf.From(
                    "529.982.247-25"
                ),
                subjectKey: subjectKey,
                correlationId: CorrelationId.New(),
                now: now.AddMinutes(-10)
            );

        var repository = new FakeAccountOpeningApplicationRepository {ActiveApplication = existingApplication};

        var unitOfWork = new FakeUnitOfWork();

        var handler = new StartApplicationHandler(
            repository: repository,
            subjectKeyFactory:
                new FakeSubjectKeyFactory(subjectKey),
            unitOfWork: unitOfWork,
            timeProvider: new FixedTimeProvider(now)
        );

        var result = await handler.HandleAsync(
            new StartApplicationCommand(
                Cpf: "529.982.247-25",
                CorrelationId: "another-correlation-id"
            )
        );

        Assert.False(result.Created);

        Assert.Equal(existingApplication.Id.Value,result.ApplicationId);

        Assert.Empty(repository.AddedApplications);

        Assert.Equal(0,unitOfWork.SaveChangesCallCount);
    }

        private sealed class
        FakeAccountOpeningApplicationRepository
        : IAccountOpeningApplicationRepository
    {
        public AccountOpeningApplication?
            ActiveApplication { get; init; }

        public List<AccountOpeningApplication>
            AddedApplications { get; } = [];

        public Task<AccountOpeningApplication?>
            FindActiveBySubjectKeyAsync(
                SubjectKey subjectKey,
                CancellationToken cancellationToken = default
            )
        {
            return Task.FromResult(
                ActiveApplication
            );
        }

        public Task AddAsync(
            AccountOpeningApplication application,
            CancellationToken cancellationToken = default
        )
        {
            AddedApplications.Add(application);

            return Task.CompletedTask;
        }
    }

    private sealed class FakeSubjectKeyFactory
        : ISubjectKeyFactory
    {
        private readonly SubjectKey _subjectKey;

        public FakeSubjectKeyFactory(
            SubjectKey subjectKey
        )
        {
            _subjectKey = subjectKey;
        }

        public SubjectKey CreateFrom(Cpf cpf)
        {
            return _subjectKey;
        }
    }

    private sealed class FakeUnitOfWork
        : IUnitOfWork
    {
        public int SaveChangesCallCount { get; private set; }

        public Task SaveChangesAsync(
            CancellationToken cancellationToken = default
        )
        {
            SaveChangesCallCount++;

            return Task.CompletedTask;
        }
    }

    private sealed class FixedTimeProvider : TimeProvider
    {
        private readonly DateTimeOffset _utcNow;

        public FixedTimeProvider(DateTimeOffset utcNow){_utcNow = utcNow;}

        public override DateTimeOffset GetUtcNow(){return _utcNow;}
    }
}