using Microsoft.EntityFrameworkCore;
using RepositoryDesignPatternTraining.Models.DomainModels.ProductAggregates;
using RepositoryDesignPatternTraining.Models.Services.Contracts;

namespace RepositoryDesignPatternTraining.Models.Services.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly OnlineShopDbContext _dbContext;

    public ProductRepository(OnlineShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #region[Create]
    public void Insert(Product product)
    {
        if(product is not null) _dbContext.Add(product);
    }
    #endregion

    #region[Read]
    public IEnumerable<Product> SelectAll()
    {
        return _dbContext.Product;
    }

    public Product SelectById(Guid Id)
    {
        return _dbContext.Product.Find(Id);
    }
    #endregion

    #region[Update]
    public void Update(Product product)
    {
        _dbContext.Entry(product).State = EntityState.Modified;
    }
    #endregion

    #region[Delete]
    public void Delete(Guid Id)
    {
        var deletedProduct = _dbContext.Product.Find(Id);
        _dbContext.Product.Remove(deletedProduct);
    }

    public void Delete(Product product)
    {
        _dbContext.Product.Remove(product);
    }
    #endregion

    #region[Save]
    public void Save()
    {
        _dbContext.SaveChanges();
    }
    #endregion
}
