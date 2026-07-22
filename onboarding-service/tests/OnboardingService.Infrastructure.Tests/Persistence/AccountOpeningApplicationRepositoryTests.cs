using Microsoft.EntityFrameworkCore;
using OnboardingService.Application.Abstractions;
using OnboardingService.Domain.Applications;
using OnboardingService.Domain.Applications.ValueObjects;
using OnboardingService.Infrastructure.Persistence;
using OnboardingService.Infrastructure.Persistence.Repositories;

namespace OnboardingService.Infrastructure.Tests.Persistence;

public sealed class AccountOpeningApplicationRepositoryTests : IClassFixture<PostgreSqlFixture>
{
    private readonly PostgreSqlFixture _fixture;

    public AccountOpeningApplicationRepositoryTests(PostgreSqlFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task AddAsync_ShouldPersistAndRehydrateApplication()
    {
        await using var dbContext = await CreateDbContextAsync();

        var repository = new AccountOpeningApplicationRepository(dbContext);

        var unitOfWork = (IUnitOfWork)dbContext;

        var createdAt = new DateTimeOffset(
            year: 2026,
            month: 7,
            day: 22,
            hour: 18,
            minute: 0,
            second: 0,
            offset: TimeSpan.Zero
        );

        var subjectKey = SubjectKey.From($"subject-key-{Guid.NewGuid()}");

        var application = AccountOpeningApplication.Start(
                applicantCpf: Cpf.From(
                    "529.982.247-25"
                ),
                subjectKey: subjectKey,
                correlationId: CorrelationId.New(),
                now: createdAt
            );

        await repository.AddAsync(application);

        await unitOfWork.SaveChangesAsync();

        dbContext.ChangeTracker.Clear();

        var persistedApplication =await repository.FindActiveBySubjectKeyAsync(subjectKey);

        Assert.NotNull(persistedApplication);

        Assert.Equal(application.Id, persistedApplication.Id);

        Assert.Equal(application.SubjectKey, persistedApplication.SubjectKey);

        Assert.Equal(application.ApplicantCpf, persistedApplication.ApplicantCpf);

        Assert.Equal(ApplicationStatus.Started, persistedApplication.Status);

        Assert.Equal(JourneyStep.Introduction,persistedApplication.CurrentStep);

        Assert.Equal(1,persistedApplication.Version);

        Assert.Equal(createdAt, persistedApplication.CreatedAt);

        Assert.Empty(persistedApplication.DomainEvents);
    }

    private async Task<OnboardingDbContext>CreateDbContextAsync()
    {
        var options = new DbContextOptionsBuilder<OnboardingDbContext>()
            .UseNpgsql(_fixture.Container.GetConnectionString())
            .Options;

        var dbContext = new OnboardingDbContext(options);

        await dbContext.Database.MigrateAsync();

        return dbContext;
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldRejectTwoActiveApplicationsForSameSubject()
    {
        await using var dbContext = await CreateDbContextAsync();
        var repository = new AccountOpeningApplicationRepository(dbContext);
        var unitOfWork = (IUnitOfWork)dbContext;
        var subjectKey = SubjectKey.From($"duplicate-subject-{Guid.NewGuid()}");

        var now = new DateTimeOffset(
            year: 2026,
            month: 7,
            day: 22,
            hour: 18,
            minute: 0,
            second: 0,
            offset: TimeSpan.Zero
        );

        var firstApplication = AccountOpeningApplication.Start(
            applicantCpf: Cpf.From("529.982.247-25"),
            subjectKey: subjectKey,
            correlationId: CorrelationId.New(),
            now: now);

        var secondApplication = AccountOpeningApplication.Start(
            applicantCpf: Cpf.From("529.982.247-25"),
            subjectKey: subjectKey,
            correlationId: CorrelationId.New(),
            now: now.AddSeconds(1));

        await repository.AddAsync(firstApplication);
        await repository.AddAsync(secondApplication);
        await Assert.ThrowsAsync<DbUpdateException>(() => unitOfWork.SaveChangesAsync());
    }
}