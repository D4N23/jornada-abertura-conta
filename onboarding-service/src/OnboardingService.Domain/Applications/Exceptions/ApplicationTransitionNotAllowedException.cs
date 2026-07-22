namespace OnboardingService.Domain.Applications.Exceptions;

public sealed class ApplicationTransitionNotAllowedException : InvalidOperationException
{
    public ApplicationStatus CurrentStatus {get;}

    public JourneyStep CurrentStep {get;}

    public string Operation {get;}

    public ApplicationTransitionNotAllowedException(
        string operation,
        ApplicationStatus currentStatus,
        JourneyStep currentStep
    ) : base (
        $"Operation '{operation}' is not allowed when " +
        $"application status is '{currentStatus}' " +
        $"and step is '{currentStep}'."
    )
    {
     Operation = operation;
     CurrentStatus = currentStatus;
     CurrentStep = currentStep;   
    }
}