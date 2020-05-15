using Microsoft.EntityFrameworkCore;

namespace StoreApp.Data {
    public class BusinessContext : DbContext {
        public BusinessContext() {

        }
        
        public BusinessContext (DbContextOptions<BusinessContext> options) : base(options) {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                // TODO Move to some properties file?
                options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=business;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }
    }
}