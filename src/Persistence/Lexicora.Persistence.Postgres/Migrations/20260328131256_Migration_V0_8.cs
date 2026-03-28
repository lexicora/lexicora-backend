using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lexicora.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_V0_8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_topics_slug_valid",
                schema: "public",
                table: "topics");

            migrationBuilder.DropCheckConstraint(
                name: "CK_entries_slug_valid",
                schema: "public",
                table: "entries");

            migrationBuilder.AddCheckConstraint(
                name: "CK_topics_slug_valid",
                schema: "public",
                table: "topics",
                sql: "slug ~* '^([a-zA-Z0-9]+(?:-[a-zA-Z0-9]+)*-[a-zA-Z0-9]{12}|[a-zA-Z0-9]{26})$' AND LENGTH(slug) <= 100");

            migrationBuilder.AddCheckConstraint(
                name: "CK_entries_slug_valid",
                schema: "public",
                table: "entries",
                sql: "slug ~* '^([a-zA-Z0-9]+(?:-[a-zA-Z0-9]+)*-[a-zA-Z0-9]{12}|[a-zA-Z0-9]{26})$' AND LENGTH(slug) <= 100");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_topics_slug_valid",
                schema: "public",
                table: "topics");

            migrationBuilder.DropCheckConstraint(
                name: "CK_entries_slug_valid",
                schema: "public",
                table: "entries");

            migrationBuilder.AddCheckConstraint(
                name: "CK_topics_slug_valid",
                schema: "public",
                table: "topics",
                sql: "slug ~* '^[a-zA-Z0-9]+(?:-[a-zA-Z0-9]+)*-[a-zA-Z0-9]{12}$' AND LENGTH(slug) <= 100");

            migrationBuilder.AddCheckConstraint(
                name: "CK_entries_slug_valid",
                schema: "public",
                table: "entries",
                sql: "slug ~* '^[a-zA-Z0-9]+(?:-[a-zA-Z0-9]+)*-[a-zA-Z0-9]{12}$' AND LENGTH(slug) <= 100");
        }
    }
}
