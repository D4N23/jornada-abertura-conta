namespace OnboardingService.Domain.Applications.ValueObjects;

public readonly record struct BirthDate
{
    public DateOnly Value {get;}

    private BirthDate(DateOnly value)
    {
        Value = value;
    }

    //valida uma nova entrada
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

    //reconstrói um dado persistido
    internal static BirthDate Rehydrate(
        DateOnly value
    )
    {
        return new BirthDate(value);
    }

    public override string ToString()
    {
      return Value.ToString("yyyy-MM-dd");
    }
}