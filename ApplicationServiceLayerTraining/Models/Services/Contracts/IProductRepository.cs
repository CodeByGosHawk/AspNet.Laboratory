using ApplicationServiceLayerTraining.Models.DomainModels.ProductAggregates;

namespace ApplicationServiceLayerTraining.Models.Services.Contracts;

public interface IProductRepository /*: IRepository<Product>*/
{
    Task<Product> SelectByProductCode(string productCode);


    ///////////////////////////////////////////////////////
    /// For Test

    Task<IEnumerable<Product>> SelectAll();
    Task<Product> SelectById(Guid id);
    Task<bool> Insert(Product obj);
    Task<bool> Update(Product obj);
    Task<bool> Delete(Guid id);
    Task<bool> Delete(Product obj);
    Task Save();
}
