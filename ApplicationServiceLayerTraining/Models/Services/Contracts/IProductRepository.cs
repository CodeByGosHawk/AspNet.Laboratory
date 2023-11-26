using ApplicationServiceLayerTraining.Models.DomainModels.ProductAggregates;
using ApplicationServiceLayerTraining.Models.Services.Contracts.RepositoryFrameworks;

namespace ApplicationServiceLayerTraining.Models.Services.Contracts;

public interface IProductRepository : IRepository<Product>
{
    Task<Product> SelectByProductCode(string productCode);
}
