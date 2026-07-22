using OnboardingService.Domain.Applications;
using OnboardingService.Domain.Applications.ValueObjects;

namespace OnboardingService.Application.Abstractions;

public interface IAccountOpeningApplicationRepository
{
    Task<AccountOpeningApplication?> FindActiveBySubjectKeyAsync(
        SubjectKey subjectKey,
        CancellationToken cancellationToken = default
    );

    Task AddAsync(
        AccountOpeningApplication application,
        CancellationToken cancellationToken = default
    );
}