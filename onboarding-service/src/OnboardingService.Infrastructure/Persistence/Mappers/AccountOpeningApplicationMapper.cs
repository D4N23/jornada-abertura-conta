using OnboardingService.Domain.Applications;
using OnboardingService.Domain.Applications.ValueObjects;
using OnboardingService.Infrastructure.Persistence.Models;
using ApplicationId = OnboardingService.Domain.Applications.ValueObjects.ApplicationId;

namespace OnboardingService.Infrastructure.Persistence.Mappers;

internal static class AccountOpeningApplicationMapper
{
    public static AccountOpeningApplicationData ToData(
        AccountOpeningApplication application
    )
    {
        ArgumentNullException.ThrowIfNull(application);

        return new AccountOpeningApplicationData
        {
            Id = application.Id.Value,
            SubjectKey = application.SubjectKey.Value,
            ApplicantCpf = application.ApplicantCpf.Value,
            Status = application.Status.ToString(),
            CurrentStep = application.CurrentStep.ToString(),
            ExpiresAt = application.Expiration.ExpiresAt,
            ApplicantFullName = application.ApplicantDraft?.FullName.Value,
            ApplicantBirthDate = application.ApplicantDraft?.BirthDate.Value,
            ApplicantEmail = application.ApplicantDraft?.Email.Value,
            ApplicantPhone = application.ApplicantDraft?.Phone.Value,
            Version = application.Version,
            CreatedAt = application.CreatedAt,
            UpdatedAt = application.UpdatedAt
        };
    }

    public static AccountOpeningApplication ToDomain(
        AccountOpeningApplicationData data
    )
    {
        ArgumentNullException.ThrowIfNull(data);
        var applicantDraft = MapApplicantDraft(data);
        return AccountOpeningApplication.Rehydrate(
            id: ApplicationId.From(data.Id),
            subjectKey: SubjectKey.From(data.SubjectKey),
            applicantCpf: Cpf.From(data.ApplicantCpf),
            status: ParseApplicationStatus(data.Status),
            currentStep: ParseJourneyStep(data.CurrentStep),
            expiration: new ApplicationExpiration(data.ExpiresAt),
            applicantDraft: applicantDraft,
            version: data.Version,
            createdAt: data.CreatedAt,
            updatedAt: data.UpdatedAt
        );
    }

    private static ApplicantDraft? MapApplicantDraft(
        AccountOpeningApplicationData data
    )
    {
        var hasNoApplicantData =
            data.ApplicantFullName is null
            && data.ApplicantBirthDate is null
            && data.ApplicantEmail is null
            && data.ApplicantPhone is null;

        if (hasNoApplicantData)
        {
            return null;
        }

        var hasIncompleteApplicantData =
            data.ApplicantFullName is null
            || data.ApplicantBirthDate is null
            || data.ApplicantEmail is null
            || data.ApplicantPhone is null;

        if (hasIncompleteApplicantData)
        {
            throw new InvalidOperationException(
                $"Application '{data.Id}' has incomplete applicant data."
            );
        }

        return new ApplicantDraft(
            FullName: FullName.From(data.ApplicantFullName),
            BirthDate: BirthDate.Rehydrate(data.ApplicantBirthDate.Value),
            Email: EmailAddress.From(data.ApplicantEmail),
            Phone: PhoneNumber.From(data.ApplicantPhone)
        );
    }

    private static ApplicationStatus ParseApplicationStatus(
        string value
    )
    {
        if (
            Enum.TryParse<ApplicationStatus>(value, ignoreCase: false, out var status)
        )
        {
            return status;
        }

        throw new InvalidOperationException(
            $"Unknown application status '{value}'."
        );
    }

    private static JourneyStep ParseJourneyStep(
        string value
    )
    {
        if (
            Enum.TryParse<JourneyStep>(
                value,
                ignoreCase: false,
                out var step
            )
        )
        {
            return step;
        }

        throw new InvalidOperationException(
            $"Unknown journey step '{value}'."
        );
    }
}