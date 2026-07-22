namespace OnboardingService.Application.Applications.StartApplication;

public sealed record StartApplicationCommand(
    string Cpf,
    string CorrelationId
);