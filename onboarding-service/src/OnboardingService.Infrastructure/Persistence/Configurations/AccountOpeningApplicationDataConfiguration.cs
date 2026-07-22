using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnboardingService.Infrastructure.Persistence.Models;

namespace OnboardingService.Infrastructure.Persistence.Configurations;

internal sealed class
    AccountOpeningApplicationDataConfiguration
    : IEntityTypeConfiguration<AccountOpeningApplicationData>
{
    public void Configure(
        EntityTypeBuilder<AccountOpeningApplicationData> builder
    )
    {
        builder.ToTable("account_opening_application");

        builder.HasKey(application => application.Id);

        builder.Property(application => application.Id)
            .HasColumnName("application_id")
            .ValueGeneratedNever();

        builder.Property(application => application.SubjectKey)
            .HasColumnName("subject_key")
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(application => application.ApplicantCpf)
            .HasColumnName("applicant_cpf")
            .HasMaxLength(11)
            .IsRequired();

        builder.Property(application => application.Status)
            .HasColumnName("status")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(application => application.CurrentStep)
            .HasColumnName("current_step")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(application => application.ExpiresAt)
            .HasColumnName("expires_at")
            .IsRequired();

        builder.Property(application => application.ApplicantFullName)
            .HasColumnName("applicant_full_name")
            .HasMaxLength(150);

        builder.Property(application => application.ApplicantBirthDate)
            .HasColumnName("applicant_birth_date");

        builder.Property(application => application.ApplicantEmail)
            .HasColumnName("applicant_email")
            .HasMaxLength(254);

        builder.Property(application => application.ApplicantPhone)
            .HasColumnName("applicant_phone")
            .HasMaxLength(11);

        builder.Property(application => application.Version)
            .HasColumnName("version")
            .IsConcurrencyToken()
            .IsRequired();

        builder.Property(application => application.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(application => application.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasIndex(application => application.SubjectKey)
            .HasDatabaseName("ux_account_opening_application_active_subject")
            .IsUnique()
            .HasFilter(
                """
                "status" NOT IN (
                    'Completed',
                    'Rejected',
                    'Expired'
                )
                """);
    }
}