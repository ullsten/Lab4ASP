using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lab4ASP.Migrations
{
    /// <inheritdoc />
    public partial class changedRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Users_FK_UserId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanHistories_Users_FK_UserId",
                table: "LoanHistories");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_FK_UserId",
                table: "Addresses");

            migrationBuilder.AlterColumn<string>(
                name: "FK_UserId",
                table: "LoanHistories",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UsersUserId",
                table: "LoanHistories",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FK_UserId",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsersId",
                table: "Addresses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsersUserId",
                table: "Addresses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoanHistories_UsersUserId",
                table: "LoanHistories",
                column: "UsersUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UsersId",
                table: "Addresses",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UsersUserId",
                table: "Addresses",
                column: "UsersUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_IdentityUser_UsersId",
                table: "Addresses",
                column: "UsersId",
                principalTable: "IdentityUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Users_UsersUserId",
                table: "Addresses",
                column: "UsersUserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanHistories_IdentityUser_FK_UserId",
                table: "LoanHistories",
                column: "FK_UserId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LoanHistories_Users_UsersUserId",
                table: "LoanHistories",
                column: "UsersUserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_IdentityUser_UsersId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Users_UsersUserId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanHistories_IdentityUser_FK_UserId",
                table: "LoanHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanHistories_Users_UsersUserId",
                table: "LoanHistories");

            migrationBuilder.DropIndex(
                name: "IX_LoanHistories_UsersUserId",
                table: "LoanHistories");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_UsersId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_UsersUserId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "UsersUserId",
                table: "LoanHistories");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "UsersUserId",
                table: "Addresses");

            migrationBuilder.AlterColumn<int>(
                name: "FK_UserId",
                table: "LoanHistories",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "FK_UserId",
                table: "Addresses",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_FK_UserId",
                table: "Addresses",
                column: "FK_UserId");

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
    }
}
