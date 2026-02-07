using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "public");

        migrationBuilder.CreateTable(
            name: "categories",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_categories", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "users",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                email = table.Column<string>(type: "text", nullable: false),
                first_name = table.Column<string>(type: "text", nullable: false),
                last_name = table.Column<string>(type: "text", nullable: false),
                password_hash = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_users", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "tasks",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                user_id = table.Column<Guid>(type: "uuid", nullable: false),
                description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                category_id = table.Column<Guid>(type: "uuid", nullable: false),
                due_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                is_completed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                completed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_tasks", x => x.id);
                table.ForeignKey(
                    name: "fk_tasks_categories_category_id",
                    column: x => x.category_id,
                    principalSchema: "public",
                    principalTable: "categories",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "ix_categories_name",
            schema: "public",
            table: "categories",
            column: "name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_tasks_category_id",
            schema: "public",
            table: "tasks",
            column: "category_id");

        migrationBuilder.CreateIndex(
            name: "ix_tasks_user_id",
            schema: "public",
            table: "tasks",
            column: "user_id");

        migrationBuilder.CreateIndex(
            name: "ix_users_email",
            schema: "public",
            table: "users",
            column: "email",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "tasks",
            schema: "public");

        migrationBuilder.DropTable(
            name: "users",
            schema: "public");

        migrationBuilder.DropTable(
            name: "categories",
            schema: "public");
    }
}
