namespace RepositoryDesignPatternTraining.Models.Services.Contracts.RepositoryFrameworks;

public interface IRepository<T> where T : class
{
    IEnumerable<T> SelectAll();
    T SelectById(Guid Id);
    bool Insert(T obj);
    bool Update(T obj);
    //string Update(Guid Id, string values);
    bool Delete(Guid Id);
    bool Delete(T obj);
    void Save();
}