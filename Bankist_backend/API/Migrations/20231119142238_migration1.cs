using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class migration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    bankId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bankName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    totalCapital = table.Column<float>(type: "real", nullable: false),
                    numberOfUsers = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.bankId);
                });

            migrationBuilder.CreateTable(
                name: "DeletedUser",
                columns: table => new
                {
                    deletedUserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    deletionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedUser", x => x.deletedUserId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    firstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    birthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    registrationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "BankUser",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bankId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    accountIssueDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankUser", x => x.id);
                    table.ForeignKey(
                        name: "FK_BankUser_Bank_bankId",
                        column: x => x.bankId,
                        principalTable: "Bank",
                        principalColumn: "bankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BankUser_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Card",
                columns: table => new
                {
                    cardNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    expirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    issueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    amount = table.Column<float>(type: "real", nullable: false),
                    bankId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Card", x => x.cardNumber);
                    table.ForeignKey(
                        name: "FK_Card_Bank_bankId",
                        column: x => x.bankId,
                        principalTable: "Bank",
                        principalColumn: "bankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Card_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Loan",
                columns: table => new
                {
                    loanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    amount = table.Column<float>(type: "real", nullable: false),
                    interest = table.Column<float>(type: "real", nullable: false),
                    rate = table.Column<float>(type: "real", nullable: false),
                    issueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    rateCount = table.Column<int>(type: "int", nullable: false),
                    totalAmount = table.Column<float>(type: "real", nullable: false),
                    ratesPayed = table.Column<int>(type: "int", nullable: false),
                    totalAmountPayed = table.Column<float>(type: "real", nullable: false),
                    remainingAmount = table.Column<float>(type: "real", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bankId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loan", x => x.loanId);
                    table.ForeignKey(
                        name: "FK_Loan_Bank_bankId",
                        column: x => x.bankId,
                        principalTable: "Bank",
                        principalColumn: "bankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loan_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Loyalty",
                columns: table => new
                {
                    loyaltyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    totalPoints = table.Column<int>(type: "int", nullable: false),
                    pointToPromotion = table.Column<int>(type: "int", nullable: false),
                    bankId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loyalty", x => x.loyaltyId);
                    table.ForeignKey(
                        name: "FK_Loyalty_Bank_bankId",
                        column: x => x.bankId,
                        principalTable: "Bank",
                        principalColumn: "bankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loyalty_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    transactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    transactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    amount = table.Column<float>(type: "real", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bankId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.transactionId);
                    table.ForeignKey(
                        name: "FK_Transaction_Bank_bankId",
                        column: x => x.bankId,
                        principalTable: "Bank",
                        principalColumn: "bankId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankUser_bankId",
                table: "BankUser",
                column: "bankId");

            migrationBuilder.CreateIndex(
                name: "IX_BankUser_userId",
                table: "BankUser",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Card_bankId",
                table: "Card",
                column: "bankId");

            migrationBuilder.CreateIndex(
                name: "IX_Card_userId",
                table: "Card",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Loan_bankId",
                table: "Loan",
                column: "bankId");

            migrationBuilder.CreateIndex(
                name: "IX_Loan_userId",
                table: "Loan",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Loyalty_bankId",
                table: "Loyalty",
                column: "bankId");

            migrationBuilder.CreateIndex(
                name: "IX_Loyalty_userId",
                table: "Loyalty",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_bankId",
                table: "Transaction",
                column: "bankId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_userId",
                table: "Transaction",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankUser");

            migrationBuilder.DropTable(
                name: "Card");

            migrationBuilder.DropTable(
                name: "DeletedUser");

            migrationBuilder.DropTable(
                name: "Loan");

            migrationBuilder.DropTable(
                name: "Loyalty");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Bank");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
