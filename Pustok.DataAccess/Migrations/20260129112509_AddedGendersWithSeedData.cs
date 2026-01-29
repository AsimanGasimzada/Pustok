using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pustok.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedGendersWithSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("491ce33b-1749-4964-b3e7-28ca163e4a6f"), "Male" },
                    { new Guid("5c08bb40-5462-4625-b9e0-47a9d4544990"), "Female" },
                    { new Guid("b92e8dda-8c53-4b56-810a-fb5b1061e18f"), "Mechanical" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Genders");
        }
    }
}
