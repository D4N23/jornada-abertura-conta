namespace OnboardingService.Domain.Applications.ValueObjects;

public readonly record struct CorrelationId(string Value)
{
    public static CorrelationId New()
    {
        return new CorrelationId(Guid.NewGuid().ToString());
    }

    public static CorrelationId From(string value)
    {
        if (String.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
              "Application ID cannot be empty.",
              nameof(value)  
            ); 
        }

        return new CorrelationId(value.Trim());
    }

    public override string ToString()
    {
        return Value.ToString();

    }

}