using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Grocery.Data
{
    public class BlinkandBuysContext : DbContext
    {
        public DbSet<Account> Account { get; set; }
        public DbSet<LoginToken> LoginToken { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }
        public DbSet<UserCart> UserCart { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<BookedService> BookedService { get; set; }
        public DbSet<ServiceProviderAvailability> ServiceProviderAvailability { get; set; }
        public DbSet<SubCategory> SubCategory { get; set; }
        public DbSet<BillingAddress> BillingAddress { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<BookedProduct> BookedProduct { get; set; }
        public DbSet<ContactUs> ContactUs { get; set; }

        public BlinkandBuysContext(DbContextOptions<BlinkandBuysContext> options) : base(options)
        {
        }
    }
}