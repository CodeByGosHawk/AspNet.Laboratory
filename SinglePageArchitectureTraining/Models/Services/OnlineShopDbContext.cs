using SinglePageArchitectureTraining.Models.DomainModels.PersonAggregates;
using SinglePageArchitectureTraining.Models.DomainModels.ProductAggregates;
using Microsoft.EntityFrameworkCore;

namespace SinglePageArchitectureTraining.Models.Services;

public class OnlineShopDbContext : DbContext
{
    public DbSet<Product> Product { get; set; }
    public DbSet<Person> Person { get; set; }

    public OnlineShopDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
