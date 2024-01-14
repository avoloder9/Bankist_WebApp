using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class m1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CardType",
                columns: table => new
                {
                    CardTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    fees = table.Column<float>(type: "real", nullable: false),
                    maxLimit = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardType", x => x.CardTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    currencyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    currencyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    currencyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    exchangeRate = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.currencyId);
                });

            migrationBuilder.CreateTable(
                name: "LoanType",
                columns: table => new
                {
                    loanTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    maxLoanAmount = table.Column<float>(type: "real", nullable: false),
                    repaymentTerm = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanType", x => x.loanTypeId);
                });

            migrationBuilder.CreateTable(
                name: "AutentificationToken",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    accountId = table.Column<int>(type: "int", nullable: false),
                    autentificationTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ipAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutentificationToken", x => x.id);
                    table.ForeignKey(
                        name: "FK_AutentificationToken_Account_accountId",
                        column: x => x.accountId,
                        principalTable: "Account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    totalCapital = table.Column<float>(type: "real", nullable: false),
                    numberOfUsers = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.id);
                    table.ForeignKey(
                        name: "FK_Bank_Account_id",
                        column: x => x.id,
                        principalTable: "Account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    firstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    birthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    registrationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                    table.ForeignKey(
                        name: "FK_User_Account_id",
                        column: x => x.id,
                        principalTable: "Account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Card",
                columns: table => new
                {
                    cardNumber = table.Column<int>(type: "int", nullable: false),
                    expirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    issueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    amount = table.Column<float>(type: "real", nullable: false),
                    cardTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    currencyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Card", x => x.cardNumber);
                    table.ForeignKey(
                        name: "FK_Card_CardType_cardTypeId",
                        column: x => x.cardTypeId,
                        principalTable: "CardType",
                        principalColumn: "CardTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Card_Currency_currencyId",
                        column: x => x.currencyId,
                        principalTable: "Currency",
                        principalColumn: "currencyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeletedUser",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    deletionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeletedUser", x => x.id);
                    table.ForeignKey(
                        name: "FK_DeletedUser_User_id",
                        column: x => x.id,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BanksUsersCards",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bankId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    cardId = table.Column<int>(type: "int", nullable: false),
                    accountIssueDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BanksUsersCards", x => x.id);
                    table.ForeignKey(
                        name: "FK_BanksUsersCards_Bank_bankId",
                        column: x => x.bankId,
                        principalTable: "Bank",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BanksUsersCards_Card_cardId",
                        column: x => x.cardId,
                        principalTable: "Card",
                        principalColumn: "cardNumber",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BanksUsersCards_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "id",
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
                    loanTypeId = table.Column<int>(type: "int", nullable: false),
                    cardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loan", x => x.loanId);
                    table.ForeignKey(
                        name: "FK_Loan_Card_cardId",
                        column: x => x.cardId,
                        principalTable: "Card",
                        principalColumn: "cardNumber",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loan_LoanType_loanTypeId",
                        column: x => x.loanTypeId,
                        principalTable: "LoanType",
                        principalColumn: "loanTypeId",
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
                    senderCardId = table.Column<int>(type: "int", nullable: false),
                    recieverCardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.transactionId);
                    table.ForeignKey(
                        name: "FK_Transaction_Card_recieverCardId",
                        column: x => x.recieverCardId,
                        principalTable: "Card",
                        principalColumn: "cardNumber",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_Card_senderCardId",
                        column: x => x.senderCardId,
                        principalTable: "Card",
                        principalColumn: "cardNumber",
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
                    bankUserCardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loyalty", x => x.loyaltyId);
                    table.ForeignKey(
                        name: "FK_Loyalty_BanksUsersCards_bankUserCardId",
                        column: x => x.bankUserCardId,
                        principalTable: "BanksUsersCards",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutentificationToken_accountId",
                table: "AutentificationToken",
                column: "accountId");

            migrationBuilder.CreateIndex(
                name: "IX_BanksUsersCards_bankId",
                table: "BanksUsersCards",
                column: "bankId");

            migrationBuilder.CreateIndex(
                name: "IX_BanksUsersCards_cardId",
                table: "BanksUsersCards",
                column: "cardId");

            migrationBuilder.CreateIndex(
                name: "IX_BanksUsersCards_userId",
                table: "BanksUsersCards",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Card_cardTypeId",
                table: "Card",
                column: "cardTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Card_currencyId",
                table: "Card",
                column: "currencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Loan_cardId",
                table: "Loan",
                column: "cardId");

            migrationBuilder.CreateIndex(
                name: "IX_Loan_loanTypeId",
                table: "Loan",
                column: "loanTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Loyalty_bankUserCardId",
                table: "Loyalty",
                column: "bankUserCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_recieverCardId",
                table: "Transaction",
                column: "recieverCardId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_senderCardId",
                table: "Transaction",
                column: "senderCardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutentificationToken");

            migrationBuilder.DropTable(
                name: "DeletedUser");

            migrationBuilder.DropTable(
                name: "Loan");

            migrationBuilder.DropTable(
                name: "Loyalty");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "LoanType");

            migrationBuilder.DropTable(
                name: "BanksUsersCards");

            migrationBuilder.DropTable(
                name: "Bank");

            migrationBuilder.DropTable(
                name: "Card");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "CardType");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
