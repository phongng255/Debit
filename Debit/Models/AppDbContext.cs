using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Debit.Models
{
    public class AppDbContext : DbContext
    {
        private readonly IConfiguration configuration;
        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
             : base(options)
        {
            this.configuration = configuration;
        }
        public DbSet<User>? Users { get; set; }
        public DbSet<Customer>? Customers { get; set; }
        public DbSet<DebitCustomer>? DebitCustomer { get; set; }
        public DbSet<Accumulate>? Accumulates { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder)
        {
            string connectionString = configuration.GetConnectionString("MySQL");
            optionbuilder.UseLazyLoadingProxies().UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)
            );
        }

    }
}

