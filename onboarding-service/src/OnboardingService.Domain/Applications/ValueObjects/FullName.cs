namespace OnboardingService.Domain.Applications.ValueObjects;

public readonly record struct FullName
{
    public string Value {get;}

    private FullName(string value)
    {
        Value = value;
    }

    public static FullName From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                "Full name cannout be empty.",
                nameof(value)
            );
        }
        var normalizedName = string.Join(
            ' ',
            value.Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries
            )
        );
        if (normalizedName.Length < 3)
        {
            throw new ArgumentException(
                "Full name cannout be empty.",
                nameof(value)
            );
        }
        if (normalizedName.Length > 150)
        {
            throw new ArgumentException(
                "Full name cannout be empty.",
                nameof(value)
            );
        }
        return new FullName(normalizedName);
    }

    public override string ToString()
    {
        return Value;
    }
}