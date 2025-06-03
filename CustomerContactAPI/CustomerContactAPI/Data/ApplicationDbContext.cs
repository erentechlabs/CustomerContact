using CustomerContactAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerContactAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSet for Customer entity
        public DbSet<Customer> Customers { get; set; } 


    }
}
