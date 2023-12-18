using API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Account> Account { get; set; }
        public DbSet<AutentificationToken> AutentificationToken { get; set; }
        public DbSet<Bank> Bank { get; set; }
        public DbSet<Card> Card { get; set; }
        public DbSet<CardType> CardType { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<DeletedUser> DeletedUser { get; set; }
        public DbSet<BanksUsersCards> BankUserCard { get; set; }
        public DbSet<Loan> Loan { get; set; }
        public DbSet<LoanType> LoanType { get; set; }
        public DbSet<Loyalty> Loyalty { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<User> User { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            modelBuilder.Entity<Account>()
        .Property(x => x.id)
        .UseIdentityColumn();
        }
    }
}
