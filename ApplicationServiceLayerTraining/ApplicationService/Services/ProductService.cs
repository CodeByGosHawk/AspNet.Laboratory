using ApplicationServiceLayerTraining.ApplicationService.Contracts;
using ApplicationServiceLayerTraining.ApplicationService.Dtos.ProductDtos;
using ApplicationServiceLayerTraining.Models.DomainModels.ProductAggregates;
using ApplicationServiceLayerTraining.Models.Services.Contracts;

namespace ApplicationServiceLayerTraining.ApplicationService.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    #region[Create]

    public async Task<bool> InsertAsync(ServiceCreateProductDto createProductDto)
    {
        if (createProductDto is null) return false;

        var createdProduct = new Product()
        {
            Id = new Guid(),
            Code = createProductDto.Code,
            Title = createProductDto.Title,
            Quantity = createProductDto.Quantity,
            UnitPrice = createProductDto.UnitPrice
        };
        bool result = await _productRepository.Insert(createdProduct);
        return result;
    }

    #endregion

    #region[Read]

    public async Task<IEnumerable<ServiceSelectProductDto>?> SelectAllAsync()
    {
        List<ServiceSelectProductDto> productsDto = new List<ServiceSelectProductDto>();
        var products = await _productRepository.SelectAll();
        if (products is null) return null;

        foreach (var product in products)
        {
            var productDto = new ServiceSelectProductDto()
            {
                Id = product.Id,
                Code = product.Code,
                Title = product.Title,
                Quantity = product.Quantity,
                UnitPrice = product.UnitPrice
            };
            productsDto.Add(productDto);
        }
        return productsDto;
    }

    public async Task<ServiceSelectProductDto?> SelectByIdAsync(Guid id)
    {
        var selectedProduct = await _productRepository.SelectById(id);
        if (selectedProduct is null) return null;

        var selectedProductDto = new ServiceSelectProductDto()
        {
            Id = selectedProduct.Id,
            Code = selectedProduct.Code,
            Title = selectedProduct.Title,
            Quantity = selectedProduct.Quantity,
            UnitPrice = selectedProduct.UnitPrice
        };
        return selectedProductDto;
    }

    public async Task<ServiceSelectProductDto?> SelectByProductCodeAsync(string productCode)
    {
        var selectedProduct = await _productRepository.SelectByProductCode(productCode);
        if (selectedProduct is null) return null;
        var selectedProductDto = new ServiceSelectProductDto()
        {
            Id = selectedProduct.Id,
            Code = selectedProduct.Code,
            Title = selectedProduct.Title,
            Quantity = selectedProduct.Quantity,
            UnitPrice = selectedProduct.UnitPrice
        };
        return selectedProductDto;
    }

    #endregion

    #region[Update]

    public async Task<bool> UpdateAsync(ServiceUpdateProductDto updateProductDto)
    {
        var updatedProduct = await _productRepository.SelectById(updateProductDto.Id);
        if (updatedProduct is null) return false;

        updatedProduct.Code = updateProductDto.Code;
        updatedProduct.Title = updateProductDto.Title;
        updatedProduct.Quantity = updateProductDto.Quantity;
        updatedProduct.UnitPrice = updateProductDto.UnitPrice;
        var result = await _productRepository.Update(updatedProduct);
        return result;
    }

    #endregion

    #region[Delete]

    public async Task<bool> DeleteAsync(ServiceDeleteProductDto deleteProductDto)
    {
        var deletedProduct = await _productRepository.SelectById(deleteProductDto.Id);
        if (deletedProduct is null) return false;

        var result = await _productRepository.Delete(deletedProduct);
        return result;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        var result = await _productRepository.Delete(id);
        return result;
    }

    #endregion

    #region[Save]

    public async Task SaveAsync()
    {
        await _productRepository.Save();
    }

    #endregion
}
