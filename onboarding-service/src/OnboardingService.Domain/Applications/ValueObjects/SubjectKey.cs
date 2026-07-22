using System.Reflection.Metadata;

namespace OnboardingService.Domain.Applications.ValueObjects;

public readonly record struct SubjectKey(string Value)
{
    public static SubjectKey From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                "Subject key cannot be empty.",
                nameof(value)
            );
        }

        return new SubjectKey(value.Trim());
    }

    public override string ToString()
    {
        return Value;
    }
}