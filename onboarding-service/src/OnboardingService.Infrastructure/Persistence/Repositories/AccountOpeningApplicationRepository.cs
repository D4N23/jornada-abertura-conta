using Microsoft.EntityFrameworkCore;
using OnboardingService.Application.Abstractions;
using OnboardingService.Domain.Applications;
using OnboardingService.Domain.Applications.ValueObjects;
using OnboardingService.Infrastructure.Persistence.Mappers;

namespace OnboardingService.Infrastructure.Persistence.Repositories;

public sealed class AccountOpeningApplicationRepository : IAccountOpeningApplicationRepository
{
    private static readonly string[] TerminalStatuses = [
        ApplicationStatus.Completed.ToString(),
        ApplicationStatus.Rejected.ToString(),
        ApplicationStatus.Expired.ToString()
    ];

    private readonly OnboardingDbContext _dbContext;

    public AccountOpeningApplicationRepository(
        OnboardingDbContext dbContext
    )
    {
        _dbContext = dbContext;
    }

    public async Task<AccountOpeningApplication?> FindActiveBySubjectKeyAsync(
            SubjectKey subjectKey,
            CancellationToken cancellationToken = default
        )
    {
        var data = await _dbContext.Applications
            .AsNoTracking()
            .SingleOrDefaultAsync(
                application =>
                    application.SubjectKey == subjectKey.Value
                    && !TerminalStatuses.Contains(
                        application.Status),cancellationToken);

        return data is null ? null : AccountOpeningApplicationMapper.ToDomain(data);
    }

    public async Task AddAsync(
        AccountOpeningApplication application,
        CancellationToken cancellationToken = default
    )
    {
        var data =AccountOpeningApplicationMapper.ToData(application);
        await _dbContext.Applications.AddAsync(data,cancellationToken);
    }
}