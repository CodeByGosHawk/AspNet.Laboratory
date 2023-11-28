﻿using ApplicationServiceLayerTraining.ApplicationService.Contracts.ServiceFrameworks;
using ApplicationServiceLayerTraining.ApplicationService.Dtos.ProductDtos;
using ApplicationServiceLayerTraining.Models.DomainModels.ProductAggregates;

namespace ApplicationServiceLayerTraining.ApplicationService.Contracts;

public interface IProductService : IService<Product,ServiceCreateProductDto,ServiceSelectProductDto,ServiceUpdateProductDto,ServiceDeleteProductDto>
{
    Task<ServiceSelectProductDto?> SelectByProductCodeAsync(string productCode);
}
