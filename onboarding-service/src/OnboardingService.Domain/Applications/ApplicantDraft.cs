using OnboardingService.Domain.Applications.ValueObjects;

namespace OnboardingService.Domain.Applications;

public sealed record ApplicantDraft(
    FullName FullName,
    BirthDate BirthDate,
    EmailAddress Email,
    PhoneNumber Phone
);