using DataBaseTest.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DataBaseTest.Data
{
    public class BankDbContext : DbContext
    {
        //public BankDbContext(IConfiguration config):base()
        //{
        //    _config = config;
        //}
        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
        {
        }
        public DbSet<EmployeeModel> Employees { get; set; }

        public DbSet<AddressModel> Addresses { get; set; }
        public DbSet<ContactModel> Contacts { get; set; }
        public DbSet<EducationModel> Educations { get; set; }
        public DbSet<EntityModel> Entities { get; set; }
        public DbSet<FullNameModel> FullNames { get; set; }
        public DbSet<LoginCredentialModel> LoginCredentials { get; set; }
        public DbSet<NameModel> Names { get; set; }
        public DbSet<NationalityModel> Nations { get; set; }
        public DbSet<PersonModel> Persons { get; set; }
        public DbSet<QualificationModel> Qualifications { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(_config.GetConnectionString("employees"));
        //}
        //private readonly IConfiguration _config;
        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);

            b.ApplyConfiguration(new NameConfig());
            b.ApplyConfiguration(new FullNameConfig());
            b.ApplyConfiguration(new NationalityConfig());
            b.ApplyConfiguration(new AddressConfig());
            b.ApplyConfiguration(new ContactConfig());
            b.ApplyConfiguration(new EntityConfig());
            b.ApplyConfiguration(new PersonConfig());
            b.ApplyConfiguration(new QualificationConfig());
            b.ApplyConfiguration(new EducationConfig());
            b.ApplyConfiguration(new LoginCredentialsConfig());
            b.ApplyConfiguration(new EmployeeConfig());
            b.ApplyConfiguration(new TransactionConfig());
            b.ApplyConfiguration(new CardConfig());
            b.ApplyConfiguration(new AccountConfig());
            //b.ApplyConfiguration(new CustomerConfig());
        }

        public virtual async Task<int> SaveChangesAsync2(object @object)
        {
            Entry(@object).Property("LastUpdatedOn").CurrentValue = DateTime.UtcNow;

            return await base.SaveChangesAsync();
        }
    }
}
