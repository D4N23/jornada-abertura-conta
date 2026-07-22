namespace OnboardingService.Domain.Applications.ValueObjects;

public readonly record struct Cpf
{
    public string Value {get;}

    private Cpf(string value)
    {
        Value = value;
    }

    public static Cpf From(string rawValue)
    {
        if (string.IsNullOrWhiteSpace(rawValue))
        {
            throw new ArgumentException(
                "CPF cannot be empty.", 
                nameof(rawValue)
            );
        }

        var normalizedCpf = new string(
            rawValue.Where(char.IsDigit).ToArray()
        ); 

        if(!IsValid(normalizedCpf))
        {
            throw new ArgumentException(
                "CPF is invalid.",
                nameof(rawValue)
            );
        }

        return new Cpf(normalizedCpf);
    }

    private static bool IsValid(string cpf)
    {
        if (cpf.Length != 11)
        {
            return false;
        }

        if(cpf.All(character => character == cpf[0]))
        {
            return false;
        }

        var firstCheckDigit = CalcuulateCheckDigit(cpf, length: 9);

        if(cpf[9] - '0' != firstCheckDigit)
        {
            return false;
        }

        var secondCheckDigit = CalcuulateCheckDigit(
            cpf,
            length: 10
        );

        return cpf[10] - '0' == secondCheckDigit;
    }

    private static int CalcuulateCheckDigit(string cpf, int length)
    {
        var sum = 0;
        var weight = length + 1;

        for (var index = 0; index < length; index++)
        {
            var digit = cpf[index] - '0';

            sum += digit * weight;
            weight--;
        }

        var remainder = sum * 10 % 11;

        return remainder == 10 ? 0 : remainder;
    }

    public override string ToString()
    {
        return Value;
    }
}