namespace OnboardingService.Domain.Applications.ValueObjects;

public readonly record struct ApplicationId(Guid Value)
{
    public static ApplicationId New()
    {
        return new ApplicationId(Guid.NewGuid());
    }

    public static ApplicationId From(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException(
              "Application ID cannot be empty.",
              nameof(value)  
            ); 
        }

        return new ApplicationId(value);
    }

    public override string ToString()
    {
        return Value.ToString();

    }

}