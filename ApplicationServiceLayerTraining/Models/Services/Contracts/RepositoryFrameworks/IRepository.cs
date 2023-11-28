namespace ApplicationServiceLayerTraining.Models.Services.Contracts.RepositoryFrameworks;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> SelectAll();
    Task<T> SelectById(Guid id);
    Task<bool> Insert(T obj);
    Task<bool> Update(T obj);
    Task<bool> Delete(Guid id);
    Task<bool> Delete(T obj);
    Task Save();
    //Task<bool> Update(Guid Id, string values);
}