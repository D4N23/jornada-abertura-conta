using ApplicationId = OnboardingService.Domain.Applications.ValueObjects.ApplicationId;

namespace OnboardingService.Domain.Applications.Exceptions;

public sealed class ApplicationExpiredException
    : InvalidOperationException
{
    public ApplicationId ApplicationId { get; }

    public DateTimeOffset ExpiresAt { get; }

    public ApplicationExpiredException(
        ApplicationId applicationId,
        DateTimeOffset expiresAt
    ) : base(
        $"Application '{applicationId}' expired at '{expiresAt:O}'."
    )
    {
        ApplicationId = applicationId;
        ExpiresAt = expiresAt;
    }
}