namespace OnboardingService.Domain.Applications.ValueObjects;

public readonly record struct BirthDate
{
    public DateOnly Value {get;}

    private BirthDate(DateOnly value)
    {
        Value = value;
    }
    public static BirthDate From(
        DateOnly value,
        DateOnly today
    )
    {
        if (value > today)
        {
            throw new ArgumentException(
                "brith date cannout be in the future.",
                nameof(value)
            );
        }
        return new BirthDate(value);
    }
    public override string ToString()
    {
      return Value.ToString("yyyy-MM-dd");
    }
}