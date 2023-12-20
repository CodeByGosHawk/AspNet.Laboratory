using SinglePageArchitectureTraining.ApplicationService.Contracts;
using SinglePageArchitectureTraining.ApplicationService.Dtos.PersonDtos;
using SinglePageArchitectureTraining.ApplicationService.Dtos.ProductDtos;
using SinglePageArchitectureTraining.Frameworks;
using SinglePageArchitectureTraining.Frameworks.Abstracts;
using SinglePageArchitectureTraining.Frameworks.Contracts;
using SinglePageArchitectureTraining.Models.DomainModels.ProductAggregates;
using SinglePageArchitectureTraining.Models.Services.Contracts;

namespace SinglePageArchitectureTraining.ApplicationService.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }



  // Create
    public async Task<IResponse<ServiceCreateProductDto>> InsertAsync(ServiceCreateProductDto createProductDto)
    {
        var response = new Response<ServiceCreateProductDto>();

        if (createProductDto is null)
        {
            response.Status = Status.NullRef;
            response.Message = "Input is null. operation failed.";
            return response;
        }

        var createdProduct = new Product()
        {
            Id = new Guid(),
            Code = createProductDto.Code,
            Title = createProductDto.Title,
            Quantity = createProductDto.Quantity,
            UnitPrice = createProductDto.UnitPrice
        };
        var insertOperationResponse = await _productRepository.Insert(createdProduct);

        if (insertOperationResponse is null)
        {
            response.Status = Status.NullRef;
            response.Message = "The repository insert operation response is null";
            return response;
        }

        if (!insertOperationResponse.IsSuccessful)
        {
            response.Status = insertOperationResponse.Status;
            response.Message = insertOperationResponse.Message;
            return response;
        }

        response.IsSuccessful = true;
        response.Status = Status.Successful;
        response.Message = "Operation successful";
        return response;
    }



  // Read
    public async Task<IResponse<ServiceSelectAllProductsDto>> SelectAllAsync()
    {
        var response = new Response<ServiceSelectAllProductsDto>();
        var selectAllOpertaionResponse = await _productRepository.SelectAll();

        if (selectAllOpertaionResponse is null)
        {
            response.Status = Status.NullRef;
            response.Message = "The repository select all operation response is null";
            return response;
        }

        if (!selectAllOpertaionResponse.IsSuccessful)
        {
            response.Status = selectAllOpertaionResponse.Status;
            response.Message = selectAllOpertaionResponse.Message;
            return response;
        }

        if (selectAllOpertaionResponse.Value is null)
        {
            response.Status = Status.NullRef;
            response.Message = "The response value of repository select all operation is null";
            return response;
        }

        var selectAllProductsDto = new ServiceSelectAllProductsDto
        {
            SelectProductDtosList = []
        };

        foreach (var product in selectAllOpertaionResponse.Value)
        {
            var productDto = new ServiceSelectProductDto()
            {
                Id = product.Id,
                Code = product.Code,
                Title = product.Title,
                Quantity = product.Quantity,
                UnitPrice = product.UnitPrice
            };
            selectAllProductsDto.SelectProductDtosList.Add(productDto);
        }
        response.IsSuccessful = true;
        response.Status = Status.Successful;
        response.Message = "Operation successful";
        response.Value = selectAllProductsDto;
        return response;
    }

    public async Task<IResponse<ServiceSelectProductDto>> SelectAsync(ServiceSelectProductDto selectProductDto)
    {
        var response = new Response<ServiceSelectProductDto>();

        if (selectProductDto is null)
        {
            response.Status = Status.NullRef;
            response.Message = "Input is null. operation failed.";
            return response;
        }

        IResponse<Product> selectOperationResponse;

        if (selectProductDto.Code is null)
        {
            selectOperationResponse = await _productRepository.SelectById(selectProductDto.Id);
        }
        else
        {
            selectOperationResponse = await _productRepository.SelectByCode(selectProductDto.Code);
        }

        if (selectOperationResponse is null)
        {
            response.Status = Status.NullRef;
            response.Message = "The repository select operation response is null";
            return response;
        }

        if (!selectOperationResponse.IsSuccessful)
        {
            response.Status = selectOperationResponse.Status;
            response.Message = selectOperationResponse.Message;
            return response;
        }

        if (selectOperationResponse.Value is null)
        {
            response.Status = Status.NullRef;
            response.Message = "The response value of repository select operation is null";
            return response;
        }

        if (selectProductDto.Code is null)
        {
            selectProductDto.Code = selectOperationResponse.Value.Code;
            selectProductDto.Title = selectOperationResponse.Value.Title;
            selectProductDto.Quantity = selectOperationResponse.Value.Quantity;
            selectProductDto.UnitPrice = selectOperationResponse.Value.UnitPrice;
        }
        else
        {
            selectProductDto.Id = selectOperationResponse.Value.Id;
            selectProductDto.Title = selectOperationResponse.Value.Title;
            selectProductDto.Quantity = selectOperationResponse.Value.Quantity;
            selectProductDto.UnitPrice = selectOperationResponse.Value.UnitPrice;
        }

        response.IsSuccessful = true;
        response.Status = Status.Successful;
        response.Message = "Operation successful";
        response.Value = selectProductDto;
        return response;
    }



  // Update
    public async Task<IResponse<ServiceUpdateProductDto>> UpdateAsync(ServiceUpdateProductDto updateProductDto)
    {
        var response = new Response<ServiceUpdateProductDto>();

        if (updateProductDto is null)
        {
            response.Status = Status.NullRef;
            response.Message = "Input is null. operation failed.";
            return response;
        }

        var selectOperationResponse = await _productRepository.SelectById(updateProductDto.Id);

        if (selectOperationResponse is null)
        {
            response.Status = Status.NullRef;
            response.Message = "The repository select operation response is null";
            return response;
        }

        if (!selectOperationResponse.IsSuccessful)
        {
            response.Status = selectOperationResponse.Status;
            response.Message = selectOperationResponse.Message;
            return response;
        }

        if (selectOperationResponse.Value is null)
        {
            response.Status = Status.NullRef;
            response.Message = "The response value of repository select operation is null";
            return response;
        }

        var updatedProduct = selectOperationResponse.Value; 

        updatedProduct.Code = updateProductDto.Code;
        updatedProduct.Title = updateProductDto.Title;
        updatedProduct.Quantity = updateProductDto.Quantity;
        updatedProduct.UnitPrice = updateProductDto.UnitPrice;

        var updateOperationResponse = await _productRepository.Update(updatedProduct);

        if (updateOperationResponse is null)
        {
            response.Status = Status.NullRef;
            response.Message = "The repository update operation response is null";
            return response;
        }

        if (!updateOperationResponse.IsSuccessful)
        {
            response.Status = selectOperationResponse.Status;
            response.Message = selectOperationResponse.Message;
            return response;
        }

        response.IsSuccessful = true;
        response.Status = Status.Successful;
        response.Message = "Operation successful";
        return response;
    }



  // Delete
    public async Task<IResponse<ServiceDeleteProductDto>> DeleteAsync(ServiceDeleteProductDto deleteProductDto)
    {
        var response = new Response<ServiceDeleteProductDto>();

        if (deleteProductDto is null)
        {
            response.Status = Status.NullRef;
            response.Message = "Input is null. operation failed.";
            return response;
        }

        var selectOperationResponse = await _productRepository.SelectById(deleteProductDto.Id);

        if (selectOperationResponse is null)
        {
            response.Status = Status.NullRef;
            response.Message = "The repository select operation response is null";
            return response;
        }

        if (!selectOperationResponse.IsSuccessful)
        {
            response.Status = selectOperationResponse.Status;
            response.Message = selectOperationResponse.Message;
            return response;
        }

        if (selectOperationResponse.Value is null)
        {
            response.Status = Status.NullRef;
            response.Message = "The response value of repository select operation is null";
            return response;
        }

        var deletedProduct = selectOperationResponse.Value;

        var deleteOperationResponse = await _productRepository.Delete(deletedProduct);

        if (deleteOperationResponse is null)
        {
            response.Status = Status.NullRef;
            response.Message = "The repository delete operation response is null";
            return response;
        }

        if (!deleteOperationResponse.IsSuccessful)
        {
            response.Status = deleteOperationResponse.Status;
            response.Message = deleteOperationResponse.Message;
            return response;
        }

        response.IsSuccessful = true;
        response.Status = Status.Successful;
        response.Message = "Operation successful";
        return response;
    }



  // Save
    public async Task SaveAsync()
    {
        await _productRepository.Save();
    }
}
