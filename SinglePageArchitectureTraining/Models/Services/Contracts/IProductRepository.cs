using SinglePageArchitectureTraining.Frameworks.Contracts;
using SinglePageArchitectureTraining.Models.DomainModels.ProductAggregates;
using SinglePageArchitectureTraining.Models.Services.Contracts.RepositoryFrameworks;

namespace SinglePageArchitectureTraining.Models.Services.Contracts;

public interface IProductRepository : IRepository<Product, IEnumerable<Product>>
{
    Task<IResponse<Product>> SelectByCode(string productCode);

}
