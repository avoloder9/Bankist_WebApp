using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class migration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutentificationToken_Bank_bankId",
                table: "AutentificationToken");

            migrationBuilder.DropForeignKey(
                name: "FK_AutentificationToken_User_userId",
                table: "AutentificationToken");

            migrationBuilder.DropIndex(
                name: "IX_AutentificationToken_bankId",
                table: "AutentificationToken");

            migrationBuilder.DropColumn(
                name: "bankId",
                table: "AutentificationToken");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "AutentificationToken",
                newName: "accountId");

            migrationBuilder.RenameIndex(
                name: "IX_AutentificationToken_userId",
                table: "AutentificationToken",
                newName: "IX_AutentificationToken_accountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AutentificationToken_Account_accountId",
                table: "AutentificationToken",
                column: "accountId",
                principalTable: "Account",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutentificationToken_Account_accountId",
                table: "AutentificationToken");

            migrationBuilder.RenameColumn(
                name: "accountId",
                table: "AutentificationToken",
                newName: "userId");

            migrationBuilder.RenameIndex(
                name: "IX_AutentificationToken_accountId",
                table: "AutentificationToken",
                newName: "IX_AutentificationToken_userId");

            migrationBuilder.AddColumn<int>(
                name: "bankId",
                table: "AutentificationToken",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AutentificationToken_bankId",
                table: "AutentificationToken",
                column: "bankId");

            migrationBuilder.AddForeignKey(
                name: "FK_AutentificationToken_Bank_bankId",
                table: "AutentificationToken",
                column: "bankId",
                principalTable: "Bank",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AutentificationToken_User_userId",
                table: "AutentificationToken",
                column: "userId",
                principalTable: "User",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
