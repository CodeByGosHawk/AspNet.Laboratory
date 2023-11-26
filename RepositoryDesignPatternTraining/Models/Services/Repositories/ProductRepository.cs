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
    public async Task<bool> Insert(Product product)
    {
        try
        {
            if (product is not null)
            {
                await _dbContext.AddAsync(product);
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
    public async Task<IEnumerable<Product>> SelectAll()
    {
        try
        {
            return await _dbContext.Product.ToListAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Product> SelectById(Guid Id)
    {
        try
        {
            return await _dbContext.Product.FindAsync(Id);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Product> SelectByProductCode(string productCode)
    {
        try
        {
            return await _dbContext.Product.FirstOrDefaultAsync(p => p.ProductCode == productCode);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region[Update]
    public async Task<bool> Update(Product product)
    {
        try
        {
            if(product is not null)
            {
                _dbContext.Entry(product).State = EntityState.Modified; // Bottleneck ? 
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
    public async Task<bool> Delete(Guid Id)
    {
        try
        {
            var deletedProduct = await _dbContext.Product.FindAsync(Id);
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

    public async Task<bool> Delete(Product product)
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
    } // Bottleneck ?
    #endregion

    #region[Save]
    public async Task Save()
    {
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion
}
