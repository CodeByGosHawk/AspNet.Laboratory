namespace ApplicationServiceLayerTraining.ApplicationService.Contracts.ServiceFrameworks;

public interface IService<T,TCreate,TRead,TUpdate,TDelete>
{
    Task<IEnumerable<TRead>?> SelectAllAsync();
    Task<TRead?> SelectByIdAsync(Guid Id);
    Task<bool> InsertAsync(TCreate obj);
    Task<bool> UpdateAsync(TUpdate obj);
    Task<bool> DeleteByIdAsync(Guid Id);
    Task<bool> DeleteAsync(TDelete obj);
    Task SaveAsync();
}


//Task<IEnumerable<T>> SelectAll();
//Task<T> SelectById(Guid Id);
//Task<bool> Insert(T obj);
//Task<bool> Update(T obj);
//Task<bool> Delete(Guid Id);
//Task<bool> Delete(T obj);
//Task Save();