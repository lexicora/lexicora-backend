using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lexicora.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Migration_V0_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "users",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    display_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    entry_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    topic_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                },
                comment: "Represents a user of the application. Each user has a unique email and username, as well as personal information like first name, last name, and display name. The EntryCount property keeps track of how many entries the user has saved across all topics, which can be useful for displaying user summaries without needing to load all entries. Auth stuff handled by the supabase auth user schema.");

            migrationBuilder.CreateTable(
                name: "topics",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    is_favorite = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    tags = table.Column<List<string>>(type: "text[]", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    entry_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_topics", x => x.id);
                    table.ForeignKey(
                        name: "FK_topics_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Topics are user-defined collections of entries. They allow users to organize their saved web pages into meaningful groups. Each topic has a name, description, and tags for categorization. The EntryCount property keeps track of how many entries are in the topic, which can be useful for displaying topic summaries without needing to load all entries.");

            migrationBuilder.CreateTable(
                name: "entries",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    topic_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    tags = table.Column<List<string>>(type: "text[]", nullable: false),
                    is_favorite = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    favicon_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    origin_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    path_url = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    language_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    site_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    excerpt = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    byline = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    published_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_entries", x => x.id);
                    table.ForeignKey(
                        name: "FK_entries_topics_topic_id",
                        column: x => x.topic_id,
                        principalSchema: "public",
                        principalTable: "topics",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_entries_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Represents a saved web page or article with metadata and content blocks.");

            migrationBuilder.CreateTable(
                name: "blocks",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    entry_id = table.Column<Guid>(type: "uuid", nullable: false),
                    parent_block_id = table.Column<Guid>(type: "uuid", nullable: true),
                    order = table.Column<double>(type: "double precision", nullable: false),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    props_json = table.Column<JsonElement>(type: "jsonb", nullable: false, defaultValueSql: "'{}'"),
                    content_json = table.Column<JsonElement>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blocks", x => x.id);
                    table.ForeignKey(
                        name: "FK_blocks_blocks_parent_block_id",
                        column: x => x.parent_block_id,
                        principalSchema: "public",
                        principalTable: "blocks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_blocks_entries_entry_id",
                        column: x => x.entry_id,
                        principalSchema: "public",
                        principalTable: "entries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Blocks represent the content of an entry. They can be nested to create complex structures. Each block has a type that determines how it should be rendered and what properties it has. The Props column is a JSONB column that stores the properties of the block, which can vary depending on the block type. The ContentJson column is used for blocks that have content that can be represented as JSON, such as tables or code blocks.");

            migrationBuilder.CreateIndex(
                name: "IX_blocks_entry_id_order",
                schema: "public",
                table: "blocks",
                columns: new[] { "entry_id", "order" });

            migrationBuilder.CreateIndex(
                name: "IX_blocks_parent_block_id",
                schema: "public",
                table: "blocks",
                column: "parent_block_id");

            migrationBuilder.CreateIndex(
                name: "IX_entries_topic_id",
                schema: "public",
                table: "entries",
                column: "topic_id");

            migrationBuilder.CreateIndex(
                name: "IX_entries_user_id",
                schema: "public",
                table: "entries",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_topics_user_id",
                schema: "public",
                table: "topics",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                schema: "public",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_username",
                schema: "public",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "blocks",
                schema: "public");

            migrationBuilder.DropTable(
                name: "entries",
                schema: "public");

            migrationBuilder.DropTable(
                name: "topics",
                schema: "public");

            migrationBuilder.DropTable(
                name: "users",
                schema: "public");
        }
    }
}
