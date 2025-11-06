using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace getstartedapp.Infrastructure.Data.Migrations.Sqlite
{
    /// <inheritdoc />
    public partial class AddPriorityDueNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1) Create a new table with the desired final schema
            migrationBuilder.CreateTable(
                name: "Todos_new",
                columns: table => new
                {
                    Id         = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title      = table.Column<string>(type: "TEXT",    nullable: false),
                    IsDone     = table.Column<bool>(type: "INTEGER",   nullable: false),

                    // New fields
                    Priority   = table.Column<int>(type: "INTEGER",    nullable: false, defaultValue: 1), // e.g., 0=Low,1=Med,2=High
                    DueDate    = table.Column<DateTime>(type: "TEXT",   nullable: true),
                    Notes      = table.Column<string>(type: "TEXT",     nullable: true),

                    // Use CURRENT_TIMESTAMP default on CREATE TABLE (allowed in SQLite)
                    CreatedUtc = table.Column<DateTime>(type: "TEXT",   nullable: false,
                        defaultValueSql: "CURRENT_TIMESTAMP"),

                    UpdatedUtc = table.Column<DateTime>(type: "TEXT",   nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos_new", x => x.Id);
                });

            // 2) Copy existing data. We omit the new columns so defaults/nulls apply.
            migrationBuilder.Sql(@"
            INSERT INTO ""Todos_new"" (Id, Title, IsDone)
            SELECT Id, Title, IsDone
            FROM ""Todos"";
            ");
            
            // 3) Drop the old table
            migrationBuilder.DropTable(name: "Todos");

            // 4) Rename the new table to the original name
            migrationBuilder.Sql(@"ALTER TABLE ""Todos_new"" RENAME TO ""Todos"";");
            
            /*
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedUtc",
                table: "Todos",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Todos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Todos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "Todos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedUtc",
                table: "Todos",
                type: "TEXT",
                nullable: true);
            */
            migrationBuilder.CreateIndex(
                name: "IX_Todos_DueDate",
                table: "Todos",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_IsDone",
                table: "Todos",
                column: "IsDone");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_Priority",
                table: "Todos",
                column: "Priority");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Todos_DueDate",
                table: "Todos");

            migrationBuilder.DropIndex(
                name: "IX_Todos_IsDone",
                table: "Todos");

            migrationBuilder.DropIndex(
                name: "IX_Todos_Priority",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "CreatedUtc",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "UpdatedUtc",
                table: "Todos");
        }
    }
}
