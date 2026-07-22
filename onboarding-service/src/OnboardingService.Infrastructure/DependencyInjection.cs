using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnboardingService.Application.Abstractions;
using OnboardingService.Infrastructure.Persistence;
using OnboardingService.Infrastructure.Persistence.Repositories;

namespace OnboardingService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString =
            configuration.GetConnectionString(
                "OnboardingDatabase"
            );

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'OnboardingDatabase' was not configured."
            );
        }

        services.AddDbContext<OnboardingDbContext>(
            options =>
            {
                options.UseNpgsql(connectionString);
            }
        );

        services.AddScoped<
            IAccountOpeningApplicationRepository,
            AccountOpeningApplicationRepository
        >();

        services.AddScoped<IUnitOfWork>(
            serviceProvider =>
                serviceProvider.GetRequiredService<
                    OnboardingDbContext
                >()
        );

        return services;
    }
}