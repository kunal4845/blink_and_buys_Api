using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Grocery.Data {
    public class BlinkandBuysContext : DbContext {
        public DbSet<Account> Account { get; set; }
        public DbSet<LoginToken> LoginToken { get; set; }

        public BlinkandBuysContext(DbContextOptions<BlinkandBuysContext> options) : base(options) {
        }
    }
}