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
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }

        public BlinkandBuysContext(DbContextOptions<BlinkandBuysContext> options) : base(options)
        {
        }
    }
}