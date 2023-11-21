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
    public bool Insert(Product product)
    {
        try
        {
            if (product is not null)
            {
                _dbContext.Add(product);
                return true;
            }
            else
            {
                return false;
            }

        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region[Read]
    public IEnumerable<Product> SelectAll()
    {
        try
        {
            return _dbContext.Product;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public Product SelectById(Guid Id)
    {
        try
        {
            return _dbContext.Product.Find(Id);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public Product SelectByProductCode(string productCode)
    {
        try
        {
            return _dbContext.Product.FirstOrDefault(p => p.ProductCode == productCode);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region[Update]
    public bool Update(Product product)
    {
        try
        {
            if(product is not null)
            {
                _dbContext.Entry(product).State = EntityState.Modified;
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region[Delete]
    public bool Delete(Guid Id)
    {
        try
        {
            var deletedProduct = _dbContext.Product.Find(Id);
            if (deletedProduct is not null)
            {
                _dbContext.Product.Remove(deletedProduct);
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public bool Delete(Product product)
    { 
        try
        {
            if (product is not null)
            {
                _dbContext.Product.Remove(product);
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region[Save]
    public void Save()
    {
        try
        {
            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion
}
