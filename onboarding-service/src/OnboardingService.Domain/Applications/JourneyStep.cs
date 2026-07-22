namespace OnboardingService.Domain.Applications;

public enum JourneyStep
{
    Introduction,
    PersonalData,
    ContactVerification,
    Documents,
    KycAnalysis,
    FraudAnalysis,
    ManualReview,
    TermsAcceptance,
    CustomerProvisioning,
    AccountOpening,
    Completed
}