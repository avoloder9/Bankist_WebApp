﻿// <auto-generated />
using System;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("API.Data.Models.Account", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<bool>("Is2FActive")
                        .HasColumnType("bit");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Account");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("API.Data.Models.AutentificationToken", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<bool>("Is2FAUnlocked")
                        .HasColumnType("bit");

                    b.Property<string>("TwoFKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("accountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("autentificationTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("ipAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("accountId");

                    b.ToTable("AutentificationToken");
                });

            modelBuilder.Entity("API.Data.Models.BanksUsersCards", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<DateTime>("accountIssueDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("bankId")
                        .HasColumnType("int");

                    b.Property<int>("cardId")
                        .HasColumnType("int");

                    b.Property<bool>("isBlock")
                        .HasColumnType("bit");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("bankId");

                    b.HasIndex("cardId");

                    b.HasIndex("userId");

                    b.ToTable("BanksUsersCards");
                });

            modelBuilder.Entity("API.Data.Models.Card", b =>
                {
                    b.Property<int>("cardNumber")
                        .HasColumnType("int");

                    b.Property<float>("amount")
                        .HasColumnType("real");

                    b.Property<float>("atmLimit")
                        .HasColumnType("real");

                    b.Property<string>("cardTypeId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("currencyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("expirationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("issueDate")
                        .HasColumnType("datetime2");

                    b.Property<float>("negativeLimit")
                        .HasColumnType("real");

                    b.Property<int>("pin")
                        .HasColumnType("int");

                    b.Property<float>("transactionLimit")
                        .HasColumnType("real");

                    b.HasKey("cardNumber");

                    b.HasIndex("cardTypeId");

                    b.HasIndex("currencyId");

                    b.ToTable("Card");
                });

            modelBuilder.Entity("API.Data.Models.CardType", b =>
                {
                    b.Property<string>("CardTypeId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<float>("fees")
                        .HasColumnType("real");

                    b.Property<float>("maxLimit")
                        .HasColumnType("real");

                    b.HasKey("CardTypeId");

                    b.ToTable("CardType");
                });

            modelBuilder.Entity("API.Data.Models.Currency", b =>
                {
                    b.Property<int>("currencyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("currencyId"));

                    b.Property<string>("currencyCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("currencyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("exchangeRate")
                        .HasColumnType("real");

                    b.Property<string>("symbol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("currencyId");

                    b.ToTable("Currency");
                });

            modelBuilder.Entity("API.Data.Models.Loan", b =>
                {
                    b.Property<int>("loanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("loanId"));

                    b.Property<float>("amount")
                        .HasColumnType("real");

                    b.Property<int>("cardId")
                        .HasColumnType("int");

                    b.Property<DateTime>("dueDate")
                        .HasColumnType("datetime2");

                    b.Property<float>("interest")
                        .HasColumnType("real");

                    b.Property<DateTime>("issueDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("loanTypeId")
                        .HasColumnType("int");

                    b.Property<float>("rate")
                        .HasColumnType("real");

                    b.Property<int>("rateCount")
                        .HasColumnType("int");

                    b.Property<int>("ratesPayed")
                        .HasColumnType("int");

                    b.Property<float>("remainingAmount")
                        .HasColumnType("real");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("totalAmount")
                        .HasColumnType("real");

                    b.Property<float>("totalAmountPayed")
                        .HasColumnType("real");

                    b.HasKey("loanId");

                    b.HasIndex("cardId");

                    b.HasIndex("loanTypeId");

                    b.ToTable("Loan");
                });

            modelBuilder.Entity("API.Data.Models.LoanType", b =>
                {
                    b.Property<int>("loanTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("loanTypeId"));

                    b.Property<float>("maxLoanAmount")
                        .HasColumnType("real");

                    b.Property<int>("maximumRepaymentMonths")
                        .HasColumnType("int");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("loanTypeId");

                    b.ToTable("LoanType");
                });

            modelBuilder.Entity("API.Data.Models.Loyalty", b =>
                {
                    b.Property<int>("loyaltyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("loyaltyId"));

                    b.Property<int>("bankUserCardId")
                        .HasColumnType("int");

                    b.Property<int>("pointToPromotion")
                        .HasColumnType("int");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("totalPoints")
                        .HasColumnType("int");

                    b.HasKey("loyaltyId");

                    b.HasIndex("bankUserCardId");

                    b.ToTable("Loyalty");
                });

            modelBuilder.Entity("API.Data.Models.SystemLogs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ExceptionMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IpAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsException")
                        .HasColumnType("bit");

                    b.Property<string>("PostData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QueryPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("SystemLogs");
                });

            modelBuilder.Entity("API.Data.Models.Transaction", b =>
                {
                    b.Property<int>("transactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("transactionId"));

                    b.Property<float>("amount")
                        .HasColumnType("real");

                    b.Property<int>("recieverCardId")
                        .HasColumnType("int");

                    b.Property<int?>("senderCardId")
                        .HasColumnType("int");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("transactionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("transactionId");

                    b.HasIndex("recieverCardId");

                    b.HasIndex("senderCardId");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("API.Data.Models.UserActivity", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("accountStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("awardsReceived")
                        .HasColumnType("int");

                    b.Property<int>("transactionsCount")
                        .HasColumnType("int");

                    b.Property<int>("userId")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("userId");

                    b.ToTable("UserActivity");
                });

            modelBuilder.Entity("API.Data.Models.Bank", b =>
                {
                    b.HasBaseType("API.Data.Models.Account");

                    b.Property<int>("numberOfUsers")
                        .HasColumnType("int");

                    b.Property<float>("totalCapital")
                        .HasColumnType("real");

                    b.ToTable("Bank");
                });

            modelBuilder.Entity("API.Data.Models.User", b =>
                {
                    b.HasBaseType("API.Data.Models.Account");

                    b.Property<DateTime>("birthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("firstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("lastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("registrationDate")
                        .HasColumnType("datetime2");

                    b.ToTable("User");
                });

            modelBuilder.Entity("API.Data.Models.DeletedUser", b =>
                {
                    b.HasBaseType("API.Data.Models.User");

                    b.Property<DateTime>("deletionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("DeletedUser");
                });

            modelBuilder.Entity("API.Data.Models.AutentificationToken", b =>
                {
                    b.HasOne("API.Data.Models.Account", "account")
                        .WithMany()
                        .HasForeignKey("accountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("account");
                });

            modelBuilder.Entity("API.Data.Models.BanksUsersCards", b =>
                {
                    b.HasOne("API.Data.Models.Bank", "bank")
                        .WithMany()
                        .HasForeignKey("bankId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Data.Models.Card", "card")
                        .WithMany()
                        .HasForeignKey("cardId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Data.Models.User", "user")
                        .WithMany()
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("bank");

                    b.Navigation("card");

                    b.Navigation("user");
                });

            modelBuilder.Entity("API.Data.Models.Card", b =>
                {
                    b.HasOne("API.Data.Models.CardType", "cardType")
                        .WithMany()
                        .HasForeignKey("cardTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Data.Models.Currency", "currency")
                        .WithMany()
                        .HasForeignKey("currencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("cardType");

                    b.Navigation("currency");
                });

            modelBuilder.Entity("API.Data.Models.Loan", b =>
                {
                    b.HasOne("API.Data.Models.Card", "card")
                        .WithMany()
                        .HasForeignKey("cardId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Data.Models.LoanType", "loanType")
                        .WithMany()
                        .HasForeignKey("loanTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("card");

                    b.Navigation("loanType");
                });

            modelBuilder.Entity("API.Data.Models.Loyalty", b =>
                {
                    b.HasOne("API.Data.Models.BanksUsersCards", "bankUserCard")
                        .WithMany()
                        .HasForeignKey("bankUserCardId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("bankUserCard");
                });

            modelBuilder.Entity("API.Data.Models.SystemLogs", b =>
                {
                    b.HasOne("API.Data.Models.Account", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("API.Data.Models.Transaction", b =>
                {
                    b.HasOne("API.Data.Models.Card", "recieverCard")
                        .WithMany()
                        .HasForeignKey("recieverCardId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("API.Data.Models.Card", "senderCard")
                        .WithMany()
                        .HasForeignKey("senderCardId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("recieverCard");

                    b.Navigation("senderCard");
                });

            modelBuilder.Entity("API.Data.Models.UserActivity", b =>
                {
                    b.HasOne("API.Data.Models.User", "user")
                        .WithMany()
                        .HasForeignKey("userId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("API.Data.Models.Bank", b =>
                {
                    b.HasOne("API.Data.Models.Account", null)
                        .WithOne()
                        .HasForeignKey("API.Data.Models.Bank", "id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("API.Data.Models.User", b =>
                {
                    b.HasOne("API.Data.Models.Account", null)
                        .WithOne()
                        .HasForeignKey("API.Data.Models.User", "id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("API.Data.Models.DeletedUser", b =>
                {
                    b.HasOne("API.Data.Models.User", null)
                        .WithOne()
                        .HasForeignKey("API.Data.Models.DeletedUser", "id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
