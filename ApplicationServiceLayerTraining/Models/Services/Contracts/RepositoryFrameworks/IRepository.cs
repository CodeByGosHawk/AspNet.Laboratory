using ApplicationServiceLayerTraining.Frameworks.Contracts;

namespace ApplicationServiceLayerTraining.Models.Services.Contracts.RepositoryFrameworks;

public interface IRepository<T, TCollection>
{
    Task<IResponse<TCollection>> SelectAll();
    Task<IResponse<T>> SelectById(Guid id);
    Task<IResponse<T>> Insert(T obj);
    Task<IResponse<T>> Update(T obj);
    Task<IResponse<T>> Delete(T obj);
    Task Save();

    //Task<bool> Update(Guid Id, string values);
}