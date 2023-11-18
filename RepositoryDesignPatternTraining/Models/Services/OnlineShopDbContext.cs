using Microsoft.EntityFrameworkCore;
using RepositoryDesignPatternTraining.Models.DomainModels.PersonAggregates;
using RepositoryDesignPatternTraining.Models.DomainModels.ProductAggregates;

namespace RepositoryDesignPatternTraining.Models.Services;

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
