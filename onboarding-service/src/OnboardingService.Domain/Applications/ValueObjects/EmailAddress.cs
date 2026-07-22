using System.Net.Mail;

namespace OnboardingService.Domain.Applications.ValueObjects;

public readonly  record struct EmailAddress
{
    public string Value {get;}

    private EmailAddress(string value)
    {
        Value = value;
    }

    public static EmailAddress From(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email address cannout be empty.", nameof(value));
        }

        var normalizedEmail = value.Trim().ToLowerInvariant();

        if (
            !MailAddress.TryCreate(
             normalizedEmail, 
             out var parsedAddress
            ) || !string.Equals(
            parsedAddress.Address, 
            normalizedEmail, 
            StringComparison.OrdinalIgnoreCase)
           )
        {
            throw new ArgumentException("Email address is invalid.", nameof(value));
        }
        return new EmailAddress(normalizedEmail);
    }

    public override string ToString()
    {
        return Value;
    }
}