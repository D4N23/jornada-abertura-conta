using Testcontainers.PostgreSql;

namespace OnboardingService.Infrastructure.Tests.Persistence;

public sealed class PostgreSqlFixture: IAsyncLifetime
{
    public PostgreSqlContainer Container { get; } =
        new PostgreSqlBuilder(
            "postgres:16-alpine"
        )
        .WithDatabase("onboarding_test")
        .WithUsername("onboarding")
        .WithPassword("onboarding")
        .Build();

    public Task InitializeAsync()
    {
        return Container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await Container.DisposeAsync();
    }

}