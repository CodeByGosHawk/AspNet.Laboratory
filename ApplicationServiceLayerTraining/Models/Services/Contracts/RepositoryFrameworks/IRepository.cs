using ApplicationServiceLayerTraining.Frameworks;

namespace ApplicationServiceLayerTraining.Models.Services.Contracts.RepositoryFrameworks;

public interface IRepository<T, TCollection>
{
    Task<Response<TCollection>> SelectAll();
    Task<Response<T>> SelectById(Guid id);
    Task<Response<T>> Insert(T obj);
    Task<Response<T>> Update(T obj);
    Task<Response<T>> Delete(Guid id);
    Task<Response<T>> Delete(T obj);
    Task Save();
    //Task<bool> Update(Guid Id, string values);
}