using Microsoft.EntityFrameworkCore;
using ApplicationServiceLayerTraining.Models.DomainModels.ProductAggregates;
using ApplicationServiceLayerTraining.Models.Services.Contracts;
using ApplicationServiceLayerTraining.Frameworks.Contracts;
using ApplicationServiceLayerTraining.Frameworks;
using ApplicationServiceLayerTraining.Frameworks.Abstracts;

namespace ApplicationServiceLayerTraining.Models.Services.Repositories;

public class ProductRepository(OnlineShopDbContext dbContext) : IProductRepository
{
    private readonly OnlineShopDbContext _dbContext = dbContext;


    // Create
    public async Task<IResponse<Product>> Insert(Product insertedProduct)
    {
        var response = new Response<Product>();
        try
        {
            if (insertedProduct is null)
            {
                response.Status = Status.NullRef;
                response.Message = "Input is null. operation failed.";
                return response;
            }

            await _dbContext.AddAsync(insertedProduct);
            response.IsSuccessful = true;
            response.Status = Status.Successful;
            response.Message = "Operation successful";
            return response;
        }
        catch (Exception)
        {
            throw;
        }
    }
    


    // Read
    public async Task<IResponse<IEnumerable<Product>>> SelectAll()
    {
        var response = new Response<IEnumerable<Product>>();
        try
        {
            var products = await _dbContext.Product.AsNoTracking().ToListAsync();

            if (products is null)
            {
                response.Status = Status.NotFound;
                response.Message = $"Selected table was not found";
                return response;
            }

            response.Value = products;
            response.IsSuccessful = true;
            response.Status = Status.Successful;
            response.Message = "Operation successful";
            return response;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IResponse<Product>> SelectById(Guid id)
    {
        var response = new Response<Product>();
        try
        {
            var selectedProduct = await _dbContext.Product.FindAsync(id);

            if (selectedProduct is null)
            {
                response.Status = Status.NotFound;
                response.Message = $"Product with Id : {id} does not exist in database";
                return response;
            }

            _dbContext.Entry(selectedProduct).State = EntityState.Detached; // Ef has no FindAsync with AsNoTracking 
            response.Value = selectedProduct;
            response.IsSuccessful = true;
            response.Status = Status.Successful;
            response.Message = "Operation successful";
            return response;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IResponse<Product>> SelectByCode(string code)
    {
        var response = new Response<Product>();
        try
        {
            if (code is null)
            {
                response.Status = Status.NullRef;
                response.Message = "Input is null. operation failed.";
                return response;
            }

            var product = await _dbContext.Product.AsNoTracking().FirstOrDefaultAsync(p => p.Code == code);

            if (product is null)
            {
                response.Status = Status.NotFound;
                response.Message = $"Product with Code : {code} does not exist in database";
                return response;
            }

            response.Value = product;
            response.IsSuccessful = true;
            response.Status = Status.Successful;
            response.Message = "Operation successful";
            return response;
        }
        catch (Exception)
        {
            throw;
        }
    }
    

    
    // Update
    public async Task<IResponse<Product>> Update(Product updatedProduct)
    {
        var response = new Response<Product>();
        try
        {
            if(updatedProduct is null)
            {
                response.Status = Status.NullRef;
                response.Message = "Input is null. operation failed.";
                return response;
            }

            //_dbContext.Entry(updatedProduct).State = EntityState.Modified; // Bottleneck ?
            _dbContext.Update(updatedProduct);                                                                       
            response.IsSuccessful = true;
            response.Status = Status.Successful;
            response.Message = "Operation successful";
            return response;
        }
        catch (Exception)
        {
            throw;
        }
    }
    


    // Delete
    public async Task<IResponse<Product>> Delete(Guid id)
    {
        var response = new Response<Product>();
        try
        {
            var deletedProduct = await _dbContext.Product.FindAsync(id);

            if (deletedProduct is null)
            {
                response.Status = Status.NotFound;
                response.Message = $"Product with Id : {id} does not exist in database";
                return response;
            }

            _dbContext.Product.Remove(deletedProduct); // Bottleneck ? 
            response.IsSuccessful = true;
            response.Status = Status.Successful;
            response.Message = "Operation successful";
            return response;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IResponse<Product>> Delete(Product deletedProduct)
    {
        var response = new Response<Product>();
        try
        {
            if (deletedProduct is null)
            {
                response.Status = Status.NullRef;
                response.Message = "Product is null. operation failed.";
                return response;
            }

            _dbContext.Product.Remove(deletedProduct);
            response.IsSuccessful = true;
            response.Status = Status.Successful;
            response.Message = "Operation successful";
            return response;
        }
        catch (Exception)
        {
            throw;
        }
    } // Bottleneck ?
    


    // Save
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
}
