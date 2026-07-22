using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnboardingService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account_opening_application",
                columns: table => new
                {
                    application_id = table.Column<Guid>(type: "uuid", nullable: false),
                    subject_key = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    applicant_cpf = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    current_step = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    expires_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    applicant_full_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    applicant_birth_date = table.Column<DateOnly>(type: "date", nullable: true),
                    applicant_email = table.Column<string>(type: "character varying(254)", maxLength: 254, nullable: true),
                    applicant_phone = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: true),
                    version = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_opening_application", x => x.application_id);
                });

            migrationBuilder.CreateIndex(
                name: "ux_account_opening_application_active_subject",
                table: "account_opening_application",
                column: "subject_key",
                unique: true,
                filter: "\"status\" NOT IN (\n    'Completed',\n    'Rejected',\n    'Expired'\n)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_opening_application");
        }
    }
}
