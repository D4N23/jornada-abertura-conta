namespace OnboardingService.Domain.Applications.ValueObjects;

public readonly record struct PhoneNumber
{
    public string Value {get;}

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static PhoneNumber From(string value)
    {
        if(string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Phone number cannot be empty.", nameof(value));
        }
        var normalizedPhone = new string(value.Where(char.IsDigit).ToArray());
        if(normalizedPhone.Length is not 10 && normalizedPhone.Length is not 11)
        {
            throw new ArgumentException("Phone number must contain 10 or 11 digits.", nameof(value));
        }
        return new PhoneNumber(normalizedPhone);
    }

    public override string ToString()
    {
        return Value;
    }
}