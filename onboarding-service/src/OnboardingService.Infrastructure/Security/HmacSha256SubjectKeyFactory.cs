using System.Security.Cryptography;
using System.Text;
using OnboardingService.Application.Abstractions;
using OnboardingService.Domain.Applications.ValueObjects;

namespace OnboardingService.Infrastructure.Security;

public sealed class HmacSha256SubjectKeyFactory
    : ISubjectKeyFactory
{
    private readonly byte[] _secret;

    public HmacSha256SubjectKeyFactory(
        string secret
    )
    {
        if (string.IsNullOrWhiteSpace(secret))
        {
            throw new ArgumentException(
                "Subject key secret cannot be empty.",
                nameof(secret)
            );
        }

        _secret = Encoding.UTF8.GetBytes(secret);

        if (_secret.Length < 32)
        {
            throw new ArgumentException(
                "Subject key secret must contain at least 32 bytes.",
                nameof(secret)
            );
        }
    }

    public SubjectKey CreateFrom(
        Cpf cpf
    )
    {
        var cpfBytes = Encoding.UTF8.GetBytes(cpf.Value);

        var hash = HMACSHA256.HashData( key: _secret, source: cpfBytes);

        var base64Url = Convert
            .ToBase64String(hash)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');

        return SubjectKey.From(base64Url);
    }
}