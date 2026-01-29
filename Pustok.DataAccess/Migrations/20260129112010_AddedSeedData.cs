using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pustok.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("51e03370-dd1b-4b8d-aa74-fab5d463cf3d"), "Default Category" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("51e03370-dd1b-4b8d-aa74-fab5d463cf3d"));
        }
    }
}
