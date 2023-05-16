using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lab4ASP.Migrations
{
    /// <inheritdoc />
    public partial class changedNameCustomerToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Customers_FK_CustomerId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanHistories_Customers_FK_CustomerId",
                table: "LoanHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Users");

            migrationBuilder.RenameColumn(
                name: "FK_CustomerId",
                table: "LoanHistories",
                newName: "FK_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_LoanHistories_FK_CustomerId",
                table: "LoanHistories",
                newName: "IX_LoanHistories_FK_UserId");

            migrationBuilder.RenameColumn(
                name: "FK_CustomerId",
                table: "Addresses",
                newName: "FK_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_FK_CustomerId",
                table: "Addresses",
                newName: "IX_Addresses_FK_UserId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Users",
                newName: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Users_FK_UserId",
                table: "Addresses",
                column: "FK_UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanHistories_Users_FK_UserId",
                table: "LoanHistories",
                column: "FK_UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Users_FK_UserId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanHistories_Users_FK_UserId",
                table: "LoanHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Customers");

            migrationBuilder.RenameColumn(
                name: "FK_UserId",
                table: "LoanHistories",
                newName: "FK_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_LoanHistories_FK_UserId",
                table: "LoanHistories",
                newName: "IX_LoanHistories_FK_CustomerId");

            migrationBuilder.RenameColumn(
                name: "FK_UserId",
                table: "Addresses",
                newName: "FK_CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_FK_UserId",
                table: "Addresses",
                newName: "IX_Addresses_FK_CustomerId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Customers",
                newName: "CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Customers_FK_CustomerId",
                table: "Addresses",
                column: "FK_CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanHistories_Customers_FK_CustomerId",
                table: "LoanHistories",
                column: "FK_CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
