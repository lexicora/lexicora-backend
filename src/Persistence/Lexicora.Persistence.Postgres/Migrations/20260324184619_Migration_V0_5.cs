using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lexicora.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_V0_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.AlterColumn<string>(
                name: "username",
                schema: "public",
                table: "users",
                type: "citext",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                schema: "public",
                table: "users",
                type: "citext",
                maxLength: 254,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(254)",
                oldMaxLength: 254);

            migrationBuilder.AlterColumn<string>(
                name: "display_name",
                schema: "public",
                table: "users",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "slug",
                schema: "public",
                table: "topics",
                type: "citext",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "slug",
                schema: "public",
                table: "entries",
                type: "citext",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_User_Email_Valid",
                schema: "public",
                table: "users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_User_Username_Valid",
                schema: "public",
                table: "users");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.AlterColumn<string>(
                name: "username",
                schema: "public",
                table: "users",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "citext",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                schema: "public",
                table: "users",
                type: "character varying(254)",
                maxLength: 254,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "citext",
                oldMaxLength: 254);

            migrationBuilder.AlterColumn<string>(
                name: "display_name",
                schema: "public",
                table: "users",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "slug",
                schema: "public",
                table: "topics",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "citext",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "slug",
                schema: "public",
                table: "entries",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "citext",
                oldMaxLength: 100);
        }
    }
}
