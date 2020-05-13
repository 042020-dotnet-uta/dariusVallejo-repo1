using Microsoft.EntityFrameworkCore;

namespace StoreApp.Data {
    public class BusinessContext : DbContext {
        public BusinessContext() {

        }
        
        public BusinessContext (DbContextOptions<BusinessContext> options) : base(options) {
        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=business;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }
    }
}