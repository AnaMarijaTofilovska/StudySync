using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudySync.Migrations
{
    /// <inheritdoc />
    public partial class SeedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Homework" },
                    { 2, "Exams" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name" },
                values: new object[,]
                {
                    { 1, "alice@example.com", "Alice" },
                    { 2, "bob@example.com", "Bob" }
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "CategoryId", "Description", "DueDate", "Priority", "Title", "UserId", "isCompleted" },
                values: new object[,]
                {
                    { 1, 1, "Test", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "low", "Math Assignment", 1, false },
                    { 2, 2, "Test New", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "high", "Study for Physics", 2, false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
