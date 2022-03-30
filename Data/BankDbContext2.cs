using DataBaseTest.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBaseTest.Data
{
    public class BankDbContext2 : DbContext
    {
        //public BankDbContext2(IConfiguration config)
        //{
        //    _config = config;
        //}
        public BankDbContext2(DbContextOptions<BankDbContext2> options) : base(options)
        {
        }

        public DbSet<CustomerModel> Customers { get; set; }

        public DbSet<AccountModel> Accounts { get; set; }
        public DbSet<AddressModel> Addresses { get; set; }
        public DbSet<CardModel> Cards { get; set; }
        public DbSet<ContactModel> Contacts { get; set; }
        public DbSet<EntityModel> Entities { get; set; }
        public DbSet<FullNameModel> FullNames { get; set; }
        public DbSet<LoginCredentialModel> LoginCredentials { get; set; }
        public DbSet<NameModel> Names { get; set; }
        public DbSet<NationalityModel> Nations { get; set; }
        public DbSet<PersonModel> Persons { get; set; }
        public DbSet<TransactionModel> Transactions { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(_config.GetConnectionString("customers"));
        //}

        //private readonly IConfiguration _config;

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            //b.Entity<AccountModel>()
            //    .Property(a => a.Balance)
            //    .HasColumnType("decimal(28, 4)");

            //b.Entity<AccountModel>()
            //    .Property(a => a.PercentageIncrease)
            //    .HasColumnType("decimal(28, 4)");

            //b.Entity<AccountModel>()
            //    .Property(a => a.PercentageDecrease)
            //    .HasColumnType("decimal(28, 4)");

            //b.Entity<AccountModel>()
            //    .Property(a => a.Debt)
            //    .HasColumnType("decimal(28, 4)");

            //b.Entity<AccountModel>()
            //    .Property(a => a.DebitLimit)
            //    .HasColumnType("decimal(28, 4)");

            //b.Entity<AccountModel>()
            //    .Property(a => a.CreditLimit)
            //    .HasColumnType("decimal(28, 4)");

            //b.Entity<AccountModel>()
            //    .Property(a => a.SmallestBalance)
            //    .HasColumnType("decimal(28, 4)");

            //b.Entity<AccountModel>()
            //    .Property(a => a.SmallestTransferIn)
            //    .HasColumnType("decimal(28, 4)");

            //b.Entity<AccountModel>()
            //    .Property(a => a.SmallestTransferOut)
            //    .HasColumnType("decimal(28, 4)");

            //b.Entity<AccountModel>()
            //    .Property(a => a.LargestBalance)
            //    .HasColumnType("decimal(28, 4)");

            //b.Entity<AccountModel>()
            //    .Property(a => a.LargestTransferIn)
            //    .HasColumnType("decimal(28, 4)");

            //b.Entity<AccountModel>()
            //    .Property(a => a.LargestTransferOut)
            //    .HasColumnType("decimal(28, 4)");

            //b.Entity<CardModel>()
            //    .Property(c => c.IssuedCost)
            //    .HasColumnType("decimal(28, 4)");

            //b.Entity<CardModel>()
            //    .Property(c => c.MonthlyRate)
            //    .HasColumnType("decimal(28, 4)");

            //b.Entity<CardModel>()
            //    .Property(c => c.WithdrawalLimit)
            //    .HasColumnType("decimal(28, 4)");

            b.ApplyConfiguration(new NameConfig());
            b.ApplyConfiguration(new FullNameConfig());
            b.ApplyConfiguration(new NationalityConfig());
            b.ApplyConfiguration(new AddressConfig());
            b.ApplyConfiguration(new ContactConfig());
            b.ApplyConfiguration(new EntityConfig());
            b.ApplyConfiguration(new PersonConfig());
            b.ApplyConfiguration(new LoginCredentialsConfig());
            b.ApplyConfiguration(new TransactionConfig());
            b.ApplyConfiguration(new CardConfig());
            b.ApplyConfiguration(new AccountConfig());
            b.ApplyConfiguration(new CustomerConfig());
        }
    }
}
