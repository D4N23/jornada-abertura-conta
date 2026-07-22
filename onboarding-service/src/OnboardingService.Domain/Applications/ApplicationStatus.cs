namespace OnboardingService.Domain.Applications;

public enum ApplicationStatus
{
    Started,
    ContractVerificationPending,
    PersonalDatatPeding,
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