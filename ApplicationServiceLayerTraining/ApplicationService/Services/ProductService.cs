using ApplicationServiceLayerTraining.ApplicationService.Contracts;
using ApplicationServiceLayerTraining.ApplicationService.Dtos.ProductDtos;
using ApplicationServiceLayerTraining.Models.Services.Contracts;

namespace ApplicationServiceLayerTraining.ApplicationService.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Task<bool> DeleteAsync(ServiceDeleteProductDto obj)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> InsertAsync(ServiceCreateProductDto obj)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<ServiceSelectProductDto>> SelectAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ServiceSelectProductDto> SelectByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(ServiceUpdateProductDto obj)
    {
        throw new NotImplementedException();
    }
}
