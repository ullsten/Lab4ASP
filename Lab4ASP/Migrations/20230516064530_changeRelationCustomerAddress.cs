using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lab4ASP.Migrations
{
    /// <inheritdoc />
    public partial class changeRelationCustomerAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Addresses_AddressesAddressId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_AddressesAddressId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "AddressesAddressId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "FK_AddressId",
                table: "Customers");

            migrationBuilder.AddColumn<int>(
                name: "FK_CustomerId",
                table: "Addresses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_FK_CustomerId",
                table: "Addresses",
                column: "FK_CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Customers_FK_CustomerId",
                table: "Addresses",
                column: "FK_CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Customers_FK_CustomerId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_FK_CustomerId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "FK_CustomerId",
                table: "Addresses");

            migrationBuilder.AddColumn<int>(
                name: "AddressesAddressId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FK_AddressId",
                table: "Customers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AddressesAddressId",
                table: "Customers",
                column: "AddressesAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Addresses_AddressesAddressId",
                table: "Customers",
                column: "AddressesAddressId",
                principalTable: "Addresses",
                principalColumn: "AddressId");
        }
    }
}
