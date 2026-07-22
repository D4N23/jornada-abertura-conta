using OnboardingService.Domain.Applications.ValueObjects;

namespace OnboardingService.Domain.Tests.Applications;

public sealed class CpfTests
{
    [Fact]
    public void From_ShouldNormalizeFormattedCpf()
    {
        var cpf = Cpf.From("529.982.247-25");

        Assert.Equal(
            "52998224725",
            cpf.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("123")]
    [InlineData("111.111.111-11")]
    [InlineData("123.456.789-00")]
    public void From_ShouldRejectInvalidCpf(string rawCpf)
    {
        Assert.Throws<ArgumentException>(
            () => Cpf.From(rawCpf)
        );
    }
}