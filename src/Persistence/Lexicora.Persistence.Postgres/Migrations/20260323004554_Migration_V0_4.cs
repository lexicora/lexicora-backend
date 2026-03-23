using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lexicora.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_V0_4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<string>>(
                name: "tags",
                schema: "public",
                table: "topics",
                type: "varchar(50)[]",
                nullable: false,
                defaultValueSql: "'{}'",
                oldClrType: typeof(List<string>),
                oldType: "text[]",
                oldDefaultValueSql: "'{}'");

            migrationBuilder.AlterColumn<List<string>>(
                name: "tags",
                schema: "public",
                table: "entries",
                type: "varchar(50)[]",
                nullable: false,
                defaultValueSql: "'{}'",
                oldClrType: typeof(List<string>),
                oldType: "text[]",
                oldDefaultValueSql: "'{}'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<string>>(
                name: "tags",
                schema: "public",
                table: "topics",
                type: "text[]",
                nullable: false,
                defaultValueSql: "'{}'",
                oldClrType: typeof(List<string>),
                oldType: "varchar(50)[]",
                oldDefaultValueSql: "'{}'");

            migrationBuilder.AlterColumn<List<string>>(
                name: "tags",
                schema: "public",
                table: "entries",
                type: "text[]",
                nullable: false,
                defaultValueSql: "'{}'",
                oldClrType: typeof(List<string>),
                oldType: "varchar(50)[]",
                oldDefaultValueSql: "'{}'");
        }
    }
}
