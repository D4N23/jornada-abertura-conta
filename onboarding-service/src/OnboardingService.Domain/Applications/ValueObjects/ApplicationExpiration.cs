namespace OnboardingService.Domain.Applications.ValueObjects;

public readonly record struct ApplicationExpiration(DateTimeOffset ExpiresAt)
{
    public static ApplicationExpiration InDays(
        DateTimeOffset createdAt,
        int days
    )
    {
        if (days <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(days),
                "Expiration days must be greater than zero."
            );
        }
        return new ApplicationExpiration(
            createdAt.AddDays(days)
        );
    }

    public bool IsExpiredAt(DateTimeOffset instant)
    {
        return instant >= ExpiresAt;
    }
}