namespace OnboardingService.Domain.Applications;

public enum ApplicationStatus
{
    Started,
    ContactVerificationPending,
    PersonalDataPending,
    DocumentsPending,
    KycPending,
    FraudAnalysisPending,
    ManualReview,
    TermsAcceptancePending,
    Approved,
    CustomerProvisionung,
    AccountOpening,
    Completed,
    Rejected,
    Expired
}