using Microsoft.EntityFrameworkCore;
using ApplicationServiceLayerTraining.Models.DomainModels.ProductAggregates;
using ApplicationServiceLayerTraining.Models.Services.Contracts;

namespace ApplicationServiceLayerTraining.Models.Services.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly OnlineShopDbContext _dbContext;

    public ProductRepository(OnlineShopDbContext dbContext) // Primary Constructor Suggest ?!
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

    public async Task<Product> SelectById(Guid id)
    {
        try
        {
            return await _dbContext.Product.FindAsync(id);
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
    public async Task<bool> Delete(Guid id)
    {
        try
        {
            var deletedProduct = await _dbContext.Product.FindAsync(id);
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
