using ApplicationServiceLayerTraining.Frameworks.Contracts;
using ApplicationServiceLayerTraining.Models.DomainModels.ProductAggregates;
using ApplicationServiceLayerTraining.Models.Services.Contracts.RepositoryFrameworks;

namespace ApplicationServiceLayerTraining.Models.Services.Contracts;

public interface IProductRepository : IRepository<Product, IEnumerable<Product>>
{
    Task<IResponse<Product>> SelectByCode(string productCode);

}
