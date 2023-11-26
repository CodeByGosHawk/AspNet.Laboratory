using ApplicationServiceLayerTraining.Models.DomainModels.PersonAggregates;
using ApplicationServiceLayerTraining.Models.DomainModels.ProductAggregates;
using Microsoft.EntityFrameworkCore;

namespace ApplicationServiceLayerTraining.Models.Services;

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
