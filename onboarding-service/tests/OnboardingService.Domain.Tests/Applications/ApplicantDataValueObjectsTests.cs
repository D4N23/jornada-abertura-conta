using OnboardingService.Domain.Applications.ValueObjects;

namespace OnboardingService.Domain.Tests.Applications;

public sealed class ApplicantDataValueObjectsTests
{
    [Fact]
    public void FullName_ShouldNormalizeWhitespace()
    {
        var fullName = FullName.From(
            "  Danilo   Sampaio  "
        );

        Assert.Equal(
            "Danilo Sampaio",
            fullName.Value
        );
    }

    [Fact]
    public void EmailAddress_ShouldNormalizeValue()
    {
        var email = EmailAddress.From(
            " Danilo@Example.com "
        );

        Assert.Equal(
            "danilo@example.com",
            email.Value
        );
    }

    [Fact]
    public void PhoneNumber_ShouldKeepOnlyDigits()
    {
        var phone = PhoneNumber.From(
            "(11) 99999-9999"
        );

        Assert.Equal(
            "11999999999",
            phone.Value
        );
    }

    [Fact]
    public void BirthDate_ShouldRejectFutureDate()
    {
        var today = new DateOnly(
            year: 2026,
            month: 7,
            day: 22
        );

        Assert.Throws<ArgumentException>(
            () => BirthDate.From(
                value: today.AddDays(1),
                today: today
            )
        );
    }
}