using Microsoft.EntityFrameworkCore;
using OnboardingService.Application.Abstractions;
using OnboardingService.Infrastructure.Persistence.Models;

namespace OnboardingService.Infrastructure.Persistence;

public sealed class OnboardingDbContext:DbContext,IUnitOfWork
{
    public OnboardingDbContext(
        DbContextOptions<OnboardingDbContext> options
    ) : base(options)
    {
    }
    
    internal DbSet<AccountOpeningApplicationData> Applications => Set<AccountOpeningApplicationData>();
    protected override void OnModelCreating(
        ModelBuilder modelBuilder
    )
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OnboardingDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    async Task IUnitOfWork.SaveChangesAsync(
        CancellationToken cancellationToken
    )
    {
        await SaveChangesAsync(cancellationToken);
    }
}