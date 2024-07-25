using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ApiCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialFirst : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salary = table.Column<double>(type: "float", nullable: false),
                    UAN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CreatedDate", "Name", "Salary", "UAN" },
                values: new object[,]
                {
                    { 1, new DateTime(2020, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Awais Ansari", 24000.0, "RAWKZY8446" },
                    { 2, new DateTime(2029, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sadain Ansari", 24000.0, "QWERTY9890" },
                    { 3, new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Awais Ansari", 24000.0, "ZXCVBN0987" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
