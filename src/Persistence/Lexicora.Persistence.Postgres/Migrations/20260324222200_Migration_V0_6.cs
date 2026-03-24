using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lexicora.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_V0_6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_User_Email_Valid",
                schema: "public",
                table: "users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_User_Username_Valid",
                schema: "public",
                table: "users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Tags_MaxCount",
                schema: "public",
                table: "topics");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Tags_MaxCount",
                schema: "public",
                table: "entries");

            migrationBuilder.AlterColumn<double>(
                name: "order",
                schema: "public",
                table: "blocks",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddCheckConstraint(
                name: "CK_users_email_valid",
                schema: "public",
                table: "users",
                sql: "email ~* '^[^@\\s]+@[^@\\s]+\\.[^@\\s]{2,}$' AND LENGTH(email) >= 6 AND LENGTH(email) <= 254");

            migrationBuilder.AddCheckConstraint(
                name: "CK_users_username_valid",
                schema: "public",
                table: "users",
                sql: "username ~* '^[a-zA-Z0-9](?:[a-zA-Z0-9]|[._](?![._]))*[a-zA-Z0-9]$' AND LENGTH(username) >= 3 AND LENGTH(username) <= 30");

            migrationBuilder.AddCheckConstraint(
                name: "CK_topics_slug_valid",
                schema: "public",
                table: "topics",
                sql: "slug ~* '^[a-zA-Z0-9]+(?:-[a-zA-Z0-9]+)*-[a-zA-Z0-9]{12}$' AND LENGTH(slug) <= 100");

            migrationBuilder.AddCheckConstraint(
                name: "CK_topics_tags_max_count",
                schema: "public",
                table: "topics",
                sql: "cardinality(tags) <= 10");

            migrationBuilder.AddCheckConstraint(
                name: "CK_entries_slug_valid",
                schema: "public",
                table: "entries",
                sql: "slug ~* '^[a-zA-Z0-9]+(?:-[a-zA-Z0-9]+)*-[a-zA-Z0-9]{12}$' AND LENGTH(slug) <= 100");

            migrationBuilder.AddCheckConstraint(
                name: "CK_entries_tags_max_count",
                schema: "public",
                table: "entries",
                sql: "cardinality(tags) <= 10");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_users_email_valid",
                schema: "public",
                table: "users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_users_username_valid",
                schema: "public",
                table: "users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_topics_slug_valid",
                schema: "public",
                table: "topics");

            migrationBuilder.DropCheckConstraint(
                name: "CK_topics_tags_max_count",
                schema: "public",
                table: "topics");

            migrationBuilder.DropCheckConstraint(
                name: "CK_entries_slug_valid",
                schema: "public",
                table: "entries");

            migrationBuilder.DropCheckConstraint(
                name: "CK_entries_tags_max_count",
                schema: "public",
                table: "entries");

            migrationBuilder.AlterColumn<double>(
                name: "order",
                schema: "public",
                table: "blocks",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldDefaultValue: 0.0);

            migrationBuilder.AddCheckConstraint(
                name: "CK_User_Email_Valid",
                schema: "public",
                table: "users",
                sql: "email ~* '^[^@\\s]+@[^@\\s]+\\.[^@\\s]{2,}$' AND LENGTH(email) >= 6");

            migrationBuilder.AddCheckConstraint(
                name: "CK_User_Username_Valid",
                schema: "public",
                table: "users",
                sql: "username ~* '^[a-zA-Z0-9](?:[a-zA-Z0-9]|[._](?![._]))*[a-zA-Z0-9]$' AND LENGTH(username) >= 3");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Tags_MaxCount",
                schema: "public",
                table: "topics",
                sql: "cardinality(tags) <= 10");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Tags_MaxCount",
                schema: "public",
                table: "entries",
                sql: "cardinality(tags) <= 10");
        }
    }
}
