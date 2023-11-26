using RepositoryDesignPatternTraining.Models.DomainModels.ProductAggregates;
using RepositoryDesignPatternTraining.Models.Services.Contracts.RepositoryFrameworks;

namespace RepositoryDesignPatternTraining.Models.Services.Contracts;

public interface IProductRepository : IRepository<Product>
{
    Task<Product> SelectByProductCode(string productCode);
}
