using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lab4ASP.Migrations
{
    /// <inheritdoc />
    public partial class addedReturnPropertiesLoanHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReturned",
                table: "LoanHistories",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnedDate",
                table: "LoanHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReturned",
                table: "LoanHistories");

            migrationBuilder.DropColumn(
                name: "ReturnedDate",
                table: "LoanHistories");
        }
    }
}
