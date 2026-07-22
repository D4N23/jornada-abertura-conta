namespace OnboardingService.Infrastructure.Persistence.Models;

internal sealed class AccountOpeningApplicationData
{
    public Guid Id { get; set; }

    public string SubjectKey { get; set; } = string.Empty;

    public string ApplicantCpf { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string CurrentStep { get; set; } = string.Empty;

    public DateTimeOffset ExpiresAt { get; set; }

    public string? ApplicantFullName { get; set; }

    public DateOnly? ApplicantBirthDate { get; set; }

    public string? ApplicantEmail { get; set; }

    public string? ApplicantPhone { get; set; }

    public long Version { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}